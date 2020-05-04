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
        detailsText = GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
    }

    public void OnPointerClick(PointerEventData eventData) //sets target to hero if clicked
    {
        if (BSM.choosingTarget)
        {
            BSM.chosenTarget = this.gameObject; //sets BSM target to this gameObject
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //shows border around hero panel in battle if hero is being hovered over
    {
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

    public void OnPointerExit(PointerEventData eventData) //hides border around hero panel in battle
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
}
