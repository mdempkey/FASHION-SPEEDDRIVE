using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // the car
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10); // keep camera on car

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // camera flow?
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = new Vector3(smoothed.x, smoothed.y, desiredPosition.z);
    }
}