using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemShopMouseEvents : MonoBehaviour //for displaying item details in shop
{
    Text itemDesc;
    Text ownedText;
    GameMenu menu;
    
    private void Start()
    {
        itemDesc = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>();
        ownedText = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/AdditionalDetailsPanel/OwnedText").GetComponent<Text>();
        menu = GameObject.Find("GameManager").GetComponent<GameMenu>();
        itemDesc.text = "";
    }

    public void ShowItemDetails()
    {
        //itemDesc.text = GetItem(GetItemName()).description;

        //Debug.Log(gameObject.name);

        if (gameObject.name.Contains("ShopBuyItemPanel") && !GameManager.instance.inConfirmation)
        {
            itemDesc.text = GetItem(gameObject.transform.Find("BuyShopItemNameText").GetComponent<Text>().text).description;
            ownedText.text = GetItemCount(gameObject.transform.Find("BuyShopItemNameText").GetComponent<Text>().text).ToString();
        }

        if (gameObject.name.Contains("ShopSellItemPanel") && !GameManager.instance.inConfirmation)
        {
            itemDesc.text = GetItem(gameObject.transform.Find("SellShopItemNameText").GetComponent<Text>().text).description;
        }
    }

    public void ProcessShop()
    {
        if (gameObject.name.Contains("ShopBuyItemPanel") && !GameManager.instance.inConfirmation)
        {
            if (CanMakeTransaction())
            {
                GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
                GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = GameManager.instance.itemShopCost.ToString();
                GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = (GameManager.instance.gold - int.Parse(GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text)).ToString();

                DisplayBuyConfirmationPanel();
                GameManager.instance.inConfirmation = true;
            }
        }

        if (gameObject.name.Contains("ShopSellItemPanel") && !GameManager.instance.inConfirmation)
        {
            //show 'How Many' window with 'sell' button that finalizes transaction
            GameManager.instance.itemShopItem = GetItem(gameObject.transform.Find("SellShopItemNameText").GetComponent<Text>().text);
            GameManager.instance.itemShopCost = GameManager.instance.itemShopItem.sellValue;

            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = GameManager.instance.itemShopCost.ToString();
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = (GameManager.instance.gold + int.Parse(GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text)).ToString();

            DisplaySellConfirmationPanel();
            GameManager.instance.inConfirmation = true;
        }

        if (gameObject.name == "ShopBuyItemPanel" && !GameManager.instance.inConfirmation)
        {
            DisplayItemShopBuyGUI();
        }

        if (gameObject.name == "ShopSellItemPanel" && !GameManager.instance.inConfirmation)
        {
            DisplayItemShopSellGUI();
        }
    }

    /*public void OnPointerClick(PointerEventData eventData) //begins item buy/sell process when clicked
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyShopItemNameText" && !GameManager.instance.inConfirmation)
        {
            //show 'How Many' window with 'buy' button that finalizes transaction
            GameManager.instance.itemShopItem = GetItem(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text);
            GameManager.instance.itemShopCost = GetItemCost(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text);

            if (CanMakeTransaction())
            {
                GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
                GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = GameManager.instance.itemShopCost.ToString();
                GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = (GameManager.instance.gold - int.Parse(GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text)).ToString();

                DisplayBuyConfirmationPanel();
                GameManager.instance.inConfirmation = true;
            }
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellShopItemNameText" && !GameManager.instance.inConfirmation)
        {
            //show 'How Many' window with 'sell' button that finalizes transaction
            GameManager.instance.itemShopItem = GetItem(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text);
            GameManager.instance.itemShopCost = GetItem(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text).sellValue;

            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = GameManager.instance.itemShopCost.ToString();
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = (GameManager.instance.gold + int.Parse(GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text)).ToString();

            DisplaySellConfirmationPanel();
            GameManager.instance.inConfirmation = true;
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyOptionButtonText" && !GameManager.instance.inConfirmation)
        {
            DisplayItemShopBuyGUI();
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellOptionButtonText" && !GameManager.instance.inConfirmation)
        {
            DisplayItemShopSellGUI();
        }
    }*/


    bool CanMakeTransaction()
    {
        if (GameManager.instance.itemShopCost > GameManager.instance.gold)
        {
            return false;
        }

        if (Inventory.instance.items.Contains(GameManager.instance.itemShopItem))
        {
            int count = 0;
            foreach (Item item in Inventory.instance.items)
            {
                if (item == GameManager.instance.itemShopItem)
                {
                    count++;
                }
            }
            if (count == 99)
            {
                return false;
            }
        }
        return true;
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
        //menu.DrawHeroItemMenuStats();
        //menu.ResetItemList();
        //menu.DrawItemList();
    }

    public Item GetItem(string name)
    {
        foreach (BaseItem item in ItemDB.instance.items)
        {
            if (name == item.item.name)
            {
                return item.item;
            }
        }

        return null;
    }

    public int GetItemCost(string name)
    {
        int cost = 0;

        foreach (BaseShopItem BSI in GameManager.instance.itemShopList)
        {
            if (BSI.item.name == name)
            {
                cost = BSI.cost;
                break;
            }
        }

        return cost;
    }

    int GetItemCount(string name)
    {
        int count = 0;
        foreach (Item item in Inventory.instance.items)
        {
            if (item.name == name)
            {
                count++;
            }
        }

        return count;
    }

    void UpdateGoldPanels()
    {
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();
    }

    void DisplayBuyConfirmationPanel()
    {
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    void DisplaySellConfirmationPanel()
    {
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

    public void ShowItemListInSellGUI()
    {
        //set and show gold
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();

        //generate item prefab for each item in item shop list
        ClearSellList();

        List<Item> itemsAccountedFor = new List<Item>();

        foreach (Item item in Inventory.instance.items)
        {
            if (item.GetType() == typeof(Item) && item.type != Item.Types.KEYITEM)
            {
                int itemCount = 0;
                if (!itemsAccountedFor.Contains(item))
                {
                    for (int i = 0; i < Inventory.instance.items.Count; i++)
                    {
                        if (Inventory.instance.items[i] == item)
                        {
                            itemCount++;
                        }
                    }

                    //NewItemPanel = Instantiate(NewItemPanel) as GameObject; //creates gameobject of newItemPanel
                    GameObject shopItemPanel = Instantiate(PrefabManager.Instance.shopSellItemPrefab);
                    shopItemPanel.transform.GetChild(0).GetComponent<Text>().text = item.name;
                    shopItemPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
                    shopItemPanel.transform.GetChild(2).GetComponent<Text>().text = itemCount.ToString();
                    shopItemPanel.transform.SetParent(GameObject.Find("GameManager/ShopCanvases").GetComponent<ShopObjectHolder>().shopItemSellListSpacer, false);

                    itemsAccountedFor.Add(item);
                }
            }
        }
    }

    void ClearSellList()
    {
        foreach (Transform child in GameObject.Find("GameManager/ShopCanvases").GetComponent<ShopObjectHolder>().shopItemSellListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    //--For mode switching--

    public void DisplayItemShopBuyGUI()
    {
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/BuyOptionButton/BuyOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/SellOptionButton/SellOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        UpdateGoldPanels();

        HideItemShopSellGUI();
    }

    public void HideItemShopBuyGUI()
    {
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void DisplayItemShopSellGUI()
    {
        ShowItemListInSellGUI();

        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/SellOptionButton/SellOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/BuyOptionButton/BuyOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        UpdateGoldPanels();

        HideItemShopBuyGUI();

    }

    public void HideItemShopSellGUI()
    {
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

    }
}
