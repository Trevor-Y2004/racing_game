using UnityEngine;

public class LapTrigger : MonoBehaviour
{
    public LapCounter lapCounter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lapCounter.CompleteLap();
        }
    }
}