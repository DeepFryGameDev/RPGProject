using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpThings : BaseScriptedEvent
{
    void AutostartStuff()
    {
        InitiateStats();
        SetRandomGridSpawnPoints();
    }

    void InitiateStats()
    {
        Debug.Log("Initializing stats");
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            hero.InitializeStats();
        }
        foreach (BaseHero hero in GameManager.instance.inactiveHeroes)
        {
            hero.InitializeStats();
        }
    }

    void SetRandomGridSpawnPoints()
    {
        List<string> savedSpawnPoints = new List<string>();
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            while (hero.spawnPoint == "")
            {
                int randomColumn = Random.Range(1, 5);
                int randomRow = Random.Range(1, 5);

                string spawnPoint = randomColumn.ToString() + randomRow.ToString();

                if (!savedSpawnPoints.Contains(spawnPoint))
                {
                    savedSpawnPoints.Add(spawnPoint);
                    hero.spawnPoint = spawnPoint;
                    Debug.Log("Setting random spawn point for " + hero._Name + " - " + spawnPoint);
                }
            }
        }

        foreach (BaseHero hero in GameManager.instance.inactiveHeroes)
        {
            while (hero.spawnPoint == "")
            {
                int randomColumn = Random.Range(1, 5);
                int randomRow = Random.Range(1, 5);

                string spawnPoint = randomColumn.ToString() + randomRow.ToString();

                if (!savedSpawnPoints.Contains(spawnPoint))
                {
                    savedSpawnPoints.Add(spawnPoint);
                    hero.spawnPoint = spawnPoint;
                    Debug.Log("Setting random spawn point for " + hero._Name + " - " + spawnPoint);
                }
            }
        }
    }
}
