using UnityEngine;

public class ClickToRollButton : MonoBehaviour
{
    public GameObject abilityCheckShower;
    public so_abilitycheck currentAbilityCheck;
    public int diceRoll;
    public GameObject div;

    public void Roll()
    {
        Debug.Log("Roll called!");
        AbilityCheckShower abilityCheckShowerScript =
            abilityCheckShower.GetComponent<AbilityCheckShower>();
        abilityCheckShowerScript.ShowAbilityCheckWindow(currentAbilityCheck, diceRoll);
        Destroy(div, 0f);
    }
}
