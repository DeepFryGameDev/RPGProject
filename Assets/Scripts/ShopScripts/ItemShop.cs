﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public List<BaseShopItem> itemShopList = new List<BaseShopItem>();

    bool inShop;

    Text goldText;
    Text numOwned;
    
    public void DisplayItemShopGUI()
    {
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/BuyOptionButton/BuyOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/SellOptionButton/SellOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        DisablePlayerInput();

        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().PauseBackground(true);
        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = true;
    }

    public void HideItemShopGUI()
    {
        if (!GameManager.instance.inConfirmation)
        {
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

            EnablePlayerInput();

            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().PauseBackground(false);
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = false;
        }
    }

    private void Update()
    {
        if (inShop && Input.GetButtonDown("Cancel"))
        {
            //clear items from list
            ClearBuyList();

            HideItemShopGUI();
            inShop = false;
        }
    }

    public void ShowItemListInBuyGUI()
    {
        inShop = true;

        //set and show gold
        goldText = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>();
        goldText.text = GameManager.instance.gold.ToString();

        //set and show number of items owned
        numOwned = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/AdditionalDetailsPanel/OwnedText").GetComponent<Text>();
        numOwned.text = "-";

        //generate item prefab for each item in item shop list
        foreach (BaseShopItem shopItem in itemShopList)
        {
            GameObject shopItemPanel = Instantiate(PrefabManager.Instance.shopBuyItemPrefab);
            shopItemPanel.transform.Find("BuyShopItemNameText").GetComponent<Text>().text = shopItem.item.name;
            shopItemPanel.transform.Find("BuyShopItemIcon").GetComponent<Image>().sprite = shopItem.item.icon;
            shopItemPanel.transform.Find("BuyShopItemCostText").GetComponent<Text>().text = shopItem.cost.ToString();
            shopItemPanel.transform.SetParent(GameObject.Find("GameManager/ShopCanvases").GetComponent<ShopObjectHolder>().shopItemBuyListSpacer, false);
        }
    }

    void ClearBuyList()
    {
        foreach (Transform child in GameObject.Find("GameManager/ShopCanvases").GetComponent<ShopObjectHolder>().shopItemBuyListSpacer.transform)
        {
            Destroy (child.gameObject);
        }
    }

    //-----------------------------

    void DisablePlayerInput()
    {
        GameObject.Find("Player").GetComponent<PlayerController2D>().enabled = false;
        GameObject.Find("Player").GetComponent<BoxCollider2D>().enabled = false;
    }

    void EnablePlayerInput()
    {
        GameObject.Find("Player").GetComponent<PlayerController2D>().enabled = true;
        GameObject.Find("Player").GetComponent<BoxCollider2D>().enabled = true;
    }
}
