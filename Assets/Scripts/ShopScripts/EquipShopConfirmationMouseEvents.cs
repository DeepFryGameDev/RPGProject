using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipShopConfirmationMouseEvents : MonoBehaviour, IPointerClickHandler //for displaying equip details in shop
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
            if (item.name == GameManager.instance.equipShopItem.name)
            {
                itemCount++;
            }
        }

        if (mode == "Buy")
        {
            int quantity = int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text);

            if (quantity < (99 - itemCount))
            {
                quantity++;
            }

            if (HasEnoughGold(quantity))
            {
                GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = quantity.ToString();

                UpdateCost(quantity);

                UpdateGoldRemaining("Buy");
            }

        } else if (mode == "Sell")
        {

            int quantity = int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text);

            if (quantity < itemCount)
            {
                quantity++;
            }

            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = quantity.ToString();

            UpdateSale(quantity);

            UpdateGoldRemaining("Sell");
        }        
    }

    void DecreaseQuantity(string mode)
    {
        if (mode == "Buy")
        {
            int quantity = int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text);

            if (quantity > 1)
            {
                quantity--;

                GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = quantity.ToString();
            }

            UpdateCost(quantity);

            UpdateGoldRemaining("Buy");
        }
        else if (mode == "Sell")
        {
            int quantity = int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text);

            if (quantity > 1)
            {
                quantity--;

                GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = quantity.ToString();
            }

            UpdateSale(quantity);

            UpdateGoldRemaining("Sell");
        }
    }

    bool HasEnoughGold(int quantity)
    {
        bool hasEnoughGold = true;
        
        int total = quantity * GameManager.instance.equipShopCost;
        
        if (total > GameManager.instance.gold)
        {
            hasEnoughGold = false;
        }

        return hasEnoughGold;
    }

    void UpdateCost(int quantity)
    {
        int total = (quantity * GameManager.instance.equipShopCost);
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = total.ToString();
    }

    void UpdateSale(int quantity)
    {
        int total = (quantity * GameManager.instance.equipShopItem.sellValue);
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = total.ToString();
    }

    void UpdateGoldRemaining(string mode)
    {
        int totalGold = GameManager.instance.gold;
        if (mode == "Buy")
        {
            int remaining = totalGold - int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text);

            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = remaining.ToString();
        } else if (mode == "Sell")
        {
            int remaining = totalGold + int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text);

            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = remaining.ToString();
        }

    }

    void CancelPurchase()
    {
        HideBuyConfirmationPanel();
        GameManager.instance.inConfirmation = false;
    }

    void HideBuyConfirmationPanel()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = "00";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = "00";

        GameManager.instance.inConfirmation = false;
    }

    void ConfirmPurchase()
    {
        if (HasEnoughGold(int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text)))
        {
            //play buy sound effect

            //subtract gold
            SubtractGold(int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text));

            //add items
            AddItems(GameManager.instance.equipShopItem, int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text));

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
        AddGold(int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text));

        //remove items
        RemoveItems(GameManager.instance.equipShopItem, int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text));

        //Reset stat panel
        ResetEquipStatDetails();

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
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
                                                                  
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = "00";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = "00";
        GameManager.instance.inConfirmation = false;
    }

    void UpdateBuyPanel()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/AdditionalDetailsPanel/OwnedText").GetComponent<Text>().text = "-";
    }

    void UpdateSellPanel()
    {
        //set and show gold
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();

        //generate item prefab for each item in item shop list
        ClearSellList();

        List<Item> itemsAccountedFor = new List<Item>();

        foreach (Item item in Inventory.instance.items)
        {
            if (item.GetType() == typeof(Equipment))
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
                    GameObject shopItemPanel = Instantiate(PrefabManager.Instance.shopSellEquipPrefab);
                    shopItemPanel.transform.GetChild(0).GetComponent<Text>().text = item.name;
                    shopItemPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
                    shopItemPanel.transform.GetChild(2).GetComponent<Text>().text = itemCount.ToString();
                    shopItemPanel.transform.SetParent(GameObject.Find("GameManager/Shops").GetComponent<ShopObjectHolder>().shopEquipSellListSpacer, false);

                    itemsAccountedFor.Add(item);
                }
            }
        }

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>().text = "";

        itemsAccountedFor.Clear();
    }

    void ClearSellList()
    {
        foreach (Transform child in GameObject.Find("GameManager/Shops").GetComponent<ShopObjectHolder>().shopEquipSellListSpacer.transform)
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

    void ResetEquipStatDetails()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/StrengthText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/StrengthText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/StaminaText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/StaminaText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/AgilityText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/AgilityText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/DexterityText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/DexterityText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/IntelligenceText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/IntelligenceText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/SpiritText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/SpiritText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/AttackText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/AttackText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/DefenseText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/DefenseText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/MagicAttackText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/MagicAttackText").GetComponent<Text>().text = "0";

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/MagicDefenseText").GetComponent<Text>().text = "0";
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/MagicDefenseText").GetComponent<Text>().text = "0";
    }

}
