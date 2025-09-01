using UnityEngine;

[CreateAssetMenu(fileName = "Buff_SPEED", menuName = "Buff/BuffSPEED")]
public class BuffSPEED : Buff
{
    private float entitySPEED, calculateSPEED;
    public override bool BeginEffect(GameObject holder)
    {
        if (!base.BeginEffect(holder)) return false;
        entitySPEED = holder.GetComponent<EntityInterface>().MoveSpeed;
        if (isMultiply)
            calculateSPEED = entitySPEED * (BaseMultiply + MultiplyEL * (GetLevel() - 1));
        else
            calculateSPEED = BaseAddition + (AdditionEL * (GetLevel() - 1));
        holder.GetComponent<EntityInterface>().B_MoveSpeed += calculateSPEED;

        return true;
    }
    public override void EndEffect()
    {
        holder.GetComponent<EntityInterface>().B_MoveSpeed -= calculateSPEED;
        base.EndEffect();
    }
    public override void calculateAgain()
    {
        holder.GetComponent<EntityInterface>().B_MoveSpeed -= calculateSPEED;

        entitySPEED = holder.GetComponent<EntityInterface>().MoveSpeed;
        if (isMultiply)
            calculateSPEED = entitySPEED * (BaseMultiply + MultiplyEL * (GetLevel() - 1));
        else
            calculateSPEED = BaseAddition + (AdditionEL * (GetLevel() - 1));
        holder.GetComponent<EntityInterface>().B_MoveSpeed += calculateSPEED;

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
            + " tốc độ di chuyển.\nTác dụng còn: " + bh.GetRemainingDuration(this).ToString("0.00") + " giây";
    }
}
