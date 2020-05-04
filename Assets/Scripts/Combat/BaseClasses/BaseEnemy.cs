using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy : BaseClass
{
    public int level;

    public int baseHP; //max HP
    public int baseMP; //max MP

    [ReadOnly] public int curHP; //current HP
    [ReadOnly] public int curMP; //current MP

    //base attack values and current attack values - base is their max value, while current could be modified at any point
    public int baseATK;
    public int baseMATK;
    public int baseDEF;
    public int baseMDEF;


    //enemies need to be implemented for all of these
    public int baseStrength; //for calculating physical attack damage
    public int baseStamina; //for calculating HP
    public int baseDexterity; //for calculating ATB gauge speed
    public int baseAgility; //for calculating dodge/crit (not yet implemented)
    public int baseIntelligence; //for calculating magic damage
    public int baseSpirit; //for calculating MP regeneration, magic defense (not yet implemented)

    public int baseHitRating;
    public int baseCritRating;
    public int baseMoveRating;
    public int baseRegenRating;

    public int baseDodgeRating;
    public int baseBlockRating;
    public int baseParryRating;
    public int baseThreatRating;

    public enum Type //can add weaknesses and stuff later
    {
        GRASS,
        FIRE,
        WATER,
        ELECTRIC
    }

    public enum Rarity //can use this later as possibility to encounter
    {
        COMMON,
        UNCOMMON,
        RARE,
        EPIC
    }

    public Type enemyType; //to access the assigned type
    public Rarity rarity; //to access the assigned rarity

    public List<BaseAttack> attacks = new List<BaseAttack>(); //possible attacks

    public int earnedEXP;

    public int goldDropped;

    public List<BaseItemDrop> itemsDropped = new List<BaseItemDrop>();
}
