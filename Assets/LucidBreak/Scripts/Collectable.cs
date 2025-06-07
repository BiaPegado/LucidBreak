using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
