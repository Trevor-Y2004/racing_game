using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCheckpoint player = other.GetComponentInParent<PlayerCheckpoint>();

        if (player != null)
        {
            player.SetCheckpoint(transform);
        }
    }
}