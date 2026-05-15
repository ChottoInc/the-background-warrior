using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private ItemSO itemSO;


    public ItemSO ItemSO => itemSO;


    public Item(ItemSO itemSO)
    {
        this.itemSO = itemSO;
    }
}
