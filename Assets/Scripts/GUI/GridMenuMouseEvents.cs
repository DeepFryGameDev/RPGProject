using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridMenuMouseEvents : MonoBehaviour
{
    string emptyGridSpriteName = "Textfield full";
    GameMenu menu;

    public void Awake()
    {
        menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
    }

    public void ChangeSpawnPoint()
    {
        if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile)
        {
            SetSpawnPoint(gameObject.name);
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile = false;
        } else
        {
            if (gameObject.GetComponent<Image>().sprite.name != emptyGridSpriteName)
            {
                SetHero(GetHero(gameObject.GetComponent<Image>().sprite));
            }
        }
    }

    public void SetHero()
    {
        menu.PlaySE(menu.confirmSE);
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile)
        {
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridMenuHero = GetHero(gameObject.transform.Find("NameText").GetComponent<Text>().text);
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile = true;

            DisableButtons(gameObject.GetComponent<Button>());
        }
    }

    public void SetHero(BaseHero hero)
    {
        menu.PlaySE(menu.confirmSE);
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile)
        {
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridMenuHero = hero;
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile = true;

            DisableButtons(gameObject.GetComponent<Button>());
        }
    }

    BaseHero GetHero(string heroName)
    {
        foreach (BaseHero h in GameManager.instance.activeHeroes)
        {
            if (h.name == heroName)
            {
                GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridTileChanging = h.spawnPoint;
                return h;
            }
        }

        return null;
    }

    BaseHero GetHero(Sprite faceImage)
    {
        foreach (BaseHero h in GameManager.instance.activeHeroes)
        {
            if (h.faceImage == faceImage)
            {
                GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridTileChanging = h.spawnPoint;
                return h;
            }
        }
        return null;
    }

    void SetSpawnPoint(string tile)
    {
        menu.PlaySE(menu.confirmSE);

        tile = tile.Replace("Grid - ", "");
        BaseHero heroSwapping = null;
        string tempTile = null;

        foreach (BaseHero h in GameManager.instance.activeHeroes)
        {
            if (h.spawnPoint == tile && h != GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridMenuHero)
            {
                heroSwapping = h;
            }
        }

        foreach (BaseHero h in GameManager.instance.activeHeroes)
        {
            if (heroSwapping != null)
            {
                if (h == GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridMenuHero)
                {
                    tempTile = h.spawnPoint;
                    h.spawnPoint = heroSwapping.spawnPoint;
                    heroSwapping.spawnPoint = tempTile;

                    GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + h.spawnPoint).GetComponent<Image>().sprite = h.faceImage;
                    GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + heroSwapping.spawnPoint).GetComponent<Image>().sprite = heroSwapping.faceImage;
                    EnableButtons();
                    break;
                }
            } else
            {
                if (h == GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridMenuHero && tile != h.spawnPoint)
                {
                    h.spawnPoint = tile;
                    GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + tile).GetComponent<Image>().sprite = h.faceImage;
                    ClearSpawnPoint(GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridTileChanging);
                    EnableButtons();
                    break;
                }
            }
        }
    }

    void ClearSpawnPoint(string tile)
    {
        GameObject objectToClear = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + tile);
        objectToClear.GetComponent<Image>().sprite = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridBG;
    }

    void DisableButtons(Button buttonToSkip)
    {
        for (int i=1; i <= GameManager.instance.activeHeroes.Count; i++)
        {
            Button buttonToDisable = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/Hero" + i + "GridPanel").GetComponent<Button>();
            if (buttonToDisable != buttonToSkip)
            {
                buttonToDisable.interactable = false;
            }
        }
    }

    void EnableButtons()
    {
        for (int i = 1; i <= GameManager.instance.activeHeroes.Count; i++)
        {
            Button buttonToEnable = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/Hero" + i + "GridPanel").GetComponent<Button>();
            buttonToEnable.interactable = true;
        }
    }
}
