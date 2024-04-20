using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;
using System.IO;

public class LoggingManager : MonoBehaviour
{
    public PlayerCollider player;
    public GameObject signArrow;
    public CalibrationManager calibrationManager;
    public GameObject[] maps;
    public Transform[] destinations;
    public GameObject turnSign;
    public GameObject absoluteArrow;
    public GameObject endCube;
    public TMP_Text endText;
    public Material successMaterial;
    public Material failMaterial;
    public float timeLimit = 600f;
    public int participantNumber = 0;
    public string conditionNumber = "A";
    public int mapNumber = 1;
    private GameObject map;
    private Transform destination;
    private Vector3 playerToDest;
    private float destinationThreshold = 2f;
    private bool isStart = false;
    private bool isFileWritten = false;
    private float startTime = 0f;
    private Vector3 previousPos;
    private string ROOT;
    private string timeString = "2024-04-11_13:00:00";
    private string savepath = "";
    private StreamWriter writer;

    // Start is called before the first frame update
    void Start()
    {
        // player.gameObject.SetActive(false);
        previousPos = player.transform.position;
        ROOT = Application.persistentDataPath;
        timeString = System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
        savepath = Path.Combine(ROOT, participantNumber + "_" + conditionNumber + "_" + mapNumber + "_" + timeString + ".csv");
        writer = new StreamWriter(savepath, true);
        string line = "Participant number, Timestamp, Relative Position x, Relative Position z, Real Speed, Hit";
        writer.WriteLine(line);
        // Assign map and destination
        if(mapNumber == 1){
            map = maps[0];
            destination = destinations[0];
        }
        else if(mapNumber == 2){
            map = maps[1];
            destination = destinations[1];
        }
        else if(mapNumber == 3){
            map = maps[2];
            destination = destinations[2];
        }
        else{
            map = maps[3];
            destination = destinations[3];
        }
        foreach (GameObject m in maps)
        {
            m.SetActive(false);
        }
        map.SetActive(true);
        calibrationManager.environmentToCalibrate = map;
        // Assign navigation methods
        if(conditionNumber == "A"){         // Turn sign
            turnSign.SetActive(true);
            absoluteArrow.SetActive(false);
            foreach (Transform child in map.transform){
                if(child.tag == "Beacon"){ // Note: leave highlight active for test
                    child.gameObject.SetActive(false);
                }
                else if(child.tag == "Highlight"){
                    Transform highlights = child.transform;
                    foreach (Transform highlight in highlights){
                        foreach (Transform canvas in highlight){
                            canvas.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else if(conditionNumber == "B"){    // Road highlights
            turnSign.SetActive(false);
            absoluteArrow.SetActive(false);
            foreach (Transform child in map.transform){
                if(child.tag == "Highlight"){
                    child.gameObject.SetActive(true);
                }
                if(child.tag == "Beacon"){
                    child.gameObject.SetActive(false);
                }
            }
        }
        else if(conditionNumber == "C"){    // Absolute Arrow
            turnSign.SetActive(false);
            absoluteArrow.SetActive(true);
            foreach (Transform child in map.transform){
                if(child.tag == "Highlight" || child.tag == "Beacon"){
                    child.gameObject.SetActive(false);
                }
            }
        }
        else{                               // Beacon
            turnSign.SetActive(false);
            absoluteArrow.SetActive(false);
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
        if(!isFileWritten && !isStart && calibrationManager.isStartButtonActivated){
            isStart = true;
            player.gameObject.SetActive(true);
            startTime = Time.time;
        }
        if(isStart){
            Vector3 relativePlayerPosition = player.transform.position - map.transform.position;
            playerToDest = player.transform.position - destination.position;
            Vector3 playerToDestXZ = new Vector3(playerToDest.x, 0f, playerToDest.z);
            float timeToNow = (Time.time - startTime);
            Vector3 displacement = player.transform.position - previousPos;
            previousPos = player.transform.position;
            float speed = new Vector3(displacement.x, 0f, displacement.z).magnitude/Time.deltaTime;
            string line = participantNumber.ToString() + "," 
                        + timeToNow.ToString("0.000") + "," 
                        + relativePlayerPosition.x.ToString("0.000") + "," 
                        + relativePlayerPosition.z.ToString("0.000") + ","
                        + speed.ToString("0.000") + ","
                        + (player.isHit ? 1 : 0);
            writer.WriteLine(line);
            if(timeToNow > timeLimit){
                isStart = false;
                player.gameObject.SetActive(false);
                writer.Close();
                endText.SetText("Time is up! You did not reach the destination. Please take off the headset.");
                Renderer renderer = endCube.GetComponent<Renderer>();
                renderer.material = failMaterial;
                endCube.SetActive(true);
                signArrow.SetActive(false);
                isFileWritten = true;
            }
            // End condition
            else if(playerToDestXZ.magnitude < destinationThreshold){
                isStart = false;
                player.gameObject.SetActive(false);
                endText.SetText("Congratulations! You have reached the destination! Please take off the headset.");
                Renderer renderer = endCube.GetComponent<Renderer>();
                renderer.material = successMaterial;
                endCube.SetActive(true);
                writer.Close();
                signArrow.SetActive(false);
                endCube.SetActive(true);
                isFileWritten = true;
            }
        }
    }
}
