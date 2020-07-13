using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridMenuMouseEvents : MonoBehaviour
{
    //Used in Grid menu to interact with mouse cursor

    string emptyGridSpriteName = "Textfield full";
    GameMenu menu;
    AudioManager AM;

    public void Awake()
    {
        menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
        AM = GameObject.Find("GameManager").GetComponent<AudioManager>();
    }

    /// <summary>
    /// Changes spawn point for clicked hero in Grid menu - if no hero is set, the hero is set to the hero on tile clicked
    /// </summary>
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

    /// <summary>
    /// Sets hero for grid menu to change grid spawn point
    /// </summary>
    public void SetHero()
    {
        menu.PlaySE(AM.confirmSE);
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile)
        {
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridMenuHero = GetHero(int.Parse(gameObject.name.Replace("HeroGridPanel - ID: ","")));
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile = true;

            DisableButtons(gameObject.GetComponent<Button>());
        }
    }

    /// <summary>
    /// Sets hero for grid menu to change grid spawn point using provided hero
    /// </summary>
    /// <param name="hero">Hero to set for spawn point changing</param>
    public void SetHero(BaseHero hero)
    {
        menu.PlaySE(AM.confirmSE);
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile)
        {
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridMenuHero = hero;
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridChoosingTile = true;

            DisableButtons(gameObject.GetComponent<Button>());
        }
    }

    /// <summary>
    /// Returns hero based on given name set in UI
    /// </summary>
    /// <param name="name">ID of hero clicked in interface</param>
    BaseHero GetHero(int ID)
    {
        foreach (BaseHero h in GameManager.instance.activeHeroes)
        {
            if (h.ID == ID)
            {
                GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridTileChanging = h.spawnPoint;
                return h;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns hero based on provided sprite in interface
    /// </summary>
    /// <param name="faceImage">Sprite object to return hero</param>
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

    /// <summary>
    /// Sets spawn point for grid menu chosen hero to given tile
    /// </summary>
    /// <param name="tile">Which grid tile to set chosen hero to</param>
    void SetSpawnPoint(string tile)
    {
        menu.PlaySE(AM.confirmSE);

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

    /// <summary>
    /// Sets grid menu interface tile to empty
    /// </summary>
    /// <param name="tile">Provided tile to clear</param>
    void ClearSpawnPoint(string tile)
    {
        GameObject objectToClear = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + tile);
        objectToClear.GetComponent<Image>().sprite = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().gridBG;
    }

    /// <summary>
    /// Sets given button as 'interactable = false;
    /// </summary>
    /// <param name="buttonToSkip">Button to disable</param>
    void DisableButtons(Button buttonToSkip)
    {
        for (int i=0; i < GameManager.instance.activeHeroes.Count; i++)
        {
            Button buttonToDisable = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/HeroGridPanel - ID: " + GameManager.instance.activeHeroes[i].ID).GetComponent<Button>();
            if (buttonToDisable != buttonToSkip)
            {
                buttonToDisable.interactable = false;
            }
        }
    }

    /// <summary>
    /// Sets all buttons as interactable
    /// </summary>
    void EnableButtons()
    {
        for (int i = 0; i < GameManager.instance.activeHeroes.Count; i++)
        {
            Button buttonToEnable = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/HeroGridPanel - ID: " + GameManager.instance.activeHeroes[i].ID).GetComponent<Button>();
            buttonToEnable.interactable = true;
        }
    }
}
