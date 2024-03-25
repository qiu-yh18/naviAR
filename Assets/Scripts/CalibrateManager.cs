// Calibration

// Step 1: Calibrate the center of the circle. 
// User walks to the center and place the left controller on the floor.
// Get the controller position (x,y,z).
// Move the circle center object to (x,y,z).

// Step 2: Calibrate the height of the environment.
// Move the environment to (-,y,-).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateManager : MonoBehaviour
{
    public GameObject controllerForCalibration;
    public GameObject controllerForTrigger;
    public GameObject environmentToCalibrate;
    public GameObject circleCenterToCalibrate;
    // public Camera mainCamera;
    private GameObject[] buildings;
    private void Start()
    {
        buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in buildings)
        {
            building.SetActive(false);
        }
        circleCenterToCalibrate.SetActive(false);
    }
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
                    // Get left controller position as circle center position
                    Vector3 circleCenterPosition = controllerForCalibration.transform.position;
                    circleCenterToCalibrate.transform.position = circleCenterPosition; 

                    Vector3 newPosition = environmentToCalibrate.transform.position;
                    newPosition.y = circleCenterPosition.y;
                    environmentToCalibrate.transform.position = newPosition;

                    // // Calibrate environment rotation
                    // Vector3 forwardNoY = mainCamera.transform.forward;
                    // forwardNoY.y = 0f;
                    // Quaternion targetRotation = Quaternion.LookRotation(forwardNoY);
                    // environmentToCalibrate.rotation = targetRotation;

                    obj.SetActive(false);
                    circleCenterToCalibrate.SetActive(true);
                    foreach (GameObject building in buildings)
                    {
                        building.SetActive(true);
                    }
                }
            }
        }
    }
}
