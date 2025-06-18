using UnityEngine;

public class ItemDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueData dialogueData;
    public bool showInteractionPrompt = true;
    public string interactionText = "Pressione E para interagir";
    
    [Header("Interaction")]
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.E;
    
    [Header("UI")]
    public GameObject interactionPrompt;
    public TMPro.TextMeshProUGUI promptText;
    
    private bool playerInRange = false;
    private bool dialogueTriggered = false;
    
    private void Start()
    {
        // Setup interaction prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
            if (promptText != null)
                promptText.text = interactionText;
        }
    }
    
    private void Update()
    {
        if (playerInRange && !dialogueTriggered)
        {
            if (Input.GetKeyDown(interactKey))
            {
                StartDialogue();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (showInteractionPrompt && interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }
    
    public void StartDialogue()
    {
        if (dialogueData == null || dialogueTriggered)
            return;
            
        dialogueTriggered = true;
        
        // Subscribe to dialogue events
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart += OnDialogueStarted;
            DialogueManager.Instance.OnDialogueEnd += OnDialogueFinished;
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
    }
    
    private void OnDialogueStarted()
    {
        // Hide interaction prompt during dialogue
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    private void OnDialogueFinished()
    {
        // Unsubscribe from events
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart -= OnDialogueStarted;
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueFinished;
        }
        
        // Reset trigger state
        dialogueTriggered = false;
        
        // Show interaction prompt again if player is still in range
        if (playerInRange && showInteractionPrompt && interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }
    
    // Public method to reset the trigger
    public void ResetTrigger()
    {
        dialogueTriggered = false;
    }
    
    // Public method to set new dialogue data
    public void SetDialogueData(DialogueData newDialogue)
    {
        dialogueData = newDialogue;
        ResetTrigger();
    }
    
    // Draw interaction range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
} 