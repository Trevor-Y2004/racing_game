using UnityEngine;
using System.Collections;

public class OilSpill : MonoBehaviour
{
    public float slipTime = 2f;
    public float slipTorque = 800f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                float direction = Random.value > 0.5f ? 1f : -1f;
                StartCoroutine(Slip(rb, direction));
            }
        }
    }

    IEnumerator Slip(Rigidbody rb, float direction)
    {
        float timer = 0f;
        while (timer < slipTime)
        {
            rb.AddTorque(Vector3.up * slipTorque * direction, ForceMode.Force);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}