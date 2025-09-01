using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityInterface
{
    [SerializeField] private float knockBackThrust = 15f;

    private Knockback knockback;
    private Flash flash;
    [SerializeField] private MonoBehaviour enemyType;
    public EnemyHealthbar healthbar;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (healthbar != null)
            healthbar.SetHealthBar(CurrentHP, MaxHP);
        DeadCheck();
    }

    public void TakeDamage(Transform damageSource, float damage)
    {
        CurrentHP -= damage;
        if (healthbar != null)
            healthbar.SetHealthBar(CurrentHP, MaxHP);
        knockback.GetKnockedBack(damageSource, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }
    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DeadCheck();
    }
    public void ResetHealthbar()
    {
        healthbar.Reset();
    }
    private void DeadCheck()
    {
        if (CurrentHP <= 0)
        {
            (enemyType as IEnemy).Dead();
        }
    }
    private void caculatedmg()
    {

    }
}
