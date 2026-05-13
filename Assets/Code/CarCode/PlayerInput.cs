using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CarMotor carMotor;
    public KeyCode driftKey = KeyCode.Space;
    public KeyCode brakeKey = KeyCode.LeftShift; // NEW

    void Start()
    {
        if (carMotor == null)
            carMotor = GetComponent<CarMotor>();
    }

    void Update()
    {
        if (carMotor == null)
            return;

        float steer = Input.GetAxis("Horizontal");
        float throttle = Input.GetAxis("Vertical");
        bool drift = Input.GetKey(driftKey);
        bool brake = Input.GetKey(brakeKey); // NEW

        carMotor.SetInputs(steer, throttle, drift, brake);
    }
}