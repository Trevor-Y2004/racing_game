using UnityEngine;

public class FinishLineCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LapCounter lap = other.GetComponentInParent<LapCounter>();
        if (lap != null)
        {
            lap.HitCheckpoint();
        }

        AILapCounter aiLap = other.GetComponentInParent<AILapCounter>();
        if (aiLap != null)
        {
            aiLap.HitCheckpoint();
        }
    }
}