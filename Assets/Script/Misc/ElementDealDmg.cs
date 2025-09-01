using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDealDmg : MonoBehaviour
{
    [SerializeField] public float dmg;
    // Start is called before the first frame update
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            animator.SetTrigger("isAttacked");
            dmg -= dmg * controller.GetDamageReduction(controller.Def);
            if (dmg <= 0) dmg = 1;
            controller.TakeDamage(this.transform, dmg);
            
        }
       
    }
}
