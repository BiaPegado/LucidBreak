using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    private Dictionary<string, Item> nameToItem = new();

    private static ItemManager instance;

    private HashSet<string> collectedItems = new HashSet<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Item item in items)
        {
            AddItem(item);
        }
    }

    private void AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("Tentando adicionar item nulo ao dicionário!");
            return;
        }
        if (item.data == null)
        {
            Debug.LogWarning("Item com nome " + item.name + " tem `data` nulo!");
            return;
        }

        if (!nameToItem.ContainsKey(item.data.itemName))
        {
            nameToItem.Add(item.data.itemName, item);
        }
    }

    public Item GetItemByName(string key)
    {
        if (nameToItem.ContainsKey(key))
        {
            return nameToItem[key];
        }
        return null;
    }

    // --- NOVOS MÉTODOS ---

    public void AddCollectedItem(string itemName)
    {
        collectedItems.Add(itemName);
    }

    public bool IsItemCollected(string itemName)
    {
        return collectedItems.Contains(itemName);
    }

    public void RemoveCollectedItem(string itemName)
    {
        if (collectedItems.Contains(itemName))
        {
            collectedItems.Remove(itemName);
            Debug.Log($"Item '{itemName}' removido da lista de coletados.");
        }
    }

}
