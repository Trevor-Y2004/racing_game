using UnityEngine;

public class car : MonoBehaviour
{
    public Rigidbody rigid;
    public WheelCollider wheel1, wheel2, wheel3, wheel4;
    public float drivespeed, steerspeed;
    float horizontalInput, verticalInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid.centerOfMass = new Vector3(0, -0.3f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        //if player isn't pressing any vertical input controls, our vertical input is 0 so we don't drive
        float motor = Input.GetAxis("Vertical") * drivespeed;

        if (rigid.linearVelocity.magnitude < 5f)
        {
        motor *= 2f;
        }

        wheel1.motorTorque = 0;
        wheel2.motorTorque = 0;
        wheel3.motorTorque = motor;
        wheel4.motorTorque = motor;
        //wheels turn at the wanted speed based on the horizontal input
        wheel1.steerAngle = steerspeed * horizontalInput;
        wheel2.steerAngle = steerspeed * horizontalInput;

        Vector3 angularVel = rigid.angularVelocity;
        angularVel.y *= 0.9f;
        rigid.angularVelocity = angularVel;
    }
}
