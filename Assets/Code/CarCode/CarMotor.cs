using UnityEngine;
using TMPro;

public class CarMotor : MonoBehaviour
{
    [Header("Car Parts")]
    public Rigidbody rigid;

    public WheelCollider wheel1; // front left
    public WheelCollider wheel2; // front right
    public WheelCollider wheel3; // rear left
    public WheelCollider wheel4; // rear right

    [Header("Driving")]
    public float drivespeed = 1200f;
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
    public float driftSteerMultiplier = 1.1f;
    public float minSpeedForDriftBoost = 8f;
    public float normalTurnDamping = 0.85f;
    public float driftTurnAssist = 0.8f;

    [Header("Drift Smoke")]
    public ParticleSystem driftSmokeLeft;
    public ParticleSystem driftSmokeRight;
    public float driftSlipThreshold = 0.3f;

    private float horizontalInput;
    private float verticalInput;
    private bool driftInput;

    public float CurrentSteerInput => horizontalInput;
    private float targetFOV;

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody>();

        if (rigid != null)
            rigid.centerOfMass = new Vector3(0, -0.3f, 0);

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
    }

    public void SetInputs(float steer, float throttle, bool drift)
    {
        horizontalInput = Mathf.Clamp(steer, -1f, 1f);
        verticalInput = Mathf.Clamp(throttle, -1f, 1f);
        driftInput = drift;
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

        HandleDriftSmoke(drifting);
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

    bool IsWheelSliding(WheelCollider wheel)
    {
        if (wheel == null)
            return false;

        WheelHit hit;

        if (wheel.GetGroundHit(out hit))
            return Mathf.Abs(hit.sidewaysSlip) > driftSlipThreshold;

        return false;
    }

    void HandleDriftSmoke(bool drifting)
    {
        bool leftSlide = IsWheelSliding(wheel3);
        bool rightSlide = IsWheelSliding(wheel4);

        if (drifting && (leftSlide || rightSlide))
        {
            if (driftSmokeLeft != null && !driftSmokeLeft.isPlaying)
                driftSmokeLeft.Play();

            if (driftSmokeRight != null && !driftSmokeRight.isPlaying)
                driftSmokeRight.Play();
        }
        else
        {
            if (driftSmokeLeft != null && driftSmokeLeft.isPlaying)
                driftSmokeLeft.Stop();

            if (driftSmokeRight != null && driftSmokeRight.isPlaying)
                driftSmokeRight.Stop();
        }
    }
}