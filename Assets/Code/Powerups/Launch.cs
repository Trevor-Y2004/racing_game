using UnityEngine;
using System.Collections;

public class Launch : MonoBehaviour
{
    public float launchForce = 5000f;
    public float glideForce = 90f;
    public float forwardForce = 500f;
    public float airSteerForce = 500f;
    public float glideDuration = 3f;
    public float rotateSpeed = 90f;
    public float bobHeight = 0.3f;
    public float bobSpeed = 2f;

    private bool used = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (!other.CompareTag("Player") && !other.CompareTag("AI"))
            return;

        Rigidbody rb = other.GetComponentInParent<Rigidbody>();

        if (rb == null)
            return;

        used = true;

        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

        StartCoroutine(Glide(rb));

        gameObject.SetActive(false);
        Invoke(nameof(Respawn), 5f);
    }

    IEnumerator Glide(Rigidbody rb)
    {
        CarMotor carScript = rb.GetComponent<CarMotor>();

        if (carScript != null)
            carScript.isGliding = true;

        float timer = 0f;

        while (timer < glideDuration)
        {
            rb.AddForce(Vector3.up * glideForce, ForceMode.Force);
            rb.AddForce(rb.transform.forward * forwardForce, ForceMode.Force);

            float horizontal = 0f;

            if (carScript != null)
                horizontal = carScript.CurrentSteerInput;

            rb.AddTorque(Vector3.up * horizontal * airSteerForce, ForceMode.Force);

            timer += Time.deltaTime;
            yield return null;
        }

        if (carScript != null)
            carScript.isGliding = false;
    }

    void Respawn()
    {
        used = false;
        gameObject.SetActive(true);
    }
}