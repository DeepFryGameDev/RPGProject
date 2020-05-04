using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BestiaryListButtonMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().BestiaryEntryClicked)
        {
            int ID = int.Parse(gameObject.name.Replace("BestiaryEnemyEntryButton - ID ", ""));
            BaseEnemyDBEntry entry = GetEnemyDBEntry(ID);
            BaseEnemy enemy = entry.enemy;

            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyNamePanel/EnemyNameText").GetComponent<Text>().text = enemy.name;
            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyLevelPanel/EnemyLevelText").GetComponent<Text>().text = enemy.level.ToString();

            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color = new Color(
                GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.r,
                GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.g,
                GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.b,
                1);
            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().sprite = entry.prefab.GetComponent<SpriteRenderer>().sprite;

            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyDescriptionPanel/EnemyDescriptionText").GetComponent<Text>().text = entry.description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().BestiaryEntryClicked)
        {
            ClearFields();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().BestiaryEntryClicked = true;
        gameObject.transform.Find("EnemyNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
    }

    void ClearFields()
    {
        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyNamePanel/EnemyNameText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyLevelPanel/EnemyLevelText").GetComponent<Text>().text = "";

        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color = new Color(
            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.r,
            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.g,
            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.b,
            0);
        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().sprite = null;

        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyDescriptionPanel/EnemyDescriptionText").GetComponent<Text>().text = "";
    }

    BaseEnemyDBEntry GetEnemyDBEntry(int ID)
    {
        foreach (BaseEnemyDBEntry entry in GameObject.Find("GameManager/EnemyDB").GetComponent<EnemyDB>().enemies)
        {
            if (entry.enemy.ID == ID)
            {
                return entry;
            }
        }
        return null;
    }
}
