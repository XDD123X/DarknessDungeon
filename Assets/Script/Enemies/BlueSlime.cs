using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : MonoBehaviour, IEnemy
{

    private Animator animator;
    private Rigidbody2D rb;
    private EnemyHealth script;
    private ObjectPool op;
    private BuffHolder bf;

    private PolygonCollider2D PolygonCollider2D;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        script = GetComponent<EnemyHealth>();
        op = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        bf = GetComponent<BuffHolder>();

        PolygonCollider2D = GetComponent<PolygonCollider2D>();
        PolygonCollider2D.enabled = false;
    }
    public void Attack()
    {
        animator.SetTrigger("isAttack");
        PolygonCollider2D.enabled=true;
    }

    public void DoneAttack()
    {
        animator.SetTrigger("isAttack");
        PolygonCollider2D.enabled = false;
    }


    public void Move(Vector2 move)
    {
        if (move != Vector2.zero)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }

    public void Dead()
    {

        script.IsDead = true;
        script.ResetHealthbar();
        animator.SetTrigger("isDead");
        
    }
    public void Spawn()
    {
        animator.SetTrigger("isSpawned");
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setDeActivate()
    {
        op.ReturnToPool(this.gameObject.ToString(), this.gameObject);
        ResetAttribute();
    }

    void ResetAttribute()
    {
        script.IsDead = false;
        if (bf != null)
        {
            // Reset all effect
            foreach (Buff b in bf.buffs)
            {
                bf.RemoveBuff(b);
            }
        }
        script.CurrentHP = script.MaxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(script.CurrentHP);
        PlayerController controller = collision.GetComponent<PlayerController>();   
        if (controller != null)
        {
            Boolean isCrit = (UnityEngine.Random.Range(0, 100) <= controller.CritChance);
            float dmg;
            // Total damage before armor reduction
            dmg = script.Atk * (isCrit ? 1 + script.CritDmg / 100f : 1);
            // Real damage will receive
            dmg -= dmg * controller.GetDamageReduction(controller.Def * (100 - script.Penetration) / 100f);
            // Remain 1 dmg through 100% damage reduction
            if (dmg <= 0) dmg = 1;
            controller.TakeDamage(this.transform, dmg);
        }
    }
}
