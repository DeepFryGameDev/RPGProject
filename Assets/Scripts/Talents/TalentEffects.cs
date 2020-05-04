using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentEffects
{
    BaseHero hero = new BaseHero();

    public void AddEffect(string effect)
    {
        hero = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().heroToCheck;

        Debug.Log("Adding effect: " + effect);

        if (effect == "Test1")
        {
            hero.currentStamina = hero.currentStamina * 2;
        }
        if (effect == "Test2")
        {
            hero.currentAgility = hero.currentAgility * 2;
        }
        if (effect == "Test3")
        {
            hero.currentStrength = hero.currentStrength * 2;
        }
    }

    public void RemoveEffect(string effect)
    {
        hero = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().heroToCheck;
        Debug.Log("Removing effect: " + effect);

        if (effect == "Test1")
        {
            hero.currentStamina = hero.currentStamina / 2;
        }
        if (effect == "Test2")
        {
            hero.currentAgility = hero.currentAgility / 2;
        }
        if (effect == "Test3")
        {
            hero.currentStrength = hero.currentStrength / 2;
        }
    }
}
