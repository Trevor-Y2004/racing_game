using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Finish line touched by: " + other.name);

        LapCounter lap = other.GetComponentInParent<LapCounter>();

        if (lap != null)
        {
            Debug.Log("LapCounter found on finish line hit");
            lap.CompleteLap();
        }
        else
        {
            Debug.Log("No LapCounter found on: " + other.name);
        }
    }
}