using UnityEngine;
using UnityEngine.UI;

public class InsightChecker : MonoBehaviour
{
    public GameObject bubbleMainObject;
    public GameObject abilityCheckShower;
    public bool IsItTrue;
    public so_playerstats player;
    public so_abilitycheck defaultVibereading;
    public int bubbleOnLastInsight;
    private int roll;

    public void TriggerInsightCheck()
    {
        BubbleSpawner bubbleScript = bubbleMainObject.GetComponent<BubbleSpawner>();
        // checking if this bubble was already insight checked
        InsightCheckedYet insightCheckedYet = bubbleScript
            .bubbles[bubbleScript.bubbles.Count - 1]
            .GetComponent<InsightCheckedYet>();
        if (!insightCheckedYet.wasChecked)
        {
            insightCheckedYet.wasChecked = true;
            int markIndex;
            markIndex = bubbleScript
                .currentDialogue.sentences[bubbleScript.currentBubble - 1]
                .IndexOf("[");
            if (markIndex >= 0)
            {
                IsItTrue = false;
            }
            else
            {
                IsItTrue = true;
            }
            // rolling
            roll = UnityEngine.Random.Range(1, 20);
            AbilityCheckShower abilityCheckShowerScript =
                abilityCheckShower.GetComponent<AbilityCheckShower>();
            abilityCheckShowerScript.ShowRollWindow(defaultVibereading, roll);
        }
    }

    public void ShowResult()
    {
        BubbleSpawner bubbleScript = bubbleMainObject.GetComponent<BubbleSpawner>();
        // checking result
        int modifier = player.Vibereading;
        int result = roll + modifier;
        if (result > defaultVibereading.DC)
        {
            Text buttonText = gameObject.GetComponentInChildren<Text>();
            buttonText.text = "Red = lie, green = truth, grey = you failed the check.";
            if (IsItTrue)
            {
                GameObject lastBubble = bubbleScript.bubbles[bubbleScript.bubbles.Count - 1];
                Text lastBubbleText = lastBubble.GetComponentInChildren<Text>();
                lastBubbleText.color = new Color(80f / 255f, 200f / 255f, 120f / 255f);
                lastBubbleText.fontStyle = FontStyle.Bold;
            }
            else
            {
                GameObject lastBubble = bubbleScript.bubbles[bubbleScript.bubbles.Count - 1];
                Text lastBubbleText = lastBubble.GetComponentInChildren<Text>();
                lastBubbleText.color = new Color(210 / 255f, 43f / 255f, 43f / 255f);
                lastBubbleText.fontStyle = FontStyle.Bold;
            }
        }
        else
        {
            GameObject lastBubble = bubbleScript.bubbles[bubbleScript.bubbles.Count - 1];
            Text lastBubbleText = lastBubble.GetComponentInChildren<Text>();
            lastBubbleText.color = Color.grey;
        }
        // this is to update the button text back to default
        bubbleOnLastInsight = bubbleScript.currentBubble;
    }

    public void Update()
    {
        BubbleSpawner bubbleScript = bubbleMainObject.GetComponent<BubbleSpawner>();
        if (bubbleOnLastInsight != bubbleScript.currentBubble)
        {
            Text buttonText = gameObject.GetComponentInChildren<Text>();
            buttonText.text = $"Insight check\r\nlast message";
        }
    }

    public void HoverEnter()
    {
        ButtonColor buttonColor = gameObject.GetComponent<ButtonColor>();
        buttonColor.OnHoverEnter();
        BubbleSpawner bubbleScript = bubbleMainObject.GetComponent<BubbleSpawner>();
        GameObject lastBubble = bubbleScript.bubbles[bubbleScript.bubbles.Count - 1];
        Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
        lastBubble.gameObject.transform.localScale += scaleChange;
    }

    public void HoverExit()
    {
        ButtonColor buttonColor = gameObject.GetComponent<ButtonColor>();
        buttonColor.OnHoverExit();
        BubbleSpawner bubbleScript = bubbleMainObject.GetComponent<BubbleSpawner>();
        GameObject lastBubble = bubbleScript.bubbles[bubbleScript.bubbles.Count - 1];
        Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
        lastBubble.gameObject.transform.localScale -= scaleChange;
    }
}
