using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference continueAction;
    public InputActionReference skipAction;
    public InputActionReference interactAction;
    
    [Header("Settings")]
    public bool disablePlayerInputDuringDialogue = true;
    public bool enableDialogueInputOnly = true;
    
    private PlayerInput playerInput;
    private DialogueManager dialogueManager;
    
    private void Start()
    {
        // Get references
        dialogueManager = DialogueManager.Instance;
        playerInput = FindObjectOfType<PlayerInput>();
        
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in scene!");
            return;
        }
        
        // Subscribe to dialogue events
        dialogueManager.OnDialogueStart += OnDialogueStarted;
        dialogueManager.OnDialogueEnd += OnDialogueEnded;
        
        // Setup input actions
        SetupInputActions();
    }
    
    private void SetupInputActions()
    {
        // Continue dialogue action
        if (continueAction != null)
        {
            continueAction.action.performed += OnContinuePressed;
        }
        
        // Skip dialogue action
        if (skipAction != null)
        {
            skipAction.action.performed += OnSkipPressed;
        }
        
        // Interact action (for triggers)
        if (interactAction != null)
        {
            interactAction.action.performed += OnInteractPressed;
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueStart -= OnDialogueStarted;
            dialogueManager.OnDialogueEnd -= OnDialogueEnded;
        }
        
        // Disable input actions
        if (continueAction != null)
            continueAction.action.performed -= OnContinuePressed;
        if (skipAction != null)
            skipAction.action.performed -= OnSkipPressed;
        if (interactAction != null)
            interactAction.action.performed -= OnInteractPressed;
    }
    
    private void OnDialogueStarted()
    {
        if (disablePlayerInputDuringDialogue && playerInput != null)
        {
            playerInput.enabled = false;
        }
        
        // Enable dialogue input actions
        if (continueAction != null)
            continueAction.action.Enable();
        if (skipAction != null)
            skipAction.action.Enable();
    }
    
    private void OnDialogueEnded()
    {
        if (disablePlayerInputDuringDialogue && playerInput != null)
        {
            playerInput.enabled = true;
        }
        
        // Disable dialogue input actions
        if (continueAction != null)
            continueAction.action.Disable();
        if (skipAction != null)
            skipAction.action.Disable();
    }
    
    private void OnContinuePressed(InputAction.CallbackContext context)
    {
        if (dialogueManager != null && dialogueManager.IsDisplayingDialogue)
        {
            dialogueManager.ContinueDialogue();
        }
    }
    
    private void OnSkipPressed(InputAction.CallbackContext context)
    {
        if (dialogueManager != null && dialogueManager.IsDisplayingDialogue)
        {
            dialogueManager.SkipDialogue();
        }
    }
    
    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        // This can be used for dialogue triggers
        // The DialogueTrigger component handles its own interaction
    }
    
    // Public methods for manual input handling
    public void ContinueDialogue()
    {
        if (dialogueManager != null && dialogueManager.IsDisplayingDialogue)
        {
            dialogueManager.ContinueDialogue();
        }
    }
    
    public void SkipDialogue()
    {
        if (dialogueManager != null && dialogueManager.IsDisplayingDialogue)
        {
            dialogueManager.SkipDialogue();
        }
    }
    
    // Method to check if dialogue is active
    public bool IsDialogueActive()
    {
        return dialogueManager != null && dialogueManager.IsDisplayingDialogue;
    }
    
    // Method to check if dialogue is typing
    public bool IsDialogueTyping()
    {
        return dialogueManager != null && dialogueManager.IsTyping;
    }
} 