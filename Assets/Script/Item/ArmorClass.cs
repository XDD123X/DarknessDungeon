using UnityEngine;


[CreateAssetMenu(fileName = "new Armor Class", menuName = "Item/Armor")]
public class ArmorClass : ItemClass
{
    [Header("Armor")]
    public ArmorType armorType;

    [SerializeField] private int Armor = 0;
    [SerializeField] private float HPAddition = 0f;

    public enum ArmorType
    {
        Boots,
        Clothes,
        Cloves
    }

    public override string ToString()
    {
        //return "<color=#9F8D7E>Giaùp</color>: " + Armor
        //    + "\n<color=#000000>Maùu</color>: " + HPAddition;
        return "Giáp: " + Armor
            + "\nMáu: " + HPAddition;
    }

    public int GetArmorAttr()
    {
        return this.Armor;
    }

    public float GetHPAttr()
    {
        return this.HPAddition;
    }

    public override ArmorClass GetArmor()
    {
        return this;
    }

    public override ConsumableClass GetConsumable()
    {
        return null;
    }

    public override ItemClass GetItem()
    {
        return this;
    }

    public override WeaponClass GetWeapon()
    {
        return null;
    }
}
