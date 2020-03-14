using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStatusEffect
{
    public string _Name;
    
    public enum EffectTypes
    {
        POISON,
        ATTACKDOWN,
        ATTACKUP,
        MAGICATTACKDOWN,
        MAGICATTACKUP,
        DEFENSEDOWN,
        DEFENSEUP,
        MAGICDEFENSEDOWN,
        MAGICDEFENSEUP
    }
    public EffectTypes effectType;
    public int turnsApplied;

    public int baseValue;
    public bool canStack;

    [HideInInspector] public int turnApplied;
}
