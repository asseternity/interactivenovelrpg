using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject canvas;
    public GameObject bubbleExample;
    public GameObject bubbleExampleImage;
    public List<GameObject> bubbles = new List<GameObject>();
    public int currentBubble = 0;
    public so_dialoguebubble currentDialogue;
    GameObject bubbleToDelete;
    public bool choicesSpawned = false;
    public so_dialoguebubble startingDialogue;
    public so_playerstats player;
    public GameObject abilityCheckShower;
    public GameObject logText;
    public GameObject dateKeeper;
    public Queue<so_dialoguebubble> scheduleEventOrder = new Queue<so_dialoguebubble>();
    public GameObject scheduleCanvas;
    so_dialoguebubble firstInSchedule;
    so_dialoguebubble scheduleEndBubble;
    public so_dialoguebubble lastShownBubble;

    public void Start()
    {
        Button continueButtonButton = continueButton.GetComponent<Button>();
        continueButtonButton.onClick.AddListener(() => SpawnBubble(startingDialogue));
        player.ResetStats();
        scheduleCanvas.gameObject.SetActive(false);
    }

    public void SpawnBubble(so_dialoguebubble dialogue)
    {
        currentDialogue = dialogue;
        if (!choicesSpawned)
        {
            if (scheduleEventOrder.Count == 0)
            {
                if (dialogue.sentences[currentBubble] == "END")
                {
                    if (dialogue.choices.Length == 0 && dialogue.abilityCheck == null)
                    {
                        if (dialogue.startsScheduleOnEnd)
                        {
                            StartSchedule(dialogue.noChoiceNextDialogue);
                        }
                        else
                        {
                            choicesSpawned = false;
                            currentBubble = 0;
                            Button continueButtonButton = continueButton.GetComponent<Button>();
                            continueButtonButton.onClick.RemoveAllListeners();
                            so_dialoguebubble nextDialogue = dialogue.noChoiceNextDialogue;
                            continueButton.onClick.AddListener(() => SpawnBubble(nextDialogue));
                            SpawnBubble(nextDialogue);
                        }
                    }
                    else if (dialogue.choices.Length > 0)
                    {
                        DisplayChoices(dialogue);
                    }
                    else
                    {
                        AbilityCheck(dialogue);
                    }
                }
                else
                {
                    // check if we are allowed to see this bubble
                    if (dialogue.requirements[currentBubble] == "")
                    {
                        if (dialogue.sentences[currentBubble] == "IMAGE")
                        {
                            DrawImage(dialogue);
                        }
                        else
                        {
                            DrawBubble(dialogue);
                        }
                    }
                    else
                    {
                        int nextViewableIndex = FindNextViewableIndex(dialogue);
                        currentBubble = nextViewableIndex;
                        DrawBubble(dialogue);
                    }
                }
            }
            else
            {
                // check if END
                if (currentDialogue.sentences[currentBubble] == "END")
                {
                    so_dialoguebubble nextInSchedule = scheduleEventOrder.Dequeue();
                    currentBubble = 0;
                    Button continueButtonButton = continueButton.GetComponent<Button>();
                    continueButtonButton.onClick.RemoveAllListeners();
                    continueButton.onClick.AddListener(() => SpawnBubble(nextInSchedule));
                    SpawnBubble(nextInSchedule);
                }
                else
                {
                    // check if we are allowed to see this bubble
                    if (dialogue.requirements[currentBubble] == "")
                    {
                        if (dialogue.sentences[currentBubble] == "IMAGE")
                        {
                            DrawImage(dialogue);
                        }
                        else
                        {
                            DrawBubble(dialogue);
                        }
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
        Debug.Log("currentBubble = " + currentBubble);
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
        // store last bubble
        lastShownBubble = dialogue;
        // scroll everything down
        MoveBubbles("text");
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
        newBubbleText.text = SliceOutFalseMark(dialogue);
        // add newBubble to a list
        bubbles.Add(newBubble);
        Logger logTextScript = logText.GetComponent<Logger>();
        logTextScript.AddLog(dialogue.sentences[currentBubble]);
        currentBubble++;
    }

    public void DrawImage(so_dialoguebubble dialogue)
    {
        // store last bubble
        lastShownBubble = dialogue;
        // scroll everything down
        MoveBubbles("image");
        // store position of spawner empty gameObect
        Vector2 spawnerPosition = gameObject.transform.position;
        spawnerPosition.x -= 400;
        spawnerPosition.y += 130;
        // create an instance of exampleBubble
        GameObject newBubble = Instantiate(
            bubbleExampleImage,
            spawnerPosition,
            Quaternion.identity
        );
        // set newBubble to be canvas' child
        newBubble.transform.SetParent(canvas.transform);
        GameObject newBubbleImageObject = newBubble
            .transform.Find("ImageBubbleSubjectMatter")
            .gameObject;
        Image newBubbleImage = newBubbleImageObject.GetComponent<Image>();
        newBubbleImage.sprite = dialogue.images[currentBubble];
        // add newBubble to a list
        bubbles.Add(newBubble);
        currentBubble++;
    }

    public void MoveBubbles(string type)
    {
        // this will be called when a new bubble is created
        // it will iterate through all bubbles
        for (int j = 0; j < bubbles.Count; j++)
        {
            // and move them down a little
            RectTransform rectTransform = bubbles[j].GetComponent<RectTransform>();
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            if (type == "image")
            {
                anchoredPosition.y -= 450;
            }
            else
            {
                anchoredPosition.y -= 230;
            }
            // assign vector back to RectTransform
            rectTransform.anchoredPosition = anchoredPosition;
            // after they are moved, call DestroyBubbles(current bubble)
        }
        DestroyBubbles();
    }

    public void DestroyBubbles()
    {
        List<GameObject> bubblesToDelete = new List<GameObject>();

        for (int h = 0; h < bubbles.Count; h++)
        {
            // test if bubble's anchored position is too low
            RectTransform rectTransform = bubbles[h].GetComponent<RectTransform>();
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            if (anchoredPosition.y < 250)
            {
                // if it is, add it to the list of bubbles to delete
                bubblesToDelete.Add(bubbles[h]);
            }
        }

        // Destroy all bubbles that need to be deleted
        foreach (GameObject bubble in bubblesToDelete)
        {
            bubbles.Remove(bubble);
            Destroy(bubble, 0f);
        }
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
            case "SMA":
                currentAmount = player.Smarts;
                if (plusOrMinus == "+")
                {
                    player.Smarts = currentAmount + changeAmountInt;
                }
                else
                {
                    player.Smarts = currentAmount - changeAmountInt;
                }
                break;
            case "BRA":
                currentAmount = player.Brawn;
                if (plusOrMinus == "+")
                {
                    player.Brawn = currentAmount + changeAmountInt;
                }
                else
                {
                    player.Brawn = currentAmount - changeAmountInt;
                }
                break;
            case "AGI":
                currentAmount = player.Agility;
                if (plusOrMinus == "+")
                {
                    player.Agility = currentAmount + changeAmountInt;
                }
                else
                {
                    player.Agility = currentAmount - changeAmountInt;
                }
                break;
            case "CHA":
                currentAmount = player.Charisma;
                if (plusOrMinus == "+")
                {
                    player.Charisma = currentAmount + changeAmountInt;
                }
                else
                {
                    player.Charisma = currentAmount - changeAmountInt;
                }
                break;
            case "POP":
                currentAmount = player.Popularity;
                if (plusOrMinus == "+")
                {
                    player.Popularity = currentAmount + changeAmountInt;
                }
                else
                {
                    player.Popularity = currentAmount - changeAmountInt;
                }
                break;
            case "VIB":
                currentAmount = player.Vibereading;
                if (plusOrMinus == "+")
                {
                    player.Vibereading = currentAmount + changeAmountInt;
                }
                else
                {
                    player.Vibereading = currentAmount - changeAmountInt;
                }
                break;
            case "STR":
                currentAmount = player.Stress;
                if (plusOrMinus == "+")
                {
                    player.Stress = currentAmount + changeAmountInt;
                }
                else
                {
                    player.Stress = currentAmount - changeAmountInt;
                }
                break;
            case "EXH":
                currentAmount = player.Exhaustion;
                if (plusOrMinus == "+")
                {
                    player.Exhaustion = currentAmount + changeAmountInt;
                }
                else
                {
                    player.Exhaustion = currentAmount - changeAmountInt;
                }
                break;
            case "DAT":
                DateKeeper dateKeeperScript = dateKeeper.GetComponent<DateKeeper>();
                dateKeeperScript.NextDay();
                break;
        }
    }

    public void AbilityCheck(so_dialoguebubble dialogue)
    {
        Debug.Log("bubble.AbilityCheck called");
        // rolling
        int roll = UnityEngine.Random.Range(1, 20);
        string applicableStat = dialogue.abilityCheck.ability;
        int playersModifier = 0;
        switch (applicableStat)
        {
            case "Smarts":
                playersModifier = player.Smarts;
                break;
            case "Brawn":
                playersModifier = player.Brawn;
                break;
            case "Agility":
                playersModifier = player.Agility;
                break;
            case "Charisma":
                playersModifier = player.Charisma;
                break;
            case "Popularity":
                playersModifier = player.Popularity;
                break;
            case "Vibereading":
                playersModifier = player.Vibereading;
                break;
        }
        int result = roll + playersModifier;
        // showing the roll
        AbilityCheckShower abilityCheckShowerScript =
            abilityCheckShower.GetComponent<AbilityCheckShower>();
        abilityCheckShowerScript.ShowRollWindow(dialogue.abilityCheck, roll);
        // checking result
        if (result >= dialogue.abilityCheck.DC)
        {
            dialogue.abilityCheck.success = true;
            choicesSpawned = false;
            currentBubble = 0;
            Button continueButtonButton = continueButton.GetComponent<Button>();
            continueButtonButton.onClick.RemoveAllListeners();
            so_dialoguebubble chosenDialogue = dialogue.abilityCheckSuccessPath;
            continueButton.onClick.AddListener(() => SpawnBubble(chosenDialogue));
            SpawnBubble(chosenDialogue);
        }
        else
        {
            dialogue.abilityCheck.success = false;
            choicesSpawned = false;
            currentBubble = 0;
            Button continueButtonButton = continueButton.GetComponent<Button>();
            continueButtonButton.onClick.RemoveAllListeners();
            so_dialoguebubble chosenDialogue = dialogue.abilityCheckFailurePath;
            continueButton.onClick.AddListener(() => SpawnBubble(chosenDialogue));
            SpawnBubble(chosenDialogue);
        }
        choicesSpawned = true;
    }

    public string SliceOutFalseMark(so_dialoguebubble dialogue)
    {
        string resultingString = dialogue.sentences[currentBubble];
        int markIndex = dialogue.sentences[currentBubble].IndexOf("[");
        if (markIndex >= 0)
        {
            resultingString = resultingString.Substring(0, markIndex);
        }
        return resultingString;
    }

    public void StartSchedule(so_dialoguebubble bubbleAfterSchedule)
    {
        // empty first in schedule
        firstInSchedule = null;
        // save the next main story bubble (after scheduled events end)
        scheduleEndBubble = bubbleAfterSchedule;
        // show schedule canvas
        scheduleCanvas.gameObject.SetActive(true);
        // hide bubble canvas
        canvas.gameObject.SetActive(false);
        // fill the stack in ScheduleOptionButton
        // await for the stack to be = 8
        // go through the stack in spawnbubble
        // until the stack is empty
        // and then just go as per usual
    }

    public void Update()
    {
        if (scheduleEventOrder.Count > 6)
        {
            // test
            Debug.Log("Seven events added to the Stack");
            // wait half a second
            Invoke("FinishSchedule", 0.5f);
        }
    }

    public void FinishSchedule()
    {
        Debug.Log("finish schedule invoke");
        // hide the schedule canvas
        scheduleCanvas.gameObject.SetActive(false);
        // show the bubble canvas
        canvas.gameObject.SetActive(true);
        // SpawnBubble
        if (firstInSchedule == null)
        {
            // add the next element of the last bubble to the end of the queue
            scheduleEventOrder.Enqueue(scheduleEndBubble);
            // clear the scheduleEndBubble var
            scheduleEndBubble = null;
            // start spawning events!
            firstInSchedule = scheduleEventOrder.Peek();
            SpawnBubble(firstInSchedule);
        }
    }
}

// bugs:
//      - regular ability checks paint bubbles like insight checks

// major:
//      - whichever classes you pick for whichever day - you get asked logic questions and depending on your answer you get more or less of
//      the stat [NEW CANVAS FOR THIS]
//      - Main Menu
//      - Saving

// minor:
//      - reduce or gain stress and exhaustion throughout
//      - relationship mechanics that are not "approval meter". something about reading / knowing people better? like, you have no idea how
//      much they like you - you can only gauge by what they tell you - but you will be given extra clues if you know them better
//      *GOOD FOR BETRAYALS AND SUCH!*
//      - Mark lies ONLY before decisions, when you can ACT fon knowing that it's a lie!
//      - Tutorial
