using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateManager : MonoBehaviour
{
    public GameObject controllerForCalibration;
    public GameObject controllerForTrigger;
    public Transform environmentToCalibrate;
    public Camera mainCamera;
    private void Update()
    {
        RaycastHit hit;
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0f)
        {
            if (Physics.Raycast(controllerForTrigger.transform.position, controllerForTrigger.transform.forward, out hit, 100))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.CompareTag("Button"))
                {
                    // Calibrate environment height
                    Vector3 newPosition = environmentToCalibrate.position;
                    newPosition.y = controllerForCalibration.transform.position.y;
                    environmentToCalibrate.position = newPosition;

                    // // Calibrate environment rotation
                    // Vector3 forwardNoY = mainCamera.transform.forward;
                    // forwardNoY.y = 0f;
                    // Quaternion targetRotation = Quaternion.LookRotation(forwardNoY);
                    // environmentToCalibrate.rotation = targetRotation;

                    obj.SetActive(false);
                }
            }
        }
    }
}
