using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Dialogue dialogue;
    private int sentenceIndex;

    void Start()
    {
        sentenceIndex = 0;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentenceIndex >= dialogue.sentences.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogue.sentences[sentenceIndex];
        sentenceIndex++;
    }

    void EndDialogue()
    {
        dialogueText.text = "End of dialogue.";
    }
}
