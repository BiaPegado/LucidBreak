using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public Image backgroundPanel;
    public Image borderPanel;
    public TextMeshProUGUI dialogueText;
    
    [Header("Font Settings")]
    public TMP_FontAsset determinationFont;
    public TMP_FontAsset defaultFont;
    
    [Header("Audio")]
    public AudioClip typewriterSound;
    public AudioClip continueSound;
    
    [Header("Animation")]
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.3f;
    
    // Private variables
    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    private bool isDisplayingDialogue = false;
    private bool isTyping = false;
    private Coroutine typewriterCoroutine;
    private Coroutine autoAdvanceCoroutine;
    
    // Events
    public System.Action OnDialogueStart;
    public System.Action OnDialogueEnd;
    public System.Action OnLineComplete;
    
    // Singleton pattern
    public static DialogueManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Setup UI
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
    
    private void Update()
    {
        // Handle input for dialogue
        if (isDisplayingDialogue)
        {
            // Continue with Enter or Space
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                ContinueDialogue();
            }
            
            // Skip with Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SkipDialogue();
            }
        }
    }
    
    public void StartDialogue(DialogueData dialogue)
    {
        if (isDisplayingDialogue)
            return;
            
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDisplayingDialogue = true;
        
        // Show dialogue panel
        dialoguePanel.SetActive(true);
        
        // Setup panel appearance
        if (backgroundPanel != null)
            backgroundPanel.color = dialogue.backgroundColor;
            
        if (borderPanel != null)
            borderPanel.color = dialogue.borderColor;
            
        // Fade in
        StartCoroutine(FadeIn());
        
        // Display first line
        DisplayCurrentLine();
        
        OnDialogueStart?.Invoke();
    }
    
    public void ContinueDialogue()
    {
        if (!isDisplayingDialogue)
            return;
            
        if (isTyping)
        {
            // Complete current line immediately
            CompleteCurrentLine();
        }
        else
        {
            // Move to next line
            currentLineIndex++;
            
            if (currentLineIndex >= currentDialogue.dialogueLines.Length)
            {
                EndDialogue();
            }
            else
            {
                DisplayCurrentLine();
            }
        }
        
        // Play continue sound
        if (continueSound != null && GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(continueSound);
    }
    
    public void SkipDialogue()
    {
        if (!isDisplayingDialogue || !currentDialogue.canSkip)
            return;
            
        EndDialogue();
    }
    
    private void DisplayCurrentLine()
    {
        if (currentLineIndex >= currentDialogue.dialogueLines.Length)
            return;
            
        var line = currentDialogue.dialogueLines[currentLineIndex];
        
        // Start typewriter effect
        StartTypewriterEffect(line);
        
        // Auto advance if enabled
        if (currentDialogue.autoAdvance)
        {
            if (autoAdvanceCoroutine != null)
                StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = StartCoroutine(AutoAdvance(line.waitTimeAfter));
        }
    }
    
    private void StartTypewriterEffect(DialogueData.DialogueLine line)
    {
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);
            
        isTyping = true;
        typewriterCoroutine = StartCoroutine(TypewriterEffect(line));
    }
    
    private IEnumerator TypewriterEffect(DialogueData.DialogueLine line)
    {
        dialogueText.text = "";
        dialogueText.font = line.useCustomFont ? determinationFont : defaultFont;
        dialogueText.color = line.textColor;
        
        string fullText = line.dialogueText;
        
        for (int i = 0; i < fullText.Length; i++)
        {
            dialogueText.text += fullText[i];
            
            // Play typewriter sound
            if (typewriterSound != null && GetComponent<AudioSource>() != null && i % 3 == 0)
                GetComponent<AudioSource>().PlayOneShot(typewriterSound);
                
            // Shake effect
            if (line.shakeText)
            {
                Vector3 originalPos = dialogueText.transform.localPosition;
                dialogueText.transform.localPosition = originalPos + Random.insideUnitSphere * 2f;
                yield return new WaitForSeconds(0.02f);
                dialogueText.transform.localPosition = originalPos;
            }
            
            yield return new WaitForSeconds(line.textSpeed);
        }
        
        CompleteCurrentLine();
    }
    
    private void CompleteCurrentLine()
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }
        
        // Show full text immediately
        var line = currentDialogue.dialogueLines[currentLineIndex];
        dialogueText.text = line.dialogueText;
        
        isTyping = false;
        OnLineComplete?.Invoke();
    }
    
    private IEnumerator AutoAdvance(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (isDisplayingDialogue && !isTyping)
        {
            ContinueDialogue();
        }
    }
    
    private void EndDialogue()
    {
        isDisplayingDialogue = false;
        
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }
        
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }
        
        // Fade out
        StartCoroutine(FadeOut());
        
        OnDialogueEnd?.Invoke();
    }
    
    private IEnumerator FadeIn()
    {
        CanvasGroup canvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = dialoguePanel.AddComponent<CanvasGroup>();
            
        canvasGroup.alpha = 0f;
        
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
    
    private IEnumerator FadeOut()
    {
        CanvasGroup canvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = dialoguePanel.AddComponent<CanvasGroup>();
            
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        dialoguePanel.SetActive(false);
    }
    
    // Public getters
    public bool IsDisplayingDialogue => isDisplayingDialogue;
    public bool IsTyping => isTyping;
    public DialogueData CurrentDialogue => currentDialogue;
    public int CurrentLineIndex => currentLineIndex;
} 