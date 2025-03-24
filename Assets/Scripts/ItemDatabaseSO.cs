using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Datebase")]

public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();

    private Dictionary<int, ItemSO> itemsByld;
    private Dictionary<string, ItemSO> itemsByname;

    public void Initialize()
    {
        itemsByld = new Dictionary<int, ItemSO>();
        itemsByname = new Dictionary<string, ItemSO>();

        foreach (var item in items)
        {
            itemsByld[item.id] = item;
            itemsByname[item.itemName] = item;

        }


    }

    //ID�� ������ ã��

    public ItemSO GetItemByld(int id)
    {
        if (itemsByld == null)
        {
            Initialize();
        }
        if (itemsByld.TryGetValue(id, out ItemSO item))
            return item;

        return null;
    }

    //�̸����� ������ ã��

    public ItemSO GetltemByname(string name)
    {
        if (itemsByname == null)
        {
            Initialize();
        }
        if (itemsByname.TryGetValue(name, out ItemSO item))
            return item;

        return null;
    }

    //Ÿ������ ������ ���͸�

    public List<ItemSO> GetItemByType(ItemType type)
    {
        return items.FindAll(item => item.itemType == type);
    }



}