using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonOperator : MonoBehaviour
{
    public so_playerstats currentPlayerStats;
    public so_scheduleactivity scheduleActivity1;
    public GameObject BubbleSpawner;
    public GameObject MainMenuContainer;
    public GameObject PauseCanvas;
    public GameObject MainMenuCanvas;
    public GameObject SaveSlotPickerCanvas;
    public GameObject LoadSlotPickerCanvas;
    public so_dialoguebubble tempBubble;

    public void SaveGame(int slot, GameObject saveSlotButton)
    {
        // overall save path
        string savePath = Application.persistentDataPath + "/save" + slot.ToString();
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

        // change the text of the button
        Text saveSlotButtonText = saveSlotButton.GetComponentInChildren<Text>();
        string saveTitle = lastBubble.sentences[currentBubbleNumber].Substring(0, 12) + "...";
        saveSlotButtonText.text = saveTitle;

        // inform player
        Debug.Log("Data saved in: " + savePath);
    }

    public void LoadGame(int slot)
    {
        // get all paths
        string loadPath = Application.persistentDataPath + "/save" + slot.ToString();
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

    public void MoveToSaveSlotSelection()
    {
        PauseCanvas.SetActive(false);
        SaveSlotPickerCanvas.SetActive(true);
    }

    public void MoveToLoadSlotSelection()
    {
        Debug.Log("MoveToLoadSlotSelection() called!");

        MainMenuCanvas.SetActive(false);
        LoadSlotPickerCanvas.SetActive(true);

        // this needs to populate the slots with proper names
        string savePath = Application.persistentDataPath + "/save";

        for (int i = 1; i < 5; i++)
        {
            string eachSavePath = savePath + i.ToString();
            if (Directory.Exists(eachSavePath))
            {
                // testing
                Debug.Log("Found a save file!");
                // searching for data
                string eachLoadSlotButtonName = "LoadSlotButton" + i.ToString();
                Transform LoadSlotPickerContainerTransform = LoadSlotPickerCanvas.transform.Find(
                    "LoadSlotPickerContainer"
                );
                GameObject LoadSlotPickerContainer = LoadSlotPickerContainerTransform.gameObject;
                Transform eachLoadSlotButtonTransform = LoadSlotPickerContainer.transform.Find(
                    eachLoadSlotButtonName
                );
                GameObject eachLoadSlotButton = eachLoadSlotButtonTransform.gameObject;
                Text buttonText = eachLoadSlotButton.GetComponent<Text>();
                // grab and store the bubble to grab the so_bubble object
                string eachBubbleLoadPath = eachSavePath + "/bubble.json";
                string bubble_json = File.ReadAllText(eachBubbleLoadPath);
                // *************this part bugs out********************
                JsonUtility.FromJsonOverwrite(bubble_json, tempBubble);
                tempBubble = JsonUtility.FromJson<so_dialoguebubble>(bubble_json);
                // grab and store bubble number to store the particular sentence in the bubble
                string eachNumberLoadPath = eachSavePath + "/bubbleNumber.txt";
                int tempBubbleNumber = int.Parse(File.ReadAllText(eachNumberLoadPath));
                // change the text
                buttonText.text = tempBubble.sentences[tempBubbleNumber];
            }
        }
    }
}

// to fix: add schedules to save system
