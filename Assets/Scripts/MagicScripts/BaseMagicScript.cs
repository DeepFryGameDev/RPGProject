using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseMagicScript 
{

    //Houses all magic processing and effects

    public BaseAttack spell;
    public BaseHero heroPerformingAction;
    public BaseHero heroReceivingAction;
    public BaseEnemy enemyPerformingAction;
    public BaseEnemy enemyReceivingAction;

    public HeroStateMachine hsm;
    public EnemyBehavior eb;

    int adjDamage;
    int baseDamage;

    //---PROCESSING MAGIC FROM HERO
    public void ProcessMagicHeroToHero(bool checkAddedEffect)
    {
        Debug.Log("checkAddedEffect: " + checkAddedEffect);

        SetBaseDamage();

        if (spell.name == "Cure 1")
        {
            Cure1HeroToHero(checkAddedEffect);
        }
        if (spell.name == "Bio 1")
        {
            Bio1HeroToHero(checkAddedEffect);
        }
        if (spell.name == "Fire 1")
        {
            Fire1HeroToHero(checkAddedEffect);
        }

        SetDamage();

        HPBaseline(heroReceivingAction, null);
    }

    public void ProcessMagicHeroToEnemy(bool checkAddedEffect)
    {
        Debug.Log("checkAddedEffect: " + checkAddedEffect);

        SetBaseDamage();

        if (spell.name == "Cure 1")
        {
            Cure1HeroToEnemy(checkAddedEffect);
        }
        if (spell.name == "Bio 1")
        {
            Bio1HeroToEnemy(checkAddedEffect);
        }
        if (spell.name == "Fire 1")
        {
            Fire1HeroToEnemy(checkAddedEffect);
        }

        SetDamage();

        HPBaseline(null, enemyReceivingAction);
    }

    //---MAGIC HERO TO HERO
    #region HEROTOHERO
    void Cure1HeroToHero(bool checkAddedEffect)
    {
        heroReceivingAction.curHP += baseDamage;

        adjDamage = baseDamage;

        Debug.Log("Casting Cure 1 on " + heroReceivingAction.name + "!");
    }

    void Bio1HeroToHero(bool checkAddedEffect)
    {
        heroReceivingAction.curHP -= baseDamage;

        adjDamage = baseDamage;

        Debug.Log("Casting Bio 1 on " + heroReceivingAction.name + "!");
    }

    void Fire1HeroToHero(bool checkAddedEffect)
    {
        if (checkAddedEffect)
        {
            int aeDamage = baseDamage * 2;
            heroReceivingAction.curHP -= aeDamage;

            adjDamage = aeDamage;
        } else
        {
            heroReceivingAction.curHP -= baseDamage;

            adjDamage = baseDamage;
        }

        Debug.Log("Casting Fire 1 on " + heroReceivingAction.name + "!");
    }
    #endregion

    //---MAGIC HERO TO ENEMY
    #region HEROTOENEMY
    void Cure1HeroToEnemy(bool checkAddedEffect)
    {
        enemyReceivingAction.curHP += spell.damage;

        adjDamage = spell.damage;

        Debug.Log("Casting Cure 1 on " + enemyReceivingAction.name + "!");
    }

    void Bio1HeroToEnemy(bool checkAddedEffect)
    {
        enemyReceivingAction.curHP -= spell.damage;

        adjDamage = spell.damage;

        Debug.Log("Casting Bio 1 on " + enemyReceivingAction.name + "!");
    }

    void Fire1HeroToEnemy(bool checkAddedEffect)
    {
        enemyReceivingAction.curHP -= spell.damage;

        adjDamage = spell.damage;

        Debug.Log("Casting Fire 1 on " + enemyReceivingAction.name + "!");
    }
    #endregion
    
    //---PROCESSING MAGIC FROM ENEMY

    public void ProcessMagicEnemyToEnemy()
    {
        if (spell.name == "Cure 1")
        {
            Cure1EnemyToEnemy();
        }
        if (spell.name == "Bio 1")
        {
            Bio1EnemyToEnemy();
        }
        if (spell.name == "Fire 1")
        {
            Fire1EnemyToEnemy();
        }

        SetDamage();

        HPBaseline(null, enemyReceivingAction);
    }

    public void ProcessMagicEnemyToHero()
    {
        if (spell.name == "Cure 1")
        {
            Cure1EnemyToHero();
        }
        if (spell.name == "Bio 1")
        {
            Bio1EnemyToHero();
        }
        if (spell.name == "Fire 1")
        {
            Fire1EnemyToHero();
        }

        SetDamage();

        HPBaseline(heroReceivingAction, null);
    }

    //---MAGIC ENEMY TO HERO
    #region ENEMYTOHERO
    void Cure1EnemyToHero()
    {
        heroReceivingAction.curHP += spell.damage;

        adjDamage = spell.damage;

        Debug.Log("Casting Cure 1 on " + heroReceivingAction.name + "!");
    }

    void Bio1EnemyToHero()
    {
        heroReceivingAction.curHP -= spell.damage; //need to add damage calculation

        adjDamage = spell.damage;

        Debug.Log("Casting Bio 1 on " + heroReceivingAction.name + "!");
    }

    void Fire1EnemyToHero()
    {
        heroReceivingAction.curHP -= spell.damage;

        adjDamage = spell.damage;

        Debug.Log("Casting Fire 1 on " + heroReceivingAction.name + "!");
    }
    #endregion

    //---MAGIC ENEMY TO ENEMY
    #region ENEMYTOENEMY
    void Cure1EnemyToEnemy()
    {
        enemyReceivingAction.curHP += spell.damage; //need to add healing calculation

        adjDamage = spell.damage;

        Debug.Log("Casting Cure 1 on " + enemyReceivingAction.name + "!");
    }

    void Bio1EnemyToEnemy()
    {
        enemyReceivingAction.curHP -= spell.damage;

        adjDamage = spell.damage;

        Debug.Log("Casting Bio 1 on " + enemyReceivingAction.name + "!");
    }

    void Fire1EnemyToEnemy()
    {
        enemyReceivingAction.curHP -= spell.damage;

        adjDamage = spell.damage;

        Debug.Log("Casting Fire 1 on " + enemyReceivingAction.name + "!");
    }
    #endregion

    void SetDamage()
    {
        if (hsm != null) //hero casting spell
        {
            hsm.magicDamage = adjDamage;
        }
        else if (eb != null) //enemy casting spell
        {
            eb.magicDamage = adjDamage;
        }
    }

    /// <summary>
    /// Formula for setting base damage of magic attack
    /// </summary>
    void SetBaseDamage()
    {
        if (hsm != null)
        {
            baseDamage = Mathf.CeilToInt(heroPerformingAction.finalIntelligence * .5f) + spell.damage;
        }
        else if (eb != null)
        {
            baseDamage = Mathf.CeilToInt(enemyPerformingAction.baseIntelligence * .5f) + spell.damage;
        }
    }

    void HPBaseline(BaseHero hero, BaseEnemy enemy)
    {
        if (hero != null)
        {
            if (hero.curHP > hero.finalMaxHP)
            {
                hero.curHP = hero.finalMaxHP;
            }
        }
        else if (enemy != null)
        {
            if (enemy.curHP > enemy.baseHP)
            {
                enemy.curHP = enemy.baseHP;
            }
        }
    }

    void MPBaseline(BaseHero hero, BaseEnemy enemy)
    {
        if (hero != null)
        {
            if (hero.curMP > hero.finalMaxMP)
            {
                hero.curMP = hero.finalMaxMP;
            }
        }
        else if (enemy != null)
        {
            if (enemy.curMP > enemy.baseMP)
            {
                enemy.curMP = enemy.baseMP;
            }
        }
    }
}
