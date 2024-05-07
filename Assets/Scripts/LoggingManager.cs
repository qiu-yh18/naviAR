using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;
using System.IO;

public class LoggingManager : MonoBehaviour
{
    public GameObject camera;
    public PlayerCollider player;
    public GameObject leftController;
    public GameObject rightController;
    public SignArrowManager signArrow;
    public AbsoluteArrow absoluteArrow;
    public CalibrationManager calibrationManager;
    public GameObject[] maps;
    public Transform[] startPoints;
    public Transform[] destinations;
    public GameObject turnSign;
    public TMP_Text participantNumberText;
    public GameObject endCube;
    public TMP_Text endText;
    public Material successMaterial;
    public Material failMaterial;
    public float timeLimit = 600f;
    public int participantNumber = 12;
    public string conditionNumber = "A";
    public int mapNumber = 1;
    private GameObject map;
    private Transform startPoint;
    private Transform destination;
    private Vector3 playerToDest;
    private float destinationThreshold = 1.5f;
    private bool isStart = false;
    private bool isFileWritten = false;
    private float startTime = 0f;
    private Vector3 previousPos;
    private string ROOT;
    private string timeString = "2024-04-11_13:00:00";
    private string savepath = "";
    private StreamWriter writer;
    private bool isCooldownActive = false; 
    private float cooldownDuration = 0.3f;
    private float cooldownTimer = 0f;
    private Transform highlights;

    // Start is called before the first frame update
    void Start()
    {
        participantNumberText.SetText(participantNumber.ToString());
        previousPos = player.transform.position;
        
        // Assign map and destination
        if(mapNumber == 1){
            map = maps[0];
            destination = destinations[0];
            startPoint = startPoints[0];
        }
        else if(mapNumber == 2){
            map = maps[1];
            destination = destinations[1];
            startPoint = startPoints[1];
        }
        else if(mapNumber == 3){
            map = maps[2];
            destination = destinations[2];
            startPoint = startPoints[2];
        }
        else{
            map = maps[3];
            destination = destinations[3];
            startPoint = startPoints[3];
        }
        foreach (GameObject m in maps)
        {
            m.SetActive(false);
        }
        absoluteArrow.destination = destination;
        
        calibrationManager.environmentToCalibrate = map;
        // Assign navigation methods
        if(conditionNumber == "A"){         // Turn sign
            turnSign.SetActive(true);
            absoluteArrow.gameObject.SetActive(false);
            foreach (Transform child in map.transform){
                if(child.tag == "Beacon"){ 
                    child.gameObject.SetActive(false);
                }
                else if(child.tag == "Highlight"){
                    child.gameObject.SetActive(true);
                    highlights = child.transform;
                    foreach (Transform highlight in highlights){
                        foreach (Transform canvas in highlight){
                            canvas.gameObject.SetActive(false);
                        }
                    }
                }
                else if(child.tag == "IntersectionArrows"){
                    child.gameObject.SetActive(false);
                }
            }
        }
        else if(conditionNumber == "B"){    // Road highlights
            turnSign.SetActive(false);
            absoluteArrow.gameObject.SetActive(false);
            foreach (Transform child in map.transform){
                if(child.tag == "Highlight"){
                    child.gameObject.SetActive(true);
                    highlights = child.transform;
                    foreach (Transform highlight in highlights){
                        foreach (Transform canvas in highlight){
                            canvas.gameObject.SetActive(false);
                        }
                    }
                }
                else if(child.tag == "Beacon"){
                    child.gameObject.SetActive(false);
                }
                else if(child.tag == "IntersectionArrows"){
                    child.gameObject.SetActive(true);
                }
            }
        }
        else if(conditionNumber == "C"){    // Absolute Arrow
            turnSign.SetActive(false);
            absoluteArrow.gameObject.SetActive(true);
            foreach (Transform child in map.transform){
                if(child.tag == "Highlight" || child.tag == "Beacon"){
                    child.gameObject.SetActive(false);
                }
            }
        }
        else{                               // Beacon
            turnSign.SetActive(false);
            absoluteArrow.gameObject.SetActive(false);
            foreach (Transform child in map.transform){
                if(child.tag == "Highlight"){
                    child.gameObject.SetActive(false);
                }
                if(child.tag == "Beacon"){
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

//     Condition	Method	        Name	        Map
//      A	        TBT Ego	        Turn signs	    1/2
//      B	        TBT Non-ego	    Road highlights	1/2
//      C	        ATCF Ego	    Absolute arrow	3/4
//      D	        ATCF Non-ego	Beacon	        3/4

    // Update is called once per frame
    void Update()
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
        if(!calibrationManager.isStartButtonActivated){
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0f){
                if (!isCooldownActive)
                {
                    participantNumber--;
                    participantNumberText.SetText(participantNumber.ToString());
                    isCooldownActive = true;
                }
            }
            if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0f){
                if (!isCooldownActive){
                    participantNumber++;
                    participantNumberText.SetText(participantNumber.ToString());
                    isCooldownActive = true;
                }
            }
            if(calibrationManager.isCircleCenterSet){
                participantNumberText.gameObject.SetActive(false);
            }
        }
        else if(!isFileWritten && !isStart){
            ROOT = Application.persistentDataPath;
            timeString = System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
            savepath = Path.Combine(ROOT, participantNumber + "_" + conditionNumber + "_" + mapNumber + "_" + timeString + ".csv");
            writer = new StreamWriter(savepath, true);
            string line = "Participant number, Timestamp, Relative Pos x, Relative Pos y, Relative Pos z, Env Pos x, Env Pos y, Env Pos z, Env Rot x, Env Rot y, Env Rot z, Real Pos x, Real Pos y, Real Pos z, Real Rot x, Real Rot y, Real Rot z, Dist to Dest, Real Speed, Hit";
            writer.WriteLine(line);
            participantNumberText.gameObject.SetActive(false);
            isStart = true;
            player.gameObject.SetActive(true);
            startTime = Time.time;
            signArrow.highlights = highlights.gameObject;
            leftController.SetActive(false);
            rightController.SetActive(false);
        }
        else if(isStart){
            Vector3 relativePlayerPosition = camera.transform.position - startPoint.position;
            playerToDest = player.transform.position - destination.position;
            Vector3 playerToDestXZ = new Vector3(playerToDest.x, 0f, playerToDest.z);
            float timeToNow = (Time.time - startTime);
            Vector3 displacement = player.transform.position - previousPos;
            previousPos = player.transform.position;
            float speed = new Vector3(displacement.x, 0f, displacement.z).magnitude/Time.deltaTime;
            string line = participantNumber.ToString() + "," 
                        + timeToNow.ToString("0.000") + "," 
                        + relativePlayerPosition.x.ToString("0.000") + "," 
                        + relativePlayerPosition.y.ToString("0.000") + "," 
                        + relativePlayerPosition.z.ToString("0.000") + ","
                        + map.transform.position.x.ToString("0.000") + "," 
                        + map.transform.position.y.ToString("0.000") + "," 
                        + map.transform.position.z.ToString("0.000") + ","
                        + map.transform.rotation.x.ToString("0.000") + "," 
                        + map.transform.rotation.y.ToString("0.000") + "," 
                        + map.transform.rotation.z.ToString("0.000") + ","
                        + camera.transform.position.x.ToString("0.000") + "," 
                        + camera.transform.position.y.ToString("0.000") + "," 
                        + camera.transform.position.z.ToString("0.000") + ","
                        + camera.transform.rotation.x.ToString("0.000") + "," 
                        + camera.transform.rotation.y.ToString("0.000") + "," 
                        + camera.transform.rotation.z.ToString("0.000") + ","
                        + playerToDestXZ.magnitude + ","
                        + speed.ToString("0.000") + ","
                        + (player.isHit ? 1 : 0);
            writer.WriteLine(line);
            // Time limit
            if(timeToNow > timeLimit){
                isStart = false;
                // player.gameObject.SetActive(false);
                writer.Close();
                // endText.SetText("Time is up! You did not reach the destination. Please take off the headset.");
                // Renderer renderer = endCube.GetComponent<Renderer>();
                // renderer.material = failMaterial;
                // endCube.SetActive(true);
                // signArrow.gameObject.SetActive(false);
                isFileWritten = true;
            }
            // End condition
            if(playerToDestXZ.magnitude < destinationThreshold){
                isStart = false;
                player.gameObject.SetActive(false);
                endText.SetText("Congratulations! You have reached the destination! Please take off the headset.");
                Renderer renderer = endCube.GetComponent<Renderer>();
                renderer.material = successMaterial;
                endCube.SetActive(true);
                writer.Close();
                signArrow.gameObject.SetActive(false);
                endCube.SetActive(true);
                isFileWritten = true;
            }
        }
    }
}
