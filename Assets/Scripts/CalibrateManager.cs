using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateManager : MonoBehaviour
{
    public GameObject controller; // Reference to the controller GameObject
    public Transform environmentToCalibrate; // Reference to the environment you want to calibrate
    public Camera mainCamera; // Reference to the main camera

    private void Update()
    {
        RaycastHit hit;
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0f)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.CompareTag("Button"))
                {
                    obj.SetActive(false);

                    // Calibrate the position along the y-axis
                    Vector3 newPosition = environmentToCalibrate.position;
                    newPosition.y = controller.transform.position.y;
                    environmentToCalibrate.position = newPosition;

                    // Calculate the rotation to align z axis with camera forward
                    Vector3 forwardNoY = mainCamera.transform.forward;
                    forwardNoY.y = 0f; // Remove the y component
                    Quaternion targetRotation = Quaternion.LookRotation(forwardNoY);

                    // Apply rotation
                    environmentToCalibrate.rotation = targetRotation;
                }
            }
        }
    }
}
