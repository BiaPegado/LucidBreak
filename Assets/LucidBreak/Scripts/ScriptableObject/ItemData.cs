using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Item Data", menuName = "Inventory/Item", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;
    public TileBase tileToPlace;
    public bool isPlantable;
}
