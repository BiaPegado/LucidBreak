using UnityEngine;
using System.Collections.Generic;

public class DialogueConditional : MonoBehaviour
{
    [System.Serializable]
    public class ConditionalDialogue
    {
        [Header("Condition")]
        public string conditionName;
        public DialogueConditionType conditionType;
        public string conditionValue;
        public int conditionIntValue;
        public float conditionFloatValue;
        public bool conditionBoolValue;
        
        [Header("Dialogue")]
        public DialogueData dialogueData;
        public bool useCustomDialogue = false;
        public string customSpeaker = "Sistema";
        public string customText = "";
        public bool useCustomFont = true;
        
        [Header("Settings")]
        public bool triggerOnce = true;
        public float delayBeforeTrigger = 0f;
        public bool checkOnStart = false;
        
        [Header("Advanced")]
        public string[] requiredItems;
        public string[] requiredFlags;
        public int requiredLevel = 0;
    }
    
    public enum DialogueConditionType
    {
        None,
        PlayerLevel,
        ItemCollected,
        FlagSet,
        TimeOfDay,
        Location,
        Health,
        Score,
        Custom
    }
    
    [Header("Conditional Dialogues")]
    public List<ConditionalDialogue> conditionalDialogues = new List<ConditionalDialogue>();
    
    [Header("Global Settings")]
    public bool checkConditionsOnUpdate = false;
    public float checkInterval = 1f;
    public bool debugMode = false;
    
    private Dictionary<string, bool> triggeredDialogues = new Dictionary<string, bool>();
    private float lastCheckTime;
    private DialogueBuilder dialogueBuilder;
    
    private void Start()
    {
        dialogueBuilder = GetComponent<DialogueBuilder>();
        if (dialogueBuilder == null)
            dialogueBuilder = gameObject.AddComponent<DialogueBuilder>();
            
        // Check conditions that should trigger on start
        foreach (var dialogue in conditionalDialogues)
        {
            if (dialogue.checkOnStart)
            {
                CheckAndTriggerDialogue(dialogue);
            }
        }
    }
    
    private void Update()
    {
        if (checkConditionsOnUpdate && Time.time - lastCheckTime >= checkInterval)
        {
            lastCheckTime = Time.time;
            CheckAllConditions();
        }
    }
    
    public void CheckAllConditions()
    {
        foreach (var dialogue in conditionalDialogues)
        {
            CheckAndTriggerDialogue(dialogue);
        }
    }
    
    public void CheckAndTriggerDialogue(ConditionalDialogue dialogue)
    {
        // Check if already triggered
        if (dialogue.triggerOnce && triggeredDialogues.ContainsKey(dialogue.conditionName))
        {
            return;
        }
        
        // Check condition
        if (CheckCondition(dialogue))
        {
            // Mark as triggered
            if (dialogue.triggerOnce)
            {
                triggeredDialogues[dialogue.conditionName] = true;
            }
            
            // Trigger dialogue with delay
            if (dialogue.delayBeforeTrigger > 0)
            {
                StartCoroutine(TriggerDialogueWithDelay(dialogue, dialogue.delayBeforeTrigger));
            }
            else
            {
                TriggerDialogue(dialogue);
            }
        }
    }
    
    private bool CheckCondition(ConditionalDialogue dialogue)
    {
        switch (dialogue.conditionType)
        {
            case DialogueConditionType.PlayerLevel:
                return CheckPlayerLevel(dialogue);
                
            case DialogueConditionType.ItemCollected:
                return CheckItemCollected(dialogue);
                
            case DialogueConditionType.FlagSet:
                return CheckFlagSet(dialogue);
                
            case DialogueConditionType.TimeOfDay:
                return CheckTimeOfDay(dialogue);
                
            case DialogueConditionType.Location:
                return CheckLocation(dialogue);
                
            case DialogueConditionType.Health:
                return CheckHealth(dialogue);
                
            case DialogueConditionType.Score:
                return CheckScore(dialogue);
                
            case DialogueConditionType.Custom:
                return CheckCustomCondition(dialogue);
                
            default:
                return false;
        }
    }
    
    private bool CheckPlayerLevel(ConditionalDialogue dialogue)
    {
        // Example: Check player level
        // You can modify this to match your game's level system
        int playerLevel = GetPlayerLevel();
        return playerLevel >= dialogue.conditionIntValue;
    }
    
    private bool CheckItemCollected(ConditionalDialogue dialogue)
    {
        // Check if specific item is collected
        if (dialogue.requiredItems != null && dialogue.requiredItems.Length > 0)
        {
            foreach (string itemName in dialogue.requiredItems)
            {
                if (!IsItemCollected(itemName))
                    return false;
            }
        }
        return true;
    }
    
    private bool CheckFlagSet(ConditionalDialogue dialogue)
    {
        // Check if specific flags are set
        if (dialogue.requiredFlags != null && dialogue.requiredFlags.Length > 0)
        {
            foreach (string flagName in dialogue.requiredFlags)
            {
                if (!IsFlagSet(flagName))
                    return false;
            }
        }
        return true;
    }
    
    private bool CheckTimeOfDay(ConditionalDialogue dialogue)
    {
        // Check time of day (example: 0-24 hour format)
        float currentHour = GetCurrentHour();
        return currentHour >= dialogue.conditionFloatValue;
    }
    
    private bool CheckLocation(ConditionalDialogue dialogue)
    {
        // Check if player is in specific location
        string currentLocation = GetCurrentLocation();
        return currentLocation == dialogue.conditionValue;
    }
    
    private bool CheckHealth(ConditionalDialogue dialogue)
    {
        // Check player health
        float currentHealth = GetPlayerHealth();
        return currentHealth <= dialogue.conditionFloatValue;
    }
    
    private bool CheckScore(ConditionalDialogue dialogue)
    {
        // Check player score
        int currentScore = GetPlayerScore();
        return currentScore >= dialogue.conditionIntValue;
    }
    
    private bool CheckCustomCondition(ConditionalDialogue dialogue)
    {
        // Custom condition - implement based on your game's needs
        return dialogue.conditionBoolValue;
    }
    
    private void TriggerDialogue(ConditionalDialogue dialogue)
    {
        if (dialogue.useCustomDialogue)
        {
            // Create custom dialogue
            dialogueBuilder.ClearDialogueLines();
            dialogueBuilder.AddDialogueLine(dialogue.customText, dialogue.useCustomFont);
            dialogueBuilder.BuildAndStartDialogue();
        }
        else if (dialogue.dialogueData != null)
        {
            // Use pre-made dialogue
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.StartDialogue(dialogue.dialogueData);
            }
        }
        
        if (debugMode)
        {
            Debug.Log($"Triggered dialogue: {dialogue.conditionName}");
        }
    }
    
    private System.Collections.IEnumerator TriggerDialogueWithDelay(ConditionalDialogue dialogue, float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerDialogue(dialogue);
    }
    
    // Helper methods - implement these based on your game's systems
    private int GetPlayerLevel()
    {
        // Implement based on your level system
        return 1;
    }
    
    private bool IsItemCollected(string itemName)
    {
        // Implement based on your inventory system
        return false;
    }
    
    private bool IsFlagSet(string flagName)
    {
        // Implement based on your flag system
        return false;
    }
    
    private float GetCurrentHour()
    {
        // Implement based on your time system
        return System.DateTime.Now.Hour;
    }
    
    private string GetCurrentLocation()
    {
        // Implement based on your location system
        return "Default";
    }
    
    private float GetPlayerHealth()
    {
        // Implement based on your health system
        return 100f;
    }
    
    private int GetPlayerScore()
    {
        // Implement based on your score system
        return 0;
    }
    
    // Public methods for external control
    public void ResetTriggeredDialogues()
    {
        triggeredDialogues.Clear();
    }
    
    public void ResetSpecificDialogue(string conditionName)
    {
        if (triggeredDialogues.ContainsKey(conditionName))
        {
            triggeredDialogues.Remove(conditionName);
        }
    }
    
    public void AddConditionalDialogue(ConditionalDialogue dialogue)
    {
        conditionalDialogues.Add(dialogue);
    }
    
    public void RemoveConditionalDialogue(int index)
    {
        if (index >= 0 && index < conditionalDialogues.Count)
        {
            conditionalDialogues.RemoveAt(index);
        }
    }
    
    // Method to manually trigger a specific dialogue
    public void TriggerDialogueByName(string conditionName)
    {
        foreach (var dialogue in conditionalDialogues)
        {
            if (dialogue.conditionName == conditionName)
            {
                TriggerDialogue(dialogue);
                break;
            }
        }
    }
} 