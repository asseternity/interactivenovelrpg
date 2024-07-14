using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleManager : MonoBehaviour
{
    public GameObject mondayBox;
    public GameObject tuesdayBox;
    public GameObject wednesdayBox;
    public GameObject thursdayBox;
    public GameObject fridayBox;
    public GameObject saturdayBox;
    public GameObject sundayBox;
    public GameObject activeBox;
    public List<GameObject> weekObjects = new List<GameObject>();
    public so_scheduleoptions testActivityOptions;

    public void Awake()
    {
        activeBox = mondayBox;
        weekObjects.Add(mondayBox);
        weekObjects.Add(tuesdayBox);
        weekObjects.Add(wednesdayBox);
        weekObjects.Add(thursdayBox);
        weekObjects.Add(fridayBox);
        weekObjects.Add(saturdayBox);
        weekObjects.Add(sundayBox);
        FillOptions(testActivityOptions);
    }

    public void Update()
    {
        foreach (GameObject obj in weekObjects)
        {
            obj.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        activeBox.gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
    }

    public void moveToNextDay()
    {
        Predicate<GameObject> match = x => x == activeBox;
        int index = weekObjects.FindIndex(match);
        if (index < 6)
        {
            activeBox = weekObjects[index + 1];
        }
        else
        {
            activeBox = weekObjects[0];
        }
    }

    public void FillOptions(so_scheduleoptions currentList)
    {
        // get the buttons
        GameObject buttonsContainer = gameObject
            .transform.Find("ScheduleOptionsContainer")
            .gameObject;
        GameObject optionButton0 = buttonsContainer
            .gameObject.transform.Find("optionButton")
            .gameObject;
        GameObject optionButton1 = buttonsContainer
            .gameObject.transform.Find("optionButton (1)")
            .gameObject;
        GameObject optionButton2 = buttonsContainer
            .gameObject.transform.Find("optionButton (2)")
            .gameObject;
        GameObject optionButton3 = buttonsContainer
            .gameObject.transform.Find("optionButton (3)")
            .gameObject;
        GameObject optionButton4 = buttonsContainer
            .gameObject.transform.Find("optionButton (4)")
            .gameObject;
        GameObject optionButton5 = buttonsContainer
            .gameObject.transform.Find("optionButton (5)")
            .gameObject;

        List<GameObject> optionButtons = new List<GameObject>();
        optionButtons.Add(optionButton0);
        optionButtons.Add(optionButton1);
        optionButtons.Add(optionButton2);
        optionButtons.Add(optionButton3);
        optionButtons.Add(optionButton4);
        optionButtons.Add(optionButton5);

        // assign scripts to buttons
        foreach (GameObject optionButton in optionButtons)
        {
            optionButton.AddComponent<ScheduleOptionButton>();
        }

        // remove all event listeners from buttons
        for (int i = 0; i < optionButtons.Count; i++)
        {
            Button optionButtonButton = optionButtons[i].GetComponent<Button>();
            optionButtonButton.onClick.RemoveAllListeners();
        }

        // get the options (regardless of length)
        for (int i = 0; i < currentList.selectableActivities.Count; i++)
        {
            // assign the activity to an inner script
            ScheduleOptionButton optionButtonScript = optionButtons[i]
                .GetComponent<ScheduleOptionButton>();
            optionButtonScript.buttonActivity = currentList.selectableActivities[i];
            Button optionButtonButton = optionButtons[i].GetComponent<Button>();
            optionButtonButton.onClick.AddListener(() => optionButtonScript.ProvideActivity());

            // change the text of the buttons
            Text buttonText = optionButtons[i].GetComponentInChildren<Text>();
            buttonText.text = currentList.selectableActivities[i].activityName;
        }

        // implementation:
        //      - new SO: so_scheduleActivity: (1) name, (2) list of so_dialogueBubble progression, (3) currentIndex
        //      - new SO: so_scheduleOptions: keeps a List of next SOs
        // concept: go through each option and assign one such object to it
    }

    public void AssignOption(string someParam)
    {
        // take the active day
        // and put into it the option assigned to the button
    }

    // [v] 1) add a new field to so_dialogueBubble: bool fromSchedule
    // [v] 2) add a new field to so_dialogueBubble: bool startSchedule
    // [v] 3) add a new so_scheduleProgress, with all activities and int levels
    // [_] 4) edit bubbleScript - have a stack of next so_dialogueBubbles - activated when (startSchedule)
    // [_] 5) populate the stack as player clicks on activities with the current level of so_schedule progress
    // [_] 6) fill the above methods
}
