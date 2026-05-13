using UnityEngine;

public class DinoAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    private int currentWaypoint = 0;
    private Rigidbody rb;

    void Start()
{
    rb = GetComponent<Rigidbody>();
    if (waypoints.Length > 0)
    {
        transform.position = waypoints[0].position;
    }
}
    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        // Move toward waypoint using Rigidbody
        Vector3 moveDirection = direction.normalized * moveSpeed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);

        // Rotate to face waypoint
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation *= Quaternion.Euler(0, 180f, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        // Check if reached waypoint
        if (direction.magnitude < 1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
}