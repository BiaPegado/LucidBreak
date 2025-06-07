using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    private Dictionary<string, Item> nameToItem = new();

    private void Awake()
    {
        foreach(Item item in items)
        {
            AddItem(item);
        }
        
    }

    private void AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("Tentando adicionar item nulo ao dicion�rio!");
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
}
