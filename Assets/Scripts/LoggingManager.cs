using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
    public int participantNumber = 0;
    public string conditionNumber = "A";
    public int mapNumber = 1;
    public GameObject endCube;
    public float timeLimit = 600f;
    private GameObject map;
    private Transform destination;
    private Vector3 playerToDest;
    private float destinationThreshold = 2f;
    private bool isStart = false;
    private bool isFileWritten = false;
    private float startTime = 0f;
    // private float durationOffTrack = 0f;
    private string ROOT;
    private string timeString = "2024-04-11_13:00:00";
    private string savepath = "";
    private StreamWriter writer;

    // Start is called before the first frame update
    void Start()
    {
        player.gameObject.SetActive(false);
        ROOT = Application.persistentDataPath;
        timeString = System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
        savepath = Path.Combine(ROOT, participantNumber + "_" + conditionNumber + "_" + mapNumber + "_" + timeString + ".csv");
        writer = new StreamWriter(savepath, true);
        string line = "Participant number, Timestamp, Relative Position x, Relative Position z, Hit";
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
                // if(child.tag == "Highlight" || child.tag == "Beacon"){ // Note: leave highlight active for test
                if(child.tag == "Beacon"){
                    child.gameObject.SetActive(false);
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
            string line = participantNumber.ToString() + "," 
                        + timeToNow.ToString("0.000") + "," 
                        + relativePlayerPosition.x.ToString("0.000") + "," 
                        + relativePlayerPosition.z.ToString("0.000") + ","
                        + (player.isHit ? 1 : 0);
            writer.WriteLine(line);
            if(timeToNow > timeLimit){
                isStart = false;
                player.gameObject.SetActive(false);
                writer.Close();
                endCube.SetActive(true);
                signArrow.SetActive(false);
                isFileWritten = true;
            }
            // End condition
            else if(playerToDestXZ.magnitude < destinationThreshold){
                isStart = false;
                player.gameObject.SetActive(false);
                writer.Close();
                signArrow.SetActive(false);
                endCube.SetActive(true);
                isFileWritten = true;
            }
        }
    }
}
