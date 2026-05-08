using UnityEngine;
using TMPro;

public class car : MonoBehaviour
{
    public Rigidbody rigid;
    public WheelCollider wheel1, wheel2, wheel3, wheel4;
    public float drivespeed, steerspeed;
    public TextMeshProUGUI speedText;
    public bool isGliding = false;
    float horizontalInput, verticalInput;

    void Start()
    {
        rigid.centerOfMass = new Vector3(0, -0.3f, 0);
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        float speed = rigid.linearVelocity.magnitude * 2.237f;
        speedText.text = Mathf.RoundToInt(speed) + " mph";
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