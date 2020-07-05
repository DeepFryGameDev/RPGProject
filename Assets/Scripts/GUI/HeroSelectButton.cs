using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectButton : MonoBehaviour
{
    //Facilitates mouse cursor interactions with hero select panel in battle - currently not using

    public GameObject HeroPrefab;
    public Text detailsText;

    public void Start()
    {
        detailsText = GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
    }

    /// <summary>
    /// Saves input of hero selection to hero prefab
    /// </summary>
    public void SelectHero()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().HeroSelection(HeroPrefab);
    }

    /// <summary>
    /// Hides selector cursor over hero
    /// </summary>
    public void HideSelector()
    {
        HeroPrefab.transform.Find("Selector").gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows selector cursor over hero
    /// </summary>
    public void ShowSelector()
    {
        HeroPrefab.transform.Find("Selector").gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides hero name on details text
    /// </summary>
    public void HideDetails()
    {
        detailsText.text = "";
    }

    /// <summary>
    /// Shows hero name on details text
    /// </summary>
    public void ShowDetails()
    {
        string heroName = this.gameObject.GetComponentInChildren<Text>().text;
        foreach (GameObject hero in GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().HeroesInBattle)
        {
            if (heroName == hero.GetComponent<HeroStateMachine>().hero.name)
            {
                detailsText.GetComponent<Text>().text = heroName;
                break;
            }
        }
    }
}
