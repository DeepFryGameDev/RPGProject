using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseClass
{
    //all heros and enemies use these values
    [HideInInspector] public string name;
    [ReadOnly] public int ID;
}
