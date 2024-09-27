using UnityEngine;

[CreateAssetMenu(fileName = "New SO_DialogueBubble", menuName = "SO_DialogueBubble")]
public class so_dialoguebubble : ScriptableObject
{
    public Sprite[] bubbles;
    public string[] requirements;
    public string[] sentences;
    public Sprite[] images;
    public string[] choices;
    public so_dialoguebubble[] consequences;
    public string[] consequencesRefs;
    public string[] labels;
    public string[] statChanges;
    public so_dialoguebubble noChoiceNextDialogue;
    public string noChoiceNextDialogueRef;
    public so_abilitycheck abilityCheck;
    public so_dialoguebubble abilityCheckSuccessPath;
    public string abilityCheckSuccessPathRef;
    public so_dialoguebubble abilityCheckFailurePath;
    public string abilityCheckFailurePathRef;
    public bool fromSchedule;
    public bool startsScheduleOnEnd;
}
