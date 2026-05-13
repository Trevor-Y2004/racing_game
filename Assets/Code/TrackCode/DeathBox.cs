using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCheckpoint player = other.GetComponentInParent<PlayerCheckpoint>();

        if (player != null)
        {
            player.Respawn();
        }
    }
}