using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TextParser : MonoBehaviour
{
    [MenuItem("Tools/Parse Script File")]
    public static void ParseScriptFile()
    {
        string path = EditorUtility.OpenFilePanel("Select Script File", "", "txt");

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("No file selected");
            return;
        }

        List<so_dialoguebubble> bubbles = ParseFile(path);

        foreach (so_dialoguebubble bubble in bubbles)
        {
            AssetDatabase.CreateAsset(bubble, $"Assets/{bubble.name}");
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Dialogue parsing and asset creation completed!");
    }

    private static List<so_dialoguebubble> ParseFile(string path)
    {
        List<so_dialoguebubble> bubbles = new List<so_dialoguebubble>();
        string[] lines = File.ReadAllLines(path);

        int numberOfSentences = 0;

        // check the length of the lines
        foreach (string line in lines)
        {
            if (line.StartsWith(" - "))
            {
                numberOfSentences++;
            }
        }

        foreach (string line in lines)
        {
            if (line.StartsWith("#"))
            {
                // means that it's the start of a new dialogue bubble
                so_dialoguebubble currentBubble =
                    ScriptableObject.CreateInstance<so_dialoguebubble>();
                bubbles.Add(currentBubble);
                currentBubble.name = line.Substring(2).Trim(); // Use the line after '#' as bubble name
            }
            else if (line.StartsWith("BubbleSprite:"))
            {
                // get sprite path
                string spritePath = line.Split(":")[1].Trim();
                // load the sprite
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                // assign bubble to every line
                for (int i = 0; i < numberOfSentences; i++)
                {
                    Sprite[] localArray = bubbles[bubbles.Count - 1].bubbles;
                    Array.Resize(ref localArray, localArray.Length + 1);
                    localArray[localArray.Length - 1] = sprite;
                    bubbles[bubbles.Count - 1].bubbles = localArray;
                }
            }
            else if (line.StartsWith("StatChange:"))
            {
                // assign to stat changes
                string instruction = line.Split(":")[1].Trim();
                string[] statChanges = instruction.Split(",");

                Array.Resize(ref bubbles[bubbles.Count - 1].statChanges, statChanges.Length);
                for (int i = 0; i < statChanges.Length; i++)
                {
                    bubbles[bubbles.Count - 1].statChanges[i] = statChanges[i];
                }
            }
            else if (line.StartsWith("Sentences:")) { }
            else if (line.StartsWith(" - "))
            {
                // assign it to sentences[]
                string actualDialogue = line.Substring(3).Trim();
                string[] sentences = bubbles[bubbles.Count - 1].sentences;
                Array.Resize(ref sentences, sentences.Length + 1);
                sentences[sentences.Length - 1] = actualDialogue;
                bubbles[bubbles.Count - 1].sentences = sentences;

                // requirements
                if (actualDialogue.Contains("*"))
                {
                    string requirement = actualDialogue.Split("*")[1].Trim();
                    string[] currentReqs = bubbles[bubbles.Count - 1].requirements;
                    Array.Resize(ref currentReqs, currentReqs.Length + 1);
                    currentReqs[currentReqs.Length - 1] = requirement;
                    bubbles[bubbles.Count - 1].requirements = currentReqs;
                }
            }
            else if (line.StartsWith("Image:"))
            {
                // resize the array to how many sentences there are
                for (int i = 0; i < numberOfSentences; i++)
                {
                    Sprite[] localArray = bubbles[bubbles.Count - 1].images;
                    Array.Resize(ref localArray, localArray.Length + 1);
                    bubbles[bubbles.Count - 1].images = localArray;
                }
                // load a sprite from path
                string spritePath = line.Split(":")[1].Trim();
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                // apply it to the line that is "IMAGE"
                for (int i = 0; i < bubbles[bubbles.Count - 1].sentences.Length; i++)
                {
                    if (bubbles[bubbles.Count - 1].sentences[i].StartsWith("IMAGE"))
                    {
                        bubbles[bubbles.Count - 1].images[i] = sprite;
                    }
                }
            }
            else if (line.StartsWith("EndType:")) { }
            else if (line.StartsWith("Choices:"))
            {
                // put in choices[]
                string[] choices = line.Substring(8).Trim().Split(',');
                bubbles[bubbles.Count - 1].choices = choices;
            }
            else if (line.StartsWith("Consequences:"))
            {
                string[] consequenceNames = line.Substring(13).Trim().Split(',');
                Array.Resize(ref bubbles[bubbles.Count - 1].consequences, consequenceNames.Length);
                // find SOs by names of bubbles
                // put in consequences[]
                for (int i = 0; i < consequenceNames.Length; i++)
                {
                    so_dialoguebubble consequence = FindBubbleByName(
                        bubbles,
                        consequenceNames[i].Trim()
                    );
                    bubbles[bubbles.Count - 1].consequences[i] = consequence;
                }
            }
            else if (line.StartsWith("Labels:"))
            {
                // put in labels[]
                string[] labels = line.Substring(8).Trim().Split(',');
                bubbles[bubbles.Count - 1].labels = labels;
            }
            else if (line.StartsWith("NextBubble:"))
            {
                // find SO by name of bubble
                // put in NextBubble
                string nextBubbleName = line.Split(":")[1].Trim();
                bubbles[bubbles.Count - 1].noChoiceNextDialogue = FindBubbleByName(
                    bubbles,
                    nextBubbleName
                );
            }
            else if (line.StartsWith("AbilityCheck:"))
            {
                // find SO_AC by path
                string abilityCheckPath = line.Split(":")[1].Trim();
                // put in Ability Check
                bubbles[bubbles.Count - 1].abilityCheck =
                    AssetDatabase.LoadAssetAtPath<so_abilitycheck>(abilityCheckPath);
            }
            else if (line.StartsWith("Win:"))
            {
                // Assign abilityCheckSuccessPath by SO name
                string winBubbleName = line.Split(":")[1].Trim();
                bubbles[bubbles.Count - 1].abilityCheckSuccessPath = FindBubbleByName(
                    bubbles,
                    winBubbleName
                );
            }
            else if (line.StartsWith("Lose:"))
            {
                // Assign abilityCheckFailurePath by SO name
                string loseBubbleName = line.Split(":")[1].Trim();
                bubbles[bubbles.Count - 1].abilityCheckFailurePath = FindBubbleByName(
                    bubbles,
                    loseBubbleName
                );
            }
            else if (line.StartsWith("---"))
            {
                // at the end add one more sentence with "END"
                string[] sentences = bubbles[bubbles.Count - 1].sentences;
                Array.Resize(ref sentences, sentences.Length + 1);
                sentences[sentences.Length - 1] = "END";
                bubbles[bubbles.Count - 1].sentences = sentences;
                // Reset sentence count for the new bubble
                numberOfSentences = 0;
            }
        }

        return bubbles;
    }

    private static so_dialoguebubble FindBubbleByName(List<so_dialoguebubble> bubbles, string name)
    {
        // Helper function to find a dialogue bubble by its name
        foreach (so_dialoguebubble bubble in bubbles)
        {
            if (bubble.name == name)
                return bubble;
        }
        return null;
    }
}
