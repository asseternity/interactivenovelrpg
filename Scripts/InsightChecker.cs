using UnityEngine;

public class InsightChecker : MonoBehaviour
{
    public GameObject bubbleMainObject;

    public void TriggerInsightCheck()
    {
        BubbleSpawner bubbleScript = bubbleMainObject.GetComponent<BubbleSpawner>();
        so_dialoguebubble currentDialogue = bubbleScript.currentDialogue;
        // so i can extract elements with currentDialogue.sentences[currentBubble], but how do I set/get things like DC?
        // write algorithm
    }
}
