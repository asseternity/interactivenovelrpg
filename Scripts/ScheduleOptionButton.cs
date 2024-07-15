using UnityEngine;
using UnityEngine.UI;

public class ScheduleOptionButton : MonoBehaviour
{
    public so_scheduleactivity buttonActivity;
    public GameObject bubbleSpawner;

    public void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => AddActivity());
    }

    public so_scheduleactivity ProvideActivity()
    {
        return buttonActivity;
    }

    public void AddActivity()
    {
        bubbleSpawner = GameObject.Find("/BubbleSpawner").gameObject;
        Debug.Log(bubbleSpawner);
        BubbleSpawner bubbleScript = bubbleSpawner.GetComponent<BubbleSpawner>();
        bubbleScript.scheduleEventOrder.Push(
            buttonActivity.activityProgression[buttonActivity.currentIndex]
        );
        foreach (so_dialoguebubble bubble in bubbleScript.scheduleEventOrder)
        {
            Debug.Log(bubble);
        }
    }
}
