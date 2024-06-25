using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public GameObject bubbleMainObject;

    public void Delete()
    {
        Destroy(gameObject, 0f);
        BubbleSpawner bubbleMainScript = bubbleMainObject.GetComponent<BubbleSpawner>();
        bubbleMainScript.choicesSpawned = false;
    }
}
