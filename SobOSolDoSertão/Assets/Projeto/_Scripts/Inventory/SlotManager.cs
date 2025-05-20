using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private Transform parentSlots;

    public List<Slot> slots = new();

    public GameObject prefab_item;
    public GameObject prefab_item_doble;
    private List<int> pontas = new();

    void Start()
    {
        LoadSlots();
        
    }

    void LoadSlots()
    {
        //Clear
        slots.Clear();

        //Add
        for (int i = 0; i < parentSlots.childCount; i++) slots.Add(parentSlots.GetChild(i).GetComponent<Slot>());

        pontas.Add(3);
        pontas.Add(7);
        pontas.Add(11);
        pontas.Add(19);
    }

    public bool Create(Item item)
    {
        if (item.slots == 1)
        {
            if (item.group)
            {
                foreach (var slot in slots)
                {
                    if (slot.full && slot.transform.childCount > 0
                    && slot.transform.GetChild(0).GetComponent<ItemInventory>().item == item &&
                    slot.transform.GetChild(0).GetComponent<ItemInventory>().qtd < item.maxGroup)
                    {
                        return Reload(slot.transform.GetChild(0).GetComponent<ItemInventory>());
                    }
                }
            }

            foreach (var slot in slots)
            {
                if (!slot.full)
                {
                    CreateItem(slot.transform, item);
                    slot.Complete();
                    return true;
                }
            }
        }
        else
        {
            if (item.group)
            {
                foreach (var slot in slots)
                {
                   if (slot.full && slot.transform.childCount > 0
                    && slot.transform.GetChild(0).GetComponent<ItemInventory>().item == item &&
                    slot.transform.GetChild(0).GetComponent<ItemInventory>().qtd < item.maxGroup)
                    {
                        return Reload(slot.transform.GetChild(0).GetComponent<ItemInventory>());
                    }
                }
            }


            for (int i = 0; i < slots.Count; i++)
            {
                bool pont = false;
                var slot = slots[i];
                foreach (var j in pontas){
                    if (i == j)
                    {
                        pont = true;
                        break;
                    }
                }

                if (!pont)
                {
                    if (!slot.full)
                    {
                        if (slots[i + 1] && !slots[i + 1].full)
                        {
                            int[] indices = new int[2];
                            indices[0] = i;
                            indices[1] = i + 1;

                            CreateItemDoble(slots[i + 1].transform, item, indices);
                            slot.Complete();
                            slots[i + 1].Complete();

                            return true;
                        }

                        else if (slots[i - 1] && !slots[i - 1].full)
                        {
                            int[] indices = new int[2];
                            indices[0] = i;
                            indices[1] = i - 1;
                            CreateItemDoble(slot.transform, item, indices);
                            slot.Complete();
                            slots[i - 1].Complete();

                            return true;
                        }
                    }
                }
            }
            
            
        }

        GameManager.controller.Alert("O inventario esta cheio!");
        return false;
    }

    void CreateItem(Transform parent, Item item)
    {
        var itemSlot = Instantiate(prefab_item, parent);
        itemSlot.GetComponent<ItemInventory>().Style(item);
    }

    void CreateItemDoble(Transform parent, Item item, int[] indices)
    {
        var itemSlot = Instantiate(prefab_item_doble, parent);
        itemSlot.GetComponent<ItemInventory>().Style(item,indices);
    }

    public bool Reload(ItemInventory item)
    {
        item.AddQtd(1);
        return true;
    }

    public bool Delete(GameObject slot,Item item)
    {
        return false;
    }

}
