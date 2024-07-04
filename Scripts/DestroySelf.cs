using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public GameObject bubbleMainObject;
    public GameObject abilityCheckShower;
    public GameObject abilityCheckDiv;
    public bool insightCheck = false;
    public GameObject insightChecker;

    public void Delete()
    {
        // close the popup
        Destroy(abilityCheckDiv.gameObject, 0f);
        // tell bubbleSpawner that the popup was closed
        BubbleSpawner bubbleMainScript = bubbleMainObject.GetComponent<BubbleSpawner>();
        bubbleMainScript.choicesSpawned = false;
        // tell abilityCheckShower (which tells canvasSwitcher) that the popup was closed
        AbilityCheckShower abilityCheckShowerScript =
            abilityCheckShower.GetComponent<AbilityCheckShower>();
        abilityCheckShowerScript.windowOpen = false;
        if (insightCheck)
        {
            InsightChecker insightCheckerScript = insightChecker.GetComponent<InsightChecker>();
            insightCheckerScript.ShowResult();
            insightCheck = false;
        }
    }
}
