using UnityEngine;

[CreateAssetMenu(fileName = "Du Chan", menuName = "Skill/Du Chan")]
public class DuChan : Ability
{
    [Header("Du Chan")]
    public float radius = 50f;
    public override void Activate(GameObject caster)
    {
        Caster = caster;
        Targets = CollectObjectsWithinRadius(Caster.transform.position, radius);
        BuffSettings bf = GameObject.FindGameObjectWithTag("Setting").gameObject.GetComponent<BuffSettings>();
        foreach (GameObject target in Targets)
        {
            if (target == Caster) continue;
            EntityInterface script = target.GetComponent<EntityInterface>();
            if (script != null)
            {
                script.CanMove = false;
                script.CanDash = false;
            }
            BuffHolder buffHolder = target.GetComponent<BuffHolder>();
            Buff slow = bf.buff_SPEED;
            slow.BaseMultiply = -0.2f;
            slow.SetLevel(1);
            slow.isMultiply = true;
            slow.name = this.name;
            slow.icon = this.iconEnable; 
            
            if (buffHolder != null)
            {
                Buff.ApplyBuff(slow, target);
            }
        }
    }

    public override void EndSkill()
    {
        foreach (GameObject target in Targets)
        {
            EntityInterface script = target.GetComponent<EntityInterface>();
            if (script != null)
            {
                script.CanMove = true;
                script.CanDash = true;
            }
        }
    }
    public override void DuringSkill()
    {
        Debug.Log("Mot giay");
    }
}
