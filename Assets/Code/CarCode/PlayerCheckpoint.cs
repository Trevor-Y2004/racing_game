using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Transform currentCheckpoint;
    private CheckpointEffect currentCheckpointEffect;

    [SerializeField] private Transform initialSpawnPoint;

    void Start()
    {
        currentCheckpoint = initialSpawnPoint;
        Respawn();
    }

    public void SetCheckpoint(Transform checkpoint, CheckpointEffect checkpointEffect)
    {
        if (currentCheckpointEffect != null && currentCheckpointEffect != checkpointEffect)
        {
            currentCheckpointEffect.ResetEffect();
        }

        currentCheckpoint = checkpoint;
        currentCheckpointEffect = checkpointEffect;

        Debug.Log("Checkpoint set: " + checkpoint.name);
    }

    public void Respawn()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogError("No checkpoint assigned!");
            return;
        }

        Vector3 spawnPos = currentCheckpoint.position + currentCheckpoint.right * 1f + Vector3.up * 1f;
        Quaternion spawnRot = Quaternion.LookRotation(currentCheckpoint.right, Vector3.up);

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = spawnPos;
        transform.rotation = spawnRot;

        rb.position = spawnPos;
        rb.rotation = spawnRot;
    }
}