using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float boostForce = 8000f;
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
        if (other.CompareTag("Player") && !used)
        {
            used = true;
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(other.transform.forward * boostForce, ForceMode.Impulse);
                car carScript = rb.GetComponent<car>();
                if (carScript != null)
                {
                    carScript.TriggerBoostFOV();
                }
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