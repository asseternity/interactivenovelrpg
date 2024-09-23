using UnityEngine;

public class SaveSlotClickFunction : MonoBehaviour
{
    public GameObject gameSaverLoader;
    public int saveSlot;
    public bool slotFilled = false;
    public GameObject saveSlotButton;

    public void SaveSlotClicked()
    {
        saveSlotButton = gameObject;
        SaveButtonOperator saveLoadScript = gameSaverLoader.GetComponent<SaveButtonOperator>();
        saveLoadScript.SaveGame(saveSlot, saveSlotButton);
    }

    public void LoadSlotClicked()
    {
        if (slotFilled)
        {
            saveSlotButton = gameObject;
            SaveButtonOperator saveLoadScript = gameSaverLoader.GetComponent<SaveButtonOperator>();
            saveLoadScript.LoadGame(saveSlot);
        }
    }
}
