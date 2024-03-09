using UnityEngine;

public class BikeController : MonoBehaviour
{
    // public float speed = 3f;
    // public float rotationSpeed = 80f;
    public GameObject environment;
    public OVRCameraRig ovrCameraRig;
    private Vector3 oldCameraPos;
    public GameObject trackingSpace;
    private Vector3 oldTrackingSpacePos;
    public Vector3 bike2CameraPositionOffset;
    public Vector3 bike2CameraRotationOffset;

    // The relative speed of the environment moving in the opposite direction. 
    // If speedGain = 1, the environment moves the same speed as the bike,
    // making the virtual speed of the bike twice as the physical speed.
    public float speedGain = 1f;

    // The rotation offset of the environment relative to the bike.
    // If angleGain = 1, the environment rotates 1 degree more than the bike.
    public float angleGain = 0.5f;

    private void Awake(){
        // Calculate the initial offset between the bike and the OVRCameraRig
        bike2CameraPositionOffset = transform.position - ovrCameraRig.transform.position;
        bike2CameraRotationOffset = transform.eulerAngles - ovrCameraRig.transform.eulerAngles;
        oldCameraPos = ovrCameraRig.transform.position;
        // Debug.Log(oldCameraPos);
        oldTrackingSpacePos = trackingSpace.transform.position;
        Debug.Log("!!!!!!");
        Debug.Log(oldTrackingSpacePos);
    }

    private void Update()
    {
        // Update the bike's position based on the calculated offset
        if (ovrCameraRig != null)
        {
            if(ovrCameraRig.transform.position != oldCameraPos){
                oldCameraPos = ovrCameraRig.transform.position;
                // Debug.Log(oldCameraPos);
            }
            if(trackingSpace.transform.position != oldTrackingSpacePos){
                oldTrackingSpacePos = trackingSpace.transform.position;
                Debug.Log("??????");
                Debug.Log(oldTrackingSpacePos);
            }
            // Calculate the updated position offset
            Vector3 updatedPositionOffset = ovrCameraRig.transform.TransformVector(bike2CameraPositionOffset);
            transform.position = ovrCameraRig.transform.position + updatedPositionOffset;

            // Calculate the updated rotation offset
            Quaternion updatedRotation = ovrCameraRig.transform.rotation * Quaternion.Euler(bike2CameraRotationOffset);
            transform.rotation = updatedRotation;
        }

        // Move the city in the opposite direction of the bike's movement
        // if (environment != null)
        // {
        //     if(angleGain>0){
        //         environment.transform.RotateAround(transform.position, Vector3.up, -angleGain * horizontalInput * rotationSpeed * Time.deltaTime);
        //         environment.transform.Rotate(Vector3.up, -0.2f*angleGain * speedGain * speed * Time.deltaTime);
        //     }
        //     if(speedGain>0){
        //         environment.transform.Translate(-speedGain*transform.forward * speed * Time.deltaTime, Space.World);
        //     }
        // }
    }
}
