using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private bool playerNearby = false;
    private GameObject player;
    private Item item;

    private bool isCollected = false;

    void Awake()
    {
        item = GetComponent<Item>();

        if (GameManager.instance != null)
        {
            bool alreadyCollected = GameManager.instance.itemManager.IsItemCollected(item.data.itemName);

            PlayerController playerController = FindObjectOfType<PlayerController>();
            bool inInventory = playerController != null && playerController.inventory.Contains(item);

            if (alreadyCollected || inInventory)
            {
                SetCollected(true);
                return;
            }
        }
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PlayerController playerController = player.GetComponent<PlayerController>();

            if (!playerController.inventory.Contains(item))
            {
                playerController.inventory.Add(item);
                GameManager.instance.itemManager.AddCollectedItem(item.data.itemName);
                SetCollected(true);
            }
            else
            {
                Debug.Log("Item já está no inventário, não vai destruir.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            player = null;
        }
    }

    public void SetCollected(bool collected)
    {
        isCollected = collected;
        gameObject.SetActive(!collected);
    }
}
