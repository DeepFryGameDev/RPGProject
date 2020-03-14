using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectButton : MonoBehaviour
{
    public GameObject HeroPrefab;
    public Text detailsText;

    public void Start()
    {
        detailsText = GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
    }

    public void SelectHero()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().HeroSelection(HeroPrefab); //Save input of enemy selection to enemy prefab
    }

    public void HideSelector() //hides selector cursor over enemy
    {
        HeroPrefab.transform.Find("Selector").gameObject.SetActive(false);
    }

    public void ShowSelector() //shows selector cursor over enemy
    {
        HeroPrefab.transform.Find("Selector").gameObject.SetActive(true);
    }

    public void HideDetails() //hides selector cursor over enemy
    {
        detailsText.text = "";
    }

    public void ShowDetails() //shows selector cursor over enemy
    {
        string heroName = this.gameObject.GetComponentInChildren<Text>().text;
        foreach (GameObject hero in GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().HeroesInBattle)
        {
            if (heroName == hero.GetComponent<HeroStateMachine>().hero._Name)
            {
                detailsText.GetComponent<Text>().text = heroName;
                break;
            }
        }
    }
}
