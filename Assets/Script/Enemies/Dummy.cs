using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Dummy : MonoBehaviour
{
    private float damageAmount;
    PlayerController controller;
    WeaponClass weapon;
    private Animator animator;
    private Flash fl;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        fl= GetComponent<Flash>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageSource damageSource = collision.GetComponent<DamageSource>();
        if (damageSource != null)
        {
            weapon = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>().GetWeapon();
            Boolean isCrit = (UnityEngine.Random.Range(0, 100) <= controller.CritChance);
            damageAmount = (weapon.weaponType == WeaponClass.WeaponType.Staff ? controller.MagicAtk : controller.Atk)
                * (isCrit ? controller.CritDmg / 100f : 1);
            if (damageAmount <= 0) damageAmount = 1;
        }

        StartCoroutine(fl.FlashRoutine());
        if (damageAmount >0 && damageAmount < 50)
        {
            animator.SetTrigger("hitLight");
        }
        if (damageAmount >= 50)
        {
            animator.SetTrigger("hitLight");
        }
    }
}
