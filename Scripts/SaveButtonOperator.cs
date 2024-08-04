using System.IO;
using UnityEngine;

public class SaveButtonOperator : MonoBehaviour
{
    public so_playerstats currentPlayerStats;
    public so_scheduleactivity scheduleActivity1;
    public GameObject BubbleSpawner;
    public GameObject MainMenuContainer;

    public void SaveGame()
    {
        // overall save path
        string savePath = Application.persistentDataPath + "/save1";
        // Ensure directory exists
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        // save so_playerStats
        string playerStats_path = savePath + "/playerStats.json";
        string playerStats_json = JsonUtility.ToJson(currentPlayerStats);
        File.WriteAllText(playerStats_path, playerStats_json);
        // save so_scheduleActivity
        string activity1_path = savePath + "/activity1.json";
        string activity1_json = JsonUtility.ToJson(scheduleActivity1);
        File.WriteAllText(activity1_path, activity1_json);
        // save current bubble
        BubbleSpawner bubbleScript = BubbleSpawner.GetComponent<BubbleSpawner>();
        // grab and save last shown bubble
        so_dialoguebubble lastBubble = bubbleScript.lastShownBubble;
        string bubble_path = savePath + "/bubble.json";
        string bubble_json = JsonUtility.ToJson(lastBubble);
        File.WriteAllText(bubble_path, bubble_json);
        // grab and save current bubble number
        int currentBubbleNumber = bubbleScript.currentBubble;
        string bubbleNumber_path = savePath + "/bubbleNumber.txt";
        File.WriteAllText(bubbleNumber_path, currentBubbleNumber.ToString());
        // inform player
        Debug.Log("Data saved in: " + savePath);
    }

    public void LoadGame()
    {
        // get all paths
        string loadPath = Application.persistentDataPath + "/save1";
        string playerStats_loadPath = loadPath + "/playerStats.json";
        string activity1_loadPath = loadPath + "/activity1.json";
        string bubble_loadPath = loadPath + "/bubble.json";
        string bubbleNumber_loadPath = loadPath + "/bubbleNumber.txt";
        // check if all files exist
        if (File.Exists(playerStats_loadPath) && File.Exists(activity1_loadPath))
        {
            // load so_playerStats
            string playerStats_json = File.ReadAllText(playerStats_loadPath);
            JsonUtility.FromJsonOverwrite(playerStats_json, currentPlayerStats);
            // load so_scheduleActivity
            string activity1_json = File.ReadAllText(activity1_loadPath);
            JsonUtility.FromJsonOverwrite(activity1_json, scheduleActivity1);
            Debug.Log("All necessary data is present at: " + loadPath);
            // load current bubble
            string bubble_json = File.ReadAllText(bubble_loadPath);
            int bubbleNumber = int.Parse(File.ReadAllText(bubbleNumber_loadPath));
            BubbleSpawner bubbleScript = BubbleSpawner.GetComponent<BubbleSpawner>();
            JsonUtility.FromJsonOverwrite(bubble_json, bubbleScript.startingDialogue);
            bubbleScript.currentBubble = bubbleNumber;
            // start game
            MainMenuOperator mmScript = MainMenuContainer.GetComponent<MainMenuOperator>();
            mmScript.StartNewGame();
        }
        else
        {
            Debug.Log("Invalid data at: " + loadPath);
        }
    }
}

// to fix: add schedules to save system
