using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject abilityCheckShower;
    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvas3;
    public Canvas canvas4;

    public void Start()
    {
        canvas1.gameObject.SetActive(true);
        canvas2.gameObject.SetActive(false);
        canvas3.gameObject.SetActive(false);
        canvas4.gameObject.SetActive(false);
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
}
