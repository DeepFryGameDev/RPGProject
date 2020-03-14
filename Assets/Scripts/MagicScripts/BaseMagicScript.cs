using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseMagicScript //for processing magic - need to add status effects in here eventually
{
    public BaseAttack spell;
    public BaseHero heroPerformingAction;
    public BaseHero heroReceivingAction;
    public BaseEnemy enemyPerformingAction;
    public BaseEnemy enemyReceivingAction;

    public HeroStateMachine hsm;
    public EnemyBehavior eb;

    //---PROCESSING MAGIC FROM HERO
    public void ProcessMagicHeroToHero()
    {
        if (spell.name == "Cure 1")
        {
            Cure1HeroToHero();
        }
        if (spell.name == "Bio 1")
        {
            Bio1HeroToHero();
        }
        if (spell.name == "Fire 1")
        {
            Fire1HeroToHero();
        }

        SpendMP();
    }

    public void ProcessMagicHeroToEnemy()
    {
        if (spell.name == "Cure 1")
        {
            Cure1HeroToEnemy();
        }
        if (spell.name == "Bio 1")
        {
            Bio1HeroToEnemy();
        }
        if (spell.name == "Fire 1")
        {
            Fire1HeroToEnemy();
        }

        SpendMP();
    }

    //---MAGIC HERO TO HERO
    #region HEROTOHERO
    void Cure1HeroToHero()
    {
        heroReceivingAction.curHP += spell.damage;

        if (heroReceivingAction.curHP > heroReceivingAction.baseHP)
        {
            heroReceivingAction.curHP = heroReceivingAction.baseHP;
        }
        Debug.Log("Casting Cure 1 on " + heroReceivingAction._Name + "!");
    }

    void Bio1HeroToHero()
    {
        heroReceivingAction.curHP -= spell.damage;

        if (heroReceivingAction.curHP > heroReceivingAction.baseHP)
        {
            heroReceivingAction.curHP = heroReceivingAction.baseHP;
        }

        Debug.Log("Casting Bio 1 on " + heroReceivingAction._Name + "!");
    }

    void Fire1HeroToHero()
    {
        heroReceivingAction.curHP -= spell.damage;

        if (heroReceivingAction.curHP > heroReceivingAction.baseHP)
        {
            heroReceivingAction.curHP = heroReceivingAction.baseHP;
        }
        Debug.Log("Casting Fire 1 on " + heroReceivingAction._Name + "!");
    }
    #endregion

    //---MAGIC HERO TO ENEMY
    #region HEROTOENEMY
    void Cure1HeroToEnemy()
    {
        enemyReceivingAction.curHP += spell.damage;

        if (enemyReceivingAction.curHP > enemyReceivingAction.baseHP)
        {
            enemyReceivingAction.curHP = enemyReceivingAction.baseHP;
        }
        Debug.Log("Casting Cure 1 on " + enemyReceivingAction._Name + "!");
    }

    void Bio1HeroToEnemy()
    {
        enemyReceivingAction.curHP -= spell.damage;

        if (enemyReceivingAction.curHP > enemyReceivingAction.baseHP)
        {
            enemyReceivingAction.curHP = enemyReceivingAction.baseHP;
        }

        Debug.Log("Casting Bio 1 on " + enemyReceivingAction._Name + "!");
    }

    void Fire1HeroToEnemy()
    {

        enemyReceivingAction.curHP -= spell.damage;

        if (enemyReceivingAction.curHP > enemyReceivingAction.baseHP)
        {
            enemyReceivingAction.curHP = enemyReceivingAction.baseHP;
        }
        Debug.Log("Casting Fire 1 on " + enemyReceivingAction._Name + "!");
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

        SpendMP();
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

        SpendMP();
    }

    //---MAGIC ENEMY TO HERO
    #region ENEMYTOHERO
    void Cure1EnemyToHero()
    {
        heroReceivingAction.curHP += spell.damage;

        if (heroReceivingAction.curHP > heroReceivingAction.baseHP)
        {
            heroReceivingAction.curHP = heroReceivingAction.baseHP;
        }

        Debug.Log("Casting Cure 1 on " + heroReceivingAction._Name + "!");
    }

    void Bio1EnemyToHero()
    {
        heroReceivingAction.curHP -= spell.damage; //need to add damage calculation

        Debug.Log("Casting Bio 1 on " + heroReceivingAction._Name + "!");
    }

    void Fire1EnemyToHero()
    {
        heroReceivingAction.curHP -= spell.damage;

        Debug.Log("Casting Fire 1 on " + heroReceivingAction._Name + "!");
    }
    #endregion

    //---MAGIC ENEMY TO ENEMY
    #region ENEMYTOENEMY
    void Cure1EnemyToEnemy()
    {
        enemyReceivingAction.curHP += spell.damage; //need to add healing calculation

        if (enemyReceivingAction.curHP > enemyReceivingAction.baseHP)
        {
            enemyReceivingAction.curHP = enemyReceivingAction.baseHP;
        }

        Debug.Log("Casting Cure 1 on " + enemyReceivingAction._Name + "!");
    }

    void Bio1EnemyToEnemy()
    {
        enemyReceivingAction.curHP -= spell.damage;

        Debug.Log("Casting Bio 1 on " + enemyReceivingAction._Name + "!");
    }

    void Fire1EnemyToEnemy()
    {

        enemyReceivingAction.curHP -= spell.damage;
        
        Debug.Log("Casting Fire 1 on " + enemyReceivingAction._Name + "!");
    }
    #endregion

    void SpendMP()
    {
        if (heroPerformingAction != null)
        {
            heroPerformingAction.curMP -= spell.MPCost;
        } else if (enemyPerformingAction != null)
        {
            enemyPerformingAction.curMP -= spell.MPCost;
        }
    }

    void SetDamage()
    {
        if (hsm != null) //hero casting spell
        {

        } else if (eb != null) //enemy casting spell
        {

        }

    }
}
