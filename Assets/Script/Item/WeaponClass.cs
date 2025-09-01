using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon Class", menuName = "Item/Weapon")]
public class WeaponClass : ItemClass
{
    [Header("Weapon")]
    public WeaponType weaponType;

    [SerializeField] private float Damage = 0;
    [SerializeField] private float MagicDamage = 0;
    [SerializeField] private int CritDamage = 0;
    [SerializeField] private int CritChance = 0;
    [SerializeField] private int Penetration = 0;
    [SerializeField] private float AttackSpeed = 0f;

    public enum WeaponType
    {
        Sword,
        Bow,
        Staff
    }

    public float getAttackSpeed()
    {
        return this.AttackSpeed;
    }

    public float GetDamage()
    {
        return this.Damage;
    }
    public float GetMagicDamage()
    {
        return this.MagicDamage;
    }
    public int GetCritDamage()
    {
        return this.CritDamage;
    }
    public int GetCritChance()
    {
        return this.CritChance;
    }
    public int GetPenetration()
    {
        return this.Penetration;
    }

    public override ArmorClass GetArmor()
    {
        return null;
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
        return this;
    }

    public override string ToString()
    {
        //return "<color=#000000>Saùt thöông vaät lyù:</color> " + Damage
        //    + "\n<color=#001EFF>Saùt thöông pheùp</color>: " + MagicDamage.ToString("0.00")
        //    + "\n<color=#FF5800>Saùt thöông baïo kích</color>: " + CritDamage.ToString("0.00") + "%"
        //    + "\n<color=#FF5800>Tæ leä baïo kích</color>: " + CritChance + "%"
        //    + "\n<color=#FF00B8>Xuyeân giaùp</color>: " + Penetration + "%"
        //    + "\n<color=#000000>Toác ñoä taán coâng</color>: " + AttackSpeed;
        return "Sát thương vật lý: " + Damage
            + "\nSát thương phép: " + MagicDamage
            + "\nSát thương bạo kích: " + Mathf.RoundToInt(CritDamage) + "%"
            + "\nTỉ lệ bạo kích: " + CritChance + "%"
            + "\nXuyên giáp: " + Penetration + "%"
            + "\nTốc độ tấn công: " + AttackSpeed;
    }
}
