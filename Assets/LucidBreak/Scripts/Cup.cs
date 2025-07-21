using UnityEditor;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public int interacted = 0;
    private bool broken = false;
    private Animator anim;
    public DialogueData dialogue;
    public DialogueData.DialogueLine novaLinha;
    public Item glassShard;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (dialogue.dialogueLines.Count > 1)
            dialogue.dialogueLines.RemoveAt(1);
        dialogue.dialogueLines[0].dialogueText = "Meu Deus quase derrubei!";

        dialogue.autoAdvance = true;
        dialogue.autoAdvanceDelay = 2f;

        novaLinha = new DialogueData.DialogueLine
        {
            dialogueText = "Eu deveria limpar isso...",
            textSpeed = 0.05f,
            waitTimeAfter = 1f,
            shakeText = false,
            textColor = Color.white,
            useCustomFont = true

        };
    }

    void Update()
    {
        if (interacted > 2 && !broken)
        {
            StartCoroutine(BreakCupAfterDelay());
        }
    }

    private System.Collections.IEnumerator BreakCupAfterDelay()
    {
        broken = true;

        Vector3 spawnLocation = new Vector3(-2.96f, 1.15f, 0f);
        anim.SetTrigger("falling");
        dialogue.dialogueLines[0].dialogueText = "Parabéns...";
        dialogue.dialogueLines.Add(novaLinha);

        Debug.Log("breaking cup...");

        yield return new WaitForSeconds(2f);

        Item droppedItem = Instantiate(glassShard, spawnLocation, Quaternion.identity);
        droppedItem.data = glassShard.data;

        Collectable collectable = droppedItem.GetComponent<Collectable>();
        if (collectable != null)
        {
            collectable.SetCollected(false);
        }

        Destroy(gameObject);
    }

    
}
