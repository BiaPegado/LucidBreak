using UnityEngine;

public class DialogueExample : MonoBehaviour
{
    [Header("Example Dialogues")]
    public DialogueData[] exampleDialogues;
    public TextAsset dialogueTextFile;
    
    [Header("Test Controls")]
    public KeyCode testKey1 = KeyCode.Alpha1;
    public KeyCode testKey2 = KeyCode.Alpha2;
    public KeyCode testKey3 = KeyCode.Alpha3;
    public KeyCode testKey4 = KeyCode.Alpha4;
    
    private DialogueBuilder dialogueBuilder;
    
    private void Start()
    {
        // Create dialogue builder for dynamic dialogues
        dialogueBuilder = gameObject.AddComponent<DialogueBuilder>();
    }
    
    private void Update()
    {
        // Test different dialogue methods
        if (Input.GetKeyDown(testKey1))
        {
            TestPreMadeDialogue();
        }
        
        if (Input.GetKeyDown(testKey2))
        {
            TestDynamicDialogue();
        }
        
        if (Input.GetKeyDown(testKey3))
        {
            TestSimpleDialogue();
        }
        
        if (Input.GetKeyDown(testKey4))
        {
            TestDialogueFromFile();
        }
    }
    
    // Test 1: Using pre-made DialogueData ScriptableObject
    private void TestPreMadeDialogue()
    {
        if (exampleDialogues != null && exampleDialogues.Length > 0)
        {
            DialogueData randomDialogue = exampleDialogues[Random.Range(0, exampleDialogues.Length)];
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.StartDialogue(randomDialogue);
            }
        }
        else
        {
            Debug.Log("No example dialogues assigned!");
        }
    }
    
    // Test 2: Building dialogue dynamically
    private void TestDynamicDialogue()
    {
        dialogueBuilder.ClearDialogueLines();
        
        // Add dialogue lines with different settings
        dialogueBuilder.AddDialogueLine("Bem-vindo ao mundo de LucidBreak!", true); // Use custom font
        dialogueBuilder.AddDialogueLine("Este é um exemplo de diálogo dinâmico.", false);
        dialogueBuilder.AddDialogueLine("Você pode criar diálogos em tempo de execução!", true);
        
        // Build and start dialogue
        dialogueBuilder.BuildAndStartDialogue();
    }
    
    // Test 3: Simple dialogue with one line
    private void TestSimpleDialogue()
    {
        DialogueData simpleDialogue = DialogueBuilder.CreateSimpleDialogue(
            "Este é um diálogo simples criado dinamicamente!",
            true
        );
        
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(simpleDialogue);
        }
    }
    
    // Test 4: Dialogue from text file
    private void TestDialogueFromFile()
    {
        if (dialogueTextFile != null)
        {
            dialogueBuilder.BuildFromTextFile(dialogueTextFile, true);
            dialogueBuilder.BuildAndStartDialogue();
        }
        else
        {
            Debug.Log("No text file assigned!");
        }
    }
    
    // Example method for creating dialogue from string array
    public void CreateDialogueFromStrings(string[] lines)
    {
        DialogueData dialogue = DialogueBuilder.CreateDialogueFromLines(lines, true);
        
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
    
    // Example method for creating dialogue with custom settings
    public void CreateCustomDialogue()
    {
        dialogueBuilder.ClearDialogueLines();
        
        // Add lines with custom settings
        dialogueBuilder.AddDialogueLine("Olá! Como você está?", 0.03f, false, Color.white, true);
        dialogueBuilder.AddDialogueLine("Estou bem, obrigado!", 0.05f, false, Color.cyan, false);
        dialogueBuilder.AddDialogueLine("Que ótimo! *sorri*", 0.04f, true, Color.yellow, true);
        
        // Customize dialogue settings
        dialogueBuilder.dialogueID = "CustomDialogue";
        dialogueBuilder.backgroundColor = new Color(0.1f, 0.1f, 0.3f, 0.9f);
        dialogueBuilder.borderColor = Color.cyan;
        
        dialogueBuilder.BuildAndStartDialogue();
    }
    
    // Example method for creating dialogue with effects
    public void CreateDramaticDialogue()
    {
        dialogueBuilder.ClearDialogueLines();
        
        dialogueBuilder.AddDialogueLine("ALGO DRAMÁTICO ACONTECEU!", 0.08f, true, Color.red, true);
        dialogueBuilder.AddDialogueLine("O mundo nunca mais será o mesmo...", 0.05f, false, Color.white, true);
        
        dialogueBuilder.BuildAndStartDialogue();
    }
} 