using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipShop : MonoBehaviour
{
    public List<BaseShopEquipment> equipShopList = new List<BaseShopEquipment>();

    public bool inShop;

    Text goldText;
    Text numOwned;

    Transform cursor;

    EquipShopMouseEvents esme;
    GameObject esmeObj;

    public enum EquipShopCursorStates
    {
        ITEMLIST,
        ITEMCONFIRM,
        OPTIONS
    }
    public EquipShopCursorStates equipShopCursorState;
    public string shopMode;
    bool dpadPressed;
    bool confirmPressed;

    int cursorOnItem;
    int cursorOnOption;
    float itemSpacer = 24.5f;

    int tempScrollDiff = 0;

    EquipShopConfirmationMouseEvents confirmationQuantityUp;
    EquipShopConfirmationMouseEvents confirmationQuantityDown;
    EquipShopConfirmationMouseEvents confirmationConfirm;
    Color pressedColor = new Color(.4f, .4f, .4f);
    Color unpressedColor = new Color(1f, 1f, 1f);

    private void Start()
    {
        cursor = GameObject.Find("GameManager/Cursor").transform;

        //set and show gold
        goldText = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>();
        goldText.text = GameManager.instance.gold.ToString();

        //set and show number of items owned
        numOwned = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/AdditionalDetailsPanel/OwnedText").GetComponent<Text>();
        numOwned.text = "-";
    }

    private void Update()
    {
        if (inShop)
        {
            if (equipShopCursorState == EquipShopCursorStates.ITEMLIST)
            {
                if (shopMode == "Buy")
                {
                    int itemCount = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.childCount;
                    int scrollDiff = itemCount - 9;

                    if (itemCount <= 9)
                    {
                        tempScrollDiff = 0;
                    }

                    if (cursor.parent != GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").transform)
                    {
                        cursor.SetParent(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").transform);
                    }

                    if (cursor.gameObject.GetComponent<CanvasGroup>().alpha != 1)
                    {
                        cursor.gameObject.GetComponent<CanvasGroup>().alpha = 1;
                    }

                    confirmationQuantityUp = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/BuyQuantityUpButton").GetComponent<EquipShopConfirmationMouseEvents>();
                    confirmationQuantityDown = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/BuyQuantityDownButton").GetComponent<EquipShopConfirmationMouseEvents>();
                    confirmationConfirm = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/ConfirmationPanel/BuyButton").GetComponent<EquipShopConfirmationMouseEvents>();

                    RectTransform spacerScroll = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipListPanel/EquipScroller/EquipListSpacer").GetComponent<RectTransform>();

                    if (!dpadPressed && cursorOnItem == 0 && tempScrollDiff == 0)
                    {
                        if (itemCount > 1)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                dpadPressed = true;
                                cursorOnItem = cursorOnItem + 1;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            equipShopCursorState = EquipShopCursorStates.OPTIONS;
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 0 && tempScrollDiff > 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnItem = cursorOnItem + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, (itemSpacer * (tempScrollDiff - 1)), 0.0f);

                            tempScrollDiff = tempScrollDiff - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem > 0 && cursorOnItem < 8)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = cursorOnItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnItem = cursorOnItem + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 8 && tempScrollDiff == 0 && scrollDiff > 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = cursorOnItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, itemSpacer, 0.0f);

                            tempScrollDiff = tempScrollDiff + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 8 && tempScrollDiff == 0 && scrollDiff == 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = cursorOnItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 8 && tempScrollDiff > 0 && (cursorOnItem + tempScrollDiff) < (itemCount - 1))
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = 8;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, (itemSpacer * (tempScrollDiff + 1)), 0.0f); //and use tempScrollDiff - 1 to go up

                            tempScrollDiff = tempScrollDiff + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 8 && tempScrollDiff > 0 && (cursorOnItem + tempScrollDiff) == (itemCount - 1))
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = 7;

                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }

                    //Debug.Log("cursorOnItem: " + cursorOnItem + ", scrollDiff: " + scrollDiff + ", tempScrollDiff: " + tempScrollDiff);

                    if (itemCount != 0)
                    {
                        if (cursorOnItem == itemCount)
                        {
                            cursorOnItem = itemCount - 1;
                        }

                        if (cursorOnItem == 0 && tempScrollDiff == 0)
                        {
                            cursor.localPosition = new Vector3(-249f, 67f, 0);
                            esmeObj = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(0).gameObject;
                        }
                        else if (cursorOnItem == 0 && tempScrollDiff > 0)
                        {
                            cursor.localPosition = new Vector3(-249f, 67f, 0);
                            esmeObj = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(tempScrollDiff).gameObject;
                        }
                        else if (cursorOnItem > 0 && tempScrollDiff > 0)
                        {
                            cursor.localPosition = new Vector3(-249f, (67f - (cursorOnItem * itemSpacer)), 0);
                            esmeObj = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(cursorOnItem + tempScrollDiff).gameObject;
                        }
                        else
                        {
                            cursor.localPosition = new Vector3(-249f, (67f - (cursorOnItem * itemSpacer)), 0);
                            esmeObj = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopBuyPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(cursorOnItem).gameObject;
                        }
                    }

                    esme = esmeObj.GetComponent<EquipShopMouseEvents>();
                    esme.ShowEquipDetails();

                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;

                        GameManager.instance.equipShopItem = esme.GetEquip(esmeObj.transform.Find("BuyShopEquipNameText").GetComponent<Text>().text);
                        GameManager.instance.equipShopCost = int.Parse(esmeObj.transform.Find("BuyShopEquipCostText").GetComponent<Text>().text);

                        if (confirmationConfirm.HasEnoughGold(1))
                        {
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            equipShopCursorState = EquipShopCursorStates.ITEMCONFIRM;
                            esme.ProcessShop();
                        }
                        else
                        {
                            AudioManager.instance.PlaySE(AudioManager.instance.cantActionSE);
                        }
                    }

                }

                if (shopMode == "Sell")
                {
                    int itemCount = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.childCount;
                    int scrollDiff = itemCount - 9;

                    if (itemCount <= 9)
                    {
                        tempScrollDiff = 0;
                    }

                    if (cursor.parent != GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").transform)
                    {
                        cursor.SetParent(GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas").transform);
                    }

                    if (cursor.gameObject.GetComponent<CanvasGroup>().alpha != 1)
                    {
                        cursor.gameObject.GetComponent<CanvasGroup>().alpha = 1;
                    }

                    confirmationQuantityUp = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/SellQuantityUpButton").GetComponent<EquipShopConfirmationMouseEvents>();
                    confirmationQuantityDown = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/SellQuantityDownButton").GetComponent<EquipShopConfirmationMouseEvents>();
                    confirmationConfirm = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/ConfirmationPanel/SellButton").GetComponent<EquipShopConfirmationMouseEvents>();

                    RectTransform spacerScroll = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipListPanel/EquipScroller/EquipListSpacer").GetComponent<RectTransform>();

                    if (!dpadPressed && cursorOnItem == 0 && tempScrollDiff == 0)
                    {
                        if (itemCount > 1)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                dpadPressed = true;
                                cursorOnItem = cursorOnItem + 1;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            equipShopCursorState = EquipShopCursorStates.OPTIONS;
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 0 && tempScrollDiff > 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnItem = cursorOnItem + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, (itemSpacer * (tempScrollDiff - 1)), 0.0f);

                            tempScrollDiff = tempScrollDiff - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem > 0 && cursorOnItem < 8)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = cursorOnItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnItem = cursorOnItem + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 8 && tempScrollDiff == 0 && scrollDiff > 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = cursorOnItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, itemSpacer, 0.0f);

                            tempScrollDiff = tempScrollDiff + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 8 && tempScrollDiff == 0 && scrollDiff == 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = cursorOnItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 8 && tempScrollDiff > 0 && (cursorOnItem + tempScrollDiff) < (itemCount - 1))
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = 8;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, (itemSpacer * (tempScrollDiff + 1)), 0.0f); //and use tempScrollDiff - 1 to go up

                            tempScrollDiff = tempScrollDiff + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && cursorOnItem == 8 && tempScrollDiff > 0 && (cursorOnItem + tempScrollDiff) == (itemCount - 1))
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnItem = 7;

                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }

                    //Debug.Log("cursorOnItem: " + cursorOnItem + ", scrollDiff: " + scrollDiff + ", tempScrollDiff: " + tempScrollDiff);

                    if (itemCount != 0)
                    {
                        if (cursorOnItem == itemCount)
                        {
                            cursorOnItem = itemCount - 1;
                        }

                        if (cursorOnItem == 0 && tempScrollDiff == 0)
                        {
                            cursor.localPosition = new Vector3(-249f, 67f, 0);
                            esmeObj = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(0).gameObject;
                        }
                        else if (cursorOnItem == 0 && tempScrollDiff > 0)
                        {
                            cursor.localPosition = new Vector3(-249f, 67f, 0);
                            esmeObj = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(tempScrollDiff).gameObject;
                        }
                        else if (cursorOnItem > 0 && tempScrollDiff > 0)
                        {
                            cursor.localPosition = new Vector3(-249f, (67f - (cursorOnItem * itemSpacer)), 0);
                            esmeObj = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(cursorOnItem + tempScrollDiff).gameObject;
                        }
                        else
                        {
                            cursor.localPosition = new Vector3(-249f, (67f - (cursorOnItem * itemSpacer)), 0);
                            esmeObj = GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipShopSellPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(cursorOnItem).gameObject;
                        }

                        esme = esmeObj.GetComponent<EquipShopMouseEvents>();
                        esme.ShowEquipDetails();
                    }
                    else
                    {
                        cursorOnItem = 0;
                        shopMode = "Buy";
                        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/BuyOptionButton").GetComponent<EquipShopMouseEvents>().DisplayEquipShopBuyGUI();
                    }


                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;

                        GameManager.instance.equipShopItem = esme.GetEquip(esmeObj.transform.Find("SellShopEquipNameText").GetComponent<Text>().text);
                        GameManager.instance.equipShopCost = esme.GetEquip(esmeObj.transform.Find("SellShopEquipNameText").GetComponent<Text>().text).sellValue;

                        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().PlaySE(AudioManager.instance.confirmSE);
                        equipShopCursorState = EquipShopCursorStates.ITEMCONFIRM;
                        esme.ProcessShop();
                    }

                    cursor.localPosition = new Vector3(-249f, 67f - (cursorOnItem * itemSpacer), 0);
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    //clear items from list
                    ClearBuyList();

                    HideEquipShopGUI();
                    inShop = false;

                    cursor.SetParent(GameObject.Find("GameManager").transform);
                    cursor.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    cursorOnItem = 0;
                    cursorOnOption = 0;
                }
            }

            if (equipShopCursorState == EquipShopCursorStates.ITEMCONFIRM)
            {
                if (Input.GetAxisRaw("DpadHorizontal") == -1 && !dpadPressed) //left
                {
                    dpadPressed = true;
                    confirmationQuantityDown.gameObject.GetComponent<Image>().color = pressedColor;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                    confirmationQuantityDown.DecreaseQuantity(shopMode);
                }

                if (Input.GetAxisRaw("DpadHorizontal") == 1 && !dpadPressed) //right
                {
                    dpadPressed = true;
                    confirmationQuantityUp.gameObject.GetComponent<Image>().color = pressedColor;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                    confirmationQuantityUp.IncreaseQuantity(shopMode);
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                    equipShopCursorState = EquipShopCursorStates.ITEMLIST;

                    if (shopMode == "Buy")
                    {
                        confirmationConfirm.ConfirmPurchase();
                        ClearBuyList();
                        ShowEquipListInBuyGUI();
                    }
                    else if (shopMode == "Sell")
                    {
                        confirmationConfirm.ConfirmSell();
                    }
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    if (shopMode == "Buy")
                    {
                        confirmationConfirm.CancelPurchase();
                    }
                    else if (shopMode == "Sell")
                    {
                        confirmationConfirm.CancelSell();
                    }

                    AudioManager.instance.PlaySE(AudioManager.instance.backSE);

                    equipShopCursorState = EquipShopCursorStates.ITEMLIST;
                }
            }

            if (equipShopCursorState == EquipShopCursorStates.OPTIONS)
            {
                GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>().text = "";

                if (Input.GetAxisRaw("DpadHorizontal") == -1 && !dpadPressed && cursorOnOption == 1) //left
                {
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                    cursorOnOption = cursorOnOption - 1;
                }

                if (Input.GetAxisRaw("DpadHorizontal") == 1 && !dpadPressed && cursorOnOption == 0) //right
                {
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                    cursorOnOption = cursorOnOption + 1;
                }

                if (Input.GetAxisRaw("DpadVertical") == -1 && !dpadPressed) //down
                {
                    dpadPressed = true;
                    equipShopCursorState = EquipShopCursorStates.ITEMLIST;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    cursorOnItem = 0;

                    if (cursorOnOption == 0) //buy 
                    {
                        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/BuyOptionButton").GetComponent<EquipShopMouseEvents>().DisplayEquipShopBuyGUI();
                    }
                    else if (cursorOnOption == 1) //sell
                    {
                        GameObject.Find("GameManager/ShopCanvases/EquipShopCanvas/ShopOptionsPanel/SellOptionButton").GetComponent<EquipShopMouseEvents>().DisplayEquipShopSellGUI();
                    }
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    //clear items from list
                    ClearBuyList();

                    HideEquipShopGUI();
                    inShop = false;

                    cursor.SetParent(GameObject.Find("GameManager").transform);
                    cursor.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    cursorOnItem = 0;
                    cursorOnOption = 0;
                }

                if (cursorOnOption == 0)
                {
                    cursor.localPosition = new Vector3(-169f, 138, 0);
                    shopMode = "Buy";
                }
                else if (cursorOnOption == 1)
                {
                    cursor.localPosition = new Vector3(89, 138, 0);
                    shopMode = "Sell";
                }
            }

            if (Input.GetAxisRaw("DpadVertical") == 0 && Input.GetAxisRaw("DpadHorizontal") == 0)
            {
                dpadPressed = false;

                confirmationQuantityDown.gameObject.GetComponent<Image>().color = unpressedColor;
                confirmationQuantityUp.gameObject.GetComponent<Image>().color = unpressedColor;
            }

            if (confirmPressed && Input.GetButtonUp("Confirm"))
            {
                confirmPressed = false;
            }
        }
    }

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

    public void ShowEquipListInBuyGUI()
    {
        //set and show gold
        goldText.text = GameManager.instance.gold.ToString();
        
        //generate item prefab for each item in item shop list
        foreach (BaseShopEquipment shopEquip in equipShopList)
        {
            GameObject shopItemPanel = Instantiate(PrefabManager.Instance.shopBuyEquipPrefab);
            shopItemPanel.transform.Find("BuyShopEquipNameText").GetComponent<Text>().text = shopEquip.equipment.name;
            shopItemPanel.transform.Find("BuyShopEquipIcon").GetComponent<Image>().sprite = shopEquip.equipment.icon;
            shopItemPanel.transform.Find("BuyShopEquipCostText").GetComponent<Text>().text = shopEquip.cost.ToString();
            shopItemPanel.transform.SetParent(GameObject.Find("GameManager/ShopCanvases").GetComponent<ShopObjectHolder>().shopEquipBuyListSpacer, false);

            if (shopEquip.cost > GameManager.instance.gold)
            {
                shopItemPanel.transform.Find("BuyShopEquipNameText").GetComponent<Text>().color = Color.gray;
            }
            else
            {
                shopItemPanel.transform.Find("BuyShopEquipNameText").GetComponent<Text>().color = Color.white;
            }
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
