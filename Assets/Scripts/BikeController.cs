using UnityEngine;

public class BikeController : MonoBehaviour
{
    // public float speed = 3f;
    // public float rotationSpeed = 80f;
    public GameObject environment;
    // public OVRCameraRig ovrCameraRig;
    public GameObject trackingSpace;
    private Vector3 oldTrackingSpacePos;
    private Vector3 movement;
    private Vector3 bike2CameraPositionOffset;
    private Vector3 bike2CameraRotationOffset;
    private bool isOldPos = false;
    public float movementThreshold = 0.5f;

    // The relative speed of the environment moving in the opposite direction. 
    // If speedGain = 1, the environment moves the same speed as the bike,
    // making the virtual speed of the bike twice as the physical speed.
    public float speedGain = 1f;

    // The rotation offset of the environment relative to the bike.
    // If angleGain = 1, the environment rotates 1 degree more than the bike.
    public float angleGain = 0.5f;

    private void Awake(){
        // Calculate the initial offset between the bike and the OVRCameraRig
        // bike2CameraPositionOffset = transform.position - trackingSpace.transform.position;
        // bike2CameraRotationOffset = transform.eulerAngles - trackingSpace.transform.eulerAngles;
        // oldTrackingSpacePos = trackingSpace.transform.position;
        // Debug.Log("!!!!!!");
        // Debug.Log(oldTrackingSpacePos);
    }

    private void Start(){
        // Calculate the initial offset between the bike and the OVRCameraRig
        bike2CameraPositionOffset = transform.position - trackingSpace.transform.position;
        bike2CameraRotationOffset = transform.eulerAngles - trackingSpace.transform.eulerAngles;
        oldTrackingSpacePos = trackingSpace.transform.position;
        // Debug.Log("@@@@");
        // Debug.Log(oldTrackingSpacePos);
    }

    private void FixedUpdate()
    {
        // Update the bike's position based on the calculated offset
        if (trackingSpace != null)
        {
            // Calculate user movement between frames
            if(trackingSpace != null && oldTrackingSpacePos != Vector3.zero){
                movement = trackingSpace.transform.position - oldTrackingSpacePos;
                movement[1] = 0.0f;
            }
            if(!isOldPos || oldTrackingSpacePos == Vector3.zero || movement.magnitude > movementThreshold){
                isOldPos = true;
                oldTrackingSpacePos = trackingSpace.transform.position;
                Debug.Log("??????");
                Debug.Log(oldTrackingSpacePos);
            }
            // bike follow camera
            // Vector3 updatedPositionOffset = trackingSpace.transform.TransformVector(bike2CameraPositionOffset);
            // transform.position = trackingSpace.transform.position + updatedPositionOffset;

            // Quaternion updatedRotation = trackingSpace.transform.rotation * Quaternion.Euler(bike2CameraRotationOffset);
            // transform.rotation = updatedRotation;
        }

        // Move the city in the opposite direction of the bike's movement
        if (environment != null)
        {
            if(angleGain > 0){
        //         environment.transform.RotateAround(transform.position, Vector3.up, -angleGain * horizontalInput * rotationSpeed * Time.deltaTime);
        //         environment.transform.Rotate(Vector3.up, -0.2f*angleGain * speedGain * speed * Time.deltaTime);
                environment.transform.RotateAround(trackingSpace.transform.position, Vector3.up, -angleGain*movement.magnitude);
            }
            if(speedGain>0){
                environment.transform.Translate(-speedGain * movement, Space.World);
            }
        }
    }
}
