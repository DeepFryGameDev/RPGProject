using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemyDBEntry
{
    public string name;
    public BaseEnemy enemy; //contains enemy details
    public GameObject prefab;
    public string description;
}
