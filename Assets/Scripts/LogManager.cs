using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogManager : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    void Start()
    {
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Append the log information to your debugText
        debugText.text += logString + "\n";
    }
}
