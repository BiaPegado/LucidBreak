using UnityEngine;
using System.Collections.Generic;

public class DialogueBuilder : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLineBuilder
    {
        public string dialogueText;
        public float textSpeed = 0.05f;
        public float waitTimeAfter = 1f;
        public bool shakeText = false;
        public Color textColor = Color.white;
        public bool useCustomFont = true; // Default to Determination font
        
        public DialogueData.DialogueLine ToDialogueLine()
        {
            return new DialogueData.DialogueLine
            {
                dialogueText = dialogueText,
                textSpeed = textSpeed,
                waitTimeAfter = waitTimeAfter,
                shakeText = shakeText,
                textColor = textColor,
                useCustomFont = useCustomFont
            };
        }
    }
    
    [Header("Builder Settings")]
    public string dialogueID = "DynamicDialogue";
    public bool canSkip = true;
    public bool autoAdvance = false;
    public float autoAdvanceDelay = 3f;
    
    [Header("UI Settings")]
    public Color backgroundColor = new Color(0, 0, 0, 0.8f);
    public Color borderColor = Color.white;
    public Vector2 dialogueBoxSize = new Vector2(800, 200);
    public Vector2 dialogueBoxPosition = new Vector2(0, -300);
    
    [Header("Dialogue Lines")]
    public List<DialogueLineBuilder> dialogueLines = new List<DialogueLineBuilder>();
    
    // Build and start dialogue
    public void BuildAndStartDialogue()
    {
        DialogueData dialogue = BuildDialogue();
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
    
    // Build dialogue from current settings
    public DialogueData BuildDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        
        // Set basic properties
        dialogue.dialogueID = dialogueID;
        dialogue.canSkip = canSkip;
        dialogue.autoAdvance = autoAdvance;
        dialogue.autoAdvanceDelay = autoAdvanceDelay;
        dialogue.backgroundColor = backgroundColor;
        dialogue.borderColor = borderColor;
        dialogue.dialogueBoxSize = dialogueBoxSize;
        dialogue.dialogueBoxPosition = dialogueBoxPosition;
        
        // Convert dialogue lines
        dialogue.dialogueLines = new DialogueData.DialogueLine[dialogueLines.Count];
        for (int i = 0; i < dialogueLines.Count; i++)
        {
            dialogue.dialogueLines[i] = dialogueLines[i].ToDialogueLine();
        }
        
        return dialogue;
    }
    
    // Add a dialogue line
    public void AddDialogueLine(string text, bool useCustomFont = true)
    {
        dialogueLines.Add(new DialogueLineBuilder
        {
            dialogueText = text,
            useCustomFont = useCustomFont
        });
    }
    
    // Add a dialogue line with more options
    public void AddDialogueLine(string text, float speed = 0.05f, bool shake = false, Color color = default, bool useCustomFont = true)
    {
        if (color == default) color = Color.white;
        
        dialogueLines.Add(new DialogueLineBuilder
        {
            dialogueText = text,
            textSpeed = speed,
            shakeText = shake,
            textColor = color,
            useCustomFont = useCustomFont
        });
    }
    
    // Clear all dialogue lines
    public void ClearDialogueLines()
    {
        dialogueLines.Clear();
    }
    
    // Remove dialogue line at index
    public void RemoveDialogueLine(int index)
    {
        if (index >= 0 && index < dialogueLines.Count)
        {
            dialogueLines.RemoveAt(index);
        }
    }
    
    // Insert dialogue line at specific index
    public void InsertDialogueLine(int index, string text, bool useCustomFont = true)
    {
        if (index >= 0 && index <= dialogueLines.Count)
        {
            dialogueLines.Insert(index, new DialogueLineBuilder
            {
                dialogueText = text,
                useCustomFont = useCustomFont
            });
        }
    }
    
    // Build dialogue from string array (simple format)
    public void BuildFromStringArray(string[] lines, bool useCustomFont = true)
    {
        ClearDialogueLines();
        
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                AddDialogueLine(line.Trim(), useCustomFont);
            }
        }
    }
    
    // Build dialogue from text file
    public void BuildFromTextFile(TextAsset textFile, bool useCustomFont = true)
    {
        if (textFile != null)
        {
            string[] lines = textFile.text.Split('\n');
            BuildFromStringArray(lines, useCustomFont);
        }
    }
    
    // Create a simple dialogue with one line
    public static DialogueData CreateSimpleDialogue(string text, bool useCustomFont = true)
    {
        DialogueBuilder builder = new GameObject("TempDialogueBuilder").AddComponent<DialogueBuilder>();
        builder.AddDialogueLine(text, useCustomFont);
        DialogueData dialogue = builder.BuildDialogue();
        DestroyImmediate(builder.gameObject);
        return dialogue;
    }
    
    // Create a dialogue from multiple lines
    public static DialogueData CreateDialogueFromLines(string[] lines, bool useCustomFont = true)
    {
        DialogueBuilder builder = new GameObject("TempDialogueBuilder").AddComponent<DialogueBuilder>();
        builder.BuildFromStringArray(lines, useCustomFont);
        DialogueData dialogue = builder.BuildDialogue();
        DestroyImmediate(builder.gameObject);
        return dialogue;
    }
} 