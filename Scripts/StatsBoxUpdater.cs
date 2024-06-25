using UnityEngine;
using UnityEngine.UI;

public class StatsBoxUpdater : MonoBehaviour
{
    public GameObject statsCanvas;
    public GameObject statsBox;
    public so_playerstats player;

    public void Start()
    {
        player.ResetStats();
    }

    public void Update()
    {
        GameObject statsBoxText = statsBox.transform.Find("StatsText").gameObject;
        Text statsBoxTextComponent = statsBoxText.GetComponent<Text>();
        statsBoxTextComponent.text =
            $"ST1: {player.ST1} \r\nST2: {player.ST2} \r\nST3: {player.ST3} \r\nST4: {player.ST4} \r\nST5: {player.ST5} \r\nST6: {player.ST6} \r\nST7: {player.ST7} \r\nST8: {player.ST8}";
    }
}
