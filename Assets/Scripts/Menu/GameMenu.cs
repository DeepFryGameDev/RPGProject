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
    GameObject Hero1MainMenuPanel;
    GameObject Hero2MainMenuPanel;
    GameObject Hero3MainMenuPanel;
    GameObject Hero4MainMenuPanel;
    GameObject Hero5MainMenuPanel;
    Image Hero1MainMenuHPProgressBar;
    Image Hero1MainMenuMPProgressBar;
    Image Hero1MainMenuEXPProgressBar;
    Image Hero2MainMenuHPProgressBar;
    Image Hero2MainMenuMPProgressBar;
    Image Hero2MainMenuEXPProgressBar;
    Image Hero3MainMenuHPProgressBar;
    Image Hero3MainMenuMPProgressBar;
    Image Hero3MainMenuEXPProgressBar;
    Image Hero4MainMenuHPProgressBar;
    Image Hero4MainMenuMPProgressBar;
    Image Hero4MainMenuEXPProgressBar;
    Image Hero5MainMenuHPProgressBar;
    Image Hero5MainMenuMPProgressBar;
    Image Hero5MainMenuEXPProgressBar;

    //for item menu objects
    GameObject Hero1ItemPanel;
    GameObject Hero2ItemPanel;
    GameObject Hero3ItemPanel;
    GameObject Hero4ItemPanel;
    GameObject Hero5ItemPanel;
    Image Hero1ItemMenuHPProgressBar;
    Image Hero1ItemMenuMPProgressBar;
    Image Hero2ItemMenuHPProgressBar;
    Image Hero2ItemMenuMPProgressBar;
    Image Hero3ItemMenuHPProgressBar;
    Image Hero3ItemMenuMPProgressBar;
    Image Hero4ItemMenuHPProgressBar;
    Image Hero4ItemMenuMPProgressBar;
    Image Hero5ItemMenuHPProgressBar;
    Image Hero5ItemMenuMPProgressBar;
    private Transform ItemListSpacer;
    GameObject NewItemPanel;

    //for magic menu objects
    Image MagicPanelHPProgressBar;
    Image MagicPanelMPProgressBar;
    [HideInInspector] public GameObject HeroSelectMagicPanel;
    [HideInInspector] public GameObject Hero1SelectMagicPanel;
    [HideInInspector] public GameObject Hero2SelectMagicPanel;
    [HideInInspector] public GameObject Hero3SelectMagicPanel;
    [HideInInspector] public GameObject Hero4SelectMagicPanel;
    [HideInInspector] public GameObject Hero5SelectMagicPanel;
    [HideInInspector] public Image Hero1MagicMenuHPProgressBar;
    [HideInInspector] public Image Hero1MagicMenuMPProgressBar;
    [HideInInspector] public Image Hero2MagicMenuHPProgressBar;
    [HideInInspector] public Image Hero2MagicMenuMPProgressBar;
    [HideInInspector] public Image Hero3MagicMenuHPProgressBar;
    [HideInInspector] public Image Hero3MagicMenuMPProgressBar;
    [HideInInspector] public Image Hero4MagicMenuHPProgressBar;
    [HideInInspector] public Image Hero4MagicMenuMPProgressBar;
    [HideInInspector] public Image Hero5MagicMenuHPProgressBar;
    [HideInInspector] public Image Hero5MagicMenuMPProgressBar;
    GameObject WhiteMagicListPanel;
    GameObject BlackMagicListPanel;
    GameObject SorceryMagicListPanel;
    Transform WhiteMagicListSpacer;
    Transform BlackMagicListSpacer;
    Transform SorceryMagicListSpacer;
    GameObject NewMagicPanel;

    //for equip menu objects
   Image EquipPanelHPProgressBar;
   Image EquipPanelMPProgressBar;
   GameObject NewEquipPanel;
   Transform EquipListSpacer;
   string equipButtonClicked;
   bool inEquipList;
   [HideInInspector] public string equipMode;

    //for status menu objects
    Image StatusPanelHPProgressBar;
    Image StatusPanelMPProgressBar;
    Image StatusPanelEXPProgressBar;

    //for grid menu objects
    GameObject Hero1GridPanel;
    GameObject Hero2GridPanel;
    GameObject Hero3GridPanel;
    GameObject Hero4GridPanel;
    GameObject Hero5GridPanel;
    Image Hero1GridMenuHPProgressBar;
    Image Hero1GridMenuMPProgressBar;
    Image Hero2GridMenuHPProgressBar;
    Image Hero2GridMenuMPProgressBar;
    Image Hero3GridMenuHPProgressBar;
    Image Hero3GridMenuMPProgressBar;
    Image Hero4GridMenuHPProgressBar;
    Image Hero4GridMenuMPProgressBar;
    Image Hero5GridMenuHPProgressBar;
    Image Hero5GridMenuMPProgressBar;
    [HideInInspector] public Sprite gridBG;
    [HideInInspector] public bool gridChoosingTile;
    [HideInInspector] public BaseHero gridMenuHero;
    [HideInInspector] public string gridTileChanging;    

    //for party menu objects
    GameObject Hero1PartyPanel;
    GameObject Hero2PartyPanel;
    GameObject Hero3PartyPanel;
    GameObject Hero4PartyPanel;
    GameObject Hero5PartyPanel;
    Image Hero1PartyMenuHPProgressBar;
    Image Hero1PartyMenuMPProgressBar;
    Image Hero2PartyMenuHPProgressBar;
    Image Hero2PartyMenuMPProgressBar;
    Image Hero3PartyMenuHPProgressBar;
    Image Hero3PartyMenuMPProgressBar;
    Image Hero4PartyMenuHPProgressBar;
    Image Hero4PartyMenuMPProgressBar;
    Image Hero5PartyMenuHPProgressBar;
    Image Hero5PartyMenuMPProgressBar;
    Transform PartyInactiveRow1Spacer;
    Transform PartyInactiveRow2Spacer;
    Transform PartyInactiveRow3Spacer;
    int row1ChildCount;
    int row2ChildCount;
    int row3ChildCount;
    GameObject InactivePartyButton;
    [HideInInspector] public BaseHero PartyHeroSelected = null;
    [HideInInspector] public string PartySelectedHeroType;

    //for talents menu objects
    Image TalentsPanelHPProgressBar;
    Image TalentsPanelMPProgressBar;

    //for quest menu objects
    GameObject QuestListButton;
    Transform ActiveQuestListSpacer;
    Transform CompletedQuestListSpacer;
    [HideInInspector] public string QuestOption;
    [HideInInspector] public bool QuestClicked;

    //for bestiary menu objects
   GameObject BestiaryEntryButton;
   Transform BestiaryEnemyListSpacer;
   [HideInInspector] public bool BestiaryEntryClicked;

    //for menu buttons
    Button ItemButton;
    Button MagicButton;
    Button EquipButton;
    Button StatusButton;
    Button PartyButton;
    Button GridButton;
    Button TalentsButton;
    Button QuestsButton;
    Button BestiaryButton;

    //for menu canvases
    GameObject MainMenuCanvas;
    GameObject ItemMenuCanvas;
    GameObject MagicMenuCanvas;
    GameObject EquipMenuCanvas;
    GameObject StatusMenuCanvas;
    GameObject PartyMenuCanvas;
    GameObject GridMenuCanvas;
    GameObject TalentsMenuCanvas;
    GameObject QuestsMenuCanvas;
    GameObject BestiaryMenuCanvas;
    
    public enum MenuStates
    {
        MAIN,
        ITEM,
        MAGIC,
        EQUIP,
        STATUS,
        PARTY,
        GRID,
        TALENTS,
        QUESTS,
        BESTIARY,
        IDLE
    }
    [ReadOnly] public MenuStates menuState; //for which menu is open

    bool buttonPressed = false;

    GraphicRaycaster raycaster;
    [ReadOnly] public BaseHero heroToCheck;

    void Start()
    {
        player = GameObject.Find("Player");

        //Set Canvases
        MainMenuCanvas = GameObject.Find("GameManager/Menus/MainMenuCanvas");
        ItemMenuCanvas = GameObject.Find("GameManager/Menus/ItemMenuCanvas");
        MagicMenuCanvas = GameObject.Find("GameManager/Menus/MagicMenuCanvas");
        HeroSelectMagicPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel");
        EquipMenuCanvas = GameObject.Find("GameManager/Menus/EquipMenuCanvas");
        StatusMenuCanvas = GameObject.Find("GameManager/Menus/StatusMenuCanvas");
        TalentsMenuCanvas = GameObject.Find("GameManager/Menus/TalentsMenuCanvas");
        QuestsMenuCanvas = GameObject.Find("GameManager/Menus/QuestsMenuCanvas");
        PartyMenuCanvas = GameObject.Find("GameManager/Menus/PartyMenuCanvas");
        GridMenuCanvas = GameObject.Find("GameManager/Menus/GridMenuCanvas");
        BestiaryMenuCanvas = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas");

        //set Main Menu Canvas group
        mainMenuCanvasGroup = MainMenuCanvas.GetComponent<CanvasGroup>();

        //Set Buttons
        ItemButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/ItemButton").GetComponent<Button>();
        MagicButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/MagicButton").GetComponent<Button>();
        EquipButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/EquipButton").GetComponent<Button>();
        StatusButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/StatusButton").GetComponent<Button>();
        TalentsButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/TalentsButton").GetComponent<Button>();
        PartyButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/PartyButton").GetComponent<Button>();
        GridButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/GridButton").GetComponent<Button>();
        QuestsButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/QuestsButton").GetComponent<Button>();
        BestiaryButton = MainMenuCanvas.transform.Find("MenuButtonsPanel/BestiaryButton").GetComponent<Button>();

        //sets spacers
        ItemListSpacer = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform; //find spacer and make connection
        WhiteMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/WhiteMagicListPanel/WhiteMagicScroller/WhiteMagicListSpacer").transform; //find spacer and make connection
        BlackMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/BlackMagicListPanel/BlackMagicScroller/BlackMagicListSpacer").transform; //find spacer and make connection
        SorceryMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/SorceryMagicListPanel/SorceryMagicScroller/SorceryMagicListSpacer").transform; //find spacer and make connection
        EquipListSpacer = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform;
        PartyInactiveRow1Spacer = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/HeroRow1Spacer").transform;
        PartyInactiveRow2Spacer = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/HeroRow2Spacer").transform;
        PartyInactiveRow3Spacer = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/HeroRow3Spacer").transform;
        ActiveQuestListSpacer = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform;
        CompletedQuestListSpacer = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform;
        BestiaryEnemyListSpacer = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform;

        //Set Hero Panels
        Hero1MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero1Panel");
        Hero2MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero2Panel");
        Hero3MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero3Panel");
        Hero4MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero4Panel");
        Hero5MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero5Panel");

        Hero1ItemPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero1ItemPanel");
        Hero2ItemPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero2ItemPanel");
        Hero3ItemPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero3ItemPanel");
        Hero4ItemPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero4ItemPanel");
        Hero5ItemPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/HeroItemPanel/Hero5ItemPanel");

        HeroSelectMagicPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel");
        Hero1SelectMagicPanel = HeroSelectMagicPanel.transform.Find("Hero1SelectMagicPanel").gameObject;
        Hero2SelectMagicPanel = HeroSelectMagicPanel.transform.Find("Hero2SelectMagicPanel").gameObject;
        Hero3SelectMagicPanel = HeroSelectMagicPanel.transform.Find("Hero3SelectMagicPanel").gameObject;
        Hero4SelectMagicPanel = HeroSelectMagicPanel.transform.Find("Hero4SelectMagicPanel").gameObject;
        Hero5SelectMagicPanel = HeroSelectMagicPanel.transform.Find("Hero5SelectMagicPanel").gameObject;

        Hero1GridPanel = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/Hero1GridPanel");
        Hero2GridPanel = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/Hero2GridPanel");
        Hero3GridPanel = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/Hero3GridPanel");
        Hero4GridPanel = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/Hero4GridPanel");
        Hero5GridPanel = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel/Hero5GridPanel");

        Hero1PartyPanel = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel/Hero1PartyPanel");
        Hero2PartyPanel = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel/Hero2PartyPanel");
        Hero3PartyPanel = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel/Hero3PartyPanel");
        Hero4PartyPanel = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel/Hero4PartyPanel");
        Hero5PartyPanel = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel/Hero5PartyPanel");

        //Set Bar Images
        Hero1MainMenuHPProgressBar = Hero1MainMenuPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero1MainMenuMPProgressBar = Hero1MainMenuPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero1MainMenuEXPProgressBar = Hero1MainMenuPanel.transform.Find("LevelProgressBarBG/LevelProgressBar").GetComponent<Image>();
        Hero2MainMenuHPProgressBar = Hero2MainMenuPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero2MainMenuMPProgressBar = Hero2MainMenuPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero2MainMenuEXPProgressBar = Hero2MainMenuPanel.transform.Find("LevelProgressBarBG/LevelProgressBar").GetComponent<Image>();
        Hero3MainMenuHPProgressBar = Hero3MainMenuPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero3MainMenuMPProgressBar = Hero3MainMenuPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero3MainMenuEXPProgressBar = Hero3MainMenuPanel.transform.Find("LevelProgressBarBG/LevelProgressBar").GetComponent<Image>();
        Hero4MainMenuHPProgressBar = Hero4MainMenuPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero4MainMenuMPProgressBar = Hero4MainMenuPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero4MainMenuEXPProgressBar = Hero4MainMenuPanel.transform.Find("LevelProgressBarBG/LevelProgressBar").GetComponent<Image>();
        Hero5MainMenuHPProgressBar = Hero5MainMenuPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero5MainMenuMPProgressBar = Hero5MainMenuPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero5MainMenuEXPProgressBar = Hero5MainMenuPanel.transform.Find("LevelProgressBarBG/LevelProgressBar").GetComponent<Image>();

        Hero1ItemMenuHPProgressBar = Hero1ItemPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero1ItemMenuMPProgressBar = Hero1ItemPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero2ItemMenuHPProgressBar = Hero2ItemPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero2ItemMenuMPProgressBar = Hero2ItemPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero3ItemMenuHPProgressBar = Hero3ItemPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero3ItemMenuMPProgressBar = Hero3ItemPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero4ItemMenuHPProgressBar = Hero4ItemPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero4ItemMenuMPProgressBar = Hero4ItemPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero5ItemMenuHPProgressBar = Hero5ItemPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero5ItemMenuMPProgressBar = Hero5ItemPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();

        Hero1MagicMenuHPProgressBar = Hero1SelectMagicPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero1MagicMenuMPProgressBar = Hero1SelectMagicPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero2MagicMenuHPProgressBar = Hero2SelectMagicPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero2MagicMenuMPProgressBar = Hero2SelectMagicPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero3MagicMenuHPProgressBar = Hero3SelectMagicPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero3MagicMenuMPProgressBar = Hero3SelectMagicPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero4MagicMenuHPProgressBar = Hero4SelectMagicPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero4MagicMenuMPProgressBar = Hero4SelectMagicPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero5MagicMenuHPProgressBar = Hero5SelectMagicPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero5MagicMenuMPProgressBar = Hero5SelectMagicPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        MagicPanelHPProgressBar = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroMagicPanel/HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        MagicPanelMPProgressBar = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroMagicPanel/MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        WhiteMagicListPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/WhiteMagicListPanel");
        BlackMagicListPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/BlackMagicListPanel");
        SorceryMagicListPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/SorceryMagicListPanel");

        EquipPanelHPProgressBar = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/HeroEquipPanel/HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        EquipPanelMPProgressBar = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/HeroEquipPanel/MPProgressBarBG/MPProgressBar").GetComponent<Image>();

        StatusPanelHPProgressBar = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/BaseStatsPanel/HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        StatusPanelMPProgressBar = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/BaseStatsPanel/MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        StatusPanelEXPProgressBar = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/BaseStatsPanel/LevelProgressBarBG/LevelProgressBar").GetComponent<Image>();

        Hero1GridMenuHPProgressBar = Hero1GridPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero1GridMenuMPProgressBar = Hero1GridPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero2GridMenuHPProgressBar = Hero2GridPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero2GridMenuMPProgressBar = Hero2GridPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero3GridMenuHPProgressBar = Hero3GridPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero3GridMenuMPProgressBar = Hero3GridPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero4GridMenuHPProgressBar = Hero4GridPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero4GridMenuMPProgressBar = Hero4GridPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero5GridMenuHPProgressBar = Hero5GridPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero5GridMenuMPProgressBar = Hero5GridPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();

        Hero1PartyMenuHPProgressBar = Hero1PartyPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero1PartyMenuMPProgressBar = Hero1PartyPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero2PartyMenuHPProgressBar = Hero2PartyPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero2PartyMenuMPProgressBar = Hero2PartyPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero3PartyMenuHPProgressBar = Hero3PartyPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero3PartyMenuMPProgressBar = Hero3PartyPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero4PartyMenuHPProgressBar = Hero4PartyPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero4PartyMenuMPProgressBar = Hero4PartyPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();
        Hero5PartyMenuHPProgressBar = Hero5PartyPanel.transform.Find("HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        Hero5PartyMenuMPProgressBar = Hero5PartyPanel.transform.Find("MPProgressBarBG/MPProgressBar").GetComponent<Image>();

        //sets party spacer child counts
        row1ChildCount = PartyInactiveRow1Spacer.childCount;
        row2ChildCount = PartyInactiveRow2Spacer.childCount;
        row3ChildCount = PartyInactiveRow3Spacer.childCount;

        equipMode = "";

        PartyHeroSelected = null;

        //disables all menu canvases
        HideCanvas(MainMenuCanvas);
        HideCanvas(ItemMenuCanvas);
        HideCanvas(MagicMenuCanvas);
        HideCanvas(HeroSelectMagicPanel);
        HideCanvas(EquipMenuCanvas);
        HideCanvas(StatusMenuCanvas);
        HideCanvas(PartyMenuCanvas);
        HideCanvas(TalentsMenuCanvas);
        HideCanvas(GridMenuCanvas);
        HideCanvas(QuestsMenuCanvas);
        HideCanvas(BestiaryMenuCanvas);

        menuState = MenuStates.IDLE; //sets menu state to idle as menu is closed
        this.raycaster = GetComponent<GraphicRaycaster>();
    }

    public void DisplayCanvas(GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = 1;
        canvas.GetComponent<CanvasGroup>().interactable = true;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void HideCanvas(GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        canvas.GetComponent<CanvasGroup>().interactable = false;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
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
        if (drawingGUI && (menuState == MenuStates.IDLE))
        {
            PauseBackground(true); //keeps background objects from processing
            ShowMainMenu();
            menuState = MenuStates.MAIN;
        }

        if (Input.GetButtonDown("Cancel") && !disableMenuExit && menuState == MenuStates.MAIN && !buttonPressed) //if cancel is pressed while on main menu
        {
            drawingGUI = false;
            mainMenuCanvasGroup.alpha = 0;
            mainMenuCanvasGroup.interactable = false;
            mainMenuCanvasGroup.blocksRaycasts = false;
            //HideCanvas(MainMenuCanvas);
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

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.GRID && !buttonPressed) //if cancel is pressed on grid menu
        {
            HideGridMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.PARTY && !buttonPressed && (PartyHeroSelected == null)) //if cancel is pressed on party menu
        {
            HidePartyMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.TALENTS && !buttonPressed) //if cancel is pressed on talents menu
        {
            HideTalentsMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.QUESTS && !buttonPressed && !QuestClicked) //if cancel is pressed on quests menu
        {
            HideQuestMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.BESTIARY && !buttonPressed && !BestiaryEntryClicked) //if cancel is pressed on quests menu
        {
            HideBestiaryMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && !buttonPressed && QuestClicked)
        {
            CancelQuestSelect();
        }

        if (Input.GetButtonDown("Cancel") && !buttonPressed && BestiaryEntryClicked)
        {
            CancelBestiarySelect();
        }

        CheckCancelPressed(); //makes sure cancel is only pressed once
    }

    //Main menu panels

    void ShowMainMenu() //draws main menu
    {
        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.blocksRaycasts = true;
        mainMenuCanvasGroup.interactable = true;
        DrawHeroStats();
        DisplayLocation();
        DisplayGold();
        heroToCheck = null;
    }

    void DrawHeroStats()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroPanels(heroCount);

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            DrawHeroFace(GameManager.instance.activeHeroes[i], GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].maxHP); //HP text
            GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].maxMP); //MP text
            GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("EXPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].currentExp + " / " + GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().levelEXPThresholds[(GameManager.instance.activeHeroes[i].currentLevel -1)]); //Exp text
            DrawHeroSpawnPoint(GameManager.instance.activeHeroes[i], GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("GridPanel").gameObject);
        }
    }

    void DrawHeroPanels(int count)
    {
        if (count == 1)
        {
            DisplayCanvas(Hero1MainMenuPanel);
            HideCanvas(Hero2MainMenuPanel);
            HideCanvas(Hero3MainMenuPanel);
            HideCanvas(Hero4MainMenuPanel);
            HideCanvas(Hero5MainMenuPanel);
        }
        else if (count == 2)
        {
            DisplayCanvas(Hero1MainMenuPanel);
            DisplayCanvas(Hero2MainMenuPanel);
            HideCanvas(Hero3MainMenuPanel);
            HideCanvas(Hero4MainMenuPanel);
            HideCanvas(Hero5MainMenuPanel);
        }
        else if (count == 3)
        {
            DisplayCanvas(Hero1MainMenuPanel);
            DisplayCanvas(Hero2MainMenuPanel);
            DisplayCanvas(Hero3MainMenuPanel);
            HideCanvas(Hero4MainMenuPanel);
            HideCanvas(Hero5MainMenuPanel);
        }
        else if (count == 4)
        {
            DisplayCanvas(Hero1MainMenuPanel);
            DisplayCanvas(Hero2MainMenuPanel);
            DisplayCanvas(Hero3MainMenuPanel);
            DisplayCanvas(Hero4MainMenuPanel);
            HideCanvas(Hero5MainMenuPanel);
        }
        else if (count == 5)
        {
            DisplayCanvas(Hero1MainMenuPanel);
            DisplayCanvas(Hero2MainMenuPanel);
            DisplayCanvas(Hero3MainMenuPanel);
            DisplayCanvas(Hero4MainMenuPanel);
            DisplayCanvas(Hero5MainMenuPanel);
        }

        DrawHeroPanelBars();
    }

    void DrawHeroPanelBars()
    {
        if (GameManager.instance.activeHeroes.Count == 1)
        {
            Hero1MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuHPProgressBar.transform.localScale.y);
            Hero1MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuMPProgressBar.transform.localScale.y);
            Hero1MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuEXPProgressBar.transform.localScale.y);
        } else if (GameManager.instance.activeHeroes.Count == 2)
        {
            Hero1MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuHPProgressBar.transform.localScale.y);
            Hero1MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuMPProgressBar.transform.localScale.y);
            Hero1MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuEXPProgressBar.transform.localScale.y);

            Hero2MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuHPProgressBar.transform.localScale.y);
            Hero2MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuMPProgressBar.transform.localScale.y);
            Hero2MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuEXPProgressBar.transform.localScale.y);
        } else if (GameManager.instance.activeHeroes.Count == 3)
        {
            Hero1MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuHPProgressBar.transform.localScale.y);
            Hero1MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuMPProgressBar.transform.localScale.y);
            Hero1MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuEXPProgressBar.transform.localScale.y);

            Hero2MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuHPProgressBar.transform.localScale.y);
            Hero2MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuMPProgressBar.transform.localScale.y);
            Hero2MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuEXPProgressBar.transform.localScale.y);

            Hero3MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuHPProgressBar.transform.localScale.y);
            Hero3MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuMPProgressBar.transform.localScale.y);
            Hero3MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuEXPProgressBar.transform.localScale.y);
        } else if (GameManager.instance.activeHeroes.Count == 4)
        {
            Hero1MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuHPProgressBar.transform.localScale.y);
            Hero1MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuMPProgressBar.transform.localScale.y);
            Hero1MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuEXPProgressBar.transform.localScale.y);

            Hero2MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuHPProgressBar.transform.localScale.y);
            Hero2MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuMPProgressBar.transform.localScale.y);
            Hero2MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuEXPProgressBar.transform.localScale.y);

            Hero3MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuHPProgressBar.transform.localScale.y);
            Hero3MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuMPProgressBar.transform.localScale.y);
            Hero3MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuEXPProgressBar.transform.localScale.y);

            Hero4MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4MainMenuHPProgressBar.transform.localScale.y);
            Hero4MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4MainMenuMPProgressBar.transform.localScale.y);
            Hero4MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4MainMenuEXPProgressBar.transform.localScale.y);
        } else if (GameManager.instance.activeHeroes.Count == 5)
        {
            Hero1MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuHPProgressBar.transform.localScale.y);
            Hero1MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuMPProgressBar.transform.localScale.y);
            Hero1MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1MainMenuEXPProgressBar.transform.localScale.y);

            Hero2MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuHPProgressBar.transform.localScale.y);
            Hero2MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuMPProgressBar.transform.localScale.y);
            Hero2MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2MainMenuEXPProgressBar.transform.localScale.y);

            Hero3MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuHPProgressBar.transform.localScale.y);
            Hero3MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuMPProgressBar.transform.localScale.y);
            Hero3MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3MainMenuEXPProgressBar.transform.localScale.y);

            Hero4MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4MainMenuHPProgressBar.transform.localScale.y);
            Hero4MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4MainMenuMPProgressBar.transform.localScale.y);
            Hero4MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4MainMenuEXPProgressBar.transform.localScale.y);

            Hero5MainMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5MainMenuHPProgressBar.transform.localScale.y);
            Hero5MainMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5MainMenuMPProgressBar.transform.localScale.y);
            Hero5MainMenuEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5MainMenuEXPProgressBar.transform.localScale.y);
        }

    }

    void DrawHeroSpawnPoint(BaseHero hero, GameObject panel)
    {
        foreach (Transform child in panel.transform)
        {
            child.gameObject.GetComponent<Image>().color = Color.white;
        }

        string spawnPoint = hero.spawnPoint;
        panel.gameObject.transform.Find("Grid - " + spawnPoint).GetComponent<Image>().color = Color.black;
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
        GameObject.Find("GameManager/Menus/MainMenuCanvas/TimeGoldPanel").transform.Find("GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();
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

            GameObject.Find("GameManager/Menus/MainMenuCanvas/TimeGoldPanel").transform.Find("TimeText").GetComponent<Text>().text = hours + ":" + minutes + ":" + seconds;
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
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(ItemMenuCanvas);
        menuState = MenuStates.ITEM;
        EraseItemDescText();
        DrawHeroItemMenuStats();
        DrawItemList();
    }

    void HideItemMenu()
    {
        ResetItemList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(ItemMenuCanvas);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
    }

    void EraseItemDescText()
    {
        GameObject.Find("ItemMenuCanvas/ItemMenuPanel/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "";
    }

    public void DrawItemList() //draws items in inventory to item list
    {
        ResetItemList();

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
        foreach (Transform child in GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform)
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
            DrawHeroFace(GameManager.instance.activeHeroes[i], GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].maxHP); //HP text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].maxMP); //MP text
        }
    }

    void DrawHeroItemMenuPanels(int count)
    {
        if (count == 1)
        {
            DisplayCanvas(Hero1ItemPanel);
            HideCanvas(Hero2ItemPanel);
            HideCanvas(Hero3ItemPanel);
            HideCanvas(Hero4ItemPanel);
            HideCanvas(Hero5ItemPanel);
        }
        else if (count == 2)
        {
            DisplayCanvas(Hero1ItemPanel);
            DisplayCanvas(Hero2ItemPanel);
            HideCanvas(Hero3ItemPanel);
            HideCanvas(Hero4ItemPanel);
            HideCanvas(Hero5ItemPanel);
        }
        else if (count == 3)
        {
            DisplayCanvas(Hero1ItemPanel);
            DisplayCanvas(Hero2ItemPanel);
            DisplayCanvas(Hero3ItemPanel);
            HideCanvas(Hero4ItemPanel);
            HideCanvas(Hero5ItemPanel);
        }
        else if (count == 4)
        {
            DisplayCanvas(Hero1ItemPanel);
            DisplayCanvas(Hero2ItemPanel);
            DisplayCanvas(Hero3ItemPanel);
            DisplayCanvas(Hero4ItemPanel);
            HideCanvas(Hero5ItemPanel);
        }
        else if (count == 5)
        {
            DisplayCanvas(Hero1ItemPanel);
            DisplayCanvas(Hero2ItemPanel);
            DisplayCanvas(Hero3ItemPanel);
            DisplayCanvas(Hero4ItemPanel);
            DisplayCanvas(Hero5ItemPanel);
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
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(MagicMenuCanvas);
        DisplayCanvas(WhiteMagicListPanel);
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDescriptionPanel/MagicDescriptionText").GetComponent<Text>().text = "";
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel/CooldownText").GetComponent<Text>().text = "-";
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel/MPCostText").GetComponent<Text>().text = "-";
        DrawMagicMenuStats(hero);
        DrawMagicListPanels(hero);
        menuState = MenuStates.MAGIC;
    }

    void DrawMagicListPanels(BaseHero hero)
    {
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
        HideCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);
    }

    public void ShowWhiteMagicListPanel()
    {
        DisplayCanvas(WhiteMagicListPanel);
        HideCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);
    }

    public void ShowBlackMagicListPanel()
    {
        HideCanvas(WhiteMagicListPanel);
        DisplayCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);
    }

    public void ShowSorceryMagicListPanel()
    {
        HideCanvas(WhiteMagicListPanel);
        HideCanvas(BlackMagicListPanel);
        DisplayCanvas(SorceryMagicListPanel);
    }

    public void DrawMagicMenuStats(BaseHero hero)
    {
        DrawHeroFace(hero, GameObject.Find("HeroMagicPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("HeroMagicPanel/NameText").GetComponent<Text>().text = hero.name; //Name text
        GameObject.Find("HeroMagicPanel/LevelText").GetComponent<Text>().text = hero.currentLevel.ToString(); //Level text
        GameObject.Find("HeroMagicPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.maxHP.ToString()); //HP text
        GameObject.Find("HeroMagicPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.maxMP.ToString()); //MP text

        MagicPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hero), 0, 1), MagicPanelHPProgressBar.transform.localScale.y);
        MagicPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hero), 0, 1), MagicPanelMPProgressBar.transform.localScale.y);
    }

    void ResetMagicList()
    {
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
    }

    void HideMagicMenu()
    {
        ResetMagicList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(MagicMenuCanvas);
        HideCanvas(WhiteMagicListPanel);
        HideCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
    }

    //--------------------

    //Equip Menu

    public void ShowEquipMenu()
    {
        Debug.Log("Equip button clicked - choose a hero");
        StartCoroutine(ChooseHeroToCheck());
    }

    IEnumerator ChooseHeroToCheck()
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
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(EquipMenuCanvas);
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
        DrawHeroFace(hero, GameObject.Find("HeroEquipPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("HeroEquipPanel/NameText").GetComponent<Text>().text = hero.name; //Name text
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
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(EquipMenuCanvas);
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
                heroToCheck.Unequip(0);

                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "ChestButton")
            {
                heroToCheck.Unequip(1);

                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "WristsButton")
            {
                heroToCheck.Unequip(2);

                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LegsButton")
            {
                heroToCheck.Unequip(3);

                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "FeetButton")
            {
                heroToCheck.Unequip(4);

                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RelicButton")
            {
                heroToCheck.Unequip(5);

                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RightHandButton")
            {
                heroToCheck.Unequip(6);

                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LeftHandButton")
            {
                heroToCheck.Unequip(7);

                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
            }

            UpdateHeroFromEquipmentStats();

            DrawEquipMenuStats(heroToCheck);

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
                    heroToCheck.Unequip(0);

                    GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "ChestButton")
                {
                    heroToCheck.Unequip(1);

                    GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "WristsButton")
                {
                    heroToCheck.Unequip(2);

                    GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "LegsButton")
                {
                    heroToCheck.Unequip(3);

                    GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "FeetButton")
                {
                    heroToCheck.Unequip(4);

                    GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "RelicButton")
                {
                    heroToCheck.Unequip(5);

                    GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "RightHandButton")
                {
                    heroToCheck.Unequip(6);

                    GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "LeftHandButton")
                {
                    heroToCheck.Unequip(7);

                    GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
                }

                UpdateHeroFromEquipmentStats();

                DrawEquipMenuStats(heroToCheck);

                return;
            }

            if (equipButtonClicked == "HeadButton")
            {
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "ChestButton")
            {
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "WristsButton")
            {
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LegsButton")
            {
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "FeetButton")
            {
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RelicButton")
            {
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LeftHandButton")
            {
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RightHandButton")
            {
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

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

            DrawEquipMenuStats(heroToCheck);

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
        
        foreach (Equipment equipment in heroToCheck.equipment)
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

        heroToCheck.currentStrength = heroToCheck.baseSTR + tempStrength;
        heroToCheck.currentStamina = heroToCheck.baseSTA + tempStamina;
        heroToCheck.currentAgility = heroToCheck.baseAGI + tempAgility;
        heroToCheck.currentDexterity = heroToCheck.baseDEX + tempDexterity;
        heroToCheck.currentIntelligence = heroToCheck.baseINT + tempIntelligence;
        heroToCheck.currentSpirit = heroToCheck.baseSPI + tempSpirit;

        heroToCheck.maxHP = heroToCheck.GetMaxHP(heroToCheck.maxHP) + tempHP;
        heroToCheck.maxMP = heroToCheck.GetMaxMP(heroToCheck.maxMP) + tempMP;

        heroToCheck.currentATK = heroToCheck.baseATK + tempATK;
        heroToCheck.currentMATK = heroToCheck.baseMATK + tempMATK;
        heroToCheck.currentDEF = heroToCheck.baseDEF + tempDEF;
        heroToCheck.currentMDEF = heroToCheck.baseMDEF + tempMDEF;

        heroToCheck.currentHitRating = heroToCheck.baseHit + tempHit;
        heroToCheck.currentCritRating = heroToCheck.baseCrit + tempCrit;
        heroToCheck.currentMoveRating = heroToCheck.baseMove + tempMove;
        heroToCheck.currentRegenRating = heroToCheck.baseRegen + tempRegen;

        heroToCheck.currentDodgeRating = heroToCheck.baseDodge + tempDodge;
        heroToCheck.currentBlockRating = heroToCheck.baseBlock + tempBlock;
        heroToCheck.currentParryRating = heroToCheck.baseParry + tempParry;
        heroToCheck.currentThreatRating = heroToCheck.baseThreat + tempThreat;

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
        int tempStrength = heroToCheck.baseSTR, tempStamina = heroToCheck.baseSTA, tempAgility = heroToCheck.baseAGI, 
            tempDexterity = heroToCheck.baseDEX, tempIntelligence = heroToCheck.baseINT, tempSpirit = heroToCheck.baseSPI;
        int tempHP = heroToCheck.GetBaseMaxHP(heroToCheck.baseHP), tempMP = heroToCheck.GetBaseMaxMP(heroToCheck.baseMP);
        int tempATK = heroToCheck.baseATK, tempMATK = heroToCheck.baseMATK, tempDEF = heroToCheck.baseDEF, tempMDEF = heroToCheck.baseMDEF;
        int tempHit = heroToCheck.baseHit, tempCrit = heroToCheck.baseCrit, tempMove = heroToCheck.baseMove, tempRegen = heroToCheck.baseRegen;
        int tempDodge = heroToCheck.baseDodge, tempBlock = heroToCheck.baseBlock, tempParry = heroToCheck.baseParry, tempThreat = heroToCheck.baseThreat;

        foreach (Equipment equipment in heroToCheck.equipment)
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
            if (tempStrength > heroToCheck.currentStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Up");
            }
            else if (tempStrength < heroToCheck.currentStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Down");
            }
            else if (tempStrength == heroToCheck.currentStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = tempStamina.ToString();
            if (tempStamina > heroToCheck.currentStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Up");
            }
            else if (tempStamina < heroToCheck.currentStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Down");
            }
            else if (tempStamina == heroToCheck.currentStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = tempAgility.ToString();
            if (tempAgility > heroToCheck.currentAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Up");
            }
            if (tempAgility < heroToCheck.currentAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Down");
            }
            else if (tempAgility == heroToCheck.currentAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = tempDexterity.ToString();
            if (tempDexterity > heroToCheck.currentDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Up");
            }
            else if (tempDexterity < heroToCheck.currentDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Down");
            }
            else if (tempDexterity == heroToCheck.currentDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = tempIntelligence.ToString();
            if (tempIntelligence > heroToCheck.currentIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Up");
            }
            else if (tempIntelligence < heroToCheck.currentIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Down");
            }
            else if (tempIntelligence == heroToCheck.currentIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = tempSpirit.ToString();
            if (tempSpirit > heroToCheck.currentSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Up");
            }
            else if (tempSpirit < heroToCheck.currentSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Down");
            }
            else if (tempSpirit == heroToCheck.currentSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = tempHP.ToString();
            if (tempHP > heroToCheck.maxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Up");
            }
            else if (tempHP < heroToCheck.maxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Down");
            }
            else if (tempHP == heroToCheck.maxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = tempMP.ToString();
            if (tempMP > heroToCheck.maxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Up");
            }
            else if (tempMP < heroToCheck.maxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Down");
            }
            else if (tempMP == heroToCheck.maxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = tempATK.ToString();
            if (tempATK > heroToCheck.currentATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Up");
            }
            else if (tempATK < heroToCheck.currentATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Down");
            }
            else if (tempATK == heroToCheck.currentATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = tempMATK.ToString();
            if (tempMATK > heroToCheck.currentMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Up");
            }
            else if (tempMATK < heroToCheck.currentMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Down");
            }
            else if (tempMATK == heroToCheck.currentMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = tempDEF.ToString();
            if (tempDEF > heroToCheck.currentDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Up");
            }
            else if (tempDEF < heroToCheck.currentDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Down");
            }
            else if (tempDEF == heroToCheck.currentDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = tempMDEF.ToString();
            if (tempMDEF > heroToCheck.currentMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Up");
            }
            else if (tempMDEF < heroToCheck.currentMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Down");
            }
            else if (tempMDEF == heroToCheck.currentMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = heroToCheck.GetHitChance(tempHit, tempAgility).ToString();
            if (heroToCheck.GetHitChance(tempHit, tempAgility) > heroToCheck.GetHitChance(heroToCheck.currentHitRating, heroToCheck.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Up");
            }
            else if (heroToCheck.GetHitChance(tempHit, tempAgility) < heroToCheck.GetHitChance(heroToCheck.currentHitRating, heroToCheck.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Down");
            }
            else if (heroToCheck.GetHitChance(tempHit, tempAgility) == heroToCheck.GetHitChance(heroToCheck.currentHitRating, heroToCheck.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = heroToCheck.GetCritChance(tempCrit, tempDexterity).ToString();
            if (heroToCheck.GetCritChance(tempCrit, tempDexterity) > heroToCheck.GetCritChance(heroToCheck.currentCritRating, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Up");
            }
            else if (heroToCheck.GetCritChance(tempCrit, tempDexterity) < heroToCheck.GetCritChance(heroToCheck.currentCritRating, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Down");
            }
            else if (heroToCheck.GetCritChance(tempCrit, tempDexterity) == heroToCheck.GetCritChance(heroToCheck.currentCritRating, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = heroToCheck.GetMoveRating(tempMove, tempDexterity).ToString();
            if (heroToCheck.GetMoveRating(tempMove, tempDexterity) > heroToCheck.GetMoveRating(heroToCheck.currentMoveRating, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Up");
            }
            else if (heroToCheck.GetMoveRating(tempMove, tempDexterity) < heroToCheck.GetMoveRating(heroToCheck.currentMoveRating, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Down");
            }
            else if (heroToCheck.GetMoveRating(tempMove, tempDexterity) == heroToCheck.GetMoveRating(heroToCheck.currentMoveRating, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = heroToCheck.GetRegen(tempRegen, tempSpirit).ToString();
            if (heroToCheck.GetRegen(tempRegen, tempSpirit) > heroToCheck.GetRegen(heroToCheck.currentRegenRating, heroToCheck.currentSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Up");
            }
            else if (heroToCheck.GetRegen(tempRegen, tempSpirit) < heroToCheck.GetRegen(heroToCheck.currentRegenRating, heroToCheck.currentSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Down");
            }
            else if (heroToCheck.GetRegen(tempRegen, tempSpirit) == heroToCheck.GetRegen(heroToCheck.currentRegenRating, heroToCheck.currentSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = heroToCheck.GetDodgeChance(tempDodge, tempAgility).ToString();
            if (heroToCheck.GetDodgeChance(tempDodge, tempAgility) > heroToCheck.GetDodgeChance(heroToCheck.currentDodgeRating, heroToCheck.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Up");
            }
            else if (heroToCheck.GetDodgeChance(tempDodge, tempAgility) < heroToCheck.GetDodgeChance(heroToCheck.currentDodgeRating, heroToCheck.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Down");
            }
            else if (heroToCheck.GetDodgeChance(tempDodge, tempAgility) == heroToCheck.GetDodgeChance(heroToCheck.currentDodgeRating, heroToCheck.currentAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = heroToCheck.GetBlockChance(tempBlock).ToString();
            if (heroToCheck.GetBlockChance(tempBlock) > heroToCheck.GetBlockChance(heroToCheck.currentBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Up");
            }
            else if (heroToCheck.GetBlockChance(tempBlock) < heroToCheck.GetBlockChance(heroToCheck.currentBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Down");
            }
            else if (heroToCheck.GetBlockChance(tempBlock) == heroToCheck.GetBlockChance(heroToCheck.currentBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = heroToCheck.GetParryChance(tempParry, tempStrength, tempDexterity).ToString();
            if (heroToCheck.GetParryChance(tempParry, tempStrength, tempDexterity) > heroToCheck.GetParryChance(heroToCheck.currentParryRating, heroToCheck.currentStrength, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Up");
            }
            else if (heroToCheck.GetParryChance(tempParry, tempStrength, tempDexterity) < heroToCheck.GetParryChance(heroToCheck.currentParryRating, heroToCheck.currentStrength, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Down");
            }
            else if (heroToCheck.GetParryChance(tempParry, tempStrength, tempDexterity) == heroToCheck.GetParryChance(heroToCheck.currentParryRating, heroToCheck.currentStrength, heroToCheck.currentDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = heroToCheck.GetThreatRating(tempThreat).ToString();
            if (heroToCheck.GetThreatRating(tempThreat) > heroToCheck.GetThreatRating(heroToCheck.currentThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Up");
            }
            else if (heroToCheck.GetThreatRating(tempThreat) < heroToCheck.GetThreatRating(heroToCheck.currentThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Down");
            }
            else if (heroToCheck.GetThreatRating(tempThreat) == heroToCheck.GetThreatRating(heroToCheck.currentThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
            }

            return;
        }

        GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = (tempStrength + toEquip.Strength).ToString();
        if ((tempStrength + toEquip.Strength) > heroToCheck.currentStrength)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Up");
        } else if ((tempStrength + toEquip.Strength) < heroToCheck.currentStrength)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Down");
        } else if ((tempStrength + toEquip.Strength) == heroToCheck.currentStrength)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = (tempStamina + toEquip.Stamina).ToString();
        if ((tempStamina + toEquip.Stamina) > heroToCheck.currentStamina)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Up");
        }
        else if ((tempStamina + toEquip.Stamina) < heroToCheck.currentStamina)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Down");
        }
        else if ((tempStamina + toEquip.Stamina) == heroToCheck.currentStamina)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = (tempAgility + toEquip.Agility).ToString();
        if ((tempAgility + toEquip.Agility) > heroToCheck.currentAgility)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Up");
        }
        else if ((tempAgility + toEquip.Agility) < heroToCheck.currentAgility)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Down");
        }
        else if ((tempAgility + toEquip.Agility) == heroToCheck.currentAgility)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = (tempDexterity + toEquip.Dexterity).ToString();
        if ((tempDexterity + toEquip.Dexterity) > heroToCheck.currentDexterity)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Up");
        }
        else if ((tempDexterity + toEquip.Dexterity) < heroToCheck.currentDexterity)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Down");
        }
        else if ((tempDexterity + toEquip.Dexterity) == heroToCheck.currentDexterity)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = (tempIntelligence + toEquip.Intelligence).ToString();
        if ((tempIntelligence + toEquip.Intelligence) > heroToCheck.currentIntelligence)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Up");
        }
        else if ((tempIntelligence + toEquip.Intelligence) < heroToCheck.currentIntelligence)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Down");
        }
        else if ((tempIntelligence + toEquip.Intelligence) == heroToCheck.currentIntelligence)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = (tempSpirit + toEquip.Spirit).ToString();
        if ((tempSpirit + toEquip.Spirit) > heroToCheck.currentSpirit)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Up");
        }
        else if ((tempSpirit + toEquip.Spirit) < heroToCheck.currentSpirit)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Down");
        }
        else if ((tempSpirit + toEquip.Spirit) == heroToCheck.currentSpirit)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");
        }


        GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = (tempHP + Mathf.RoundToInt(toEquip.Stamina * .75f)).ToString();
        if ((tempHP + Mathf.RoundToInt(toEquip.Stamina * .75f)) > heroToCheck.maxHP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Up");
        }
        else if ((tempHP + Mathf.RoundToInt(toEquip.Stamina * .75f)) < heroToCheck.maxHP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Down");
        }
        else if ((tempHP + Mathf.RoundToInt(toEquip.Stamina * .75f)) == heroToCheck.maxHP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = (tempMP + Mathf.RoundToInt(toEquip.Intelligence * .5f)).ToString();
        if ((tempMP + Mathf.RoundToInt(toEquip.Intelligence * .5f)) > heroToCheck.maxMP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Up");
        }
        else if ((tempMP + Mathf.RoundToInt(toEquip.Intelligence * .5f)) < heroToCheck.maxMP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Down");
        }
        else if ((tempMP + Mathf.RoundToInt(toEquip.Intelligence * .5f)) == heroToCheck.maxMP)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");
        }


        GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = (tempATK + toEquip.ATK + Mathf.RoundToInt(toEquip.Strength * .5f)).ToString();
        if ((tempATK + toEquip.ATK + Mathf.RoundToInt(toEquip.Strength * .5f)) > heroToCheck.currentATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Up");
        }
        else if ((tempATK + toEquip.ATK + Mathf.RoundToInt(toEquip.Strength * .5f)) < heroToCheck.currentATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Down");
        }
        else if ((tempATK + toEquip.ATK + Mathf.RoundToInt(toEquip.Strength * .5f)) == heroToCheck.currentATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = (tempMATK + toEquip.MATK + Mathf.RoundToInt(toEquip.Intelligence * .5f)).ToString();
        if ((tempMATK + toEquip.MATK + Mathf.RoundToInt(toEquip.Intelligence * .5f)) > heroToCheck.currentMATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Up");
        }
        else if ((tempMATK + toEquip.MATK + Mathf.RoundToInt(toEquip.Intelligence * .5f)) < heroToCheck.currentMATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Down");
        }
        else if ((tempMATK + toEquip.MATK + Mathf.RoundToInt(toEquip.Intelligence * .5f)) == heroToCheck.currentMATK)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = (tempDEF + toEquip.DEF + Mathf.RoundToInt(toEquip.Stamina * .6f)).ToString();
        if ((tempDEF + toEquip.DEF + Mathf.RoundToInt(toEquip.Stamina * .6f)) > heroToCheck.currentDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Up");
        }
        else if ((tempDEF + toEquip.DEF + Mathf.RoundToInt(toEquip.Stamina * .6f)) < heroToCheck.currentDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Down");
        }
        else if ((tempDEF + toEquip.DEF + Mathf.RoundToInt(toEquip.Stamina * .6f)) == heroToCheck.currentDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = (tempMDEF + toEquip.MDEF + Mathf.RoundToInt(toEquip.Stamina * .5f)).ToString();
        if ((tempMDEF + toEquip.MDEF + Mathf.RoundToInt(toEquip.Stamina * .5f)) > heroToCheck.currentMDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Up");
        }
        else if ((tempMDEF + toEquip.MDEF + Mathf.RoundToInt(toEquip.Stamina * .5f)) < heroToCheck.currentMDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Down");
        }
        else if ((tempMDEF + toEquip.MDEF + Mathf.RoundToInt(toEquip.Stamina * .5f)) == heroToCheck.currentMDEF)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");
        }


        GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = (heroToCheck.GetHitChance((tempHit + toEquip.hit),(tempAgility + toEquip.Agility))).ToString();
        if ((heroToCheck.GetHitChance((tempHit + toEquip.hit), (tempAgility + toEquip.Agility))) > heroToCheck.GetHitChance(heroToCheck.currentHitRating, heroToCheck.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Up");
        }
        else if ((heroToCheck.GetHitChance((tempHit + toEquip.hit), (tempAgility + toEquip.Agility))) < heroToCheck.GetHitChance(heroToCheck.currentHitRating, heroToCheck.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Down");
        }
        else if ((heroToCheck.GetHitChance((tempHit + toEquip.hit), (tempAgility + toEquip.Agility))) == heroToCheck.GetHitChance(heroToCheck.currentHitRating, heroToCheck.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = (heroToCheck.GetCritChance((tempCrit + toEquip.crit), (tempDexterity + toEquip.Dexterity))).ToString();
        if ((heroToCheck.GetCritChance((tempCrit + toEquip.crit), (tempDexterity + toEquip.Dexterity))) > heroToCheck.GetCritChance(heroToCheck.currentCritRating, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Up");
        }
        else if ((heroToCheck.GetCritChance((tempCrit + toEquip.crit), (tempDexterity + toEquip.Dexterity))) < heroToCheck.GetCritChance(heroToCheck.currentCritRating, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Down");
        }
        else if ((heroToCheck.GetCritChance((tempCrit + toEquip.crit), (tempDexterity + toEquip.Dexterity))) == heroToCheck.GetCritChance(heroToCheck.currentCritRating, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = (heroToCheck.GetMoveRating((tempMove + toEquip.move), (tempDexterity + toEquip.Dexterity))).ToString();
        if ((heroToCheck.GetMoveRating((tempMove + toEquip.move), (tempDexterity + toEquip.Dexterity))) > heroToCheck.GetMoveRating(heroToCheck.currentMoveRating, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Up");
        }
        else if ((heroToCheck.GetMoveRating((tempMove + toEquip.move), (tempDexterity + toEquip.Dexterity))) < heroToCheck.GetMoveRating(heroToCheck.currentMoveRating, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Down");
        }
        else if ((heroToCheck.GetMoveRating((tempMove + toEquip.move), (tempDexterity + toEquip.Dexterity))) == heroToCheck.GetMoveRating(heroToCheck.currentMoveRating, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = (heroToCheck.GetRegen((tempRegen + toEquip.regen), (tempSpirit + toEquip.Spirit))).ToString();
        if ((heroToCheck.GetRegen((tempRegen + toEquip.regen), (tempSpirit + toEquip.Spirit))) > heroToCheck.GetRegen(heroToCheck.currentRegenRating, heroToCheck.currentSpirit))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Up");
        }
        else if ((heroToCheck.GetRegen((tempRegen + toEquip.regen), (tempSpirit + toEquip.Spirit))) < heroToCheck.GetRegen(heroToCheck.currentRegenRating, heroToCheck.currentSpirit))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Down");
        }
        else if ((heroToCheck.GetRegen((tempRegen + toEquip.regen), (tempSpirit + toEquip.Spirit))) == heroToCheck.GetRegen(heroToCheck.currentRegenRating, heroToCheck.currentSpirit))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = (heroToCheck.GetDodgeChance((tempDodge + toEquip.dodge), (tempAgility + toEquip.Agility))).ToString();
        if ((heroToCheck.GetDodgeChance((tempDodge + toEquip.dodge), (tempAgility + toEquip.Agility))) > heroToCheck.GetDodgeChance(heroToCheck.currentDodgeRating, heroToCheck.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Up");
        }
        else if ((heroToCheck.GetDodgeChance((tempDodge + toEquip.dodge), (tempAgility + toEquip.Agility))) < heroToCheck.GetDodgeChance(heroToCheck.currentDodgeRating, heroToCheck.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Down");
        }
        else if ((heroToCheck.GetDodgeChance((tempDodge + toEquip.dodge), (tempAgility + toEquip.Agility))) == heroToCheck.GetDodgeChance(heroToCheck.currentDodgeRating, heroToCheck.currentAgility))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = (heroToCheck.GetBlockChance((tempBlock + toEquip.block))).ToString();
        if ((heroToCheck.GetBlockChance((tempBlock + toEquip.block))) > heroToCheck.GetBlockChance(heroToCheck.currentBlockRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Up");
        }
        else if ((heroToCheck.GetBlockChance((tempBlock + toEquip.block))) < heroToCheck.GetBlockChance(heroToCheck.currentBlockRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Down");
        }
        else if ((heroToCheck.GetBlockChance((tempBlock + toEquip.block))) == heroToCheck.GetBlockChance(heroToCheck.currentBlockRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = (heroToCheck.GetParryChance((tempParry + toEquip.parry), (tempStrength + toEquip.Strength), (tempDexterity + toEquip.Dexterity))).ToString();
        if ((heroToCheck.GetParryChance((tempParry + toEquip.parry), (tempStrength + toEquip.Strength), (tempDexterity + toEquip.Dexterity))) > heroToCheck.GetParryChance(heroToCheck.currentParryRating, heroToCheck.currentStrength, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Up");
        }
        else if ((heroToCheck.GetParryChance((tempParry + toEquip.parry), (tempStrength + toEquip.Strength), (tempDexterity + toEquip.Dexterity))) < heroToCheck.GetParryChance(heroToCheck.currentParryRating, heroToCheck.currentStrength, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Down");
        }
        else if ((heroToCheck.GetParryChance((tempParry + toEquip.parry), (tempStrength + toEquip.Strength), (tempDexterity + toEquip.Dexterity))) == heroToCheck.GetParryChance(heroToCheck.currentParryRating, heroToCheck.currentStrength, heroToCheck.currentDexterity))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = (heroToCheck.GetThreatRating(tempThreat + toEquip.threat)).ToString();
        if ((heroToCheck.GetThreatRating(tempThreat + toEquip.threat)) > heroToCheck.GetThreatRating(heroToCheck.currentThreatRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Up");
        }
        else if ((heroToCheck.GetThreatRating(tempThreat + toEquip.threat)) < heroToCheck.GetThreatRating(heroToCheck.currentThreatRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Down");
        }
        else if ((heroToCheck.GetThreatRating(tempThreat + toEquip.threat)) == heroToCheck.GetThreatRating(heroToCheck.currentThreatRating))
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
        }
    }

    public void ResetEquipmentStatUpdates()
    {
        GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = heroToCheck.currentStrength.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = heroToCheck.currentStamina.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = heroToCheck.currentAgility.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = heroToCheck.currentDexterity.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = heroToCheck.currentIntelligence.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = heroToCheck.currentSpirit.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");

        GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = heroToCheck.maxHP.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = heroToCheck.maxMP.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");

        GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = heroToCheck.currentATK.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = heroToCheck.currentMATK.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = heroToCheck.currentDEF.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = heroToCheck.currentMDEF.ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");

        GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = heroToCheck.GetHitChance(heroToCheck.currentHitRating, heroToCheck.currentAgility).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = heroToCheck.GetCritChance(heroToCheck.currentCritRating, heroToCheck.currentDexterity).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = heroToCheck.GetMoveRating(heroToCheck.currentMoveRating, heroToCheck.currentDexterity).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = heroToCheck.GetRegen(heroToCheck.currentRegenRating, heroToCheck.currentSpirit).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");

        GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = heroToCheck.GetDodgeChance(heroToCheck.currentDodgeRating, heroToCheck.currentAgility).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = heroToCheck.GetBlockChance(heroToCheck.currentBlockRating).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = heroToCheck.GetParryChance(heroToCheck.currentParryRating, heroToCheck.currentStrength, heroToCheck.currentDexterity).ToString();
        ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
        GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = heroToCheck.GetThreatRating(heroToCheck.currentThreatRating).ToString();
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
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(StatusMenuCanvas);
        menuState = MenuStates.STATUS;
        DrawStatusMenuBaseStats(hero);
        DrawStatusMenuStats(hero);
    }

    void DrawStatusMenuBaseStats(BaseHero hero)
    {
        DrawHeroFace(hero, GameObject.Find("StatusMenuPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/NameText").GetComponent<Text>().text = hero.name; //Name text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/LevelText").GetComponent<Text>().text = hero.currentLevel.ToString(); //Level text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.maxHP.ToString()); //HP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.maxMP.ToString()); //MP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/EXPText").GetComponent<Text>().text = (hero.currentExp + " / " + GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().levelEXPThresholds[hero.currentLevel - 1]); //EXP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/ToNextLevelText").GetComponent<Text>().text = (GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().levelEXPThresholds[hero.currentLevel - 1] - hero.currentExp).ToString(); //To next level text

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
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(StatusMenuCanvas);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
    }

    //--------------------

    //Party Menu

    public void ShowPartyMenu()
    {
        DrawPartyMenu();
    }

    void DrawPartyMenu()
    {
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(PartyMenuCanvas);
        menuState = MenuStates.PARTY;

        DrawPartyActiveHeroes();
        DrawInactiveHeroButtons();
    }

    public void DrawPartyActiveHeroes()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroPartyMenuPanels(heroCount);

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            DrawHeroFace(GameManager.instance.activeHeroes[i], GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(i).Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(i).Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(i).Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].maxHP); //HP text
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].maxMP); //MP text

            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(i).name = "Active Hero Panel - ID " + GameManager.instance.activeHeroes[i].ID; //rename Panel
        }
    }

    void DrawHeroPartyMenuPanels(int count)
    {
        if (count == 1)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawInactivePanel(Hero2PartyPanel);
            DrawInactivePanel(Hero3PartyPanel);
            DrawInactivePanel(Hero4PartyPanel);
            DrawInactivePanel(Hero5PartyPanel);
        }
        else if (count == 2)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawActivePanel(Hero2PartyPanel);
            DrawInactivePanel(Hero3PartyPanel);
            DrawInactivePanel(Hero4PartyPanel);
            DrawInactivePanel(Hero5PartyPanel);
        }
        else if (count == 3)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawActivePanel(Hero2PartyPanel);
            DrawActivePanel(Hero3PartyPanel);
            DrawInactivePanel(Hero4PartyPanel);
            DrawInactivePanel(Hero5PartyPanel);
        }
        else if (count == 4)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawActivePanel(Hero2PartyPanel);
            DrawActivePanel(Hero3PartyPanel);
            DrawActivePanel(Hero4PartyPanel);
            DrawInactivePanel(Hero5PartyPanel);
        }
        else if (count == 5)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawActivePanel(Hero2PartyPanel);
            DrawActivePanel(Hero3PartyPanel);
            DrawActivePanel(Hero4PartyPanel);
            DrawActivePanel(Hero5PartyPanel);
        }

        DrawHeroPartyMenuPanelBars();
    }

    void DrawActivePanel(GameObject panel)
    {
        panel.transform.Find("FacePanel").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("NameText").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("LevelLabel").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("LevelText").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("HPLabel").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("HPText").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("HPProgressBarBG").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("MPLabel").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("MPText").GetComponent<CanvasGroup>().alpha = 1;
        panel.transform.Find("MPProgressBarBG").GetComponent<CanvasGroup>().alpha = 1;
    }

    void DrawInactivePanel(GameObject panel)
    {
        panel.transform.Find("FacePanel").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("NameText").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("LevelLabel").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("LevelText").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("HPLabel").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("HPText").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("HPProgressBarBG").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("MPLabel").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("MPText").GetComponent<CanvasGroup>().alpha = 0;
        panel.transform.Find("MPProgressBarBG").GetComponent<CanvasGroup>().alpha = 0;
        panel.name = "Empty Hero Panel";
    }

    void DrawHeroPartyMenuPanelBars()
    {
        if (GameManager.instance.activeHeroes.Count == 1)
        {
            Hero1PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuHPProgressBar.transform.localScale.y);
            Hero1PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 2)
        {
            Hero1PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuHPProgressBar.transform.localScale.y);
            Hero1PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuMPProgressBar.transform.localScale.y);

            Hero2PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2PartyMenuHPProgressBar.transform.localScale.y);
            Hero2PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2PartyMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 3)
        {
            Hero1PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuHPProgressBar.transform.localScale.y);
            Hero1PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuMPProgressBar.transform.localScale.y);

            Hero2PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2PartyMenuHPProgressBar.transform.localScale.y);
            Hero2PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2PartyMenuMPProgressBar.transform.localScale.y);

            Hero3PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3PartyMenuHPProgressBar.transform.localScale.y);
            Hero3PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3PartyMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 4)
        {
            Hero1PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuHPProgressBar.transform.localScale.y);
            Hero1PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuMPProgressBar.transform.localScale.y);

            Hero2PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2PartyMenuHPProgressBar.transform.localScale.y);
            Hero2PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2PartyMenuMPProgressBar.transform.localScale.y);

            Hero3PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3PartyMenuHPProgressBar.transform.localScale.y);
            Hero3PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3PartyMenuMPProgressBar.transform.localScale.y);

            Hero4PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4PartyMenuHPProgressBar.transform.localScale.y);
            Hero4PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4PartyMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 5)
        {
            Hero1PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuHPProgressBar.transform.localScale.y);
            Hero1PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1PartyMenuMPProgressBar.transform.localScale.y);

            Hero2PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2PartyMenuHPProgressBar.transform.localScale.y);
            Hero2PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2PartyMenuMPProgressBar.transform.localScale.y);

            Hero3PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3PartyMenuHPProgressBar.transform.localScale.y);
            Hero3PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3PartyMenuMPProgressBar.transform.localScale.y);

            Hero4PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4PartyMenuHPProgressBar.transform.localScale.y);
            Hero4PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4PartyMenuMPProgressBar.transform.localScale.y);

            Hero5PartyMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5PartyMenuHPProgressBar.transform.localScale.y);
            Hero5PartyMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5PartyMenuMPProgressBar.transform.localScale.y);
        }

    }

    public void DrawInactiveHeroButtons() //draws inactive hero buttons to panel
    {
        ResetInactiveHeroList();

        foreach (BaseHero hero in GameManager.instance.inactiveHeroes)
        {
            //NewItemPanel = Instantiate(NewItemPanel) as GameObject; //creates gameobject of newItemPanel
            InactivePartyButton = Instantiate(PrefabManager.Instance.inactiveHeroButton);
            InactivePartyButton.transform.Find("NameText").GetComponent<Text>().text = hero.name;
            DrawHeroFace(hero, InactivePartyButton.transform.Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            InactivePartyButton.name = "Inactive Hero Button - ID " + hero.ID;
            
            if (row1ChildCount <= 2)
            {
                InactivePartyButton.transform.SetParent(PartyInactiveRow1Spacer, false);
                row1ChildCount++;
            } else if (row2ChildCount <= 2)
            {
                InactivePartyButton.transform.SetParent(PartyInactiveRow2Spacer, false);
                row2ChildCount++;
            }
            else if (row3ChildCount <= 2)
            {
                InactivePartyButton.transform.SetParent(PartyInactiveRow3Spacer, false);
                row3ChildCount++;
            }
        }
    }

    public void ResetInactiveHeroList()
    {
        foreach (Transform child in PartyInactiveRow1Spacer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in PartyInactiveRow2Spacer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in PartyInactiveRow3Spacer.transform)
        {
            Destroy(child.gameObject);
        }

        row1ChildCount = 0;
        row2ChildCount = 0;
        row3ChildCount = 0;
    }

    void HidePartyMenu()
    {
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(PartyMenuCanvas);
        menuState = MenuStates.MAIN;
    }

    //--------------------

    //Grid Menu

    public void ShowGridMenu() //displays grid menu
    {
        DrawGridMenu();
        DrawHeroGridMenuStats();
    }

    void DrawGridMenu()
    {
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(GridMenuCanvas);
        menuState = MenuStates.GRID;
    }

    void HideGridMenu()
    {
        ResetItemList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(GridMenuCanvas);
        menuState = MenuStates.MAIN;
    }

    public void DrawHeroGridMenuStats()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroGridMenuPanels(heroCount);

        DrawGridFaces();

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            DrawHeroFace(GameManager.instance.activeHeroes[i], GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].maxHP); //HP text
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].maxMP); //MP text
        }
    }

    void DrawGridFaces()
    {
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + hero.spawnPoint).GetComponent<Image>().sprite = hero.faceImage;
        }
    }

    void DrawHeroGridMenuPanels(int count)
    {
        if (count == 1)
        {
            DisplayCanvas(Hero1GridPanel);
            HideCanvas(Hero2GridPanel);
            HideCanvas(Hero3GridPanel);
            HideCanvas(Hero4GridPanel);
            HideCanvas(Hero5GridPanel);
        }
        else if (count == 2)
        {
            DisplayCanvas(Hero1GridPanel);
            DisplayCanvas(Hero2GridPanel);
            HideCanvas(Hero3GridPanel);
            HideCanvas(Hero4GridPanel);
            HideCanvas(Hero5GridPanel);
        }
        else if (count == 3)
        {
            DisplayCanvas(Hero1GridPanel);
            DisplayCanvas(Hero2GridPanel);
            DisplayCanvas(Hero3GridPanel);
            HideCanvas(Hero4GridPanel);
            HideCanvas(Hero5GridPanel);
        }
        else if (count == 4)
        {
            DisplayCanvas(Hero1GridPanel);
            DisplayCanvas(Hero2GridPanel);
            DisplayCanvas(Hero3GridPanel);
            DisplayCanvas(Hero4GridPanel);
            HideCanvas(Hero5GridPanel);
        }
        else if (count == 5)
        {
            DisplayCanvas(Hero1GridPanel);
            DisplayCanvas(Hero2GridPanel);
            DisplayCanvas(Hero3GridPanel);
            DisplayCanvas(Hero4GridPanel);
            DisplayCanvas(Hero5GridPanel);
        }

        DrawHeroGridMenuPanelBars();
    }

    void DrawHeroGridMenuPanelBars()
    {
        if (GameManager.instance.activeHeroes.Count == 1)
        {
            Hero1GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuHPProgressBar.transform.localScale.y);
            Hero1GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 2)
        {
            Hero1GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuHPProgressBar.transform.localScale.y);
            Hero1GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuMPProgressBar.transform.localScale.y);

            Hero2GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2GridMenuHPProgressBar.transform.localScale.y);
            Hero2GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2GridMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 3)
        {
            Hero1GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuHPProgressBar.transform.localScale.y);
            Hero1GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuMPProgressBar.transform.localScale.y);

            Hero2GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2GridMenuHPProgressBar.transform.localScale.y);
            Hero2GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2GridMenuMPProgressBar.transform.localScale.y);

            Hero3GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3GridMenuHPProgressBar.transform.localScale.y);
            Hero3GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3GridMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 4)
        {
            Hero1GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuHPProgressBar.transform.localScale.y);
            Hero1GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuMPProgressBar.transform.localScale.y);

            Hero2GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2GridMenuHPProgressBar.transform.localScale.y);
            Hero2GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2GridMenuMPProgressBar.transform.localScale.y);

            Hero3GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3GridMenuHPProgressBar.transform.localScale.y);
            Hero3GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3GridMenuMPProgressBar.transform.localScale.y);

            Hero4GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4GridMenuHPProgressBar.transform.localScale.y);
            Hero4GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4GridMenuMPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 5)
        {
            Hero1GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuHPProgressBar.transform.localScale.y);
            Hero1GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1GridMenuMPProgressBar.transform.localScale.y);

            Hero2GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2GridMenuHPProgressBar.transform.localScale.y);
            Hero2GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2GridMenuMPProgressBar.transform.localScale.y);

            Hero3GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3GridMenuHPProgressBar.transform.localScale.y);
            Hero3GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3GridMenuMPProgressBar.transform.localScale.y);

            Hero4GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4GridMenuHPProgressBar.transform.localScale.y);
            Hero4GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4GridMenuMPProgressBar.transform.localScale.y);

            Hero5GridMenuHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5GridMenuHPProgressBar.transform.localScale.y);
            Hero5GridMenuMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5GridMenuMPProgressBar.transform.localScale.y);
        }

    }

    //--------------------

    //Talent Menu

    public void ShowTalentsMenu()
    {
        StartCoroutine(ChooseHeroForTalentsMenu());
    }

    IEnumerator ChooseHeroForTalentsMenu()
    {
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }
        DrawTalentsMenu(heroToCheck);
        DrawHeroTalents(heroToCheck);
    }

    void DrawTalentsMenu(BaseHero hero)
    {
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(TalentsMenuCanvas);
        menuState = MenuStates.TALENTS;

        DrawTalentsMenuHeroPanel(hero);
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = "";
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = "";
    }

    public void DrawTalentsMenuHeroPanel(BaseHero hero)
    {
        DrawHeroFace(hero, GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/NameText").GetComponent<Text>().text = hero.name; //Name text
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/LevelText").GetComponent<Text>().text = hero.currentLevel.ToString(); //Level text
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.maxHP.ToString()); //HP text
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.maxMP.ToString()); //MP text

        StatusPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hero), 0, 1), StatusPanelHPProgressBar.transform.localScale.y);
        StatusPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hero), 0, 1), StatusPanelMPProgressBar.transform.localScale.y);
    }

    void DrawHeroTalents(BaseHero hero)
    {
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent1/Talent1Button/TalentIcon"), hero.level1Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent1/Talent2Button/TalentIcon"), hero.level1Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent1/Talent3Button/TalentIcon"), hero.level1Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent2/Talent1Button/TalentIcon"), hero.level2Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent2/Talent2Button/TalentIcon"), hero.level2Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent2/Talent3Button/TalentIcon"), hero.level2Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent3/Talent1Button/TalentIcon"), hero.level3Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent3/Talent2Button/TalentIcon"), hero.level3Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent3/Talent3Button/TalentIcon"), hero.level3Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent4/Talent1Button/TalentIcon"), hero.level4Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent4/Talent2Button/TalentIcon"), hero.level4Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent4/Talent3Button/TalentIcon"), hero.level4Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent5/Talent1Button/TalentIcon"), hero.level5Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent5/Talent2Button/TalentIcon"), hero.level5Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent5/Talent3Button/TalentIcon"), hero.level5Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent6/Talent1Button/TalentIcon"), hero.level6Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent6/Talent2Button/TalentIcon"), hero.level6Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent6/Talent3Button/TalentIcon"), hero.level6Talents[2]);
    }

    void DrawTalentIcon(GameObject obj, BaseTalent talent)
    {
        obj.GetComponent<Image>().sprite = talent.icon;
        if (talent.isActive)
        {
            DrawActiveTalent(obj.GetComponent<Image>());
        } else
        {
            DrawInactiveTalent(obj.GetComponent<Image>());
        }
    }

    void DrawInactiveTalent(Image icon)
    {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, .25f);
    }

    void DrawActiveTalent(Image icon)
    {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
    }

    void HideTalentsMenu()
    {
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(TalentsMenuCanvas);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
    }

    //--------------------

    //Quest Menu

    public void ShowQuestMenu() //displays item menu
    {
        DrawQuestMenu();
    }

    void DrawQuestMenu()
    {
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(QuestsMenuCanvas);
        menuState = MenuStates.QUESTS;

        DrawActiveQuestList();
        DrawCompletedQuestList();

        ShowActiveQuestsPanel();

        ClearQuestMenuFields();
    }

    void HideQuestMenu()
    {
        ResetActiveQuestList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(QuestsMenuCanvas);
        menuState = MenuStates.MAIN;
    }

    public void DrawActiveQuestList() //draws quest to active quest list
    {
        ResetActiveQuestList();

        foreach (BaseQuest quest in GameManager.instance.activeQuests)
        {
            QuestListButton = Instantiate(PrefabManager.Instance.activeQuestListButton);
            QuestListButton.transform.Find("QuestNameText").GetComponent<Text>().text = quest.name;
            QuestListButton.transform.Find("QuestLevelText").GetComponent<Text>().text = quest.level.ToString();
            QuestListButton.name = "ActiveQuestListButton - ID " + quest.ID.ToString();

            QuestListButton.transform.SetParent(ActiveQuestListSpacer, false);
        }
    }

    public void DrawCompletedQuestList() //draws quest to active quest list
    {
        ResetCompletedQuestList();

        foreach (BaseQuest quest in GameManager.instance.completedQuests)
        {
            QuestListButton = Instantiate(PrefabManager.Instance.activeQuestListButton);
            QuestListButton.transform.Find("QuestNameText").GetComponent<Text>().text = quest.name;
            QuestListButton.transform.Find("QuestLevelText").GetComponent<Text>().text = quest.level.ToString();
            QuestListButton.name = "ActiveQuestListButton - ID " + quest.ID.ToString();

            QuestListButton.transform.SetParent(CompletedQuestListSpacer, false);
        }
    }

    public void ShowActiveQuestsPanel()
    {
        QuestOption = "Active";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ShowCompletedQuestsPanel()
    {
        QuestOption = "Completed";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void ClearQuestMenuFields()
    {
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestNamePanel/QuestNameText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestLevelText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq1").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq2").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq3").GetComponent<Text>().text = "";

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestDetailsPanel/QuestDescriptionText").GetComponent<Text>().text = "";

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ExpText").GetComponent<Text>().text = "";

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color = new Color(GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.r, 
            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.g, 
            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.b, 0f);

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel/ItemDescription").GetComponent<Text>().text = "";

        //----

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestNamePanel/QuestNameText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestLevelRequirementsPanel/QuestLevelText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq1").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq2").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestLevelRequirementsPanel/QuestReq3").GetComponent<Text>().text = "";

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestDetailsPanel/QuestDescriptionText").GetComponent<Text>().text = "";

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ExpText").GetComponent<Text>().text = "";

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color = new Color(GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.r,
            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.g,
            GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>().color.b, 0f);

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestRewardsPanel/ItemDescription").GetComponent<Text>().text = "";
    }

    public void ResetActiveQuestList()
    {
        foreach (Transform child in ActiveQuestListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetCompletedQuestList()
    {
        foreach (Transform child in CompletedQuestListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void CancelQuestSelect()
    {
        QuestClicked = false;

        foreach (Transform child in GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform)
        {
            child.Find("QuestNameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
            child.Find("QuestLevelText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        foreach (Transform child in GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform)
        {
            child.Find("QuestNameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
            child.Find("QuestLevelText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        ClearQuestMenuFields();
    }

    //--------------------

    //Bestiary Menu

    public void ShowBestiaryMenu() //displays item menu
    {
        DrawBestiaryMenu();
    }

    void DrawBestiaryMenu()
    {
        HideCanvas(MainMenuCanvas);
        DisplayCanvas(BestiaryMenuCanvas);
        menuState = MenuStates.BESTIARY;

        DrawBestiaryEntryList();
    }

    public void DrawBestiaryEntryList() //draws quest to active quest list
    {
        ResetBestiaryList();

        ClearBestiaryMenuFields();

        foreach (BaseBestiaryEntry entry in GameManager.instance.bestiaryEntries)
        {
            BestiaryEntryButton = Instantiate(PrefabManager.Instance.bestiaryEntryButton);
            BestiaryEntryButton.transform.Find("EnemyNameText").GetComponent<Text>().text = entry.enemy.name;
            BestiaryEntryButton.name = "BestiaryEnemyEntryButton - ID " + entry.enemy.ID.ToString();

            BestiaryEntryButton.transform.SetParent(BestiaryEnemyListSpacer, false);
        }
    }

    public void ClearBestiaryMenuFields()
    {
        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyNamePanel/EnemyNameText").GetComponent<Text>().text = "";
        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyLevelPanel/EnemyLevelText").GetComponent<Text>().text = "";

        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color = new Color(
            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.r,
            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.g,
            GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().color.b,
            0);
        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel/EnemyGraphicImage").GetComponent<Image>().sprite = null;

        GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyDescriptionPanel/EnemyDescriptionText").GetComponent<Text>().text = "";
    }

    void CancelBestiarySelect()
    {
        BestiaryEntryClicked = false;

        foreach (Transform child in GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform)
        {
            child.Find("EnemyNameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
        ClearBestiaryMenuFields();
    }

    public void ResetBestiaryList()
    {
        foreach (Transform child in BestiaryEnemyListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void HideBestiaryMenu()
    {
        ResetActiveQuestList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(BestiaryMenuCanvas);
        menuState = MenuStates.MAIN;
    }

    //--------------------

    //Methods for running above menus

    public void PauseBackground(bool pause) //keeps background objects from processing while pause=true by enabling the script's inMenu
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
            baseLineEXP = (GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().levelEXPThresholds[hero.currentLevel - 1]);
            heroEXP = hero.currentExp;
        } else
        {
            baseLineEXP = (GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().levelEXPThresholds[hero.currentLevel - 1] - GameObject.Find("GameManager/HeroDB").GetComponent<HeroDB>().levelEXPThresholds[hero.currentLevel - 2]);
            heroEXP = (hero.currentExp - baseLineEXP);
            //Debug.Log("baseLine: " + baseLineEXP);
        }

        //Debug.Log(heroEXP + " / " + baseLineEXP + ": " + calcEXP);

        float calcEXP = heroEXP / baseLineEXP;

        return calcEXP;
    }

    public void DrawHeroFace(BaseHero hero, Image faceImage)
    {
        if (GameManager.instance.activeHeroes.Contains(hero))
        {
            int index = GameManager.instance.activeHeroes.IndexOf(hero);
            faceImage.sprite = GameManager.instance.activeHeroes[index].faceImage;
        }

        if (GameManager.instance.inactiveHeroes.Contains(hero))
        {
            int index = GameManager.instance.inactiveHeroes.IndexOf(hero);
            faceImage.sprite = GameManager.instance.inactiveHeroes[index].faceImage;
        }
    }

    BaseHero GetHeroByID(int ID)
    {
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            if (hero.ID == ID)
            {
                return hero;
            }
        }

        foreach (BaseHero hero in GameManager.instance.inactiveHeroes)
        {
            if (hero.ID == ID)
            {
                return hero;
            }
        }

        return null;
    }
}
