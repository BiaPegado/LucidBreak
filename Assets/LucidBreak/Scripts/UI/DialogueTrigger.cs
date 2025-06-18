using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueData dialogueData;
    public bool triggerOnStart = false;
    public bool triggerOnTrigger = false;
    public bool triggerOnInteract = true;
    public string interactKey = "E";
    
    [Header("Trigger Settings")]
    public float interactionRange = 2f;
    public LayerMask playerLayer = 1;
    public bool showInteractionPrompt = true;
    
    [Header("Events")]
    public UnityEvent OnDialogueStart;
    public UnityEvent OnDialogueEnd;
    
    [Header("UI")]
    public GameObject interactionPrompt;
    public TMPro.TextMeshProUGUI promptText;
    
    private bool playerInRange = false;
    private bool dialogueTriggered = false;
    
    private void Start()
    {
        if (triggerOnStart && dialogueData != null)
        {
            StartDialogue();
        }
        
        // Setup interaction prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
            if (promptText != null)
                promptText.text = $"Pressione {interactKey} para interagir";
        }
    }
    
    private void Update()
    {
        if (triggerOnInteract && playerInRange && !dialogueTriggered)
        {
            if (Input.GetKeyDown(interactKey))
            {
                StartDialogue();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnTrigger && other.CompareTag("Player"))
        {
            StartDialogue();
        }
        
        if (triggerOnInteract && other.CompareTag("Player"))
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
        
        OnDialogueStart?.Invoke();
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
            
        OnDialogueEnd?.Invoke();
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
        if (triggerOnInteract)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
} 