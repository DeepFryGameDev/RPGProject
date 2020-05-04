using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpThings : BaseScriptedEvent
{
    void AutostartStuff()
    {
        InitiateStats();
        SetRandomGridSpawnPoints();

        AssignQuestIDs();
        AssignHeroIDs();
        AssignEnemyIDs();

        AddHeroes();
    }

    void InitiateStats()
    {
        //Debug.Log("Initializing stats");
        foreach (BaseHero hero in GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().heroes)
        {
            hero.InitializeStats();
        }
    }

    void SetRandomGridSpawnPoints()
    {
        List<string> savedSpawnPoints = new List<string>();

        foreach (BaseHero hero in GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().heroes)
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
                    Debug.Log("Setting random spawn point for " + hero.name + " - " + spawnPoint);
                }
            }
        }
    }

    void AssignQuestIDs()
    {
        foreach (BaseQuest quest in GameObject.Find("GameManager/QuestDB").GetComponent<QuestDB>().quests)
        {
            Debug.Log("Assigning ID " + GameObject.Find("GameManager/QuestDB").GetComponent<QuestDB>().quests.IndexOf(quest) + " to quest " + quest.name);
            quest.ID = GameObject.Find("GameManager/QuestDB").GetComponent<QuestDB>().quests.IndexOf(quest);
        }
    }

    void AddHeroes()
    {
        GameManager.instance.activeHeroes.Add(GetHero(0));
        GameManager.instance.activeHeroes.Add(GetHero(1));

        GameManager.instance.inactiveHeroes.Add(GetHero(2));
    }

    private BaseHero GetHero(int ID)
    {
        foreach (BaseHero hero in GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().heroes)
        {
            if (hero.ID == ID)
            {
                return hero;
            }
        }
        return null;
    }

    public void AssignHeroIDs()
    {
        foreach (BaseHero hero in GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().heroes)
        {
            hero.ID = GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().heroes.IndexOf(hero);
        }
    }

    public void AssignEnemyIDs()
    {
        int ID;
        foreach (BaseEnemyDBEntry enemyEntry in GameObject.Find("GameManager/EnemyDB").GetComponent<EnemyDB>().enemies)
        {
            ID = GameObject.Find("GameManager/EnemyDB").GetComponent<EnemyDB>().enemies.IndexOf(enemyEntry);
            enemyEntry.enemy.ID = ID;
            enemyEntry.enemy.name = GameObject.Find("GameManager/EnemyDB").GetComponent<EnemyDB>().enemies[ID].name;
            Debug.Log(enemyEntry.enemy.name);
        }
    }

}
