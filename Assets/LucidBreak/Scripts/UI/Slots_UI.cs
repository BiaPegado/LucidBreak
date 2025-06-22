using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class Slots_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemIcon;
    public Image background;
    public TextMeshProUGUI quantityText;
    public Inventory_UI inventoryUI;
    public int slotIndex;

    private Color originalColor;
    private Color hoverColor = new Color32(160, 160, 160, 255);
    private Color selectedColor = new Color32(100, 100, 200, 255);
    private static Slots_UI selectedSlot;

    private static Slots_UI draggingSlot;
    private static GameObject draggingIcon;
    private static RectTransform draggingIconTransform;
    private static bool isRightMouseDrag = false;
    private void Awake()
    {
        originalColor = background.color;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    private void Update()
    {
        if (selectedSlot == this && Input.GetKeyDown(KeyCode.Q))
        {
            inventoryUI.Remove(selectedSlot.slotIndex);
        }
    }
    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Inventory fromInv = inventoryUI.Player.inventory;
            Inventory toInv = null;

            foreach (var slot in FindObjectsByType<Slots_UI>(FindObjectsSortMode.None))
            {
                if (slot != this && slot.inventoryUI != this.inventoryUI)
                {
                    toInv = slot.inventoryUI.Player.inventory;
                    break;
                }
            }

            if (toInv == null) toInv = fromInv;

            inventoryUI.ShiftTransfer(fromInv, slotIndex, toInv);
            return;
        }

        if (selectedSlot == this)
        {
            Deselect();
        }
        else
        {
            if (selectedSlot != null)
                selectedSlot.Deselect();

            Select();
        }
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (background.color == originalColor) background.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (background.color == hoverColor) background.color = originalColor;
    }

    public void SetItem(Inventory.Slot slot)
    {
        if (itemIcon.sprite != slot.icon || quantityText.text != slot.count.ToString())
        {
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1); 
            quantityText.text = slot.count.ToString(); 
        }
    }

    public void SetEmpty()
    {
        if (itemIcon.color.a != 0)
        {
            itemIcon.sprite = null;
            itemIcon.color = new Color(1, 1, 1, 0); 
            quantityText.text = ""; 
        }
    }

    public void Select()
    {
        background.color = selectedColor;
        selectedSlot = this;
        inventoryUI.Player.inventory.SelectSlot(this.slotIndex);
    }

    public void Deselect()
    {
        background.color = originalColor;
        if (selectedSlot == this)
            selectedSlot = null;
    }

    /*
    public void OnBeginDrag(PointerEventData eventData)
    {
        var slot = inventoryUI.Player.inventory.slots[slotIndex];
        if (slot.itemName == "") return;

        draggingSlot = this;
        isRightMouseDrag = eventData.button == PointerEventData.InputButton.Right;
        Canvas canvas = GetComponentInParent<Canvas>();
        draggingIcon = GameObject.Instantiate(inventoryUI.dragIcon.gameObject, canvas.transform);
        draggingIconTransform = draggingIcon.GetComponent<RectTransform>();
        draggingIcon.GetComponent<Image>().sprite = itemIcon.sprite;
        draggingIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        draggingIcon.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingIcon != null)
        {
            draggingIconTransform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingIcon != null)
            Destroy(draggingIcon);

        draggingIcon = null;
        draggingIconTransform = null;

        if (draggingSlot == null || eventData.pointerEnter == null) return;

        var targetSlot = eventData.pointerEnter.GetComponentInParent<Slots_UI>();
        if (targetSlot != null)
        {
            var fromInventory = draggingSlot.inventoryUI.Player.inventory;
            var toInventory = targetSlot.inventoryUI.Player.inventory;

            var fromIndex = draggingSlot.slotIndex;
            var toIndex = targetSlot.slotIndex;

            var fromSlot = fromInventory.slots[fromIndex];
            var toSlot = toInventory.slots[toIndex];

            if (fromSlot.itemName == "") return;

            if (isRightMouseDrag && fromSlot.count > 1)
            {
                int halfCount = fromSlot.count / 2;

                if (toSlot.itemName == "" || toSlot.itemName == fromSlot.itemName)
                {
                    toSlot.itemName = fromSlot.itemName;
                    toSlot.icon = fromSlot.icon;
                    toSlot.count += halfCount;
                    fromSlot.count -= halfCount;
                }
            }
            else
            {
                if (fromInventory != toInventory)
                    inventoryUI.TransferBetweenInventories(fromInventory, fromIndex, toInventory, toIndex);
                else if (draggingSlot != targetSlot)
                    inventoryUI.SwapSlots(fromIndex, toIndex);
            }

            inventoryUI.RefreshToolbar();
            //inventoryUI.RefreshInventory();
            targetSlot.inventoryUI.RefreshToolbar();
           // targetSlot.inventoryUI.RefreshInventory();
        }

        draggingSlot = null;
    }
    */
}


