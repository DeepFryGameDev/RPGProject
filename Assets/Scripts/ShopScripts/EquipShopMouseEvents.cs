using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipShopMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler //for displaying equip details in shop
{
    Text itemDesc;
    Text ownedText;
    GameMenu menu;
    
    private void Start()
    {
        itemDesc = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>();
        ownedText = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/AdditionalDetailsPanel/OwnedText").GetComponent<Text>();
        menu = GameObject.Find("GameManager").GetComponent<GameMenu>();
        itemDesc.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData) //sets item description panel with item's description
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyShopEquipNameText" && !GameManager.instance.inConfirmation)
        {
            itemDesc.text = GetEquip(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text).description;
            ownedText.text = GetEquipCount(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text).ToString();
            UpdateStatPanels(GetEquip(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text));
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellShopEquipNameText" && !GameManager.instance.inConfirmation)
        {
            itemDesc.text = GetEquip(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text).description;
            UpdateStatPanels(GetEquip(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text));
        }
    }

    public void OnPointerExit(PointerEventData eventData) //clears item description panel
    {
        if (!GameManager.instance.inConfirmation)
        {
            itemDesc.text = "";
            ownedText.text = "-";
            ClearEquipStatDetails();
        }
    }

    public void OnPointerClick(PointerEventData eventData) //begins item buy/sell process when clicked
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyShopEquipNameText" && !GameManager.instance.inConfirmation)
        {
            //show 'How Many' window with 'buy' button that finalizes transaction
            GameManager.instance.equipShopItem = GetEquip(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text);
            GameManager.instance.equipShopCost = GetEquipCost(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text);

            if (CanMakeTransaction())
            {
                GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
                GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = GameManager.instance.equipShopCost.ToString();
                GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = (GameManager.instance.gold - int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text)).ToString();

                DisplayBuyConfirmationPanel();
                GameManager.instance.inConfirmation = true;
            }
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellShopEquipNameText" && !GameManager.instance.inConfirmation)
        {
            //show 'How Many' window with 'sell' button that finalizes transaction
            GameManager.instance.equipShopItem = GetEquip(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text);
            GameManager.instance.equipShopCost = GetEquip(eventData.pointerCurrentRaycast.gameObject.GetComponent<Text>().text).sellValue;

            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/QuantityText").GetComponent<Text>().text = "1";
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text = GameManager.instance.equipShopCost.ToString();
            GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/RemainingText").GetComponent<Text>().text = (GameManager.instance.gold + int.Parse(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/TotalGoldText").GetComponent<Text>().text)).ToString();

            DisplaySellConfirmationPanel();
            GameManager.instance.inConfirmation = true;
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "BuyOptionButtonText" && !GameManager.instance.inConfirmation)
        {
            DisplayEquipShopBuyGUI();
        }

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "SellOptionButtonText" && !GameManager.instance.inConfirmation)
        {
            DisplayEquipShopSellGUI();
        }
    }


    bool CanMakeTransaction()
    {
        bool canBuy = true;

        if (GameManager.instance.equipShopCost > GameManager.instance.gold)
        {
            canBuy = false;
        }

        return canBuy;
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

    Equipment GetEquip(string name)
    {
        Equipment equip = null;

        foreach (BaseShopEquipment BSE in GameManager.instance.equipShopList)
        {
            if (BSE.equipment.name == name)
            {
                equip = BSE.equipment;
                break;
            }
        }

        return equip;
    }

    int GetEquipCost(string name)
    {
        int cost = 0;

        foreach (BaseShopEquipment BSE in GameManager.instance.equipShopList)
        {
            if (BSE.equipment.name == name)
            {
                cost = BSE.cost;
                break;
            }
        }

        return cost;
    }

    int GetEquipCount(string name)
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
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();
    }

    void DisplayBuyConfirmationPanel()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    void DisplaySellConfirmationPanel()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

    public void ShowItemListInSellGUI()
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
    }

    void ClearSellList()
    {
        foreach (Transform child in GameObject.Find("GameManager/Shops").GetComponent<ShopObjectHolder>().shopEquipSellListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    //--For mode switching--

    public void DisplayEquipShopBuyGUI()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/BuyOptionButton/BuyOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/SellOptionButton/SellOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        UpdateGoldPanels();

        HideEquipShopSellGUI();
    }

    public void HideEquipShopBuyGUI()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void DisplayEquipShopSellGUI()
    {
        ShowItemListInSellGUI();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/SellOptionButton/SellOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/BuyOptionButton/BuyOptionButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        UpdateGoldPanels();

        HideEquipShopBuyGUI();

    }

    public void HideEquipShopSellGUI()
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void UpdateStatPanels(Equipment equip)
    {
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/StrengthText").GetComponent<Text>().text = equip.Strength.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/StrengthText").GetComponent<Text>().text = equip.Strength.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/StaminaText").GetComponent<Text>().text = equip.Stamina.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/StaminaText").GetComponent<Text>().text = equip.Stamina.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/AgilityText").GetComponent<Text>().text = equip.Agility.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/AgilityText").GetComponent<Text>().text = equip.Agility.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/DexterityText").GetComponent<Text>().text = equip.Dexterity.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/DexterityText").GetComponent<Text>().text = equip.Dexterity.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/IntelligenceText").GetComponent<Text>().text = equip.Intelligence.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/IntelligenceText").GetComponent<Text>().text = equip.Intelligence.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/SpiritText").GetComponent<Text>().text = equip.Spirit.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/SpiritText").GetComponent<Text>().text = equip.Spirit.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/AttackText").GetComponent<Text>().text = equip.ATK.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/AttackText").GetComponent<Text>().text = equip.ATK.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/DefenseText").GetComponent<Text>().text = equip.DEF.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/DefenseText").GetComponent<Text>().text = equip.DEF.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/MagicAttackText").GetComponent<Text>().text = equip.MATK.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/MagicAttackText").GetComponent<Text>().text = equip.MATK.ToString();

        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipDetailsPanel/MagicDefenseText").GetComponent<Text>().text = equip.MDEF.ToString();
        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipDetailsPanel/MagicDefenseText").GetComponent<Text>().text = equip.MDEF.ToString();
    }


    void ClearEquipStatDetails()
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
