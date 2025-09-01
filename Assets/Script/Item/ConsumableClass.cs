using System;
using UnityEngine;


[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    [SerializeField]
    private float HealthRegenation;

    // =========================

    [SerializeField]
    private float Duration;

    [SerializeField]
    private Boolean isMultiply;

    [SerializeField]
    private float MultiplyAmount;

    [SerializeField]
    private float AdditionAmount;


    public enum PotionType
    {
        Healing,
        BuffATK,
        BuffCRIT,
        BuffDEF,
        BuffSPEED
    }

    public PotionType type;

    public bool Use(GameObject player)
    {
        PlayerController playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        BuffSettings buffSettings = GameObject.FindGameObjectWithTag("Setting").GetComponent<BuffSettings>();
        switch (type)
        {
            case PotionType.Healing:
                {
                    if (playerScript.CurrentHP == playerScript.MaxHP) return false;
                    playerScript.CurrentHP = (playerScript.CurrentHP + HealthRegenation > playerScript.MaxHP ? playerScript.MaxHP : playerScript.CurrentHP + HealthRegenation);
                    return true;
                }
            case PotionType.BuffATK:
                {
                    BuffATK atk = Buff.CreateInstance<BuffATK>();
                    atk.SetLevel(1);
                    atk.Duration = Duration;
                    atk.BaseMultiply = MultiplyAmount;
                    atk.BaseAddition = AdditionAmount;
                    atk.isMultiply = isMultiply;
                    atk.icon = this.itemIcon;
                    atk.name = this.itemName;
                    Buff.ApplyBuff(atk, player);
                    break;
                }
            case PotionType.BuffCRIT:
                {
                    BuffCRIT crit = Buff.CreateInstance<BuffCRIT>();
                    crit.SetLevel(1);
                    crit.Duration = Duration;
                    crit.BaseMultiply = MultiplyAmount;
                    crit.BaseAddition = AdditionAmount;
                    crit.isMultiply = isMultiply;
                    crit.icon = this.itemIcon;
                    crit.name = this.itemName;
                    Buff.ApplyBuff(crit, player);
                    break;

                }
            case PotionType.BuffDEF:
                {
                    BuffDEF def = Buff.CreateInstance<BuffDEF>();
                    def.SetLevel(1);
                    def.Duration = Duration;
                    def.BaseMultiply = MultiplyAmount;
                    def.BaseAddition = AdditionAmount;
                    def.isMultiply = isMultiply;
                    def.icon = this.itemIcon;
                    def.name = this.itemName;
                    Buff.ApplyBuff(def, player);
                    break;

                }
            case PotionType.BuffSPEED:
                {
                    BuffSPEED speed = Buff.CreateInstance<BuffSPEED>();
                    speed.SetLevel(1);
                    speed.Duration = Duration;
                    speed.BaseMultiply = MultiplyAmount;
                    speed.BaseAddition = AdditionAmount;
                    speed.isMultiply = isMultiply;
                    speed.icon = this.itemIcon;
                    speed.name = this.itemName;
                    Buff.ApplyBuff(speed, player);
                    break;
                }
            default:
                {
                    return false;
                }
        }
        return true;
    }

    public override string ToString()
    {
        string result = "Mô tả: Tăng " + (isMultiply ? (MultiplyAmount*100) + "%" : AdditionAmount);
        switch (type)
        {
            case PotionType.Healing:
                {
                    return "Mô tả: Hồi " + HealthRegenation + " máu ngay lập tức";
                }
            case PotionType.BuffATK:
                {
                    result += " sức mạnh công kích ";
                    break;
                }
            case PotionType.BuffCRIT:
                {
                    result += " tỉ lệ bạo kích lẫn ST bạo kích ";
                    break;
                }
            case PotionType.BuffDEF:
                {
                    result += " phòng ngự ";
                    break;
                }
            case PotionType.BuffSPEED:
                {
                    result += " tốc độ di chuyển ";
                    break;
                }
        }
        return result + " trong " + Duration + " giây";
    }
    public override ItemClass GetItem()
    {
        return this;
    }

    public override WeaponClass GetWeapon()
    {
        return null;
    }

    public override ConsumableClass GetConsumable()
    {
        return this;
    }

    public override ArmorClass GetArmor()
    {
        return null;
    }
}
