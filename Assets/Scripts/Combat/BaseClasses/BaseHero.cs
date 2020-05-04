using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BaseHero : BaseClass
{
    [ReadOnly] public string spawnPoint;

    public GameObject heroPrefab; //each hero needs its own prefab
    public Sprite faceImage;

    public int currentLevel = 1;
    public int currentExp;
    
    public int baseHP; //max HP
    public int baseMP; //max MP

    //base attack values and current attack values - base is their max value, while current could be modified at any point
    public int baseATK;
    public int baseMATK;
    public int baseDEF;
    public int baseMDEF;

    //enemies need to be implemented for all of these
    public int baseSTR; //for calculating physical attack damage
    public int baseSTA; //for calculating HP
    public int baseDEX; //for calculating ATB gauge speed
    public int baseAGI; //for calculating dodge/crit (not yet implemented)
    public int baseINT; //for calculating magic damage
    public int baseSPI; //for calculating MP regeneration, magic defense (not yet implemented)

    public int baseHit;
    public int baseCrit;
    public int baseMove;
    public int baseRegen;

    public int baseDodge;
    public int baseBlock;
    public int baseParry;
    public int baseThreat;

    //modifiers for leveling purposes.  The higher the modifier, the more effect they are at gaining that particular stat
    public float strengthMod;
    public float staminaMod;
    public float intelligenceMod;
    public float dexterityMod;
    public float agilityMod;
    public float spiritMod;

    public BaseTalent[] level1Talents = new BaseTalent[3];
    public BaseTalent[] level2Talents = new BaseTalent[3];
    public BaseTalent[] level3Talents = new BaseTalent[3];
    public BaseTalent[] level4Talents = new BaseTalent[3];
    public BaseTalent[] level5Talents = new BaseTalent[3];
    public BaseTalent[] level6Talents = new BaseTalent[3];

    public BaseAttack attack;
    public List<BaseAttack> MagicAttacks = new List<BaseAttack>(); //unit's magic attacks

    public Equipment[] equipment = new Equipment[System.Enum.GetNames(typeof(EquipmentSlot)).Length];

    [ReadOnly] public int curHP; //current HP
    [ReadOnly] public int maxHP;

    [ReadOnly] public int curMP; //current MP
    [ReadOnly] public int maxMP;

    [ReadOnly] public int currentStrength;
    [ReadOnly] public int currentStamina;
    [ReadOnly] public int currentAgility;
    [ReadOnly] public int currentDexterity;
    [ReadOnly] public int currentIntelligence;
    [ReadOnly] public int currentSpirit;
    [ReadOnly] public int currentATK;
    [ReadOnly] public int currentMATK;
    [ReadOnly] public int currentDEF;
    [ReadOnly] public int currentMDEF;

    [ReadOnly] public int currentHitRating;
    [ReadOnly] public int currentCritRating;
    [ReadOnly] public int currentMoveRating;
    [ReadOnly] public int currentRegenRating;

    [ReadOnly] public int currentDodgeRating;
    [ReadOnly] public int currentBlockRating;
    [ReadOnly] public int currentParryRating;
    [ReadOnly] public int currentThreatRating;

    [HideInInspector] public int levelBeforeExp;
    [HideInInspector] public int expBeforeAddingExp;

    public void InitializeStats()
    {
        maxHP = GetBaseMaxHP(baseHP);
        curHP = maxHP;
        maxMP = GetBaseMaxMP(baseMP);
        curMP = maxMP;

        currentStrength = baseSTR;
        currentStamina = baseSTA;
        currentAgility = baseAGI;
        currentDexterity = baseDEX;
        currentIntelligence = baseINT;
        currentSpirit = baseSPI;

        currentATK = baseATK;
        currentMATK = baseMATK;
        currentDEF = baseDEF;
        currentMDEF = baseMDEF;
        currentHitRating = baseHit;
        currentCritRating = baseCrit;
        currentMoveRating = baseMove;
        currentRegenRating = baseRegen;

        currentDodgeRating = baseDodge;
        currentBlockRating = baseBlock;
        currentParryRating = baseParry;
        currentThreatRating = baseThreat;
    }

    public void Equip(Equipment newEquip)
    {
        int slotIndex = (int)newEquip.equipmentSlot;

        Equipment oldEquipment = null;

        if (equipment[slotIndex] != null)
        {
            oldEquipment = equipment[slotIndex];
            Inventory.instance.Add(oldEquipment);
        }
        
        equipment[slotIndex] = newEquip;

        Inventory.instance.Remove(newEquip);
    }

    public void Unequip (int slotIndex)
    {
        if (equipment[slotIndex] != null)
        {
            Equipment oldEquipment = equipment[slotIndex];
            Inventory.instance.Add(oldEquipment);

            equipment[slotIndex] = null;
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < equipment.Length; i++)
        {
            Unequip(i);
        }
    }

    //public MultiDimensionalInt[] testArray;
    public void LevelUp()
    {
        currentLevel++;
        Debug.Log(name + " has leveled up from " + levelBeforeExp + " to " + currentLevel);
        ProcessStatLevelUps();
    }

    public void ProcessStatLevelUps()
    {
        //Debug.Log("Strength: " + strength + ", strengthMod: " + strengthMod);
        baseSTR = baseSTR + Mathf.RoundToInt(baseSTR * strengthMod);
        //Debug.Log("New strength: " + strength);

        //Debug.Log("Stamina: " + stamina + ", staminaModifier: " + staminaModifier);
        baseSTA = baseSTA + Mathf.RoundToInt(baseSTA * staminaMod);
        //Debug.Log("New stamina: " + stamina);

        //Debug.Log("Intelligence: " + intelligence + ", intelligenceModifer: " + intelligenceModifier);
        baseINT = baseINT + Mathf.RoundToInt(baseINT * intelligenceMod);
        //Debug.Log("New intelligence: " + intelligence);

        //Debug.Log("Spirit: " + spirit + ", spiritModifier: " + spiritModifier);
        baseSPI = baseSPI + Mathf.RoundToInt(baseSPI * spiritMod);
        //Debug.Log("New spirit: " + spirit);

        //Debug.Log("Dexterity: " + dexterity + ", dexterityModifier: " + dexterityModifier);
        baseDEX = baseDEX + Mathf.RoundToInt(baseDEX * dexterityMod);
        //Debug.Log("New dexterity: " + dexterity);

        //Debug.Log("Agility: " + agility + ", agilityModifier: " + agilityModifier);
        baseAGI = baseAGI + Mathf.RoundToInt(baseAGI * agilityMod);
        //Debug.Log("New agility: " + agility);

        UpdateBaseStats();

        learnNewAttacks();
    }

    //stats are affected by parameters here when leveling up
    void UpdateBaseStats()
    {
        baseATK = GetATK(baseATK);
        baseMATK = GetMATK(baseMATK);
        baseDEF = GetDEF(baseDEF);
        baseMDEF = GetMDEF(baseMDEF);

        baseHP = GetBaseMaxHP(baseHP);
        baseMP = GetBaseMaxMP(baseMP);

        curHP = baseHP; //if full heal should occur on levelup, using for debugging purposes for now
        curMP = baseMP; //if MP should be restored on levelup, using for debugging purposes for now

        GetCurrentStats();
    }

    void GetCurrentStats()
    {
        int tempStrength = 0, tempStamina = 0, tempAgility = 0, tempDexterity = 0, tempIntelligence = 0, tempSpirit = 0;
        int tempHP = 0, tempMP = 0;
        int tempATK = 0, tempMATK = 0, tempDEF = 0, tempMDEF = 0;
        int tempHit = 0, tempCrit = 0, tempMove = 0, tempRegen = 0;
        int tempDodge = 0, tempBlock = 0, tempParry = 0, tempThreat = 0;

        foreach (Equipment equipment in equipment)
        {
            if (equipment != null)
            {
                tempStrength += equipment.Strength;
                tempStamina += equipment.Stamina;
                tempAgility += equipment.Agility;
                tempDexterity += equipment.Dexterity;
                tempIntelligence += equipment.Intelligence;
                tempSpirit += equipment.Spirit;

                tempHP += Mathf.RoundToInt(equipment.Stamina * .75f);
                tempMP += Mathf.RoundToInt(equipment.Intelligence * .5f);

                tempATK += equipment.ATK + Mathf.RoundToInt(equipment.Strength * .5f);
                tempMATK += equipment.MATK + Mathf.RoundToInt(equipment.Intelligence * .5f);
                tempDEF += equipment.DEF + Mathf.RoundToInt(equipment.Stamina * .6f);
                tempMDEF += equipment.MDEF + Mathf.RoundToInt(equipment.Stamina * .5f);

                tempHit += equipment.hit;
                tempCrit += equipment.crit;
                tempMove += equipment.move;
                tempRegen += equipment.regen;

                tempDodge += equipment.dodge;
                tempBlock += equipment.block;
                tempParry += equipment.parry;
                tempThreat += equipment.threat;
            }
        }

        currentStrength = baseSTR + tempStrength;
        currentStamina = baseSTA + tempStamina;
        currentAgility = baseAGI + tempAgility;
        currentDexterity = baseDEX + tempDexterity;
        currentIntelligence = baseINT + tempIntelligence;
        currentSpirit = baseSPI + tempSpirit;

        maxHP = GetBaseMaxHP(baseHP) + tempHP;
        maxMP = GetBaseMaxMP(baseMP) + tempMP;

        currentATK = baseATK + tempATK;
        currentMATK = baseMATK + tempMATK;
        currentDEF = baseDEF + tempDEF;
        currentMDEF = baseMDEF + tempMDEF;

        currentHitRating = baseHit + tempHit;
        currentCritRating = baseCrit + tempCrit;
        currentMoveRating = baseMove + tempMove;
        currentRegenRating = baseRegen + tempRegen;

        currentDodgeRating = baseDodge + tempDodge;
        currentBlockRating = baseBlock + tempBlock;
        currentParryRating = baseParry + tempParry;
        currentThreatRating = baseThreat + tempThreat;
    }

    void learnNewAttacks()
    {
        if (name == "Test dude 1")
        {
            if (currentLevel == 4)
            {
                //Debug.Log("Learned the thing!");
            }
        }

        if (name == "Test dude 2")
        {
            if (currentLevel == 2)
            {
                //Debug.Log("Learned the other thing!");
            }
        }
    }

    //----------------------------------NOTE--------------------------------------
    //if formulas are updated, they should also be updated in GameMenu - ShowEquipmentStats, ChangeEquipment, DrawEquipMenuStats, ResetEquipmentUpdates, and DrawStatusMenuStats
    //----------------------------------------------------------------------------

    public int GetATK(int attack)
    {
        int ATK = Mathf.RoundToInt(attack + (currentStrength * .5f));

        return ATK;
    }

    public int GetMATK(int magicAttack)
    {
        int MATK = Mathf.RoundToInt(magicAttack + currentIntelligence * .5f);

        return MATK;
    }

    public int GetDEF(int defense)
    {
        int DEF = Mathf.RoundToInt(defense + (currentStamina * .6f));

        return DEF;
    }

    public int GetMDEF(int magicDefense)
    {
        int MDEF = Mathf.RoundToInt(magicDefense + (currentStamina * .5f));

        return MDEF;
    }

    public int GetBaseMaxHP(int hp)
    {
        int HP = Mathf.RoundToInt(hp + (baseSTA * .75f));

        return HP;
    }

    public int GetMaxHP(int hp)
    {
        int HP = Mathf.RoundToInt(hp + (currentStamina * .75f));

        return HP;
    }

    public int GetBaseMaxMP(int mp)
    {
        int MP = Mathf.RoundToInt(mp + (baseINT * .5f));

        return MP;
    }

    public int GetMaxMP(int mp)
    {
        int MP = Mathf.RoundToInt(mp + (currentIntelligence * .5f));

        return MP;
    }


    public int GetHitChance(int hitRating, int agility)
    {
        int hit = Mathf.FloorToInt(agility * .12f) + Mathf.FloorToInt(hitRating * .25f) + 75;

        if (hit > 100)
        {
            hit = 100;
        }

        return hit;
    }

    public int GetCritChance(int critRating, int dexterity)
    {
        int crit = Mathf.FloorToInt(dexterity * .12f) + Mathf.FloorToInt(critRating * .25f);

        if (crit > 100)
        {
            crit = 100;
        }
        return crit;
    }

    public int GetMoveRating(int moveRating, int dexterity)
    {
        int move = Mathf.FloorToInt(dexterity * .02f) + Mathf.CeilToInt(moveRating * .05f);

        return move;
    }

    public int GetRegen(int regenRating, int spirit)
    {
        int regen = Mathf.CeilToInt(spirit * .15f) + regenRating;

        return regen;
    }

    public int GetDodgeChance(int dodgeRating, int agility)
    {
        int dodge = Mathf.FloorToInt(dodgeRating * .25f) + Mathf.FloorToInt(agility * .2f);

        if (dodge > 100)
        {
            dodge = 100;
        }

        return dodge;
    }

    public int GetBlockChance(int blockRating)
    {
        int block = Mathf.FloorToInt(blockRating * .25f);

        if (block > 100)
        {
            block = 100;
        }

        return block;
    }

    public int GetParryChance(int parryRating, int strength, int dexterity)
    {
        int parry = Mathf.FloorToInt(parryRating * .25f) + Mathf.FloorToInt(strength * .1f) + Mathf.FloorToInt(dexterity * .05f);

        if (parry > 100)
        {
            parry = 100;
        }

        return parry;
    }

    public int GetThreatRating(int threatRating)
    {
        int threat = Mathf.FloorToInt(threatRating * .5f);

        return threat;
    }
}
