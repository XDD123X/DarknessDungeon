using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ThienXaDummy : MonoBehaviour
{
    public float damage;
    public float decayTime = 5f;
    public float radius = 1f;
    public Boolean canCrit;

    public EntityInterface caster;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = decayTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        } else
        {
            
            foreach (GameObject target in CollectObjectsWithinRadius(this.gameObject.transform.position, radius))
            {
                if (caster.gameObject.CompareTag("Player") && target.CompareTag("Player")) return;
                if (caster.gameObject.CompareTag("Enemy") && target.CompareTag("Enemy")) return;

                // Algorithm for damage
                EntityInterface script = target.GetComponent<EntityInterface>();
                if (script != null)
                {
                    float dmg = 0;
                    if (canCrit)
                    {
                        Boolean isCrit = (UnityEngine.Random.Range(0, 100) <= caster.CritChance);
                        dmg = (isCrit ? caster.CritDmg * damage : damage);
                    } else
                    {
                        dmg = damage;
                    }
                    dmg -= script.GetDamageReduction(script.Def) * dmg;
                    if (target.CompareTag("Enemy"))
                    {
                        (script as EnemyHealth).TakeDamage(dmg);
                    } else
                    {
                        (script as PlayerController).TakeDamage(dmg);
                    }
                }
            }
            Destroy(this.gameObject);
        }
    }
    protected List<GameObject> CollectObjectsWithinRadius(Vector3 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        List<GameObject> objects = new List<GameObject>();

        foreach (Collider2D collider in hitColliders)
        {
            objects.Add(collider.gameObject);
        }

        return objects;
    }

}
