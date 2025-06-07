using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public PlayerController Player;
    public List<Slots_UI> toolbarSlots = new List<Slots_UI>();
    public List<Slots_UI> inventorySlots = new List<Slots_UI>();
    public SpriteRenderer heldItemRenderer;

    public Image dragIcon;
    public RectTransform dragIconRect; 

    void Awake()
    {
        Player.inventory.inventoryUI = this;

        //inventoryPanel.SetActive(false);

        dragIconRect = dragIcon.GetComponent<RectTransform>();
        dragIcon.gameObject.SetActive(false);
    }

    /*
    public void ToggleInventory()
    {
        if (!inventoryPanel.activeSelf)
        {
            if (!PlayerController.hasInventoryOpen)
            {
                RefreshInventory();
                inventoryPanel.SetActive(true);
                PlayerController.hasInventoryOpen = true;
            }
            
        }
        else
        {
            inventoryPanel.SetActive(false);
            PlayerController.hasInventoryOpen = false;
        }
    }
    */
    public void RefreshToolbar()
    {
        for (int i = 0; i < Player.inventory.toolbarSize; i++)
        {
            Slots_UI slotUI = toolbarSlots[i];
            slotUI.inventoryUI = this;
            slotUI.slotIndex = i;

            if (Player.inventory.slots[i].itemName != "")
            {
                slotUI.SetItem(Player.inventory.slots[i]);
            }
            else
            {
                slotUI.SetEmpty();
            }
        }
    }
    /*
    public void RefreshInventory()
    {
        for (int i = 0; i < Player.inventory.slots.Count; i++)
        {
            Slots_UI slotUI = i < Player.inventory.toolbarSize ? toolbarSlots[i] : inventorySlots[i - Player.inventory.toolbarSize];
            slotUI.inventoryUI = this;
            slotUI.slotIndex = i;

            if (Player.inventory.slots[i].itemName != "")
            {
                slotUI.SetItem(Player.inventory.slots[i]);
            }
            else
            {
                slotUI.SetEmpty();
            }
        }
    }
    */

    public void Remove(int slotID)
    {
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(Player.inventory.slots[slotID].itemName);
       
        if (itemToDrop != null)
        {
            Player.DropItem(itemToDrop);
            Player.inventory.RemoveUsingIndex(slotID);
            //RefreshInventory();
            RefreshToolbar();
        }
    }

    public void SelectToolbarSlot(int index)
    {
        if (index < 0 || index >= toolbarSlots.Count)
            return;

        foreach (var slot in toolbarSlots)
            slot.Deselect();

        toolbarSlots[index].Select();
    }

    /*
    public void ShiftTransfer(Inventory fromInv, int fromIdx, Inventory toInv)
    {
        var fromSlot = fromInv.slots[fromIdx];
        if (fromSlot.itemName == "") return;

        for (int i = 0; i < toInv.slots.Count; i++)
        {
            var toSlot = toInv.slots[i];
            if (toSlot.itemName == fromSlot.itemName)
            {
                toSlot.count += fromSlot.count;
                fromSlot.Clear();
                RefreshToolbar();
                RefreshInventory();
                toInv.inventoryUI?.RefreshInventory();
                return;
            }
        }

        for (int i = 0; i < toInv.slots.Count; i++)
        {
            var toSlot = toInv.slots[i];
            if (toSlot.itemName == "")
            {
                MoveSlot(fromSlot, toSlot, true);
                RefreshToolbar();
                RefreshInventory();
                toInv.inventoryUI?.RefreshInventory();
                return;
            }
        }
    }
    */

    public void MoveSlot(Inventory.Slot from, Inventory.Slot to, bool isEmpty)
    {
        if (isEmpty) to.count = from.count;
        else to.count += from.count;
        to.itemName = from.itemName;
        to.icon = from.icon;

        from.itemName = "";
        from.count = 0;
        from.icon = null;
    }

    public void SwapSlots(int indexA, int indexB)
    {
        TransferBetweenInventories(Player.inventory, indexA, Player.inventory, indexB);
    }
    
    public void TransferBetweenInventories(Inventory fromInv, int fromIdx, Inventory toInv, int toIdx)
    {
        var fromSlot = fromInv.slots[fromIdx];
        var toSlot = toInv.slots[toIdx];

        if (toSlot.itemName == "" || toSlot.itemName == fromSlot.itemName)
        {
            toSlot.itemName = fromSlot.itemName;
            toSlot.icon = fromSlot.icon;
            toSlot.count += fromSlot.count;
            fromSlot.Clear();
        }
        else
        {
            var temp = new Inventory.Slot
            {
                itemName = toSlot.itemName,
                icon = toSlot.icon,
                count = toSlot.count
            };

            toSlot.itemName = fromSlot.itemName;
            toSlot.icon = fromSlot.icon;
            toSlot.count = fromSlot.count;

            fromSlot.itemName = temp.itemName;
            fromSlot.icon = temp.icon;
            fromSlot.count = temp.count;
        }
    }

}
