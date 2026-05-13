using System.Collections;
using UnityEngine;

public class AxeBlade : MonoBehaviour
{
    public float knockForce = 50000f;
    public float stunTime = 1f;
    public float hitCooldown = 1f;

    private Vector3 lastPosition;
    private Vector3 axeVelocity;
    private bool canHit = true;

    void Start()
    {
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        axeVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit)
            return;

        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            CarMotor carMotor = other.GetComponentInParent<CarMotor>();

            if (rb != null)
            {
                Vector3 knockDirection = axeVelocity.normalized;

                if (knockDirection.magnitude < 0.1f)
                    knockDirection = transform.forward;

                rb.linearVelocity = Vector3.zero;
                rb.AddForce(knockDirection * knockForce + Vector3.up * 4f, ForceMode.Impulse);
            }

            if (carMotor != null)
                StartCoroutine(StunPlayer(carMotor));

            StartCoroutine(HitCooldown());
        }
    }

    IEnumerator StunPlayer(CarMotor carMotor)
    {
        carMotor.enabled = false;
        yield return new WaitForSeconds(stunTime);
        carMotor.enabled = true;
    }

    IEnumerator HitCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(hitCooldown);
        canHit = true;
    }
}