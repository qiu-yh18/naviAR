using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LoggingManager : MonoBehaviour
{
    public Transform player;
    public Transform destination;
    public GameObject signArrow;
    public CalibrationManager calibrationManager;
    private Vector3 playerToDest;
    private float destinationThreshold = 2f;
    private bool isStart = false;
    private bool isFileWritten = false;
    private float startTime = 0f;
    private float endTime = 0f;
    private float duration = 0f;
    private string ROOT;
    private string subjectname = "2022-10-04_13:00:00";
    private string savepath = "";
    private StreamWriter writer;
    // Start is called before the first frame update
    void Start()
    {
        ROOT = Application.persistentDataPath;
        subjectname = System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
        savepath = Path.Combine(ROOT, subjectname + ".csv");
        writer = new StreamWriter(savepath, true);
        string line = "Participant number, Completion time";
        writer.WriteLine(line);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFileWritten && !isStart && calibrationManager.isStartButtonActivated){
            isStart = true;
            startTime = Time.time;
            signArrow.SetActive(false);
        }
        playerToDest = player.position - destination.position;
        Vector3 playerToDestXZ = new Vector3(playerToDest.x, 0f, playerToDest.z);
        if(isStart && playerToDestXZ.magnitude < destinationThreshold){
            isStart = false;
            endTime = Time.time;
            duration = endTime - startTime;
            string line = "1," + duration.ToString("0.000");
            writer.WriteLine(line);
            writer.Close();
            signArrow.SetActive(true);
            isFileWritten = true;
        }
    }
}
