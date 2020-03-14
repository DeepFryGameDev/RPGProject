using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameMenu : MonoBehaviour
{
    CanvasGroup mainMenuCanvasGroup;

    GameObject player;
    public bool drawingGUI = false;
    public bool disableMenu = false;
    public bool disableMenuExit = false;
    [HideInInspector]public bool menuCalled = false;

    //for all main menu objects
    public GameObject Hero1Panel;
    public GameObject Hero2Panel;
    public GameObject Hero3Panel;
    public GameObject Hero4Panel;
    public GameObject Hero5Panel;
    public Image Hero1HPProgressBar;
    public Image Hero1MPProgressBar;
    public Image Hero1EXPProgressBar;
    public Image Hero2HPProgressBar;
    public Image Hero2MPProgressBar;
    public Image Hero2EXPProgressBar;
    public Image Hero3HPProgressBar;
    public Image Hero3MPProgressBar;
    public Image Hero3EXPProgressBar;
    public Image Hero4HPProgressBar;
    public Image Hero4MPProgressBar;
    public Image Hero4EXPProgressBar;
    public Image Hero5HPProgressBar;
    public Image Hero5MPProgressBar;
    public Image Hero5EXPProgressBar;

    //for item menu objects
    public GameObject Hero1ItemPanel;
    public GameObject Hero2ItemPanel;
    public GameObject Hero3ItemPanel;
    public GameObject Hero4ItemPanel;
    public GameObject Hero5ItemPanel;
    public Image Hero1ItemMenuHPProgressBar;
    public Image Hero1ItemMenuMPProgressBar;
    public Image Hero2ItemMenuHPProgressBar;
    public Image Hero2ItemMenuMPProgressBar;
    public Image Hero3ItemMenuHPProgressBar;
    public Image Hero3ItemMenuMPProgressBar;
    public Image Hero4ItemMenuHPProgressBar;
    public Image Hero4ItemMenuMPProgressBar;
    public Image Hero5ItemMenuHPProgressBar;
    public Image Hero5ItemMenuMPProgressBar;
    private Transform ItemListSpacer;
    public GameObject NewItemPanel; //for each individual item in item menu

    //for magic menu objects
    public Image MagicPanelHPProgressBar;
    public Image MagicPanelMPProgressBar;
    public GameObject HeroSelectMagicPanel;
    public GameObject Hero1SelectMagicPanel;
    public GameObject Hero2SelectMagicPanel;
    public GameObject Hero3SelectMagicPanel;
    public GameObject Hero4SelectMagicPanel;
    public GameObject Hero5SelectMagicPanel;
    public Image Hero1MagicMenuHPProgressBar;
    public Image Hero1MagicMenuMPProgressBar;
    public Image Hero2MagicMenuHPProgressBar;
    public Image Hero2MagicMenuMPProgressBar;
    public Image Hero3MagicMenuHPProgressBar;
    public Image Hero3MagicMenuMPProgressBar;
    public Image Hero4MagicMenuHPProgressBar;
    public Image Hero4MagicMenuMPProgressBar;
    public Image Hero5MagicMenuHPProgressBar;
    public Image Hero5MagicMenuMPProgressBar;
    public GameObject WhiteMagicListPanel;
    public GameObject BlackMagicListPanel;
    public GameObject SorceryMagicListPanel;
    private Transform WhiteMagicListSpacer;
    private Transform BlackMagicListSpacer;
    private Transform SorceryMagicListSpacer;
    public GameObject NewMagicPanel;

    //for equip menu objects
    public Image EquipPanelHPProgressBar;
    public Image EquipPanelMPProgressBar;
    public GameObject NewEquipPanel;
    private Transform EquipListSpacer;
    private string equipButtonClicked;
    bool inEquipList;
    [HideInInspector] public string equipMode;

    //for status menu objects
    public Image StatusPanelHPProgressBar;
    public Image StatusPanelMPProgressBar;
    public Image StatusPanelEXPProgressBar;

    //for menu buttons
    public Button ItemButton;
    public Button MagicButton;
    public Button EquipButton;
    public Button StatusButton;
    public Button PartyButton;
    public Button OrderButton;
    public Button GridButton;
    public Button ConfigButton;
    public Button QuitButton;

    //for menu canvases
    public Canvas MainMenuCanvas;
    public Canvas ItemMenuCanvas;
    public Canvas MagicMenuCanvas;
    public Canvas EquipMenuCanvas;
    public Canvas StatusMenuCanvas;
    public Canvas PartyMenuCanvas;
    public Canvas OrderMenuCanvas;
    public Canvas GridMenuCanvas;
    public Canvas ConfigMenuCanvas;
    public Canvas QuitMenuCanvas;
    
    [HideInInspector] public BaseHero HeroForMagicMenu; //hero to be set for entering magic menu
    [HideInInspector] public BaseHero HeroForEquipMenu;
    
    public enum MenuStates
    {
        MAIN,
        ITEM,
        MAGIC,
        EQUIP,
        STATUS,
        PARTY,
        ORDER,
        GRID,
        CONFIG,
        QUIT,
        IDLE
    }
    public MenuStates menuState; //for which menu is open

    bool buttonPressed = false;

    GraphicRaycaster raycaster;
    BaseHero heroToCheck;

    void Start()
    {
        mainMenuCanvasGroup = MainMenuCanvas.GetComponent<CanvasGroup>();
        player = GameObject.Find("Player");

        mainMenuCanvasGroup.alpha = 0; //hide menu

        //sets all menu canvases to visible
        ItemMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
        MagicMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
        EquipMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
        StatusMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
        PartyMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
        OrderMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
        GridMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
        ConfigMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
        QuitMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;

        //disables all menu canvases
        mainMenuCanvasGroup.gameObject.SetActive(false);
        ItemMenuCanvas.gameObject.SetActive(false);
        MagicMenuCanvas.gameObject.SetActive(false);
        WhiteMagicListPanel.gameObject.SetActive(false);
        BlackMagicListPanel.gameObject.SetActive(false);
        SorceryMagicListPanel.gameObject.SetActive(false);
        HeroSelectMagicPanel.gameObject.SetActive(false);
        EquipMenuCanvas.gameObject.SetActive(false);
        StatusMenuCanvas.gameObject.SetActive(false);
        PartyMenuCanvas.gameObject.SetActive(false);
        OrderMenuCanvas.gameObject.SetActive(false);
        GridMenuCanvas.gameObject.SetActive(false);
        ConfigMenuCanvas.gameObject.SetActive(false);
        QuitMenuCanvas.gameObject.SetActive(false);

        //sets spacers
        ItemListSpacer = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform; //find spacer and make connection
        WhiteMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/WhiteMagicListPanel/WhiteMagicScroller/WhiteMagicListSpacer").transform; //find spacer and make connection
        BlackMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/BlackMagicListPanel/BlackMagicScroller/BlackMagicListSpacer").transform; //find spacer and make connection
        SorceryMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/SorceryMagicListPanel/SorceryMagicScroller/SorceryMagicListSpacer").transform; //find spacer and make connection
        EquipListSpacer = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform;

        equipMode = "";

        menuState = MenuStates.IDLE; //sets menu state to idle as menu is closed
        this.raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        if ((Input.GetButtonDown("Menu") && !disableMenu) || menuCalled) //if menu is called (and not disabled)
        {
            drawingGUI = true;
        }

        DisplayTime(); //keeps time counter updated
    }

    private void OnGUI() //Actually draws the menu
    {
        if (drawingGUI && (mainMenuCanvasGroup.alpha == 0))
        {
            PauseBackground(true); //keeps background objects from processing
            ShowMainMenu();
            menuState = MenuStates.MAIN;
        }

        if (Input.GetButtonDown("Cancel") && !disableMenuExit && menuState == MenuStates.MAIN && !buttonPressed) //if cancel is pressed while on main menu
        {
            drawingGUI = false;
            mainMenuCanvasGroup.alpha = 0;
            MainMenuCanvas.gameObject.SetActive(false);
            PauseBackground(false);
            menuCalled = false;
            menuState = MenuStates.IDLE;
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.ITEM && !buttonPressed) //if cancel is pressed on item menu
        {
            HideItemMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.MAGIC && !buttonPressed) //if cancel is pressed on magic menu
        {
            HideMagicMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.EQUIP && !buttonPressed && !inEquipList) //if cancel is pressed on magic menu
        {
            HideEquipMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && !buttonPressed && inEquipList)
        {
            CancelFromEquipList();
            inEquipList = false;
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.STATUS && !buttonPressed) //if cancel is pressed on status menu
        {
            HideStatusMenu();
            ShowMainMenu();
        }

        CheckCancelPressed(); //makes sure cancel is only pressed once
    }

    //Main menu panels

    void ShowMainMenu() //draws main menu
    {
        MainMenuCanvas.gameObject.SetActive(true);
        mainMenuCanvasGroup.alpha = 1;
        DrawHeroStats();
        DisplayLocation();
        DisplayGold();
    }

    void DrawHeroStats()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroPanels(heroCount);

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            GameObject.Find("Hero" + (i + 1) + "Panel").transform.GetChild(1).GetComponent<Text>().text = GameManager.instance.activeHeroes[i]._Name; //Name text
            GameObject.Find("Hero" + (i + 1) + "Panel").transform.GetChild(3).GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("Hero" + (i + 1) + "Panel").transform.GetChild(5).GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].maxHP); //HP text
            GameObject.Find("Hero" + (i + 1) + "Panel").transform.GetChild(8).GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].maxMP); //MP text
            GameObject.Find("Hero" + (i + 1) + "Panel").transform.GetChild(11).GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].currentExp + " / " + GameManager.instance.toLevelUp[(GameManager.instance.activeHeroes[i].currentLevel -1)]); //Exp text
        }
    }

    void DrawHeroPanels(int count)
    {
        if (count == 1)
        {
            Hero1Panel.SetActive(true);
            Hero2Panel.SetActive(false);
            Hero3Panel.SetActive(false);
            Hero4Panel.SetActive(false);
            Hero5Panel.SetActive(false);
        }
        else if (count == 2)
        {
            Hero1Panel.SetActive(true);
            Hero2Panel.SetActive(true);
            Hero3Panel.SetActive(false);
            Hero4Panel.SetActive(false);
            Hero5Panel.SetActive(false);
        }
        else if (count == 3)
        {
            Hero1Panel.SetActive(true);
            Hero2Panel.SetActive(true);
            Hero3Panel.SetActive(true);
            Hero4Panel.SetActive(false);
            Hero5Panel.SetActive(false);
        }
        else if (count == 4)
        {
            Hero1Panel.SetActive(true);
            Hero2Panel.SetActive(true);
            Hero3Panel.SetActive(true);
            Hero4Panel.SetActive(true);
            Hero5Panel.SetActive(false);
        }
        else if (count == 5)
        {
            Hero1Panel.SetActive(true);
            Hero2Panel.SetActive(true);
            Hero3Panel.SetActive(true);
            Hero4Panel.SetActive(true);
            Hero5Panel.SetActive(true);
        }

        DrawHeroPanelBars();
    }

    void DrawHeroPanelBars()
    {
        if (GameManager.instance.activeHeroes.Count == 1)
        {
            Hero1HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1HPProgressBar.transform.localScale.y);
            Hero1MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MPProgressBar.transform.localScale.y);
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);
        } else if (GameManager.instance.activeHeroes.Count == 2)
        {
            Hero1HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1HPProgressBar.transform.localScale.y);
            Hero1MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MPProgressBar.transform.localScale.y);
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);

            Hero2HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2HPProgressBar.transform.localScale.y);
            Hero2MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MPProgressBar.transform.localScale.y);
            Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2EXPProgressBar.transform.localScale.y);
        } else if (GameManager.instance.activeHeroes.Count == 3)
        {
            Hero1HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1HPProgressBar.transform.localScale.y);
            Hero1MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MPProgressBar.transform.localScale.y);
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);

            Hero2HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2HPProgressBar.transform.localScale.y);
            Hero2MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MPProgressBar.transform.localScale.y);
            Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2EXPProgressBar.transform.localScale.y);

            Hero3HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3HPProgressBar.transform.localScale.y);
            Hero3MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MPProgressBar.transform.localScale.y);
            Hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3EXPProgressBar.transform.localScale.y);
        } else if (GameManager.instance.activeHeroes.Count == 4)
        {
            Hero1HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1HPProgressBar.transform.localScale.y);
            Hero1MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MPProgressBar.transform.localScale.y);
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);

            Hero2HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2HPProgressBar.transform.localScale.y);
            Hero2MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MPProgressBar.transform.localScale.y);
            Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2EXPProgressBar.transform.localScale.y);

            Hero3HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3HPProgressBar.transform.localScale.y);
            Hero3MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MPProgressBar.transform.localScale.y);
            Hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3EXPProgressBar.transform.localScale.y);

            Hero4HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4HPProgressBar.transform.localScale.y);
            Hero4MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4MPProgressBar.transform.localScale.y);
            Hero4EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4EXPProgressBar.transform.localScale.y);
        } else if (GameManager.instance.activeHeroes.Count == 5)
        {
            Hero1HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1HPProgressBar.transform.localScale.y);
            Hero1MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MPProgressBar.transform.localScale.y);
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);

            Hero2HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2HPProgressBar.transform.localScale.y);
            Hero2MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MPProgressBar.transform.localScale.y);
            Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2EXPProgressBar.transform.localScale.y);

            Hero3HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3HPProgressBar.transform.localScale.y);
            Hero3MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MPProgressBar.transform.localScale.y);
            Hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3EXPProgressBar.transform.localScale.y);

            Hero4HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4HPProgressBar.transform.localScale.y);
            Hero4MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4MPProgressBar.transform.localScale.y);
            Hero4EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4EXPProgressBar.transform.localScale.y);

            Hero5HPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5HPProgressBar.transform.localScale.y);
            Hero5MPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5MPProgressBar.transform.localScale.y);
            Hero5EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5EXPProgressBar.transform.localScale.y);
        }

    }

    void DisplayLocation() //draws location area (could be updated eventually so it's easier to update location being displayed)
    {
        string sector = SceneManager.GetActiveScene().name; //sector is the scene name
        DisplayCity(sector);
        DisplaySector(sector); //city is which area the sector is located in
    }

    void DisplaySector(string sector)
    {
        GameObject.Find("LocationPanel").transform.GetChild(1).GetComponent<Text>().text = sector;
    }

    void DisplayCity(string sector) //could be updated for easier access
    {
        string city = "";

        if (sector == "Test Town")
        {
            city = "Debug Land";
        }

        if (sector == "Fight Zone")
        {
            city = "Debug Land";
        }

        GameObject.Find("LocationPanel").transform.GetChild(0).GetComponent<Text>().text = city;
    }

    void DisplayGold()
    {
        GameObject.Find("TimeGoldPanel").transform.GetChild(3).GetComponent<Text>().text = GameManager.instance.gold.ToString();
    }

    void DisplayTime() //shows the current time
    {
        if (menuState == MenuStates.MAIN)
        {
            string hours = GameManager.instance.hours.ToString();
            string minutes = GameManager.instance.minutes.ToString();
            string seconds = GameManager.instance.seconds.ToString();

            if (hours.Length == 1)
            {
                hours = "0" + hours;
            }

            if (minutes.Length == 1)
            {
                minutes = "0" + minutes;
            }

            if (seconds.Length == 1)
            {
                seconds = "0" + seconds;
            }

            GameObject.Find("TimeGoldPanel").transform.GetChild(1).GetComponent<Text>().text = hours + ":" + minutes + ":" + seconds;
        }
    }

    //--------------------

    //Item Menu

    public void ShowItemMenu() //displays item menu
    {
        DrawItemMenu();
    }

    void DrawItemMenu()
    {
        MainMenuCanvas.gameObject.SetActive(false);
        ItemMenuCanvas.gameObject.SetActive(true);
        menuState = MenuStates.ITEM;
        EraseItemDescText();
        DrawHeroItemMenuStats();
        DrawItemList();
    }

    void HideItemMenu()
    {
        ResetItemList();
        MainMenuCanvas.gameObject.SetActive(true);
        ItemMenuCanvas.gameObject.SetActive(false);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
    }

    void EraseItemDescText()
    {
        GameObject.Find("ItemMenuCanvas/ItemMenuPanel/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "";
    }

    public void DrawItemList() //draws items in inventory to item list
    {
        List<Item> itemsAccountedFor = new List<Item>();

        foreach (Item item in Inventory.instance.items)
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
                NewItemPanel = Instantiate(PrefabManager.Instance.itemPrefab);
                NewItemPanel.transform.GetChild(0).GetComponent<Text>().text = item.name;
                NewItemPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
                NewItemPanel.transform.GetChild(2).GetComponent<Text>().text = itemCount.ToString();
                if (!item.usableInMenu)
                {
                    NewItemPanel.transform.GetChild(0).GetComponent<Text>().color = Color.gray;
                    NewItemPanel.transform.GetChild(2).GetComponent<Text>().color = Color.gray;
                }
                NewItemPanel.transform.SetParent(ItemListSpacer, false);
                itemsAccountedFor.Add(item);
            }
        }

        itemsAccountedFor.Clear();
    }

    public void ResetItemList()
    {
        foreach (Transform child in GameObject.Find("ItemListPanel/ItemScroller/ItemListSpacer").transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DrawHeroItemMenuStats()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroItemMenuPanels(heroCount);

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero" + (i + 1) + "ItemPanel/NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i]._Name; //Name text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero" + (i + 1) + "ItemPanel/LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero" + (i + 1) + "ItemPanel/HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].maxHP); //HP text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero" + (i + 1) + "ItemPanel/MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].maxMP); //MP text
        }
    }

    void DrawHeroItemMenuPanels(int count)
    {
        if (count == 1)
        {
            Hero1ItemPanel.SetActive(true);
            Hero2ItemPanel.SetActive(false);
            Hero3ItemPanel.SetActive(false);
            Hero4ItemPanel.SetActive(false);
            Hero5ItemPanel.SetActive(false);
        }
        else if (count == 2)
        {
            Hero1ItemPanel.SetActive(true);
            Hero2ItemPanel.SetActive(true);
            Hero3ItemPanel.SetActive(false);
            Hero4ItemPanel.SetActive(false);
            Hero5ItemPanel.SetActive(false);
        }
        else if (count == 3)
        {
            Hero1ItemPanel.SetActive(true);
            Hero2ItemPanel.SetActive(true);
            Hero3ItemPanel.SetActive(true);
            Hero4ItemPanel.SetActive(false);
            Hero5ItemPanel.SetActive(false);
        }
        else if (count == 4)
        {
            Hero1ItemPanel.SetActive(true);
            Hero2ItemPanel.SetActive(true);
            Hero3ItemPanel.SetActive(true);
            Hero4ItemPanel.SetActive(true);
            Hero5ItemPanel.SetActive(false);
        }
        else if (count == 5)
        {
            Hero1ItemPanel.SetActive(true);
            Hero2ItemPanel.SetActive(true);
            Hero3ItemPanel.SetActive(true);
            Hero4ItemPanel.SetActive(true);
            Hero5ItemPanel.SetActive(true);
        }

        DrawHeroItemMenuPanelBars();
    }

    void DrawHeroItemMenuPanelBars()
    {
        if (GameManager.instance.activeHeroes.Count == 1)
        {
            Hero1ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuHPProgressBar.transform.localScale.y);
            Hero1ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 2)
        {
            Hero1ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuHPProgressBar.transform.localScale.y);
            Hero1ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuMPProgressBar.transform.localScale.y);

            Hero2ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2ItemMenuHPProgressBar.transform.localScale.y);
            Hero2ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2ItemMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 3)
        {
            Hero1ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuHPProgressBar.transform.localScale.y);
            Hero1ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuMPProgressBar.transform.localScale.y);

            Hero2ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2ItemMenuHPProgressBar.transform.localScale.y);
            Hero2ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2ItemMenuMPProgressBar.transform.localScale.y);

            Hero3ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3ItemMenuHPProgressBar.transform.localScale.y);
            Hero3ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3ItemMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 4)
        {
            Hero1ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuHPProgressBar.transform.localScale.y);
            Hero1ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuMPProgressBar.transform.localScale.y);

            Hero2ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2ItemMenuHPProgressBar.transform.localScale.y);
            Hero2ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2ItemMenuMPProgressBar.transform.localScale.y);

            Hero3ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3ItemMenuHPProgressBar.transform.localScale.y);
            Hero3ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3ItemMenuMPProgressBar.transform.localScale.y);

            Hero4ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4ItemMenuHPProgressBar.transform.localScale.y);
            Hero4ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4ItemMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 5)
        {
            Hero1ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuHPProgressBar.transform.localScale.y);
            Hero1ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1ItemMenuMPProgressBar.transform.localScale.y);

            Hero2ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2ItemMenuHPProgressBar.transform.localScale.y);
            Hero2ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2ItemMenuMPProgressBar.transform.localScale.y);

            Hero3ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3ItemMenuHPProgressBar.transform.localScale.y);
            Hero3ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3ItemMenuMPProgressBar.transform.localScale.y);

            Hero4ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4ItemMenuHPProgressBar.transform.localScale.y);
            Hero4ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4ItemMenuMPProgressBar.transform.localScale.y);

            Hero5ItemMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5ItemMenuHPProgressBar.transform.localScale.y);
            Hero5ItemMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5ItemMenuMPProgressBar.transform.localScale.y);
        }

    }

    //--------------------

    //Magic Menu

    public void ShowMagicMenu() //shows magic menu
    {
        Debug.Log("Magic button clicked - choose a hero");
        StartCoroutine(ChooseHeroForMagicMenu());
    }

    IEnumerator ChooseHeroForMagicMenu()
    {
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }
        DrawMagicMenu(heroToCheck);
    }

    void DrawMagicMenu(BaseHero hero)
    {
        HeroForMagicMenu = hero;
        MainMenuCanvas.gameObject.SetActive(false);
        MagicMenuCanvas.gameObject.SetActive(true);
        WhiteMagicListPanel.gameObject.SetActive(true);
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDescriptionPanel/MagicDescriptionText").GetComponent<Text>().text = "";
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel/CooldownText").GetComponent<Text>().text = "-";
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel/MPCostText").GetComponent<Text>().text = "-";
        DrawMagicMenuStats(hero);
        DrawMagicListPanels(hero);
        menuState = MenuStates.MAGIC;
    }

    void DrawMagicListPanels(BaseHero hero)
    {
        WhiteMagicListPanel.gameObject.SetActive(true);
        BlackMagicListPanel.gameObject.SetActive(true);
        SorceryMagicListPanel.gameObject.SetActive(true);
        foreach (BaseAttack magicAttack in hero.MagicAttacks)
        {
            NewMagicPanel = Instantiate(PrefabManager.Instance.magicPrefab);
            NewMagicPanel.transform.GetChild(0).GetComponent<Text>().text = magicAttack.name;

            if (magicAttack.magicClass == BaseAttack.MagicClass.WHITE)
            {
                NewMagicPanel.transform.SetParent(WhiteMagicListSpacer, false);
            }
            if (magicAttack.magicClass == BaseAttack.MagicClass.BLACK)
            {
                NewMagicPanel.transform.SetParent(BlackMagicListSpacer, false);
            }
            if (magicAttack.magicClass == BaseAttack.MagicClass.SORCERY)
            {
                NewMagicPanel.transform.SetParent(SorceryMagicListSpacer, false);
            }
            if (hero.curMP < magicAttack.MPCost)
            {
                magicAttack.enoughMP = false;
                NewMagicPanel.transform.GetChild(0).GetComponent<Text>().color = Color.gray;
            } else
            {
                magicAttack.enoughMP = true;
            }
            if (!magicAttack.usableInMenu)
            {
                NewMagicPanel.transform.GetChild(0).GetComponent<Text>().color = Color.gray;
            }
        }
        BlackMagicListPanel.gameObject.SetActive(false);
        SorceryMagicListPanel.gameObject.SetActive(false);
    }

    public void ShowWhiteMagicListPanel()
    {
        WhiteMagicListPanel.gameObject.SetActive(true);
        BlackMagicListPanel.gameObject.SetActive(false);
        SorceryMagicListPanel.gameObject.SetActive(false);
    }

    public void ShowBlackMagicListPanel()
    {
        WhiteMagicListPanel.gameObject.SetActive(false);
        BlackMagicListPanel.gameObject.SetActive(true);
        SorceryMagicListPanel.gameObject.SetActive(false);
    }

    public void ShowSorceryMagicListPanel()
    {
        WhiteMagicListPanel.gameObject.SetActive(false);
        BlackMagicListPanel.gameObject.SetActive(false);
        SorceryMagicListPanel.gameObject.SetActive(true);
    }

    public void DrawMagicMenuStats(BaseHero hero)
    {
        GameObject.Find("HeroMagicPanel/NameText").GetComponent<Text>().text = hero._Name; //Name text
        GameObject.Find("HeroMagicPanel/LevelText").GetComponent<Text>().text = hero.currentLevel.ToString(); //Level text
        GameObject.Find("HeroMagicPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.maxHP.ToString()); //HP text
        GameObject.Find("HeroMagicPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.maxMP.ToString()); //MP text

        MagicPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hero), 0, 1), MagicPanelHPProgressBar.transform.localScale.y);
        MagicPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hero), 0, 1), MagicPanelMPProgressBar.transform.localScale.y);
    }

    void ResetMagicList()
    {
        WhiteMagicListPanel.gameObject.SetActive(true);
        BlackMagicListPanel.gameObject.SetActive(true);
        SorceryMagicListPanel.gameObject.SetActive(true);
        foreach (Transform child in GameObject.Find("WhiteMagicListPanel/WhiteMagicScroller/WhiteMagicListSpacer").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("BlackMagicListPanel/BlackMagicScroller/BlackMagicListSpacer").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("SorceryMagicListPanel/SorceryMagicScroller/SorceryMagicListSpacer").transform)
        {
            Destroy(child.gameObject);
        }
        WhiteMagicListPanel.gameObject.SetActive(false);
        BlackMagicListPanel.gameObject.SetActive(false);
        SorceryMagicListPanel.gameObject.SetActive(false);
    }

    void HideMagicMenu()
    {
        ResetMagicList();
        MainMenuCanvas.gameObject.SetActive(true);
        MagicMenuCanvas.gameObject.SetActive(false);
        WhiteMagicListPanel.gameObject.SetActive(false);
        BlackMagicListPanel.gameObject.SetActive(false);
        SorceryMagicListPanel.gameObject.SetActive(false);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
    }

    //--------------------

    //Equip Menu

    public void ShowEquipMenu()
    {
        Debug.Log("Equip button clicked - choose a hero");
        StartCoroutine(ChooseHeroForEquipMenu());
    }

    IEnumerator ChooseHeroForEquipMenu()
    {
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }
        DrawEquipMenu(heroToCheck);
    }

    void DrawEquipMenu(BaseHero hero)
    {
        HeroForEquipMenu = hero;
        MainMenuCanvas.gameObject.SetActive(false);
        EquipMenuCanvas.gameObject.SetActive(true);
        GameObject.Find("EquipMenuCanvas/EquipMenuPanel/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>().text = "";
        ChangeEquipMode("Equip");
        DrawInitialArrows();
        DrawEquipMenuStats(hero);
        DrawCurrentEquipment(hero);
        menuState = MenuStates.EQUIP;
    }

    void DrawCurrentEquipment(BaseHero hero)
    {
        string baseEquipPath = "GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipSlotsPanel/";

        if (hero.equipment[0] == null)
        {
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = hero.equipment[0].name;
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = hero.equipment[0].icon;

            Color temp = GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
        }
        if (hero.equipment[1] == null)
        {
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = hero.equipment[1].name;
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = hero.equipment[1].icon;

            Color temp = GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
        }
        if (hero.equipment[2] == null)
        {
            GameObject.Find(baseEquipPath + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = hero.equipment[2].name;
            GameObject.Find(baseEquipPath + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = hero.equipment[2].icon;

            Color temp = GameObject.Find(baseEquipPath + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;
        }
        if (hero.equipment[3] == null)
        {
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = hero.equipment[3].name;
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = hero.equipment[3].icon;

            Color temp = GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
        }
        if (hero.equipment[4] == null)
        {
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = hero.equipment[4].name;
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = hero.equipment[4].icon;
        }
        if (hero.equipment[5] == null)
        {
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = hero.equipment[5].name;
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = hero.equipment[5].icon;

            Color temp = GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
        }
        if (hero.equipment[6] == null)
        {
            GameObject.Find(baseEquipPath + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color = temp;
        } else
        {
            GameObject.Find(baseEquipPath + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = hero.equipment[6].name;
            GameObject.Find(baseEquipPath + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = hero.equipment[6].icon;

            Color temp = GameObject.Find(baseEquipPath + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color = temp;
        }
        if (hero.equipment[7] == null)
        {
            GameObject.Find(baseEquipPath + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = hero.equipment[7].name;
            GameObject.Find(baseEquipPath + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = hero.equipment[7].icon;

            Color temp = GameObject.Find(baseEquipPath + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
        }

    }

    void DrawEquipMenuStats(BaseHero hero)
    {
        //For HeroEquipPanel
        GameObject.Find("HeroEquipPanel/NameText").GetComponent<Text>().text = hero._Name; //Name text
        GameObject.Find("HeroEquipPanel/LevelText").GetComponent<Text>().text = hero.currentLevel.ToString(); //Level text
        GameObject.Find("HeroEquipPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.maxHP.ToString()); //HP text
        GameObject.Find("HeroEquipPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.maxMP.ToString()); //MP text

        EquipPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hero), 0, 1), EquipPanelHPProgressBar.transform.localScale.y);
        EquipPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hero), 0, 1), EquipPanelMPProgressBar.transform.localScale.y);

        //For EquipStatsPanel
        //STR-SPI
        GameObject.Find("EquipStatsPanel/BaseStrengthText").GetComponent<Text>().text = hero.currentStrength.ToString();
        GameObject.Find("EquipStatsPanel/BaseStaminaText").GetComponent<Text>().text = hero.currentStamina.ToString();
        GameObject.Find("EquipStatsPanel/BaseAgilityText").GetComponent<Text>().text = hero.currentAgility.ToString();
        GameObject.Find("EquipStatsPanel/BaseDexterityText").GetComponent<Text>().text = hero.currentDexterity.ToString();
        GameObject.Find("EquipStatsPanel/BaseIntelligenceText").GetComponent<Text>().text = hero.currentIntelligence.ToString();
        GameObject.Find("EquipStatsPanel/BaseSpiritText").GetComponent<Text>().text = hero.currentSpirit.ToString();

        GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = hero.currentStrength.ToString();
        GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = hero.currentStamina.ToString();
        GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = hero.currentAgility.ToString();
        GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = hero.currentDexterity.ToString();
        GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = hero.currentIntelligence.ToString();
        GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = hero.currentSpirit.ToString();

        //HP & MP
        GameObject.Find("EquipStatsPanel/BaseHPText").GetComponent<Text>().text = hero.maxHP.ToString();
        GameObject.Find("EquipStatsPanel/BaseMPText").GetComponent<Text>().text = hero.maxMP.ToString();

        GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = hero.maxHP.ToString();
        GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = hero.maxMP.ToString();

        //ATK-MDEF
        GameObject.Find("EquipStatsPanel/BaseAttackText").GetComponent<Text>().text = hero.currentATK.ToString();
        GameObject.Find("EquipStatsPanel/BaseMagicAttackText").GetComponent<Text>().text = hero.currentMATK.ToString();
        GameObject.Find("EquipStatsPanel/BaseDefenseText").GetComponent<Text>().text = hero.currentDEF.ToString();
        GameObject.Find("EquipStatsPanel/BaseMagicDefenseText").GetComponent<Text>().text = hero.currentMDEF.ToString();

        GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = hero.currentATK.ToString();
        GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = hero.currentMATK.ToString();
        GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = hero.currentDEF.ToString();
        GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = hero.currentMDEF.ToString();

        //Other stats
        GameObject.Find("EquipStatsPanel/BaseHitText").GetComponent<Text>().text = hero.GetHitChance(hero.currentHitRating, hero.currentAgility).ToString();
        GameObject.Find("EquipStatsPanel/BaseCritText").GetComponent<Text>().text = hero.GetCritChance(hero.currentCritRating, hero.currentDexterity).ToString();
        GameObject.Find("EquipStatsPanel/BaseMPRegenText").GetComponent<Text>().text = hero.GetRegen(hero.currentRegenRating, hero.currentSpirit).ToString();
        GameObject.Find("EquipStatsPanel/BaseMoveText").GetComponent<Text>().text = hero.GetMoveRating(hero.currentMoveRating, hero.currentDexterity).ToString();
        GameObject.Find("EquipStatsPanel/BaseDodgeText").GetComponent<Text>().text = hero.GetDodgeChance(hero.currentDodgeRating, hero.currentAgility).ToString();
        GameObject.Find("EquipStatsPanel/BaseBlockText").GetComponent<Text>().text = hero.GetBlockChance(hero.currentBlockRating).ToString();
        GameObject.Find("EquipStatsPanel/BaseParryText").GetComponent<Text>().text = hero.GetParryChance(hero.currentParryRating, hero.currentStrength, hero.currentDexterity).ToString();
        GameObject.Find("EquipStatsPanel/BaseThreatText").GetComponent<Text>().text = hero.GetThreatRating(hero.currentThreatRating).ToString();

        GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = hero.GetHitChance(hero.currentHitRating, hero.currentAgility).ToString();
        GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = hero.GetCritChance(hero.currentCritRating, hero.currentDexterity).ToString();
        GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = hero.GetRegen(hero.currentRegenRating, hero.currentSpirit).ToString();
        GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = hero.GetMoveRating(hero.currentMoveRating, hero.currentDexterity).ToString();
        GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = hero.GetDodgeChance(hero.currentDodgeRating, hero.currentAgility).ToString();
        GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = hero.GetBlockChance(hero.currentBlockRating).ToString();
        GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = hero.GetParryChance(hero.currentParryRating, hero.currentStrength, hero.currentDexterity).ToString();
        GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = hero.GetThreatRating(hero.currentThreatRating).ToString();
    }

    void DrawInitialArrows()
    {
        ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");

        ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");

        ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");

        ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
    }

    void ChangeArrow(GameObject GO, string arrow)
    {
        foreach (Image arrowImage in GO.GetComponentsInChildren<Image>())
        {
            Color temp = arrowImage.color;
            if (arrowImage.gameObject.name == arrow)
            {
                temp.a = 1f;
            } else
            {
                temp.a = 0f;
            }
            arrowImage.color = temp;
        }
    }

    void HideEquipMenu()
    {
        MainMenuCanvas.gameObject.SetActive(true);
        EquipMenuCanvas.gameObject.SetActive(false);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
    }

    public void ListEquipment()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        equipButtonClicked = buttonName;

        if (equipMode == "Equip")
        {
            inEquipList = true;
            List<Equipment> equipmentList = new List<Equipment>();
            List<Equipment> equipmentAccountedFor = new List<Equipment>();

            foreach (Transform child in GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform)
            {
                Destroy(child.gameObject);
            }

            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = "None";
            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;
            Color tempColor = NewEquipPanel.transform.GetChild(1).GetComponent<Image>().color;
            tempColor.a = 0f;
            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().color = tempColor;
            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = "";
            NewEquipPanel.transform.SetParent(EquipListSpacer, false);

            foreach (Item equipment in Inventory.instance.items)
            {
                if (equipment.GetType().ToString() == "Equipment")
                {
                    equipmentList.Add((Equipment)equipment);
                }
            }

            if (buttonName == "HeadButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "HEAD")
                    {
                        int equipCount = 0;
                        if (!equipmentAccountedFor.Contains(equip))
                        {
                            for (int i = 0; i < equipmentList.Count; i++)
                            {
                                if (equipmentList[i] == equip)
                                {
                                    equipCount++;
                                }
                            }

                            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
                            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = equip.name;
                            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = equip.icon;
                            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = equipCount.ToString();
                            NewEquipPanel.transform.SetParent(EquipListSpacer, false);
                            equipmentAccountedFor.Add(equip);
                        }
                    }
                }
            }

            if (buttonName == "WristsButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "WRISTS")
                    {
                        int equipCount = 0;
                        if (!equipmentAccountedFor.Contains(equip))
                        {
                            for (int i = 0; i < equipmentList.Count; i++)
                            {
                                if (equipmentList[i] == equip)
                                {
                                    equipCount++;
                                }
                            }

                            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
                            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = equip.name;
                            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = equip.icon;
                            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = equipCount.ToString();
                            NewEquipPanel.transform.SetParent(EquipListSpacer, false);
                            equipmentAccountedFor.Add(equip);
                        }
                    }
                }
            }

            if (buttonName == "ChestButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "CHEST")
                    {
                        int equipCount = 0;
                        if (!equipmentAccountedFor.Contains(equip))
                        {
                            for (int i = 0; i < equipmentList.Count; i++)
                            {
                                if (equipmentList[i] == equip)
                                {
                                    equipCount++;
                                }
                            }

                            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
                            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = equip.name;
                            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = equip.icon;
                            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = equipCount.ToString();
                            NewEquipPanel.transform.SetParent(EquipListSpacer, false);
                            equipmentAccountedFor.Add(equip);
                        }
                    }
                }
            }

            if (buttonName == "LegsButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "LEGS")
                    {
                        int equipCount = 0;
                        if (!equipmentAccountedFor.Contains(equip))
                        {
                            for (int i = 0; i < equipmentList.Count; i++)
                            {
                                if (equipmentList[i] == equip)
                                {
                                    equipCount++;
                                }
                            }

                            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
                            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = equip.name;
                            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = equip.icon;
                            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = equipCount.ToString();
                            NewEquipPanel.transform.SetParent(EquipListSpacer, false);
                            equipmentAccountedFor.Add(equip);
                        }
                    }
                }
            }

            if (buttonName == "FeetButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "FEET")
                    {
                        int equipCount = 0;
                        if (!equipmentAccountedFor.Contains(equip))
                        {
                            for (int i = 0; i < equipmentList.Count; i++)
                            {
                                if (equipmentList[i] == equip)
                                {
                                    equipCount++;
                                }
                            }

                            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
                            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = equip.name;
                            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = equip.icon;
                            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = equipCount.ToString();
                            NewEquipPanel.transform.SetParent(EquipListSpacer, false);
                            equipmentAccountedFor.Add(equip);
                        }
                    }
                }
            }

            if (buttonName == "RelicButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "RELIC")
                    {
                        int equipCount = 0;
                        if (!equipmentAccountedFor.Contains(equip))
                        {
                            for (int i = 0; i < equipmentList.Count; i++)
                            {
                                if (equipmentList[i] == equip)
                                {
                                    equipCount++;
                                }
                            }

                            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
                            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = equip.name;
                            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = equip.icon;
                            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = equipCount.ToString();
                            NewEquipPanel.transform.SetParent(EquipListSpacer, false);
                            equipmentAccountedFor.Add(equip);
                        }
                    }
                }
            }

            if (buttonName == "RightHandButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "RIGHTHAND")
                    {
                        int equipCount = 0;
                        if (!equipmentAccountedFor.Contains(equip))
                        {
                            for (int i = 0; i < equipmentList.Count; i++)
                            {
                                if (equipmentList[i] == equip)
                                {
                                    equipCount++;
                                }
                            }

                            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
                            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = equip.name;
                            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = equip.icon;
                            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = equipCount.ToString();
                            NewEquipPanel.transform.SetParent(EquipListSpacer, false);
                            equipmentAccountedFor.Add(equip);
                        }
                    }
                }
            }

            if (buttonName == "LeftHandButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "LEFTHAND")
                    {
                        int equipCount = 0;
                        if (!equipmentAccountedFor.Contains(equip))
                        {
                            for (int i = 0; i < equipmentList.Count; i++)
                            {
                                if (equipmentList[i] == equip)
                                {
                                    equipCount++;
                                }
                            }

                            NewEquipPanel = Instantiate(PrefabManager.Instance.equipPrefab);
                            NewEquipPanel.transform.GetChild(0).GetComponent<Text>().text = equip.name;
                            NewEquipPanel.transform.GetChild(1).GetComponent<Image>().sprite = equip.icon;
                            NewEquipPanel.transform.GetChild(2).GetComponent<Text>().text = equipCount.ToString();
                            NewEquipPanel.transform.SetParent(EquipListSpacer, false);
                            equipmentAccountedFor.Add(equip);
                        }
                    }
                }
            }
        }

        if (equipMode == "Remove")
        {
            string equipMenuBase = "GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipSlotsPanel/";

            foreach (Transform child in GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform)
            {
                Destroy(child.gameObject);
            }

            if (equipButtonClicked == "HeadButton")
            {
                HeroForEquipMenu.Unequip(0);

                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "ChestButton")
            {
                HeroForEquipMenu.Unequip(1);

                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "WristsButton")
            {
                HeroForEquipMenu.Unequip(2);

                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LegsButton")
            {
                HeroForEquipMenu.Unequip(3);

                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "FeetButton")
            {
                HeroForEquipMenu.Unequip(4);

                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RelicButton")
            {
                HeroForEquipMenu.Unequip(5);

                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RightHandButton")
            {
                HeroForEquipMenu.Unequip(6);

                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LeftHandButton")
            {
                HeroForEquipMenu.Unequip(7);

                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
            }

            UpdateHeroFromEquipmentStats();

            DrawEquipMenuStats(HeroForEquipMenu);

        }
    }

    public void ChangeEquipment(Equipment toEquip)
    {
        string equipMenuBase = "GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipSlotsPanel/";

        if (equipMode == "Equip")
        {
            if (toEquip == null)
            {
                foreach (Transform child in GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform)
                {
                    Destroy(child.gameObject);
                }

                if (equipButtonClicked == "HeadButton")
                {
                    HeroForEquipMenu.Unequip(0);

                    GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "ChestButton")
                {
                    HeroForEquipMenu.Unequip(1);

                    GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "WristsButton")
                {
                    HeroForEquipMenu.Unequip(2);

                    GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "LegsButton")
                {
                    HeroForEquipMenu.Unequip(3);

                    GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "FeetButton")
                {
                    HeroForEquipMenu.Unequip(4);

                    GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "RelicButton")
                {
                    HeroForEquipMenu.Unequip(5);

                    GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "RightHandButton")
                {
                    HeroForEquipMenu.Unequip(6);

                    GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "LeftHandButton")
                {
                    HeroForEquipMenu.Unequip(7);

                    GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
                }

                UpdateHeroFromEquipmentStats();

                DrawEquipMenuStats(HeroForEquipMenu);

                return;
            }

            if (equipButtonClicked == "HeadButton")
            {
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                HeroForEquipMenu.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "ChestButton")
            {
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                HeroForEquipMenu.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "WristsButton")
            {
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                HeroForEquipMenu.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LegsButton")
            {
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                HeroForEquipMenu.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "FeetButton")
            {
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                HeroForEquipMenu.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RelicButton")
            {
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                HeroForEquipMenu.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LeftHandButton")
            {
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                HeroForEquipMenu.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RightHandButton")
            {
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                HeroForEquipMenu.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color = temp;
            }

            foreach (Transform child in GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform)
            {
                Destroy(child.gameObject);
            }

            GameObject.Find("EquipMenuCanvas/EquipMenuPanel/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>().text = "";

            UpdateHeroFromEquipmentStats();

            DrawEquipMenuStats(HeroForEquipMenu);

            inEquipList = false;
        }
    }

    private void UpdateHeroFromEquipmentStats()
    {
        int tempStrength = 0, tempStamina = 0, tempAgility = 0, tempDexterity = 0, tempIntelligence = 0, tempSpirit = 0;
        int tempHP = 0, tempMP = 0;
        int tempATK = 0, tempMATK = 0, tempDEF = 0, tempMDEF = 0;
        int tempHit = 0, tempCrit = 0, tempMove = 0, tempRegen = 0;
        int tempDodge = 0, tempBlock = 0, tempParry = 0, tempThreat = 0;
        
        foreach (Equipment equipment in HeroForEquipMenu.equipment)
        {
            if (equipment != null)
            {
                tempStrength += equipment.Strength;
                tempStamina += equipment.Stamina;
                tempAgility += equipment.Agility;
                tempDexterity += equipment.Dexterity;
                tempIntelligence += equipment.Intelligence;
                tempSpirit += equipment.Spirit;

                tempHP += Mathf.RoundToInt(equipment.Stamina * .75f);
                tempMP += Mathf.RoundToInt(equipment.Intelligence * .5f);

                tempATK += equipment.ATK + Mathf.RoundToInt(equipment.Strength * .5f);
                tempMATK += equipment.MATK + Mathf.RoundToInt(equipment.Intelligence * .5f);
                tempDEF += equipment.DEF + Mathf.RoundToInt(equipment.Stamina * .6f);
                tempMDEF += equipment.MDEF + Mathf.RoundToInt(equipment.Stamina * .5f);

                tempHit += equipment.hit;
                tempCrit += equipment.crit;
                tempMove += equipment.move;
                tempRegen += equipment.regen;

                tempDodge += equipment.dodge;
                tempBlock += equipment.block;
                tempParry += equipment.parry;
                tempThreat += equipment.threat;
            }
        }

        HeroForEquipMenu.currentStrength = HeroForEquipMenu.baseStrength + tempStrength;
        HeroForEquipMenu.currentStamina = HeroForEquipMenu.baseStamina + tempStamina;
        HeroForEquipMenu.currentAgility = HeroForEquipMenu.baseAgility + tempAgility;
        HeroForEquipMenu.currentDexterity = HeroForEquipMenu.baseDexterity + tempDexterity;
        HeroForEquipMenu.currentIntelligence = HeroForEquipMenu.baseIntelligence + tempIntelligence;
        HeroForEquipMenu.currentSpirit = HeroForEquipMenu.baseSpirit + tempSpirit;

        HeroForEquipMenu.maxHP = HeroForEquipMenu.GetMaxHP(HeroForEquipMenu.baseHP) + tempHP;
        HeroForEquipMenu.maxMP = HeroForEquipMenu.GetMaxMP(HeroForEquipMenu.baseMP) + tempMP;

        HeroForEquipMenu.currentATK = HeroForEquipMenu.baseATK + tempATK;
        HeroForEquipMenu.currentMATK = HeroForEquipMenu.baseMATK + tempMATK;
        HeroForEquipMenu.currentDEF = HeroForEquipMenu.baseDEF + tempDEF;
        HeroForEquipMenu.currentMDEF = HeroForEquipMenu.baseMDEF + tempMDEF;

        HeroForEquipMenu.currentHitRating = HeroForEquipMenu.baseHitRating + tempHit;
        HeroForEquipMenu.currentCritRating = HeroForEquipMenu.baseCritRating + tempCrit;
        HeroForEquipMenu.currentMoveRating = HeroForEquipMenu.baseMoveRating + tempMove;
        HeroForEquipMenu.currentRegenRating = HeroForEquipMenu.baseRegenRating + tempRegen;

        HeroForEquipMenu.currentDodgeRating = HeroForEquipMenu.baseDodgeRating + tempDodge;
        HeroForEquipMenu.currentBlockRating = HeroForEquipMenu.baseBlockRating + tempBlock;
        HeroForEquipMenu.currentParryRating = HeroForEquipMenu.baseParryRating + tempParry;
        HeroForEquipMenu.currentThreatRating = HeroForEquipMenu.baseThreatRating + tempThreat;

        ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");
        
        ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");
        
        ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");
        
        ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
        
        ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
    }

    public void ShowEquipmentStatUpdates(Equipment toEquip)
    {
        int tempStrength = HeroForEquipMenu.baseStrength, tempStamina = HeroForEquipMenu.baseStamina, tempAgility = HeroForEquipMenu.baseAgility, 
            tempDexterity = HeroForEquipMenu.baseDexterity, tempIntelligence = HeroForEquipMenu.baseIntelligence, tempSpirit = HeroForEquipMenu.baseSpirit;
        int tempHP = HeroForEquipMenu.GetMaxHP(HeroForEquipMenu.baseHP), tempMP = HeroForEquipMenu.GetMaxMP(HeroForEquipMenu.baseMP);
        int tempATK = HeroForEquipMenu.baseATK, tempMATK = HeroForEquipMenu.baseMATK, tempDEF = HeroForEquipMenu.baseDEF, tempMDEF = HeroForEquipMenu.baseMDEF;
        int tempHit = HeroForEquipMenu.baseHitRating, tempCrit = HeroForEquipMenu.baseCritRating, tempMove = HeroForEquipMenu.baseMoveRating, tempRegen = HeroForEquipMenu.baseRegenRating;
        int tempDodge = HeroForEquipMenu.baseDodgeRating, tempBlock = HeroForEquipMenu.baseBlockRating, tempParry = HeroForEquipMenu.baseParryRating, tempThreat = HeroForEquipMenu.baseThreatRating;

        foreach (Equipment equipment in HeroForEquipMenu.equipment)
        {
            if (equipment != null && toEquip == null) //if choosing "None", skip this equipment in loop.
            {
                if (
                    equipment.equipmentSlot.ToString() == "HEAD" && equipButtonClicked == "HeadButton" ||
                    equipment.equipmentSlot.ToString() == "WRISTS" && equipButtonClicked == "WristsButton" ||
                    equipment.equipmentSlot.ToString() == "CHEST" && equipButtonClicked == "Chestbutton" ||
                    equipment.equipmentSlot.ToString() == "LEGS" && equipButtonClicked == "LegsButton" ||
                    equipment.equipmentSlot.ToString() == "FEET" && equipButtonClicked == "FeetButton" ||
                    equipment.equipmentSlot.ToString() == "RELIC" && equipButtonClicked == "RelicButton" ||
                    equipment.equipmentSlot.ToString() == "LEFTHAND" && equipButtonClicked == "LeftHandButton" ||
                    equipment.equipmentSlot.ToString() == "RIGHTHAND" && equipButtonClicked == "RightHandButton"
                    )
                {
                    continue;
                } else
                {
                    tempStrength += equipment.Strength;
                    tempStamina += equipment.Stamina;
                    tempAgility += equipment.Agility;
                    tempDexterity += equipment.Dexterity;
                    tempIntelligence += equipment.Intelligence;
                    tempSpirit += equipment.Spirit;

                    tempHP += Mathf.RoundToInt(equipment.Stamina * .75f);
                    tempMP += Mathf.RoundToInt(equipment.Intelligence * .5f);

                    tempATK += equipment.ATK + Mathf.RoundToInt(equipment.Strength * .5f);
                    tempMATK += equipment.MATK + Mathf.RoundToInt(equipment.Intelligence * .5f);
                    tempDEF += equipment.DEF + Mathf.RoundToInt(equipment.Stamina * .6f);
                    tempMDEF += equipment.MDEF + Mathf.RoundToInt(equipment.Stamina * .5f);

                    tempHit += equipment.hit;
                    tempCrit += equipment.crit;
                    tempMove += equipment.move;
                    tempRegen += equipment.regen;

                    tempDodge += equipment.dodge;
                    tempBlock += equipment.block;
                    tempParry += equipment.parry;
                    tempThreat += equipment.threat;
                    continue;
                }
                
            }

            if (equipment != null && equipment.equipmentSlot != toEquip.equipmentSlot)
            {
                tempStrength += equipment.Strength;
                tempStamina += equipment.Stamina;
                tempAgility += equipment.Agility;
                tempDexterity += equipment.Dexterity;
                tempIntelligence += equipment.Intelligence;
                tempSpirit += equipment.Spirit;

                tempHP += Mathf.RoundToInt(equipment.Stamina * .75f);
                tempMP += Mathf.RoundToInt(equipment.Intelligence * .5f);

                tempATK += equipment.ATK + Mathf.RoundToInt(equipment.Strength * .5f);
                tempMATK += equipment.MATK + Mathf.RoundToInt(equipment.Intelligence * .5f);
                tempDEF += equipment.DEF + Mathf.RoundToInt(equipment.Stamina * .6f);
                tempMDEF += equipment.MDEF + Mathf.RoundToInt(equipment.Stamina * .5f);

                tempHit += equipment.hit;
                tempCrit += equipment.crit;
                tempMove += equipment.move;
                tempRegen += equipment.regen;

                tempDodge += equipment.dodge;
                tempBlock += equipment.block;
                tempParry += equipment.parry;
                tempThreat += equipment.threat;
            }
        }

        if (toEquip == null) //if choosing "None" (or unequipping the slot)
        {
            GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = tempStrength.ToString();
            if (tempStrength > HeroForEquipMenu.currentStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Up");
            }
            else if (tempStrength < HeroForEquipMenu.currentStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Down");
            }
            else if (tempStrength == HeroForEquipMenu.currentStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = tempStamina.ToString();
            if (tempStamina > HeroForEquipMenu.currentStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Up");
            }
            else if (tempStamina < HeroForEquipMenu.currentStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Down");
            }
            else if (tempStamina == HeroForEquipMenu.currentStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = tempAgility.ToString();
            if (tempAgility > HeroForEquipMenu.currentAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Up");
            }
            if (tempAgility < HeroForEquipMenu.currentAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Down");
            }
            else if (tempAgility == HeroForEquipMenu.currentAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = tempDexterity.ToString();
            if (tempDexterity > HeroForEquipMenu.currentDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Up");
            }
            else if (tempDexterity < HeroForEquipMenu.currentDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Down");
            }
            else if (tempDexterity == HeroForEquipMenu.currentDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = tempIntelligence.ToString();
            if (tempIntelligence > HeroForEquipMenu.currentIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Up");
            }
            else if (tempIntelligence < HeroForEquipMenu.currentIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Down");
            }
            else if (tempIntelligence == HeroForEquipMenu.currentIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = tempSpirit.ToString();
            if (tempSpirit > HeroForEquipMenu.currentSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Up");
            }
            else if (tempSpirit < HeroForEquipMenu.currentSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Down");
            }
            else if (tempSpirit == HeroForEquipMenu.currentSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = tempHP.ToString();
            if (tempHP > HeroForEquipMenu.maxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Up");
            }
            else if (tempHP < HeroForEquipMenu.maxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Down");
            }
            else if (tempHP == HeroForEquipMenu.maxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = tempMP.ToString();
            if (tempMP > HeroForEquipMenu.maxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Up");
            }
            else if (tempMP < HeroForEquipMenu.maxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Down");
            }
            else if (tempMP == HeroForEquipMenu.maxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = tempATK.ToString();
            if (tempATK > HeroForEquipMenu.currentATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Up");
            }
            else if (tempATK < HeroForEquipMenu.currentATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Down");
            }
            else if (tempATK == HeroForEquipMenu.currentATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = tempMATK.ToString();
            if (tempMATK > HeroForEquipMenu.currentMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Up");
            }
            else if (tempMATK < HeroForEquipMenu.currentMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Down");
            }
            else if (tempMATK == HeroForEquipMenu.currentMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = tempDEF.ToString();
            if (tempDEF > HeroForEquipMenu.currentDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Up");
            }
            else if (tempDEF < HeroForEquipMenu.currentDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Down");
            }
            else if (tempDEF == HeroForEquipMenu.currentDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = tempMDEF.ToString();
            if (tempMDEF > HeroForEquipMenu.currentMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Up");
            }
            else if (tempMDEF < HeroForEquipMenu.currentMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Down");
            }
            else if (tempMDEF == HeroForEquipMenu.currentMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = HeroForEquipMenu.GetHitChance(tempHit, tempAgility).ToString();
            if (HeroForEquipMenu.GetHitChance(tempHit, tempAgility) > HeroForEquipMenu.GetHitChance(HeroForEquipMenu.currentHitRating, HeroForEquipMenu.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Up");
            }
            else if (HeroForEquipMenu.GetHitChance(tempHit, tempAgility) < HeroForEquipMenu.GetHitChance(HeroForEquipMenu.currentHitRating, HeroForEquipMenu.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Down");
            }
            else if (HeroForEquipMenu.GetHitChance(tempHit, tempAgility) == HeroForEquipMenu.GetHitChance(HeroForEquipMenu.currentHitRating, HeroForEquipMenu.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = HeroForEquipMenu.GetCritChance(tempCrit, tempDexterity).ToString();
            if (HeroForEquipMenu.GetCritChance(tempCrit, tempDexterity) > HeroForEquipMenu.GetCritChance(HeroForEquipMenu.currentCritRating, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Up");
            }
            else if (HeroForEquipMenu.GetCritChance(tempCrit, tempDexterity) < HeroForEquipMenu.GetCritChance(HeroForEquipMenu.currentCritRating, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Down");
            }
            else if (HeroForEquipMenu.GetCritChance(tempCrit, tempDexterity) == HeroForEquipMenu.GetCritChance(HeroForEquipMenu.currentCritRating, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = HeroForEquipMenu.GetMoveRating(tempMove, tempDexterity).ToString();
            if (HeroForEquipMenu.GetMoveRating(tempMove, tempDexterity) > HeroForEquipMenu.GetMoveRating(HeroForEquipMenu.currentMoveRating, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Up");
            }
            else if (HeroForEquipMenu.GetMoveRating(tempMove, tempDexterity) < HeroForEquipMenu.GetMoveRating(HeroForEquipMenu.currentMoveRating, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Down");
            }
            else if (HeroForEquipMenu.GetMoveRating(tempMove, tempDexterity) == HeroForEquipMenu.GetMoveRating(HeroForEquipMenu.currentMoveRating, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = HeroForEquipMenu.GetRegen(tempRegen, tempSpirit).ToString();
            if (HeroForEquipMenu.GetRegen(tempRegen, tempSpirit) > HeroForEquipMenu.GetRegen(HeroForEquipMenu.currentRegenRating, HeroForEquipMenu.currentSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Up");
            }
            else if (HeroForEquipMenu.GetRegen(tempRegen, tempSpirit) < HeroForEquipMenu.GetRegen(HeroForEquipMenu.currentRegenRating, HeroForEquipMenu.currentSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Down");
            }
            else if (HeroForEquipMenu.GetRegen(tempRegen, tempSpirit) == HeroForEquipMenu.GetRegen(HeroForEquipMenu.currentRegenRating, HeroForEquipMenu.currentSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = HeroForEquipMenu.GetDodgeChance(tempDodge, tempAgility).ToString();
            if (HeroForEquipMenu.GetDodgeChance(tempDodge, tempAgility) > HeroForEquipMenu.GetDodgeChance(HeroForEquipMenu.currentDodgeRating, HeroForEquipMenu.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Up");
            }
            else if (HeroForEquipMenu.GetDodgeChance(tempDodge, tempAgility) < HeroForEquipMenu.GetDodgeChance(HeroForEquipMenu.currentDodgeRating, HeroForEquipMenu.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Down");
            }
            else if (HeroForEquipMenu.GetDodgeChance(tempDodge, tempAgility) == HeroForEquipMenu.GetDodgeChance(HeroForEquipMenu.currentDodgeRating, HeroForEquipMenu.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = HeroForEquipMenu.GetBlockChance(tempBlock).ToString();
            if (HeroForEquipMenu.GetBlockChance(tempBlock) > HeroForEquipMenu.GetBlockChance(HeroForEquipMenu.currentBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Up");
            }
            else if (HeroForEquipMenu.GetBlockChance(tempBlock) < HeroForEquipMenu.GetBlockChance(HeroForEquipMenu.currentBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Down");
            }
            else if (HeroForEquipMenu.GetBlockChance(tempBlock) == HeroForEquipMenu.GetBlockChance(HeroForEquipMenu.currentBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = HeroForEquipMenu.GetParryChance(tempParry, tempStrength, tempDexterity).ToString();
            if (HeroForEquipMenu.GetParryChance(tempParry, tempStrength, tempDexterity) > HeroForEquipMenu.GetParryChance(HeroForEquipMenu.currentParryRating, HeroForEquipMenu.currentStrength, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Up");
            }
            else if (HeroForEquipMenu.GetParryChance(tempParry, tempStrength, tempDexterity) < HeroForEquipMenu.GetParryChance(HeroForEquipMenu.currentParryRating, HeroForEquipMenu.currentStrength, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Down");
            }
            else if (HeroForEquipMenu.GetParryChance(tempParry, tempStrength, tempDexterity) == HeroForEquipMenu.GetParryChance(HeroForEquipMenu.currentParryRating, HeroForEquipMenu.currentStrength, HeroForEquipMenu.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = HeroForEquipMenu.GetThreatRating(tempThreat).ToString();
            if (HeroForEquipMenu.GetThreatRating(tempThreat) > HeroForEquipMenu.GetThreatRating(HeroForEquipMenu.currentThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Up");
            }
            else if (HeroForEquipMenu.GetThreatRating(tempThreat) < HeroForEquipMenu.GetThreatRating(HeroForEquipMenu.currentThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Down");
            }
            else if (HeroForEquipMenu.GetThreatRating(tempThreat) == HeroForEquipMenu.GetThreatRating(HeroForEquipMenu.currentThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
            }

            return;
        }

        GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = (tempStrength + toEquip.Strength).ToString();
        if ((tempStrength + toEquip.Strength) > HeroForEquipMenu.currentStrength)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Up");
        } else if ((tempStrength + toEquip.Strength) < HeroForEquipMenu.currentStrength)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Down");
        } else if ((tempStrength + toEquip.Strength) == HeroForEquipMenu.currentStrength)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = (tempStamina + toEquip.Stamina).ToString();
        if ((tempStamina + toEquip.Stamina) > HeroForEquipMenu.currentStamina)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Up");
        }
        else if ((tempStamina + toEquip.Stamina) < HeroForEquipMenu.currentStamina)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Down");
        }
        else if ((tempStamina + toEquip.Stamina) == HeroForEquipMenu.currentStamina)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = (tempAgility + toEquip.Agility).ToString();
        if ((tempAgility + toEquip.Agility) > HeroForEquipMenu.currentAgility)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Up");
        }
        else if ((tempAgility + toEquip.Agility) < HeroForEquipMenu.currentAgility)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Down");
        }
        else if ((tempAgility + toEquip.Agility) == HeroForEquipMenu.currentAgility)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = (tempDexterity + toEquip.Dexterity).ToString();
        if ((tempDexterity + toEquip.Dexterity) > HeroForEquipMenu.currentDexterity)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Up");
        }
        else if ((tempDexterity + toEquip.Dexterity) < HeroForEquipMenu.currentDexterity)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Down");
        }
        else if ((tempDexterity + toEquip.Dexterity) == HeroForEquipMenu.currentDexterity)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = (tempIntelligence + toEquip.Intelligence).ToString();
        if ((tempIntelligence + toEquip.Intelligence) > HeroForEquipMenu.currentIntelligence)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Up");
        }
        else if ((tempIntelligence + toEquip.Intelligence) < HeroForEquipMenu.currentIntelligence)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Down");
        }
        else if ((tempIntelligence + toEquip.Intelligence) == HeroForEquipMenu.currentIntelligence)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = (tempSpirit + toEquip.Spirit).ToString();
        if ((tempSpirit + toEquip.Spirit) > HeroForEquipMenu.currentSpirit)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Up");
        }
        else if ((tempSpirit + toEquip.Spirit) < HeroForEquipMenu.currentSpirit)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Down");
        }
        else if ((tempSpirit + toEquip.Spirit) == HeroForEquipMenu.currentSpirit)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");
        }


        GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = (tempHP + Mathf.RoundToInt(toEquip.Stamina * .75f)).ToString();
        if ((tempHP + Mathf.RoundToInt(toEquip.Stamina * .75f)) > HeroForEquipMenu.maxHP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Up");
        }
        else if ((tempHP + Mathf.RoundToInt(toEquip.Stamina * .75f)) < HeroForEquipMenu.maxHP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Down");
        }
        else if ((tempHP + Mathf.RoundToInt(toEquip.Stamina * .75f)) == HeroForEquipMenu.maxHP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = (tempMP + Mathf.RoundToInt(toEquip.Intelligence * .5f)).ToString();
        if ((tempMP + Mathf.RoundToInt(toEquip.Intelligence * .5f)) > HeroForEquipMenu.maxMP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Up");
        }
        else if ((tempMP + Mathf.RoundToInt(toEquip.Intelligence * .5f)) < HeroForEquipMenu.maxMP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Down");
        }
        else if ((tempMP + Mathf.RoundToInt(toEquip.Intelligence * .5f)) == HeroForEquipMenu.maxMP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");
        }


        GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = (tempATK + toEquip.ATK + Mathf.RoundToInt(toEquip.Strength * .5f)).ToString();
        if ((tempATK + toEquip.ATK + Mathf.RoundToInt(toEquip.Strength * .5f)) > HeroForEquipMenu.currentATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Up");
        }
        else if ((tempATK + toEquip.ATK + Mathf.RoundToInt(toEquip.Strength * .5f)) < HeroForEquipMenu.currentATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Down");
        }
        else if ((tempATK + toEquip.ATK + Mathf.RoundToInt(toEquip.Strength * .5f)) == HeroForEquipMenu.currentATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = (tempMATK + toEquip.MATK + Mathf.RoundToInt(toEquip.Intelligence * .5f)).ToString();
        if ((tempMATK + toEquip.MATK + Mathf.RoundToInt(toEquip.Intelligence * .5f)) > HeroForEquipMenu.currentMATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Up");
        }
        else if ((tempMATK + toEquip.MATK + Mathf.RoundToInt(toEquip.Intelligence * .5f)) < HeroForEquipMenu.currentMATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Down");
        }
        else if ((tempMATK + toEquip.MATK + Mathf.RoundToInt(toEquip.Intelligence * .5f)) == HeroForEquipMenu.currentMATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = (tempDEF + toEquip.DEF + Mathf.RoundToInt(toEquip.Stamina * .6f)).ToString();
        if ((tempDEF + toEquip.DEF + Mathf.RoundToInt(toEquip.Stamina * .6f)) > HeroForEquipMenu.currentDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Up");
        }
        else if ((tempDEF + toEquip.DEF + Mathf.RoundToInt(toEquip.Stamina * .6f)) < HeroForEquipMenu.currentDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Down");
        }
        else if ((tempDEF + toEquip.DEF + Mathf.RoundToInt(toEquip.Stamina * .6f)) == HeroForEquipMenu.currentDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = (tempMDEF + toEquip.MDEF + Mathf.RoundToInt(toEquip.Stamina * .5f)).ToString();
        if ((tempMDEF + toEquip.MDEF + Mathf.RoundToInt(toEquip.Stamina * .5f)) > HeroForEquipMenu.currentMDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Up");
        }
        else if ((tempMDEF + toEquip.MDEF + Mathf.RoundToInt(toEquip.Stamina * .5f)) < HeroForEquipMenu.currentMDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Down");
        }
        else if ((tempMDEF + toEquip.MDEF + Mathf.RoundToInt(toEquip.Stamina * .5f)) == HeroForEquipMenu.currentMDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");
        }


        GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = (HeroForEquipMenu.GetHitChance((tempHit + toEquip.hit),(tempAgility + toEquip.Agility))).ToString();
        if ((HeroForEquipMenu.GetHitChance((tempHit + toEquip.hit), (tempAgility + toEquip.Agility))) > HeroForEquipMenu.GetHitChance(HeroForEquipMenu.currentHitRating, HeroForEquipMenu.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Up");
        }
        else if ((HeroForEquipMenu.GetHitChance((tempHit + toEquip.hit), (tempAgility + toEquip.Agility))) < HeroForEquipMenu.GetHitChance(HeroForEquipMenu.currentHitRating, HeroForEquipMenu.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Down");
        }
        else if ((HeroForEquipMenu.GetHitChance((tempHit + toEquip.hit), (tempAgility + toEquip.Agility))) == HeroForEquipMenu.GetHitChance(HeroForEquipMenu.currentHitRating, HeroForEquipMenu.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = (HeroForEquipMenu.GetCritChance((tempCrit + toEquip.crit), (tempDexterity + toEquip.Dexterity))).ToString();
        if ((HeroForEquipMenu.GetCritChance((tempCrit + toEquip.crit), (tempDexterity + toEquip.Dexterity))) > HeroForEquipMenu.GetCritChance(HeroForEquipMenu.currentCritRating, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Up");
        }
        else if ((HeroForEquipMenu.GetCritChance((tempCrit + toEquip.crit), (tempDexterity + toEquip.Dexterity))) < HeroForEquipMenu.GetCritChance(HeroForEquipMenu.currentCritRating, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Down");
        }
        else if ((HeroForEquipMenu.GetCritChance((tempCrit + toEquip.crit), (tempDexterity + toEquip.Dexterity))) == HeroForEquipMenu.GetCritChance(HeroForEquipMenu.currentCritRating, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = (HeroForEquipMenu.GetMoveRating((tempMove + toEquip.move), (tempDexterity + toEquip.Dexterity))).ToString();
        if ((HeroForEquipMenu.GetMoveRating((tempMove + toEquip.move), (tempDexterity + toEquip.Dexterity))) > HeroForEquipMenu.GetMoveRating(HeroForEquipMenu.currentMoveRating, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Up");
        }
        else if ((HeroForEquipMenu.GetMoveRating((tempMove + toEquip.move), (tempDexterity + toEquip.Dexterity))) < HeroForEquipMenu.GetMoveRating(HeroForEquipMenu.currentMoveRating, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Down");
        }
        else if ((HeroForEquipMenu.GetMoveRating((tempMove + toEquip.move), (tempDexterity + toEquip.Dexterity))) == HeroForEquipMenu.GetMoveRating(HeroForEquipMenu.currentMoveRating, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = (HeroForEquipMenu.GetRegen((tempRegen + toEquip.regen), (tempSpirit + toEquip.Spirit))).ToString();
        if ((HeroForEquipMenu.GetRegen((tempRegen + toEquip.regen), (tempSpirit + toEquip.Spirit))) > HeroForEquipMenu.GetRegen(HeroForEquipMenu.currentRegenRating, HeroForEquipMenu.currentSpirit))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Up");
        }
        else if ((HeroForEquipMenu.GetRegen((tempRegen + toEquip.regen), (tempSpirit + toEquip.Spirit))) < HeroForEquipMenu.GetRegen(HeroForEquipMenu.currentRegenRating, HeroForEquipMenu.currentSpirit))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Down");
        }
        else if ((HeroForEquipMenu.GetRegen((tempRegen + toEquip.regen), (tempSpirit + toEquip.Spirit))) == HeroForEquipMenu.GetRegen(HeroForEquipMenu.currentRegenRating, HeroForEquipMenu.currentSpirit))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = (HeroForEquipMenu.GetDodgeChance((tempDodge + toEquip.dodge), (tempAgility + toEquip.Agility))).ToString();
        if ((HeroForEquipMenu.GetDodgeChance((tempDodge + toEquip.dodge), (tempAgility + toEquip.Agility))) > HeroForEquipMenu.GetDodgeChance(HeroForEquipMenu.currentDodgeRating, HeroForEquipMenu.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Up");
        }
        else if ((HeroForEquipMenu.GetDodgeChance((tempDodge + toEquip.dodge), (tempAgility + toEquip.Agility))) < HeroForEquipMenu.GetDodgeChance(HeroForEquipMenu.currentDodgeRating, HeroForEquipMenu.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Down");
        }
        else if ((HeroForEquipMenu.GetDodgeChance((tempDodge + toEquip.dodge), (tempAgility + toEquip.Agility))) == HeroForEquipMenu.GetDodgeChance(HeroForEquipMenu.currentDodgeRating, HeroForEquipMenu.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = (HeroForEquipMenu.GetBlockChance((tempBlock + toEquip.block))).ToString();
        if ((HeroForEquipMenu.GetBlockChance((tempBlock + toEquip.block))) > HeroForEquipMenu.GetBlockChance(HeroForEquipMenu.currentBlockRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Up");
        }
        else if ((HeroForEquipMenu.GetBlockChance((tempBlock + toEquip.block))) < HeroForEquipMenu.GetBlockChance(HeroForEquipMenu.currentBlockRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Down");
        }
        else if ((HeroForEquipMenu.GetBlockChance((tempBlock + toEquip.block))) == HeroForEquipMenu.GetBlockChance(HeroForEquipMenu.currentBlockRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = (HeroForEquipMenu.GetParryChance((tempParry + toEquip.parry), (tempStrength + toEquip.Strength), (tempDexterity + toEquip.Dexterity))).ToString();
        if ((HeroForEquipMenu.GetParryChance((tempParry + toEquip.parry), (tempStrength + toEquip.Strength), (tempDexterity + toEquip.Dexterity))) > HeroForEquipMenu.GetParryChance(HeroForEquipMenu.currentParryRating, HeroForEquipMenu.currentStrength, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Up");
        }
        else if ((HeroForEquipMenu.GetParryChance((tempParry + toEquip.parry), (tempStrength + toEquip.Strength), (tempDexterity + toEquip.Dexterity))) < HeroForEquipMenu.GetParryChance(HeroForEquipMenu.currentParryRating, HeroForEquipMenu.currentStrength, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Down");
        }
        else if ((HeroForEquipMenu.GetParryChance((tempParry + toEquip.parry), (tempStrength + toEquip.Strength), (tempDexterity + toEquip.Dexterity))) == HeroForEquipMenu.GetParryChance(HeroForEquipMenu.currentParryRating, HeroForEquipMenu.currentStrength, HeroForEquipMenu.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = (HeroForEquipMenu.GetThreatRating(tempThreat + toEquip.threat)).ToString();
        if ((HeroForEquipMenu.GetThreatRating(tempThreat + toEquip.threat)) > HeroForEquipMenu.GetThreatRating(HeroForEquipMenu.currentThreatRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Up");
        }
        else if ((HeroForEquipMenu.GetThreatRating(tempThreat + toEquip.threat)) < HeroForEquipMenu.GetThreatRating(HeroForEquipMenu.currentThreatRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Down");
        }
        else if ((HeroForEquipMenu.GetThreatRating(tempThreat + toEquip.threat)) == HeroForEquipMenu.GetThreatRating(HeroForEquipMenu.currentThreatRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
        }
    }

    public void ResetEquipmentStatUpdates()
    {
        GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = HeroForEquipMenu.currentStrength.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = HeroForEquipMenu.currentStamina.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = HeroForEquipMenu.currentAgility.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = HeroForEquipMenu.currentDexterity.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = HeroForEquipMenu.currentIntelligence.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = HeroForEquipMenu.currentSpirit.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");

        GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = HeroForEquipMenu.maxHP.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = HeroForEquipMenu.maxMP.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");

        GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = HeroForEquipMenu.currentATK.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = HeroForEquipMenu.currentMATK.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = HeroForEquipMenu.currentDEF.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = HeroForEquipMenu.currentMDEF.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");

        GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = HeroForEquipMenu.GetHitChance(HeroForEquipMenu.currentHitRating, HeroForEquipMenu.currentAgility).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = HeroForEquipMenu.GetCritChance(HeroForEquipMenu.currentCritRating, HeroForEquipMenu.currentDexterity).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = HeroForEquipMenu.GetMoveRating(HeroForEquipMenu.currentMoveRating, HeroForEquipMenu.currentDexterity).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = HeroForEquipMenu.GetRegen(HeroForEquipMenu.currentRegenRating, HeroForEquipMenu.currentSpirit).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");

        GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = HeroForEquipMenu.GetDodgeChance(HeroForEquipMenu.currentDodgeRating, HeroForEquipMenu.currentAgility).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = HeroForEquipMenu.GetBlockChance(HeroForEquipMenu.currentBlockRating).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = HeroForEquipMenu.GetParryChance(HeroForEquipMenu.currentParryRating, HeroForEquipMenu.currentStrength, HeroForEquipMenu.currentDexterity).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = HeroForEquipMenu.GetThreatRating(HeroForEquipMenu.currentThreatRating).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");

    }

    void CancelFromEquipList()
    {
        equipButtonClicked = null;

        foreach (Transform child in GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ChangeEquipMode(string mode)
    {
        if (mode == "Equip")
        {
            equipMode = "Equip";
            Debug.Log("equipMode: Equip");
            GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipOptionsPanel/EquipOptionButton/EquipButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
            GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipOptionsPanel/RemoveOptionButton/RemoveButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        if (mode == "Remove")
        {
            equipMode = "Remove";
            Debug.Log("equipMode: Remove");
            GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipOptionsPanel/EquipOptionButton/EquipButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
            GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipOptionsPanel/RemoveOptionButton/RemoveButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        }
    }

    //--------------------

    //Status Menu

    public void ShowStatusMenu() //displays status menu
    {
        Debug.Log("Status button clicked - choose a hero");
        StartCoroutine(ChooseHeroForStatusMenu());
    }

    IEnumerator ChooseHeroForStatusMenu()
    {
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }

        DrawStatusMenu(heroToCheck);
    }

    void DrawStatusMenu(BaseHero hero)
    {
        MainMenuCanvas.gameObject.SetActive(false);
        StatusMenuCanvas.gameObject.SetActive(true);
        menuState = MenuStates.STATUS;
        DrawStatusMenuBaseStats(hero);
        DrawStatusMenuStats(hero);
    }

    void DrawStatusMenuBaseStats(BaseHero hero)
    {
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/NameText").GetComponent<Text>().text = hero._Name; //Name text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/LevelText").GetComponent<Text>().text = hero.currentLevel.ToString(); //Level text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.maxHP.ToString()); //HP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.maxMP.ToString()); //MP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/EXPText").GetComponent<Text>().text = (hero.currentExp + " / " + GameManager.instance.toLevelUp[hero.currentLevel - 1]); //EXP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/ToNextLevelText").GetComponent<Text>().text = (GameManager.instance.toLevelUp[hero.currentLevel - 1] - hero.currentExp).ToString(); //To next level text

        StatusPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hero), 0, 1), StatusPanelHPProgressBar.transform.localScale.y);
        StatusPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hero), 0, 1), StatusPanelMPProgressBar.transform.localScale.y);
        StatusPanelEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(hero), 0, 1), StatusPanelEXPProgressBar.transform.localScale.y);
    }

    void DrawStatusMenuStats(BaseHero hero)
    {
        GameObject.Find("StatusMenuPanel/StatsPanel/StrengthText").GetComponent<Text>().text = hero.currentStrength.ToString(); //Strength text
        GameObject.Find("StatusMenuPanel/StatsPanel/StaminaText").GetComponent<Text>().text = hero.currentStamina.ToString(); //Stamina text
        GameObject.Find("StatusMenuPanel/StatsPanel/AgilityText").GetComponent<Text>().text = hero.currentAgility.ToString(); //Agility text
        GameObject.Find("StatusMenuPanel/StatsPanel/DexterityText").GetComponent<Text>().text = hero.currentDexterity.ToString(); //Dexterity text
        GameObject.Find("StatusMenuPanel/StatsPanel/IntelligenceText").GetComponent<Text>().text = hero.currentIntelligence.ToString(); //Intelligence text
        GameObject.Find("StatusMenuPanel/StatsPanel/SpiritText").GetComponent<Text>().text = hero.currentSpirit.ToString(); //Spirit text

        GameObject.Find("StatusMenuPanel/StatsPanel/AttackText").GetComponent<Text>().text = hero.currentATK.ToString(); //Attack text
        GameObject.Find("StatusMenuPanel/StatsPanel/MagicAttackText").GetComponent<Text>().text = hero.currentMATK.ToString(); //Magic Attack text
        GameObject.Find("StatusMenuPanel/StatsPanel/DefenseText").GetComponent<Text>().text = hero.currentDEF.ToString(); //Defense text
        GameObject.Find("StatusMenuPanel/StatsPanel/MagicDefenseText").GetComponent<Text>().text = hero.currentMDEF.ToString(); //Magic Defense text

        GameObject.Find("StatusMenuPanel/StatsPanel/HitChanceText").GetComponent<Text>().text = hero.GetHitChance(hero.currentHitRating, hero.currentAgility).ToString(); //Hit Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/CritChanceText").GetComponent<Text>().text = hero.GetCritChance(hero.currentCritRating, hero.currentDexterity).ToString(); //Crit Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/MoveRatingText").GetComponent<Text>().text = hero.GetMoveRating(hero.currentMoveRating, hero.currentDexterity).ToString(); //Move Rating text
        GameObject.Find("StatusMenuPanel/StatsPanel/MPPerTurnText").GetComponent<Text>().text = hero.GetRegen(hero.currentRegenRating, hero.currentSpirit).ToString(); //MP Regen text
        GameObject.Find("StatusMenuPanel/StatsPanel/DodgeChanceText").GetComponent<Text>().text = hero.GetDodgeChance(hero.currentDodgeRating, hero.currentAgility).ToString(); //Dodge Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/BlockChanceText").GetComponent<Text>().text = hero.GetBlockChance(hero.currentBlockRating).ToString(); //Block Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/ParryChanceText").GetComponent<Text>().text = hero.GetParryChance(hero.currentParryRating, hero.currentStrength, hero.currentDexterity).ToString(); //Parry Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/ThreatText").GetComponent<Text>().text = hero.GetThreatRating(hero.currentThreatRating).ToString(); //Threat Rating text
    }

    void HideStatusMenu()
    {
        MainMenuCanvas.gameObject.SetActive(true);
        StatusMenuCanvas.gameObject.SetActive(false);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
    }

    //--------------------

    //Party Menu

    //--------------------

    //Order Menu

    //--------------------

    //Grid Menu

    //--------------------

    //Config Menu

    //--------------------

    //Quit Menu

    //--------------------

    //Methods for running above menus

    void PauseBackground(bool pause) //keeps background objects from processing while pause=true by enabling the script's inMenu
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        player = GameObject.Find("Player");
        if (pause)
        {
            player.GetComponent<PlayerController2D>().enabled = false;
            foreach (GameObject GO in allObjects)
            {
                if (GO.GetComponent<DialogueEvents>() != null)
                {
                    foreach (BaseScriptedEvent script in GO.GetComponent<DialogueEvents>().activeScripts)
                    {
                        script.inMenu = true;
                    }
                }
            }
        }
        else
        {
            player.GetComponent<PlayerController2D>().enabled = true;
            foreach (GameObject GO in allObjects)
            {
                if (GO.GetComponent<DialogueEvents>() != null)
                {
                    foreach (BaseScriptedEvent script in GO.GetComponent<DialogueEvents>().activeScripts)
                    {
                        script.inMenu = false;
                    }
                }
            }
        }
    }

    void CheckCancelPressed() //makes sure cancel button is only processed once when pressed
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //Debug.Log("buttonPressed");
            buttonPressed = true;
        }
        if (buttonPressed)
        {
            if (Input.GetButtonUp("Cancel"))
            {
                //Debug.Log("button released");
                buttonPressed = false;
            }
        }
    }

    void GetHeroClicked() //sets heroToCheck based on which hero panel is clicked
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            List<RaycastResult> results = GetEventSystemRaycastResults();

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "Hero1Panel")
                {
                    heroToCheck = GameManager.instance.activeHeroes[0];
                }
                if (result.gameObject.name == "Hero2Panel")
                {
                    heroToCheck = GameManager.instance.activeHeroes[1];
                }
                if (result.gameObject.name == "Hero3Panel")
                {
                    heroToCheck = GameManager.instance.activeHeroes[2];
                }
                if (result.gameObject.name == "Hero4Panel")
                {
                    heroToCheck = GameManager.instance.activeHeroes[3];
                }
                if (result.gameObject.name == "Hero5Panel")
                {
                    heroToCheck = GameManager.instance.activeHeroes[4];
                }
            }
        }
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    float GetProgressBarValuesHP(BaseHero hero)
    {
        float heroHP = hero.curHP;
        float heroBaseHP = hero.maxHP;
        float calc_HP;
        
        calc_HP = heroHP / heroBaseHP;

        return calc_HP;
    }

    float GetProgressBarValuesMP(BaseHero hero)
    {
        float heroMP = hero.curMP;
        float heroBaseMP = hero.maxMP;
        float calc_MP;
        
        calc_MP = heroMP / heroBaseMP;

        return calc_MP;
    }

    float GetProgressBarValuesEXP(BaseHero hero)
    {
        float baseLineEXP;
        float heroEXP;

        if (hero.currentLevel == 1)
        {
            baseLineEXP = (GameManager.instance.toLevelUp[hero.currentLevel - 1]);
            heroEXP = hero.currentExp;
        } else
        {
            baseLineEXP = (GameManager.instance.toLevelUp[hero.currentLevel - 1] - GameManager.instance.toLevelUp[hero.currentLevel - 2]);
            heroEXP = (hero.currentExp - baseLineEXP);
            //Debug.Log("baseLine: " + baseLineEXP);
        }

        //Debug.Log(heroEXP + " / " + baseLineEXP + ": " + calcEXP);

        float calcEXP = heroEXP / baseLineEXP;

        return calcEXP;
    }
}
