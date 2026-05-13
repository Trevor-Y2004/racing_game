using UnityEngine;
using TMPro;

public class StartLight : MonoBehaviour
{
    public Light startLight;
    public LapCounter lapCounter;
    public TextMeshProUGUI countdownText;
    public car carController;

    public AudioSource audioSource;
    public AudioClip beepLow;
    public AudioClip beepHigh;

    private float timer = 0f;
    private bool started = false;

    private bool beep1Played = false;
    private bool beep2Played = false;
    private bool beep3Played = false;

    void Start()
    {
        carController.enabled = false;
        startLight.color = Color.red;
        startLight.enabled = false;
        countdownText.text = "GET READY";
    }

    void Update()
    {
        if (started) return;

        timer += Time.deltaTime;

        if (timer >= 1f && !beep1Played)
        {
            FlashLight();
            audioSource.PlayOneShot(beepLow);
            beep1Played = true;
        }

        if (timer >= 2f && !beep2Played)
        {
            FlashLight();
            audioSource.PlayOneShot(beepLow);
            beep2Played = true;
        }

        if (timer >= 3f && !beep3Played)
        {
            FlashLight();
            audioSource.PlayOneShot(beepLow);
            beep3Played = true;
        }

        if (timer >= 4f && !started)
        {
            StartRace();
        }
    }

    void FlashLight()
    {
        startLight.color = Color.red;
        startLight.enabled = true;
        Invoke(nameof(TurnLightOff), 0.2f);
    }

    void TurnLightOff()
    {
        startLight.enabled = false;
    }

    void StartRace()
    {
        started = true;

        audioSource.PlayOneShot(beepHigh);

        startLight.enabled = true;
        startLight.color = Color.green;

        countdownText.text = "GO!";

        carController.enabled = true;
        lapCounter.StartRace();

        Invoke(nameof(ClearText), 1f);
    }

    void ClearText()
    {
        countdownText.text = "";
        startLight.enabled = false;
    }
}