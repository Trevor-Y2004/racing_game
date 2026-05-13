using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CarMotor carMotor;
    public KeyCode driftKey = KeyCode.Space;

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

        carMotor.SetInputs(steer, throttle, drift);
    }
}