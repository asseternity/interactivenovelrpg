using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    public List<string> logs = new List<string>();

    public void AddLog(string log)
    {
        logs.Add(log);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(
            rectTransform.sizeDelta.x,
            rectTransform.sizeDelta.y + 60
        );
    }

    public string DisplayLogs()
    {
        string result = "";
        foreach (var log in logs)
        {
            result = result + "\r\n \r\n" + log;
        }
        return result;
    }

    public void Update()
    {
        Text textComponent = gameObject.GetComponent<Text>();
        textComponent.text = DisplayLogs();
    }
}
