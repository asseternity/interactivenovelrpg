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
            $"Smarts: {player.Smarts} \r\nBrawn: {player.Brawn} \r\nAgility: {player.Agility} \r\nCharisma: {player.Charisma} \r\nPopularity: {player.Popularity} \r\nVibereading: {player.Vibereading}";
    }
}
