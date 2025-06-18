using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectAction
{
    [Tooltip("Objeto que será afetado")]
    public GameObject targetObject;
    [Tooltip("Ação a ser executada")]
    public enum ActionType { Show, Hide, Toggle }
    public ActionType actionType = ActionType.Show;
    [Tooltip("Delay antes de executar esta ação (em segundos)")]
    public float delay = 0f;
    [Tooltip("Se true, esta ação será executada apenas uma vez")]
    public bool executeOnce = false;
    [HideInInspector]
    public bool hasExecuted = false;
}

public class SequentialObjectTrigger : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool triggerOnStart = false;
    public bool triggerOnTrigger = false;
    public bool triggerOnInteract = true;
    public string interactKey = "E";
    
    [Header("Trigger Settings")]
    public float interactionRange = 2f;
    public LayerMask playerLayer = 1;
    public bool showInteractionPrompt = true;
    
    [Header("Object Actions")]
    [Tooltip("Lista de ações a serem executadas em sequência")]
    public List<ObjectAction> objectActions = new List<ObjectAction>();
    
    [Header("Dialogue (Optional)")]
    [Tooltip("Se quiser mostrar um diálogo junto com as ações")]
    public DialogueData dialogueData;
    
    [Header("Events")]
    public UnityEvent OnSequenceStart;
    public UnityEvent OnSequenceComplete;
    
    [Header("UI")]
    public GameObject interactionPrompt;
    public TMPro.TextMeshProUGUI promptText;
    
    private bool playerInRange = false;
    private bool sequenceTriggered = false;
    private bool sequenceRunning = false;
    
    private void Start()
    {
        // Configurar objetos inicialmente
        SetupInitialObjectStates();
        
        if (triggerOnStart)
        {
            TriggerSequence();
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
        if (triggerOnInteract && playerInRange && !sequenceTriggered && !sequenceRunning)
        {
            if (Input.GetKeyDown(interactKey))
            {
                TriggerSequence();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnTrigger && other.CompareTag("Player"))
        {
            TriggerSequence();
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
    
    private void SetupInitialObjectStates()
    {
        foreach (var action in objectActions)
        {
            if (action.targetObject != null)
            {
                // Configurar estado inicial baseado na primeira ação do tipo Show
                bool shouldBeVisible = false;
                foreach (var checkAction in objectActions)
                {
                    if (checkAction.targetObject == action.targetObject && checkAction.actionType == ObjectAction.ActionType.Show)
                    {
                        shouldBeVisible = true;
                        break;
                    }
                }
                action.targetObject.SetActive(shouldBeVisible);
            }
        }
    }
    
    public void TriggerSequence()
    {
        if (sequenceTriggered || sequenceRunning)
            return;
            
        sequenceTriggered = true;
        sequenceRunning = true;
        
        // Iniciar diálogo se houver
        if (dialogueData != null && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart += OnDialogueStarted;
            DialogueManager.Instance.OnDialogueEnd += OnDialogueFinished;
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
        
        OnSequenceStart?.Invoke();
        
        // Esconder prompt de interação
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
        // Iniciar sequência de ações
        StartCoroutine(ExecuteObjectActions());
    }
    
    private IEnumerator ExecuteObjectActions()
    {
        foreach (var action in objectActions)
        {
            // Verificar se a ação já foi executada (se executeOnce for true)
            if (action.executeOnce && action.hasExecuted)
                continue;
            
            // Aguardar delay se houver
            if (action.delay > 0)
            {
                yield return new WaitForSeconds(action.delay);
            }
            
            // Executar ação
            ExecuteAction(action);
            
            // Marcar como executada se executeOnce for true
            if (action.executeOnce)
            {
                action.hasExecuted = true;
            }
        }
        
        // Sequência completa
        sequenceRunning = false;
        OnSequenceComplete?.Invoke();
    }
    
    private void ExecuteAction(ObjectAction action)
    {
        if (action.targetObject == null)
            return;
            
        switch (action.actionType)
        {
            case ObjectAction.ActionType.Show:
                action.targetObject.SetActive(true);
                break;
            case ObjectAction.ActionType.Hide:
                action.targetObject.SetActive(false);
                break;
            case ObjectAction.ActionType.Toggle:
                action.targetObject.SetActive(!action.targetObject.activeSelf);
                break;
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
        sequenceTriggered = false;
        
        // Mostrar prompt novamente se o jogador ainda estiver no alcance
        if (playerInRange && showInteractionPrompt && interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }
    
    // Método público para resetar o trigger
    public void ResetTrigger()
    {
        sequenceTriggered = false;
        sequenceRunning = false;
        
        // Resetar estado de executeOnce
        foreach (var action in objectActions)
        {
            action.hasExecuted = false;
        }
    }
    
    // Método público para executar uma ação específica
    public void ExecuteSpecificAction(int actionIndex)
    {
        if (actionIndex >= 0 && actionIndex < objectActions.Count)
        {
            ExecuteAction(objectActions[actionIndex]);
        }
    }
    
    // Método público para adicionar uma nova ação
    public void AddAction(GameObject targetObject, ObjectAction.ActionType actionType, float delay = 0f)
    {
        ObjectAction newAction = new ObjectAction
        {
            targetObject = targetObject,
            actionType = actionType,
            delay = delay
        };
        objectActions.Add(newAction);
    }
    
    // Método público para limpar todas as ações
    public void ClearActions()
    {
        objectActions.Clear();
    }
    
    // Desenhar alcance de interação no editor
    private void OnDrawGizmosSelected()
    {
        if (triggerOnInteract)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
        
        // Desenhar linhas para os objetos
        foreach (var action in objectActions)
        {
            if (action.targetObject != null)
            {
                switch (action.actionType)
                {
                    case ObjectAction.ActionType.Show:
                        Gizmos.color = Color.green;
                        break;
                    case ObjectAction.ActionType.Hide:
                        Gizmos.color = Color.red;
                        break;
                    case ObjectAction.ActionType.Toggle:
                        Gizmos.color = Color.blue;
                        break;
                }
                Gizmos.DrawLine(transform.position, action.targetObject.transform.position);
            }
        }
    }
} 