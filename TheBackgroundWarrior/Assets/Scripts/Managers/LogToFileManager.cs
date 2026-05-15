using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Redirects all Debug.Log output to a file, including errors and warnings.
/// Attach this to any GameObject in your first scene.
/// </summary>
public class LogToFileManager : MonoBehaviour
{
    [SerializeField] bool isEnabled;

    private static string logFilePath;
    private static StreamWriter logWriter;

    public static LogToFileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!isEnabled) return;

        // Ensure it persists between scene loads
        DontDestroyOnLoad(gameObject);

        // Create a folder for logs if it doesn't exist
        string logDir = Path.Combine(Application.persistentDataPath, "Logs");
        Directory.CreateDirectory(logDir);

        // Create a new file with timestamp
        logFilePath = Path.Combine(logDir, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
        logWriter = new StreamWriter(logFilePath, false);
        logWriter.AutoFlush = true;

        // Subscribe to log messages
        Application.logMessageReceived += HandleLog;

        Debug.Log($"[LogToFile] Logging started at: {logFilePath}");
    }

    private void OnDestroy()
    {
        if (Instance != this) return;

        Application.logMessageReceived -= HandleLog;
        if (logWriter != null)
        {
            logWriter.Flush();
            logWriter.Close();
            logWriter = null;
        }
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] [{type}] {logString}";
        if (type == LogType.Error || type == LogType.Exception)
            logEntry += $"\n{stackTrace}";

        logWriter.WriteLine(logEntry);
    }

    
}
