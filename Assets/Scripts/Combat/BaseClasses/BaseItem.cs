using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem
{
    [ReadOnly] public string name;
    [ReadOnly] public int ID;
    
    public Item item;
}
