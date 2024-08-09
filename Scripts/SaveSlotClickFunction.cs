using UnityEngine;

public class SaveSlotClickFunction : MonoBehaviour
{
    public GameObject gameSaverLoader;
    public int saveSlot;
    public GameObject saveSlotButton;

    public void SaveSlotClicked()
    {
        saveSlotButton = gameObject;
        SaveButtonOperator saveLoadScript = gameSaverLoader.GetComponent<SaveButtonOperator>();
        saveLoadScript.SaveGame(saveSlot, saveSlotButton);
    }
}
