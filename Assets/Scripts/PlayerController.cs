using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject sonarPrefab;

    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float echoCD = 1.0f;
    [SerializeField] private float nextEcho;

    [SerializeField] private bool canMove = false;
    [SerializeField] private bool canUsePing = false;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextEcho)
        {
            nextEcho = Time.time + echoCD;
            FirePing();
            Debug.Log("Echo");

        }

        if(!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.linearVelocity = new Vector2(movement.x * speed, movement.y * speed);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;

        if(!canMove)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetCanUsePing(bool value)
    {
        canUsePing = value;
    }

    public void ForceFirePing()
    {
        nextEcho = Time.time + echoCD;
        FirePing();
    }


    void FirePing()
    {
        Instantiate(sonarPrefab, transform.position, Quaternion.identity);
        

    }

}
