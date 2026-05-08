using UnityEngine;
using TMPro;

public class StartLight : MonoBehaviour
{
    public Light startLight;
    public LapCounter lapCounter;
    public TextMeshProUGUI countdownText;
    public car carController;

    private float timer = 0f;
    private bool started = false;

    void Start()
    {
        carController.enabled = false;
        startLight.color = Color.red;
        startLight.enabled = true;
    }

    void Update()
    {
        if (started) return;

        timer += Time.deltaTime;

        // Flash red for 3 seconds
        if (timer < 3f)
        {
            startLight.enabled = Mathf.Sin(timer * 5f) > 0;
            countdownText.text = "GET READY";
        }
        // Turn green and GO
        else if (timer >= 3f && !started)
        {
            startLight.enabled = true;
            startLight.color = Color.green;
            countdownText.text = "GO!";
            carController.enabled = true;
            lapCounter.StartRace();
            started = true;
            Invoke("ClearText", 1f);
        }
    }

    void ClearText()
    {
        countdownText.text = "";
        startLight.enabled = false;
    }
}