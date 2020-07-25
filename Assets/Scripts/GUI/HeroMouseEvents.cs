using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    BattleStateMachine BSM;
    HeroStateMachine HSM;
    public Text detailsText;

    //likely not using

    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        detailsText = GameObject.Find("BattleCanvas/BattleUI/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
    }

    /// <summary>
    /// Sets chosenTarget of BSM to clicked hero
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (BSM.choosingTarget)
        {
            GameObject.Find("GameManager/BGS").GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.confirmSE);
            BSM.chosenTarget = this.gameObject; //sets BSM target to this gameObject
        }
    }

    /// <summary>
    /// Shows border around hero panel in battle if hero is being hovered over
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(GetHeroByID(int.Parse(gameObject.name.Replace("BattleHero - ID ", ""))).name);

        foreach (Transform child in GameObject.Find("BattleCanvas/HeroPanel/HeroPanelSpacer").transform)
        {
            if (child.Find("HeroName").GetComponent<Text>().text == this.GetComponent<HeroStateMachine>().hero.name)
            {
                HSM = this.GetComponent<HeroStateMachine>();
                HSM.HeroPanel.transform.Find("BorderCanvas").GetComponent<CanvasGroup>().alpha = 1.0f;
            }
        }
        detailsText.GetComponent<Text>().text = this.gameObject.GetComponent<HeroStateMachine>().hero.name;
    }

    /// <summary>
    /// Hides border around hero panel in battle
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Transform child in GameObject.Find("BattleCanvas/HeroPanel/HeroPanelSpacer").transform)
        {
            if (child.Find("HeroName").GetComponent<Text>().text == this.GetComponent<HeroStateMachine>().hero.name)
            {
                HSM = this.GetComponent<HeroStateMachine>();
                HSM.HeroPanel.transform.Find("BorderCanvas").GetComponent<CanvasGroup>().alpha = 0.0f;
            }
        }
        detailsText.GetComponent<Text>().text = "";
    }

    /// <summary>
    /// Returns the hero by given ID based on HeroDB
    /// </summary>
    /// <param name="ID">ID of hero needing to be returned</param>
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
}
