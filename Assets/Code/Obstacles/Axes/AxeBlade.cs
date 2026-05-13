using UnityEngine;

public class AxeBlade : MonoBehaviour
{
    public float knockForce = 3000f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                Vector3 knockDirection = other.transform.position - transform.position;
                knockDirection.Normalize();
                rb.AddForce(knockDirection * knockForce, ForceMode.Impulse);
            }
        }
    }
}