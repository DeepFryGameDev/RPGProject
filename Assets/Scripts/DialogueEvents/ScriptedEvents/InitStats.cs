using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitStats : BaseScriptedEvent
{
    void InitiateStats()
    {
        Debug.Log("Initializing stats");
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            hero.InitializeStats();
        }
    }
}
