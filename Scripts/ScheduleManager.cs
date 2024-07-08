using System;
using System.Collections.Generic;
using UnityEngine;

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
}
