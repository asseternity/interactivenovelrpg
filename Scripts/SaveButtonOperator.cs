using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject CanvasSwitcher;

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
        // also grab the queue
        Queue<so_dialoguebubble> currentQueue = new Queue<so_dialoguebubble>();
        currentQueue = bubbleScript.scheduleEventOrder;
        string queue_path = savePath + "/queue.json";
        string queue_json = JsonUtility.ToJson(currentQueue);
        File.WriteAllText(queue_path, queue_json);
        // save scheduleEndBubble
        if (bubbleScript.scheduleEndBubble != null)
        {
            string SEB_path = savePath + "/SEB.json";
            string SEB_json = JsonUtility.ToJson(bubbleScript.scheduleEndBubble);
            File.WriteAllText(SEB_path, SEB_json);
        }
        // grab and save current bubble number
        int currentBubbleNumber = bubbleScript.currentBubble;
        string bubbleNumber_path = savePath + "/bubbleNumber.txt";
        File.WriteAllText(bubbleNumber_path, currentBubbleNumber.ToString());

        // change the text of the button
        Text saveSlotButtonText = saveSlotButton.GetComponentInChildren<Text>();
        int saveBubbleNumber;
        if (currentBubbleNumber != 0)
        {
            saveBubbleNumber = currentBubbleNumber - 1;
        }
        else
        {
            saveBubbleNumber = currentBubbleNumber;
        }
        string saveTitle = lastBubble.sentences[saveBubbleNumber];
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
        string queue_loadPath = loadPath + "/queue.json";
        // check if all files exist
        if (
            File.Exists(playerStats_loadPath)
            && File.Exists(activity1_loadPath)
            && File.Exists(bubble_loadPath)
            && File.Exists(bubbleNumber_loadPath)
        )
        {
            BubbleSpawner bubbleScript = BubbleSpawner.GetComponent<BubbleSpawner>();
            // load so_playerStats
            string playerStats_json = File.ReadAllText(playerStats_loadPath);
            JsonUtility.FromJsonOverwrite(playerStats_json, bubbleScript.player);
            // load so_scheduleActivity
            string activity1_json = File.ReadAllText(activity1_loadPath);
            JsonUtility.FromJsonOverwrite(activity1_json, scheduleActivity1);
            // ***[_] this will later go to the exact activity objects***
            // load current bubble
            string bubble_json = File.ReadAllText(bubble_loadPath);
            int bubbleNumber = int.Parse(File.ReadAllText(bubbleNumber_loadPath));
            JsonUtility.FromJsonOverwrite(bubble_json, bubbleScript.startingDialogue);
            bubbleScript.currentBubble = bubbleNumber;
            // load current queue
            string queue_json = File.ReadAllText(queue_loadPath);
            JsonUtility.FromJsonOverwrite(queue_json, bubbleScript.scheduleEventOrder);
            // load scheduleEndBubble
            string SEB_loadPath = loadPath + "/SEB.json";
            if (File.Exists(SEB_loadPath))
            {
                string SEB_json = File.ReadAllText(SEB_loadPath);
                JsonUtility.FromJsonOverwrite(SEB_json, bubbleScript.scheduleEndBubble);
            }
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
        CanvasSwitcher canvasSwitcherScript = CanvasSwitcher.GetComponent<CanvasSwitcher>();
        if (!canvasSwitcherScript.wasUsingSchedule)
        {
            PauseCanvas.SetActive(false);
            SaveSlotPickerCanvas.SetActive(true);
        }
    }

    public void MoveToLoadSlotSelection()
    {
        // remember which window was open (for back button)
        Canvas MainMenuUI = MainMenuCanvas.GetComponent<Canvas>();
        Canvas PauseUI = PauseCanvas.GetComponent<Canvas>();
        CanvasSwitcher canvasSwitcherScript = CanvasSwitcher.GetComponent<CanvasSwitcher>();

        if (MainMenuUI.isActiveAndEnabled)
        {
            canvasSwitcherScript.canvasOpenedBeforeLoadSlots = MainMenuUI;
        }
        else
        {
            canvasSwitcherScript.canvasOpenedBeforeLoadSlots = PauseUI;
        }
        // switch canvases
        Debug.Log("MoveToLoadSlotSelection() called!");
        MainMenuCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
        LoadSlotPickerCanvas.SetActive(true);
        // this needs to populate the slots with proper names
        string savePath = Application.persistentDataPath + "/save";
        for (int i = 1; i < 5; i++)
        {
            string eachSavePath = savePath + i.ToString();
            if (Directory.Exists(eachSavePath))
            {
                // searching for load slot text components
                Transform LoadSlotPickerContainerTransform = LoadSlotPickerCanvas.transform.Find(
                    "LoadSlotPickerContainer"
                );
                GameObject LoadSlotPickerContainer = LoadSlotPickerContainerTransform.gameObject;
                string eachLoadSlotButtonName = "LoadSlotButton" + i.ToString();
                Transform eachLoadSlotButtonTransform = LoadSlotPickerContainer.transform.Find(
                    eachLoadSlotButtonName
                );
                GameObject eachLoadSlotButton = eachLoadSlotButtonTransform.gameObject;
                Text buttonText = eachLoadSlotButton.GetComponentInChildren<Text>();
                // grab and store the bubble to grab the so_bubble object
                string eachBubbleLoadPath = eachSavePath + "/bubble.json";
                string bubble_json = File.ReadAllText(eachBubbleLoadPath);
                if (tempBubble == null)
                {
                    tempBubble = ScriptableObject.CreateInstance<so_dialoguebubble>();
                }
                JsonUtility.FromJsonOverwrite(bubble_json, tempBubble);
                // tempBubble = JsonUtility.FromJson<so_dialoguebubble>(bubble_json);
                // grab and store bubble number to store the particular sentence in the bubble
                string eachNumberLoadPath = eachSavePath + "/bubbleNumber.txt";
                int tempBubbleNumber = int.Parse(File.ReadAllText(eachNumberLoadPath));
                // change the text
                int saveBubbleNumber;
                if (tempBubbleNumber != 0)
                {
                    saveBubbleNumber = tempBubbleNumber - 1;
                }
                else
                {
                    saveBubbleNumber = tempBubbleNumber;
                }
                buttonText.text = tempBubble.sentences[saveBubbleNumber];
            }
        }
    }
}

// bug: load slots' text is filled for every slot, even if there's only 1 save data
// to fix: fill the boxes for OVERsaving
