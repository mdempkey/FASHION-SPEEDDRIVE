using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 5f;       // max speed
    public float smoothness = 10f;

    [Header("Visuals")]
    public Sprite normalSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    [Header("Booster Settings")]
    public float normalSpeed = 5f; 
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; 
    private Vector2 input;
    
    // Boosters
    private bool hasShield = false;
    private float scoreMultiplier = 1f;

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
        normalSpeed = moveSpeed; // going to normal speed

        if (normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        if (input.x > 0)
        {
            if (spriteRenderer.sprite != rightSprite)
            {
                spriteRenderer.sprite = rightSprite;
            }
        }
        else if (input.x < 0)
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
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, smoothness * Time.fixedDeltaTime);
    }

    // boosters
    
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }
    
    IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        moveSpeed = normalSpeed * multiplier;
        Debug.Log($"Speed Boost Active! Speed: {moveSpeed}");
        
        yield return new WaitForSeconds(duration);
        
        moveSpeed = normalSpeed;
        Debug.Log("Speed Boost Ended");
    }
    
    public void ApplyShield(float duration)
    {
        StartCoroutine(ShieldCoroutine(duration));
    }
    
    IEnumerator ShieldCoroutine(float duration)
    {
        hasShield = true;
        Debug.Log("Shield Active!");
        
        yield return new WaitForSeconds(duration);
        
        hasShield = false;
        Debug.Log("Shield Ended");
    }
    
    public void AddTime(float seconds)
    {
        Debug.Log($"CarController.AddTime called with {seconds} seconds");
    
        if (GameTimer.instance != null)
        {
            Debug.Log("GameTimer instance found!");
            GameTimer.instance.AddTime(seconds);
        }
        else
        {
            Debug.LogWarning("GameTimer instance not found!");
        }
    }
    
    public void ApplyScoreMultiplier(float multiplier, float duration)
    {
        StartCoroutine(ScoreMultiplierCoroutine(multiplier, duration));
    }
    
    IEnumerator ScoreMultiplierCoroutine(float multiplier, float duration)
    {
        scoreMultiplier = multiplier;
        Debug.Log($"Score Multiplier: x{multiplier}");
        
        yield return new WaitForSeconds(duration);
        
        scoreMultiplier = 1f;
        Debug.Log("Score Multiplier Ended");
    }
    
    public void AddScore(int points)
    {
        int finalScore = Mathf.RoundToInt(points * scoreMultiplier);
        Debug.Log($"Score +{finalScore}");
        
    }
    
    public bool HasShield()
    {
        return hasShield;
    }
}

