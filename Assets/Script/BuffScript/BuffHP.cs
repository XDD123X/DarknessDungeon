using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff_HP", menuName = "Buff/BuffHP")]
public class BuffHP : Buff
{
    private float entityMaxHP;
    private float calculateMaxHP;
    public override bool BeginEffect(GameObject holder)
    {
        if (!base.BeginEffect(holder)) return false;
        entityMaxHP = holder.GetComponent<EntityInterface>().MaxHP;
        if (isMultiply)
            calculateMaxHP = entityMaxHP * (BaseMultiply + MultiplyEL * (GetLevel() - 1));
        else
            calculateMaxHP = BaseAddition + (AdditionEL * (GetLevel() - 1));
        holder.GetComponent<EntityInterface>().B_MaxHP += calculateMaxHP;

        return true;
    }
    public override void EndEffect()
    {
        holder.GetComponent<EntityInterface>().B_MaxHP -= calculateMaxHP;

        base.EndEffect();
    }
    public override void calculateAgain()
    {
        holder.GetComponent<EntityInterface>().B_MaxHP -= calculateMaxHP;
        entityMaxHP = holder.GetComponent<EntityInterface>().MaxHP;
        if (isMultiply)
            calculateMaxHP = entityMaxHP * (BaseMultiply + MultiplyEL * (GetLevel() - 1));
        else
            calculateMaxHP = BaseAddition + (AdditionEL * (GetLevel() - 1));
        holder.GetComponent<EntityInterface>().B_MaxHP += calculateMaxHP;
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
            + " máu tối đa.\nTác dụng còn: " + bh.GetRemainingDuration(this).ToString("0.00") + " giây";

    }
}
