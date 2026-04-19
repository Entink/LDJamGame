using UnityEngine;

public class EchoEnemy2D : MonoBehaviour
{
    [Header("Speeds")]
    public float roamSpeed = 1.5f;
    public float investigateSpeed = 16.0f;
    
    [Header("Smoothness (Lerp Factors)")]
    public float acceleration = 5f;    
    public float rotationSpeed = 8f;   

    [Header("Roaming Settings")]
    public float directionChangeInterval = 3f; 
    
    [Header("Searching Settings")]
    [Tooltip("How long the enemy keeps walking forward after reaching the ping source")]
    public float searchDuration = 8.5f; 

    [SerializeField] private Rigidbody2D rb2d;
    private Vector2 targetPosition;
    private bool isInvestigating = false;
    
    private Vector2 currentRoamDirection;
    private float roamTimer;
    
    private Vector2 lastInvestigateDirection;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f; 
        
        PickNewRoamDirection();
    }

    void OnParticleCollision(GameObject other)
    {
        targetPosition = other.transform.position; 
        isInvestigating = true;
    }

    void Update()
    {
        if (!isInvestigating)
        {
            roamTimer -= Time.deltaTime;
            if (roamTimer <= 0)
            {
                PickNewRoamDirection();
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 desiredVelocity;

        if (isInvestigating)
        {
            Vector2 currentPos2D = new Vector2(transform.position.x, transform.position.y);
            Vector2 directionToTarget = (targetPosition - currentPos2D).normalized;
            
            if (directionToTarget.sqrMagnitude > 0.01f)
            {
                lastInvestigateDirection = directionToTarget;
            }

            desiredVelocity = directionToTarget * investigateSpeed;

            if (Vector2.Distance(currentPos2D, targetPosition) < 0.5f)
            {
                isInvestigating = false;
                
                currentRoamDirection = lastInvestigateDirection; 
                
                roamTimer = searchDuration; 
            }
        }
        else
        {
            desiredVelocity = currentRoamDirection * roamSpeed;
        }

        rb2d.linearVelocity = Vector2.Lerp(rb2d.linearVelocity, desiredVelocity, Time.fixedDeltaTime * acceleration);

        if (rb2d.linearVelocity.sqrMagnitude > 0.1f) 
        {
            float angle = Mathf.Atan2(rb2d.linearVelocity.y, rb2d.linearVelocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    void PickNewRoamDirection()
    {
        currentRoamDirection = Random.insideUnitCircle.normalized;
        roamTimer = directionChangeInterval;
    }
}