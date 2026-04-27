using UnityEngine;
using TMPro;

public class LapCounter : MonoBehaviour
{
    public int totalLaps = 3;
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI timerText;

    private int currentLap = 1;
    private float timer = 0f;
    private bool raceFinished = false;

    void Update()
    {
        if (!raceFinished)
        {
            timer += Time.deltaTime;
            int minutes = (int)(timer / 60f);
            int seconds = (int)(timer % 60f);
            int milliseconds = (int)((timer * 100f) % 100f);
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        lapText.text = "Lap " + currentLap + "/" + totalLaps;
    }

    public void CompleteLap()
    {
        if (raceFinished) return;
        if (currentLap >= totalLaps)
        {
            raceFinished = true;
            lapText.text = "Finished!";
        }
        else
        {
            currentLap++;
        }
    }
}