using System.Collections.Generic;
using UnityEngine;

//public class EntityInterface<T> : MonoBehaviour where T : EntityInterface<T>
public class EntityInterface : MonoBehaviour
{
    [SerializeField] private float currentHP, baseMaxHP;

    [SerializeField] private float baseMoveSpeed, baseDashSpeed, baseAttackSpeed;

    [SerializeField] private int baseDef, baseCritChance, baseCritDMG, basePenetration;
    [SerializeField] private float baseAtk, baseMagicAtk;

    private bool isDead, isMoving, isAttacking, isAttacked, isDashing;
    private bool canMove, canAttack, canDash;

    private const int MaxDef = 1000;

    // Equipment attribute
    private int e_def, e_critChance, e_critDmg, e_penetration;
    private float e_atk, e_magicAtk, e_moveSpeed, e_dashSpeed, e_attackSpeed;
    private float e_maxHP;

    // Buff attribute
    private int b_def, b_critChance, b_critDmg, b_penetration;
    private float b_atk, b_magicAtk, b_moveSpeed, b_dashSpeed, b_attackSpeed;
    private float b_maxHP;

    //private static T instance;
    //public static T Instance { get { return instance; } }

    //protected virtual void Awake()
    //{
    //	if (instance != null && this.gameObject != null)
    //	{
    //		Destroy(this.gameObject);
    //	}
    //	else
    //	{
    //		instance = (T)this;
    //	}

    //	if (!gameObject.transform.parent)
    //	{
    //		DontDestroyOnLoad(gameObject);
    //	}
    //}

    void Move()
    {

    }

    void Attack()
    {

    }

    public float GetDamageReduction(float aDef)
    {
        return aDef / (float)MaxDef;
    }

    #region This is buff attribute 
    public float B_MaxHP
    {
        get { return b_maxHP; }
        set { b_maxHP = value; }
    }
    public float B_MoveSpeed
    {
        get { return b_moveSpeed; }
        set { b_moveSpeed = value; }
    }
    public float B_DashSpeed
    {
        get { return b_dashSpeed; }
        set { b_dashSpeed = value; }
    }
    public float B_AttackSpeed
    {
        get { return b_attackSpeed; }
        set { b_attackSpeed = value; }
    }
    public int B_Def
    {
        get { return b_def; }
        set { b_def = value; }
    }
    public int B_CritChance
    {
        get { return b_critChance; }
        set { b_critChance = value; }
    }
    public int B_CritDMG
    {
        get { return b_critDmg; }
        set { b_critDmg = value; }
    }
    public int B_Penetration
    {
        get { return b_penetration; }
        set { b_penetration = value; }
    }
    public float B_Atk
    {
        get { return b_atk; }
        set { b_atk = value; }
    }
    public float B_MagicAtk
    {
        get { return b_magicAtk; }
        set { b_magicAtk = value; }
    }
    #endregion

    #region This is equiment attribute
    public float E_MaxHP
    {
        get { return e_maxHP; }
        set { e_maxHP = value; }
    }
    public float E_MoveSpeed
    {
        get { return e_moveSpeed; }
        set { e_moveSpeed = value; }
    }
    public float E_DashSpeed
    {
        get { return e_dashSpeed; }
        set { e_dashSpeed = value; }
    }
    public float E_AttackSpeed
    {
        get { return e_attackSpeed; }
        set { e_attackSpeed = value; }
    }
    public int E_Def
    {
        get { return e_def; }
        set { e_def = value; }
    }
    public int E_CritChance
    {
        get { return e_critChance; }
        set { e_critChance = value; }
    }
    public int E_CritDMG
    {
        get { return e_critDmg; }
        set { e_critDmg = value; }
    }
    public int E_Penetration
    {
        get { return e_penetration; }
        set { e_penetration = value; }
    }
    public float E_Atk
    {
        get { return e_atk; }
        set { e_atk = value; }
    }
    public float E_MagicAtk
    {
        get { return e_magicAtk; }
        set { e_magicAtk = value; }
    }
    #endregion

    #region This is base attribute of entity
    public int BaseDef
    {
        get { return baseDef; }
        set { baseDef = value; }
    }
    public int BaseCritChance
    {
        get { return baseCritChance; }
        set { baseCritChance = value; }
    }
    public int BaseCritDMG
    {
        get { return baseCritDMG; }
        set { baseCritDMG = value; }
    }
    public int BasePenetration
    {
        get { return basePenetration; }
        set { basePenetration = value; }
    }
    public float BaseMaxHP
    {
        get { return baseMaxHP; }
        set { baseMaxHP = value; }
    }
    public float BaseMoveSpeed
    {
        get { return baseMoveSpeed; }
        set { baseMoveSpeed = value; }
    }
    public float BaseDashSpeed
    {
        get { return baseDashSpeed; }
        set { baseDashSpeed = value; }
    }
    public float BaseAttackSpeed
    {
        get { return baseAttackSpeed; }
        set { baseAttackSpeed = value; }
    }
    public float BaseAtk
    {
        get { return baseAtk; }
        set { baseAtk = value; }
    }
    public float BaseMagicAtk
    {
        get { return baseMagicAtk; }
        set { baseMagicAtk = value; }
    }
    #endregion

    #region This is main attribute
    public float MagicAtk
    {
        get { return baseMagicAtk + e_magicAtk + b_magicAtk; }
    }

    public float MoveSpeed
    {
        get { return baseMoveSpeed + e_moveSpeed + b_moveSpeed; }
    }
    public float DashSpeed
    {
        get { return baseDashSpeed + e_dashSpeed + b_dashSpeed; }
    }
    public float AttackSpeed
    {
        get
        {
            float temp = baseAttackSpeed + e_attackSpeed + b_attackSpeed;
            return (temp <= 0.8f ? 0.8f : temp);
        }
    }
    public float MaxHP
    {
        get { return baseMaxHP + e_maxHP + b_maxHP; }
    }

    public float CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
    public float Atk
    {
        get { return baseAtk + e_atk + b_atk; }
    }

    public int Def
    {
        get
        {
            //int temp = baseDef + e_def + b_def;
            //return (temp >= MaxDef ? MaxDef : temp);
            return baseDef + e_def + b_def; 
        }
    }

    public int CritChance
    {
        get
        {
            int temp = baseCritChance + e_critChance + b_critChance;
            return (temp >= 100 ? 100 : temp);
        }
    }

    public int CritDmg
    {
        get { return baseCritDMG + e_critDmg + b_critDmg; }
    }

    public int Penetration
    {
        get {
            int temp = basePenetration + e_penetration + b_penetration;
            return (temp >= 100 ? 100 : temp);
        }
    }

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public bool IsAttacked
    {
        get { return isAttacked; }
        set { isAttacked = value; }
    }

    public bool IsDashing
    {
        get { return isDashing; }
        set { isDashing = value; }
    }
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public bool CanAttack
    {
        get { return canAttack; }
        set { canAttack = value; }
    }
    public bool CanDash
    {
        get { return canDash; }
        set { canDash = value; }
    }
    #endregion
}
