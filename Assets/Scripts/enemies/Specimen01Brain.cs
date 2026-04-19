using UnityEngine;

public class Specimen01Brain : MonoBehaviour
{
    [SerializeField] private EnemyMotor motor;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private LayerMask wallMask;

    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Speeds")]
    [SerializeField] private float patrolSpeed = 1.8f;
    [SerializeField] private float chaseSpeed = 1.4f;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float waypointReachDistance = 0.2f;
    [SerializeField] private float loseSightDelay = 1.2f;
    [SerializeField] private float frontDotThreshold = 0.35f;

    private int patrolIndex;
    private float loseSightTimer;
    private Vector2 facingDirection = Vector2.right;
    private State currentState;

    private enum State
    {
        Patrol,
        Chase
    }

    private void Awake()
    {
        motor = GetComponent<EnemyMotor>();

        TryFindPlayer();
    }

    private void Start()
    {
        currentState = State.Patrol;
    }

    private void Update()
    {
        if (playerRb == null || patrolPoints.Length == 0)
            return;

        switch(currentState)
        {
            case State.Patrol:
                UpdatePatrol();
                if (CanSeePlayer())
                    currentState = State.Chase;
                break;

            case State.Chase:
                UpdateChase();
                break;
        }
    }
    
    private void UpdatePatrol()
    {
        motor.SetSpeed(patrolSpeed);

        Vector2 targetPos = patrolPoints[patrolIndex].position;
        Vector2 toTarget = targetPos - (Vector2)transform.position;

        if(toTarget.magnitude <= waypointReachDistance)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            return;
        }

        facingDirection = toTarget.normalized;
        motor.SetMoveDirection(facingDirection);
        
    }

    private bool CanSeePlayer()
    {
        Vector2 origin = transform.position;
        Vector2 toPlayer = playerRb.position - (Vector2)transform.position;
        float distance = toPlayer.magnitude;

        if (distance > detectionRange)
            return false;

        Vector2 dir = toPlayer.normalized;
        float dot = Vector2.Dot(facingDirection.normalized, dir);

        if (dot < frontDotThreshold)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, wallMask);
        return hit.collider == null;
    }

    private void UpdateChase()
    {
        motor.SetSpeed(chaseSpeed);

        Vector2 toPlayer = playerRb.position - (Vector2)transform.position;
        facingDirection = toPlayer.normalized;
        motor.SetMoveDirection(facingDirection);

        if (CanSeePlayer())
        {
            loseSightTimer = loseSightDelay;
        }
        else
        {
            loseSightTimer -= Time.deltaTime;
            if (loseSightTimer <= 0f)
                currentState = State.Patrol;
        }
    }

    private void TryFindPlayer()
    {
        GameObject playerObject = GameObject.FindWithTag(playerTag);

        playerRb = playerObject.GetComponent<Rigidbody2D>();
    }

}
