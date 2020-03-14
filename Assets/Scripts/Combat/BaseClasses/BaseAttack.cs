using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour
{
    new public string name; //Name of Attack
    public string description; //Used for menu interface - not yet implemented
    public int damage; //Base Damage
    public int MPCost; //cost of attack
    public int cooldown; //not yet implemented - will be used for number of turns between each time spell can be cast

    public int patternIndex;
    public int rangeIndex;

    public float threatMultiplier = 1; //how much threat does this attack apply?

    public enum Type
    {
        PHYSICAL,
        MAGIC
    }
    public Type type; //used to determine how to calculate damage
    public List<BaseStatusEffect> statusEffects = new List<BaseStatusEffect>(); //contains all status effects that the spell will inflict/heal
    public enum MagicClass
    {
        WHITE,
        BLACK,
        SORCERY
    }
    public MagicClass magicClass; //used for determining how statusEffects list is affected, as well as how magic is displayed in menu
    public enum UseStates
    {
        HERO,
        ALLHEROES,
        ENEMY,
        ALLENEMIES
    }
    public UseStates useState; //used to determine which targets the spell will effect in battle
    public bool usableInMenu; //if spell can be cast in menu
    [HideInInspector] public bool enoughMP; //checks to determine if spell can be cast due to not having enough MP
}
