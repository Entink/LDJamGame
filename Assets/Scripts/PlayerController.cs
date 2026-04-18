using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject sonarPrefab;

    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float echoCD = 1.0f;
    [SerializeField] private float nextEcho;
    [SerializeField] private float echoSpeed = 4f;
    [SerializeField] private float echoThickness = 1f;
    [SerializeField] private float echoMaxRadius = 10f;

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

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.linearVelocity = new Vector2(movement.x * speed, movement.y * speed) * Time.fixedDeltaTime;
    }


    void FirePing()
    {
        // Spawn the wave at the player's exact position
        Instantiate(sonarPrefab, transform.position, Quaternion.identity);
        

    }

}
