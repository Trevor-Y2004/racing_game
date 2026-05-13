using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class RaceManager : MonoBehaviour
{
    public GameObject leaderboardPanel;
    public TextMeshProUGUI leaderboardText;

    private List<RacerInfo> racers = new List<RacerInfo>();

    void Start()
    {
        if (leaderboardPanel != null)
            leaderboardPanel.SetActive(false);
    }

    void Update()
    {
        if (leaderboardPanel != null && leaderboardPanel.activeSelf)
            UpdateLeaderboard();
    }

    public void RegisterRacer(string racerName, int totalLaps)
    {
        if (racers.Any(r => r.racerName == racerName))
            return;

        racers.Add(new RacerInfo(racerName, totalLaps));
        Debug.Log("Registered racer: " + racerName);
    }

    public void UpdateRacer(string racerName, int currentLap, float raceTime, bool finished, int totalLaps)
    {
        RacerInfo racer = racers.FirstOrDefault(r => r.racerName == racerName);

        if (racer == null)
        {
            RegisterRacer(racerName, totalLaps);
            racer = racers.FirstOrDefault(r => r.racerName == racerName);
        }

        racer.currentLap = currentLap;
        racer.raceTime = raceTime;
        racer.finished = finished;
        racer.totalLaps = totalLaps;
    }

    public void ShowLeaderboard()
    {
        if (leaderboardPanel != null)
            leaderboardPanel.SetActive(true);

        UpdateLeaderboard();
    }

    void UpdateLeaderboard()
    {
        if (leaderboardText == null)
            return;

        var sortedRacers = racers
            .OrderByDescending(r => r.finished)
            .ThenByDescending(r => r.currentLap)
            .ThenBy(r => r.raceTime)
            .ToList();

        leaderboardText.text = "Race Results\n\n";

        for (int i = 0; i < sortedRacers.Count; i++)
        {
            RacerInfo r = sortedRacers[i];

            leaderboardText.text +=
                (i + 1) + ". " +
                r.racerName +
                " | Lap " + r.currentLap + "/" + r.totalLaps +
                " | " + FormatTime(r.raceTime) +
                " | " + (r.finished ? "Finished" : "Racing") +
                "\n";
        }
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int milliseconds = (int)((time * 100f) % 100f);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}

public class RacerInfo
{
    public string racerName;
    public int currentLap;
    public int totalLaps;
    public float raceTime;
    public bool finished;

    public RacerInfo(string name, int laps)
    {
        racerName = name;
        totalLaps = laps;
        currentLap = 1;
        raceTime = 0f;
        finished = false;
    }
}