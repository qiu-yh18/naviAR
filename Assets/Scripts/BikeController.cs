using UnityEngine;

public class BikeController : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 80f;
    public GameObject environment;
    public OVRCameraRig ovrCameraRig;
    public Vector3 bike2CameraPositionOffset;
    public Vector3 bike2CameraRotationOffset;

    // The relative speed of the environment moving in the opposite direction. 
    // If speedGain = 1, the environment moves the same speed as the bike,
    // making the virtual speed of the bike twice as the physical speed.
    public float speedGain = 1f;

    // The rotation offset of the environment relative to the bike.
    // If angleGain = 1, the environment rotates 1 degree more than the bike.
    public float angleGain = 0.5f;

    private void Start(){
        // Calculate the initial offset between the bike and the OVRCameraRig
        bike2CameraPositionOffset = transform.position - ovrCameraRig.transform.position;
        bike2CameraRotationOffset = transform.eulerAngles - ovrCameraRig.transform.eulerAngles;
    }

    private void Update()
    {
        // Set the bike's position to the OVRCameraRig's position with the offset
        if (ovrCameraRig != null)
        {
            transform.position = ovrCameraRig.transform.position + ovrCameraRig.transform.TransformVector(bike2CameraPositionOffset);
            
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
