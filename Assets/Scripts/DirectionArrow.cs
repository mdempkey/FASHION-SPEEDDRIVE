using UnityEngine;
using UnityEngine.UI;

public class DirectionArrow : MonoBehaviour
{
    public RawImage arrowImage;
    public Transform playerBus;
    public Transform currentTarget;
    public Camera gameCamera; // Assign your main camera
    
    public float rotationOffset = 0f; // Adjust based on arrow sprite orientation

    void Update()
    {
        if (currentTarget == null || playerBus == null || gameCamera == null) return;

        // Convert world positions to screen space
        Vector3 targetScreenPos = gameCamera.WorldToScreenPoint(currentTarget.position);
        Vector3 playerScreenPos = gameCamera.WorldToScreenPoint(playerBus.position);
        
        // Calculate direction in screen space
        Vector2 direction = new Vector2(
            targetScreenPos.x - playerScreenPos.x,
            targetScreenPos.y - playerScreenPos.y
        );
        
        // Calculate angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rotate the arrow (adjust rotationOffset based on your arrow's default orientation)
        arrowImage.transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }
}