using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestDB : MonoBehaviour
{
    public List<BaseQuest> quests = new List<BaseQuest>();

    #region Singleton
    public static QuestDB instance; //call instance to get the single active QuestDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of QuestDB found!");
            return;
        }

        instance = this;
    }
    #endregion

    public void AddToActiveQuests(BaseQuest quest)
    {
        Debug.Log("Quest active! - " + quest.name);
        GameManager.instance.activeQuests.Add(quest);
    }

    public void CompleteQuest(BaseQuest quest)
    {
        Debug.Log("Quest completed! - " + quest.name);

        if (quest.type == BaseQuest.types.GATHER)
        {
            for (int i = 0; i < quest.gatherReqs[0].quantity; i++)
            {
                Inventory.instance.Remove(quest.gatherReqs[0].item);
            }
        }

        if (quest.type == BaseQuest.types.KILLTARGETS)
        {
            quest.killReqs[0].targetsKilled = 0;
        }

        AddGoldFromQuest(quest.rewardGold);

        AddExpFromQuest(quest.rewardExp);

        if (quest.rewardItems.Count > 0)
        {
            AddRewardItem(quest.rewardItems[0].item);
        }

        RemoveFromActiveQuests(quest);
        AddToCompletedQuests(quest);
    }

    void AddGoldFromQuest(int gold)
    {
        GameManager.instance.gold += gold;
    }

    void AddExpFromQuest(int EXP)
    {
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            hero.currentExp += EXP;
            //need to check for levelup here
        }
    }

    void AddRewardItem(Item item)
    {
        Inventory.instance.Add(item);
    }

    void RemoveFromActiveQuests(BaseQuest quest)
    {
        GameManager.instance.activeQuests.Remove(quest);
    }

    void AddToCompletedQuests(BaseQuest quest)
    {
        GameManager.instance.completedQuests.Add(quest);
    }

    public void UpdateQuestObjectives()
    {
        foreach (BaseQuest quest in GameManager.instance.activeQuests)
        {
            if (quest.type == BaseQuest.types.GATHER)
            {
                foreach (Item item in Inventory.instance.items)
                {
                    if (item == quest.gatherReqs[0].item)
                    {
                        quest.gatherReqs[0].inInventory++;
                    }
                }

                if (quest.gatherReqs[0].inInventory >= quest.gatherReqs[0].quantity)
                {
                    quest.fulfilled = true;
                } else
                {
                    quest.fulfilled = false;
                }
            }
            if (quest.type == BaseQuest.types.KILLTARGETS)
            {
                if (quest.killReqs[0].targetsKilled >= quest.killReqs[0].quantity)
                {
                    quest.fulfilled = true;
                } else
                {
                    quest.fulfilled = false;
                }
            }
            if (quest.type == BaseQuest.types.BOOLEAN)
            {
                bool boolFulfilled = true;

                foreach (BaseQuestBoolRequirement bqr in quest.boolReqs)
                {
                    if (!bqr.objectiveFulfilled)
                    {
                        boolFulfilled = false;
                    }
                }

                if (boolFulfilled)
                {
                    quest.fulfilled = true;
                } else
                {
                    quest.fulfilled = false;
                }
            }
        }
    }
}
