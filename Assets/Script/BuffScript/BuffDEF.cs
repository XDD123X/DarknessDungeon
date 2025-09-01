using UnityEngine;

[CreateAssetMenu(fileName = "Buff_DEF", menuName = "Buff/BuffDEF")]
public class BuffDEF : Buff
{
    private int calculateDEF, entityDEF;
    public override bool BeginEffect(GameObject holder)
    {
        if (!base.BeginEffect(holder)) return false;
        entityDEF = holder.GetComponent<EntityInterface>().Def;
        if (isMultiply)
            calculateDEF = Mathf.RoundToInt(entityDEF * (BaseMultiply + MultiplyEL * (GetLevel() - 1)));
        else
            calculateDEF = Mathf.RoundToInt(BaseAddition + AdditionEL * (GetLevel() - 1));
        holder.GetComponent<EntityInterface>().B_Def += calculateDEF;
        return true;
    }
    public override void EndEffect()
    {
        holder.GetComponent<EntityInterface>().B_Def -= calculateDEF;
        base.EndEffect();
    }
    public override void calculateAgain()
    {
        holder.GetComponent<EntityInterface>().B_Def -= calculateDEF;
        entityDEF = holder.GetComponent<EntityInterface>().Def;
        if (isMultiply)
            calculateDEF = Mathf.RoundToInt(entityDEF * (BaseMultiply + MultiplyEL * (GetLevel() - 1)));
        else
            calculateDEF = Mathf.RoundToInt(BaseAddition + AdditionEL * (GetLevel() - 1));
        holder.GetComponent<EntityInterface>().B_Def += calculateDEF;
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
            + " phòng ngự.\nTác dụng còn: " + bh.GetRemainingDuration(this).ToString("0.00") + " giây";

    }
}
