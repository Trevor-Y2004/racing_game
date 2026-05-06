using UnityEngine;
using System.Collections;

public class Launch : MonoBehaviour
{
    public float launchForce = 5000f;
    public float glideForce = 90f;
    public float forwardForce = 500f;
    public float airSteerForce = 500f;
    public float glideDuration = 3f;
    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!used)
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                used = true;
                rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
                StartCoroutine(Glide(rb));
                gameObject.SetActive(false);
                Invoke("Respawn", 5f);
            }
        }
    }

    IEnumerator Glide(Rigidbody rb)
    {
        float timer = 0f;
        while (timer < glideDuration)
        {
            // Gentle descent
            rb.AddForce(Vector3.up * glideForce, ForceMode.Force);
            // Keep pushing forward
            rb.AddForce(rb.transform.forward * forwardForce, ForceMode.Force);
            // Air steering
            float horizontal = Input.GetAxis("Horizontal");
            rb.AddTorque(Vector3.up * horizontal * airSteerForce, ForceMode.Force);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    void Respawn()
    {
        used = false;
        gameObject.SetActive(true);
    }
}