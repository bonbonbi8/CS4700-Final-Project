using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;        // drag your Player here in Inspector
    public float smoothTime = 0.15f;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (!target) return;

        // Desired position (stick to player X/Y, keep current camera Z)
        Vector3 desired = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Smoothly move camera toward desired position
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}
