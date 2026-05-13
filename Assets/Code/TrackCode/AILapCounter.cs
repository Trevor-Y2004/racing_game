using UnityEngine;

public class AILapCounter : MonoBehaviour
{
    public int totalLaps = 3;

    private int currentLap = 1;
    private bool raceStarted = false;
    private bool raceFinished = false;
    private bool passedCheckpoint = false;

    public int CurrentLap => currentLap;
    public bool RaceFinished => raceFinished;

    public void StartRace()
    {
        raceStarted = true;
        currentLap = 1;
        raceFinished = false;
        passedCheckpoint = false;
    }

    public void HitCheckpoint()
    {
        if (!raceStarted || raceFinished) return;

        passedCheckpoint = true;
        Debug.Log(gameObject.name + " reached checkpoint");
    }

    public void CompleteLap()
    {
        if (!raceStarted || raceFinished) return;

        if (!passedCheckpoint)
        {
            Debug.Log(gameObject.name + " tried to finish lap without checkpoint");
            return;
        }

        passedCheckpoint = false;

        if (currentLap >= totalLaps)
        {
            raceFinished = true;
            Debug.Log(gameObject.name + " finished the race!");
        }
        else
        {
            currentLap++;
            Debug.Log(gameObject.name + " is now on lap " + currentLap);
        }
    }
}