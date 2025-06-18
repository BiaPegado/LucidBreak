using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "LucidBreak/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        [Header("Text Content")]
        [TextArea(3, 8)]
        public string dialogueText;
        
        [Header("Timing")]
        public float textSpeed = 0.05f; // Speed of typewriter effect
        public float waitTimeAfter = 1f; // Time to wait after this line
        
        [Header("Special Effects")]
        public bool shakeText = false;
        public Color textColor = Color.white;
        public bool useCustomFont = true; // Default to Determination font
    }
    
    [Header("Dialogue Settings")]
    public string dialogueID;
    public DialogueLine[] dialogueLines;
    
    [Header("Global Settings")]
    public bool canSkip = true;
    public bool autoAdvance = false;
    public float autoAdvanceDelay = 3f;
    
    [Header("UI Settings")]
    public Color backgroundColor = new Color(0, 0, 0, 0.8f);
    public Color borderColor = Color.white;
    public Vector2 dialogueBoxSize = new Vector2(800, 200);
    public Vector2 dialogueBoxPosition = new Vector2(0, -300); // Bottom of screen
} 