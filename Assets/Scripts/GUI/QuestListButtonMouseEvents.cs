using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestListButtonMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    GameMenu menu;
    AudioManager AM;

    public void Awake()
    {
        menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
        AM = GameObject.Find("GameManager").GetComponent<AudioManager>();
    }

    /// <summary>
    /// Changes quest menu text components to hovered quest object
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestOption == "Active")
        {
            if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestClicked)
            {
                int ID = int.Parse(gameObject.name.Replace("ActiveQuestListButton - ID ", ""));

                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestNamePanel/QuestNameText").GetComponent<Text>().text = GetQuestByID(ID).name;
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestLevelText").GetComponent<Text>().text = GetQuestByID(ID).level.ToString();
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq1").GetComponent<Text>().text = GetQuestReq(GetQuestByID(ID), 1); //This needs to be updated to allow checking multiple requirements
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq2").GetComponent<Text>().text = "";
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq3").GetComponent<Text>().text = "";

                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestDetailsPanel/QuestDescriptionText").GetComponent<Text>().text = GetQuestByID(ID).description;

                if (GetQuestByID(ID).rewardGold > 0)
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>().text = GetQuestByID(ID).rewardGold.ToString();
                }
                else
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>().text = "00";
                }

                if (GetQuestByID(ID).rewardExp > 0)
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ExpText").GetComponent<Text>().text = GetQuestByID(ID).rewardExp.ToString();
                }
                else
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ExpText").GetComponent<Text>().text = "00";
                }

                if (GetQuestByID(ID).rewardItems.Count > 0)
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color = new Color(GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.r,
    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.g,
    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.b, 1.0f);
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().sprite = GetQuestByID(ID).rewardItems[0].item.icon;

                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemText").GetComponent<Text>().text = GetQuestByID(ID).rewardItems[0].item.name;
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemDescription").GetComponent<Text>().text = GetQuestByID(ID).rewardItems[0].item.description;
                }
                else
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemText").GetComponent<Text>().text = "None";
                }
            }
        }

        if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestOption == "Completed")
        {
            if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestClicked)
            {
                int ID = int.Parse(gameObject.name.Replace("ActiveQuestListButton - ID ", ""));

                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestNamePanel/QuestNameText").GetComponent<Text>().text = GetQuestByID(ID).name;
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestLevelRequirementsPanel/QuestLevelText").GetComponent<Text>().text = GetQuestByID(ID).level.ToString();
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq1").GetComponent<Text>().text = GetQuestReq(GetQuestByID(ID), 1); //This needs to be updated to allow checking multiple requirements
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq2").GetComponent<Text>().text = "";
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq3").GetComponent<Text>().text = "";

                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestDetailsPanel/QuestDescriptionText").GetComponent<Text>().text = GetQuestByID(ID).description;

                if (GetQuestByID(ID).rewardGold > 0)
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>().text = GetQuestByID(ID).rewardGold.ToString();
                }
                else
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>().text = "00";
                }

                if (GetQuestByID(ID).rewardExp > 0)
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ExpText").GetComponent<Text>().text = GetQuestByID(ID).rewardExp.ToString();
                }
                else
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ExpText").GetComponent<Text>().text = "00";
                }

                if (GetQuestByID(ID).rewardItems.Count > 0)
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color = new Color(GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.r,
    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.g,
    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.b, 1.0f);
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().sprite = GetQuestByID(ID).rewardItems[0].item.icon;

                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemText").GetComponent<Text>().text = GetQuestByID(ID).rewardItems[0].item.name;
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemDescription").GetComponent<Text>().text = GetQuestByID(ID).rewardItems[0].item.description;
                }
                else
                {
                    GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemText").GetComponent<Text>().text = "None";
                }
            }
        }
    }

    /// <summary>
    /// Clears quest menu text fields
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestClicked)
        {
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().ClearQuestMenuFields();
        }
    }

    /// <summary>
    /// Sets chosen quest to track on clicked quest so OnPointerEnter doesn't trigger
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        menu.PlaySE(AM.confirmSE);

        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestClicked = true;
        gameObject.transform.Find("QuestNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        gameObject.transform.Find("QuestLevelText").GetComponent<Text>().fontStyle = FontStyle.Bold;
    }

    /// <summary>
    /// Returns quest by given ID
    /// </summary>
    /// <param name="ID">Id of quest to return</param>
    BaseQuest GetQuestByID(int ID)
    {
        foreach (BaseQuest quest in QuestDB.instance.quests)
        {
            if (quest.ID == ID)
            {
                return quest;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns quest requirement in text format by given quest and requirement index
    /// </summary>
    /// <param name="quest">Quest to gather requirement</param>
    /// <param name="reqIndex">Requirement index to check</param>
    string GetQuestReq(BaseQuest quest, int reqIndex)
    {
        string req = "";

        reqIndex = reqIndex - 1;

        if (quest.type == BaseQuest.types.GATHER)
        {
            int itemCount = 0;
            foreach (Item item in Inventory.instance.items)
            {
                if (item == quest.gatherReqs[reqIndex].item && itemCount < quest.gatherReqs[reqIndex].quantity)
                {
                    itemCount++;
                }
            }

            if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestOption == "Active")
            {
                req = quest.gatherReqs[reqIndex].item.name + ": " + itemCount + "/" + quest.gatherReqs[reqIndex].quantity;
            }

            if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestOption == "Completed")
            {
                req = quest.gatherReqs[reqIndex].item.name + ": " + quest.gatherReqs[0].quantity + "/" + quest.gatherReqs[reqIndex].quantity;
            }
        }

        if (quest.type == BaseQuest.types.KILLTARGETS)
        {
            int targetsKilled = quest.killReqs[reqIndex].targetsKilled;
            if (targetsKilled > quest.killReqs[reqIndex].quantity)
            {
                targetsKilled = quest.killReqs[reqIndex].quantity;
            }

            if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestOption == "Active")
            {
                req = GetEnemy(quest.killReqs[reqIndex].enemyID).name + ": " + targetsKilled + "/" + quest.killReqs[reqIndex].quantity;
            }

            if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestOption == "Completed")
            {
                req = GetEnemy(quest.killReqs[reqIndex].enemyID).name + ": " + quest.killReqs[reqIndex].quantity + "/" + quest.killReqs[reqIndex].quantity;
            }
        }

        if (quest.type == BaseQuest.types.BOOLEAN)
        {
            if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestOption == "Active")
            {
                if (quest.boolReqs[reqIndex].objectiveFulfilled)
                {
                    req = quest.boolReqs[reqIndex].boolDescription + " - Done";
                }
                else
                {
                    req = quest.boolReqs[reqIndex].boolDescription;
                }
            }

            if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().QuestOption == "Completed")
            {
                req = quest.boolReqs[reqIndex].boolDescription + " - Done";
            }            
        }

        return req;
    }

    /// <summary>
    /// Returns enemy based on given ID
    /// </summary>
    /// <param name="ID">ID of enemy to be returned</param>
    BaseEnemy GetEnemy(int ID)
    {
        foreach (BaseEnemyDBEntry entry in EnemyDB.instance.enemies)
        {
            if (entry.enemy.ID == ID)
            {
                return entry.enemy;
            }
        }
        return null;
    }
}
