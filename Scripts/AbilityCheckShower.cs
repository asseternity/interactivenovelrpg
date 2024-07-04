using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCheckShower : MonoBehaviour
{
    public GameObject rollDivExample;
    public GameObject clickDivExample;
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
    public bool windowOpen = false;

    public void ShowRollWindow(so_abilitycheck abilitycheck, int diceRoll)
    {
        windowOpen = true;
        GameObject clickDivInstance = Instantiate(
            clickDivExample,
            new Vector2(
                clickDivExample.transform.position.x,
                clickDivExample.transform.position.y - 800
            ),
            quaternion.identity
        );
        clickDivInstance.transform.SetParent(canvas.transform);
        GameObject clickButtonObject = clickDivInstance.transform.Find("RollButton").gameObject;
        ClickToRollButton clickButtonScript = clickButtonObject.GetComponent<ClickToRollButton>();
        clickButtonScript.currentAbilityCheck = abilitycheck;
        clickButtonScript.diceRoll = diceRoll;

        // set description text
        string abilityName = abilitycheck.ability;
        GameObject rollDescription = clickDivInstance.transform.Find("RollResult").gameObject;
        Text rollDescriptionText = rollDescription.GetComponent<Text>();
        rollDescriptionText.text =
            $"You triggered a {abilityName} ability check.\r\nClick below to roll.";

        // set button text
        FieldInfo fieldInfo = typeof(so_playerstats).GetField(abilityName);
        int correctAbilityScore = (int)fieldInfo.GetValue(player);
        Text clickButtonText = clickButtonObject.GetComponentInChildren<Text>();
        clickButtonText.text = $"\r\nRoll 1d20 + {correctAbilityScore}\r\n(your {abilityName})";
    }

    public void ShowAbilityCheckWindow(so_abilitycheck abilitycheck, int diceRoll)
    {
        windowOpen = true;
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

        // change the text
        string abilityName = abilitycheck.ability;
        FieldInfo fieldInfo = typeof(so_playerstats).GetField(abilityName);

        int correctAbilityScore = (int)fieldInfo.GetValue(player);
        Text rollText = frame.GetComponentInChildren<Text>();
        rollText.text =
            $"You rolled a {diceRoll}\r\nYour {abilitycheck.ability} is +{correctAbilityScore}\r\n\r\nTotal result: {diceRoll + correctAbilityScore}.";

        // change the image
        GameObject rollImageObject = frame.transform.Find("DiceSprite").gameObject;
        Image rollImage = rollImageObject.GetComponent<Image>();
        switch (diceRoll)
        {
            case 1:
                rollImage.sprite = result1;
                break;
            case 2:
                rollImage.sprite = result2;
                break;
            case 3:
                rollImage.sprite = result3;
                break;
            case 4:
                rollImage.sprite = result4;
                break;
            case 5:
                rollImage.sprite = result5;
                break;
            case 6:
                rollImage.sprite = result6;
                break;
            case 7:
                rollImage.sprite = result7;
                break;
            case 8:
                rollImage.sprite = result8;
                break;
            case 9:
                rollImage.sprite = result9;
                break;
            case 10:
                rollImage.sprite = result10;
                break;
            case 11:
                rollImage.sprite = result11;
                break;
            case 12:
                rollImage.sprite = result12;
                break;
            case 13:
                rollImage.sprite = result13;
                break;
            case 14:
                rollImage.sprite = result14;
                break;
            case 15:
                rollImage.sprite = result15;
                break;
            case 16:
                rollImage.sprite = result16;
                break;
            case 17:
                rollImage.sprite = result17;
                break;
            case 18:
                rollImage.sprite = result18;
                break;
            case 19:
                rollImage.sprite = result19;
                break;
            case 20:
                rollImage.sprite = result20;
                break;
        }

        // close button
        GameObject closeButtonImage = rollDivInstance.transform.Find("CloseButtonImage").gameObject;
        DestroySelf closeButtonScript = closeButtonImage.GetComponent<DestroySelf>();
        closeButtonScript.abilityCheckDiv = rollDivInstance;

        // vibereading mechanic - to show results not immediately
        closeButtonScript.insightCheck = true;
    }
}
