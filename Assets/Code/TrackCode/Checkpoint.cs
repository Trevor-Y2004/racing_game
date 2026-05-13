using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointEffect checkpointEffect;

    private void Awake()
    {
        checkpointEffect = GetComponent<CheckpointEffect>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCheckpoint player = other.GetComponentInParent<PlayerCheckpoint>();

        if (player != null && player.CompareTag("Player"))
        {
            player.SetCheckpoint(transform, checkpointEffect);

            if (checkpointEffect != null)
                checkpointEffect.PlayEffect();
        }
    }
}