// Calibration

// Step 1: Calibrate the center of the circle. 
// User walks to the center and place the left controller on the floor.
// Get the controller position (x,y,z).
// Move the circle center object to (x,y,z).

// Step 2: Calibrate the height of the environment.
// Move the environment to (camera.x,circleCenter.y,camera.z).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateManager : MonoBehaviour
{
    public GameObject controllerForCalibration; // left
    public GameObject controllerForTrigger; // right
    public GameObject environmentToCalibrate;
    public GameObject circleCenterToCalibrate;
    public Camera mainCamera;
    private GameObject[] buildings;
    private bool calibrationStarted = false;
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
        // The circle center follows the left controller. Fix the position when environment calibration starts.
        if(!calibrationStarted && controllerForCalibration){
            circleCenterToCalibrate.transform.position = controllerForCalibration.transform.position;
        }
        RaycastHit hit;
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0f)
        {
            if (Physics.Raycast(controllerForTrigger.transform.position, controllerForTrigger.transform.forward, out hit, 100))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.CompareTag("Button"))
                {
                    // Get left controller position as circle center position
                    // Vector3 circleCenterPosition = controllerForCalibration.transform.position;
                    // circleCenterToCalibrate.transform.position = new Vector3(circleCenterPosition.x, circleCenterPosition.y+5f, circleCenterPosition.z); 

                    calibrationStarted = true;

                    Vector3 newPosition = mainCamera.transform.position;
                    newPosition.y = circleCenterToCalibrate.transform.position.y;
                    environmentToCalibrate.transform.position = newPosition;

                    // // Calculate the rotation to align the x-axis with the camera's forward direction
                    // Vector3 targetDirection = mainCamera.transform.forward;
                    // targetDirection.y = 0f; // Ignore vertical component
                    // Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                    // // Apply the rotation to the environment
                    // environmentToCalibrate.transform.rotation = targetRotation;

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
