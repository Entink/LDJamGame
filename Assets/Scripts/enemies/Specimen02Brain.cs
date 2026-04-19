using UnityEngine;

public class Specimen02Brain : MonoBehaviour
{
    [SerializeField] private EnemyMotor motor;

    [Header("Speeds")]
    [SerializeField] private float roamSpeed = 0.35f;
    [SerializeField] private float investigateSpeed = 3.5f;

    [Header("Roaming")]
    [SerializeField] private float directionChangeInterval = 3f;

    [Header("Investigating")]
    [SerializeField] private float targetReachDistance = 0.5f;
    [SerializeField] private float searchDuration = 2f;

    private State currentState;

    private Vector2 currentRoamDirection;
    private Vector2 investigateTargetPosition;
    private Vector2 lastInvestigateDirection = Vector2.right;

    private float roamTimer;
    private float searchTimer;

    private enum State
    {
        Roam,
        Investigate,
        Search
    }

    private void Awake()
    {
        motor = GetComponent<EnemyMotor>();
    }

    private void Start()
    {
        currentState = State.Roam;
        PickNewRoamDirection();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Roam:
                UpdateRoam();
                break;

            case State.Investigate:
                UpdateInvestigate();
                break;

            case State.Search:
                UpdateSearch();
                break;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other == null)
            return;

        Vector2 newTargetPosition = other.transform.position;

        StartInvestigating(newTargetPosition);
    }

    private void UpdateRoam()
    {
        motor.SetSpeed(roamSpeed);
        motor.SetMoveDirection(currentRoamDirection);

        roamTimer -= Time.deltaTime;
        if (roamTimer <= 0f)
            PickNewRoamDirection();
    }

    private void UpdateInvestigate()
    {
        Vector2 currentPosition = transform.position;
        Vector2 toTarget = investigateTargetPosition - currentPosition;

        if (toTarget.sqrMagnitude > 0.001f)
            lastInvestigateDirection = toTarget.normalized;

        motor.SetSpeed(investigateSpeed);
        motor.SetMoveDirection(lastInvestigateDirection);

        if (toTarget.magnitude <= targetReachDistance)
            StartSearch();
    }

    private void UpdateSearch()
    {
        motor.SetSpeed(roamSpeed);
        motor.SetMoveDirection(lastInvestigateDirection);

        searchTimer -= Time.deltaTime;

        if (searchTimer <= 0f)
        {
            currentState = State.Roam;
            PickNewRoamDirection();
        }
    }

    private void StartInvestigating(Vector2 targetPosition)
    {
        investigateTargetPosition = targetPosition;
        currentState = State.Investigate;
    }

    private void StartSearch()
    {
        currentState = State.Search;
        searchTimer = searchDuration;
    }

    private void PickNewRoamDirection()
    {
        currentRoamDirection = Random.insideUnitCircle.normalized;

        if (currentRoamDirection.sqrMagnitude <= 0.001f)
            currentRoamDirection = Vector2.right;

        roamTimer = directionChangeInterval;
    }
}