using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySelectEvents : MonoBehaviour
{
    Image SelectedHeroMenuHPProgressBar;
    Image SelectedHeroMenuMPProgressBar;
    Image SelectedHeroMenuEXPProgressBar;
    GameMenu menu;

    public void Awake()
    {
        SelectedHeroMenuHPProgressBar = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        SelectedHeroMenuMPProgressBar = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        SelectedHeroMenuEXPProgressBar = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/LevelProgressBarBG/LevelProgressBar").GetComponent<Image>();
        menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel") && menu.PartyHeroSelected != null) //if cancel is pressed after selecting a hero
        {
            menu.PartyHeroSelected = null;
            HideSelectedHeroPanel();
            UnboldButtons();
            menu.PartySelectedHeroType = "";
        }
    }

    public void ShowSelectedHeroPanel()
    {
        if (menu.PartyHeroSelected == null)
        {
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

            BaseHero hoveredHero = GetHeroByID(int.Parse(gameObject.name.Replace("Inactive Hero Button - ID", "")));
            DrawHeroFace(hoveredHero, GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/FacePanel").GetComponent<Image>());
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/NameText").GetComponent<Text>().text = hoveredHero.name;
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/LevelText").GetComponent<Text>().text = hoveredHero.currentLevel.ToString(); //Level text
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/HPText").GetComponent<Text>().text = (hoveredHero.curHP + " / " + hoveredHero.finalMaxHP); //HP text
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/MPText").GetComponent<Text>().text = (hoveredHero.curMP + " / " + hoveredHero.finalMaxMP); //MP text

            if (hoveredHero.currentLevel == 1)
            {
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/EXPText").GetComponent<Text>().text = hoveredHero.currentExp.ToString(); //Exp text
            }
            else
            {
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/EXPText").GetComponent<Text>().text = (hoveredHero.currentExp + " / " + HeroDB.instance.levelEXPThresholds[(hoveredHero.currentLevel - 1)]); //Exp text
            }

            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/ToNextLevelText").GetComponent<Text>().text = (HeroDB.instance.levelEXPThresholds[hoveredHero.currentLevel - 1] - hoveredHero.currentExp).ToString(); //To next level text

            SelectedHeroMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hoveredHero), 0, 1), SelectedHeroMenuHPProgressBar.transform.localScale.y);
            SelectedHeroMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hoveredHero), 0, 1), SelectedHeroMenuMPProgressBar.transform.localScale.y);
            SelectedHeroMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(hoveredHero), 0, 1), SelectedHeroMenuEXPProgressBar.transform.localScale.y);
        }        
    }

    public void ShowSelectedActiveHeroPanel()
    {
        if (menu.PartyHeroSelected == null)
        {
            if (gameObject.name.Contains(" - ID"))
            {
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().alpha = 1;
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().interactable = true;
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

                BaseHero hoveredHero = GetHeroByID(int.Parse(gameObject.name.Replace("Active Hero Panel - ID", "")));
                DrawHeroFace(hoveredHero, GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/FacePanel").GetComponent<Image>());
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/NameText").GetComponent<Text>().text = hoveredHero.name;
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/LevelText").GetComponent<Text>().text = hoveredHero.currentLevel.ToString(); //Level text
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/HPText").GetComponent<Text>().text = (hoveredHero.curHP + " / " + hoveredHero.finalMaxHP); //HP text
                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/MPText").GetComponent<Text>().text = (hoveredHero.curMP + " / " + hoveredHero.finalMaxMP); //MP text

                if (hoveredHero.currentLevel == 1)
                {
                    GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/EXPText").GetComponent<Text>().text = hoveredHero.currentExp.ToString(); //Exp text
                }
                else
                {
                    GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/EXPText").GetComponent<Text>().text = (hoveredHero.currentExp + " / " + HeroDB.instance.levelEXPThresholds[(hoveredHero.currentLevel - 1)]); //Exp text
                }

                GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel/ToNextLevelText").GetComponent<Text>().text = (HeroDB.instance.levelEXPThresholds[hoveredHero.currentLevel - 1] - hoveredHero.currentExp).ToString(); //To next level text

                SelectedHeroMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hoveredHero), 0, 1), SelectedHeroMenuHPProgressBar.transform.localScale.y);
                SelectedHeroMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hoveredHero), 0, 1), SelectedHeroMenuMPProgressBar.transform.localScale.y);
                SelectedHeroMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(hoveredHero), 0, 1), SelectedHeroMenuEXPProgressBar.transform.localScale.y);
            }
        }
    }

    public void HideSelectedHeroPanel()
    {
        if (menu.PartyHeroSelected == null)
        {
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void SelectHeroFromInactive()
    {
        menu.PlaySE(menu.confirmSE);
        if (menu.PartyHeroSelected == null)
        {
            menu.PartyHeroSelected = GetHeroByID(int.Parse(gameObject.name.Replace("Inactive Hero Button - ID ", "")));

            gameObject.transform.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Bold;

            menu.PartySelectedHeroType = "Inactive";
        }

        if (menu.PartyHeroSelected != null && menu.PartySelectedHeroType == "Active")
        {
            Debug.Log("process the switch from active to inactive");
            Debug.Log("Switching " + GameObject.Find("GameManager").GetComponent<GameMenu>().PartyHeroSelected.name + " with " + GetHeroByID(int.Parse(gameObject.name.Replace("Inactive Hero Button - ID ", ""))).name);
            SwitchActiveToInactive(GameObject.Find("GameManager").GetComponent<GameMenu>().PartyHeroSelected, GetHeroByID(int.Parse(gameObject.name.Replace("Inactive Hero Button - ID ", ""))));
        }
    }

    public void SelectHeroFromActive()
    {
        menu.PlaySE(menu.confirmSE);
        if (menu.PartyHeroSelected == null)
        {
            if (gameObject.name.Contains(" - ID"))
            {
                menu.PartyHeroSelected = GetHeroByID(int.Parse(gameObject.name.Replace("Active Hero Panel - ID ", "")));

                gameObject.transform.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Bold;

                menu.PartySelectedHeroType = "Active";
                return;
            }
        }

        if (menu.PartyHeroSelected != null && menu.PartySelectedHeroType == "Inactive")
        {
            Debug.Log("process the switch from inactive to active");

            foreach (BaseHero hero in GameManager.instance.activeHeroes)
            {
                if (hero.spawnPoint == menu.PartyHeroSelected.spawnPoint)
                {
                    Debug.Log("Assigning new random spawn point for " + menu.PartyHeroSelected.name);

                    while (hero.spawnPoint == menu.PartyHeroSelected.spawnPoint)
                    {
                        int randomColumn = Random.Range(1, 5);
                        int randomRow = Random.Range(1, 5);

                        string spawnPoint = randomColumn.ToString() + randomRow.ToString();

                        menu.PartyHeroSelected.spawnPoint = spawnPoint;
                        Debug.Log("Setting random spawn point for " + menu.PartyHeroSelected.name + " - " + spawnPoint);
                    }
                }
            }

            if (gameObject.name == "Empty Hero Panel")
            {
                Debug.Log("switching to empty panel");
                AddInactiveToEmpty(menu.PartyHeroSelected);
                
            } else
            {
                Debug.Log("Switching " + menu.PartyHeroSelected.name + " with " + GetHeroByID(int.Parse(gameObject.name.Replace("Active Hero Panel - ID ", ""))).name);
                SwitchInactiveToActive(menu.PartyHeroSelected, GetHeroByID(int.Parse(gameObject.name.Replace("Active Hero Panel - ID ", ""))));
            }
        }

        if (menu.PartyHeroSelected != null && menu.PartySelectedHeroType == "Active" && gameObject.name.Contains("Active Hero Panel - ID "))
        {
            BaseHero heroClicked = GetHeroByID(int.Parse(gameObject.name.Replace("Active Hero Panel - ID ", "")));

            if (heroClicked != menu.PartyHeroSelected)
            {
                Debug.Log("Switching " + menu.PartyHeroSelected.name + " with " + GetHeroByID(int.Parse(gameObject.name.Replace("Active Hero Panel - ID ", ""))).name);
                SwitchActives(menu.PartyHeroSelected, GetHeroByID(int.Parse(gameObject.name.Replace("Active Hero Panel - ID ", ""))));
            }
        }
    }

    void AddInactiveToEmpty(BaseHero hero)
    {
        GameManager.instance.inactiveHeroes.Remove(hero);
        GameManager.instance.activeHeroes.Add(hero);

        menu.DrawPartyActiveHeroes();
        menu.DrawInactiveHeroButtons();

        menu.PartyHeroSelected = null;
        HideSelectedHeroPanel();
        UnboldButtons();
        menu.PartySelectedHeroType = "";
    }

    void SwitchInactiveToActive(BaseHero fromHero, BaseHero toHero)
    {
        int activeHeroIndex = GameManager.instance.activeHeroes.IndexOf(toHero);
        int inactiveHeroIndex = GameManager.instance.inactiveHeroes.IndexOf(fromHero);
        
        GameManager.instance.activeHeroes.Insert(activeHeroIndex, fromHero);
        GameManager.instance.activeHeroes.Remove(toHero);

        GameManager.instance.inactiveHeroes.Insert(inactiveHeroIndex, toHero);
        GameManager.instance.inactiveHeroes.Remove(fromHero);

        menu.DrawPartyActiveHeroes();
        menu.DrawInactiveHeroButtons();

        menu.PartyHeroSelected = null;
        HideSelectedHeroPanel();
        UnboldButtons();
        menu.PartySelectedHeroType = "";
    }

    void SwitchActiveToInactive(BaseHero fromHero, BaseHero toHero)
    {
        int activeHeroIndex = GameManager.instance.activeHeroes.IndexOf(fromHero);
        int inactiveHeroIndex = GameManager.instance.inactiveHeroes.IndexOf(toHero);

        GameManager.instance.activeHeroes.Insert(activeHeroIndex, toHero);
        GameManager.instance.activeHeroes.Remove(fromHero);

        GameManager.instance.inactiveHeroes.Insert(inactiveHeroIndex, fromHero);
        GameManager.instance.inactiveHeroes.Remove(toHero);

        menu.DrawPartyActiveHeroes();
        menu.DrawInactiveHeroButtons();

        menu.PartyHeroSelected = null;
        HideSelectedHeroPanel();
        UnboldButtons();
        menu.PartySelectedHeroType = "";
    }

    void SwitchActives(BaseHero fromHero, BaseHero toHero)
    {
        int activeHeroIndex = GameManager.instance.activeHeroes.IndexOf(fromHero);
        int otherActiveHeroIndex = GameManager.instance.activeHeroes.IndexOf(toHero);
        
        if (activeHeroIndex > otherActiveHeroIndex)
        {
            GameManager.instance.activeHeroes.Remove(fromHero);
            GameManager.instance.activeHeroes.Remove(toHero);

            GameManager.instance.activeHeroes.Insert(otherActiveHeroIndex, fromHero);
            GameManager.instance.activeHeroes.Insert(activeHeroIndex, toHero);
        } else
        {
            GameManager.instance.activeHeroes.Remove(fromHero);
            GameManager.instance.activeHeroes.Remove(toHero);

            GameManager.instance.activeHeroes.Insert(activeHeroIndex, toHero);
            GameManager.instance.activeHeroes.Insert(otherActiveHeroIndex, fromHero);
        }
        

        menu.DrawPartyActiveHeroes();

        menu.PartyHeroSelected = null;
        HideSelectedHeroPanel();
        UnboldButtons();
        menu.PartySelectedHeroType = "";
    }

    void UnboldButtons()
    {
        foreach (Transform child in GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/HeroRow1Spacer").transform)
        {
            child.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
        foreach (Transform child in GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/HeroRow2Spacer").transform)
        {
            child.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
        foreach (Transform child in GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/HeroRow3Spacer").transform)
        {
            child.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        foreach (Transform child in GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform)
        {
            child.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
    }

    BaseHero GetHeroByID(int ID)
    {
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            if (hero.ID == ID)
            {
                return hero;
            }
        }

        foreach (BaseHero hero in GameManager.instance.inactiveHeroes)
        {
            if (hero.ID == ID)
            {
                return hero;
            }
        }

        return null;
    }

    void DrawHeroFace(BaseHero hero, Image faceImage)
    {
        if (GameManager.instance.activeHeroes.Contains(hero))
        {
            int index = GameManager.instance.activeHeroes.IndexOf(hero);
            faceImage.sprite = GameManager.instance.activeHeroes[index].faceImage;
        }

        if (GameManager.instance.inactiveHeroes.Contains(hero))
        {
            int index = GameManager.instance.inactiveHeroes.IndexOf(hero);
            faceImage.sprite = GameManager.instance.inactiveHeroes[index].faceImage;
        }
    }

    float GetProgressBarValuesHP(BaseHero hero)
    {
        float heroHP = hero.curHP;
        float heroBaseHP = hero.finalMaxHP;
        float calc_HP;

        calc_HP = heroHP / heroBaseHP;

        return calc_HP;
    }

    float GetProgressBarValuesMP(BaseHero hero)
    {
        float heroMP = hero.curMP;
        float heroBaseMP = hero.finalMaxMP;
        float calc_MP;

        calc_MP = heroMP / heroBaseMP;

        return calc_MP;
    }

    float GetProgressBarValuesEXP(BaseHero hero)
    {
        float baseLineEXP;
        float heroEXP;

        if (hero.currentLevel == 1)
        {
            baseLineEXP = (HeroDB.instance.levelEXPThresholds[hero.currentLevel - 1]);
            heroEXP = hero.currentExp;
        }
        else
        {
            baseLineEXP = (HeroDB.instance.levelEXPThresholds[hero.currentLevel - 1] - HeroDB.instance.levelEXPThresholds[hero.currentLevel - 2]);
            heroEXP = (hero.currentExp - baseLineEXP);
            //Debug.Log("baseLine: " + baseLineEXP);
        }

        //Debug.Log(heroEXP + " / " + baseLineEXP + ": " + calcEXP);

        float calcEXP = heroEXP / baseLineEXP;

        return calcEXP;
    }
}
