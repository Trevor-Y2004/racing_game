using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public TextMeshProUGUI speedText;

    void Update()
    {
        float speed = carRigidbody.linearVelocity.magnitude * 3.6f; // km/h

        speedText.text = Mathf.RoundToInt(speed) + " km/h";
    }
}