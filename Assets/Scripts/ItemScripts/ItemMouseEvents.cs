using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler //for displaying item details in menus
{

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

    Item GetItem(string name) //get item from name
    {
        foreach (Item item in Inventory.instance.items)
        {
            if (item.name == name)
            {
                return item;
            }
        }
        return null;
    }

    string GetItemName() //gets item name from the item name text
    {
        return gameObject.transform.Find("NewItemNameText").GetComponent<Text>().text;
    }

    public void OnPointerEnter(PointerEventData eventData) //sets item description panel with item's description
    {
        itemDesc.text = GetItem(GetItemName()).description;
    }

    public void OnPointerExit(PointerEventData eventData) //clears item description panel
    {
        itemDesc.text = "";
    }

    public void OnPointerClick(PointerEventData eventData) //begins item process when clicked
    {
        if (!GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().itemCustomizeModeOn)
        {
            if (GetItem(GetItemName()).usableInMenu)
            {
                if (GetItem(GetItemName()).useState == Item.UseStates.HERO)
                {
                    UnboldItems();
                    gameObject.transform.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
                    menu.itemChoosingHero = true;

                    itemUsed = GetItem(GetItemName());
                    StartCoroutine(ProcessItem());
                }
            }
        } else //itemCustomizeModeOn is true
        {
            if (!menu.itemIndexSwapAPicked)
            {
                menu.itemIndexSwapA = Inventory.instance.items.IndexOf(GetItem(GetItemName()));
                gameObject.transform.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Bold;

                menu.itemIndexSwapAPicked = true;
            } else
            {
                menu.itemIndexSwapB = Inventory.instance.items.IndexOf(GetItem(GetItemName()));
                SwapItemsInList();
                menu.DrawItemList();
            }
        }
    }

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

    void UnboldItems()
    {
        foreach (Transform child in GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform)
        {
            child.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
    }

    static List<RaycastResult> GetEventSystemRaycastResults() //gets all objects being clicked on
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public IEnumerator ProcessItem()
    {
        yield return ChooseHero(); //choose hero to use item on
        if (menu.itemChoosingHero)
        {
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

    void RemoveItemFromInventory(Item item)
    {
        Inventory.instance.Remove(item);
    }

    public IEnumerator ChooseHero() //gets hero to apply item to based on which hero panel is clicked
    {
        Debug.Log("choose a hero");
        while (hero == null)
        {
            GetHeroClicked();
            yield return null;
        }
    }

    void GetHeroClicked() //sets the hero based on which hero item panel is clicked
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

    void UpdateUI() //updates interface
    {
        menu.DrawHeroItemMenuStats();
        menu.ResetItemList();
        menu.DrawItemList();
    }
}
