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
        bubbleScript.scheduleEventOrder.Enqueue(
            buttonActivity.activityProgression[buttonActivity.currentIndex]
        );
        Debug.Log(bubbleScript.scheduleEventOrder.Count);

        // change the activeBox language
        GameObject scheduleUI = GameObject.Find("ScheduleContainer");
        ScheduleManager scheduleScript = scheduleUI.GetComponent<ScheduleManager>();
        Text activeBoxText = scheduleScript.activeBox.transform.Find("Choice").GetComponent<Text>();
        activeBoxText.text = buttonActivity.activityName;
    }
}
