﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipShop : MonoBehaviour
{
    public List<BaseShopEquipment> equipShopList = new List<BaseShopEquipment>();

    bool inShop;

    Text goldText;
    Text numOwned;
    
    public void DisplayEquipShopGUI()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/BuyOptionButton/BuyOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/SellOptionButton/SellOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        DisablePlayerInput();

        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().PauseBackground(true);
        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = true;
    }

    public void HideEquipShopGUI()
    {
        if (!GameManager.instance.inConfirmation)
        {
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

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

            HideEquipShopGUI();
            inShop = false;
        }
    }

    public void ShowEquipListInBuyGUI()
    {
        inShop = true;

        //set and show gold
        goldText = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>();
        goldText.text = GameManager.instance.gold.ToString();

        //set and show number of items owned
        numOwned = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/AdditionalDetailsPanel/OwnedText").GetComponent<Text>();
        numOwned.text = "-";

        //generate item prefab for each item in item shop list
        foreach (BaseShopEquipment shopEquip in equipShopList)
        {
            GameObject shopItemPanel = Instantiate(PrefabManager.Instance.shopBuyEquipPrefab);
            shopItemPanel.transform.Find("BuyShopEquipNameText").GetComponent<Text>().text = shopEquip.equipment.name;
            shopItemPanel.transform.Find("BuyShopEquipIcon").GetComponent<Image>().sprite = shopEquip.equipment.icon;
            shopItemPanel.transform.Find("BuyShopEquipCostText").GetComponent<Text>().text = shopEquip.cost.ToString();
            shopItemPanel.transform.SetParent(GameObject.Find("GameManager/ShopCanvases").GetComponent<ShopObjectHolder>().shopEquipBuyListSpacer, false);
        }
    }

    void ClearBuyList()
    {
        foreach (Transform child in GameObject.Find("GameManager/ShopCanvases").GetComponent<ShopObjectHolder>().shopEquipBuyListSpacer.transform)
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
