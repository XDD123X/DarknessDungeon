using System;
using System.Linq;
using UnityEngine;

public class Buff : ScriptableObject
{
    public new string name;

    public static Buff Clone(Buff buff)
    {
        Buff result = ScriptableObject.CreateInstance<Buff>();
        result.name = buff.name;
        result.type = buff.type;
        result.icon = buff.icon;
        result.Duration = buff.Duration;
        result.DurationEL = buff.DurationEL;
        result.MultiplyEL = buff.MultiplyEL;
        result.BaseMultiply = buff.BaseMultiply;
        result.level = buff.level;
        return result;
    }

    public float Duration;
    public Sprite icon;
    protected GameObject holder;

    public Boolean isMultiply;

    // Multiply base
    public float BaseMultiply = 0.2f;
    // Multiply each level
    public float MultiplyEL = 0.1f;
    // Duration increase each level
    public float DurationEL = 0f;

    // Addition base
    public float BaseAddition = 10f;
    // Addition each level
    public float AdditionEL = 0f;

    private BuffHolder TargetScript;

    public enum Type
    {
        BUFF,
        DEBUFF
    }
    //public enum BuffType
    //{
    //    ATK,
    //    DEF,
    //    SPEED,
    //    CRIT,
    //    CRITDMG,
    //    HPREGEN,
    //    HPGAIN
    //}

    //public BuffType buffType;
    public Type type;
    [SerializeField] protected int level;
    public static void ApplyBuff(Buff buff, GameObject target)
    {
        buff.BeginEffect(target);
    }

    public virtual bool BeginEffect(GameObject holder)
    {
        this.holder = holder;
        TargetScript = holder.GetComponent<BuffHolder>();

        //Buff duplicate = TargetScript.buffs.
        //    FirstOrDefault(p => p.GetType() == this.GetType() && p.isMultiply == this.isMultiply);
        Buff duplicate = TargetScript.buffs.FirstOrDefault(p => p.Equals(this));
        if (duplicate == null)
        {
            TargetScript.InsertNewBuff(this);
        }
        else
        {
            if (duplicate.level == this.level)
            {
                TargetScript.ResetCD(duplicate);
                return false;
            }
            else if (duplicate.level < this.level)
            {
                int index = TargetScript.buffs.IndexOf(duplicate);
                duplicate.EndEffect();
                //TargetScript.duration.RemoveAt(index);
                TargetScript.InsertNewBuff(this, index);
            } else
            {
                return false;
            }
        }
        return true;
    }

    public void SetLevel(int value)
    {
        level = value;
    }

    public int GetLevel()
    {
        return (this.level > 0 ? this.level : 1);
    }

    public virtual void EndEffect()
    {
        holder.GetComponent<BuffHolder>().buffs.Remove(this);
    }
    public virtual void calculateAgain() { }

    public override bool Equals(object other)
    {
        Buff compare = other as Buff;
        return this.name.Equals(compare.name);
    }
}
