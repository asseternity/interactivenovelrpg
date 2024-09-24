using System.Collections.Generic;
using System.IO;
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
                    bubbles[bubbles.Count - 1].bubbles[i] = sprite;
                }
            }
            else if (line.StartsWith("StatChange:"))
            {
                // assign to stat changes
                string instruction = line.Split(":")[1].Trim();
                string[] statChanges = instruction.Split(",");
                bubbles[bubbles.Count - 1].statChanges[0] = statChanges[0];
                bubbles[bubbles.Count - 1].statChanges[1] = statChanges[1];
                bubbles[bubbles.Count - 1].statChanges[2] = statChanges[2];
            }
            else if (line.StartsWith("Sentences:")) { }
            else if (line.StartsWith(" - "))
            {
                // assign it to sentences[]
                string actualDialogue = line.Split(":")[1].Trim();
                bubbles[bubbles.Count - 1].sentences[
                    bubbles[bubbles.Count - 1].sentences.Length - 1
                ] = actualDialogue;
            }
            else if (line.StartsWith("Image:"))
            {
                // load a sprite from path
            }
            else if (line.StartsWith("EndType:"))
            {
                // just for me
            }
            else if (line.StartsWith("Choices:"))
            {
                // put in choices[]
            }
            else if (line.StartsWith("Consequences:"))
            {
                // find SOs by names of bubbles
                // put in consequences[]
            }
            else if (line.StartsWith("Labels:"))
            {
                // put in labels[]
            }
            else if (line.StartsWith("NextBubble:"))
            {
                // find SO by name of bubble
                // put in NextBubble
            }
            else if (line.StartsWith("AbilityCheck:"))
            {
                // find SO_AC by path
                // put in Ability Check
            }
            else if (line.StartsWith("Win:"))
            {
                // find SO by name of bubble
                // put in bubble after win
            }
            else if (line.StartsWith("Lose:"))
            {
                // find SO by name of bubble
                // put in bubble after loss
            }
        }

        // at the end add one more sentence with "END"

        return bubbles;
    }
}
