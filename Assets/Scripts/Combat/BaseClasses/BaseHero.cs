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

    public BaseAttack attack;
    public List<BaseAttack> MagicAttacks = new List<BaseAttack>(); //unit's magic attacks

    public BaseTalent[] level1Talents = new BaseTalent[3];
    public BaseTalent[] level2Talents = new BaseTalent[3];
    public BaseTalent[] level3Talents = new BaseTalent[3];
    public BaseTalent[] level4Talents = new BaseTalent[3];
    public BaseTalent[] level5Talents = new BaseTalent[3];
    public BaseTalent[] level6Talents = new BaseTalent[3];

    public List<BaseAttackLearn> attacksToLearn = new List<BaseAttackLearn>();

    public Equipment[] equipment = new Equipment[System.Enum.GetNames(typeof(EquipmentSlot)).Length];

    [ReadOnly] public int curHP; //current HP
    [HideInInspector] public int preEquipmentHP;

    [ReadOnly] public int curMP; //current MP
    [HideInInspector] public int preEquipmentMP;

    [HideInInspector] public int preEquipmentStrength;
    [HideInInspector] public int preEquipmentStamina;
    [HideInInspector] public int preEquipmentAgility;
    [HideInInspector] public int preEquipmentDexterity;
    [HideInInspector] public int preEquipmentIntelligence;
    [HideInInspector] public int preEquipmentSpirit;
    [HideInInspector] public int preEquipmentATK;
    [HideInInspector] public int preEquipmentMATK;
    [HideInInspector] public int preEquipmentDEF;
    [HideInInspector] public int preEquipmentMDEF;

    [HideInInspector] public int preEquipmentHitRating;
    [HideInInspector] public int preEquipmentCritRating;
    [HideInInspector] public int preEquipmentMoveRating;
    [HideInInspector] public int preEquipmentRegenRating;

    [HideInInspector] public int preEquipmentDodgeRating;
    [HideInInspector] public int preEquipmentBlockRating;
    [HideInInspector] public int preEquipmentParryRating;
    [HideInInspector] public int preEquipmentThreatRating;
    
    [HideInInspector] public int fromEquipmentHP;
    [HideInInspector] public int fromEquipmentMP;

    [HideInInspector] public int fromEquipmentStrength;
    [HideInInspector] public int fromEquipmentStamina;
    [HideInInspector] public int fromEquipmentAgility;
    [HideInInspector] public int fromEquipmentDexterity;
    [HideInInspector] public int fromEquipmentIntelligence;
    [HideInInspector] public int fromEquipmentSpirit;
    [HideInInspector] public int fromEquipmentATK;
    [HideInInspector] public int fromEquipmentMATK;
    [HideInInspector] public int fromEquipmentDEF;
    [HideInInspector] public int fromEquipmentMDEF;

    [HideInInspector] public int fromEquipmentHitRating;
    [HideInInspector] public int fromEquipmentCritRating;
    [HideInInspector] public int fromEquipmentMoveRating;
    [HideInInspector] public int fromEquipmentRegenRating;

    [HideInInspector] public int fromEquipmentDodgeRating;
    [HideInInspector] public int fromEquipmentBlockRating;
    [HideInInspector] public int fromEquipmentParryRating;
    [HideInInspector] public int fromEquipmentThreatRating;

    [HideInInspector] public int postEquipmentHP;
    [HideInInspector] public int postEquipmentMP;

    [HideInInspector] public int postEquipmentStrength;
    [HideInInspector] public int postEquipmentStamina;
    [HideInInspector] public int postEquipmentAgility;
    [HideInInspector] public int postEquipmentDexterity;
    [HideInInspector] public int postEquipmentIntelligence;
    [HideInInspector] public int postEquipmentSpirit;
    [HideInInspector] public int postEquipmentATK;
    [HideInInspector] public int postEquipmentMATK;
    [HideInInspector] public int postEquipmentDEF;
    [HideInInspector] public int postEquipmentMDEF;

    [HideInInspector] public int postEquipmentHitRating;
    [HideInInspector] public int postEquipmentCritRating;
    [HideInInspector] public int postEquipmentMoveRating;
    [HideInInspector] public int postEquipmentRegenRating;

    [HideInInspector] public int postEquipmentDodgeRating;
    [HideInInspector] public int postEquipmentBlockRating;
    [HideInInspector] public int postEquipmentParryRating;
    [HideInInspector] public int postEquipmentThreatRating;

    [ReadOnly] public int finalMaxHP;
    [ReadOnly] public int finalMaxMP;

    [ReadOnly] public int finalStrength;
    [ReadOnly] public int finalStamina;
    [ReadOnly] public int finalAgility;
    [ReadOnly] public int finalDexterity;
    [ReadOnly] public int finalIntelligence;
    [ReadOnly] public int finalSpirit;
    [ReadOnly] public int finalATK;
    [ReadOnly] public int finalMATK;
    [ReadOnly] public int finalDEF;
    [ReadOnly] public int finalMDEF;

    [ReadOnly] public int finalHitRating;
    [ReadOnly] public int finalCritRating;
    [ReadOnly] public int finalMoveRating;
    [ReadOnly] public int finalRegenRating;

    [ReadOnly] public int finalDodgeRating;
    [ReadOnly] public int finalBlockRating;
    [ReadOnly] public int finalParryRating;
    [ReadOnly] public int finalThreatRating;

    [ReadOnly] public int fireEff;
    [ReadOnly] public int fireDef;
    [ReadOnly] public int frostEff;
    [ReadOnly] public int frostDef;
    [ReadOnly] public int lightningEff;
    [ReadOnly] public int lightningDef;
    [ReadOnly] public int waterEff;
    [ReadOnly] public int waterDef;
    [ReadOnly] public int natureEff;
    [ReadOnly] public int natureDef;
    [ReadOnly] public int earthEff;
    [ReadOnly] public int earthDef;
    [ReadOnly] public int holyEff;
    [ReadOnly] public int holyDef;
    [ReadOnly] public int darkEff;
    [ReadOnly] public int darkDef;

    [ReadOnly] public int blindEff;
    [ReadOnly] public int blindDef;
    [ReadOnly] public int silenceEff;
    [ReadOnly] public int silenceDef;
    [ReadOnly] public int sleepEff;
    [ReadOnly] public int sleepDef;
    [ReadOnly] public int confuseEff;
    [ReadOnly] public int confuseDef;
    [ReadOnly] public int poisonEff;
    [ReadOnly] public int poisonDef;
    [ReadOnly] public int petrifyEff;
    [ReadOnly] public int petrifyDef;
    [ReadOnly] public int slowEff;
    [ReadOnly] public int slowDef;
    [ReadOnly] public int zombieEff;
    [ReadOnly] public int zombieDef;

    [HideInInspector] public int levelBeforeExp;
    [HideInInspector] public int expBeforeAddingExp;

    /// <summary>
    /// Sets all stat variables to base stats for later manipulation
    /// </summary>
    public void InitializeStats()
    {
        preEquipmentStrength = baseSTR;
        preEquipmentStamina = baseSTA;
        preEquipmentAgility = baseAGI;
        preEquipmentDexterity = baseDEX;
        preEquipmentIntelligence = baseINT;
        preEquipmentSpirit = baseSPI;

        preEquipmentATK = baseATK;
        preEquipmentMATK = baseMATK;
        preEquipmentDEF = baseDEF;
        preEquipmentMDEF = baseMDEF;
        preEquipmentHitRating = baseHit;
        preEquipmentCritRating = baseCrit;
        preEquipmentMoveRating = baseMove;
        preEquipmentRegenRating = baseRegen;

        preEquipmentDodgeRating = baseDodge;
        preEquipmentBlockRating = baseBlock;
        preEquipmentParryRating = baseParry;
        preEquipmentThreatRating = baseThreat;

        UpdateStats();

        curHP = finalMaxHP;
        curMP = finalMaxMP;
    }

    /// <summary>
    /// Sets given equip to appropriate slot on hero's equipment slot, and removes it from the inventory
    /// </summary>
    /// <param name="newEquip">Equip to be set to equipment slot</param>
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

    /// <summary>
    /// Removes equipment from given equipment slot, and adds it back to the inventory
    /// </summary>
    /// <param name="slotIndex">Index of the equipment slot to be removed</param>
    public void Unequip (int slotIndex)
    {
        if (equipment[slotIndex] != null)
        {
            Equipment oldEquipment = equipment[slotIndex];
            Inventory.instance.Add(oldEquipment);

            equipment[slotIndex] = null;
        }
    }

    /// <summary>
    /// Removes all equipment from hero and adds them back to the inventory
    /// </summary>
    public void UnequipAll()
    {
        for (int i = 0; i < equipment.Length; i++)
        {
            Unequip(i);
        }
    }

    /// <summary>
    /// Increases hero level and calls methods for stats to be increased
    /// </summary>
    public void LevelUp()
    {
        currentLevel++;
        Debug.Log(name + " has leveled up from " + levelBeforeExp + " to " + currentLevel);
        ProcessStatLevelUps();
    }

    /// <summary>
    /// Increases base stat values using their modifiers and adds any potential new attacks
    /// </summary>
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

        curHP = baseHP; //if full heal should occur on levelup, using for debugging purposes for now
        curMP = baseMP; //if MP should be restored on levelup, using for debugging purposes for now

        UpdateStats();

        learnNewAttacks();
    }

    /// <summary>
    /// Updates secondary stats and updates other variables for stats (from equipment, talents, etc)
    /// </summary>
    public void UpdateStats()
    {
        /*baseATK = GetATK(baseSTR, baseATK);
        baseMATK = GetMATK(baseINT, baseMATK);
        baseDEF = GetDEF(baseSTA, baseDEF);
        baseMDEF = GetMDEF(baseSTA, baseMDEF);

        Debug.Log("old Max HP for " + name + ": " + baseHP);
        baseHP = GetMaxHP(baseSTA, baseHP);
        Debug.Log("new Max HP for " + name + ": " + baseHP);
        baseMP = GetMaxMP(baseINT, baseMP);*/
        
        GetCurrentStatsFromEquipment();

        UpdateStatsFromTalents();
    }

    /// <summary>
    /// Sets hero stats after taking equipped items into account
    /// </summary>
    public void GetCurrentStatsFromEquipment()
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

        fromEquipmentStrength = tempStrength;
        fromEquipmentStamina = tempStamina;
        fromEquipmentAgility = tempAgility;
        fromEquipmentDexterity = tempDexterity;
        fromEquipmentIntelligence = tempIntelligence;
        fromEquipmentSpirit = tempSpirit;

        fromEquipmentHP = GetMaxHP(fromEquipmentStamina, baseHP);
        fromEquipmentMP = GetMaxMP(fromEquipmentIntelligence, baseMP);

        fromEquipmentATK = tempATK;
        fromEquipmentMATK = tempMATK;
        fromEquipmentDEF = tempDEF;
        fromEquipmentMDEF = tempMDEF;

        fromEquipmentHitRating = tempHit;
        fromEquipmentCritRating = tempCrit;
        fromEquipmentMoveRating = tempMove;
        fromEquipmentRegenRating = tempRegen;

        fromEquipmentDodgeRating = tempDodge;
        fromEquipmentBlockRating = tempBlock;
        fromEquipmentParryRating = tempParry;
        fromEquipmentThreatRating = tempThreat;
        
        UpdatePostEquipmentStats();
    }

    /// <summary>
    /// Sets newly updated stats by adding base and equipment stats
    /// </summary>
    void UpdatePostEquipmentStats()
    {
        postEquipmentStrength = baseSTR + fromEquipmentStrength;
        postEquipmentStamina = baseSTA + fromEquipmentStamina;
        postEquipmentAgility = baseAGI + fromEquipmentAgility;
        postEquipmentDexterity = baseDEX + fromEquipmentDexterity;
        postEquipmentIntelligence = baseINT + fromEquipmentIntelligence;
        postEquipmentSpirit = baseSPI + fromEquipmentSpirit;

        postEquipmentATK = baseATK + fromEquipmentATK;
        postEquipmentMATK = baseMATK + fromEquipmentMATK;
        postEquipmentDEF = baseDEF + fromEquipmentDEF;
        postEquipmentMDEF = baseMDEF + fromEquipmentMDEF;

        postEquipmentHitRating = baseHit + fromEquipmentHitRating;
        postEquipmentCritRating = baseCrit + fromEquipmentCritRating;
        postEquipmentMoveRating = fromEquipmentMoveRating;
        postEquipmentRegenRating = baseRegen + fromEquipmentRegenRating;

        postEquipmentDodgeRating = baseDodge + fromEquipmentDodgeRating;
        postEquipmentBlockRating = baseBlock + fromEquipmentBlockRating;
        postEquipmentParryRating = baseParry + fromEquipmentParryRating;
        postEquipmentThreatRating = baseThreat + fromEquipmentThreatRating;
        
        postEquipmentHP = GetMaxHP(postEquipmentStamina, fromEquipmentHP);
        postEquipmentMP = GetMaxMP(postEquipmentIntelligence, fromEquipmentMP);
    }

    /// <summary>
    /// Adds in post-equipment stats with any potential talents set as active
    /// </summary>
    public void UpdateStatsFromTalents()
    {
        finalStrength = postEquipmentStrength;
        finalStamina = postEquipmentStamina;
        finalAgility = postEquipmentAgility;
        finalDexterity = postEquipmentDexterity;
        finalIntelligence = postEquipmentIntelligence;
        finalSpirit = postEquipmentSpirit;

        finalATK = postEquipmentATK;
        finalMATK = postEquipmentMATK;
        finalDEF = postEquipmentDEF;
        finalMDEF = postEquipmentMDEF;

        finalHitRating = postEquipmentHitRating;
        finalCritRating = postEquipmentCritRating;
        finalMoveRating = postEquipmentMoveRating;
        finalRegenRating = postEquipmentRegenRating;

        finalDodgeRating = postEquipmentDodgeRating;
        finalBlockRating = postEquipmentBlockRating;
        finalParryRating = postEquipmentParryRating;
        finalThreatRating = postEquipmentThreatRating;
        
        finalMaxHP = postEquipmentHP;
        finalMaxMP = postEquipmentMP;

        TalentEffects effect = new TalentEffects();
        
        foreach (BaseTalent talent in level1Talents)
        {
            if (talent.isActive)
            {
                effect.AddEffect(talent.effect, this);
            }
        }

        foreach (BaseTalent talent in level2Talents)
        {
            if (talent.isActive)
            {
                effect.AddEffect(talent.effect, this);
            }
        }

        foreach (BaseTalent talent in level3Talents)
        {
            if (talent.isActive)
            {
                effect.AddEffect(talent.effect, this);
            }
        }

        foreach (BaseTalent talent in level4Talents)
        {
            if (talent.isActive)
            {
                effect.AddEffect(talent.effect, this);
            }
        }

        foreach (BaseTalent talent in level5Talents)
        {
            if (talent.isActive)
            {
                effect.AddEffect(talent.effect, this);
            }
        }

        foreach (BaseTalent talent in level6Talents)
        {
            if (talent.isActive)
            {
                effect.AddEffect(talent.effect, this);
            }
        }
        
        UpdateFinalStats();
    }

    /// <summary>
    /// Sets final stats and updates UI if needed
    /// </summary>
    void UpdateFinalStats()
    {
        finalMaxHP = GetMaxHP(finalStamina, finalMaxHP);
        finalMaxMP = GetMaxMP(finalIntelligence, finalMaxMP);

        if (curHP > finalMaxHP)
        {
            curHP = finalMaxHP;
        } //keeps current HP from being higher than max

        if (curMP > finalMaxMP)
        {
            curMP = finalMaxMP;
        } //keeps current MP from being higher than max

        finalATK = GetATK(finalStrength, finalATK);
        finalMATK = GetMATK(finalIntelligence, finalMATK);
        finalDEF = GetDEF(finalStamina, finalDEF);
        finalMDEF = GetMDEF(finalStamina, finalMDEF);

        finalHitRating = GetHitChance(finalHitRating, finalAgility);
        finalCritRating = GetCritChance(finalCritRating, finalDexterity);
        finalMoveRating = GetMoveRating(finalMoveRating, finalDexterity) + baseMove;
        finalRegenRating = GetRegen(finalRegenRating, finalSpirit);

        finalDodgeRating = GetDodgeChance(finalDodgeRating, finalAgility);
        finalBlockRating = GetBlockChance(finalBlockRating);
        finalParryRating = GetParryChance(finalParryRating, finalStrength, finalDexterity);
        finalThreatRating = GetThreatRating(finalThreatRating);

        if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().heroToCheck != null)
        {
            UpdatePanels();
        }
    }

    /// <summary>
    /// Updates menu interface with newly updated stats
    /// </summary>
    void UpdatePanels()
    {
        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().DrawTalentsMenuHeroPanel();
        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().DrawEquipMenuStats();
    }

    /// <summary>
    /// Adds new attacks for hero if they have reached appropriate level
    /// </summary>
    void learnNewAttacks()
    {
        foreach (BaseAttackLearn attackToLearn in attacksToLearn)
        {
            if (currentLevel == attackToLearn.level)
            {
                Debug.Log(name + " learned attack: " + attackToLearn.attack.name);
            }

            MagicAttacks.Add(attackToLearn.attack);
        }
    }

    //----------------------------------------------------------------------------
    //Formulas for setting secondary stats
    //----------------------------------------------------------------------------

    public int GetATK(int strength, int attack)
    {
        int ATK = Mathf.RoundToInt(attack + (strength * .5f));

        return ATK;
    }

    public int GetMATK(int intelligence, int magicAttack)
    {
        int MATK = Mathf.RoundToInt(magicAttack + (intelligence * .5f));

        return MATK;
    }

    public int GetDEF(int stamina, int defense)
    {
        int DEF = Mathf.RoundToInt(defense + (stamina * .6f));

        return DEF;
    }

    public int GetMDEF(int stamina, int magicDefense)
    {
        int MDEF = Mathf.RoundToInt(magicDefense + (stamina * .5f));

        return MDEF;
    }

    public int GetMaxHP(int stamina, int hp)
    {
        int HP = Mathf.RoundToInt(hp + (stamina * .75f));

        return HP;
    }

    public int GetMaxMP(int intelligence, int mp)
    {
        int MP = Mathf.RoundToInt(mp + (intelligence * .75f));

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
