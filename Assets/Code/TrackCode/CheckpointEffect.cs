using UnityEngine;

public class CheckpointEffect : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public string animationTriggerName = "Checkpoint";

    private bool activated = false;

    public void PlayEffect()
    {
        if (activated) return;

        activated = true;

        if (animator != null)
            animator.SetTrigger(animationTriggerName);

        if (audioSource != null)
            audioSource.Play();
    }

    public void ResetEffect()
    {
        activated = false;
    }
}