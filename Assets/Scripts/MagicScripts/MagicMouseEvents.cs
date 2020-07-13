using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler //for setting magic menu details
{

    Text magicDesc;
    Text MPCostText;
    Text cooldownText;
    BaseHero heroToCastOn;
    BaseHero heroFromMenu;
    BaseMagicScript magicScript;
    GameMenu menu;
    AudioManager AM;
    BaseAttack magicUsed;

    Coroutine processMagic = null;
    Coroutine chooseHero = null;

    private void Start()
    {
        magicDesc = GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDescriptionPanel/MagicDescriptionText").GetComponent<Text>();
        MPCostText = GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel/MPCostText").GetComponent<Text>();
        cooldownText = GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel/CooldownText").GetComponent<Text>();
        menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>(); //the menu script on GameManager
        AM = GameObject.Find("GameManager").GetComponent<AudioManager>();
        heroFromMenu = menu.heroToCheck; //hero that was chosen to enter the magic menu with
    }

    BaseAttack GetMagic(string name) //gets the actual magic spell
    {
        foreach (BaseAttack magic in heroFromMenu.MagicAttacks)
        {
            if (magic.name == name)
            {
                return magic;
            }
        }
        return null;
    }

    string GetMagicName() //gets the spell name based on which panel is clicked
    {
        return gameObject.transform.Find("Name").GetComponent<Text>().text;
    }

    public void OnPointerEnter(PointerEventData eventData) //sets magic details on magic menu based on which magic panel is being hovered
    {
        magicDesc.text = GetMagic(GetMagicName()).description;
        MPCostText.text = GetMagic(GetMagicName()).MPCost.ToString();
        cooldownText.text = GetMagic(GetMagicName()).cooldown.ToString();
    }

    public void OnPointerExit(PointerEventData eventData) //sets magic details on magic menus to blank values when not hovering any magic panels
    {
        magicDesc.text = "";
        MPCostText.text = "-";
        cooldownText.text = "-";
    }

    public void OnPointerClick(PointerEventData eventData) //starts magic processing upon clicking magic spell
    {
        if (GetMagic(GetMagicName()).usableInMenu && GetMagic(GetMagicName()).enoughMP)
        {
            if (GetMagic(GetMagicName()).useState == BaseAttack.UseStates.HERO)
            {
                magicUsed = GetMagic(GetMagicName());
                processMagic = StartCoroutine(ProcessMagic());
            }
        } else
        {
            menu.PlaySE(AM.cantActionSE);
        }
    }

    static List<RaycastResult> GetEventSystemRaycastResults() //gets all objects clicked 
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public IEnumerator ProcessMagic()
    {
        menu.DisplayCanvas(menu.HeroSelectMagicPanel);

        //yield return ChooseHero(); //choose hero to cast spell on
        chooseHero = StartCoroutine(ChooseHero());

        yield return chooseHero;

        magicScript = new BaseMagicScript();
        magicScript.spell = magicUsed; //sets the spell to be cast in magic script
        magicScript.heroPerformingAction = heroFromMenu; //sets the casting hero in magic script
        magicScript.heroReceivingAction = heroToCastOn; //sets the receiving hero in magic script
        magicScript.ProcessMagicHeroToHero(false); //processes the spell
        UpdateUI(); //updates interface
        magicUsed = null; //sets magicUsed to null so it isn't used again next time
        magicScript = null; //sets magicScript to null so it can't be used again next time
        heroToCastOn = null; //sets hero to be cast on to null so they arent cast on again next time
    }

    public IEnumerator ChooseHero() //chooses hero based on which hero panel is clicked
    {
        menu.PlaySE(AM.confirmSE);

        DrawHeroSelectPanel();
        yield return menu.AnimateMagicHeroSelectPanel();

        menu.choosingHeroForMagicMenu = true;
        while (heroToCastOn == null)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                StopCoroutine(processMagic);
                StopCoroutine(chooseHero);
            }

            GetHeroClicked();
            yield return null;
        }

        if (heroToCastOn != null)
        {
            menu.PlaySE(AM.healSE);
        }
        
        yield return menu.AnimateMagicHeroSelectPanel();

        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero1SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero2SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero3SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero4SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero5SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();

        menu.HideCanvas(menu.HeroSelectMagicPanel);

        menu.choosingHeroForMagicMenu = false;
    }

    void DrawHeroSelectPanel() //shows hero panels to be selected to cast spell on
    {
        menu.DisplayCanvas(menu.HeroSelectMagicPanel);
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroSelectPanels(heroCount);

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            GameObject.Find("Hero" + (i + 1) + "SelectMagicPanel").transform.Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            menu.DrawHeroFace(GameManager.instance.activeHeroes[i], GameObject.Find("Hero" + (i + 1) + "SelectMagicPanel").transform.Find("FacePanel").GetComponent<Image>());
            GameObject.Find("Hero" + (i + 1) + "SelectMagicPanel").transform.Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("Hero" + (i + 1) + "SelectMagicPanel").transform.Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].finalMaxHP); //HP text
            GameObject.Find("Hero" + (i + 1) + "SelectMagicPanel").transform.Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].finalMaxMP); //MP text
        }
    }

    void DrawHeroSelectPanels(int count) //draws hero panels based on how many active heroes
    {
        if (count == 1)
        {
            menu.Hero1SelectMagicPanel.SetActive(true);
            menu.Hero2SelectMagicPanel.SetActive(false);
            menu.Hero3SelectMagicPanel.SetActive(false);
            menu.Hero4SelectMagicPanel.SetActive(false);
            menu.Hero5SelectMagicPanel.SetActive(false);
        }
        else if (count == 2)
        {
            menu.Hero1SelectMagicPanel.SetActive(true);
            menu.Hero2SelectMagicPanel.SetActive(true);
            menu.Hero3SelectMagicPanel.SetActive(false);
            menu.Hero4SelectMagicPanel.SetActive(false);
            menu.Hero5SelectMagicPanel.SetActive(false);
        }
        else if (count == 3)
        {
            menu.Hero1SelectMagicPanel.SetActive(true);
            menu.Hero2SelectMagicPanel.SetActive(true);
            menu.Hero3SelectMagicPanel.SetActive(true);
            menu.Hero4SelectMagicPanel.SetActive(false);
            menu.Hero5SelectMagicPanel.SetActive(false);
        }
        else if (count == 4)
        {
            menu.Hero1SelectMagicPanel.SetActive(true);
            menu.Hero2SelectMagicPanel.SetActive(true);
            menu.Hero3SelectMagicPanel.SetActive(true);
            menu.Hero4SelectMagicPanel.SetActive(true);
            menu.Hero5SelectMagicPanel.SetActive(false);
        }
        else if (count == 5)
        {
            menu.Hero1SelectMagicPanel.SetActive(true);
            menu.Hero2SelectMagicPanel.SetActive(true);
            menu.Hero3SelectMagicPanel.SetActive(true);
            menu.Hero4SelectMagicPanel.SetActive(true);
            menu.Hero5SelectMagicPanel.SetActive(true);
        }

        DrawHeroPanelBars();
    }

    void DrawHeroPanelBars() //draws the magic menu HP/MP bars
    {
        if (GameManager.instance.activeHeroes.Count == 1)
        {
            menu.Hero1MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero1MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 2)
        {
            menu.Hero1MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero1MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero2MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), menu.Hero2MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero2MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), menu.Hero2MagicMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 3)
        {
            menu.Hero1MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero1MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero2MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), menu.Hero2MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero2MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), menu.Hero2MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero3MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), menu.Hero3MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero3MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), menu.Hero3MagicMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 4)
        {
            menu.Hero1MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero1MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero2MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), menu.Hero2MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero2MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), menu.Hero2MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero3MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), menu.Hero3MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero3MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), menu.Hero3MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero4MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), menu.Hero4MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero4MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), menu.Hero4MagicMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 5)
        {
            menu.Hero1MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero1MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), menu.Hero1MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero2MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), menu.Hero2MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero2MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), menu.Hero2MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero3MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), menu.Hero3MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero3MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), menu.Hero3MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero4MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), menu.Hero4MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero4MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), menu.Hero4MagicMenuMPProgressBar.transform.localScale.y);

            menu.Hero5MagicMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[4]), 0, 1), menu.Hero5MagicMenuHPProgressBar.transform.localScale.y);
            menu.Hero5MagicMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[4]), 0, 1), menu.Hero5MagicMenuMPProgressBar.transform.localScale.y);
        }

    }

    void GetHeroClicked() //gets hero based on which hero panel is clicked
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            List<RaycastResult> results = GetEventSystemRaycastResults();

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "Hero1SelectMagicPanel")
                {
                    heroToCastOn = GameManager.instance.activeHeroes[0];
                }
                if (result.gameObject.name == "Hero2SelectMagicPanel")
                {
                    heroToCastOn = GameManager.instance.activeHeroes[1];
                }
                if (result.gameObject.name == "Hero3SelectMagicPanel")
                {
                    heroToCastOn = GameManager.instance.activeHeroes[2];
                }
                if (result.gameObject.name == "Hero4SelectMagicPanel")
                {
                    heroToCastOn = GameManager.instance.activeHeroes[3];
                }
                if (result.gameObject.name == "Hero5SelectMagicPanel")
                {
                    heroToCastOn = GameManager.instance.activeHeroes[4];
                }
            }
        }
    }

    void UpdateUI() //updates interface
    {
        menu.DrawMagicMenuStats();
        foreach (Transform child in GameObject.Find("WhiteMagicListPanel/WhiteMagicScroller/WhiteMagicListSpacer").transform)
        {
            if (heroFromMenu.curMP < GetMagic(child.gameObject.GetComponentInChildren<Text>().text).MPCost)
            {
                child.gameObject.GetComponentInChildren<Text>().color = Color.gray;
                GetMagic(child.gameObject.GetComponentInChildren<Text>().text).enoughMP = false;
            }
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
}
