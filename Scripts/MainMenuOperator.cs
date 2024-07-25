using UnityEngine;
using UnityEngine.UI;

public class MainMenuOperator : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject bubbleCanvas;
    public GameObject energyAndDateCanvas;

    public void StartNewGame()
    {
        bubbleCanvas.gameObject.SetActive(true);
        energyAndDateCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
    }
}
