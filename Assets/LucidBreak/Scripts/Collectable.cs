using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Item item = GetComponent<Item>();
            if (item != null)
            {
                other.gameObject.GetComponent<PlayerController>().inventory.Add(item);
                Destroy(gameObject);
            }
        }
    }
}
