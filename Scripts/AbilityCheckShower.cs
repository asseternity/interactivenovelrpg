using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCheckShower : MonoBehaviour
{
    public GameObject rollDivExample;
    public Sprite result1;
    public Sprite result2;
    public Sprite result3;
    public Sprite result4;
    public Sprite result5;
    public Sprite result6;
    public Sprite result7;
    public Sprite result8;
    public Sprite result9;
    public Sprite result10;
    public Sprite result11;
    public Sprite result12;
    public Sprite result13;
    public Sprite result14;
    public Sprite result15;
    public Sprite result16;
    public Sprite result17;
    public Sprite result18;
    public Sprite result19;
    public Sprite result20;
    public so_playerstats player;
    public GameObject canvas;

    public void ShowAbilityCheckWindow(so_abilitycheck abilitycheck, int diceRoll)
    {
        GameObject rollDivInstance = Instantiate(
            rollDivExample,
            new Vector2(
                rollDivExample.transform.position.x,
                rollDivExample.transform.position.y - 800
            ),
            quaternion.identity
        );
        rollDivInstance.transform.SetParent(canvas.transform);
        GameObject frame = rollDivInstance.transform.Find("Frame").gameObject;
        Text rollText = frame.GetComponentInChildren<Text>();
        Image rollImage = frame.GetComponentInChildren<Image>();
        rollText.text =
            $"You rolled a {diceRoll}\r\nYour {abilitycheck.ability} is +{player.Vibereading}\r\n\r\nTotal result: {diceRoll + player.Vibereading}.";
    }
}
