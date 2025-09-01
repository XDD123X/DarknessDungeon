using System;
using System.Collections.Generic;
using UnityEngine;

public class ListDropItem : MonoBehaviour
{
    [System.Serializable]
    public class DropTable
    {
        public List<ItemClass> items;
        public float chance;
        public float propRealChance;
        public float RealChance
        {
            get { return propRealChance; }
            set { propRealChance = value; }
        }
    }

    public DropTable[] dropTables;
    public float dropChance = 50f;
    public EntityInterface script;
    public GameObject prefabLoot;

    private float totalChance;
    private Boolean isDroping;

    // Start is called before the first frame update
    void Start()
    {
        totalChance = 0;
        foreach (DropTable dt in dropTables)
        {
            totalChance += dt.chance;
        }
        foreach (DropTable dt in dropTables)
        {
            dt.RealChance = dt.chance / totalChance * 100;
        }
        Array.Sort(dropTables, (x, y) => y.RealChance.CompareTo(x.RealChance));
        for (int i = 1;i< dropTables.Length; i++)
        {
            dropTables[i].RealChance += dropTables[i-1].RealChance;
        }
        script = GetComponent<EntityInterface>();
    }
    public void Reset()
    {
        isDroping = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (script.IsDead && UnityEngine.Random.Range(1, 100) <= dropChance && !isDroping)
        {
            GameObject loot = Instantiate(prefabLoot, this.transform.position, Quaternion.identity);
            ItemDrop id = loot.GetComponent<ItemDrop>();

            int dropTable = UnityEngine.Random.Range(0, 100);
            foreach (DropTable dt in dropTables)
            {

                //if (dt.items.Count == 0) continue;
                if (dropTable <= dt.RealChance)
                {
                    isDroping = true;

                    int randomIndex = UnityEngine.Random.Range(0, dt.items.Count);
                    //Debug.Log(randomIndex + " / " + dt.items[randomIndex]);
                    id.item = dt.items[randomIndex];
                    id.amount = 1;
                    break;
                }
            }
        }
    }
}
