using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSupport : MonoBehaviour
{
    EntityInterface entityInterface;
    Waves scriptWave;
    ObjectPool pooler;
    public string key;

    // Start is called before the first frame update
    void Start()
    {
        scriptWave = GameObject.FindGameObjectWithTag("Setting").GetComponent<Waves>(); 
        entityInterface = GetComponent<EntityInterface>();
        pooler = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (entityInterface.IsDead)
        {
            scriptWave.leftOver--;
            pooler.ReturnToPool(key ,this.gameObject);
        }
    }

}
