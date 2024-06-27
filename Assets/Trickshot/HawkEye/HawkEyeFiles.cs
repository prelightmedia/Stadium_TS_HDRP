using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class HawkEyeFiles : MonoBehaviour
{
    //public string dir = "HawkEye/Data/joints/";

    public string dir = "HawkEye/Data/";
    //public string file = "*.football.samples.joints";

    //public string file = "2022_251164_5047371_######.football.samples";
    //public int start = 409965;
    //public int end = 410465;

    [TextArea(4, 5)]
    public string notes;


    private List<string> jointDataList = new List<string>();
    public bool isReady = false;

    public UnityEvent OnReady;


    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        LoadAllJointData();
    }

    private void LoadAllJointData()
    {
        string directoryPath = Path.Combine(Application.streamingAssetsPath, dir + "joints/");
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Directory does not exist: " + directoryPath);
            return;
        }

        string[] files = Directory.GetFiles(directoryPath, "*.football.samples.joints");
        foreach (string file in files)
        {
            string jsonContent = File.ReadAllText(file);
            jointDataList.Add(jsonContent);
        }

        OnReady?.Invoke();
        isReady = true;
    }

    public string GetJoint(int index)
    {
        if (index >= 0 && index < jointDataList.Count)
        {
            return jointDataList[index];
        }
        Debug.LogError("Index out of range: " + index);
        return null;
    }

    /*
    public string GetJoint(int index)
    {
        string directoryPath = Path.Combine(Application.dataPath, dir + "joints/");

        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Directory does not exist: " + directoryPath);
            return null;
        }

        // Get all files in the directory
        string[] files = Directory.GetFiles(directoryPath, "*.football.samples.joints");
        string jsonContent = File.ReadAllText(files[index]);
        return jsonContent;
    }
    */

    public string GetBall(int index)
    {
        string directoryPath = Path.Combine(Application.streamingAssetsPath, dir + "ball/");

        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Directory does not exist: " + directoryPath);
            return null;
        }

        // Get all files in the directory
        string[] files = Directory.GetFiles(directoryPath, "*.football.samples.ball");
        string jsonContent = File.ReadAllText(files[index]);
        return jsonContent;
    }
}
