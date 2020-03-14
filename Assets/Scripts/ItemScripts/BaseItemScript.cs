using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseItemScript
{
    public string scriptToRun = ""; //to be set in hero/enemy state manager to determine which item workflow should be run

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
    }

    void PotionToHero(BaseHero hero)
    {
        hero.curHP += 20;

        if (hero.curHP > hero.maxHP)
        {
            hero.curHP = hero.maxHP;
        }
        Debug.Log("Healing " + hero._Name + " for 20 HP!");
    }

    void EtherToHero(BaseHero hero)
    {
        hero.curMP += 10;

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
    }

    void PotionToEnemy(BaseEnemy enemy)
    {
        enemy.curHP += 20;

        if (enemy.curHP > enemy.baseHP)
        {
            enemy.curHP = enemy.baseHP;
        }
        Debug.Log("Healing " + enemy._Name + " for 20 HP!");
    }

    void EtherToEnemy(BaseEnemy enemy)
    {
        enemy.curMP += 10;

        if (enemy.curMP > enemy.baseMP)
        {
            enemy.curMP = enemy.baseMP;
        }
        Debug.Log("Recovering " + enemy._Name + "'s MP by 10!");
    }

}
