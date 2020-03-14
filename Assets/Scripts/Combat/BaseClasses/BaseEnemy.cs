using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy : BaseClass
{
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

    public int earnedEXP;

    public int goldDropped;

    public List<BaseItemDrop> itemsDropped = new List<BaseItemDrop>();
}
