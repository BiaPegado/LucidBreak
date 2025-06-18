using UnityEngine;
using UnityEngine.UI;

public class DialogueCleanup : MonoBehaviour
{
    [ContextMenu("Cleanup Duplicate Dialogue Panels")]
    public void CleanupDuplicateDialoguePanels()
    {
        // Find all DialoguePanels
        GameObject[] dialoguePanels = GameObject.FindGameObjectsWithTag("DialoguePanel");
        
        if (dialoguePanels.Length > 1)
        {
            Debug.Log($"Found {dialoguePanels.Length} DialoguePanels. Keeping the first one, destroying the rest...");
            
            // Keep the first one, destroy the rest
            for (int i = 1; i < dialoguePanels.Length; i++)
            {
                Debug.Log($"Destroying duplicate DialoguePanel: {dialoguePanels[i].name}");
                DestroyImmediate(dialoguePanels[i]);
            }
        }
        else if (dialoguePanels.Length == 1)
        {
            Debug.Log("Found 1 DialoguePanel. No cleanup needed.");
        }
        else
        {
            Debug.Log("No DialoguePanels found.");
        }
        
        // Also check for DialogueManagers
        DialogueManager[] managers = FindObjectsOfType<DialogueManager>();
        if (managers.Length > 1)
        {
            Debug.Log($"Found {managers.Length} DialogueManagers. Keeping the first one, destroying the rest...");
            
            for (int i = 1; i < managers.Length; i++)
            {
                Debug.Log($"Destroying duplicate DialogueManager: {managers[i].name}");
                DestroyImmediate(managers[i].gameObject);
            }
        }
    }
    
    [ContextMenu("Fix Dialogue Panel Colors")]
    public void FixDialoguePanelColors()
    {
        GameObject[] dialoguePanels = GameObject.FindGameObjectsWithTag("DialoguePanel");
        
        foreach (GameObject panel in dialoguePanels)
        {
            // Fix background color
            Image background = panel.transform.Find("Background")?.GetComponent<Image>();
            if (background != null)
            {
                background.color = new Color(0, 0, 0, 0.8f); // Preto transparente
                Debug.Log($"Fixed background color for {panel.name}");
            }
            
            // Fix border color
            Image border = panel.transform.Find("Border")?.GetComponent<Image>();
            if (border != null)
            {
                border.color = Color.white; // Branco
                Debug.Log($"Fixed border color for {panel.name}");
            }
            
            // Fix text color
            TMPro.TextMeshProUGUI text = panel.transform.Find("DialogueText")?.GetComponent<TMPro.TextMeshProUGUI>();
            if (text != null)
            {
                text.color = Color.white; // Branco
                Debug.Log($"Fixed text color for {panel.name}");
            }
        }
    }
    
    [ContextMenu("Reset Dialogue System")]
    public void ResetDialogueSystem()
    {
        // Cleanup duplicates
        CleanupDuplicateDialoguePanels();
        
        // Fix colors
        FixDialoguePanelColors();
        
        Debug.Log("Dialogue system reset complete!");
    }
}