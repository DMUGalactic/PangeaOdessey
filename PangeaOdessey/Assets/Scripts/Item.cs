using UnityEngine;

public enum EquipmentType
{
    Head,
    Body,
    Gloves,
    Shoes
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemImage;
    public EquipmentType equipmentType;

    public int Hp;
    public float Damage;
    public float Speed;
}