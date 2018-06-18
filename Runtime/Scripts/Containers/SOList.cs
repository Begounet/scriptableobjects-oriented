using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOList<ItemType> : SOBaseVariable
{
    public List<ItemType> list;

    public void Add(ItemType item)
    {
        list.Add(item);
    }
}
