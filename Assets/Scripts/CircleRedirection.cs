using UnityEngine;

public class CircleRedirection : MonoBehaviour
{
    public bool isGrid = false;
    public float alpha = 1.3f; // Rotation redirection coefficient
    public float beta = 0.5f; // Position redirection coefficient
    private float radius = 12f;
    public CalibrationManager calibrationManager;
    public Transform playerTransform; // camera, will ignore y position
    public Transform circleCenterTransform;
    // public float minDistanceThreshold = 0.1f; // Minimum distance threshold from the circle center
    private Vector3 currentPlayerPositionXZ;
    private Vector3 previousPlayerPositionXZ;
    private Vector3 startPositionXZ;
    private Vector3 circleCenterPositionXZ;
    private float xOnLine;
    private float previousXOnLine;
    private bool isRedirect = false;
    private float rotationAngle = 0f;

    void Start(){
        radius = calibrationManager.radius;
    }

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
        // radius = Mathf.Abs((startPositionXZ - circleCenterPositionXZ).magnitude);
        float theta = Mathf.Atan2(currentPlayerPositionXZ.z - circleCenterPositionXZ.z, currentPlayerPositionXZ.x - circleCenterPositionXZ.x);
        previousXOnLine = radius * Mathf.Cos(theta);
        isRedirect = true;
    }

    void UpdateRedirection()
    {
        currentPlayerPositionXZ = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
        Vector3 displacementXZ = currentPlayerPositionXZ - previousPlayerPositionXZ;
        transform.position -= beta * displacementXZ;
        float distanceToCenter = Vector3.Distance(currentPlayerPositionXZ, circleCenterPositionXZ);
        float normalizedDistance = distanceToCenter / radius;
        float theta = Mathf.Atan2(currentPlayerPositionXZ.z - circleCenterPositionXZ.z, currentPlayerPositionXZ.x - circleCenterPositionXZ.x);
        xOnLine = radius * Mathf.Cos(theta);
        float displacementX = xOnLine - previousXOnLine;
        // For grid maps, disable redirection when the user is in the circle.
        if(isGrid){
            if(normalizedDistance >= 0.55){
                rotationAngle = Mathf.Atan2(displacementX, radius) * Mathf.Rad2Deg * alpha * Mathf.Sign(theta);
            }
            else{ 
                rotationAngle = 0f;
            }
        }
        // For non-grid maps, there is always redirection.
        else{
            rotationAngle = Mathf.Atan2(displacementX, radius) * Mathf.Rad2Deg * alpha * Mathf.Sign(theta) * normalizedDistance;
        }
        transform.RotateAround(currentPlayerPositionXZ, Vector3.up, rotationAngle);
        previousXOnLine = xOnLine;
        previousPlayerPositionXZ = currentPlayerPositionXZ;
    }

}
