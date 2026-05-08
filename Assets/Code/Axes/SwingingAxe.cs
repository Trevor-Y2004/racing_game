using UnityEngine;

public class SwingingAxe : MonoBehaviour
{
    public float swingSpeed = 2f;
    public float swingAngle = 60f;

    private float startAngle;

    void Start()
    {
        startAngle = transform.eulerAngles.x;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.eulerAngles = new Vector3(startAngle + angle, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}