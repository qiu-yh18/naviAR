using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LoggingManager : MonoBehaviour
{
    public Transform player;
    public GameObject signArrow;
    public CalibrationManager calibrationManager;
    public GameObject[] maps;
    public Transform[] destinations;
    public int participantNumber = 0;
    public string condition = "A";
    public int map = 1;
    private Transform destination;
    private Vector3 playerToDest;
    private Vector3 previousPlayerPosition;
    private float destinationThreshold = 2f;
    private bool isStart = false;
    private bool isFileWritten = false;
    private float startTime = 0f;
    private float endTime = 0f;
    private float duration = 0f;
    private float distance = 0f;
    private int countHitWall = 0;
    private float durationOffTrack = 0f;
    private string ROOT;
    private string timeString = "2024-04-11_13:00:00";
    private string savepath = "";
    private StreamWriter writer;
    // Start is called before the first frame update
    void Start()
    {
        ROOT = Application.persistentDataPath;
        timeString = System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
        savepath = Path.Combine(ROOT, participantNumber + "_" + condition + "_" + map + "_" + timeString + ".csv");
        writer = new StreamWriter(savepath, true);
        string line = "Participant number, Timestamp, Relative Position x, Relative Position y, Hit";
        writer.WriteLine(line);
        previousPlayerPosition = player.position;
        if(map == 1){
            destination = destinations[0];
        }
        else if(map == 2){
            destination = destinations[1];
        }
        else if(map == 3){
            destination = destinations[2];
        }
        else{
            destination = destinations[3];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFileWritten && !isStart && calibrationManager.isStartButtonActivated){
            isStart = true;
            startTime = Time.time;
        }
        playerToDest = player.position - destination.position;
        Vector3 playerToDestXZ = new Vector3(playerToDest.x, 0f, playerToDest.z);
        if(isStart){
            distance += new Vector3(player.position.x-previousPlayerPosition.x, 0f, player.position.z-previousPlayerPosition.z).magnitude;
            previousPlayerPosition = player.position;
            // if(){

            // }
            if(playerToDestXZ.magnitude < destinationThreshold){
                isStart = false;
                endTime = Time.time;
                duration = endTime - startTime;
                string line = "1," + duration.ToString("0.000") + "," + distance.ToString("0.000") + "," + countHitWall.ToString();
                writer.WriteLine(line);
                writer.Close();
                signArrow.SetActive(false);
                isFileWritten = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Building" && isStart){
            countHitWall++;
        }
    }
}
