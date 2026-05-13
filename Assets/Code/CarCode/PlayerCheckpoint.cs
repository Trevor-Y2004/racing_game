using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Transform currentCheckpoint;

    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        if (currentCheckpoint == null)
            return;

        Vector3 spawnPos = currentCheckpoint.position + currentCheckpoint.right * 1f + Vector3.up * 1f;

        transform.position = spawnPos;

        // Face the cube's X direction - local view
        transform.rotation = Quaternion.LookRotation(currentCheckpoint.right, Vector3.up);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}