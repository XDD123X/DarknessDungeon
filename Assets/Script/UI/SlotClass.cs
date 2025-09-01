using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;
    public static int INIT = 0;
    private int ID;
    public SlotClass()
    {
        item = null;
        quantity = 0;
        ID = ++INIT;
    }

    public SlotClass(ItemClass item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
        ID = ++INIT;
    }    

    public SlotClass(SlotClass sc)
    {
        item = sc.GetItem();
        quantity = sc.GetQuantity();
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
    public ItemClass GetItem() { return item; }
    public int GetID()
    {
        return ID;
    }
    public int GetQuantity() { return quantity; }

    public void SetQuantity(int quantity) { this.quantity = quantity; }
    public void AddQuantity(int quantity) { this.quantity += quantity;}
    public void SetItem(ItemClass item) {  this.item = item; }
    public void SetSlot(ItemClass item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
