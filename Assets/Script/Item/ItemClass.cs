using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public Sprite itemIcon;
    public int itemID;


    public bool isStackable;
    public abstract ItemClass GetItem();
    public abstract WeaponClass GetWeapon();
    public abstract ConsumableClass GetConsumable();
    public abstract ArmorClass GetArmor();

    public override bool Equals(object other)
    {
        ItemClass o = other as ItemClass;
        return o.itemID == this.itemID;
    }
}
