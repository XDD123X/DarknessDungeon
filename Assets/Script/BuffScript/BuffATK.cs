using UnityEngine;

[CreateAssetMenu(fileName = "Buff_ATK", menuName = "Buff/BuffATK")]
public class BuffATK : Buff
{
    float EntityATK, CalulateATK;
    public override bool BeginEffect(GameObject holder)
    {
        if (!base.BeginEffect(holder)) return false;
        EntityATK = holder.GetComponent<EntityInterface>().Atk;

        if (isMultiply)
            CalulateATK = EntityATK * (BaseMultiply + MultiplyEL * (GetLevel() - 1));
        else
            CalulateATK = BaseAddition + AdditionEL * (GetLevel() - 1);

        holder.GetComponent<EntityInterface>().B_Atk += CalulateATK;
        return true;
    }
    public override void EndEffect()
    {
        holder.GetComponent<EntityInterface>().B_Atk -= CalulateATK;
        base.EndEffect();
    }

    public override void calculateAgain()
    {
        holder.GetComponent<EntityInterface>().B_Atk -= CalulateATK;
        EntityATK = holder.GetComponent<EntityInterface>().Atk;
        if (isMultiply)
            CalulateATK = EntityATK * (BaseMultiply + MultiplyEL * (GetLevel() - 1));
        else
            CalulateATK = BaseAddition + AdditionEL * (GetLevel() - 1);
        holder.GetComponent<EntityInterface>().B_Atk += CalulateATK;
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
            + " sức mạnh vật lý.\nTác dụng còn: " + bh.GetRemainingDuration(this).ToString("0.00") + " giây";

    }
}
