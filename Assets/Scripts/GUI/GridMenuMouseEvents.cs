using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridMenuMouseEvents : MonoBehaviour
{
   public void ChangeSpawnPoint()
    {
        if (GameObject.Find("GameManager").GetComponent<GameMenu>().gridChoosingTile)
        {
            SetSpawnPoint(gameObject.name);
            GameObject.Find("GameManager").GetComponent<GameMenu>().gridChoosingTile = false;
        }
    }

    public void SetHero()
    {
        if (!GameObject.Find("GameManager").GetComponent<GameMenu>().gridChoosingTile)
        {
            GameObject.Find("GameManager").GetComponent<GameMenu>().gridMenuHero = GetHero(gameObject.transform.Find("NameText").GetComponent<Text>().text);
            GameObject.Find("GameManager").GetComponent<GameMenu>().gridChoosingTile = true;

            DisableButtons(gameObject.GetComponent<Button>());
        }
    }

    BaseHero GetHero(string heroName)
    {
        foreach (BaseHero h in GameManager.instance.activeHeroes)
        {
            if (h._Name == heroName)
            {
                GameObject.Find("GameManager").GetComponent<GameMenu>().gridTileChanging = h.spawnPoint;
                return h;
            }
        }

        return null;
    }

    void SetSpawnPoint(string tile)
    {
        tile = tile.Replace("Grid - ", "");
        BaseHero heroSwapping = null;
        string tempTile = null;

        foreach (BaseHero h in GameManager.instance.activeHeroes)
        {
            if (h.spawnPoint == tile && h != GameObject.Find("GameManager").GetComponent<GameMenu>().gridMenuHero)
            {
                heroSwapping = h;
            }
        }

        foreach (BaseHero h in GameManager.instance.activeHeroes)
        {
            if (heroSwapping != null)
            {
                if (h == GameObject.Find("GameManager").GetComponent<GameMenu>().gridMenuHero)
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
                if (h == GameObject.Find("GameManager").GetComponent<GameMenu>().gridMenuHero && tile != h.spawnPoint)
                {
                    h.spawnPoint = tile;
                    GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + tile).GetComponent<Image>().sprite = h.faceImage;
                    ClearSpawnPoint(GameObject.Find("GameManager").GetComponent<GameMenu>().gridTileChanging);
                    EnableButtons();
                    break;
                }
            }
        }
    }

    void ClearSpawnPoint(string tile)
    {
        GameObject objectToClear = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + tile);
        objectToClear.GetComponent<Image>().sprite = GameObject.Find("GameManager").GetComponent<GameMenu>().gridBG;
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
