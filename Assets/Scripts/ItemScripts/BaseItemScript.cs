using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseItemScript
{
    public string scriptToRun = ""; //to be set in hero/enemy state manager to determine which item workflow should be run

    public EnemyBehavior eb;
    public HeroStateMachine hsm;
    int adjValue;

    public void ProcessItemToHero(BaseHero hero) //used by hero/enemy state manager to process the item workflow
    {
        if (scriptToRun == "Potion")
        {
            PotionToHero(hero);
        }

        if (scriptToRun == "Ether")
        {
            EtherToHero(hero);
        }

        if (scriptToRun == "High Potion")
        {
            HighPotionToHero(hero);
        }

        SetValue();
    }

    void PotionToHero(BaseHero hero)
    {
        adjValue = 20;

        hero.curHP += adjValue;

        if (hero.curHP > hero.finalMaxHP)
        {
            hero.curHP = hero.finalMaxHP;
        }
        Debug.Log("Healing " + hero.name + " for 20 HP!");
    }

    void HighPotionToHero(BaseHero hero)
    {
        adjValue = 50;

        hero.curHP += adjValue;

        if (hero.curHP > hero.finalMaxHP)
        {
            hero.curHP = hero.finalMaxHP;
        }
        Debug.Log("Healing " + hero.name + " for 50 HP!");
    }

    void EtherToHero(BaseHero hero)
    {
        adjValue = 10;

        hero.curMP += adjValue;

        if (hero.curMP > hero.finalMaxMP)
        {
            hero.curMP = hero.finalMaxMP;
        }
        Debug.Log("Recovering " + hero.name + "'s MP by 10!");
    }

    public void ProcessItemToEnemy(BaseEnemy enemy) //used by hero/enemy state manager to process the item workflow
    {
        if (scriptToRun == "Potion")
        {
            PotionToEnemy(enemy);
        }

        if (scriptToRun == "Ether")
        {
            EtherToEnemy(enemy);
        }

        if (scriptToRun == "High Potion")
        {
            HighPotionToEnemy(enemy);
        }

        SetValue();
    }

    void PotionToEnemy(BaseEnemy enemy)
    {
        adjValue = 20;
        enemy.curHP += adjValue;

        if (enemy.curHP > enemy.baseHP)
        {
            enemy.curHP = enemy.baseHP;
        }
        Debug.Log("Healing " + enemy.name + " for 20 HP!");
    }

    void HighPotionToEnemy(BaseEnemy enemy)
    {
        adjValue = 50;
        enemy.curHP += adjValue;

        if (enemy.curHP > enemy.baseHP)
        {
            enemy.curHP = enemy.baseHP;
        }
        Debug.Log("Healing " + enemy.name + " for 50 HP!");
    }

    void EtherToEnemy(BaseEnemy enemy)
    {
        adjValue = 10;
        enemy.curMP += adjValue;

        if (enemy.curMP > enemy.baseMP)
        {
            enemy.curMP = enemy.baseMP;
        }
        Debug.Log("Recovering " + enemy.name + "'s MP by 10!");
    }

    void SetValue()
    {
        if (eb != null)
        {
            eb.itemDamage = adjValue;
        } else if (hsm != null)
        {
            hsm.itemDamage = adjValue;
        }
    }

}
