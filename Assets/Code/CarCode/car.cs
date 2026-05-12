using UnityEngine;

public class car : MonoBehaviour
{
    public Rigidbody rigid;

    public WheelCollider wheel1; // Front Left
    public WheelCollider wheel2; // Front Right
    public WheelCollider wheel3; // Back Left
    public WheelCollider wheel4; // Back Right

    public float drivespeed = 1900f;
    public float steerspeed = 55f;

    public KeyCode driftKey = KeyCode.Space;

    public float normalSidewaysStiffness = 1f;
    public float driftSidewaysStiffness = 0.4f;

    public float driftSteerMultiplier = 1.1f;
    public float minSpeedForDriftBoost = 8f;

    public float normalTurnDamping = 0.85f;

    public float driftTurnAssist = 0.8f;

    float horizontalInput;
    float verticalInput;

    void Start()
    {
        rigid.centerOfMass = new Vector3(0, -0.3f, 0);
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        bool drifting = Input.GetKey(driftKey);

        float motor = verticalInput * drivespeed;

        if (drifting)
        {
            motor *= 1.2f;
        }

        if (rigid.linearVelocity.magnitude < 5f)
        {
            motor *= 2f;
        }

        //for the rear wheel drive
        wheel1.motorTorque = 0;
        wheel2.motorTorque = 0;
        wheel3.motorTorque = motor;
        wheel4.motorTorque = motor;

        //steering
        float currentSteerSpeed = steerspeed;

        if (drifting && rigid.linearVelocity.magnitude > minSpeedForDriftBoost)
        {
            currentSteerSpeed *= driftSteerMultiplier;
        }

        wheel1.steerAngle = currentSteerSpeed * horizontalInput;
        wheel2.steerAngle = currentSteerSpeed * horizontalInput;

        HandleDrift(drifting);

        // Helps keep drift turning in the direction you're steering
        if (drifting && Mathf.Abs(horizontalInput) > 0.1f)
        {
            float driftTurnForce = horizontalInput * driftTurnAssist;
            rigid.AddTorque(Vector3.up * driftTurnForce, ForceMode.Acceleration);
        }

        // Reduce turning momentum only during normal driving
        if (!drifting)
        {
            Vector3 angularVel = rigid.angularVelocity;
            angularVel.y *= normalTurnDamping;
            rigid.angularVelocity = angularVel;
        }
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
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = stiffness;
        wheel.sidewaysFriction = sidewaysFriction;
    }
}