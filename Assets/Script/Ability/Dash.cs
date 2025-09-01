using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Dash", menuName = "Skill/Dash")]
public class Dash : Ability 
{

    EntityInterface scrip;
    float currSpeed;
    public override void Activate(GameObject caster)
    { 
        Caster = caster;
        scrip = caster.GetComponent<EntityInterface>();

        scrip.IsDashing = true;
        currSpeed = scrip.BaseMoveSpeed;
        scrip.BaseMoveSpeed = scrip.DashSpeed;
        caster.GetComponent<TrailRenderer>().emitting = true;
        
    }

    public override void EndSkill()
    {
        Caster.GetComponent<TrailRenderer>().emitting = false;
        scrip.BaseMoveSpeed = currSpeed;
        scrip.IsDashing = false;
    }
}
