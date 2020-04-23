using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemShopConfirmationMouseEvents : MonoBehaviour, IPointerClickHandler //for displaying item details in shop
{
    public void OnPointerClick(PointerEventData eventData) //begins item buy/sell process when clicked
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyQuantityUpButton")
        {
            IncreaseQuantity("Buy");
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyQuantityDownButton")
        {
            DecreaseQuantity("Buy");
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyCancelButtonText")
        {
            CancelPurchase();
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyButtonText")
        {
            ConfirmPurchase();
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellQuantityUpButton")
        {
            IncreaseQuantity("Sell");
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellQuantityDownButton")
        {
            DecreaseQuantity("Sell");
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellCancelButtonText")
        {
            CancelSell();
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellButtonText")
        {
            ConfirmSell();
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

    void IncreaseQuantity(string mode)
    {
        int itemCount = 0;
        foreach (Item item in Inventory.instance.items)
        {
            if (item.name == GameManager.instance.itemShopItem.name)
            {
                itemCount++;
            }
        }

        if (mode == "Buy")
        {
            int quantity = int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text);

            if (quantity < (99 - itemCount))
            {
                quantity++;
            }

            if (HasEnoughGold(quantity))
            {
                GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = quantity.ToString();

                UpdateCost(quantity);

                UpdateGoldRemaining("Buy");
            }

        } else if (mode == "Sell")
        {

            int quantity = int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text);

            if (quantity < itemCount)
            {
                quantity++;
            }

            GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = quantity.ToString();

            UpdateSale(quantity);

            UpdateGoldRemaining("Sell");
        }        
    }

    void DecreaseQuantity(string mode)
    {
        if (mode == "Buy")
        {
            int quantity = int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text);

            if (quantity > 1)
            {
                quantity--;

                GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = quantity.ToString();
            }

            UpdateCost(quantity);

            UpdateGoldRemaining("Buy");
        }
        else if (mode == "Sell")
        {
            int quantity = int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text);

            if (quantity > 1)
            {
                quantity--;

                GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = quantity.ToString();
            }

            UpdateSale(quantity);

            UpdateGoldRemaining("Sell");
        }
    }

    bool HasEnoughGold(int quantity)
    {
        bool hasEnoughGold = true;
        
        int total = quantity * GameManager.instance.itemShopCost;
        
        if (total > GameManager.instance.gold)
        {
            hasEnoughGold = false;
        }

        return hasEnoughGold;
    }

    void UpdateCost(int quantity)
    {
        int total = (quantity * GameManager.instance.itemShopCost);
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = total.ToString();
    }

    void UpdateSale(int quantity)
    {
        int total = (quantity * GameManager.instance.itemShopItem.sellValue);
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = total.ToString();
    }

    void UpdateGoldRemaining(string mode)
    {
        int totalGold = GameManager.instance.gold;
        if (mode == "Buy")
        {
            int remaining = totalGold - int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text);

            GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = remaining.ToString();
        } else if (mode == "Sell")
        {
            int remaining = totalGold + int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text);

            GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = remaining.ToString();
        }

    }

    void CancelPurchase()
    {
        HideBuyConfirmationPanel();
        GameManager.instance.inConfirmation = false;
    }

    void HideBuyConfirmationPanel()
    {
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = "00";
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = "00";

        GameManager.instance.inConfirmation = false;
    }

    void ConfirmPurchase()
    {
        if (HasEnoughGold(int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text)))
        {
            //play buy sound effect

            //subtract gold
            SubtractGold(int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text));

            //add items
            AddItems(GameManager.instance.itemShopItem, int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text));

            //hide confirmation panel
            HideBuyConfirmationPanel();

            //update shop panel with new values
            UpdateBuyPanel();
        }
    }

    void ConfirmSell()
    {
        //play buy sound effect

        //add gold
        AddGold(int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text));

        //remove items
        RemoveItems(GameManager.instance.itemShopItem, int.Parse(GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text));

        //hide confirmation panel
        HideSellConfirmationPanel();

        //update shop panel with new values
        UpdateSellPanel();
    }

    void CancelSell()
    {
        HideSellConfirmationPanel();
        GameManager.instance.inConfirmation = false;
    }

    void HideSellConfirmationPanel()
    {
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
                                                                  
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = "00";
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = "00";
        GameManager.instance.inConfirmation = false;
    }

    void UpdateBuyPanel()
    {
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();

        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopBuyPanel/AdditionalDetailsPanel/OwnedText").GetComponent<Text>().text = "-";
    }

    void UpdateSellPanel()
    {
        //set and show gold
        GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemShopSellPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();

        //generate item prefab for each item in item shop list
        ClearSellList();

        List<Item> itemsAccountedFor = new List<Item>();

        foreach (Item item in Inventory.instance.items)
        {
            if (item.GetType() == typeof(Item))
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
                    shopItemPanel.transform.SetParent(GameObject.Find("GameManager/Shops").GetComponent<ShopObjectHolder>().shopItemSellListSpacer, false);

                    itemsAccountedFor.Add(item);
                }
            }

            GameObject.Find("GameManager/Shops/ItemShopCanvas/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "";
        }
            
    }

    void ClearSellList()
    {
        foreach (Transform child in GameObject.Find("GameManager/Shops").GetComponent<ShopObjectHolder>().shopItemSellListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void SubtractGold(int goldToSubtract)
    {
        GameManager.instance.gold -= goldToSubtract;
    }

    void AddGold(int goldToAdd)
    {
        GameManager.instance.gold += goldToAdd;
    }

    void AddItems(Item itemToAdd, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Inventory.instance.Add(itemToAdd);
        }
    }

    void RemoveItems(Item itemToAdd, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Inventory.instance.Remove(itemToAdd);
        }
    }

}
