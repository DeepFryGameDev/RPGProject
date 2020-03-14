using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseTroop //troops are enemy groups that can be called when initiating battle via script or region
{
    public string _Name; //to identify each troop
    public float encounterChance; //not yet implemented - chance to encounter from region
    public List<GameObject> enemies = new List<GameObject>(); //which enemies
}
