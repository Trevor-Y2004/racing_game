using UnityEngine;
using TMPro;

public class StartLight : MonoBehaviour
{
    [Header("Lights")]
    public Light light1;
    public Light light2;
    public Light light3;

    [Header("UI")]
    public TextMeshProUGUI countdownText;

    [Header("Cars")]
    public CarMotor[] carControllers;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip beepLow;
    public AudioClip beepHigh;

    private float timer = 0f;
    private bool started = false;

    private bool step1 = false;
    private bool step2 = false;
    private bool step3 = false;
    private bool step4 = false;

    void Start()
    {
        SetCarsEnabled(false);

        // Turn off lights initially
        SetLight(light1, false, Color.red);
        SetLight(light2, false, Color.red);
        SetLight(light3, false, Color.red);

        if (countdownText != null)
            countdownText.text = "GET READY";
    }

    void Update()
    {
        if (started) return;

        timer += Time.deltaTime;

        // First beep
        if (timer >= 1f && !step1)
        {
            ActivateLight(light1, Color.red);
            PlayBeepLow();
            step1 = true;
        }

        // Second beep
        if (timer >= 2f && !step2)
        {
            ActivateLight(light2, Color.red);
            PlayBeepLow();
            step2 = true;
        }

        // Third beep
        if (timer >= 3f && !step3)
        {
            ActivateLight(light3, Color.red);
            PlayBeepLow();
            step3 = true;
        }

        // Final beep (GO)
        if (timer >= 4f && !step4)
        {
            StartRace();
            step4 = true;
        }
    }

    void ActivateLight(Light light, Color color)
    {
        if (light == null) return;

        light.color = color;
        light.enabled = true;
    }

    void SetLight(Light light, bool state, Color color)
    {
        if (light == null) return;

        light.enabled = state;
        light.color = color;
    }

    void PlayBeepLow()
    {
        if (audioSource != null && beepLow != null)
            audioSource.PlayOneShot(beepLow);
    }

    void PlayBeepHigh()
    {
        if (audioSource != null && beepHigh != null)
            audioSource.PlayOneShot(beepHigh);
    }

    void StartRace()
    {
        started = true;

        PlayBeepHigh();

        // Turn ALL lights green
        SetLight(light1, true, Color.green);
        SetLight(light2, true, Color.green);
        SetLight(light3, true, Color.green);

        if (countdownText != null)
            countdownText.text = "GO!";

        SetCarsEnabled(true);

        // Start race timers (player + AI)
        foreach (CarMotor car in carControllers)
        {
            if (car != null)
            {
                LapCounter lap = car.GetComponent<LapCounter>();
                if (lap != null)
                    lap.StartRace();

                AILapCounter aiLap = car.GetComponent<AILapCounter>();
                if (aiLap != null)
                    aiLap.StartRace();
            }
        }

        Invoke(nameof(ClearText), 1f);
    }

    void SetCarsEnabled(bool enabled)
    {
        foreach (CarMotor car in carControllers)
        {
            if (car != null)
                car.enabled = enabled;
        }
    }

    void ClearText()
    {
        if (countdownText != null)
            countdownText.text = "";
    }
}