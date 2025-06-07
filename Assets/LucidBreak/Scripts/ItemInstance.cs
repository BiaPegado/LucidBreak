using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemInstance : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropItem(Item item)
    {
        Vector3 spawnLocation = transform.position;
        Vector2 rawOffset = Random.insideUnitCircle.normalized * Random.Range(0.7f, 1.25f);

        Item droppedItem = Instantiate(item, spawnLocation + (Vector3)rawOffset, Quaternion.identity);
        droppedItem.data = item.data;
        droppedItem.rb.AddForce(rawOffset * 2f, ForceMode2D.Impulse);
    }
}

