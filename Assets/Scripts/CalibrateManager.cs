using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateManager : MonoBehaviour
{
    public GameObject controllerForCalibration; // left
    public GameObject controllerForTrigger; // right
    public GameObject environmentToCalibrate;
    public GameObject circleCenterToCalibrate;
    public GameObject buttonSetCircleCenter;
    public GameObject buttonStart;
    public Camera mainCamera;
    private GameObject[] buildings;
    private bool isCircleCenterSet = false;
    public bool isStartButtonActivated = false;
    private bool isCooldownActive = false; 
    private float cooldownDuration = 1.5f;
    private float cooldownTimer = 0f;

    private void Start()
    {
        buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in buildings)
        {
            building.SetActive(false);
        }
        circleCenterToCalibrate.SetActive(false);
        buttonSetCircleCenter.SetActive(true);
        buttonStart.SetActive(false);
    }

    private void Update()
    {
        if (isCooldownActive)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                isCooldownActive = false;
                cooldownTimer = 0f;
            }
        }

        RaycastHit hit;
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0f)
        {
            if (Physics.Raycast(controllerForTrigger.transform.position, controllerForTrigger.transform.forward, out hit, 100))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.CompareTag("Button"))
                {
                    // Only proceed if the cooldown is not active
                    if (!isCooldownActive)
                    {
                        // Stage 1: walk to the center, place left controller on the ground, click set circle center
                        if (!isCircleCenterSet)
                        {
                            circleCenterToCalibrate.transform.position = controllerForCalibration.transform.position;
                            circleCenterToCalibrate.SetActive(true);
                            isCircleCenterSet = true;
                            buttonSetCircleCenter.SetActive(false);
                            buttonStart.SetActive(true);
                            isCooldownActive = true;
                        }
                        // Stage 2: pick up left controller, walk to starting point, click start game
                        else if (!isStartButtonActivated)
                        {
                            // Calibrate environment position
                            Vector3 newPosition = mainCamera.transform.position;
                            newPosition.y = circleCenterToCalibrate.transform.position.y;
                            environmentToCalibrate.transform.position = newPosition;
                            // Calibrate environment rotation
                            // Vector3 targetDirection = mainCamera.transform.forward;
                            // targetDirection.y = 0f;
                            // Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                            // environmentToCalibrate.transform.rotation = targetRotation;

                            // Directly set environment rotation to main camera rotation
                            // environmentToCalibrate.transform.rotation = mainCamera.transform.rotation;

                            // Get the Euler angles of the main camera's rotation
                            // Vector3 cameraEulerAngles = mainCamera.transform.rotation.eulerAngles;

                            // // Zero out the y-component
                            // cameraEulerAngles.y = 0f;

                            // // Construct a new Quaternion with the modified Euler angles
                            // Quaternion newXZRotation = Quaternion.Euler(cameraEulerAngles);

                            // // Apply the rotation to the environment
                            // environmentToCalibrate.transform.rotation = newXZRotation;


                            // Enable buildings
                            foreach (GameObject building in buildings)
                            {
                                building.SetActive(true);
                            }
                            buttonSetCircleCenter.SetActive(false);
                            buttonStart.SetActive(false);
                            isStartButtonActivated = true;
                            isCooldownActive = true;
                        }
                    }
                }
            }
        }
    }
}
