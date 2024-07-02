using System;
using UnityEngine;
using UnityEngine.UI;

public class DateKeeper : MonoBehaviour
{
    public DateTime day = new DateTime(2010, 9, 13);

    public void NextDay()
    {
        day = day.AddDays(1);
    }

    public string WriteDate()
    {
        string monthAbbreviation = day.ToString("MMM");
        return $"{day.DayOfWeek}\r\n {day.Day} {monthAbbreviation} {day.Year}";
    }

    public void Update()
    {
        Text text = gameObject.GetComponent<Text>();
        text.text = WriteDate();
    }
}
