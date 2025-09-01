using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [Header("Ability")]
    public int id;
    public Sprite iconEnable;
    public Sprite iconDisable;
    public string Name;
    public string description;
    public float activeTime;
    public float CDTime;
    [SerializeField] private int level;

    private GameObject caster;
    private List<GameObject> targets;

    public int Level
    {
        get { return (level <= 0 ? 1 : level); }
        set { level = value; }
    }
    public GameObject Caster
    {
        get { return caster; }
        set { caster = value; }
    }

    public List<GameObject> Targets
    {
        get { return targets; }
        set { targets = value; }
    }

    public virtual void Activate(GameObject caster)
    {
    }
    public virtual void DuringSkill() { }
    public virtual void EndSkill() { }

    public override string ToString()
    {
        return base.ToString();
    }

    public override bool Equals(object other)
    {
        Ability compare = other as Ability;
        //return this.name.Equals(compare.name);
        return this.id == compare.id;
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
