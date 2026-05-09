using UnityEngine;
using TMPro;

public class car : MonoBehaviour
{
    public Rigidbody rigid;
    public WheelCollider wheel1, wheel2, wheel3, wheel4;
    public float drivespeed, steerspeed;
    public TextMeshProUGUI speedText;
    public bool isGliding = false;
    public LapCounter lapCounter;
    public Camera carCamera;
    public float normalFOV = 60f;
    public float boostFOV = 80f;
    public float fovSpeed = 5f;
    private float targetFOV;
    float horizontalInput, verticalInput;

    void Start()
    {
        rigid.centerOfMass = new Vector3(0, -0.3f, 0);
        speedText.text = "";
        targetFOV = normalFOV;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (lapCounter.raceStarted)
        {
            float speed = rigid.linearVelocity.magnitude * 2.237f;
            speedText.text = Mathf.RoundToInt(speed) + " mph";
        }

        carCamera.fieldOfView = Mathf.Lerp(carCamera.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);
    }

    public void TriggerBoostFOV()
    {
        targetFOV = boostFOV;
        Invoke("ResetFOV", 1f);
    }

    void ResetFOV()
    {
        targetFOV = normalFOV;
    }

    void FixedUpdate()
    {
        float motor = Input.GetAxis("Vertical") * drivespeed;

        if (rigid.linearVelocity.magnitude < 5f)
        {
            motor *= 2f;
        }

        wheel1.motorTorque = 0;
        wheel2.motorTorque = 0;
        wheel3.motorTorque = motor;
        wheel4.motorTorque = motor;

        float speed = rigid.linearVelocity.magnitude;
        float steerLimit = Mathf.Lerp(steerspeed, 15f, speed / 30f);
        wheel1.steerAngle = steerLimit * horizontalInput;
        wheel2.steerAngle = steerLimit * horizontalInput;

        if (!isGliding)
        {
            Vector3 angularVel = rigid.angularVelocity;
            angularVel.y *= 0.9f;
            rigid.angularVelocity = angularVel;
        }
    }
}