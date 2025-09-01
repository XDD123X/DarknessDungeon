using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff_CRIT", menuName = "Buff/BuffCRIT")]
public class BuffCRIT : Buff
{
    int CalculateCRITChance, EntityCRITChance;
    int CalculateCRITDamage, EntityCRITDamage;
    public override bool BeginEffect(GameObject holder)
    {
        if (!base.BeginEffect(holder)) return false;
        EntityCRITChance = holder.GetComponent<EntityInterface>().CritChance;
        EntityCRITDamage = holder.GetComponent<EntityInterface>().CritDmg;
        if (isMultiply)
        {
            CalculateCRITChance = Mathf.RoundToInt(EntityCRITChance * (BaseMultiply + MultiplyEL * (GetLevel() - 1)));
            CalculateCRITDamage = Mathf.RoundToInt(EntityCRITDamage * (BaseMultiply + MultiplyEL * (GetLevel() - 1)));

        }
        else
        {
            CalculateCRITChance = Mathf.RoundToInt(BaseAddition + AdditionEL * (GetLevel() - 1));
            CalculateCRITDamage = Mathf.RoundToInt(BaseAddition + AdditionEL * (GetLevel() - 1));
        }

        holder.GetComponent<EntityInterface>().B_CritChance += CalculateCRITChance;
        holder.GetComponent<EntityInterface>().B_CritDMG += CalculateCRITDamage;

        return true;
    }
    public override void EndEffect()
    {
        holder.GetComponent<EntityInterface>().B_CritChance -= CalculateCRITChance;
        holder.GetComponent<EntityInterface>().B_CritDMG -= CalculateCRITDamage;

        base.EndEffect();
    }
    public override void calculateAgain()
    {
        holder.GetComponent<EntityInterface>().B_CritChance -= CalculateCRITChance;
        holder.GetComponent<EntityInterface>().B_CritDMG -= CalculateCRITDamage;
        EntityCRITChance = holder.GetComponent<EntityInterface>().CritChance;
        EntityCRITDamage = holder.GetComponent<EntityInterface>().CritDmg;
        if (isMultiply)
        {
            CalculateCRITChance = Mathf.RoundToInt(EntityCRITChance * (BaseMultiply + MultiplyEL * (GetLevel() - 1)));
            CalculateCRITDamage = Mathf.RoundToInt(EntityCRITDamage * (BaseMultiply + MultiplyEL * (GetLevel() - 1)));

        }
        else
        {
            CalculateCRITChance = Mathf.RoundToInt(BaseAddition + AdditionEL * (GetLevel() - 1));
            CalculateCRITDamage = Mathf.RoundToInt(BaseAddition + AdditionEL * (GetLevel() - 1));
        }
        holder.GetComponent<EntityInterface>().B_CritChance += CalculateCRITChance;
        holder.GetComponent<EntityInterface>().B_CritDMG += CalculateCRITDamage;

    }

    public override string ToString()
    {
        BuffHolder bh = holder.GetComponent<BuffHolder>();
        string modify = "";
        float calMultiply = ((BaseMultiply + (MultiplyEL * (GetLevel() - 1))) * 100);
        float calAddition = (BaseAddition + (AdditionEL * (GetLevel() - 1)));
        if (isMultiply)
        {
            if (calMultiply >= 0)
            {
                modify = "Tăng";
            }
            else
            {
                modify = "Giảm";
            }
        }
        else
        {
            if (calAddition >= 0)
            {
                modify = "Tăng";
            }
            else
            {
                modify = "Giảm";
            }
        }
        return "Mô tả: " + modify + " "
            + (isMultiply ? calMultiply + "%"
            : calAddition)
            + " tỉ lệ bạo kích và sát thương bạo kích.\nTác dụng còn: " + bh.GetRemainingDuration(this).ToString("0.00") + " giây";

    }

}
