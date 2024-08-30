using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOperator : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject bubbleCanvas;
    public GameObject energyAndDateCanvas;
    public GameObject canvasSwitcher;
    public GameObject loadSlotPickerCanvas;
    public GameObject BubbleSpawner;

    public void StartNewGame()
    {
        bubbleCanvas.gameObject.SetActive(true);
        energyAndDateCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
        loadSlotPickerCanvas.gameObject.SetActive(false);

        // allow pausing
        CanvasSwitcher canvasSwitcherScript = canvasSwitcher.GetComponent<CanvasSwitcher>();
        canvasSwitcherScript.gameStarted = true;

        // reset bubbleScript
        BubbleSpawner bubbleScript = BubbleSpawner.GetComponent<BubbleSpawner>();
        bubbleScript.WipeBubbles();
        bubbleScript.bubbles = new List<GameObject>();
        bubbleScript.StartGame();
        bubbleScript.currentBubble = 0;
    }
}
