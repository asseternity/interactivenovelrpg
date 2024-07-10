using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ScheduleOptions", menuName = "SO_ScheduleOptions")]
public class so_scheduleoptions : ScriptableObject
{
    public List<so_scheduleactivity> selectableActivities = new List<so_scheduleactivity>();
}
