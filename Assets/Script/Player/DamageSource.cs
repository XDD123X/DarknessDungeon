using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class DamageSource : MonoBehaviour
{
    private float damageAmount;
    PlayerController controller;
    WeaponClass weapon;
    ObjectPool op;
    Canvas damageUI;
    Camera mainCamera;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GameObject tryOP = GameObject.Find("ObjectPool");
        if (tryOP != null)
        {
            op = tryOP.GetComponent<ObjectPool>();
        }
        damageUI = GameObject.FindGameObjectWithTag("HealthbarUI").GetComponent<Canvas>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (this.tag == "aoe")
        {
            damageAmount = 50;
            enemyHealth?.TakeDamage(this.transform, damageAmount);
        }
        else
        {
            if (enemyHealth != null)
            {
                weapon = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>().GetWeapon();

                // ===============================
                // Crit attack
                Boolean isCrit = (UnityEngine.Random.Range(0, 100) <= controller.CritChance);
                // Total damage before armor reduction
                damageAmount = (weapon.weaponType == WeaponClass.WeaponType.Staff ? controller.MagicAtk : controller.Atk)
                    * (isCrit ? controller.CritDmg / 100f : 1);
                // Real damage will receive
                damageAmount -= damageAmount * enemyHealth.GetDamageReduction(enemyHealth.Def * (100 - controller.Penetration) / 100f);
                // Remain 1 dmg through 100% damage reduction
                if (damageAmount <= 0) damageAmount = 1;
                // ================================
                //Vector3 realPos = Camera.main.WorldToScreenPoint(other.gameObject.transform.position);

                GameObject dmgText = op.SpawnFromPool("floatingdamage", other.gameObject.transform.position, Quaternion.identity);
                controller.PlaySound("Enemy");
                // Display dmgs
                FloatingDamage fd = dmgText.GetComponent<FloatingDamage>();
                fd.Show(damageAmount);

                enemyHealth?.TakeDamage(this.transform, damageAmount);
            }
            IWeaponDmgDeal dmgDeal = GetComponent<IWeaponDmgDeal>();
            if (dmgDeal != null)
            {
                dmgDeal.DoneDealDmg(other);
            }
        }
    }
}
