using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float boostForce = 8000f;
    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used)
        {
            used = true;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(other.transform.forward * boostForce, ForceMode.Impulse);
            }
            gameObject.SetActive(false);
            Invoke("Respawn", 5f);
        }
    }

    void Respawn()
    {
        used = false;
        gameObject.SetActive(true);
    }
}