using UnityEngine;

public class CircleRedirection : MonoBehaviour
{
    public float alpha = 3.0f;
    public CalibrateManager calibrationManager;
    public Transform playerTransform; // camera, will ignore y position
    private Vector3 currentPlayerPositionXZ;
    private Vector3 previousPlayerPositionXZ;
    private Vector3 startPositionXZ;
    // private Vector3 startRotationXZ;
    public Transform circleCenterTransform;
    private Vector3 circleCenterPositionXZ;
    private float radius;
    private float xOnLine;
    private float previousXOnLine;
    private bool isRedirect = false;
    void Start()
    {

    }
    void Update()
    {
        if(calibrationManager.isStartButtonActivated){
            if(!isRedirect){
                startPositionXZ = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
                previousPlayerPositionXZ = startPositionXZ;
                currentPlayerPositionXZ = startPositionXZ;
                circleCenterPositionXZ = new Vector3(circleCenterTransform.position.x, 0f, circleCenterTransform.position.z);
                radius = Mathf.Abs((startPositionXZ - circleCenterPositionXZ).magnitude);
                previousXOnLine = 0f;
                isRedirect = true;
            }
            else{
                currentPlayerPositionXZ = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
                float theta = Mathf.Atan2(currentPlayerPositionXZ.z - circleCenterPositionXZ.z, currentPlayerPositionXZ.x - circleCenterPositionXZ.x);
                xOnLine = radius * Mathf.Cos(theta);
                float displacementX = xOnLine - previousXOnLine;
                float rotationAngle = Mathf.Atan2(displacementX, radius) * Mathf.Rad2Deg * alpha;
                transform.RotateAround(playerTransform.position, Vector3.up, -rotationAngle);
                previousPlayerPositionXZ = currentPlayerPositionXZ;
                previousXOnLine = xOnLine;
            }
        }
    }
}
