using System.IO;
using UnityEngine;

public class SaveButtonOperator : MonoBehaviour
{
    void Start() { }

    public void SaveGame()
    {
        string savePath = Application.persistentDataPath + "/save1";
        string path1 = savePath + "/data1.txt";
        File.WriteAllText(path1, "123");
        Debug.Log("Data saved to: " + savePath);
    }

    public void LoadGame()
    {
        string savePath = Application.persistentDataPath + "/save1";
        string path1 = savePath + "/data1.txt";
        if (File.Exists(path1))
        {
            // imported vars setting = File.ReadAllText(path);
            Debug.Log("Data loaded from: " + path1);
        }
        else
        {
            Debug.Log("No save dada found at: " + path1);
        }
    }
}

// to fix: add schedule progress to save system
