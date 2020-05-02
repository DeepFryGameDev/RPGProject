using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestListButtonMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameObject.Find("GameManager").GetComponent<GameMenu>().QuestClicked)
        {
            int ID = int.Parse(gameObject.name.Replace("ActiveQuestListButton - ID ", ""));

            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestNamePanel/QuestNameText").GetComponent<Text>().text = GetQuestByID(ID).name;
            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestLevelText").GetComponent<Text>().text = GetQuestByID(ID).level.ToString();
            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq1").GetComponent<Text>().text = GetQuestReq1(GetQuestByID(ID)); //update this to show requirements (make new method)
            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq2").GetComponent<Text>().text = "";
            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq3").GetComponent<Text>().text = "";

            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestDetailsPanel/QuestDescriptionText").GetComponent<Text>().text = GetQuestByID(ID).description;

            if (GetQuestByID(ID).rewardGold > 0)
            {
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>().text = GetQuestByID(ID).rewardGold.ToString();
            } else
            {
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>().text = "00";
            }
            
            if (GetQuestByID(ID).rewardExp > 0)
            {
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ExpText").GetComponent<Text>().text = GetQuestByID(ID).rewardExp.ToString();
            } else
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
            } else
            {
                GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemText").GetComponent<Text>().text = "None";
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GameObject.Find("GameManager").GetComponent<GameMenu>().QuestClicked)
        {
            GameObject.Find("GameManager").GetComponent<GameMenu>().ClearQuestMenuFields();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("GameManager").GetComponent<GameMenu>().QuestClicked = true;
        gameObject.transform.Find("QuestNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        gameObject.transform.Find("QuestLevelText").GetComponent<Text>().fontStyle = FontStyle.Bold;
    }

    BaseQuest GetQuestByID(int ID)
    {
        foreach (BaseQuest quest in GameObject.Find("GameManager/QuestDB").GetComponent<QuestDB>().quests)
        {
            if (quest.ID == ID)
            {
                return quest;
            }
        }
        return null;
    }

    string GetQuestReq1(BaseQuest quest)
    {
        string req = "";

        if (quest.type == BaseQuest.types.GATHER)
        {
            int itemCount = 0;
            foreach (Item item in Inventory.instance.items)
            {
                if (item == quest.gatherReqs[0].item)
                {
                    itemCount++;
                }
            }
            req = quest.gatherReqs[0].item.name + ": " + itemCount + "/" + quest.gatherReqs[0].quantity;
        }

        if (quest.type == BaseQuest.types.KILLTARGETS)
        {
            int targetsKilled = quest.killReqs[0].targetsKilled;
            if (targetsKilled > quest.killReqs[0].quantity)
            {
                targetsKilled = quest.killReqs[0].quantity;
            }
            req = quest.killReqs[0].target.GetComponent<EnemyStateMachine>().name + ": " + targetsKilled + "/" + quest.killReqs[0].quantity;
        }

        if (quest.type == BaseQuest.types.BOOLEAN)
        {
            if (quest.boolReqs[0].objectiveFulfilled)
            {
                req = quest.boolReqs[0].boolDescription + " - Done";
            } else
            {
                req = quest.boolReqs[0].boolDescription;
            }
            
        }

        return req;
    }
}
