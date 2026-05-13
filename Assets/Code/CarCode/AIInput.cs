using UnityEngine;

public class AIInput : MonoBehaviour
{
    [Header("References")]
    public CarMotor carMotor;
    public Transform trackPathParent;
    public Transform player;

    [Header("Pathing")]
    public float checkpointReachDistance = 12f;
    public float lookAheadDistance = 8f;

    [Header("Racing Lines")]
    public float laneWidth = 4f;
    public float racingLineOffset = 0f;
    public bool randomizeRacingLine = true;

    [Header("AI Driving")]
    public float throttleAmount = 1f;
    public float cornerSlowdownAngle = 40f;
    public float sharpCornerSlowdown = 0.55f;
    public float driftAngle = 35f;

    [Header("Rubberbanding")]
    public bool useRubberbanding = true;
    public float rubberbandStrength = 0.25f;
    public float minThrottleMultiplier = 0.75f;
    public float maxThrottleMultiplier = 1.25f;

    [Header("Obstacle Avoidance")]
    public bool useObstacleAvoidance = true;
    public float avoidDistance = 12f;
    public float avoidStrength = 0.8f;
    public LayerMask obstacleLayers;

    private Transform[] checkpoints;
    private int currentCheckpointIndex;

    void Start()
    {
        if (carMotor == null)
            carMotor = GetComponent<CarMotor>();

        LoadCheckpoints();

        if (randomizeRacingLine)
            racingLineOffset = Random.Range(-1f, 1f);
    }

    void Update()
    {
        if (carMotor == null || checkpoints == null || checkpoints.Length == 0)
            return;

        Transform checkpoint = checkpoints[currentCheckpointIndex];

        Vector3 targetPosition =
            checkpoint.position +
            checkpoint.right * racingLineOffset * laneWidth;

        Vector3 directionToTarget = targetPosition - transform.position;
        Vector3 localTarget = transform.InverseTransformPoint(targetPosition);

        float steer = Mathf.Clamp(localTarget.x / Mathf.Max(localTarget.magnitude, 0.01f), -1f, 1f);

        if (useObstacleAvoidance)
        {
            steer += GetObstacleAvoidanceSteer();
            steer = Mathf.Clamp(steer, -1f, 1f);
        }

        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        float throttle = throttleAmount;

        if (angleToTarget > cornerSlowdownAngle)
            throttle *= sharpCornerSlowdown;

        if (useRubberbanding)
            throttle *= GetRubberbandMultiplier();

        throttle = Mathf.Clamp(throttle, -1f, 1f);

        bool drift = angleToTarget > driftAngle;

        carMotor.SetInputs(steer, throttle, drift);

        if (Vector3.Distance(transform.position, checkpoint.position) < checkpointReachDistance)
        {
            GoToNextCheckpoint();
        }
    }

    void LoadCheckpoints()
    {
        if (trackPathParent == null)
        {
            Debug.LogError(name + " has no Track Path Parent assigned.");
            return;
        }

        checkpoints = new Transform[trackPathParent.childCount];

        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i] = trackPathParent.GetChild(i);
        }
    }

    void GoToNextCheckpoint()
    {
        currentCheckpointIndex++;

        if (currentCheckpointIndex >= checkpoints.Length)
            currentCheckpointIndex = 0;

        if (randomizeRacingLine)
            racingLineOffset = Random.Range(-1f, 1f);
    }

    float GetRubberbandMultiplier()
    {
        if (player == null)
            return 1f;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        bool roughlyBehindPlayer =
            Vector3.Dot(player.position - transform.position, transform.forward) > 0f;

        if (roughlyBehindPlayer)
        {
            return Mathf.Clamp(
                1f + distanceToPlayer * 0.01f * rubberbandStrength,
                minThrottleMultiplier,
                maxThrottleMultiplier
            );
        }

        return Mathf.Clamp(
            1f - distanceToPlayer * 0.005f * rubberbandStrength,
            minThrottleMultiplier,
            maxThrottleMultiplier
        );
    }

    float GetObstacleAvoidanceSteer()
    {
        float avoidSteer = 0f;

        Vector3 origin = transform.position + Vector3.up * 1f;

        bool centerHit = Physics.Raycast(
            origin,
            transform.forward,
            avoidDistance,
            obstacleLayers
        );

        bool leftHit = Physics.Raycast(
            origin,
            -transform.right,
            avoidDistance * 0.6f,
            obstacleLayers
        );

        bool rightHit = Physics.Raycast(
            origin,
            transform.right,
            avoidDistance * 0.6f,
            obstacleLayers
        );

        if (centerHit)
            avoidSteer += Random.value > 0.5f ? avoidStrength : -avoidStrength;

        if (leftHit)
            avoidSteer += avoidStrength;

        if (rightHit)
            avoidSteer -= avoidStrength;

        return avoidSteer;
    }
}