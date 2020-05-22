using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using System;

public class GameMenu : MonoBehaviour
{
    CanvasGroup mainMenuCanvasGroup;

    GameObject player;
    public bool drawingGUI = false;
    public bool disableMenu = false;
    public bool disableMenuExit = false;
    [HideInInspector]public bool menuCalled = false;

    public bool choosingHero = false;

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
    private Transform KeyItemListSpacer;
    GameObject NewItemPanel;
    [HideInInspector] public bool itemChoosingHero;
    [HideInInspector] public bool itemCustomizeModeOn;
    [HideInInspector] public int itemIndexSwapA;
    [HideInInspector] public int itemIndexSwapB;
    [HideInInspector] public bool itemIndexSwapAPicked;

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
    public bool choosingHeroForMagicMenu = false;

    //for equip menu objects
   Image EquipPanelHPProgressBar;
   Image EquipPanelMPProgressBar;
   GameObject NewEquipPanel;
   Transform EquipListSpacer;
   public string equipButtonClicked;
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

        heroToCheck = null;

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
        ItemListSpacer = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform;
        KeyItemListSpacer = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform;
        WhiteMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/WhiteMagicListPanel/WhiteMagicScroller/WhiteMagicListSpacer").transform;
        BlackMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/BlackMagicListPanel/BlackMagicScroller/BlackMagicListSpacer").transform;
        SorceryMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/SorceryMagicListPanel/SorceryMagicScroller/SorceryMagicListSpacer").transform;
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

        TalentsPanelHPProgressBar = GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/HPProgressBarBG/HPProgressBar").GetComponent<Image>();
        TalentsPanelMPProgressBar = GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/MPProgressBarBG/MPProgressBar").GetComponent<Image>();

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

            UnboldButton(MagicButton);
            UnboldButton(EquipButton);
            UnboldButton(StatusButton);
            UnboldButton(TalentsButton);
            GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero1Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
            GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero2Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
            GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero3Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
            GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero4Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
            GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero5Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
            heroToCheck = null;
            choosingHero = false;

            //HideCanvas(MainMenuCanvas);
            PauseBackground(false);
            menuCalled = false;
            menuState = MenuStates.IDLE;
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.ITEM && !buttonPressed && !itemCustomizeModeOn && !itemChoosingHero) //if cancel is pressed on item menu
        {
            HideItemMenu();
            ShowMainMenu();
        }

        if (Input.GetButtonDown("Cancel") && menuState == MenuStates.MAGIC && !buttonPressed && !choosingHeroForMagicMenu) //if cancel is pressed on magic menu
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

        if (Input.GetButtonDown("Cancel") && !buttonPressed && itemCustomizeModeOn)
        {
            CancelCustomizeMode();
        }

        if (Input.GetButtonDown("Cancel") && !buttonPressed && itemChoosingHero)
        {
            CancelItemChoosingHero();
        }

        if (Input.GetButtonDown("Cancel") && !buttonPressed && choosingHeroForMagicMenu)
        {
            CancelMagicChoosingHero();
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
            GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].finalMaxHP); //HP text
            GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].finalMaxMP); //MP text
            GameObject.Find("MainMenuCanvas/HeroInfoPanel").transform.GetChild(i).Find("EXPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].currentExp + " / " + HeroDB.instance.levelEXPThresholds[(GameManager.instance.activeHeroes[i].currentLevel -1)]); //Exp text
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
        ShowUseItemMenu();

        HideArrangeMenu();
        itemCustomizeModeOn = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/ArrangeButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
    }

    void HideItemMenu()
    {
        ResetItemList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(ItemMenuCanvas);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
        itemCustomizeModeOn = false;
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
            if (!itemsAccountedFor.Contains(item) && item.type != Item.Types.KEYITEM)
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

            if (item.type == Item.Types.KEYITEM)
            {
                NewItemPanel = Instantiate(PrefabManager.Instance.keyItemPrefab);
                NewItemPanel.transform.GetChild(0).GetComponent<Text>().text = item.name;
                NewItemPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
                if (!item.usableInMenu)
                {

                }
                NewItemPanel.transform.SetParent(KeyItemListSpacer, false);
                itemsAccountedFor.Add(item);
            }
        }

        itemsAccountedFor.Clear();
    }

    public void ResetItemList()
    {
        foreach (Transform child in ItemListSpacer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in KeyItemListSpacer.transform)
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
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].finalMaxHP); //HP text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].finalMaxMP); //MP text
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

    public void ShowUseItemMenu()
    {
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/UseButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/KeyItemsButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
    }

    public void ShowKeyItemMenu()
    {
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/UseButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/KeyItemsButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
    }

    public void ShowArrangeMenu()
    {
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    void HideArrangeMenu()
    {
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ItemArrange(string mode)
    {
        if (mode == "Customize")
        {
            itemCustomizeModeOn = true;
            GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/ArrangeButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        } else
        {
            SortItems(mode);
            DrawItemList();
        }

        HideArrangeMenu();
    }

    void SortItems(string option)
    {
        CancelCustomizeMode();

        List<Item> items = Inventory.instance.items;

        if (option == "Field")
        {
            List<Item> fieldItems = new List<Item>();
            List<Item> battleItems = new List<Item>();
            List<Item> equipmentItems = new List<Item>();

            foreach (Item item in items)
            {
                if (item is Equipment)
                {
                    equipmentItems.Add(item);
                }
                else
                {
                    if (item.usableInMenu)
                    {
                        fieldItems.Add(item);
                    }
                    else
                    {
                        battleItems.Add(item);
                    }
                }
            }

            items.Clear(); //resets item list for adding them again with new sorting

            foreach (Item item in fieldItems)
            {
                items.Add(item);
            }

            foreach (Item item in battleItems)
            {
                items.Add(item);
            }

            foreach (Item item in equipmentItems)
            {
                items.Add(item);
            }

            Inventory.instance.items = items;
        }

        if (option == "Battle")
        {
            List<Item> fieldItems = new List<Item>();
            List<Item> battleItems = new List<Item>();
            List<Item> equipmentItems = new List<Item>();

            foreach (Item item in items)
            {
                if (item is Equipment)
                {
                    equipmentItems.Add(item);
                }
                else
                {
                    if (item.usableInMenu)
                    {
                        fieldItems.Add(item);
                    }
                    else
                    {
                        battleItems.Add(item);
                    }
                }
            }

            items.Clear(); //resets item list for adding them again with new sorting

            foreach (Item item in battleItems)
            {
                items.Add(item);
            }

            foreach (Item item in fieldItems)
            {
                items.Add(item);
            }

            foreach (Item item in equipmentItems)
            {
                items.Add(item);
            }

            Inventory.instance.items = items;
        }

        if (option == "Type")
        {
            List<Item> restoreItems = new List<Item>();
            List<Item> healStatusItems = new List<Item>();
            List<Item> damageItems = new List<Item>();
            List<Item> inflictStatusItems = new List<Item>();
            List<Item> equipmentItems = new List<Item>();

            foreach (Item item in items)
            {
                if (item is Equipment)
                {
                    equipmentItems.Add(item);
                }
                else
                {
                    if (item.type == Item.Types.RESTORATIVE)
                    {
                        restoreItems.Add(item);
                    }
                    else if (item.type == Item.Types.HEALSTATUS)
                    {
                        healStatusItems.Add(item);
                    }
                    else if (item.type == Item.Types.DAMAGE)
                    {
                        damageItems.Add(item);
                    }
                    else if (item.type == Item.Types.INFLICTSTATUS)
                    {
                        inflictStatusItems.Add(item);
                    }
                }
            }

            items.Clear(); //resets item list for adding them again with new sorting

            foreach (Item item in restoreItems)
            {
                items.Add(item);
            }

            foreach (Item item in healStatusItems)
            {
                items.Add(item);
            }

            foreach (Item item in damageItems)
            {
                items.Add(item);
            }

            foreach (Item item in inflictStatusItems)
            {
                items.Add(item);
            }

            foreach (Item item in equipmentItems)
            {
                items.Add(item);
            }

            Inventory.instance.items = items;

        }

        if (option == "Name")
        {
            List<Item> normalItems = new List<Item>();
            List<Item> equipment = new List<Item>();

            foreach (Item item in Inventory.instance.items)
            {
                if (item is Equipment)
                {
                    equipment.Add(item);
                }
                else
                {
                    normalItems.Add(item);
                }
            }

            items.Clear();

            normalItems = normalItems.OrderBy(item => item.name).ToList();
            equipment = equipment.OrderBy(item => item.name).ToList();

            foreach (Item item in normalItems)
            {
                items.Add(item);
            }

            foreach (Item item in equipment)
            {
                items.Add(item);
            }

            Inventory.instance.items = items;
        }

        if (option == "Most")
        {
            List<Item> tempItems = Inventory.instance.items;
            List<Item> sortedItems = new List<Item>();
            List<Item> sortedEquips = new List<Item>();
            List<Item> itemsAccountedFor = new List<Item>();

            int itemCount = 0;

            Item highestCountItem = null;
            int highestCount = 0;

            List<Item> diffItems = new List<Item>();

            foreach (Item item in tempItems) //to get the individual items (discluding count)
            {
                if (!diffItems.Contains(item))
                {
                    diffItems.Add(item);
                }
            }

            for (int c = 0; c < diffItems.Count; c++)
            {
                foreach (Item item in tempItems)
                {
                    if (!itemsAccountedFor.Contains(item))
                    {
                        for (int i = 0; i < tempItems.Count; i++)
                        {
                            if (tempItems[i] == item)
                            {
                                itemCount++;
                            }
                        }
                    }

                    if (itemCount > highestCount)
                    {
                        highestCountItem = item; //gets the item in the entire list with the highest count
                        highestCount = itemCount;
                    }
                    itemCount = 0;
                }

                //add the highest item
                for (int i = 0; i < highestCount; i++)
                {
                    if (highestCountItem is Equipment)
                    {
                        sortedEquips.Add(highestCountItem);
                    }
                    else
                    {
                        sortedItems.Add(highestCountItem);
                    }
                }

                itemsAccountedFor.Add(highestCountItem);
                highestCount = 0;
            }
            Inventory.instance.items = sortedItems;

            foreach (Item item in sortedEquips)
            {
                Inventory.instance.Add(item);
            }
        }

        if (option == "Least")
        {
            List<Item> tempItems = Inventory.instance.items;
            List<Item> sortedItems = new List<Item>();
            List<Item> sortedEquips = new List<Item>();
            List<Item> itemsAccountedFor = new List<Item>();

            int itemCount = 0;

            Item lowestCountItem = null;
            int lowestCount = 0;

            List<Item> diffItems = new List<Item>();

            foreach (Item item in tempItems) //to get the individual items (discluding count)
            {
                if (!diffItems.Contains(item))
                {
                    diffItems.Add(item);
                }
            }

            for (int c = 0; c < diffItems.Count; c++)
            {
                foreach (Item item in tempItems)
                {
                    if (!itemsAccountedFor.Contains(item))
                    {
                        for (int i = 0; i < tempItems.Count; i++)
                        {
                            if (tempItems[i] == item)
                            {
                                itemCount++;
                            }
                        }

                        if (itemCount < lowestCount || lowestCount == 0)
                        {
                            lowestCountItem = item; //gets the item in the entire list with the highest count
                            lowestCount = itemCount;
                        }
                        itemCount = 0;
                    }
                }

                //add the highest item
                for (int i = 0; i < lowestCount; i++)
                { 
                    if (lowestCountItem is Equipment)
                    {
                        sortedEquips.Add(lowestCountItem);
                    } else
                    {
                        sortedItems.Add(lowestCountItem);
                    }
                    
                }

                itemsAccountedFor.Add(lowestCountItem);
                lowestCount = 0;
            }

            Inventory.instance.items = sortedItems;

            foreach (Item item in sortedEquips)
            {
                Inventory.instance.Add(item);
            }
        }
    }

    void CancelCustomizeMode()
    {
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/ArrangeButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
        itemCustomizeModeOn = false;
    }

    void CancelItemChoosingHero()
    {
        foreach (Transform child in GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform)
        {
            child.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        itemChoosingHero = false;
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
        choosingHero = true;
        BoldButton(MagicButton);
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }
        UnboldButton(MagicButton);
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

        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/WhiteMagicButton/WhiteMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/BlackMagicButton/BlackMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/SorceryMagicButton/SorceryMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

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
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/WhiteMagicButton/WhiteMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/BlackMagicButton/BlackMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/SorceryMagicButton/SorceryMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        DisplayCanvas(WhiteMagicListPanel);
        HideCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);
    }

    public void ShowBlackMagicListPanel()
    { 
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/WhiteMagicButton/WhiteMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/BlackMagicButton/BlackMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/SorceryMagicButton/SorceryMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        HideCanvas(WhiteMagicListPanel);
        DisplayCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);
    }

    public void ShowSorceryMagicListPanel()
    {
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/WhiteMagicButton/WhiteMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/BlackMagicButton/BlackMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/SorceryMagicButton/SorceryMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;

        HideCanvas(WhiteMagicListPanel);
        HideCanvas(BlackMagicListPanel);
        DisplayCanvas(SorceryMagicListPanel);
    }

    public void DrawMagicMenuStats(BaseHero hero)
    {
        DrawHeroFace(hero, GameObject.Find("HeroMagicPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("HeroMagicPanel/NameText").GetComponent<Text>().text = hero.name; //Name text
        GameObject.Find("HeroMagicPanel/LevelText").GetComponent<Text>().text = hero.currentLevel.ToString(); //Level text
        GameObject.Find("HeroMagicPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.finalMaxHP.ToString()); //HP text
        GameObject.Find("HeroMagicPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.finalMaxMP.ToString()); //MP text

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

    void CancelMagicChoosingHero()
    {
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero1SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero2SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero3SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero4SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero5SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();

        HideCanvas(HeroSelectMagicPanel);

        choosingHeroForMagicMenu = false;
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
        choosingHero = true;
        BoldButton(EquipButton);
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }
        UnboldButton(EquipButton);
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

    public void DrawEquipMenuStats(BaseHero hero)
    {
        //For HeroEquipPanel
        DrawHeroFace(hero, GameObject.Find("HeroEquipPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("HeroEquipPanel/NameText").GetComponent<Text>().text = hero.name; //Name text
        GameObject.Find("HeroEquipPanel/LevelText").GetComponent<Text>().text = hero.currentLevel.ToString(); //Level text
        GameObject.Find("HeroEquipPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.finalMaxHP.ToString()); //HP text
        GameObject.Find("HeroEquipPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.finalMaxMP.ToString()); //MP text

        EquipPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hero), 0, 1), EquipPanelHPProgressBar.transform.localScale.y);
        EquipPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hero), 0, 1), EquipPanelMPProgressBar.transform.localScale.y);
        
        //For EquipStatsPanel
        //STR-SPI
        GameObject.Find("EquipStatsPanel/BaseStrengthText").GetComponent<Text>().text = hero.finalStrength.ToString();
        GameObject.Find("EquipStatsPanel/BaseStaminaText").GetComponent<Text>().text = hero.finalStamina.ToString();
        GameObject.Find("EquipStatsPanel/BaseAgilityText").GetComponent<Text>().text = hero.finalAgility.ToString();
        GameObject.Find("EquipStatsPanel/BaseDexterityText").GetComponent<Text>().text = hero.finalDexterity.ToString();
        GameObject.Find("EquipStatsPanel/BaseIntelligenceText").GetComponent<Text>().text = hero.finalIntelligence.ToString();
        GameObject.Find("EquipStatsPanel/BaseSpiritText").GetComponent<Text>().text = hero.finalSpirit.ToString();

        GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = hero.finalStrength.ToString();
        GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = hero.finalStamina.ToString();
        GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = hero.finalAgility.ToString();
        GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = hero.finalDexterity.ToString();
        GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = hero.finalIntelligence.ToString();
        GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = hero.finalSpirit.ToString();

        //HP & MP
        GameObject.Find("EquipStatsPanel/BaseHPText").GetComponent<Text>().text = hero.finalMaxHP.ToString();
        GameObject.Find("EquipStatsPanel/BaseMPText").GetComponent<Text>().text = hero.finalMaxMP.ToString();

        GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = hero.finalMaxHP.ToString();
        GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = hero.finalMaxMP.ToString();

        //ATK-MDEF
        GameObject.Find("EquipStatsPanel/BaseAttackText").GetComponent<Text>().text = hero.finalATK.ToString();
        GameObject.Find("EquipStatsPanel/BaseMagicAttackText").GetComponent<Text>().text = hero.finalMATK.ToString();
        GameObject.Find("EquipStatsPanel/BaseDefenseText").GetComponent<Text>().text = hero.finalDEF.ToString();
        GameObject.Find("EquipStatsPanel/BaseMagicDefenseText").GetComponent<Text>().text = hero.finalMDEF.ToString();

        GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = hero.finalATK.ToString();
        GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = hero.finalMATK.ToString();
        GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = hero.finalDEF.ToString();
        GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = hero.finalMDEF.ToString();

        //Other stats
        GameObject.Find("EquipStatsPanel/BaseHitText").GetComponent<Text>().text = hero.GetHitChance(hero.finalHitRating, hero.finalAgility).ToString();
        GameObject.Find("EquipStatsPanel/BaseCritText").GetComponent<Text>().text = hero.GetCritChance(hero.finalCritRating, hero.finalDexterity).ToString();
        GameObject.Find("EquipStatsPanel/BaseMPRegenText").GetComponent<Text>().text = hero.GetRegen(hero.finalRegenRating, hero.finalSpirit).ToString();
        GameObject.Find("EquipStatsPanel/BaseMoveText").GetComponent<Text>().text = hero.GetMoveRating(hero.finalMoveRating, hero.finalDexterity).ToString();
        GameObject.Find("EquipStatsPanel/BaseDodgeText").GetComponent<Text>().text = hero.GetDodgeChance(hero.finalDodgeRating, hero.finalAgility).ToString();
        GameObject.Find("EquipStatsPanel/BaseBlockText").GetComponent<Text>().text = hero.GetBlockChance(hero.finalBlockRating).ToString();
        GameObject.Find("EquipStatsPanel/BaseParryText").GetComponent<Text>().text = hero.GetParryChance(hero.finalParryRating, hero.finalStrength, hero.finalDexterity).ToString();
        GameObject.Find("EquipStatsPanel/BaseThreatText").GetComponent<Text>().text = hero.GetThreatRating(hero.finalThreatRating).ToString();

        GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = hero.GetHitChance(hero.finalHitRating, hero.finalAgility).ToString();
        GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = hero.GetCritChance(hero.finalCritRating, hero.finalDexterity).ToString();
        GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = hero.GetRegen(hero.finalRegenRating, hero.finalSpirit).ToString();
        GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = hero.GetMoveRating(hero.finalMoveRating, hero.finalDexterity).ToString();
        GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = hero.GetDodgeChance(hero.finalDodgeRating, hero.finalAgility).ToString();
        GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = hero.GetBlockChance(hero.finalBlockRating).ToString();
        GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = hero.GetParryChance(hero.finalParryRating, hero.finalStrength, hero.finalDexterity).ToString();
        GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = hero.GetThreatRating(hero.finalThreatRating).ToString();
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

            heroToCheck.GetCurrentStatsFromEquipment();
            heroToCheck.UpdateStatsFromTalents();

            DrawEquipMenuStats(heroToCheck);
            
            UpdateEquipmentArrowsToNeutral();
        }
    }

    public void RemoveAllEquipment()
    {
        string equipMenuBase = "GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipSlotsPanel/";

        Color temp;

        foreach (Transform child in GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform)
        {
            Destroy(child.gameObject);
        }
        
        heroToCheck.Unequip(0);

        GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.Unequip(1);

        GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.Unequip(2);

        GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "WristsSlot/WristsButton/WristsSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.Unequip(3);

        GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.Unequip(4);

        GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.Unequip(5);

        GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.Unequip(6);

        GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "RightHandSlot/RightHandButton/RightHandSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.Unequip(7);

        GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "LeftHandSlot/LeftHandButton/LeftHandSlotIcon").GetComponent<Image>().color = temp;
        
        heroToCheck.GetCurrentStatsFromEquipment();
        heroToCheck.UpdateStatsFromTalents();

        UpdateEquipmentArrowsToNeutral();

        DrawEquipMenuStats(heroToCheck);
    }

    public void ChangeEquipment(Equipment toEquip)
    {
        string equipMenuBase = "GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipSlotsPanel/";

        if (equipMode == "Equip")
        {
            if (toEquip == null)
            {
                foreach (Transform child in EquipListSpacer.transform)
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

                UpdateEquipmentArrowsToNeutral();

                heroToCheck.GetCurrentStatsFromEquipment();

                heroToCheck.UpdateStatsFromTalents();

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

            UpdateEquipmentArrowsToNeutral();

            heroToCheck.GetCurrentStatsFromEquipment();

            heroToCheck.UpdateStatsFromTalents();

            DrawEquipMenuStats(heroToCheck);

            inEquipList = false;

            foreach (Item item in Inventory.instance.items.ToList())
            {
                if (item.name == "New Item")
                {
                    item.RemoveFromInventory();
                }
            }
        }
    }

    private void UpdateEquipmentArrowsToNeutral()
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
        ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
        
        ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
        ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
    }

    Equipment GetCurrentEquippedInSlot()
    {
        if (equipButtonClicked == "HeadButton")
        {
            return heroToCheck.equipment[0];
        }

        if (equipButtonClicked == "ChestButton")
        {
            return heroToCheck.equipment[1];
        }

        if (equipButtonClicked == "WristsButton")
        {
            return heroToCheck.equipment[2];
        }

        if (equipButtonClicked == "LegsButton")
        {
            return heroToCheck.equipment[3];
        }

        if (equipButtonClicked == "FeetButton")
        {
            return heroToCheck.equipment[4];
        }

        if (equipButtonClicked == "RelicButton")
        {
            return heroToCheck.equipment[5];
        }

        if (equipButtonClicked == "RightHandButton")
        {
            return heroToCheck.equipment[6];
        }

        if (equipButtonClicked == "LeftHandButton")
        {
            return heroToCheck.equipment[7];
        }

        return null;
    }

    int GetCurrentEquippedSlotIndex()
    {
        if (equipButtonClicked == "HeadButton")
        {
            return 0;
        }

        if (equipButtonClicked == "ChestButton")
        {
            return 1;
        }

        if (equipButtonClicked == "WristsButton")
        {
            return 2;
        }

        if (equipButtonClicked == "LegsButton")
        {
            return 3;
        }

        if (equipButtonClicked == "FeetButton")
        {
            return 4;
        }

        if (equipButtonClicked == "RelicButton")
        {
            return 5;
        }

        if (equipButtonClicked == "RightHandButton")
        {
            return 6;
        }

        if (equipButtonClicked == "LeftHandButton")
        {
            return 7;
        }

        Debug.LogWarning("Illegal slot index, returning 0");
        return 0;
    }

    BaseHero TempHeroForEquip()
    {
        BaseHero tempHero = new BaseHero();

        tempHero.baseHP = heroToCheck.baseHP;
        tempHero.baseMP = heroToCheck.baseMP;

        tempHero.baseATK = heroToCheck.baseATK;
        tempHero.baseMATK = heroToCheck.baseMATK;
        tempHero.baseDEF = heroToCheck.baseDEF;
        tempHero.baseMDEF = heroToCheck.baseMDEF;

        tempHero.baseSTR = heroToCheck.baseSTR;
        tempHero.baseSTA = heroToCheck.baseSTA;
        tempHero.baseDEX = heroToCheck.baseDEX;
        tempHero.baseAGI = heroToCheck.baseAGI;
        tempHero.baseINT = heroToCheck.baseINT;
        tempHero.baseSPI = heroToCheck.baseSPI;

        tempHero.strengthMod = heroToCheck.strengthMod;
        tempHero.staminaMod = heroToCheck.staminaMod;
        tempHero.intelligenceMod = heroToCheck.intelligenceMod;
        tempHero.dexterityMod = heroToCheck.dexterityMod;
        tempHero.agilityMod = heroToCheck.agilityMod;
        tempHero.spiritMod = heroToCheck.spiritMod;

        tempHero.baseHit = heroToCheck.baseHit;
        tempHero.baseCrit = heroToCheck.baseCrit;
        tempHero.baseMove = heroToCheck.baseMove;
        tempHero.baseRegen = heroToCheck.baseRegen;

        tempHero.baseDodge = heroToCheck.baseDodge;
        tempHero.baseBlock = heroToCheck.baseBlock;
        tempHero.baseParry = heroToCheck.baseParry;
        tempHero.baseThreat = heroToCheck.baseThreat;

        for (int i = 0; i < 8; i++)
        {
            tempHero.equipment[i] = heroToCheck.equipment[i];
        }

        tempHero.currentLevel = heroToCheck.currentLevel;

        tempHero.level1Talents = heroToCheck.level1Talents;
        tempHero.level2Talents = heroToCheck.level2Talents;
        tempHero.level3Talents = heroToCheck.level3Talents;
        tempHero.level4Talents = heroToCheck.level4Talents;
        tempHero.level5Talents = heroToCheck.level5Talents;
        tempHero.level6Talents = heroToCheck.level6Talents;

        //tempHero.InitializeStats();

        tempHero.finalMaxHP = heroToCheck.finalMaxHP;
        tempHero.finalMaxMP = heroToCheck.finalMaxMP;

        tempHero.finalStrength = heroToCheck.finalStrength;
        tempHero.finalStamina = heroToCheck.finalStamina;
        tempHero.finalAgility = heroToCheck.finalAgility;
        tempHero.finalDexterity = heroToCheck.finalDexterity;
        tempHero.finalIntelligence = heroToCheck.finalIntelligence;
        tempHero.finalSpirit = heroToCheck.finalSpirit;
        tempHero.finalATK = heroToCheck.finalATK;
        tempHero.finalMATK = heroToCheck.finalMATK;
        tempHero.finalDEF = heroToCheck.finalDEF;
        tempHero.finalMDEF = heroToCheck.finalMDEF;

        tempHero.finalHitRating = heroToCheck.finalHitRating;
        tempHero.finalCritRating = heroToCheck.finalCritRating;
        tempHero.finalMoveRating = heroToCheck.finalMoveRating;
        tempHero.finalRegenRating = heroToCheck.finalRegenRating;

        tempHero.finalDodgeRating = heroToCheck.finalDodgeRating;
        tempHero.finalBlockRating = heroToCheck.finalBlockRating;
        tempHero.finalParryRating = heroToCheck.finalParryRating;
        tempHero.finalThreatRating = heroToCheck.finalThreatRating;

        return tempHero;
    }

    Equipment TempEquip(Equipment toEquip)
    {
        Equipment newEquip = ScriptableObject.CreateInstance("Equipment") as Equipment;

        newEquip.Strength = toEquip.Strength;
        newEquip.Stamina = toEquip.Stamina;
        newEquip.Agility = toEquip.Agility;
        newEquip.Dexterity = toEquip.Dexterity;
        newEquip.Intelligence = toEquip.Intelligence;
        newEquip.Spirit = toEquip.Spirit;

        newEquip.ATK = toEquip.ATK;
        newEquip.DEF = toEquip.DEF;

        newEquip.MATK = toEquip.MATK;
        newEquip.MDEF = toEquip.MDEF;

        newEquip.threat = toEquip.threat;

        newEquip.hit = toEquip.hit;
        newEquip.crit = toEquip.crit;

        newEquip.move = toEquip.move;
        newEquip.regen = toEquip.regen;

        newEquip.dodge = toEquip.dodge;
        newEquip.parry = toEquip.parry;
        newEquip.block = toEquip.block;

        return newEquip;
}

    public void ShowEquipmentStatUpdates(Equipment toEquip)
    {
        //heroToCheck.GetCurrentStatsFromEquipment();
        //heroToCheck.UpdateStatsFromTalents();

        BaseHero tempHero = new BaseHero();
        tempHero = TempHeroForEquip();
        
        if (toEquip != null) //showing stats for valid equipment
        {
            //Debug.Log(toEquip.name);
            int slotIndex = (int)toEquip.equipmentSlot;
            Equipment tempEquip = TempEquip(toEquip);
            tempHero.equipment[slotIndex] = tempEquip;
            
        } else //showing stats for unequipping
        {
            //get item currently equipped in this slot
            if (GetCurrentEquippedInSlot() != null) //subtract stats from currently equipped weapon
            {
                tempHero.equipment[GetCurrentEquippedSlotIndex()] = null;
            }
        }
        
        tempHero.GetCurrentStatsFromEquipment();
        tempHero.UpdateStatsFromTalents();

        GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = tempHero.finalStrength.ToString();
            if (tempHero.finalStrength > heroToCheck.finalStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Up");
            }
            else if (tempHero.finalStrength < heroToCheck.finalStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Down");
            }
            else if (tempHero.finalStrength == heroToCheck.finalStrength)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = tempHero.finalStamina.ToString();
            
            if (tempHero.finalStamina > heroToCheck.finalStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Up");
            }
            else if (tempHero.finalStamina < heroToCheck.finalStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Down");
            }
            else if (tempHero.finalStamina == heroToCheck.finalStamina)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = tempHero.finalAgility.ToString();
            if (tempHero.finalAgility > heroToCheck.finalAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Up");
            }
            if (tempHero.finalAgility < heroToCheck.finalAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Down");
            }
            else if (tempHero.finalAgility == heroToCheck.finalAgility)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = tempHero.finalDexterity.ToString();
            if (tempHero.finalDexterity > heroToCheck.finalDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Up");
            }
            else if (tempHero.finalDexterity < heroToCheck.finalDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Down");
            }
            else if (tempHero.finalDexterity == heroToCheck.finalDexterity)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = tempHero.finalIntelligence.ToString();
            if (tempHero.finalIntelligence > heroToCheck.finalIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Up");
            }
            else if (tempHero.finalIntelligence < heroToCheck.finalIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Down");
            }
            else if (tempHero.finalIntelligence == heroToCheck.finalIntelligence)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = tempHero.finalSpirit.ToString();
            if (tempHero.finalSpirit > heroToCheck.finalSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Up");
            }
            else if (tempHero.finalSpirit < heroToCheck.finalSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Down");
            }
            else if (tempHero.finalSpirit == heroToCheck.finalSpirit)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = tempHero.finalMaxHP.ToString();
            if (tempHero.finalMaxHP > heroToCheck.finalMaxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Up");
            }
            else if (tempHero.finalMaxHP < heroToCheck.finalMaxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Down");
            }
            else if (tempHero.finalMaxHP == heroToCheck.finalMaxHP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = tempHero.finalMaxMP.ToString();
            if (tempHero.finalMaxMP > heroToCheck.finalMaxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Up");
            }
            else if (tempHero.finalMaxMP < heroToCheck.finalMaxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Down");
            }
            else if (tempHero.finalMaxMP == heroToCheck.finalMaxMP)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = tempHero.finalATK.ToString();
            if (tempHero.finalATK > heroToCheck.finalATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Up");
            }
            else if (tempHero.finalATK < heroToCheck.finalATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Down");
            }
            else if (tempHero.finalATK == heroToCheck.finalATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = tempHero.finalMATK.ToString();
            if (tempHero.finalMATK > heroToCheck.finalMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Up");
            }
            else if (tempHero.finalMATK < heroToCheck.finalMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Down");
            }
            else if (tempHero.finalMATK == heroToCheck.finalMATK)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = tempHero.finalDEF.ToString();
            if (tempHero.finalDEF > heroToCheck.finalDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Up");
            }
            else if (tempHero.finalDEF < heroToCheck.finalDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Down");
            }
            else if (tempHero.finalDEF == heroToCheck.finalDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = tempHero.finalMDEF.ToString();
            if (tempHero.finalMDEF > heroToCheck.finalMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Up");
            }
            else if (tempHero.finalMDEF < heroToCheck.finalMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Down");
            }
            else if (tempHero.finalMDEF == heroToCheck.finalMDEF)
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");
            }


            GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = tempHero.GetHitChance(tempHero.finalHitRating, tempHero.finalAgility).ToString();
            if (tempHero.GetHitChance(tempHero.finalHitRating, tempHero.finalAgility) > heroToCheck.GetHitChance(heroToCheck.finalHitRating, heroToCheck.finalAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Up");
            }
            else if (tempHero.GetHitChance(tempHero.finalHitRating, tempHero.finalAgility) < heroToCheck.GetHitChance(heroToCheck.finalHitRating, heroToCheck.finalAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Down");
            }
            else if (tempHero.GetHitChance(tempHero.finalHitRating, tempHero.finalAgility) == heroToCheck.GetHitChance(heroToCheck.finalHitRating, heroToCheck.finalAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = tempHero.GetCritChance(tempHero.finalCritRating, tempHero.finalDexterity).ToString();
            if (tempHero.GetCritChance(tempHero.finalCritRating, tempHero.finalDexterity) > heroToCheck.GetCritChance(heroToCheck.finalCritRating, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Up");
            }
            else if (tempHero.GetCritChance(tempHero.finalCritRating, tempHero.finalDexterity) < heroToCheck.GetCritChance(heroToCheck.finalCritRating, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Down");
            }
            else if (tempHero.GetCritChance(tempHero.finalCritRating, tempHero.finalDexterity) == heroToCheck.GetCritChance(heroToCheck.finalCritRating, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = tempHero.GetMoveRating(tempHero.finalMoveRating, tempHero.finalDexterity).ToString();
            if (tempHero.GetMoveRating(tempHero.finalMoveRating, tempHero.finalDexterity) > heroToCheck.GetMoveRating(heroToCheck.finalMoveRating, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Up");
            }
            else if (tempHero.GetMoveRating(tempHero.finalMoveRating, tempHero.finalDexterity) < heroToCheck.GetMoveRating(heroToCheck.finalMoveRating, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Down");
            }
            else if (tempHero.GetMoveRating(tempHero.finalMoveRating, tempHero.finalDexterity) == heroToCheck.GetMoveRating(heroToCheck.finalMoveRating, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = tempHero.GetRegen(tempHero.finalRegenRating, tempHero.finalSpirit).ToString();
            if (tempHero.GetRegen(tempHero.finalRegenRating, tempHero.finalSpirit) > heroToCheck.GetRegen(heroToCheck.finalRegenRating, heroToCheck.finalSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Up");
            }
            else if (tempHero.GetRegen(tempHero.finalRegenRating, tempHero.finalSpirit) < heroToCheck.GetRegen(heroToCheck.finalRegenRating, heroToCheck.finalSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Down");
            }
            else if (tempHero.GetRegen(tempHero.finalRegenRating, tempHero.finalSpirit) == heroToCheck.GetRegen(heroToCheck.finalRegenRating, heroToCheck.finalSpirit))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = tempHero.GetDodgeChance(tempHero.finalDodgeRating, tempHero.finalAgility).ToString();
            if (tempHero.GetDodgeChance(tempHero.finalDodgeRating, tempHero.finalAgility) > heroToCheck.GetDodgeChance(heroToCheck.finalDodgeRating, heroToCheck.finalAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Up");
            }
            else if (tempHero.GetDodgeChance(tempHero.finalDodgeRating, tempHero.finalAgility) < heroToCheck.GetDodgeChance(heroToCheck.finalDodgeRating, heroToCheck.finalAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Down");
            }
            else if (tempHero.GetDodgeChance(tempHero.finalDodgeRating, tempHero.finalAgility) == heroToCheck.GetDodgeChance(heroToCheck.finalDodgeRating, heroToCheck.finalAgility))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = tempHero.GetBlockChance(tempHero.finalBlockRating).ToString();
            if (tempHero.GetBlockChance(tempHero.finalBlockRating) > heroToCheck.GetBlockChance(heroToCheck.finalBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Up");
            }
            else if (tempHero.GetBlockChance(tempHero.finalBlockRating) < heroToCheck.GetBlockChance(heroToCheck.finalBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Down");
            }
            else if (tempHero.GetBlockChance(tempHero.finalBlockRating) == heroToCheck.GetBlockChance(heroToCheck.finalBlockRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = tempHero.GetParryChance(tempHero.finalParryRating, tempHero.finalStrength, tempHero.finalDexterity).ToString();
            if (tempHero.GetParryChance(tempHero.finalParryRating, tempHero.finalStrength, tempHero.finalDexterity) > heroToCheck.GetParryChance(heroToCheck.finalParryRating, heroToCheck.finalStrength, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Up");
            }
            else if (tempHero.GetParryChance(tempHero.finalParryRating, tempHero.finalStrength, tempHero.finalDexterity) < heroToCheck.GetParryChance(heroToCheck.finalParryRating, heroToCheck.finalStrength, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Down");
            }
            else if (tempHero.GetParryChance(tempHero.finalParryRating, tempHero.finalStrength, tempHero.finalDexterity) == heroToCheck.GetParryChance(heroToCheck.finalParryRating, heroToCheck.finalStrength, heroToCheck.finalDexterity))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
            }

            GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = tempHero.GetThreatRating(tempHero.finalThreatRating).ToString();
            if (tempHero.GetThreatRating(tempHero.finalThreatRating) > heroToCheck.GetThreatRating(heroToCheck.finalThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Up");
            }
            else if (tempHero.GetThreatRating(tempHero.finalThreatRating) < heroToCheck.GetThreatRating(heroToCheck.finalThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Down");
            }
            else if (tempHero.GetThreatRating(tempHero.finalThreatRating) == heroToCheck.GetThreatRating(heroToCheck.finalThreatRating))
            {
                ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
            }
    }

    public void ResetEquipmentStatUpdates()
    {
        //heroToCheck.GetCurrentStatsFromEquipment();
        //heroToCheck.UpdateStatsFromTalents();
        
        if (heroToCheck != null)
        {
            GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = heroToCheck.finalStrength.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/StrengthArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = heroToCheck.finalStamina.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/StaminaArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = heroToCheck.finalAgility.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/AgilityArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = heroToCheck.finalDexterity.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/DexterityArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = heroToCheck.finalIntelligence.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/IntelligenceArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = heroToCheck.finalSpirit.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/SpiritArrow"), "Neutral");

            GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = heroToCheck.finalMaxHP.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/HPArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = heroToCheck.finalMaxMP.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPArrow"), "Neutral");

            GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = heroToCheck.finalATK.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/AttackArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = heroToCheck.finalMATK.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicAttackArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = heroToCheck.finalDEF.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/DefenseArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = heroToCheck.finalMDEF.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/MagicDefenseArrow"), "Neutral");

            GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = heroToCheck.GetHitChance(heroToCheck.finalHitRating, heroToCheck.finalAgility).ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = heroToCheck.GetCritChance(heroToCheck.finalCritRating, heroToCheck.finalDexterity).ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = heroToCheck.GetMoveRating(heroToCheck.finalMoveRating, heroToCheck.finalDexterity).ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = heroToCheck.GetRegen(heroToCheck.finalRegenRating, heroToCheck.finalSpirit).ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");

            GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = heroToCheck.GetDodgeChance(heroToCheck.finalDodgeRating, heroToCheck.finalAgility).ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = heroToCheck.GetBlockChance(heroToCheck.finalBlockRating).ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = heroToCheck.GetParryChance(heroToCheck.finalParryRating, heroToCheck.finalStrength, heroToCheck.finalDexterity).ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = heroToCheck.GetThreatRating(heroToCheck.finalThreatRating).ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
        }
        
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
        choosingHero = true;
        BoldButton(StatusButton);
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }
        UnboldButton(StatusButton);
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
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.finalMaxHP.ToString()); //HP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.finalMaxMP.ToString()); //MP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/EXPText").GetComponent<Text>().text = (hero.currentExp + " / " + HeroDB.instance.levelEXPThresholds[hero.currentLevel - 1]); //EXP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/ToNextLevelText").GetComponent<Text>().text = (HeroDB.instance.levelEXPThresholds[hero.currentLevel - 1] - hero.currentExp).ToString(); //To next level text

        StatusPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hero), 0, 1), StatusPanelHPProgressBar.transform.localScale.y);
        StatusPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hero), 0, 1), StatusPanelMPProgressBar.transform.localScale.y);
        StatusPanelEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(hero), 0, 1), StatusPanelEXPProgressBar.transform.localScale.y);
    }

    void DrawStatusMenuStats(BaseHero hero)
    {
        GameObject.Find("StatusMenuPanel/StatsPanel/StrengthText").GetComponent<Text>().text = hero.finalStrength.ToString(); //Strength text
        GameObject.Find("StatusMenuPanel/StatsPanel/StaminaText").GetComponent<Text>().text = hero.finalStamina.ToString(); //Stamina text
        GameObject.Find("StatusMenuPanel/StatsPanel/AgilityText").GetComponent<Text>().text = hero.finalAgility.ToString(); //Agility text
        GameObject.Find("StatusMenuPanel/StatsPanel/DexterityText").GetComponent<Text>().text = hero.finalDexterity.ToString(); //Dexterity text
        GameObject.Find("StatusMenuPanel/StatsPanel/IntelligenceText").GetComponent<Text>().text = hero.finalIntelligence.ToString(); //Intelligence text
        GameObject.Find("StatusMenuPanel/StatsPanel/SpiritText").GetComponent<Text>().text = hero.finalSpirit.ToString(); //Spirit text

        GameObject.Find("StatusMenuPanel/StatsPanel/AttackText").GetComponent<Text>().text = hero.finalATK.ToString(); //Attack text
        GameObject.Find("StatusMenuPanel/StatsPanel/MagicAttackText").GetComponent<Text>().text = hero.finalMATK.ToString(); //Magic Attack text
        GameObject.Find("StatusMenuPanel/StatsPanel/DefenseText").GetComponent<Text>().text = hero.finalDEF.ToString(); //Defense text
        GameObject.Find("StatusMenuPanel/StatsPanel/MagicDefenseText").GetComponent<Text>().text = hero.finalMDEF.ToString(); //Magic Defense text

        GameObject.Find("StatusMenuPanel/StatsPanel/HitChanceText").GetComponent<Text>().text = hero.GetHitChance(hero.finalHitRating, hero.finalAgility).ToString(); //Hit Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/CritChanceText").GetComponent<Text>().text = hero.GetCritChance(hero.finalCritRating, hero.finalDexterity).ToString(); //Crit Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/MoveRatingText").GetComponent<Text>().text = hero.GetMoveRating(hero.finalMoveRating, hero.finalDexterity).ToString(); //Move Rating text
        GameObject.Find("StatusMenuPanel/StatsPanel/MPPerTurnText").GetComponent<Text>().text = hero.GetRegen(hero.finalRegenRating, hero.finalSpirit).ToString(); //MP Regen text
        GameObject.Find("StatusMenuPanel/StatsPanel/DodgeChanceText").GetComponent<Text>().text = hero.GetDodgeChance(hero.finalDodgeRating, hero.finalAgility).ToString(); //Dodge Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/BlockChanceText").GetComponent<Text>().text = hero.GetBlockChance(hero.finalBlockRating).ToString(); //Block Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/ParryChanceText").GetComponent<Text>().text = hero.GetParryChance(hero.finalParryRating, hero.finalStrength, hero.finalDexterity).ToString(); //Parry Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/ThreatText").GetComponent<Text>().text = hero.GetThreatRating(hero.finalThreatRating).ToString(); //Threat Rating text
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
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].finalMaxHP); //HP text
            GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].finalMaxMP); //MP text

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
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].finalMaxHP); //HP text
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].finalMaxMP); //MP text
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
        choosingHero = true;
        BoldButton(TalentsButton);
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }
        UnboldButton(TalentsButton);
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
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/HPText").GetComponent<Text>().text = (hero.curHP.ToString() + " / " + hero.finalMaxHP.ToString()); //HP text
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/MPText").GetComponent<Text>().text = (hero.curMP.ToString() + " / " + hero.finalMaxMP.ToString()); //MP text

        TalentsPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(hero), 0, 1), TalentsPanelHPProgressBar.transform.localScale.y);
        TalentsPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(hero), 0, 1), TalentsPanelMPProgressBar.transform.localScale.y);
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
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero1Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[0];
                }
                if (result.gameObject.name == "Hero2Panel")
                {
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero2Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[1];
                }
                if (result.gameObject.name == "Hero3Panel")
                {
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero3Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[2];
                }
                if (result.gameObject.name == "Hero4Panel")
                {
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero4Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[3];
                }
                if (result.gameObject.name == "Hero5Panel")
                {
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/HeroInfoPanel/Hero5Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[4];
                }
            }
        }
    }

    void BoldButton(Button button)
    {
        button.gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
    }

    void UnboldButton(Button button)
    {
        button.gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
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
        float heroBaseHP = hero.finalMaxHP;
        float calc_HP;
        
        calc_HP = heroHP / heroBaseHP;

        return calc_HP;
    }

    float GetProgressBarValuesMP(BaseHero hero)
    {
        float heroMP = hero.curMP;
        float heroBaseMP = hero.finalMaxMP;
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
            baseLineEXP = (HeroDB.instance.levelEXPThresholds[hero.currentLevel - 1]);
            heroEXP = hero.currentExp;
        } else
        {
            baseLineEXP = (HeroDB.instance.levelEXPThresholds[hero.currentLevel - 1] - HeroDB.instance.levelEXPThresholds[hero.currentLevel - 2]);
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
