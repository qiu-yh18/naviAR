using UnityEngine;

public class CircleRedirection : MonoBehaviour
{
    public float alpha = 5.0f;
    public Transform playerTransform;
    private Vector3 currentPlayerPositionXZ;
    private Vector3 previousPlayerPositionXZ;
    private Vector3 startPositionXZ;
    private Vector3 startRotationXZ;
    public Transform originTransform;
    private Vector3 circleOriginPositionXZ;
    private float radius;
    private float xOnLine;
    private float previousXOnLine;
    void Start()
    {
        startPositionXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        previousPlayerPositionXZ = startPositionXZ;
        circleOriginPositionXZ = new Vector3(originTransform.position.x, 0, originTransform.position.z);
        radius = Mathf.Abs((startPositionXZ - circleOriginPositionXZ).magnitude);
        previousXOnLine = 0f;
    }

    void Update()
    {
        currentPlayerPositionXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        float theta = Mathf.Atan2(currentPlayerPositionXZ.z - circleOriginPositionXZ.z, currentPlayerPositionXZ.x - circleOriginPositionXZ.x);
        xOnLine = radius * Mathf.Cos(theta);
        float displacementX = xOnLine - previousXOnLine;
        float rotationAngle = Mathf.Atan2(displacementX, radius) * Mathf.Rad2Deg * alpha;
        transform.RotateAround(playerTransform.position, Vector3.up, -rotationAngle);
        previousPlayerPositionXZ = currentPlayerPositionXZ;
        previousXOnLine = xOnLine;
    }
}
