using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject canvas;
    public GameObject bubbleExample;
    public List<GameObject> bubbles = new List<GameObject>();
    int currentBubble = 0;
    GameObject bubbleToDelete;
    bool choicesSpawned = false;
    public so_dialoguebubble startingDialogue;
    public so_playerstats player;

    public void Start()
    {
        Button continueButtonButton = continueButton.GetComponent<Button>();
        continueButtonButton.onClick.AddListener(() => SpawnBubble(startingDialogue));
    }

    public void SpawnBubble(so_dialoguebubble dialogue)
    {
        if (!choicesSpawned)
        {
            if (dialogue.sentences[currentBubble] == "END")
            {
                if (dialogue.choices.Length == 0)
                {
                    choicesSpawned = false;
                    currentBubble = 0;
                    Button continueButtonButton = continueButton.GetComponent<Button>();
                    continueButtonButton.onClick.RemoveAllListeners();
                    so_dialoguebubble chosenDialogue = dialogue.noChoiceNextDialogue;
                    continueButton.onClick.AddListener(() => SpawnBubble(chosenDialogue));
                    SpawnBubble(chosenDialogue);
                }
                else
                {
                    DisplayChoices(dialogue);
                }
            }
            else
            {
                if (dialogue.requirements[currentBubble] == "")
                {
                    DrawBubble(dialogue);
                }
                else
                {
                    int nextViewableIndex = FindNextViewableIndex(dialogue);
                    currentBubble = nextViewableIndex;
                    DrawBubble(dialogue);
                }
            }
        }
    }

    public int FindNextViewableIndex(so_dialoguebubble dialogue)
    {
        int nextEmptyIndex = 0;
        int nextValidIndex = 0;
        int endIndex = dialogue.requirements.Length - 1;

        for (int i = currentBubble; i < dialogue.requirements.Length; i++)
        {
            if (dialogue.requirements[i] == "")
            {
                nextEmptyIndex = i;
                break;
            }
        }

        for (int i = currentBubble; i < dialogue.requirements.Length; i++)
        {
            if (dialogue.requirements[i] != null)
            {
                for (int j = 0; j < player.decisions.Length; j++)
                {
                    if (player.decisions[j] == dialogue.requirements[i])
                    {
                        nextValidIndex = i;
                    }
                }
            }
        }

        if (nextEmptyIndex == 0 && nextValidIndex == 0)
        {
            return endIndex;
        }
        else
        {
            if (nextEmptyIndex == 0)
            {
                return nextValidIndex;
            }
            else if (nextValidIndex == 0)
            {
                return nextEmptyIndex;
            }
            else
            {
                if (nextEmptyIndex < nextValidIndex)
                {
                    return nextEmptyIndex;
                }
                else
                {
                    return nextValidIndex;
                }
            }
        }
    }

    public void DrawBubble(so_dialoguebubble dialogue)
    {
        MoveBubbles();
        // store position of spawner empty gameObect
        Vector2 spawnerPosition = gameObject.transform.position;
        spawnerPosition.x -= 400;
        spawnerPosition.y += 230;
        // create an instance of exampleBubble
        GameObject newBubble = Instantiate(bubbleExample, spawnerPosition, Quaternion.identity);
        // set newBubble to be canvas' child
        newBubble.transform.SetParent(canvas.transform);
        // find ref to image component of newBubble
        Image newBubbleSprite = newBubble.GetComponent<Image>();
        // set dialogue bubble sprite to the assigned one
        newBubbleSprite.sprite = dialogue.bubbles[currentBubble];
        // find Text, the child of newBubble
        GameObject newBubbleTextObject = newBubble.transform.Find("DialogueBubbleText").gameObject;
        // find ref to text component of Text, the child of newBubble
        Text newBubbleText = newBubbleTextObject.GetComponent<Text>();
        // set dialogue text to the assigned string
        newBubbleText.text = dialogue.sentences[currentBubble];
        // add newBubble to a list
        bubbles.Add(newBubble);
        currentBubble++;
    }

    public void MoveBubbles()
    {
        // this will be called when a new bubble is created
        // it will iterate through all bubbles
        for (int j = 0; j < bubbles.Count; j++)
        {
            // and move them down a little
            Vector2 positionOfCurrentBubble = bubbles[j].transform.position;
            positionOfCurrentBubble.y -= 230;
            // assigned vector back to transform
            bubbles[j].transform.position = new Vector2(
                bubbles[j].transform.position.x,
                positionOfCurrentBubble.y
            );
            // and because we call it after creation of every single one
            // they will be moved down one by one
            // after they are moved, call DestroyBubbles(current bubble)
        }
        DestroyBubbles();
    }

    public void DestroyBubbles()
    {
        for (int h = 0; h < bubbles.Count; h++)
        { // test if bubble's position is too low
            Vector2 bubblePosition = bubbles[h].transform.position;
            if (bubblePosition.y < 250)
            // if it is, Destroy()
            {
                bubbleToDelete = bubbles[h];
            }
        }
        bubbles.Remove(bubbleToDelete);
        Destroy(bubbleToDelete, 0f);
    }

    public GameObject choiceboxA;
    public GameObject choiceboxB;
    public GameObject choiceboxC;

    public void DisplayChoices(so_dialoguebubble dialogue)
    {
        // if there is no more bubbleage in dialogue
        // then spawner calls this method, passing it the dialogue scriptableobject
        // this method creates buttons and displays choices

        // create button a
        Vector2 positionA = choiceboxA.transform.position;
        positionA.x += 356;
        GameObject newChoiceboxA = GameObject.Instantiate(
            choiceboxA,
            positionA,
            quaternion.identity
        );
        newChoiceboxA.transform.SetParent(canvas.transform);
        GameObject newChoiceboxA_textObject = newChoiceboxA.transform.Find("ButtonText").gameObject;
        Text newChoiceboxA_text = newChoiceboxA_textObject.GetComponent<Text>();
        newChoiceboxA_text.text = dialogue.choices[0];
        Button buttonA = newChoiceboxA.GetComponent<Button>();
        buttonA.onClick.AddListener(() => ChoicePicked(dialogue, 0));

        // create button b
        Vector2 positionB = choiceboxB.transform.position;
        positionB.x += 356;
        GameObject newChoiceboxB = GameObject.Instantiate(
            choiceboxB,
            positionB,
            quaternion.identity
        );
        newChoiceboxB.transform.SetParent(canvas.transform);
        GameObject newChoiceboxB_textObject = newChoiceboxB.transform.Find("ButtonText").gameObject;
        Text newChoiceboxB_text = newChoiceboxB_textObject.GetComponent<Text>();
        newChoiceboxB_text.text = dialogue.choices[1];
        Button buttonB = newChoiceboxB.GetComponent<Button>();
        buttonB.onClick.AddListener(() => ChoicePicked(dialogue, 1));

        // create button c
        Vector2 positionC = choiceboxC.transform.position;
        positionC.x += 356;
        GameObject newChoiceboxC = GameObject.Instantiate(
            choiceboxC,
            positionC,
            quaternion.identity
        );
        newChoiceboxC.transform.SetParent(canvas.transform);
        GameObject newChoiceboxC_textObject = newChoiceboxC.transform.Find("ButtonText").gameObject;
        Text newChoiceboxC_text = newChoiceboxC_textObject.GetComponent<Text>();
        newChoiceboxC_text.text = dialogue.choices[2];
        Button buttonC = newChoiceboxC.GetComponent<Button>();
        buttonC.onClick.AddListener(() => ChoicePicked(dialogue, 2));

        spawnedChoiceboxA = newChoiceboxA;
        spawnedChoiceboxB = newChoiceboxB;
        spawnedChoiceboxC = newChoiceboxC;

        choicesSpawned = true;
    }

    GameObject spawnedChoiceboxA;
    GameObject spawnedChoiceboxB;
    GameObject spawnedChoiceboxC;
    public Button continueButton;

    public void ChoicePicked(so_dialoguebubble dialogue, int choiceNumber)
    {
        Destroy(spawnedChoiceboxA, 0f);
        Destroy(spawnedChoiceboxB, 0f);
        Destroy(spawnedChoiceboxC, 0f);
        choicesSpawned = false;
        currentBubble = 0;
        Button continueButtonButton = continueButton.GetComponent<Button>();
        continueButtonButton.onClick.RemoveAllListeners();
        so_dialoguebubble chosenDialogue = dialogue.consequences[choiceNumber];
        continueButton.onClick.AddListener(() => SpawnBubble(chosenDialogue));
        SpawnBubble(chosenDialogue);

        // logging the decisions
        string[] newDecisions = new string[player.decisions.Length + 1];
        for (int i = 0; i < player.decisions.Length; i++)
        {
            newDecisions[i] = player.decisions[i];
        }
        newDecisions[newDecisions.Length - 1] = dialogue.labels[choiceNumber];
        player.decisions = newDecisions;

        // stat changes
        string statChangeString = dialogue.statChanges[choiceNumber];
        string[] statChangeSymbols = statChangeString.Select(c => c.ToString()).ToArray();

        string statToChange = "";
        string plusOrMinus = "";
        string changeAmount = "";

        for (int i = 0; i < 3; i++)
        {
            statToChange += statChangeSymbols[i];
        }

        if (statChangeSymbols[3] == "+")
        {
            plusOrMinus = "+";
        }
        else
        {
            plusOrMinus = "-";
        }

        for (int j = 4; j < statChangeSymbols.Length; j++)
        {
            changeAmount += statChangeSymbols[j];
        }

        int changeAmountInt = int.Parse(changeAmount);
        int currentAmount;

        switch (statToChange)
        {
            case "ST1":
                currentAmount = player.ST1;
                if (plusOrMinus == "+")
                {
                    player.ST1 = currentAmount + changeAmountInt;
                }
                else
                {
                    player.ST1 = currentAmount - changeAmountInt;
                }
                break;
            case "ST2":
                currentAmount = player.ST2;
                if (plusOrMinus == "+")
                {
                    player.ST2 = currentAmount + changeAmountInt;
                }
                else
                {
                    player.ST2 = currentAmount - changeAmountInt;
                }
                break;
            case "ST3":
                currentAmount = player.ST3;
                if (plusOrMinus == "+")
                {
                    player.ST3 = currentAmount + changeAmountInt;
                }
                else
                {
                    player.ST3 = currentAmount - changeAmountInt;
                }
                break;
            case "ST4":
                currentAmount = player.ST4;
                if (plusOrMinus == "+")
                {
                    player.ST4 = currentAmount + changeAmountInt;
                }
                else
                {
                    player.ST4 = currentAmount - changeAmountInt;
                }
                break;
            case "ST5":
                currentAmount = player.ST5;
                if (plusOrMinus == "+")
                {
                    player.ST5 = currentAmount + changeAmountInt;
                }
                else
                {
                    player.ST5 = currentAmount - changeAmountInt;
                }
                break;
            case "ST6":
                currentAmount = player.ST6;
                if (plusOrMinus == "+")
                {
                    player.ST6 = currentAmount + changeAmountInt;
                }
                else
                {
                    player.ST6 = currentAmount - changeAmountInt;
                }
                break;
            case "ST7":
                currentAmount = player.ST7;
                if (plusOrMinus == "+")
                {
                    player.ST7 = currentAmount + changeAmountInt;
                }
                else
                {
                    player.ST7 = currentAmount - changeAmountInt;
                }
                break;
            case "ST8":
                currentAmount = player.ST8;
                if (plusOrMinus == "+")
                {
                    player.ST8 = currentAmount + changeAmountInt;
                }
                else
                {
                    player.ST8 = currentAmount - changeAmountInt;
                }
                break;
        }
    }
}

// to do:
// - stress (mental energy) and exhaustion (physical energy) meters in upper right corner - with lines that go up and down
// - reduce or gain stress and exhaustion throughout
// - date and weekday in bottom right
// - at the beginning of every week - create your schedule by clicking and dragging classes [MAKE NEW CANVAS FOR THIS]
// - whichever classes you pick for whichever day - you get asked logic questions and depending on your answer you get more or less of
// the stat [NEW CANVAS FOR THIS]
// - skill checks
// roll a 1d20 + skill modifier for a skill check [NEW CANVAS FOR THIS]
// - relationship mechanics that are not "approval meter". something about reading / knowing people better? like, you have no idea how
// much they like you - you can only gauge by what they tell you - but you will be given extra clues if you know them better
// *GOOD FOR BETRAYALS AND SUCH!*
