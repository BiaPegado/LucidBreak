using System.Linq;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class TrashCan : MonoBehaviour
{
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.E;
    public string[] requiredItemNames = { "GlassShard", "PictureFrame" };
    private int trashedItems = 0;
    public Item vidroPrefab; 
    private bool playerInRange = false;
    public Inventory playerInventory;

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>();
        if (playerInventory == null)
            Debug.LogWarning("Inventário não encontrado.");
    }



    private void Update()
    {
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<Inventory>();
            if (playerInventory == null)
                Debug.LogWarning("Inventário não encontrado.");
        }
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            TryDispose();
        }
    }

    private void TryDispose()
    {
        if (playerInventory == null || playerInventory.selectedSlot == null)
        {
            Debug.Log("Inventário ou slot não selecionado.");
            return;
        }

        if (requiredItemNames.Contains(playerInventory.selectedSlot.itemName))
        {
            playerInventory.selectedSlot.RemoveItem();
            playerInventory.inventoryUI.RefreshToolbar();
            trashedItems++;
            if (trashedItems > 1)
            {
                Vector2 location = new Vector2(1.46f, 1.76f);
                Vector2 rawOffset = Random.insideUnitCircle.normalized * 1f;
                Vector2 forceDirection = (Vector2.down + rawOffset).normalized;
                Item droppedItem = Instantiate(vidroPrefab, location, Quaternion.identity);
                droppedItem.rb.AddForce(forceDirection * 5f, ForceMode2D.Impulse);
                GameManager.instance.finalCounter++;
            }

        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
