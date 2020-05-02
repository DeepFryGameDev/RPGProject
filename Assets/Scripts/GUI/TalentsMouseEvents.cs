using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentsMouseEvents : MonoBehaviour
{
    TalentEffects effect = new TalentEffects();
    string effectToAdd;
    string effectToRemove;
    BaseHero hero = new BaseHero();

    public void OnMouseOver()
    {
        hero = GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck;
        if (gameObject.transform.parent.name == "Talent1")
        {
            if (gameObject.name == "Talent1Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level1Talents[0].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level1Talents[0].description;
            }
            if (gameObject.name == "Talent2Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level1Talents[1].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level1Talents[1].description;
            }
            if (gameObject.name == "Talent3Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level1Talents[2].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level1Talents[2].description;
            }
        }
        if (gameObject.transform.parent.name == "Talent2")
        {
            if (gameObject.name == "Talent1Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level2Talents[0].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level2Talents[0].description;
            }
            if (gameObject.name == "Talent2Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level2Talents[1].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level2Talents[1].description;
            }
            if (gameObject.name == "Talent3Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level2Talents[2].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level2Talents[2].description;
            }
        }
        if (gameObject.transform.parent.name == "Talent3")
        {
            if (gameObject.name == "Talent1Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level3Talents[0].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level3Talents[0].description;
            }
            if (gameObject.name == "Talent2Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level3Talents[1].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level3Talents[1].description;
            }
            if (gameObject.name == "Talent3Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level3Talents[2].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level3Talents[2].description;
            }
        }
        if (gameObject.transform.parent.name == "Talent4")
        {
            if (gameObject.name == "Talent1Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level4Talents[0].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level4Talents[0].description;
            }
            if (gameObject.name == "Talent2Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level4Talents[1].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level4Talents[1].description;
            }
            if (gameObject.name == "Talent3Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level4Talents[2].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level4Talents[2].description;
            }
        }
        if (gameObject.transform.parent.name == "Talent5")
        {
            if (gameObject.name == "Talent1Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level5Talents[0].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level5Talents[0].description;
            }
            if (gameObject.name == "Talent2Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level5Talents[1].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level5Talents[1].description;
            }
            if (gameObject.name == "Talent3Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level5Talents[2].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level5Talents[2].description;
            }
        }
        if (gameObject.transform.parent.name == "Talent6")
        {
            if (gameObject.name == "Talent1Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level6Talents[0].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level6Talents[0].description;
            }
            if (gameObject.name == "Talent2Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level6Talents[1].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level6Talents[1].description;
            }
            if (gameObject.name == "Talent3Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = hero.level6Talents[2].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = hero.level6Talents[2].description;
            }
        }
    }

    public void OnMouseExit()
    {
        GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = "";
    }

    public void TalentButtonClicked()
    {
        hero = GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck;
        effectToAdd = "";
        effectToRemove = "";

        if (gameObject.transform.parent.name == "Talent1") //for debugging -- when ready, add  && hero.currentLevel >= 15
        {
            foreach (BaseTalent talent in hero.level1Talents)
            {
                if (talent.isActive)
                {
                    effectToRemove = talent.effect;
                }
            }
            if (effectToRemove != "")
            {
                effect.RemoveEffect(effectToRemove);
            }

            hero.level1Talents[0].isActive = false;
            hero.level1Talents[1].isActive = false;
            hero.level1Talents[2].isActive = false;
            if (gameObject.name == "Talent1Button")
            {
                hero.level1Talents[0].isActive = true;
                effectToAdd = hero.level1Talents[0].effect;
            }
            if (gameObject.name == "Talent2Button")
            {
                hero.level1Talents[1].isActive = true;
                effectToAdd = hero.level1Talents[1].effect;
            }
            if (gameObject.name == "Talent3Button")
            {
                hero.level1Talents[2].isActive = true;
                effectToAdd = hero.level1Talents[2].effect;
            }
            
            effect.AddEffect(effectToAdd);

            SetActiveTalent(gameObject.transform.parent.gameObject, gameObject);
        }

        if (gameObject.transform.parent.name == "Talent2" && hero.currentLevel >= 30)
        {
            hero.level2Talents[0].isActive = false;
            hero.level2Talents[1].isActive = false;
            hero.level2Talents[2].isActive = false;
            if (gameObject.name == "Talent1Button")
            {
                hero.level2Talents[0].isActive = true;
            }
            if (gameObject.name == "Talent2Button")
            {
                hero.level2Talents[1].isActive = true;
            }
            if (gameObject.name == "Talent3Button")
            {
                hero.level2Talents[2].isActive = true;
            }
            SetActiveTalent(gameObject.transform.parent.gameObject, gameObject);
        }
        if (gameObject.transform.parent.name == "Talent3" && hero.currentLevel >= 45)
        {
            hero.level3Talents[0].isActive = false;
            hero.level3Talents[1].isActive = false;
            hero.level3Talents[2].isActive = false;
            if (gameObject.name == "Talent1Button")
            {
                hero.level3Talents[0].isActive = true;
            }
            if (gameObject.name == "Talent2Button")
            {
                hero.level3Talents[1].isActive = true;
            }
            if (gameObject.name == "Talent3Button")
            {
                hero.level3Talents[2].isActive = true;
            }
            SetActiveTalent(gameObject.transform.parent.gameObject, gameObject);
        }
        if (gameObject.transform.parent.name == "Talent4" && hero.currentLevel >= 60)
        {
            hero.level4Talents[0].isActive = false;
            hero.level4Talents[1].isActive = false;
            hero.level4Talents[2].isActive = false;
            if (gameObject.name == "Talent1Button")
            {
                hero.level4Talents[0].isActive = true;
            }
            if (gameObject.name == "Talent2Button")
            {
                hero.level4Talents[1].isActive = true;
            }
            if (gameObject.name == "Talent3Button")
            {
                hero.level4Talents[2].isActive = true;
            }
            SetActiveTalent(gameObject.transform.parent.gameObject, gameObject);
        }
        if (gameObject.transform.parent.name == "Talent5" && hero.currentLevel >= 75)
        {
            hero.level5Talents[0].isActive = false;
            hero.level5Talents[1].isActive = false;
            hero.level5Talents[2].isActive = false;
            if (gameObject.name == "Talent1Button")
            {
                hero.level5Talents[0].isActive = true;
            }
            if (gameObject.name == "Talent2Button")
            {
                hero.level5Talents[1].isActive = true;
            }
            if (gameObject.name == "Talent3Button")
            {
                hero.level5Talents[2].isActive = true;
            }
            SetActiveTalent(gameObject.transform.parent.gameObject, gameObject);
        }
        if (gameObject.transform.parent.name == "Talent6" && hero.currentLevel >= 90)
        {
            hero.level6Talents[0].isActive = false;
            hero.level6Talents[1].isActive = false;
            hero.level6Talents[2].isActive = false;
            if (gameObject.name == "Talent1Button")
            {
                hero.level6Talents[0].isActive = true;
            }
            if (gameObject.name == "Talent2Button")
            {
                hero.level6Talents[1].isActive = true;
            }
            if (gameObject.name == "Talent3Button")
            {
                hero.level6Talents[2].isActive = true;
            }
            SetActiveTalent(gameObject.transform.parent.gameObject, gameObject);
        }
    }

    void DrawInactiveTalent(Image icon)
    {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, .25f);
    }

    void DrawActiveTalent(Image icon)
    {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
    }

    void SetActiveTalent(GameObject parent, GameObject talentChosen)
    {
        DrawInactiveTalent(GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/" + parent.name + "/Talent1Button/TalentIcon").GetComponent<Image>());
        DrawInactiveTalent(GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/" + parent.name + "/Talent2Button/TalentIcon").GetComponent<Image>());
        DrawInactiveTalent(GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/" + parent.name + "/Talent3Button/TalentIcon").GetComponent<Image>());

        DrawActiveTalent(GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/" + parent.name + "/" + talentChosen.name + "/TalentIcon").GetComponent<Image>());
        
        GameObject.Find("GameManager").GetComponent<GameMenu>().DrawTalentsMenuHeroPanel(GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck);
    }
}