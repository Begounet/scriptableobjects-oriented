using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOList<ItemType> : SOBaseVariable
{
    public ItemType this[int idx]
    {
        get { return list[idx]; }
        set { list[idx] = value; }
    }

    public int Count
    {
        get { return list.Count; }
    }


    public List<ItemType> list;


    public void Add(ItemType item)
    {
        InitIFN();
        list.Add(item);
    }

    public bool IsValidIndex(int idx)
    {
        return (idx >= 0 && idx < list.Count);
    }

    private void InitIFN()
    {
        if (list == null)
        {
            list = new List<ItemType>();
        }
    }
}
