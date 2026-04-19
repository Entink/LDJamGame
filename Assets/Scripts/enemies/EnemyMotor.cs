using UnityEngine;

public class EnemyMotor : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 2f;

    private Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    public void StopMoving()
    {
        moveDirection = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

}
