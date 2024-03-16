using UnityEngine;

public class CircleRedirection : MonoBehaviour
{
    // Adjust this parameter to control the curvature gain effect
    public float alpha = 5.0f;

    public Transform playerTransform;
    private Vector3 previousPlayerPosition;

    void Start()
    {
        // Initialize the previous player position
        previousPlayerPosition = playerTransform.position;
    }

    void Update()
    {
        // Calculate the displacement of the player since the last frame
        Vector3 displacement = playerTransform.position - previousPlayerPosition;

        // Project the displacement onto the XZ plane
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);

        // Calculate the rotation axis using cross product
        Vector3 rotationAxis = Vector3.Cross(Vector3.forward, displacementXZ).normalized;

        // Calculate the rotation angle based on the displacement magnitude
        float rotationAngle = displacementXZ.magnitude * alpha;

        // Apply rotation to the environment
        transform.RotateAround(playerTransform.position, rotationAxis, -rotationAngle); // Negative rotation for environment

        // Update the previous player position
        previousPlayerPosition = playerTransform.position;
    }
}
