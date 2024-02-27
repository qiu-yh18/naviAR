using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRCameraRigKeyboardController : MonoBehaviour
{
    public float speed = 3f;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("CameraHorizontal");
        float verticalInput = Input.GetAxis("CameraVertical");

        // Adjust the OVRCameraRig's position based on the keyboard input
        transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime);
    }
}
