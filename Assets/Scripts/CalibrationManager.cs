using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationManager : MonoBehaviour
{
    public GameObject controllerForCalibration; // left
    public GameObject controllerForTrigger; // right
    public GameObject environmentToCalibrate;
    public GameObject circleCenterToCalibrate;
    public GameObject buttonSetCircleCenter;
    public GameObject buttonStart;
    public Camera mainCamera;
    public GameObject turnSign;
    public float radius = 12f;
    public bool isStartButtonActivated = false;
    private bool isCircleCenterSet = false;
    private bool isCooldownActive = false; 
    private float cooldownDuration = 1.5f;
    private float cooldownTimer = 0f;
    private GameObject[] buildings;
    private Quaternion initialRotation;

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
        turnSign.SetActive(false);

        // Record the initial rotation of the environment relative to the user
        initialRotation = Quaternion.Inverse(mainCamera.transform.rotation) * environmentToCalibrate.transform.rotation;
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
                    if (!isCooldownActive)
                    {
                        // Calibration Stage 1: 
                        // Experimenter walks to the center, place left controller on the ground, click set circle center with right controller.
                        if (!isCircleCenterSet)
                        {
                            circleCenterToCalibrate.transform.position = controllerForCalibration.transform.position;
                            circleCenterToCalibrate.SetActive(true);
                            isCircleCenterSet = true;
                            isCooldownActive = true;
                            buttonSetCircleCenter.SetActive(false);
                            buttonStart.SetActive(true);

                            // Calibrate environment rotation
                            Quaternion targetRotation = Quaternion.LookRotation(
                                new Vector3(mainCamera.transform.forward.x, 0f, mainCamera.transform.forward.z), 
                                Vector3.up
                            ) * initialRotation;
                            environmentToCalibrate.transform.rotation = targetRotation;
                            
                            // Calibrate environment position
                            Vector3 newPosition = mainCamera.transform.position;
                            newPosition.y = controllerForCalibration.transform.position.y;
                            newPosition.z -= radius;
                            environmentToCalibrate.transform.position = newPosition;

                            // Enable buildings
                            environmentToCalibrate.SetActive(true);
                            foreach (GameObject building in buildings)
                            {
                                building.SetActive(true);
                            }
                        }
                        // Calibration Stage 2: 
                        // Experimenter picks up left controller, walks to starting point, gives headset and right controller to participant. 
                        // Participant clicks start game, give the controller back to experimenter.
                        else if (!isStartButtonActivated)
                        {
                            buttonStart.SetActive(false);
                            isStartButtonActivated = true;
                            turnSign.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
