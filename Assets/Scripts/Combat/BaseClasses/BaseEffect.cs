using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseEffect //For status effects
{
    public string effectName; //name of status effect
    public string effectType; //used to apply from effectType (in BaseStatusEffect)
    public int turnsRemaining; //turns remaining in the battle
    public int baseValue; //value for the status effect

    public StatusEffect effect; //to store the status effect to be applied
}
