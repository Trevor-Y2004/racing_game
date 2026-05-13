using UnityEngine;
using TMPro;

public class CarMotor : MonoBehaviour
{
    [Header("Car Parts")]
    public Rigidbody rigid;
    public Transform carVisual;

    public WheelCollider wheel1;
    public WheelCollider wheel2;
    public WheelCollider wheel3;
    public WheelCollider wheel4;

    [Header("Driving")]
    public float drivespeed = 2500f;
    public float steerspeed = 55f;

    [Header("UI / Camera")]
    public TextMeshProUGUI speedText;
    public LapCounter lapCounter;
    public Camera carCamera;
    public float normalFOV = 60f;
    public float boostFOV = 80f;
    public float fovSpeed = 5f;

    [Header("Drifting")]
    public bool isGliding = false;
    public float normalSidewaysStiffness = 1f;
    public float driftSidewaysStiffness = 0.4f;
    public float driftSteerMultiplier = 0.4f;
    public float minSpeedForDriftBoost = 8f;
    public float normalTurnDamping = 0.85f;
    public float driftTurnAssist = 0.8f;

    [Header("Drift Momentum")]
    public float lateralGripDuringDrift = 0.96f;
    public float forwardMomentumDuringDrift = 1.002f;

    [Header("Drift Visuals")]
    public float driftVisualTilt = 10f;
    public float driftVisualYaw = 8f;
    public float driftVisualSpeed = 6f;



    [Header("Drift Sparks")]
    public ParticleSystem driftSmokeLeft;
    public ParticleSystem driftSmokeRight;
    public float driftSlipThreshold = 0.18f;
    public float maxSparkEmission = 150f;

    public Color weakDriftColor = Color.cyan;
    public Color mediumDriftColor = new Color(1f, 0.45f, 0f);
    public Color strongDriftColor = new Color(1f, 0f, 1f);

    private float driftTimer = 0f;

    [Header("Braking")]
    public float brakeForce = 600f;

    private float horizontalInput;
    private float verticalInput;
    private bool driftInput;
    private bool brakeInput;

    public float CurrentSteerInput => horizontalInput;
    private float targetFOV;

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody>();

        if (rigid != null)
            rigid.centerOfMass = new Vector3(0, -1f, 0);

        if (speedText != null)
            speedText.text = "";

        targetFOV = normalFOV;
    }

    void Update()
    {
        if (lapCounter != null && lapCounter.raceStarted && rigid != null && speedText != null)
        {
            float speed = rigid.linearVelocity.magnitude * 2.237f;
            speedText.text = Mathf.RoundToInt(speed) + " mph";
        }

        if (carCamera != null)
        {
            carCamera.fieldOfView = Mathf.Lerp(
                carCamera.fieldOfView,
                targetFOV,
                fovSpeed * Time.deltaTime
            );
        }

        HandleDriftVisuals();
    }

    public void SetInputs(float steer, float throttle, bool drift, bool brake)
    {
        horizontalInput = Mathf.Clamp(steer, -1f, 1f);
        verticalInput = Mathf.Clamp(throttle, -1f, 1f);
        driftInput = drift;
        brakeInput = brake;
    }

    public void TriggerBoostFOV()
    {
        targetFOV = boostFOV;
        Invoke(nameof(ResetFOV), 1f);
    }

    void ResetFOV()
    {
        targetFOV = normalFOV;
    }

    void FixedUpdate()
    {
        if (rigid == null || wheel1 == null || wheel2 == null || wheel3 == null || wheel4 == null)
            return;

        bool drifting = driftInput;

        float motor = verticalInput * drivespeed;

        if (drifting)
            motor *= 1.2f;

        if (rigid.linearVelocity.magnitude < 5f)
            motor *= 2f;

        if (brakeInput)
            motor *= 0.2f;

        wheel1.motorTorque = 0;
        wheel2.motorTorque = 0;
        wheel3.motorTorque = motor;
        wheel4.motorTorque = motor;

        float currentSteerSpeed = steerspeed;

        if (drifting && rigid.linearVelocity.magnitude > minSpeedForDriftBoost)
            currentSteerSpeed *= driftSteerMultiplier;

        wheel1.steerAngle = currentSteerSpeed * horizontalInput;
        wheel2.steerAngle = currentSteerSpeed * horizontalInput;

        HandleDrift(drifting);

        if (brakeInput)
        {
            float brakeStrength = brakeForce * 0.00005f;
            rigid.linearVelocity *= 1f - (brakeStrength * Time.fixedDeltaTime);
        }

        if (drifting)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rigid.linearVelocity);

            localVelocity.x *= lateralGripDuringDrift;
            localVelocity.z *= forwardMomentumDuringDrift;

            rigid.linearVelocity = transform.TransformDirection(localVelocity);
        }

        if (drifting && Mathf.Abs(horizontalInput) > 0.1f)
        {
            float driftTurnForce = horizontalInput * driftTurnAssist;
            rigid.AddTorque(Vector3.up * driftTurnForce, ForceMode.Acceleration);
        }

        if (!drifting && !isGliding)
        {
            Vector3 angularVel = rigid.angularVelocity;
            angularVel.y *= normalTurnDamping;
            rigid.angularVelocity = angularVel;
        }

        if (driftInput && Mathf.Abs(horizontalInput) > 0.1f)
        {
            driftTimer += Time.fixedDeltaTime;
        }
        else
        {
            driftTimer = 0f;
        }

        HandleDriftSparks(drifting);
    }

    void HandleDriftVisuals()
    {
        if (carVisual == null)
            return;

        float targetTilt = 0f;
        float targetYaw = 0f;

        if (driftInput && Mathf.Abs(horizontalInput) > 0.1f)
        {
            targetTilt = -horizontalInput * driftVisualTilt;
            targetYaw = horizontalInput * driftVisualYaw;
        }

        Quaternion targetRotation = Quaternion.Euler(0f, targetYaw, targetTilt);

        carVisual.localRotation = Quaternion.Lerp(
            carVisual.localRotation,
            targetRotation,
            driftVisualSpeed * Time.deltaTime
        );
    }

    void HandleDrift(bool drifting)
    {
        if (drifting)
        {
            SetSidewaysStiffness(wheel1, normalSidewaysStiffness);
            SetSidewaysStiffness(wheel2, normalSidewaysStiffness);
            SetSidewaysStiffness(wheel3, driftSidewaysStiffness);
            SetSidewaysStiffness(wheel4, driftSidewaysStiffness);
        }
        else
        {
            SetSidewaysStiffness(wheel1, normalSidewaysStiffness);
            SetSidewaysStiffness(wheel2, normalSidewaysStiffness);
            SetSidewaysStiffness(wheel3, normalSidewaysStiffness);
            SetSidewaysStiffness(wheel4, normalSidewaysStiffness);
        }
    }

    void SetSidewaysStiffness(WheelCollider wheel, float stiffness)
    {
        if (wheel == null)
            return;

        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = stiffness;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    float GetWheelSlip(WheelCollider wheel)
    {
        if (wheel == null)
            return 0f;

        WheelHit hit;

        if (wheel.GetGroundHit(out hit))
            return Mathf.Abs(hit.sidewaysSlip);

        return 0f;
    }

    void HandleDriftSparks(bool drifting)
    {
        bool shouldSmoke = drifting && Mathf.Abs(horizontalInput) > 0.1f && rigid.linearVelocity.magnitude > 5f;

        if (shouldSmoke)
        {
            if(driftSmokeLeft != null && !driftSmokeLeft.isPlaying)
                driftSmokeLeft.Play();
            if(driftSmokeRight != null && !driftSmokeRight.isPlaying)
                driftSmokeRight.Play();
        }
        else
        {
            if (driftSmokeLeft != null && driftSmokeLeft.isPlaying)
                driftSmokeLeft.Stop();
            if (driftSmokeRight != null && driftSmokeRight.isPlaying)
                driftSmokeRight.Stop();
        }
        // float leftSlip = GetWheelSlip(wheel3);
        // float rightSlip = GetWheelSlip(wheel4);

        // float strongestSlip = Mathf.Max(leftSlip, rightSlip);

        // float sparkStrength = Mathf.Clamp01((strongestSlip - driftSlipThreshold) * 4f);

        // bool shouldSpark = drifting && sparkStrength > 0.05f;

        // UpdateSparkParticle(driftSmokeLeft, shouldSpark, sparkStrength);
        // UpdateSparkParticle(driftSmokeRight, shouldSpark, sparkStrength);

    }

    void UpdateSparkParticle(ParticleSystem sparkSystem, bool shouldSpark, float strength)
    {
        if (sparkSystem == null)
            return;

        var emission = sparkSystem.emission;
        var main = sparkSystem.main;

        emission.rateOverTime = maxSparkEmission * strength;

        if (driftTimer < 1f)
        {
            main.startColor = weakDriftColor; // blue
        }
        else if (driftTimer < 2f)
        {
            main.startColor = mediumDriftColor; // orange
        }
        else
        {
            main.startColor = strongDriftColor; // purple
        }

        if (shouldSpark)
        {
            if (!sparkSystem.isPlaying)
                sparkSystem.Play();
        }
        else
        {
            if (sparkSystem.isPlaying)
                sparkSystem.Stop();
        }
    }
}