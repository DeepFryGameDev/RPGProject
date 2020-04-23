using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentsMouseEvents : MonoBehaviour
{
    public void OnMouseOver()
    {
        if (gameObject.transform.parent.name == "Talent1")
        {
            if (gameObject.name == "Talent1Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck.level1Talents[0].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck.level1Talents[0].description;
            }
            if (gameObject.name == "Talent2Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck.level1Talents[1].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck.level1Talents[1].description;
            }
            if (gameObject.name == "Talent3Button")
            {
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck.level1Talents[2].name;
                GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = GameObject.Find("GameManager").GetComponent<GameMenu>().heroToCheck.level1Talents[2].description;
            }
        }
    }

    public void OnMouseExit()
    {
        GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = "";
    }
}
