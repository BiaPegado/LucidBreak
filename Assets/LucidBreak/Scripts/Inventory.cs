using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int toolbarSize = 9;
    public Inventory_UI inventoryUI;
    public Slot selectedSlot = null;
    private static readonly HashSet<string> usableItems = new() { "Axe" };

    [System.Serializable]
    public class Slot
    {
        public string itemName = "";
        public int count = 0;
        public int maxAllowed = 20;
        public Sprite icon;

        public bool CanAddItem()
        {
            return count < maxAllowed;
        }

        public void AddItem(Item item)
        {
            itemName = item.data.itemName;
            icon = item.data.icon;
            count++;
        }

        public void RemoveItem()
        {
            if (count > 0)
            {
                count--;
                if (count == 0)
                {
                    Clear();
                }
            }
        }
        public void Clear()
        {
            itemName = "";
            icon = null;
            count = 0;
        }

    }

    [SerializeField] public List<Slot> slots = new(); 

    private void Awake()
    {
        if (slots == null || slots.Count == 0)
        {
            InitSlots(3);
        }
    }

    public void InitSlots(int numSlots)
    {
        slots.Clear();
        for (int i = 0; i < numSlots; i++)
        {
            slots.Add(new Slot());
        }
    }

    public void Add(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.data.itemName && slot.CanAddItem())
            {
                slot.AddItem(item);
                inventoryUI?.RefreshToolbar();
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (string.IsNullOrEmpty(slot.itemName))
            {
                slot.AddItem(item);
                inventoryUI?.RefreshToolbar();
                return;
            }
        }
    }

    public void RemoveUsingIndex(int index)
    {
        slots[index].RemoveItem();
        inventoryUI?.RefreshToolbar();
    }

    public bool IsToolbarSlot(int index)
    {
        return index >= 0 && index < toolbarSize;
    }

    public void SelectSlot(int index)
    {
        if (slots != null && slots.Count > index)
        {
            selectedSlot = slots[index];
            ChangeItemHeld();
        }
    }

    public void ChangeItemHeld()
    {
        if (selectedSlot.icon == null)
        {
            inventoryUI.heldItemRenderer.color = new Color(1, 1, 1, 0);
        }
        else
        {
            inventoryUI.heldItemRenderer.color = Color.white;
            inventoryUI.heldItemRenderer.sprite = selectedSlot.icon;
            inventoryUI.heldItemRenderer.transform.localScale = usableItems.Contains(selectedSlot.itemName)
                ? Vector3.one
                : new Vector3(0.5f, 0.5f, 1f);
        }
    }

    public bool Contains(Item item)
    {
        if (item == null || item.data == null) return false;

        string targetName = item.data.itemName;

        foreach (Slot slot in slots)
        {
            if (slot.itemName == targetName && slot.count > 0)
            {
                return true;
            }
        }
        return false;
    }

}
