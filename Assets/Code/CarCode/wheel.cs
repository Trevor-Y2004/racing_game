using UnityEngine;

public class wheel : MonoBehaviour
{
    public WheelCollider wheelCollider;
    public Transform wheelMesh;

    // Change this in the Inspector if the wheel spins the wrong way
    public Vector3 rotationOffset = new Vector3(0, 0, 90);

    void Update()
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        wheelMesh.position = position;
        wheelMesh.rotation = rotation * Quaternion.Euler(rotationOffset);
    }
}