using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEquipment
{
    [ReadOnly] public string name;
    [ReadOnly] public int ID;
    
    public Equipment equipment;
}
