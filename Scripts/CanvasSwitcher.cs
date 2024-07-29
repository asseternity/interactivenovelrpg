using Unity.VisualScripting;
using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject abilityCheckShower;
    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvas3;
    public Canvas canvas4;
    public Canvas mainMenuCanvas;
    public Canvas energyAndDateCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas scheduleCanvas;

    public void Start()
    {
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);
        canvas3.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
        energyAndDateCanvas.gameObject.SetActive(false);
        pauseMenuCanvas.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void SetCanvas1()
    {
        AbilityCheckShower abilityCheckShowerScript =
            abilityCheckShower.GetComponent<AbilityCheckShower>();
        if (abilityCheckShowerScript.windowOpen == false)
        {
            canvas1.gameObject.SetActive(true);
            canvas2.gameObject.SetActive(false);
            canvas3.gameObject.SetActive(false);
            canvas4.gameObject.SetActive(false);
        }
    }

    public void SetCanvas2()
    {
        AbilityCheckShower abilityCheckShowerScript =
            abilityCheckShower.GetComponent<AbilityCheckShower>();
        if (abilityCheckShowerScript.windowOpen == false)
        {
            canvas1.gameObject.SetActive(false);
            canvas2.gameObject.SetActive(true);
            canvas3.gameObject.SetActive(false);
            canvas4.gameObject.SetActive(false);
        }
    }

    public void SetCanvas3()
    {
        AbilityCheckShower abilityCheckShowerScript =
            abilityCheckShower.GetComponent<AbilityCheckShower>();
        if (abilityCheckShowerScript.windowOpen == false)
        {
            canvas1.gameObject.SetActive(false);
            canvas2.gameObject.SetActive(false);
            canvas3.gameObject.SetActive(true);
            canvas4.gameObject.SetActive(false);
        }
    }

    public void SetCanvas4()
    {
        AbilityCheckShower abilityCheckShowerScript =
            abilityCheckShower.GetComponent<AbilityCheckShower>();
        if (abilityCheckShowerScript.windowOpen == false)
        {
            canvas1.gameObject.SetActive(false);
            canvas2.gameObject.SetActive(false);
            canvas3.gameObject.SetActive(false);
            canvas4.gameObject.SetActive(true);
        }
    }

    public bool gameStarted = false;
    public bool isPaused = false;
    public bool wasUsingSchedule = false;

    public void TogglePause()
    {
        if (gameStarted)
        {
            if (isPaused)
            {
                if (wasUsingSchedule == true)
                {
                    scheduleCanvas.gameObject.SetActive(true);
                    canvas1.gameObject.SetActive(false);
                }
                else
                {
                    canvas1.gameObject.SetActive(true);
                }
                wasUsingSchedule = false;
                canvas2.gameObject.SetActive(false);
                canvas3.gameObject.SetActive(false);
                canvas4.gameObject.SetActive(false);
                energyAndDateCanvas.gameObject.SetActive(true);
                pauseMenuCanvas.gameObject.SetActive(false);

                isPaused = false;
            }
            else
            {
                if (scheduleCanvas.gameObject.activeSelf)
                {
                    wasUsingSchedule = true;
                    scheduleCanvas.gameObject.SetActive(false);
                }
                canvas1.gameObject.SetActive(false);
                canvas2.gameObject.SetActive(false);
                canvas3.gameObject.SetActive(false);
                canvas4.gameObject.SetActive(false);
                energyAndDateCanvas.gameObject.SetActive(true);
                pauseMenuCanvas.gameObject.SetActive(true);

                isPaused = true;
            }
        }
    }
}
