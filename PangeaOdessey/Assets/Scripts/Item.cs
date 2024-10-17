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
    public string itemName;
    public Sprite itemImage;
    public EquipmentType equipmentType;

    public int Hp;
    public int Damage;
    public int Speed;
}