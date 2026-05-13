using UnityEngine;

public class CheckpointEffect : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public string animationTriggerName = "Checkpoint";

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        PlayerCheckpoint player = other.GetComponentInParent<PlayerCheckpoint>();

        if (player != null && player.CompareTag("Player"))
        {
            activated = true;

            if (animator != null)
                animator.SetTrigger(animationTriggerName);

            if (audioSource != null)
                audioSource.Play();
        }
    }
}