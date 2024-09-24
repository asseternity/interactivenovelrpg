using UnityEditor;
using UnityEngine;

public class ScriptCreator : EditorWindow
{
    // so_dialogueBubble vars
    public Sprite[] bubbles;
    public string[] requirements;
    public string[] sentences;
    public Sprite[] images;
    public string[] choices;
    public so_dialoguebubble[] consequences;
    public string[] labels;
    public string[] statChanges;
    public so_dialoguebubble noChoiceNextDialogue;
    public so_abilitycheck abilityCheck;
    public so_dialoguebubble abilityCheckSuccessPath;
    public so_dialoguebubble abilityCheckFailurePath;
    public bool fromSchedule;
    public bool startsScheduleOnEnd;

    // Create a window menu item
    [MenuItem("Tools/Script Creator")]
    public static void ShowWindow()
    {
        GetWindow<ScriptCreator>("Script Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Script Creator", EditorStyles.boldLabel);

        // input fields
        SerializedObject so = new SerializedObject(this);
        EditorGUILayout.PropertyField(so.FindProperty("bubbles"), true);
        EditorGUILayout.PropertyField(so.FindProperty("requirements"), true);
        EditorGUILayout.PropertyField(so.FindProperty("sentences"), true);
        EditorGUILayout.PropertyField(so.FindProperty("images"), true);
        EditorGUILayout.PropertyField(so.FindProperty("choices"), true);
        EditorGUILayout.PropertyField(so.FindProperty("consequences"), true);
        EditorGUILayout.PropertyField(so.FindProperty("labels"), true);
        EditorGUILayout.PropertyField(so.FindProperty("statChanges"), true);
        noChoiceNextDialogue = (so_dialoguebubble)
            EditorGUILayout.ObjectField(
                "No Choice Next Dialogue",
                noChoiceNextDialogue,
                typeof(so_dialoguebubble),
                false
            );
        abilityCheck = (so_abilitycheck)
            EditorGUILayout.ObjectField(
                "Ability Check",
                abilityCheck,
                typeof(so_abilitycheck),
                false
            );
        abilityCheckSuccessPath = (so_dialoguebubble)
            EditorGUILayout.ObjectField(
                "Ability Check Success Path",
                abilityCheckSuccessPath,
                typeof(so_dialoguebubble),
                false
            );
        abilityCheckFailurePath = (so_dialoguebubble)
            EditorGUILayout.ObjectField(
                "Ability Check Failure Path",
                abilityCheckFailurePath,
                typeof(so_dialoguebubble),
                false
            );
        fromSchedule = EditorGUILayout.Toggle("From Schedule", fromSchedule);
        startsScheduleOnEnd = EditorGUILayout.Toggle("Starts Schedule On End", startsScheduleOnEnd);

        // Apply changes made to the serialized object
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Create Dialogue Bubble"))
        {
            CreateDialogueBubble();
        }
    }

    void CreateDialogueBubble()
    {
        // Create a new instance of the ScriptableObject
        so_dialoguebubble newBubble = ScriptableObject.CreateInstance<so_dialoguebubble>();

        // Assign values from the fields in the editor
        newBubble.bubbles = bubbles;
        newBubble.requirements = requirements;
        newBubble.sentences = sentences;
        newBubble.images = images;
        newBubble.choices = choices;
        newBubble.consequences = consequences;
        newBubble.labels = labels;
        newBubble.statChanges = statChanges;
        newBubble.noChoiceNextDialogue = noChoiceNextDialogue;
        newBubble.abilityCheck = abilityCheck;
        newBubble.abilityCheckSuccessPath = abilityCheckSuccessPath;
        newBubble.abilityCheckFailurePath = abilityCheckFailurePath;
        newBubble.fromSchedule = fromSchedule;
        newBubble.startsScheduleOnEnd = startsScheduleOnEnd;

        // Save the new ScriptableObject as an asset in the project
        AssetDatabase.CreateAsset(newBubble, "Assets/Editor/SO_DB/NewSO_DialogueBubble.asset");
        AssetDatabase.SaveAssets();

        Debug.Log("Dialogue Bubble Created Successfully!");
    }
}
