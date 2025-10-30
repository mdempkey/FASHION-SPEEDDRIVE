using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class SceneExitFlasher : MonoBehaviour
{
    [Header("Flashing Settings")]
    [Tooltip("How quickly the sprite toggles visibility (smaller number is faster flash).")]
    public float flashRate = 0.25f; 

    [Header("Scene Transition Settings")]
    [Tooltip("The name of the scene to load when the player enters this trigger. Set to 'FashionScene' by default.")]
    public string nextSceneName = "FashionScene"; 
    [Tooltip("The Tag of the object that should trigger the transition (usually 'Player').")]
    public string playerTag = "Player";

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SceneExitFlasher requires a SpriteRenderer component on the same GameObject.");
            enabled = false;
            return;
        }

        StartCoroutine(ContinuousFlash());
    }

   
    IEnumerator ContinuousFlash()
    {
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashRate);
        }
    }

 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player reached the exit! Loading scene: " + nextSceneName);
            
            StopAllCoroutines(); 
            
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
