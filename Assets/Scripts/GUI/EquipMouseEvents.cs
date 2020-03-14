using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler //for displaying item details in menus
{

    Text equipDesc;
    BaseHero hero;
    GameMenu menu;

    private void Start()
    {
        equipDesc = GameObject.Find("EquipMenuCanvas/EquipMenuPanel/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>();
        menu = GameObject.Find("GameManager").GetComponent<GameMenu>();
    }

    Equipment GetEquip(string name) //get item from name
    {
        foreach (Item equipment in Inventory.instance.items)
        {
            if (equipment.name == name)
            {
                return (Equipment)equipment;
            }
        }
        return null;
    }

    string GetEquipName() //gets item name from the item name text
    {
        return gameObject.transform.Find("NewEquipNameText").GetComponent<Text>().text;
    }

    public void OnPointerEnter(PointerEventData eventData) //sets item description panel with item's description
    {
        if (GetEquip(GetEquipName()) != null)
        {
            equipDesc.text = GetEquip(GetEquipName()).description;
            menu.ShowEquipmentStatUpdates(GetEquip(GetEquipName()));
        } else
        {
            menu.ShowEquipmentStatUpdates(null);
        }
    }

    public void OnPointerExit(PointerEventData eventData) //clears item description panel
    {
        equipDesc.text = "";
        menu.ResetEquipmentStatUpdates();
    }

    public void OnPointerClick(PointerEventData eventData) //begins item process when clicked
    {
        menu.ChangeEquipment(GetEquip(GetEquipName()));
    }

    static List<RaycastResult> GetEventSystemRaycastResults() //gets all objects being clicked on
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    void UpdateUI() //updates interface
    {

    }
}
