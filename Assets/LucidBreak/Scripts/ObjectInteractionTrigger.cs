using UnityEngine;
using UnityEngine.Events;

public class ObjectInteractionTrigger : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool triggerOnStart = false;
    public bool triggerOnTrigger = false;
    public bool triggerOnInteract = true;
    public KeyCode interactKey = KeyCode.E;

    [Header("Trigger Settings")]
    public float interactionRange = 2f;
    public LayerMask playerLayer = 1;
    public bool showInteractionPrompt = true;
    
    [Header("Object to Show/Hide")]
    [Tooltip("Objeto que será mostrado/escondido quando interagir")]
    public GameObject objectToToggle;
    [Tooltip("Se true, o objeto será mostrado. Se false, será escondido")]
    public bool showObject = true;
    [Tooltip("Se true, o objeto será escondido após a interação")]
    public bool hideAfterInteraction = false;
    [Tooltip("Tempo em segundos antes de esconder o objeto (se hideAfterInteraction for true)")]
    public float hideDelay = 3f;
    
    [Header("Dialogue (Optional)")]
    [Tooltip("Se quiser mostrar um diálogo junto com o objeto")]
    public DialogueData dialogueData;
    
    [Header("Events")]
    public UnityEvent OnInteractionStart;
    public UnityEvent OnInteractionEnd;
    
    [Header("UI")]
    public GameObject interactionPrompt;
    public TMPro.TextMeshProUGUI promptText;
    
    private bool playerInRange = false;
    private bool interactionTriggered = false;
    private bool objectCurrentlyVisible = false;
    
    private void Start()
    {
        // Configurar o objeto inicialmente escondido se showObject for true
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
            objectCurrentlyVisible = false;
        }
        
        if (triggerOnStart)
        {
            TriggerInteraction();
        }
        
        // Setup interaction prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
            if (promptText != null)
                promptText.text = $"Pressione {interactKey.ToString().Replace("Alpha", "")} para interagir";

        }
    }
    
    private void Update()
    {
        if (triggerOnInteract && playerInRange && !interactionTriggered)
        {
            if (Input.GetKeyDown(interactKey))
            {
                TriggerInteraction();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnTrigger && other.CompareTag("Player"))
        {
            TriggerInteraction();
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
    
    public void TriggerInteraction()
    {
        if (interactionTriggered)
            return;
            
        interactionTriggered = true;
        
        // Mostrar/esconder o objeto
        if (objectToToggle != null)
        {
            if (showObject && !objectCurrentlyVisible)
            {
                ShowObject();
            }
            else if (!showObject && objectCurrentlyVisible)
            {
                HideObject();
            }
        }
        
        // Iniciar diálogo se houver
        if (dialogueData != null && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart += OnDialogueStarted;
            DialogueManager.Instance.OnDialogueEnd += OnDialogueFinished;
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
        
        OnInteractionStart?.Invoke();
        
        // Esconder prompt de interação
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    private void ShowObject()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(true);
            objectCurrentlyVisible = true;
            
            // Se deve esconder após interação, agendar o escondimento
            if (hideAfterInteraction)
            {
                Invoke(nameof(HideObject), hideDelay);
            }
        }
    }
    
    private void HideObject()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
            objectCurrentlyVisible = false;
        }
    }
    
    private void OnDialogueStarted()
    {
        // Esconder prompt durante diálogo
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    private void OnDialogueFinished()
    {
        // Desinscrever dos eventos
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart -= OnDialogueStarted;
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueFinished;
        }
        
        // Resetar estado do trigger
        interactionTriggered = false;
        
        // Mostrar prompt novamente se o jogador ainda estiver no alcance
        if (playerInRange && showInteractionPrompt && interactionPrompt != null)
            interactionPrompt.SetActive(true);
            
        OnInteractionEnd?.Invoke();
    }
    
    // Método público para resetar o trigger
    public void ResetTrigger()
    {
        interactionTriggered = false;
    }
    
    // Método público para mostrar o objeto manualmente
    public void ShowObjectManually()
    {
        ShowObject();
    }
    
    // Método público para esconder o objeto manualmente
    public void HideObjectManually()
    {
        HideObject();
    }
    
    // Método público para alternar a visibilidade do objeto
    public void ToggleObject()
    {
        if (objectCurrentlyVisible)
        {
            HideObject();
        }
        else
        {
            ShowObject();
        }
    }
    
    // Método público para definir novo objeto
    public void SetObjectToToggle(GameObject newObject)
    {
        objectToToggle = newObject;
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
            objectCurrentlyVisible = false;
        }
    }
    
    // Desenhar alcance de interação no editor
    private void OnDrawGizmosSelected()
    {
        if (triggerOnInteract)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
        
        // Desenhar linha para o objeto se houver
        if (objectToToggle != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, objectToToggle.transform.position);
        }
    }
} 