using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TaskLogger : MonoBehaviour
{
    [SerializeField] private string fileName;
    private StreamWriter writer;

    // Start is called before the first frame update
    void Start()
    {
        // create a folder
        Directory.CreateDirectory(Application.persistentDataPath + "/AttentoVR_logs/");
        Debug.Log("Path = " + Application.persistentDataPath + "/AttentoVR_logs/");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteToFile(string text)
    {
        writer.WriteLine(text);
    }

    public void StartTaskLogging()
    {
        writer = new StreamWriter(Application.persistentDataPath + "/AttentoVR_logs/" + fileName + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt", true);
        writer.AutoFlush = true;
        writer.WriteLine("Date et heure: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    public void CloseFile()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }

    private void OnApplicationQuit()
    {
        CloseFile();
    }
}
