using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffect
{
    public EnemyBehavior eb;
    public HeroStateMachine hsm;

    public Color elementColor;

    public void ProcessEffect(string effectName, string effectType, int baseVal, GameObject thisObject)
    {

        String target = "";

        //checks if thisObject has HeroStateMachine or EnemyStateMachine attached
        if ((thisObject.GetComponent("HeroStateMachine") as HeroStateMachine) != null) //is hero
        {
            target = "Hero";
        }
        if ((thisObject.GetComponent("EnemyStateMachine") as EnemyStateMachine) != null) //is enemy
        {
            target = "Enemy";
        }
        if (target == "")
        {
            Debug.LogWarning("Something went wrong with status effect - hero and enemy not found");
        }


        //process effects
        if (effectType == "POISON")
        {
            Poison(thisObject, baseVal, target);
        }
    }

    void Poison(GameObject thisObject, int baseVal, string target)
    {
        
        if (target == "Enemy")
        {
        Debug.Log("Processing poison to enemy");
        Debug.Log(thisObject.GetComponent<EnemyStateMachine>().enemy._Name + " HP: " + thisObject.GetComponent<EnemyStateMachine>().enemy.curHP + " / " + thisObject.GetComponent<EnemyStateMachine>().enemy.baseHP);
        Debug.Log(thisObject.GetComponent<EnemyStateMachine>().enemy._Name + " taking " + baseVal + " damage from poison.");
        thisObject.GetComponent<EnemyStateMachine>().enemyBehavior.TakeDamage(baseVal);
        Debug.Log(thisObject.GetComponent<EnemyStateMachine>().enemy._Name + " HP: " + thisObject.GetComponent<EnemyStateMachine>().enemy.curHP + " / " + thisObject.GetComponent<EnemyStateMachine>().enemy.baseHP);
        }

        if (target == "Hero")
        {
        Debug.Log("Processing poison to hero");
        Debug.Log(thisObject.GetComponent<HeroStateMachine>().hero._Name + " HP: " + thisObject.GetComponent<HeroStateMachine>().hero.curHP + " / " + thisObject.GetComponent<HeroStateMachine>().hero.baseHP);
        Debug.Log(thisObject.GetComponent<HeroStateMachine>().name + " taking " + baseVal + " damage from poison.");
        thisObject.GetComponent<HeroStateMachine>().TakeDamage(baseVal);
        Debug.Log(thisObject.GetComponent<HeroStateMachine>().hero._Name + " HP: " + thisObject.GetComponent<HeroStateMachine>().hero.curHP + " / " + thisObject.GetComponent<HeroStateMachine>().hero.baseHP);
        }

        elementColor = new Color(0, 0.75f, 0);
        SetDamage(baseVal);
    }

    void SetDamage(int damage)
    {
        if (hsm != null)
        {
            hsm.effectDamage = damage;
        } else if (eb != null)
        {
            eb.effectDamage = damage;
        }
    }
}
