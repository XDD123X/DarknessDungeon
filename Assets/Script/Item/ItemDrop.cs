using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public ItemClass item;
    public int amount;
    private InventoryManagement inventoryManagement;
    private ObjectPool pooler;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManagement = GameObject.Find("Canvas").GetComponent<InventoryManagement>();
        pooler = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SelfDestroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inventoryManagement.Add(item, amount);
            //pooler.ReturnToPool("Item", this.gameObject);
            SelfDestroy();

        }
    }
}
