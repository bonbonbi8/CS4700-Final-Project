using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class InGameConsole : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text logText;

    [Header("Settings")]
    public int maxLines = 20;               // Number of lines shown in the log
    private readonly Queue<string> lines = new Queue<string>();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Optional type prefix
        string prefix = type switch
        {
            LogType.Warning => "[W] ",
            LogType.Error => "[ERR] ",
            LogType.Exception => "[EXC] ",
            _ => ""
        };

        // Add line to queue
        lines.Enqueue(prefix + logString);

        // Trim oldest lines
        while (lines.Count > maxLines)
            lines.Dequeue();

        // Rebuild text box
        logText.text = string.Join("\n", lines);
    }
}
