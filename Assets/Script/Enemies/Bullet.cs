using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private bool isEnemyBullet = false;
    [SerializeField] private float BulletRange = 10f;
    public float dmg;
    public EnemyHealth enemyHealth;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveBullet();
        DetectFireDistance();
    }

    public void UpdateBulletRange(float BulletRange)
    {
        this.BulletRange = BulletRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            Boolean isCrit = (UnityEngine.Random.Range(0, 100) <= controller.CritChance);
            // Total damage before armor reduction
            dmg = enemyHealth.Atk * (isCrit ? 1 + enemyHealth.CritDmg / 100f : 1);
            // Real damage will receive
            dmg -= dmg * controller.GetDamageReduction(controller.Def * (100 - enemyHealth.Penetration) / 100f);
            // Remain 1 dmg through 100% damage reduction
            if (dmg <= 0) dmg = 1;
            controller.TakeDamage(this.transform,dmg);
            Destroy(gameObject);
        }
        else
        {
            if (other.tag != "Enemy" && other.tag != "enemyBullet")
            {
                Destroy(gameObject);
            }
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > BulletRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveBullet()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
