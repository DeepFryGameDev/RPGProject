using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BestiaryListButtonMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //For handling bestiary menu mouse cursor events - is attached to all instantiated bestiary entries

    /// <summary>
    /// Sets GUI objects in bestiary menu to hovered enemy
    /// </summary>
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

    /// <summary>
    /// Resets all GUI objects to empty fields
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().BestiaryEntryClicked)
        {
            ClearFields();
        }
    }

    /// <summary>
    /// Sets Menu to lock clicked bestiary entry so other entries won't be affected when hovered
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().BestiaryEntryClicked)
        {
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().BestiaryEntryClicked = true;
            gameObject.transform.Find("EnemyNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        }
    }

    /// <summary>
    /// Clears all GUI fields
    /// </summary>
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

    /// <summary>
    /// Returns Enemy DB entry by given ID
    /// </summary>
    /// <param name="ID">ID of enemy entry to be returned</param>
    BaseEnemyDBEntry GetEnemyDBEntry(int ID)
    {
        foreach (BaseEnemyDBEntry entry in GameObject.Find("GameManager/DBs/EnemyDB").GetComponent<EnemyDB>().enemies)
        {
            if (entry.enemy.ID == ID)
            {
                return entry;
            }
        }
        return null;
    }
}
