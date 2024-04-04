using UnityEngine;

public class CircleRedirection : MonoBehaviour
{
    public float alpha = 3.0f;
    public CalibrateManager calibrationManager;
    public Transform playerTransform; // camera, will ignore y position
    public Transform circleCenterTransform;
    public float minDistanceThreshold = 0.1f; // Minimum distance threshold from the circle center
    private Vector3 currentPlayerPositionXZ;
    private Vector3 previousPlayerPositionXZ;
    private Vector3 startPositionXZ;
    private Vector3 circleCenterPositionXZ;
    private float radius;
    private float xOnLine;
    private float previousXOnLine;
    private bool isRedirect = false;

    void Update()
    {
        if (calibrationManager.isStartButtonActivated)
        {
            if (!isRedirect)
            {
                InitializeRedirection();
            }
            else
            {
                UpdateRedirection();
            }
        }
        else
        {
            isRedirect = false;
        }
    }

    void InitializeRedirection()
    {
        startPositionXZ = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
        currentPlayerPositionXZ = startPositionXZ;
        previousPlayerPositionXZ = startPositionXZ;
        circleCenterPositionXZ = new Vector3(circleCenterTransform.position.x, 0f, circleCenterTransform.position.z);
        radius = Mathf.Abs((startPositionXZ - circleCenterPositionXZ).magnitude);
        previousXOnLine = 0f;
        isRedirect = true;
    }

    void UpdateRedirection()
    {
        currentPlayerPositionXZ = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
        float distanceToCenter = Vector3.Distance(currentPlayerPositionXZ, circleCenterPositionXZ);
        float normalizedDistance = Mathf.Clamp01(distanceToCenter / radius); // normalize distance to [0,1]
        float theta = Mathf.Atan2(currentPlayerPositionXZ.z - circleCenterPositionXZ.z, currentPlayerPositionXZ.x - circleCenterPositionXZ.x);
        xOnLine = radius * Mathf.Cos(theta);
        float displacementX = xOnLine - previousXOnLine;
        float rotationAngle = Mathf.Atan2(displacementX, radius) * Mathf.Rad2Deg * alpha * Mathf.Sign(theta) * normalizedDistance;
        transform.RotateAround(playerTransform.position, Vector3.up, rotationAngle);
        previousXOnLine = xOnLine;
    }

}
