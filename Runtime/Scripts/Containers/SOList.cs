using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOList<ItemType> : SOBaseVariable
{
    public List<ItemType> list;

    public void Add(ItemType item)
    {
        InitIFN();
        list.Add(item);
    }

    private void InitIFN()
    {
        if (list == null)
        {
            list = new List<ItemType>();
        }
    }
}
