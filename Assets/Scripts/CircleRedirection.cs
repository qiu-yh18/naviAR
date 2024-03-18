using UnityEngine;

public class CircleRedirection : MonoBehaviour
{
    // Adjust this parameter to control the curvature gain effect
    public float alpha = 5.0f;

    public Transform playerTransform;
    private Vector3 previousPlayerPosition;
    private Vector3 startingPoint;

    void Start()
    {
        // Initialize the previous player position
        previousPlayerPosition = playerTransform.position;
        startingPoint = playerTransform.position;
    }

    void Update()
    {
        // Calculate the displacement of the player since the last frame
        Vector3 displacement = playerTransform.position - previousPlayerPosition;

        // Calculate the rotation angle based on the magnitude of the displacement vector
        float rotationAngle = displacement.x * alpha;

        // Apply rotation to the environment around the starting point
        transform.RotateAround(playerTransform.position, Vector3.up, -rotationAngle); // Negative rotation for environment

        // Update the previous player position
        previousPlayerPosition = playerTransform.position;
    }
}
