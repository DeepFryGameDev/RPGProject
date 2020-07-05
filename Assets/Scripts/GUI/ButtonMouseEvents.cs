using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Used for combat action buttons interaction with mouse cursor

    BattleStateMachine BSM;
    HeroStateMachine HSM;
    public Text detailsText;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
            detailsText = GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
        }
    }

    /// <summary>
    /// Shows details about which action button is being hovered
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData) 
    {
        string actionName = this.gameObject.GetComponentInChildren<Text>().text;
        if (gameObject.name.Contains("ActionButton"))
        {
            if (actionName == "Attack")
            {
                detailsText.GetComponent<Text>().text = "Perform a physical attack";
            }
            if (actionName == "Magic")
            {
                detailsText.GetComponent<Text>().text = "Perform a magic attack";
            }
            if (actionName == "Item")
            {
                detailsText.GetComponent<Text>().text = "Use an item from your inventory";
            }
            if (actionName == "Action")
            {
                detailsText.GetComponent<Text>().text = "Perform an attack, cast magic, or use an item";
            }
            if (actionName == "Defend")
            {
                detailsText.GetComponent<Text>().text = "Take 50% damage until next turn";
            }
        }
        if (gameObject.name.Contains("MagicButton"))
        {
            string magicName = this.gameObject.GetComponentInChildren<Text>().text;
            
            for (int i = 0; i < GameManager.instance.activeHeroes.Count; i++)
            {
                foreach (BaseAttack magic in GameManager.instance.activeHeroes[i].MagicAttacks)
                {
                    if (magic.name == magicName)
                    {
                        detailsText.GetComponent<Text>().text = magic.description;
                        break;
                    }
                }
            }
        }
        if (gameObject.name.Contains("ItemButton"))
        {
            string itemName = this.gameObject.transform.Find("Text").GetComponent<Text>().text;

            foreach (Item item in Inventory.instance.items)
            {
                if (item.name == itemName)
                {
                    detailsText.GetComponent<Text>().text = item.description;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Clears details text when mouse cursor exits action button
    /// </summary>
    public void OnPointerExit(PointerEventData eventData) 
    {
        detailsText.GetComponent<Text>().text = "";
    }

    /// <summary>
    /// Clears details text when mouse cursor exits action button
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        detailsText.GetComponent<Text>().text = "";
    }
}
