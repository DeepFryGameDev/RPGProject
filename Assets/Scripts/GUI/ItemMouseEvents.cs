using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler //for displaying item details in menus
{
    //Facilitates mouse cursor interaction with item objects instantiated in item menu
    Text itemDesc;
    BaseHero hero;
    BaseItemScript itemScript = new BaseItemScript();
    GameMenu menu;
    Item itemUsed;

    private void Start()
    {
        itemDesc = GameObject.Find("ItemMenuCanvas/ItemMenuPanel/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>();
        menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
    }

    /// <summary>
    /// Returns item by given ID
    /// </summary>
    /// <param name="ID">Given ID to return item</param>
    Item GetItem(int ID)
    {
        foreach (BaseItem item in ItemDB.instance.items)
        {
            if (item.ID == ID)
            { 
                return item.item;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns equipment by given ID
    /// </summary>
    /// <param name="ID">Given ID to return equipment</param>
    Item GetEquipment(int ID)
    {
        foreach (BaseEquipment equip in EquipmentDB.instance.equipment)
        {
            if (equip.ID == ID)
            {
                return equip.equipment;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns item ID from the attached GameObject name
    /// </summary>
    int GetItemID()
    {
        foreach (BaseItem item in ItemDB.instance.items)
        {
            if (gameObject.transform.Find("NewItemNameText").GetComponent<Text>().text == item.name)
            {
                return item.ID;
            }
        }
        return 0;
    }

    /// <summary>
    /// Returns equip ID from the attached GameObject name
    /// </summary>
    int GetEquipID()
    {
        foreach (BaseEquipment equip in EquipmentDB.instance.equipment)
        {
            if (gameObject.transform.Find("NewItemNameText").GetComponent<Text>().text == equip.name)
            {
                return equip.ID;
            }
        }
        return 0;
    }

    /// <summary>
    /// Returns item type (item or equipment) from attached GameObject
    /// </summary>
    string GetItemType()
    {
        foreach (BaseEquipment equip in EquipmentDB.instance.equipment)
        {
            if (gameObject.transform.Find("NewItemNameText").GetComponent<Text>().text == equip.name)
            {
                return "equip";
            }
        }
        return "item";
    }

    /// <summary>
    /// Sets item description panel text to the attached item description
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetItemType() == "equip")
        {
            itemDesc.text = GetEquipment(GetEquipID()).description;
        } else
        {
            itemDesc.text = GetItem(GetItemID()).description;
        }

    }

    /// <summary>
    /// Clears item description text from panel
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        itemDesc.text = "";
    }

    /// <summary>
    /// Begins item processing when item object in item menu is clicked, or helps swap items if item sort customize mode is on
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetItemType() == "item")
        {
            if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().itemCustomizeModeOn)
            {
                if (GetItem(GetItemID()).usableInMenu)
                {
                    if (GetItem(GetItemID()).useState == Item.UseStates.HERO)
                    {
                        UnboldItems();

                        menu.PlaySE(menu.confirmSE);

                        gameObject.transform.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
                        menu.itemChoosingHero = true;

                        itemUsed = GetItem(GetItemID());
                        StartCoroutine(ProcessItem());
                    }
                }
                else
                {
                    menu.PlaySE(menu.cantActionSE);
                }
            }
            else //itemCustomizeModeOn is true
            {
                if (!menu.itemIndexSwapAPicked)
                {
                    menu.PlaySE(menu.confirmSE);
                    menu.itemIndexSwapA = Inventory.instance.items.IndexOf(GetItem(GetItemID()));
                    gameObject.transform.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;

                    menu.itemIndexSwapAPicked = true;
                }
                else
                {
                    menu.PlaySE(menu.confirmSE);
                    menu.itemIndexSwapB = Inventory.instance.items.IndexOf(GetItem(GetItemID()));
                    SwapItemsInList();
                    menu.DrawItemList();
                }
            }
        }

        if (GetItemType() == "equip")
        {
            if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().itemCustomizeModeOn)
            {
                if (GetEquipment(GetEquipID()).usableInMenu)
                {
                    if (GetEquipment(GetEquipID()).useState == Item.UseStates.HERO)
                    {
                        UnboldItems();

                        menu.PlaySE(menu.confirmSE);

                        gameObject.transform.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
                        menu.itemChoosingHero = true;

                        itemUsed = GetEquipment(GetEquipID());
                        StartCoroutine(ProcessItem());
                    }
                }
                else
                {
                    menu.PlaySE(menu.cantActionSE);
                }
            }
            else //itemCustomizeModeOn is true
            {
                if (!menu.itemIndexSwapAPicked)
                {
                    menu.PlaySE(menu.confirmSE);
                    menu.itemIndexSwapA = Inventory.instance.items.IndexOf(GetEquipment(GetEquipID()));
                    gameObject.transform.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;

                    menu.itemIndexSwapAPicked = true;
                }
                else
                {
                    menu.PlaySE(menu.confirmSE);
                    menu.itemIndexSwapB = Inventory.instance.items.IndexOf(GetEquipment(GetEquipID()));
                    SwapItemsInList();
                    menu.DrawItemList();
                }
            }
        }
    }

    /// <summary>
    /// Swaps positions of items in inventory using items set as itemIndexSwapA and itemIndexSwapB
    /// </summary>
    public void SwapItemsInList()
    {
        Item tmp = Inventory.instance.items[menu.itemIndexSwapA];
        Inventory.instance.items[menu.itemIndexSwapA] = Inventory.instance.items[menu.itemIndexSwapB];
        Inventory.instance.items[menu.itemIndexSwapB] = tmp;

        menu.itemIndexSwapAPicked = false;
        menu.itemIndexSwapA = 0;
        menu.itemIndexSwapB = 0;

        UnboldItems();
    }

    /// <summary>
    /// Unbolds all item object texts in item list
    /// </summary>
    void UnboldItems()
    {
        foreach (Transform child in GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform)
        {
            child.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
    }

    /// <summary>
    /// Gets all objects being clicked
    /// </summary>
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    /// <summary>
    /// Processes item use on chosen hero
    /// </summary>
    public IEnumerator ProcessItem()
    {
        yield return ChooseHero(); //choose hero to use item on
        if (menu.itemChoosingHero)
        {
            menu.PlaySE(menu.healSE);
            itemScript.scriptToRun = gameObject.transform.Find("NewItemNameText").GetComponent<Text>().text; //sets item to be used
            itemScript.ProcessItemToHero(hero); //processes the item to selected hero
            RemoveItemFromInventory(itemUsed); //removes the item from inventory
            itemUsed = null; //sets itemUsed to null so it isnt the same item used again next time
            hero = null; //sets hero to null so it isnt the same hero item is used on next time
            menu.itemChoosingHero = false;
            UnboldItems();
            UpdateUI(); //updates interface after item is cast
        } else
        {
            itemUsed = null; //sets itemUsed to null so it isnt the same item used again next time
            hero = null; //sets hero to null so it isnt the same hero item is used on next time
            menu.itemChoosingHero = false;
            StopCoroutine(ProcessItem());
        }
    }

    /// <summary>
    /// Removes given item from inventory
    /// </summary>
    /// <param name="item">Provided item to be removed from inventory</param>
    void RemoveItemFromInventory(Item item)
    {
        Inventory.instance.Remove(item);
    }

    /// <summary>
    /// Coroutine.  Allows player to choose hero to apply item to hero panel clicked
    /// </summary>
    public IEnumerator ChooseHero()
    {
        Debug.Log("choose a hero");
        while (hero == null)
        {
            GetHeroClicked();
            yield return null;
        }
    }

    /// <summary>
    /// Sets hero based on which hero item panel is clicked
    /// </summary>
    void GetHeroClicked()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            List<RaycastResult> results = GetEventSystemRaycastResults();

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "Hero1ItemPanel")
                {
                    hero = GameManager.instance.activeHeroes[0];
                }
                if (result.gameObject.name == "Hero2ItemPanel")
                {
                    hero = GameManager.instance.activeHeroes[1];
                }
                if (result.gameObject.name == "Hero3ItemPanel")
                {
                    hero = GameManager.instance.activeHeroes[2];
                }
                if (result.gameObject.name == "Hero4ItemPanel")
                {
                    hero = GameManager.instance.activeHeroes[3];
                }
                if (result.gameObject.name == "Hero5ItemPanel")
                {
                    hero = GameManager.instance.activeHeroes[4];
                }
            }
        }
    }

    /// <summary>
    /// Updates interface with newly updated item list
    /// </summary>
    void UpdateUI()
    {
        menu.DrawHeroItemMenuStats();
        menu.ResetItemList();
        menu.DrawItemList();
    }
}
