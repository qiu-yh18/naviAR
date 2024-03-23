using UnityEngine;

public class CircleRedirection : MonoBehaviour
{
    public float alpha = 5.0f;
    public Transform playerTransform;
    private Vector3 previousPlayerPositionXZ;
    private Vector3 startPositionXZ;
    public Transform originTransform;
    private Vector3 circleOriginPositionXZ;
    private float radius;
    void Start()
    {
        startPositionXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        previousPlayerPositionXZ = startPositionXZ;
        circleOriginPositionXZ = new Vector3(originTransform.position.x, 0, originTransform.position.z);
        radius = Math.abs((startPositionXZ - circleOriginPositionXZ).magnitude);
    }

    void Update()
    {
        Vector3 displacementXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z) - previousPlayerPositionXZ;
        Vector3 toOrigin = circleOriginPositionXZ - previousPlayerPositionXZ;
        float product = Vector3.Dot(displacementXZ.normalized, toOrigin.normalized);
        int direction = (int)Mathf.Sign(product);
        float rotationAngle = displacementXZ.magnitude * alpha * direction;
        transform.RotateAround(playerTransform.position, Vector3.up, -rotationAngle);
        previousPlayerPositionXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
    }
}
