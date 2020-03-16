﻿using System.Collections;
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

        SetValue();
    }

    void PotionToHero(BaseHero hero)
    {
        adjValue = 20;

        hero.curHP += adjValue;

        if (hero.curHP > hero.maxHP)
        {
            hero.curHP = hero.maxHP;
        }
        Debug.Log("Healing " + hero._Name + " for 20 HP!");
    }

    void EtherToHero(BaseHero hero)
    {
        adjValue = 10;

        hero.curMP += adjValue;

        if (hero.curMP > hero.maxMP)
        {
            hero.curMP = hero.maxMP;
        }
        Debug.Log("Recovering " + hero._Name + "'s MP by 10!");
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
        Debug.Log("Healing " + enemy._Name + " for 20 HP!");
    }

    void EtherToEnemy(BaseEnemy enemy)
    {
        adjValue = 10;
        enemy.curMP += adjValue;

        if (enemy.curMP > enemy.baseMP)
        {
            enemy.curMP = enemy.baseMP;
        }
        Debug.Log("Recovering " + enemy._Name + "'s MP by 10!");
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
