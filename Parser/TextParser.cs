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

        List<so_dialoguebubble> finalBubbles = ParseFile(path);

        foreach (so_dialoguebubble bubble in finalBubbles)
        {
            string assetPath = $"Assets/Parser/SO_DB/{bubble.name}.asset";
            AssetDatabase.CreateAsset(bubble, assetPath);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Dialogue parsing and asset creation completed!");

        FillNextBubbles(path);
    }

    private static List<so_dialoguebubble> ParseFile(string path)
    {
        List<so_dialoguebubble> bubbles = new List<so_dialoguebubble>();
        string[] lines = File.ReadAllLines(path);

        int numberOfSentences = 0;

        foreach (string line in lines)
        {
            if (line.StartsWith("#"))
            {
                // reset number of lines for new bubble
                numberOfSentences = 0;

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
                // get sprite name
                string[] spritePathParts = spritePath.Split("/");
                string spriteName = spritePathParts[spritePathParts.Length - 1].Trim();
                // get the image file path
                string imagePath = "";
                for (int i = 0; i < spritePathParts.Length - 1; i++)
                {
                    imagePath += spritePathParts[i];
                    if (i + 2 < spritePathParts.Length)
                    {
                        imagePath += "/";
                    }
                }
                // load the sprite
                Sprite[] sprites = AssetDatabase
                    .LoadAllAssetRepresentationsAtPath(imagePath)
                    .OfType<Sprite>()
                    .ToArray();
                Sprite sprite = sprites.FirstOrDefault(s => s.name == spriteName);
                // assign bubble to every line
                for (int i = 0; i < numberOfSentences; i++)
                {
                    if (bubbles[bubbles.Count - 1].bubbles == null)
                    {
                        bubbles[bubbles.Count - 1].bubbles = new Sprite[0];
                    }
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
                numberOfSentences++;

                // assign it to sentences[]
                string dashRemoved = line.Substring(3).Trim();
                string actualDialogue;
                if (dashRemoved.Contains("*"))
                {
                    actualDialogue = dashRemoved.Split('*')[0];
                }
                else
                {
                    actualDialogue = dashRemoved;
                }
                if (bubbles[bubbles.Count - 1].sentences == null)
                {
                    bubbles[bubbles.Count - 1].sentences = new string[0];
                }
                string[] sentences = bubbles[bubbles.Count - 1].sentences;
                Array.Resize(ref sentences, sentences.Length + 1);
                sentences[sentences.Length - 1] = actualDialogue;
                bubbles[bubbles.Count - 1].sentences = sentences;

                // requirements
                if (actualDialogue.Contains("*"))
                {
                    string requirement = actualDialogue.Split("*")[1].Trim();
                    if (bubbles[bubbles.Count - 1].requirements == null)
                    {
                        bubbles[bubbles.Count - 1].requirements = new string[0];
                    }
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
                    if (bubbles[bubbles.Count - 1].images == null)
                    {
                        bubbles[bubbles.Count - 1].images = new Sprite[0];
                    }
                    Sprite[] localArray = bubbles[bubbles.Count - 1].images;
                    Array.Resize(ref localArray, localArray.Length + 1);
                    bubbles[bubbles.Count - 1].images = localArray;
                }
                // get sprite path
                string spritePath = line.Split(":")[1].Trim();
                // get sprite name
                string[] spritePathParts = spritePath.Split("/");
                string spriteName = spritePathParts[spritePathParts.Length - 1].Trim();
                // get the image file path
                string imagePath = "";
                for (int i = 0; i < spritePathParts.Length - 1; i++)
                {
                    imagePath += spritePathParts[i];
                    if (i + 2 < spritePathParts.Length)
                    {
                        imagePath += "/";
                    }
                }
                // load the sprite
                Sprite[] sprites = AssetDatabase
                    .LoadAllAssetRepresentationsAtPath(imagePath)
                    .OfType<Sprite>()
                    .ToArray();
                Sprite sprite = sprites.FirstOrDefault(s => s.name == spriteName);
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
                for (int i = 0; i < consequenceNames.Length; i++)
                {
                    if (bubbles[bubbles.Count - 1].consequencesRefs == null)
                    {
                        bubbles[bubbles.Count - 1].consequencesRefs = new string[0];
                    }
                    Array.Resize(
                        ref bubbles[bubbles.Count - 1].consequencesRefs,
                        consequenceNames.Length
                    );
                    bubbles[bubbles.Count - 1].consequencesRefs[i] = consequenceNames[i];
                }
                // make consequences not null
                if (bubbles[bubbles.Count - 1].consequences == null)
                {
                    bubbles[bubbles.Count - 1].consequences = new so_dialoguebubble[
                        consequenceNames.Length
                    ];
                }
            }
            else if (line.StartsWith("Labels:"))
            {
                // put in labels[]
                string labelCheck = line.Split(':')[1].Trim();
                string[] labels = new string[0];
                if (labelCheck != "")
                {
                    labels = line.Substring(8).Trim().Split(',');
                    bubbles[bubbles.Count - 1].labels = labels;
                }
            }
            else if (line.StartsWith("NextBubble:"))
            {
                string nextBubbleName = line.Split(":")[1].Trim();
                bubbles[bubbles.Count - 1].noChoiceNextDialogueRef = nextBubbleName;
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
                string winBubbleName = line.Split(":")[1].Trim();
                bubbles[bubbles.Count - 1].abilityCheckSuccessPathRef = winBubbleName;
            }
            else if (line.StartsWith("Lose:"))
            {
                string loseBubbleName = line.Split(":")[1].Trim();
                bubbles[bubbles.Count - 1].abilityCheckFailurePathRef = loseBubbleName;
            }
            else if (line.StartsWith("---"))
            {
                // at the end add one more sentence with "END"
                string[] sentences = bubbles[bubbles.Count - 1].sentences;
                Array.Resize(ref sentences, sentences.Length + 1);
                sentences[sentences.Length - 1] = "END";
                bubbles[bubbles.Count - 1].sentences = sentences;
            }
        }

        return bubbles;
    }

    private static void FillNextBubbles(string path)
    {
        // Define the folder path where your assets are stored
        string folderPath = "Assets/Parser/SO_DB";
        // Find all asset paths in the specified folder that are of type so_dialoguebubble
        string[] assetPaths = AssetDatabase.FindAssets("", new[] { folderPath });
        Debug.Log($"Found {assetPaths.Length} so_dialoguebubble assets.");
        // Convert the asset paths to so_dialoguebubble objects
        so_dialoguebubble[] realBubbles = assetPaths
            .Select(guid =>
                AssetDatabase.LoadAssetAtPath<so_dialoguebubble>(
                    AssetDatabase.GUIDToAssetPath(guid)
                )
            )
            .ToArray();

        // Check if bubbles were loaded
        Debug.Log($"Loaded {realBubbles.Length} so_dialoguebubble assets.");

        foreach (var realBubble in realBubbles)
        {
            Debug.Log(realBubble.name);
        }

        // so... what is the algorithm here?
        // I have an array of bubbles already created
        // The properties to be filled are: Consequences, Next, Win and Lose
        // 1) I have to run the path through the parser again, to create an array of imaginary bubbles.
        List<so_dialoguebubble> imaginaryBubbles = ParseFile(path);
        // 2) In FileParser, I just put the text value of the bubbles name in Consequences, Next, Win and Lose
        // 3) Then I have pairs of real = imaginary
        // 4) For each pair, if each of these four fields are not empty, I find the bubble by name from IMAGINARY array in the REAL array
        // and change the REAL one's data
        for (int i = 0; i < realBubbles.Length; i++)
        {
            for (int j = 0; j < imaginaryBubbles[i].consequencesRefs.Length; j++)
            {
                var nextBubble = realBubbles.FirstOrDefault(b =>
                    b.name == imaginaryBubbles[i].consequencesRefs[j]
                );
                if (nextBubble != null)
                {
                    realBubbles[i].consequences[j] = nextBubble; // Update reference
                }
            }
            if (!string.IsNullOrEmpty(realBubbles[i].noChoiceNextDialogueRef))
            {
                var nextBubble = realBubbles.FirstOrDefault(b =>
                    b.name == imaginaryBubbles[i].noChoiceNextDialogueRef
                );
                if (nextBubble != null)
                {
                    realBubbles[i].noChoiceNextDialogue = nextBubble; // Update reference
                }
            }

            if (!string.IsNullOrEmpty(realBubbles[i].abilityCheckSuccessPathRef))
            {
                var successBubble = realBubbles.FirstOrDefault(b =>
                    b.name == imaginaryBubbles[i].abilityCheckSuccessPathRef
                );
                if (successBubble != null)
                {
                    realBubbles[i].abilityCheckSuccessPath = successBubble; // Update reference
                }
            }

            if (!string.IsNullOrEmpty(realBubbles[i].abilityCheckFailurePathRef))
            {
                var failureBubble = realBubbles.FirstOrDefault(b =>
                    b.name == imaginaryBubbles[i].abilityCheckFailurePathRef
                );
                if (failureBubble != null)
                {
                    realBubbles[i].abilityCheckFailurePath = failureBubble; // Update reference
                }
            }

            // Mark the asset as dirty to ensure changes are saved
            EditorUtility.SetDirty(realBubbles[i]);
        }

        // Save all changes to the assets
        AssetDatabase.SaveAssets();
    }
}
