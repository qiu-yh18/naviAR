using UnityEngine;

public class CircleRedirection : MonoBehaviour
{
    // Adjust this parameter to control the curvature gain effect
    public float alpha = 5.0f;
    public Transform playerTransform;
    private Vector3 previousPlayerPositionXZ;
    private Vector3 startPositionXZ;
    public Vector3 circleOriginPosition;

    void Start()
    {
        // Initialize the previous player position
        startPositionXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        previousPlayerPositionXZ = startPositionXZ;
    }

    void FixedUpdate()
    {
        // Transform player's position into environment's local coordinate system
        // Vector3 playerLocalPosition = transform.InverseTransformPoint(playerTransform.position);

        // Calculate the displacement of the player since the last frame in environment's local coordinates
        // Vector3 displacement = playerLocalPosition - transform.InverseTransformPoint(previousPlayerPosition);
        // Debug.Log(displacement);

        Vector3 displacementXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z) - previousPlayerPositionXZ;
        Vector3 toOrigin = circleOriginPosition - previousPlayerPositionXZ;
        float product = Vector3.Dot(displacementXZ.normalized, toOrigin.normalized);
        int direction = (int)Mathf.Sign(product);
        float rotationAngle = displacementXZ.magnitude * alpha * direction;
        // Calculate the rotation angle based on the magnitude of the displacement vector
        // float rotationAngle = displacementXZ.magnitude * alpha;
        
        // Apply rotation to the environment around the starting point
        transform.RotateAround(playerTransform.position, Vector3.up, -rotationAngle); // Negative rotation for environment

        // Update the previous player position
        previousPlayerPositionXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
    }
}
