using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSetup : MonoBehaviour
{
    [Header("Setup Mode")]
    public bool autoSetup = true;
    public bool createUIElements = true;
    public bool setupInputHandler = true;
    
    [Header("UI References")]
    public Canvas targetCanvas;
    public GameObject dialoguePanelPrefab;
    
    [Header("Font Settings")]
    public TMP_FontAsset determinationFont;
    public TMP_FontAsset defaultFont;
    
    [Header("Audio")]
    public AudioClip typewriterSound;
    public AudioClip continueSound;
    
    private void Start()
    {
        if (autoSetup)
        {
            SetupDialogueSystem();
        }
    }
    
    [ContextMenu("Setup Dialogue System")]
    public void SetupDialogueSystem()
    {
        Debug.Log("Setting up Dialogue System...");
        
        // Find or create DialogueManager
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            GameObject managerGO = new GameObject("DialogueManager");
            dialogueManager = managerGO.AddComponent<DialogueManager>();
            Debug.Log("Created DialogueManager");
        }
        
        // Create UI elements if needed
        if (createUIElements)
        {
            CreateDialogueUI(dialogueManager);
        }
        
        // Setup fonts
        if (determinationFont != null)
            dialogueManager.determinationFont = determinationFont;
        if (defaultFont != null)
            dialogueManager.defaultFont = defaultFont;
            
        // Setup audio
        if (typewriterSound != null)
            dialogueManager.typewriterSound = typewriterSound;
        if (continueSound != null)
            dialogueManager.continueSound = continueSound;
            
        // Setup input handler
        if (setupInputHandler)
        {
            SetupInputHandler();
        }
        
        Debug.Log("Dialogue System setup complete!");
    }
    
    private void CreateDialogueUI(DialogueManager dialogueManager)
    {
        // Check if dialogue panel already exists
        if (dialogueManager.dialoguePanel != null)
        {
            Debug.Log("DialoguePanel already exists, skipping creation");
            return;
        }

        // Find canvas
        Canvas canvas = targetCanvas;
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasGO = new GameObject("DialogueCanvas");
                canvas = canvasGO.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGO.AddComponent<CanvasScaler>();
                canvasGO.AddComponent<GraphicRaycaster>();
                Debug.Log("Created new Canvas for dialogue UI");
            }
        }
        
        // Create dialogue panel
        GameObject dialoguePanel = new GameObject("DialoguePanel");
        dialoguePanel.transform.SetParent(canvas.transform, false);
        
        // Add RectTransform
        RectTransform panelRect = dialoguePanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 0.3f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Add background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(dialoguePanel.transform, false);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.8f); // Fundo preto transparente
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Add border
        GameObject border = new GameObject("Border");
        border.transform.SetParent(dialoguePanel.transform, false);
        Image borderImage = border.AddComponent<Image>();
        borderImage.color = Color.white; // Borda branca
        RectTransform borderRect = border.GetComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = new Vector2(-2, -2);
        borderRect.offsetMax = new Vector2(2, 2);
        
        // Add dialogue text
        GameObject dialogueTextGO = new GameObject("DialogueText");
        dialogueTextGO.transform.SetParent(dialoguePanel.transform, false);
        TextMeshProUGUI dialogueText = dialogueTextGO.AddComponent<TextMeshProUGUI>();
        dialogueText.text = "Dialogue text will appear here...";
        dialogueText.fontSize = 20;
        dialogueText.color = Color.white; // Texto branco
        dialogueText.alignment = TextAlignmentOptions.TopLeft;
        dialogueText.enableWordWrapping = true;
        RectTransform textRect = dialogueTextGO.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0.1f);
        textRect.anchorMax = new Vector2(1, 0.9f);
        textRect.offsetMin = new Vector2(20, 0);
        textRect.offsetMax = new Vector2(-20, 0);
        
        // Assign references to DialogueManager
        dialogueManager.dialoguePanel = dialoguePanel;
        dialogueManager.backgroundPanel = bgImage;
        dialogueManager.borderPanel = borderImage;
        dialogueManager.dialogueText = dialogueText;
        
        // Add AudioSource if not exists
        if (dialogueManager.GetComponent<AudioSource>() == null)
        {
            AudioSource audioSource = dialogueManager.gameObject.AddComponent<AudioSource>();
        }
        
        Debug.Log("Created dialogue UI elements");
    }
    
    private void SetupInputHandler()
    {
        DialogueInputHandler inputHandler = FindObjectOfType<DialogueInputHandler>();
        if (inputHandler == null)
        {
            GameObject inputHandlerGO = new GameObject("DialogueInputHandler");
            inputHandler = inputHandlerGO.AddComponent<DialogueInputHandler>();
            Debug.Log("Created DialogueInputHandler");
        }
        
        // Note: InputActionReferences need to be set up manually in the Inspector
        // based on your Input Actions asset
    }
    
    [ContextMenu("Test Dialogue System")]
    public void TestDialogueSystem()
    {
        if (DialogueManager.Instance != null)
        {
            // Create a test dialogue
            DialogueData testDialogue = DialogueBuilder.CreateSimpleDialogue(
                "Teste do sistema de diálogos funcionando!", 
                true
            );
            
            DialogueManager.Instance.StartDialogue(testDialogue);
            Debug.Log("Test dialogue started!");
        }
        else
        {
            Debug.LogError("DialogueManager not found! Run setup first.");
        }
    }
    
    [ContextMenu("Create Example Dialogue Data")]
    public void CreateExampleDialogueData()
    {
        // Create an example DialogueData asset
        DialogueData exampleDialogue = ScriptableObject.CreateInstance<DialogueData>();
        exampleDialogue.dialogueID = "ExampleDialogue";
        
        // Create dialogue lines
        exampleDialogue.dialogueLines = new DialogueData.DialogueLine[]
        {
            new DialogueData.DialogueLine
            {
                dialogueText = "Bem-vindo ao LucidBreak!",
                textSpeed = 0.06f,
                useCustomFont = true
            },
            new DialogueData.DialogueLine
            {
                dialogueText = "Este é um exemplo de diálogo simples.",
                textSpeed = 0.04f,
                useCustomFont = true
            },
            new DialogueData.DialogueLine
            {
                dialogueText = "Espero que você goste do jogo!",
                textSpeed = 0.06f,
                useCustomFont = true
            }
        };
        
        // Save the asset
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(exampleDialogue, "Assets/LucidBreak/Scripts/UI/ExampleDialogue.asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("Created example dialogue asset: Assets/LucidBreak/Scripts/UI/ExampleDialogue.asset");
        #endif
    }
} 