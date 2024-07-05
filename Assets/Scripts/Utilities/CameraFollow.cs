using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target != null)
        {
            // Directly set the camera's position to the target's position
            Vector3 targetPosition = target.position;
            targetPosition.z = transform.position.z; // Keep the camera's Z position fixed
            transform.position = targetPosition;
        }
    }
}
