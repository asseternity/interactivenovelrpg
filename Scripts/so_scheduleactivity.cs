using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ScheduleActivity", menuName = "SO_ScheduleActivity")]
public class so_scheduleactivity : ScriptableObject
{
    public string activityName;
    public List<so_dialoguebubble> activityProgression = new List<so_dialoguebubble>();
    public int currentIndex = 0;
}
