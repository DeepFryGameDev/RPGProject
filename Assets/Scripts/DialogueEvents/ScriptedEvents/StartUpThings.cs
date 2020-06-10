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

        AddEquipment(0);
        AddEquipment(1);
        AddEquipment(2);
        AddEquipment(3);
        AddEquipment(4);
        AddEquipment(5);
        AddEquipment(6);
        AddEquipment(7);

        //DisableButtons(); //<-----Enable when menu is complete
    }

    void InitiateStats()
    {
        //Debug.Log("Initializing stats");
        foreach (BaseHero hero in HeroDB.instance.heroes)
        {
            hero.InitializeStats();
        }
    }

    void SetRandomGridSpawnPoints()
    {
        List<string> savedSpawnPoints = new List<string>();

        foreach (BaseHero hero in HeroDB.instance.heroes)
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
        foreach (BaseQuest quest in QuestDB.instance.quests)
        {
            Debug.Log("Assigning ID " + QuestDB.instance.quests.IndexOf(quest) + " to quest " + quest.name);
            quest.ID = QuestDB.instance.quests.IndexOf(quest);
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
        foreach (BaseHero hero in HeroDB.instance.heroes)
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
        foreach (BaseHero hero in HeroDB.instance.heroes)
        {
            hero.ID = HeroDB.instance.heroes.IndexOf(hero);
        }
    }

    public void AssignEnemyIDs()
    {
        int ID;
        foreach (BaseEnemyDBEntry enemyEntry in EnemyDB.instance.enemies)
        {
            ID = EnemyDB.instance.enemies.IndexOf(enemyEntry);
            enemyEntry.enemy.ID = ID;
            enemyEntry.enemy.name = EnemyDB.instance.enemies[ID].name;
        }
    }

    void DisableButtons()
    {
        ChangeMenuButtonAccess(MenuButtons.Talents, false);
        ChangeMenuButtonAccess(MenuButtons.Party, false);
        ChangeMenuButtonAccess(MenuButtons.Grid, false);
        ChangeMenuButtonAccess(MenuButtons.Quests, false);
        ChangeMenuButtonAccess(MenuButtons.Bestiary, false);
    }

}
