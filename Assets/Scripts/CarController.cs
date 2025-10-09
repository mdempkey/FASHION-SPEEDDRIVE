using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 5f;       // max speed
    public float smoothness = 10f;    

    private Rigidbody2D rb;
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; 
    }

    void Update()
    {
        // input on keyboard
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void FixedUpdate()
    {
        
        Vector2 targetVelocity = input * moveSpeed;

        //smoothness
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, smoothness * Time.fixedDeltaTime);
    }
}