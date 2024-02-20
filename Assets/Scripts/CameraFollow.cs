using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;
    public Vector3 cameraOffset = new Vector3(0f, 1f, -1f);
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        // Calculate the target position based on the bike's position and camera offset
        Vector3 targetPosition = targetObject.position + targetObject.TransformDirection(cameraOffset);

        // Smoothly interpolate between the current position and the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationSpeed);

        // Calculate the target rotation based on the bike's forward direction
        Quaternion targetRotation = Quaternion.LookRotation(targetObject.forward, targetObject.up);

        // Smoothly interpolate between the current rotation and target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
