using UnityEngine;

// Remember to attach this script to a GameObject that has a SpriteRenderer and a Rigidbody2D.
public class CarController : MonoBehaviour
{
    public float moveSpeed = 5f;       // max speed
    public float smoothness = 10f;

    [Header("Visuals")]
   
    public Sprite normalSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; 
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("CarController requires a SpriteRenderer component.");
            enabled = false; 
            return;
        }

        rb.gravityScale = 0;

        // Set the initial sprite to the normal one
        if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    void Update()
    {
        // Input on keyboard
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        if (input.x > 0) // Moving right
        {
            
            if (spriteRenderer.sprite != rightSprite)
            {
                spriteRenderer.sprite = rightSprite;
            }
        }
        else if (input.x < 0) // Moving left
        {
            if (spriteRenderer.sprite != leftSprite)
            {
                spriteRenderer.sprite = leftSprite;
            }
        }
        else 
        {
            if (spriteRenderer.sprite != normalSprite)
            {
                spriteRenderer.sprite = normalSprite;
            }
        }
    }

    void FixedUpdate()
    {

        Vector2 targetVelocity = input * moveSpeed;

        // smoothness
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, smoothness * Time.fixedDeltaTime);
    }
}