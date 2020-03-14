﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    BattleStateMachine BSM;
    HeroStateMachine HSM;
    public Text detailsText;

    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        detailsText = GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
    }

    public void OnPointerClick(PointerEventData eventData) //sets target to hero if clicked
    {
        //if (BSM.choosingTarget)
        //{
            //BSM.target = this.gameObject;
        //}
    }

    public void OnPointerEnter(PointerEventData eventData) //shows border around hero panel in battle if hero is being hovered over
    {
        foreach (Transform child in GameObject.Find("BattleCanvas/HeroPanel/HeroPanelSpacer").transform)
        {
            if (child.Find("HeroName").GetComponent<Text>().text == this.GetComponent<HeroStateMachine>().hero._Name)
            {
                HSM = this.GetComponent<HeroStateMachine>();
                HSM.HeroPanel.transform.Find("BorderCanvas").GetComponent<CanvasGroup>().alpha = 1.0f;
            }
        }
        detailsText.GetComponent<Text>().text = this.gameObject.GetComponent<HeroStateMachine>().hero._Name;
    }

    public void OnPointerExit(PointerEventData eventData) //hides border around hero panel in battle
    {
        foreach (Transform child in GameObject.Find("BattleCanvas/HeroPanel/HeroPanelSpacer").transform)
        {
            if (child.Find("HeroName").GetComponent<Text>().text == this.GetComponent<HeroStateMachine>().hero._Name)
            {
                HSM = this.GetComponent<HeroStateMachine>();
                HSM.HeroPanel.transform.Find("BorderCanvas").GetComponent<CanvasGroup>().alpha = 0.0f;
            }
        }
        detailsText.GetComponent<Text>().text = "";
    }
}
