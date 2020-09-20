using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public List<BaseShopItem> itemShopList = new List<BaseShopItem>();

    public bool inShop;

    Text goldText;
    Text numOwned;

    Transform cursor;

    ItemShopMouseEvents isme;
    GameObject ismeObj;

    public enum ItemShopCursorStates
    {
        ITEMLIST,
        ITEMCONFIRM,
        OPTIONS
    }
    public ItemShopCursorStates itemShopCursorState;
    public string shopMode;
    bool dpadPressed;
    bool confirmPressed;

    int cursorOnItem;
    int cursorOnOption;
    float itemSpacer = 24.5f;

    int tempScrollDiff = 0;

    ItemShopConfirmationMouseEvents confirmationQuantityUp;
    ItemShopConfirmationMouseEvents confirmationQuantityDown;
    ItemShopConfirmationMouseEvents confirmationConfirm;
    Color pressedColor = new Color(.4f, .4f, .4f);
    Color unpressedColor = new Color(1f, 1f, 1f);

    private void Start()
    {
        cursor = GameObject.Find("GameManager/Cursor").transform;

        //set and show gold
        goldText = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/AdditionalDetailsPanel/GoldText").GetComponent<Text>();
        goldText.text = GameManager.instance.gold.ToString();

        //set and show number of items owned
        numOwned = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/AdditionalDetailsPanel/OwnedText").GetComponent<Text>();
        numOwned.text = "-";
    }

    private void Update()
    {
        if (inShop)
        {
            if (itemShopCursorState == ItemShopCursorStates.ITEMLIST)
            {
                if (shopMode == "Buy")
                {
                    int itemCount = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.childCount;
                    int scrollDiff = itemCount - 9;

                    if (itemCount <= 9)
                    {
                        tempScrollDiff = 0;
                    }

                    if (cursor.parent != GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").transform)
                    {
                        cursor.SetParent(GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").transform);
                    }

                    if (cursor.gameObject.GetComponent<CanvasGroup>().alpha != 1)
                    {
                        cursor.gameObject.GetComponent<CanvasGroup>().alpha = 1;
                    }

                    confirmationQuantityUp = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/BuyQuantityUpButton").GetComponent<ItemShopConfirmationMouseEvents>();
                    confirmationQuantityDown = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/BuyQuantityDownButton").GetComponent<ItemShopConfirmationMouseEvents>();
                    confirmationConfirm = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ConfirmationPanel/BuyButton").GetComponent<ItemShopConfirmationMouseEvents>();

                    RectTransform spacerScroll = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ItemListPanel/ItemScroller/ItemListSpacer").GetComponent<RectTransform>();

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
                            itemShopCursorState = ItemShopCursorStates.OPTIONS;
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
                            ismeObj = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(0).gameObject;
                        }
                        else if (cursorOnItem == 0 && tempScrollDiff > 0)
                        {
                            cursor.localPosition = new Vector3(-249f, 67f, 0);
                            ismeObj = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(tempScrollDiff).gameObject;
                        }
                        else if (cursorOnItem > 0 && tempScrollDiff > 0)
                        {
                            cursor.localPosition = new Vector3(-249f, (67f - (cursorOnItem * itemSpacer)), 0);
                            ismeObj = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(cursorOnItem + tempScrollDiff).gameObject;
                        }
                        else
                        {
                            cursor.localPosition = new Vector3(-249f, (67f - (cursorOnItem * itemSpacer)), 0);
                            ismeObj = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopBuyPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(cursorOnItem).gameObject;
                        }
                    }
                    
                    isme = ismeObj.GetComponent<ItemShopMouseEvents>();                    
                    isme.ShowItemDetails();

                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;

                        GameManager.instance.itemShopItem = isme.GetItem(ismeObj.transform.Find("BuyShopItemNameText").GetComponent<Text>().text);
                        GameManager.instance.itemShopCost = int.Parse(ismeObj.transform.Find("BuyShopItemCostText").GetComponent<Text>().text);

                        if (confirmationConfirm.HasEnoughGold(1))
                        {
                            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().PlaySE(AudioManager.instance.confirmSE);
                            itemShopCursorState = ItemShopCursorStates.ITEMCONFIRM;
                            isme.ProcessShop();
                        } else
                        {
                            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().PlaySE(AudioManager.instance.cantActionSE);
                        }
                    }
                    
                }

                if (shopMode == "Sell")
                {
                    int itemCount = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.childCount;
                    int scrollDiff = itemCount - 9;

                    if (itemCount <= 9)
                    {
                        tempScrollDiff = 0;
                    }

                    if (cursor.parent != GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").transform)
                    {
                        cursor.SetParent(GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas").transform);
                    }

                    if (cursor.gameObject.GetComponent<CanvasGroup>().alpha != 1)
                    {
                        cursor.gameObject.GetComponent<CanvasGroup>().alpha = 1;
                    }

                    confirmationQuantityUp = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/SellQuantityUpButton").GetComponent<ItemShopConfirmationMouseEvents>();
                    confirmationQuantityDown = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/SellQuantityDownButton").GetComponent<ItemShopConfirmationMouseEvents>();
                    confirmationConfirm = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ConfirmationPanel/SellButton").GetComponent<ItemShopConfirmationMouseEvents>();

                    RectTransform spacerScroll = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ItemListPanel/ItemScroller/ItemListSpacer").GetComponent<RectTransform>();

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
                            itemShopCursorState = ItemShopCursorStates.OPTIONS;
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
                            ismeObj = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(0).gameObject;
                        }
                        else if (cursorOnItem == 0 && tempScrollDiff > 0)
                        {
                            cursor.localPosition = new Vector3(-249f, 67f, 0);
                            ismeObj = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(tempScrollDiff).gameObject;
                        }
                        else if (cursorOnItem > 0 && tempScrollDiff > 0)
                        {
                            cursor.localPosition = new Vector3(-249f, (67f - (cursorOnItem * itemSpacer)), 0);
                            ismeObj = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(cursorOnItem + tempScrollDiff).gameObject;
                        }
                        else
                        {
                            cursor.localPosition = new Vector3(-249f, (67f - (cursorOnItem * itemSpacer)), 0);
                            ismeObj = GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemShopSellPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(cursorOnItem).gameObject;
                        }

                        isme = ismeObj.GetComponent<ItemShopMouseEvents>();
                        isme.ShowItemDetails();
                    } else
                    {
                        cursorOnItem = 0;
                        shopMode = "Buy";
                        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/BuyOptionButton").GetComponent<ItemShopMouseEvents>().DisplayItemShopBuyGUI();
                    }


                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;

                        GameManager.instance.itemShopItem = isme.GetItem(ismeObj.transform.Find("SellShopItemNameText").GetComponent<Text>().text);
                        GameManager.instance.itemShopCost = isme.GetItem(ismeObj.transform.Find("SellShopItemNameText").GetComponent<Text>().text).sellValue;

                        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().PlaySE(AudioManager.instance.confirmSE);
                        itemShopCursorState = ItemShopCursorStates.ITEMCONFIRM;
                        isme.ProcessShop();
                    }

                    cursor.localPosition = new Vector3(-249f, 67f - (cursorOnItem * itemSpacer), 0);
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    //clear items from list
                    ClearBuyList();

                    HideItemShopGUI();
                    inShop = false;

                    cursor.SetParent(GameObject.Find("GameManager").transform);
                    cursor.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    cursorOnItem = 0;
                    cursorOnOption = 0;
                }
            }

            if (itemShopCursorState == ItemShopCursorStates.ITEMCONFIRM)
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

                    itemShopCursorState = ItemShopCursorStates.ITEMLIST;

                    if (shopMode == "Buy")
                    {
                        confirmationConfirm.ConfirmPurchase();
                        ClearBuyList();
                        ShowItemListInBuyGUI();
                    } else if (shopMode == "Sell")
                    {
                        confirmationConfirm.ConfirmSell();
                    }
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    if (shopMode == "Buy")
                    {
                        confirmationConfirm.CancelPurchase();
                    } else if (shopMode == "Sell")
                    {
                        confirmationConfirm.CancelSell();
                    }

                    AudioManager.instance.PlaySE(AudioManager.instance.backSE);

                    itemShopCursorState = ItemShopCursorStates.ITEMLIST;
                }
            }

            if (itemShopCursorState == ItemShopCursorStates.OPTIONS)
            {
                GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "";

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
                    itemShopCursorState = ItemShopCursorStates.ITEMLIST;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    cursorOnItem = 0;

                    if (cursorOnOption == 0) //buy 
                    {
                        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/BuyOptionButton").GetComponent<ItemShopMouseEvents>().DisplayItemShopBuyGUI();
                    } else if (cursorOnOption == 1) //sell
                    {
                        GameObject.Find("GameManager/ShopCanvases/ItemShopCanvas/ShopOptionsPanel/SellOptionButton").GetComponent<ItemShopMouseEvents>().DisplayItemShopSellGUI();
                    }
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    //clear items from list
                    ClearBuyList();

                    HideItemShopGUI();
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
                } else if (cursorOnOption == 1)
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

            AudioManager.instance.PlaySE(AudioManager.instance.backSE);
        }
    }

    public void ShowItemListInBuyGUI()
    {
        goldText.text = GameManager.instance.gold.ToString();

        //generate item prefab for each item in item shop list
        foreach (BaseShopItem shopItem in itemShopList)
        {
            GameObject shopItemPanel = Instantiate(PrefabManager.Instance.shopBuyItemPrefab);
            shopItemPanel.transform.Find("BuyShopItemNameText").GetComponent<Text>().text = shopItem.item.name;
            shopItemPanel.transform.Find("BuyShopItemIcon").GetComponent<Image>().sprite = shopItem.item.icon;
            shopItemPanel.transform.Find("BuyShopItemCostText").GetComponent<Text>().text = shopItem.cost.ToString();
            shopItemPanel.transform.SetParent(GameObject.Find("GameManager/ShopCanvases").GetComponent<ShopObjectHolder>().shopItemBuyListSpacer, false);
            
            if (shopItem.cost > GameManager.instance.gold)
            {
                shopItemPanel.transform.Find("BuyShopItemNameText").GetComponent<Text>().color = Color.gray;
            } else
            {
                shopItemPanel.transform.Find("BuyShopItemNameText").GetComponent<Text>().color = Color.white;
            }
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
