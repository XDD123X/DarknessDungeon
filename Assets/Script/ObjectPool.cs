using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, int> activeObjectsCount;

    public void Add(string tag, GameObject prefab, int size)
    {
        Pool pool = new Pool()
        {
            tag = tag,
            prefab = prefab,
            size = size
        };

        pools.Add(pool);

        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj;
            obj = Instantiate(pool.prefab);
            obj.AddComponent<WaveSupport>();
            obj.GetComponent<WaveSupport>().SetActive(false);
            obj.GetComponent<WaveSupport>().key = tag;
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(pool.tag, objectPool);
        activeObjectsCount.Add(pool.tag, 0);
    }
    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        activeObjectsCount = new Dictionary<string, int>();
    }
    void Start()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj;
                if (pool.tag == "Healthbar" || pool.tag == "floatingdamage")
                {
                    obj = Instantiate(pool.prefab, GameObject.FindGameObjectWithTag("HealthbarUI").transform);
                }
                else
                {
                    obj = Instantiate(pool.prefab);
                }
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
            activeObjectsCount.Add(pool.tag, 0);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning("No available objects in pool with tag " + tag);
            return null;
        }


        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        Rigidbody2D entityBody = objectToSpawn.GetComponent<Rigidbody2D>();
        if (entityBody != null)
        {
            entityBody.velocity = Vector2.zero;
            entityBody.angularVelocity = 0f;
        }

        // Display healthbar
        if (objectToSpawn.CompareTag("Enemy"))
        {
            GameObject healthbar = poolDictionary["Healthbar"].Dequeue();
            healthbar.SetActive(true);

            healthbar.transform.position = position;
            healthbar.transform.rotation = rotation;
            activeObjectsCount["Healthbar"]++;
            EnemyHealthbar hbScript = healthbar.GetComponent<EnemyHealthbar>();
            hbScript.target = objectToSpawn.transform;

            EnemyHealth enemyScript = objectToSpawn.GetComponent<EnemyHealth>();
            enemyScript.healthbar = hbScript;

            objectToSpawn.GetComponent<WaveSupport>().SetActive(true);
        }


        objectToSpawn.SetActive(true);


        objectToSpawn.transform.position = position;
        if (!objectToSpawn.CompareTag("Enemy")) {
            objectToSpawn.transform.rotation = rotation;

        }

        activeObjectsCount[tag]++;


        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return;
        }
        ListDropItem ldi = objectToReturn.GetComponent<ListDropItem>();
        if (ldi != null)
        {
            ldi.Reset();
        }
        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
        activeObjectsCount[tag]--;
    }

    public bool CanSpawn(string tag)
    {
        return poolDictionary.ContainsKey(tag) && activeObjectsCount[tag] < pools.Find(pool => pool.tag == tag).size;
    }
}
