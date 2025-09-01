using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thien Xa", menuName = "Skill/Thien Xa")]
public class ThienXa : Ability
{
    [Header("Thien Xa")]
    public float radius = 50f;
    public GameObject lightning;
    public int amount = 10;
    public int wavesTime = 3;
    private int timer;

    // Dmg base + (dmg base caster * magic dmg of caster)
    public float DamageBase = 100f;
    public float DamageBaseCaster = 0.1f;

    public float RadiusOfLightning = 1f;

    public Boolean canCrit = false;

    public override void Activate(GameObject caster)
    {
        Caster = caster;
    }

    public override void EndSkill()
    {
        timer = 0;
    }
    public override void DuringSkill()
    {
        timer++;
        if (timer % wavesTime == 0)
        {
            for (int index = 0; index < amount; index++)
            {
                Vector2 pos = GetRandomPositionInCircle(Caster.transform.position, radius);

                GameObject create = Instantiate(lightning, pos, Quaternion.identity);
                // Let the object have the script
                ThienXaDummy txd = create.GetComponent<ThienXaDummy>();

                txd.damage = DamageBase + DamageBaseCaster * Caster.GetComponent<EntityInterface>().MagicAtk;

                txd.radius = RadiusOfLightning;
                txd.canCrit = canCrit;
                txd.caster = Caster.GetComponent<EntityInterface>();
            }
            timer = 0;
        }
    }

    Vector2 GetRandomPositionInCircle(Vector2 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);


        float distance = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f)) * radius;


        float x = center.x + distance * Mathf.Cos(angle);
        float y = center.y + distance * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}