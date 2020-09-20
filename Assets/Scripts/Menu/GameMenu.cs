using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using System.IO;
using UnityEditor;
using TMPro;

public class GameMenu : MonoBehaviour
{
    #region Objects

    CanvasGroup mainMenuCanvasGroup;

    GameObject player;
    public bool drawingGUI = false;
    public bool disableMenu = false;
    public bool disableMenuExit = false;
    [HideInInspector] public bool menuCalled = false;
    [ReadOnly] public bool inMenu = false;

    public bool choosingHero = false;

    //for menu positioning
    Vector3 currentCameraPosition;
    Vector3 menuCameraPosition;
    GameObject gameCameraObj;
    Camera gameCamera;
    float currentCameraSize;
    float menuCameraSize;

    //menu audio sources
    AudioSource menuAudioSource;
    AudioSource animAudioSource;

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
    bool inArrangeMenu;
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
    TextMeshProUGUI StatusPanelDetailsText;

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
    Transform PartyInactiveRowSpacer;
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

    //for animators
    Animator MM_infoPanelAnimator;
    Animator MM_menuButtonsPanelAnimator;
    Animator MM_timeGoldPanelAnimator;
    Animator MM_locationPanelAnimator;
    Animator Items_itemOptionsPanel;
    Animator Items_itemDescriptionPanel;
    Animator Items_heroItemPanel;
    Animator Items_itemListPanel;
    Animator Items_keyItemListPanel;
    Animator Items_arrangeOptionsPanel;
    Animator Magic_heroMagicPanel;
    Animator Magic_magicOptionsPanel;
    Animator Magic_magicDetailsPanel;
    Animator Magic_magicDescriptionPanel;
    Animator Magic_whiteMagicListPanel;
    Animator Magic_blackMagicListPanel;
    Animator Magic_sorceryMagicListPanel;
    Animator Magic_heroSelectMagicPanel;
    Animator Equip_equipDescriptionPanel;
    Animator Equip_heroEquipPanel;
    Animator Equip_equipOptionsPanel;
    Animator Equip_equipSlotsPanel;
    Animator Equip_equipListPanel;
    Animator Equip_equipStatsPanel;
    Animator Status_baseStatsPanel;
    Animator Status_passivePanel;
    Animator Status_detailsPanel;
    Animator Status_statsPanel;
    Animator Status_equipPanel;
    Animator Status_elePanel;
    Animator Status_statusPanel;
    Animator Status_activesPanel;
    Animator Status_magicPanel;
    Animator Talents_heroPanel;
    Animator Talents_talentsPanel;
    Animator Talents_talentDetailsPanel;
    Animator Party_activeHeroesPanel;
    Animator Party_inactiveHeroesPanel;
    Animator Party_selectedHeroPanel;
    Animator Grid_heroGridPanel;
    Animator Grid_gridPanel;
    Animator Quest_questListOptions;
    Animator Quest_questListPanel;
    Animator Quest_questNamePanel;
    Animator Quest_questLevelRequirementsPanel;
    Animator Quest_questDetailsPanel;
    Animator Quest_questRewardsPanel;
    Animator Bestiary_enemyListPanel;
    Animator Bestiary_enemyNamePanel;
    Animator Bestiary_enemyLevelPanel;
    Animator Bestiary_enemyGraphicPanel;
    Animator Bestiary_enemyDescriptionPanel;
    Animator Bestiary_enemyDetailsPanel;
    bool inAnimation;

    Transform cursor;
    bool dpadPressed;
    bool cancelPressed;
    readonly float cursorMenuX = 605.5f;
    readonly float cursorHeroSelectX = 135f;

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
    
    public enum MainCursorStates
    {
        ITEM,
        MAGIC,
        EQUIP,
        STATUS,
        TALENTS,
        PARTY,
        GRID,
        QUESTS,
        BESTIARY,
        HEROSELECT1,
        HEROSELECT2,
        HEROSELECT3,
        HEROSELECT4,
        HEROSELECT5,
        IDLE
    }
    [ReadOnly] public MainCursorStates mainCursorState; //for where the cursor is at a given moment on the main menu

    int tempScrollDiff = 0;

    #region item menu cursor objects

    public enum ItemMenuCursorStates
    {
        OPTIONUSE,
        OPTIONKEYITEMS,
        OPTIONARRANGE,
        USEITEMS,
        CHECKKEYITEMS,
        ARRANGEITEMS,
        IDLE
    }
    [ReadOnly] public ItemMenuCursorStates itemMenuCursorState;

    public enum ItemHeroSelect
    {
        HERO1,
        HERO2,
        HERO3,
        HERO4,
        HERO5
    }

    int cursorOnItem = 0;
    float itemListPosX = 520;
    float itemListSpacerY = 25f;
    float itemHeroChoosingPosX = 200f;
    float itemHeroChoosingSpacerY = 50f;
    float itemListScrollBy = 25f;
    int cursorOnItemHero = 0;
    GameObject itemSelected;
    string itemMode;
    int arrangeOption = 0;
    float arrangeOptionPosX = 551f;
    float arrangeOptionSpacerY = 21f;
    int arrangeCustomizeItem = 0;

    #endregion

    #region magic menu cursor objects

    public enum MagicMenuCursorStates
    {
        OPTIONWHITE,
        OPTIONBLACK,
        OPTIONSORC,
        USEMAGIC,
        IDLE
    }
    [ReadOnly] public MagicMenuCursorStates magicMenuCursorState;

    public enum MagicHeroSelect
    {
        HERO1,
        HERO2,
        HERO3,
        HERO4,
        HERO5
    }

    int cursorOnMagic = 0;
    float magicPosX = 164f;
    float magicPosY = 574f;
    float magicSpacerX = 177f;
    float magicSpacerY = 42f;
    float magicHeroX = 275f;
    float magicHeroSpacerY = 56f;
    int cursorOnMagicHero = 0;
    bool magicChoosingHero;
    MagicMouseEvents selectedMagic;
    string chosenMagic = "White";

    #endregion

    #region equip menu cursor objects

    public enum EquipMenuCursorStates
    {
        EQUIPSLOTS,
        EQUIPOPTION,
        REMOVEOPTION,
        REMOVEALLOPTION,
        EQUIPLIST,
        IDLE
    }
    [ReadOnly] public EquipMenuCursorStates equipMenuCursorState;

    int cursorOnEquipSlot = 0;
    float equipRow1X = 148f;
    float equipRow2X = 431f;
    float equipSlotY = 623f;
    float equipSlotSpacerY = 19f;

    float equipListX = 523f;
    float equipListSpacerY = 18.03f;
    int cursorOnEquipList = 0;
    float equipListScrollBy = 18f;

    EquipMouseEvents equipListItem;

    #endregion

    #region status menu cursor objects

    public enum StatusMenuCursorStates
    {
        STATS,
        EQUIPMENT,
        ELEMENTS,
        RESISTANCES,
        SKILLS,
        IDLE
    }
    [ReadOnly] public StatusMenuCursorStates statusMenuCursorState;

    int cursorOnStat = 0;
    int cursorOnEle = 0;
    int cursorOnRes = 0;
    int cursorOnEquip = 0;
    bool inStatusSkills;

    readonly string statColor = "<color=#ffa500ff>";

    readonly string fireColor =      "<color=#ff0000ff>";
    readonly string frostColor =     "<color=#00bfffff>";
    readonly string lightningColor = "<color=#ffff00ff>";
    readonly string waterColor =     "<color=#0000ffff>";
    readonly string natureColor =    "<color=#6b8e23ff>";
    readonly string earthColor =     "<color=#8b4513ff>";
    readonly string holyColor =      "<color=#c0c0c0ff>";
    readonly string darknessColor =  "<color=#800000ff>";

    readonly string blindColor =     "<color=#2f4f4fff>";
    readonly string silenceColor =   "<color=#f5f5dcff>";
    readonly string sleepColor =     "<color=#4682b4ff>";
    readonly string confuseColor =   "<color=#ffc0cbff>";
    readonly string poisonColor =    "<color=#32cd32ff>";
    readonly string petrifyColor =   "<color=#808080ff>";
    readonly string slowColor =      "<color=#ffa500ff>";
    readonly string zombieColor =    "<color=#98fb98ff>";

    #endregion

    #region talents menu cursor objects

    public enum TalentsMenuCursorStates
    {
        TALENTS,
        IDLE
    }
    [ReadOnly] public TalentsMenuCursorStates talentMenuCursorState;

    int cursorOnTalent = 0;
    int talentRow = 0;
    int talentColumn = 0;

    float talentColumnSpacer = 119f;
    float talentRowSpacer = 53f;

    #endregion

    #region party menu cursor objects

    public enum PartyMenuCursorStates
    {
        ACTIVE,
        INACTIVE,
        IDLE
    }
    [ReadOnly] public PartyMenuCursorStates partyMenuCursorState;

    int cursorOnPartyActive = 0;
    int cursorOnPartyInactive = 0;

    float partyActiveSpacer = 62f;

    float inactivePartySpacerX = 115f;
    float inactivePartySpacerY = 28.5f;

    PartySelectEvents pse1;
    PartySelectEvents pse2;

    bool partyChoosing;
    string partyChoosingMode;

    #endregion

    #region grid menu cursor objects

    public enum GridMenuCursorStates
    {
        HEROES,
        GRID,
        IDLE
    }
    [ReadOnly] public GridMenuCursorStates gridMenuCursorState;

    int cursorOnGridHero = 0;

    float gridHeroSpacer = 62f;

    float gridSpacer = 63f;

    int gridColumn;
    int gridRow;

    #endregion

    #region quests menu cursor objects

    public enum QuestMenuCursorStates
    {
        QUESTLIST,
        OPTIONS,
        IDLE
    }
    [ReadOnly] public QuestMenuCursorStates questMenuCursorState;

    int cursorOnQuest = 0;
    float questListSpacer = 37f;
    float questListScrollBy = 36.45f;
    QuestListButtonMouseEvents questSelected;

    int cursorOnQuestOption;

    #endregion

    #region bestiary menu cursor objects

    public enum BestiaryMenuCursorStates
    {
        BESTIARYLIST,
        IDLE
    }
    [ReadOnly] public BestiaryMenuCursorStates bestiaryMenuCursorState;

    int cursorOnBest = 0;
    float bestListSpacer = 31f;
    float bestListScrollBy = 31f;
    BestiaryListButtonMouseEvents bestSelected;

    #endregion

    bool buttonPressed = false;
    bool confirmPressed = false;

    GraphicRaycaster raycaster;
    [ReadOnly] public BaseHero heroToCheck;

    Coroutine menuToDraw = null;

    #endregion

    void Start() //Sets objects
    {
        //set BG
        gameCameraObj = GameObject.Find("Player/FollowCamera");
        gameCamera = gameCameraObj.GetComponent<Camera>();
        menuCameraPosition = new Vector3(419.3f, 568.9f, -1);
        menuCameraSize = 168.4568f;
        currentCameraSize = gameCamera.orthographicSize;

        //-----The below until end of Start are all static and should not be changed-----

        player = GameObject.Find("Player");

        heroToCheck = null;

        mainCursorState = MainCursorStates.IDLE;
        cursor = GameObject.Find("GameManager/Cursor").transform;

        menuAudioSource = GameObject.Find("GameManager/Menus/MenuSounds").GetComponent<AudioSource>();
        animAudioSource = menuAudioSource = GameObject.Find("GameManager/Menus/AnimationSounds").GetComponent<AudioSource>();

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
        ItemButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/ItemButton").GetComponent<Button>();
        MagicButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/MagicButton").GetComponent<Button>();
        EquipButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/EquipButton").GetComponent<Button>();
        StatusButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/StatusButton").GetComponent<Button>();
        TalentsButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/TalentsButton").GetComponent<Button>();
        PartyButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/PartyButton").GetComponent<Button>();
        GridButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/GridButton").GetComponent<Button>();
        QuestsButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/QuestsButton").GetComponent<Button>();
        BestiaryButton = MainMenuCanvas.transform.Find("MainMenuPanel/MenuButtonsPanel/BestiaryButton").GetComponent<Button>();

        //sets spacers
        ItemListSpacer = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform;
        KeyItemListSpacer = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform;
        WhiteMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/WhiteMagicListPanel/WhiteMagicScroller/WhiteMagicListSpacer").transform;
        BlackMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/BlackMagicListPanel/BlackMagicScroller/BlackMagicListSpacer").transform;
        SorceryMagicListSpacer = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/SorceryMagicListPanel/SorceryMagicScroller/SorceryMagicListSpacer").transform;
        EquipListSpacer = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform;
        PartyInactiveRowSpacer = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/InactiveHeroesSpacer").transform;
        ActiveQuestListSpacer = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform;
        CompletedQuestListSpacer = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform;
        BestiaryEnemyListSpacer = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform;

        //Set Hero Panels
        Hero1MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero1Panel");
        Hero2MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero2Panel");
        Hero3MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero3Panel");
        Hero4MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero4Panel");
        Hero5MainMenuPanel = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero5Panel");

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
        StatusPanelDetailsText = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/DetailsPanel/DetailsText").GetComponent<TextMeshProUGUI>();

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

        //Set animators
        MM_infoPanelAnimator = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel").GetComponent<Animator>();
        MM_menuButtonsPanelAnimator = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/MenuButtonsPanel").GetComponent<Animator>();
        MM_timeGoldPanelAnimator = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/TimeGoldPanel").GetComponent<Animator>();
        MM_locationPanelAnimator = GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/LocationPanel").GetComponent<Animator>();
        Items_itemOptionsPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel").GetComponent<Animator>();
        Items_itemDescriptionPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemDescriptionPanel").GetComponent<Animator>();
        Items_heroItemPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").GetComponent<Animator>();
        Items_itemListPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<Animator>();
        Items_keyItemListPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<Animator>();
        Items_arrangeOptionsPanel = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<Animator>();
        Magic_heroMagicPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroMagicPanel").GetComponent<Animator>();
        Magic_magicOptionsPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel").GetComponent<Animator>();
        Magic_magicDetailsPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel").GetComponent<Animator>();
        Magic_magicDescriptionPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/MagicDescriptionPanel").GetComponent<Animator>();
        Magic_whiteMagicListPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/WhiteMagicListPanel").GetComponent<Animator>();
        Magic_blackMagicListPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/BlackMagicListPanel").GetComponent<Animator>();
        Magic_sorceryMagicListPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/SorceryMagicListPanel").GetComponent<Animator>();
        Magic_heroSelectMagicPanel = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel").GetComponent<Animator>();
        Equip_equipDescriptionPanel = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipDescriptionPanel").GetComponent<Animator>();
        Equip_heroEquipPanel = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/HeroEquipPanel").GetComponent<Animator>();
        Equip_equipOptionsPanel = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipOptionsPanel").GetComponent<Animator>();
        Equip_equipSlotsPanel = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipSlotsPanel").GetComponent<Animator>();
        Equip_equipListPanel = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel").GetComponent<Animator>();
        Equip_equipStatsPanel = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipStatsPanel").GetComponent<Animator>();
        Status_baseStatsPanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/BaseStatsPanel").GetComponent<Animator>();
        Status_passivePanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/PassivePanel").GetComponent<Animator>();
        Status_detailsPanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/DetailsPanel").GetComponent<Animator>();
        Status_statsPanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatsPanel").GetComponent<Animator>();
        Status_equipPanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel").GetComponent<Animator>();
        Status_elePanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel").GetComponent<Animator>();
        Status_statusPanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel").GetComponent<Animator>();
        Status_activesPanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ActiveEffectsPanel").GetComponent<Animator>();
        Status_magicPanel = GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/MagicPanel").GetComponent<Animator>();
        Talents_heroPanel = GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/HeroPanel").GetComponent<Animator>();
        Talents_talentsPanel = GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel").GetComponent<Animator>();
        Talents_talentDetailsPanel = GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel").GetComponent<Animator>();
        Party_activeHeroesPanel = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").GetComponent<Animator>();
        Party_inactiveHeroesPanel = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel").GetComponent<Animator>();
        Party_selectedHeroPanel = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/SelectedHeroPanel").GetComponent<Animator>();
        Grid_heroGridPanel = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel").GetComponent<Animator>();
        Grid_gridPanel = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel").GetComponent<Animator>();
        Quest_questListOptions = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestListOptions").GetComponent<Animator>();
        Quest_questListPanel = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestListPanel").GetComponent<Animator>();
        Quest_questNamePanel = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestNamePanel").GetComponent<Animator>();
        Quest_questLevelRequirementsPanel = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestLevelRequirementsPanel").GetComponent<Animator>();
        Quest_questDetailsPanel = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestDetailsPanel").GetComponent<Animator>();
        Quest_questRewardsPanel = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel/QuestRewardsPanel").GetComponent<Animator>();
        Bestiary_enemyListPanel = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel").GetComponent<Animator>();
        Bestiary_enemyNamePanel = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyNamePanel").GetComponent<Animator>();
        Bestiary_enemyLevelPanel = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyLevelPanel").GetComponent<Animator>();
        Bestiary_enemyGraphicPanel = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyGraphicPanel").GetComponent<Animator>();
        Bestiary_enemyDescriptionPanel = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyDescriptionPanel").GetComponent<Animator>();
        Bestiary_enemyDetailsPanel = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyDetailsPanel").GetComponent<Animator>();
        
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

    void Update() //Allows for menu to be called when 'Menu' button is pressed, and also keeps time counter updated
    {
        if ((Input.GetButtonDown("Menu") && !disableMenu) || menuCalled) //if menu is called (and not disabled)
        {
            drawingGUI = true;
        }

        DisplayTime();

        if (drawingGUI)
        {
            UpdateCursor();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            cancelPressed = true;
            //buttonPressed = true;
        }
    }

    private void OnGUI() //Actually draws the menu, and facilitates cancelling from various menus
    {
        if (drawingGUI && (menuState == MenuStates.IDLE))
        {
            gameCameraObj = GameObject.Find("Player/FollowCamera");
            gameCamera = gameCameraObj.GetComponent<Camera>();
            currentCameraPosition = gameCameraObj.transform.position;

            //ScreenshotBG();
            //StartCoroutine(ScreenShotCoroutine());

            gameCamera.transform.parent = null;
            gameCamera.transform.position = menuCameraPosition;
            gameCamera.orthographicSize = menuCameraSize;

            PauseBackground(true); //keeps background objects from processing

            inMenu = true;

            ShowMainMenu();

            cursor.gameObject.GetComponent<CanvasGroup>().alpha = 1;

            menuState = MenuStates.MAIN;
        }

        /*if (cancelPressed)
        {
            Debug.Log("buttonPressed: " + buttonPressed + ", itemCustomizeModeOn: " + itemCustomizeModeOn + ", itemChoosingHero: " + itemChoosingHero + ", inArrangeMenu: " + inArrangeMenu + ", cancelPressed: " + cancelPressed);
        }*/

        if (!disableMenuExit && menuState == MenuStates.MAIN && !buttonPressed && (Input.GetButtonDown("Cancel")  || cancelPressed)) //if cancel is pressed while on main menu
        {
            StartCoroutine(ExitMainMenu());
            cursor.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            inMenu = false;
        }

        if (menuState == MenuStates.ITEM && !buttonPressed && !itemCustomizeModeOn && !itemChoosingHero && !inArrangeMenu && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on item menu
        {
            StartCoroutine(HideItemMenu());
        }

        if (menuState == MenuStates.MAGIC && !buttonPressed && !choosingHeroForMagicMenu && !magicChoosingHero && magicMenuCursorState != MagicMenuCursorStates.USEMAGIC && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on magic menu
        {
            StartCoroutine(HideMagicMenu());
        }

        if (menuState == MenuStates.EQUIP && !buttonPressed && !inEquipList && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on magic menu
        {
            StartCoroutine(HideEquipMenu());
        }

        if (menuState == MenuStates.STATUS && !buttonPressed && !inStatusSkills && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on status menu
        {
            StartCoroutine(HideStatusMenu());
        }

        if (menuState == MenuStates.TALENTS && !buttonPressed && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on talents menu
        {
            StartCoroutine(HideTalentsMenu());
        }

        if (menuState == MenuStates.PARTY && !buttonPressed && PartyHeroSelected == null && !partyChoosing && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on party menu
        {
            StartCoroutine(HidePartyMenu());
        }

        if (menuState == MenuStates.GRID && !buttonPressed && !gridChoosingTile && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on grid menu
        {
            StartCoroutine(HideGridMenu());
        }

        if (menuState == MenuStates.QUESTS && !buttonPressed && !QuestClicked && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on quests menu
        {
            StartCoroutine(HideQuestMenu());
        }

        if (menuState == MenuStates.BESTIARY && !buttonPressed && !BestiaryEntryClicked && (Input.GetButtonDown("Cancel") || cancelPressed)) //if cancel is pressed on bestiary menu
        {
            StartCoroutine(HideBestiaryMenu());
        }

        if (!buttonPressed && inEquipList && (Input.GetButtonDown("Cancel") || cancelPressed))
        {
            CancelFromEquipList();
            inEquipList = false;
        }

        if (!buttonPressed && QuestClicked && (Input.GetButtonDown("Cancel") || cancelPressed))
        {
            CancelQuestSelect();
        }

        if (!buttonPressed && BestiaryEntryClicked && (Input.GetButtonDown("Cancel") || cancelPressed))
        {
            CancelBestiarySelect();
        }

        if (!buttonPressed && itemCustomizeModeOn && (Input.GetButtonDown("Cancel") || cancelPressed))
        {
            PlaySE(AudioManager.instance.backSE);

            CancelCustomizeMode();
        }

        if (!buttonPressed && inArrangeMenu && (Input.GetButtonDown("Cancel") || cancelPressed))
        {
            PlaySE(AudioManager.instance.backSE);

            StartCoroutine(HideArrangeMenu());
        }

        if (!buttonPressed && itemChoosingHero && (Input.GetButtonDown("Cancel") || cancelPressed))
        {
            CancelItemChoosingHero();
        }

        if (!buttonPressed && choosingHeroForMagicMenu && (Input.GetButtonDown("Cancel") || cancelPressed))
        {
            StartCoroutine(CancelMagicChoosingHero());
        }

        //CheckCancelPressed(); //makes sure cancel is only pressed once

        if (cancelPressed)
        {
            cancelPressed = false;
            //buttonPressed = false;
        }
            
    }

    #region Cursor Functions

    void UpdateCursor()
    {
        if (mainCursorState == MainCursorStates.ITEM)
        {
            cursor.position = new Vector3(cursorMenuX, 725f, 0);
        }

        if (mainCursorState == MainCursorStates.MAGIC)
        {
            cursor.position = new Vector3(cursorMenuX, 700f, 0);
        }

        if (mainCursorState == MainCursorStates.EQUIP)
        {
            cursor.position = new Vector3(cursorMenuX, 675f, 0);
        }

        if (mainCursorState == MainCursorStates.STATUS)
        {
            cursor.position = new Vector3(cursorMenuX, 650f, 0);
        }

        if (mainCursorState == MainCursorStates.TALENTS)
        {
            cursor.position = new Vector3(cursorMenuX, 628f, 0);
        }

        if (mainCursorState == MainCursorStates.PARTY)
        {
            cursor.position = new Vector3(cursorMenuX, 603f, 0);
        }

        if (mainCursorState == MainCursorStates.GRID)
        {
            cursor.position = new Vector3(cursorMenuX, 579.6f, 0);
        }

        if (mainCursorState == MainCursorStates.QUESTS)
        {
            cursor.position = new Vector3(cursorMenuX, 555f, 0);
        }

        if (mainCursorState == MainCursorStates.BESTIARY)
        {
            cursor.position = new Vector3(cursorMenuX, 533f, 0);
        }

        if (mainCursorState == MainCursorStates.HEROSELECT1)
        {
            cursor.position = new Vector3(cursorHeroSelectX, 697f, 0);
        }

        if (mainCursorState == MainCursorStates.HEROSELECT2)
        {
            cursor.position = new Vector3(cursorHeroSelectX, 633f, 0);
        }

        if (mainCursorState == MainCursorStates.HEROSELECT3)
        {
            cursor.position = new Vector3(cursorHeroSelectX, 569f, 0);
        }

        if (mainCursorState == MainCursorStates.HEROSELECT4)
        {
            cursor.position = new Vector3(cursorHeroSelectX, 505, 0);
        }

        if (mainCursorState == MainCursorStates.HEROSELECT5)
        {
            cursor.position = new Vector3(cursorHeroSelectX, 441f, 0);
        }

        UpdateCursorOnInput();

        if (menuState == MenuStates.ITEM)
            UpdateItemCursor();

        if (menuState == MenuStates.MAGIC)
            UpdateMagicCursor();

        if (menuState == MenuStates.EQUIP)
            UpdateEquipCursor();

        if (menuState == MenuStates.STATUS)
            UpdateStatusCursor();

        if (menuState == MenuStates.TALENTS)
            UpdateTalentsCursor();

        if (menuState == MenuStates.PARTY)
            UpdatePartyCursor();

        if (menuState == MenuStates.GRID)
            UpdateGridCursor();

        if (menuState == MenuStates.QUESTS)
            UpdateQuestCursor();

        if (menuState == MenuStates.BESTIARY)
            UpdateBestiaryCursor();

        //CheckStates(); //comment out if dont need to test - this keeps outputting where each state is at any given time

        //----------leave at bottom
        if (Input.GetButtonUp("Confirm"))
        {
            confirmPressed = false;
        }

        /*if (Input.GetButtonUp("Cancel"))
        {
            buttonPressed = false;
        }*/

        if (Input.GetAxisRaw("DpadVertical") == 0 && Input.GetAxisRaw("DpadHorizontal") == 0)
        {
            dpadPressed = false;
        }
        else
        {
            //Debug.Log("Dpad is pressed: " + Input.GetAxisRaw("DpadVertical") + "," + Input.GetAxisRaw("DpadHorizontal"));
        }
    }

    void UpdateCursorOnInput()
    {
        if (!dpadPressed && (int)mainCursorState == 0 && !inAnimation)
        {
            if (Input.GetAxisRaw("DpadVertical") == 1) //up
            {
                mainCursorState = MainCursorStates.BESTIARY;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }

            if (Input.GetAxisRaw("DpadVertical") == -1) //down
            {
                mainCursorState += 1;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }
        } else if (!dpadPressed && (int)mainCursorState == 8 && !inAnimation)
        {
            if (Input.GetAxisRaw("DpadVertical") == 1) //up
            {
                mainCursorState -= 1;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }

            if (Input.GetAxisRaw("DpadVertical") == -1) //down
            {
                mainCursorState = MainCursorStates.ITEM;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }
        } else if (!dpadPressed && (int)mainCursorState >= 0 && (int)mainCursorState <= 8 && !inAnimation)
        {
            if (Input.GetAxisRaw("DpadVertical") == 1) //up
            {
                mainCursorState -= 1;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }

            if (Input.GetAxisRaw("DpadVertical") == -1) //down
            {
                mainCursorState += 1;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }
        } else if (!dpadPressed && (int)mainCursorState == 9 && GameManager.instance.activeHeroes.Count() > 1 && !inAnimation)
        {
            if (Input.GetAxisRaw("DpadVertical") == 1) //up
            {
                mainCursorState = mainCursorState + (GameManager.instance.activeHeroes.Count() - 1);
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }

            if (Input.GetAxisRaw("DpadVertical") == -1) //down
            {
                mainCursorState += 1;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }
        } else if (!dpadPressed && (int)mainCursorState > 9 && (int)mainCursorState < 13 && GameManager.instance.activeHeroes.Count() > 1 && !inAnimation)
        {
            if (Input.GetAxisRaw("DpadVertical") == 1) //up
            {
                mainCursorState -= 1;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }

            if (Input.GetAxisRaw("DpadVertical") == -1) //down
            {
                if (((int)mainCursorState - 8) != GameManager.instance.activeHeroes.Count())
                {
                    mainCursorState += 1;
                } else
                {
                    mainCursorState = MainCursorStates.HEROSELECT1;
                }
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }
            
        } else if (!dpadPressed && (int)mainCursorState == 13 && !inAnimation)
        {
            if (Input.GetAxisRaw("DpadVertical") == 1) //up
            {
                mainCursorState -= 1;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }

            if (Input.GetAxisRaw("DpadVertical") == -1) //down
            {
                mainCursorState = MainCursorStates.HEROSELECT1;
                dpadPressed = true;
                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
            }
        }

        if (Input.GetButtonDown("Confirm") && (int)mainCursorState >= 0 && (int)mainCursorState <= 8 && !confirmPressed && !inAnimation)
        {
            confirmPressed = true;
            ProcessMenuNavigation((int)mainCursorState);
            tempScrollDiff = 0;
        }

        if (Input.GetButtonDown("Confirm") && (int)mainCursorState >= 9 && (int)mainCursorState <= 13 && !confirmPressed && !inAnimation)
        {
            confirmPressed = true;
            heroToCheck = GameManager.instance.activeHeroes[(int)mainCursorState - 9];
            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
        }
        
    }

    void HideCursor()
    {
        cursor.GetComponent<CanvasGroup>().alpha = 0;
    }

    void ShowCursor()
    {
        cursor.GetComponent<CanvasGroup>().alpha = 1;
    }

    void ProcessMenuNavigation(int option)
    {
        //Debug.Log("option pressed: " + option);

        //0 - Item
        if (option == 0)
        {
            ShowItemMenu();
            itemCustomizeModeOn = false;
        }

        //1 - Magic - Hero select
        if (option == 1)
        {
            ShowMagicMenu();
        }

        //2 - Equip - Hero select
        if (option == 2)
        {
            ShowEquipMenu();
        }

        //3 - Status - Hero select
        if (option == 3)
        {
            ShowStatusMenu();
        }

        //4 - Talents - Hero select
        if (option == 4)
        {
            ShowTalentsMenu();
        }

        //5 - Party
        if (option == 5)
        {
            ShowPartyMenu();
        }

        //6 - Grid
        if (option == 6)
        {
            ShowGridMenu();
        }

        //7 - Quests
        if (option == 7)
        {
            ShowQuestMenu();
        }

        //8 - Bestiary
        if (option == 8)
        {
            ShowBestiaryMenu();
        }
    }

    void CheckStates()
    {
        Debug.Log("itemMenuCursorState: " + (int)itemMenuCursorState + " - " + itemMenuCursorState);
        Debug.Log("mainCursorState: " + (int)mainCursorState + " - " + mainCursorState);
        Debug.Log("menuState: " + (int)menuState + " - " + menuState);
        Debug.Log("magicMenuCursorState: " + (int)magicMenuCursorState + " - " + magicMenuCursorState);
    }

    void UpdateItemCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && itemMenuCursorState != ItemMenuCursorStates.IDLE && !inAnimation)
        {
            if (itemChoosingHero) //when choosing hero
            {
                if (!dpadPressed && cursorOnItemHero == 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnItemHero = cursorOnItemHero + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItemHero = GameManager.instance.activeHeroes.Count - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                } else if (!dpadPressed && cursorOnItemHero > 0 && cursorOnItemHero < (GameManager.instance.activeHeroes.Count - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItemHero = cursorOnItemHero - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnItemHero = cursorOnItemHero + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                } else if (!dpadPressed && cursorOnItemHero == (GameManager.instance.activeHeroes.Count - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItemHero = cursorOnItemHero - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnItemHero = 0;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    itemSelected.GetComponent<ItemMouseEvents>().hero = GameManager.instance.activeHeroes[cursorOnItemHero];
                    Debug.Log("using on " + GameManager.instance.activeHeroes[cursorOnItemHero].name);
                }

                if (cursorOnItemHero == 0)
                {
                    cursor.position = new Vector3(itemHeroChoosingPosX, 635f, 0);
                }
                else
                {
                    cursor.position = new Vector3(itemHeroChoosingPosX, (635f - (itemHeroChoosingSpacerY * cursorOnItemHero)), 0);
                }

            }

            if (itemMenuCursorState == ItemMenuCursorStates.USEITEMS && !itemChoosingHero) //When choosing item 
            {
                int totalItemsInList = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.childCount;
                int scrollDiff = totalItemsInList - 10;

                if (totalItemsInList <= 10)
                {
                    tempScrollDiff = 0;
                }

                RectTransform spacerScroll = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").GetComponent<RectTransform>();

                //Debug.Log(cursorOnItem);

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    itemSelected.GetComponent<ItemMouseEvents>().hero = null;
                    UseItem();
                    if (itemChoosingHero)
                    {
                        cursor.position = new Vector3(135f, 637f, 0);
                    }
                }

                if (!dpadPressed && cursorOnItem == 0 && tempScrollDiff == 0)
                {
                    if (totalItemsInList > 1)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnItem = cursorOnItem + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        itemMenuCursorState = ItemMenuCursorStates.OPTIONUSE;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
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
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (itemListScrollBy * (tempScrollDiff - 1)), 0.0f);

                        tempScrollDiff = tempScrollDiff - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnItem > 0 && cursorOnItem < 9)
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
                else if (!dpadPressed && cursorOnItem == 9 && tempScrollDiff == 0 && scrollDiff > 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItem = cursorOnItem - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, itemListScrollBy, 0.0f);

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }else if (!dpadPressed && cursorOnItem == 9 && tempScrollDiff == 0 && scrollDiff == 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItem = cursorOnItem - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                } else if (!dpadPressed && cursorOnItem == 9 && tempScrollDiff > 0 && (cursorOnItem + tempScrollDiff) < (totalItemsInList - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItem = 8;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (itemListScrollBy * (tempScrollDiff + 1)), 0.0f); //and use tempScrollDiff - 1 to go up

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                } else if (!dpadPressed && cursorOnItem == 9 && tempScrollDiff > 0 && (cursorOnItem + tempScrollDiff) == (totalItemsInList - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItem = 8;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (totalItemsInList != 0)
                {
                    if (cursorOnItem == totalItemsInList)
                    {
                        cursorOnItem = totalItemsInList - 1;
                    }

                    if (cursorOnItem == 0 && tempScrollDiff == 0)
                    {
                        cursor.position = new Vector3(itemListPosX, 645f, 0);
                        itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(0).gameObject;
                    }
                    else if (cursorOnItem == 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(itemListPosX, 645f, 0);
                        itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(tempScrollDiff).gameObject;
                    }
                    else if (cursorOnItem > 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(itemListPosX, (645f - (itemListSpacerY * cursorOnItem)), 0);
                        itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(cursorOnItem + tempScrollDiff).gameObject;
                    }
                    else
                    {
                        cursor.position = new Vector3(itemListPosX, (645f - (itemListSpacerY * cursorOnItem)), 0);
                        itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(cursorOnItem).gameObject;
                    }
                    if (itemSelected.GetComponent<ItemMouseEvents>().GetItemType() == "equip")
                    {
                        itemSelected.GetComponent<ItemMouseEvents>().itemDesc.text = itemSelected.GetComponent<ItemMouseEvents>().GetEquipment(itemSelected.GetComponent<ItemMouseEvents>().GetEquipID()).description;
                    } else if (itemSelected.GetComponent<ItemMouseEvents>().GetItemType() == "item")
                    {
                        itemSelected.GetComponent<ItemMouseEvents>().itemDesc.text = itemSelected.GetComponent<ItemMouseEvents>().GetItem(itemSelected.GetComponent<ItemMouseEvents>().GetItemID()).description;
                    }
                }
            }

            if (itemMenuCursorState == ItemMenuCursorStates.CHECKKEYITEMS) //interact with key items
            {
                int totalItemsInList = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.childCount;
                int scrollDiff = totalItemsInList - 10;

                if (totalItemsInList <= 10)
                {
                    tempScrollDiff = 0;
                }

                RectTransform spacerScroll = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").GetComponent<RectTransform>();
                
                if (!dpadPressed && cursorOnItem == 0 && tempScrollDiff == 0)
                {
                    if (totalItemsInList > 1)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnItem = cursorOnItem + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        itemMenuCursorState = ItemMenuCursorStates.OPTIONKEYITEMS;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
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
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (itemListScrollBy * (tempScrollDiff - 1)), 0.0f);

                        tempScrollDiff = tempScrollDiff - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnItem > 0 && cursorOnItem < 9)
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
                else if (!dpadPressed && cursorOnItem == 9 && tempScrollDiff == 0 && scrollDiff > 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItem = cursorOnItem - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, itemListScrollBy, 0.0f);

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnItem == 9 && tempScrollDiff == 0 && scrollDiff == 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItem = cursorOnItem - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnItem == 9 && tempScrollDiff > 0 && (cursorOnItem + tempScrollDiff) < (totalItemsInList - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItem = 8;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (itemListScrollBy * (tempScrollDiff + 1)), 0.0f); //and use tempScrollDiff - 1 to go up

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnItem == 9 && tempScrollDiff > 0 && (cursorOnItem + tempScrollDiff) == (totalItemsInList - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnItem = 8;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (totalItemsInList != 0)
                {
                    if (cursorOnItem == totalItemsInList)
                    {
                        cursorOnItem = totalItemsInList - 1;
                    }

                    if (cursorOnItem == 0 && tempScrollDiff == 0)
                    {
                        cursor.position = new Vector3(itemListPosX, 645f, 0);
                        itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(0).gameObject;
                    }
                    else if (cursorOnItem == 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(itemListPosX, 645f, 0);
                        itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(tempScrollDiff).gameObject;
                    }
                    else if (cursorOnItem > 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(itemListPosX, (645f - (itemListSpacerY * cursorOnItem)), 0);
                        itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(cursorOnItem + tempScrollDiff).gameObject;
                    }
                    else
                    {
                        cursor.position = new Vector3(itemListPosX, (645f - (itemListSpacerY * cursorOnItem)), 0);
                        itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(cursorOnItem).gameObject;
                    }
                    if (itemSelected.GetComponent<ItemMouseEvents>().GetItemType() == "equip")
                    {
                        itemSelected.GetComponent<ItemMouseEvents>().itemDesc.text = itemSelected.GetComponent<ItemMouseEvents>().GetEquipment(itemSelected.GetComponent<ItemMouseEvents>().GetEquipID()).description;
                    }
                    else if (itemSelected.GetComponent<ItemMouseEvents>().GetItemType() == "item")
                    {
                        itemSelected.GetComponent<ItemMouseEvents>().itemDesc.text = itemSelected.GetComponent<ItemMouseEvents>().GetItem(itemSelected.GetComponent<ItemMouseEvents>().GetItemID()).description;
                    }
                }
            }

            if (itemMenuCursorState == ItemMenuCursorStates.ARRANGEITEMS) //interact with items to be arranged
            {
                if (!dpadPressed)
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed && arrangeOption == -1)
                    {
                        arrangeOption = 0;
                        ShowArrangeMenu();
                        confirmPressed = true;
                        itemMenuCursorState = ItemMenuCursorStates.ARRANGEITEMS;
                    }

                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        dpadPressed = true;
                        itemMenuCursorState = ItemMenuCursorStates.OPTIONKEYITEMS;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        if (GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().alpha == 1)
                        {
                            StartCoroutine(HideArrangeMenu());
                        }
                    }

                    if (arrangeOption == -1 && Input.GetAxisRaw("DpadVertical") == -1) //pressing down on the arrange button
                    {
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        if (itemMode == "Use")
                        {
                            itemMenuCursorState = ItemMenuCursorStates.USEITEMS;
                        } else if (itemMode == "KeyItems")
                        {
                            itemMenuCursorState = ItemMenuCursorStates.CHECKKEYITEMS;
                        }
                    }

                    if (arrangeOption == 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            dpadPressed = true;
                            arrangeOption++;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    } else if (arrangeOption > 0 && arrangeOption < 7)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            dpadPressed = true;
                            arrangeOption++;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            dpadPressed = true;
                            arrangeOption--;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    } else if (arrangeOption == 7)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            dpadPressed = true;
                            arrangeOption--;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                }

                if (arrangeOption == 0 || arrangeOption == -1)
                {
                    cursor.position = new Vector3(567f, 721f, 0); //arrange option button
                } else if (arrangeOption > 0)
                {
                    cursor.position = new Vector3(arrangeOptionPosX, 694f - (arrangeOptionSpacerY * (arrangeOption - 1)), 0);
                }

                if (arrangeOption == 0) //arrange option button
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        arrangeOption = -1;
                        StartCoroutine(HideArrangeMenu());
                        confirmPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                } else if (arrangeOption == 1) //customize
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        ItemArrange("Customize");
                        confirmPressed = true;
                        arrangeCustomizeItem = 0;
                        arrangeOption = -2;
                    }
                } else if (arrangeOption == 2) //field
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        ItemArrange("Field");
                        confirmPressed = true;
                        arrangeOption = -1;
                    }
                } else if (arrangeOption == 3) //battle
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        ItemArrange("Battle");
                        confirmPressed = true;
                        arrangeOption = -1;
                    }
                } else if (arrangeOption == 4) //type
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        ItemArrange("Type");
                        confirmPressed = true;
                        arrangeOption = -1;
                    }
                } else if (arrangeOption == 5) //name
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        ItemArrange("Name");
                        confirmPressed = true;
                        arrangeOption = -1;
                    }
                } else if (arrangeOption == 6) //most
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        ItemArrange("Most");
                        confirmPressed = true;
                        arrangeOption = -1;
                    }
                } else if (arrangeOption == 7) //least
                {
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        ItemArrange("Least");
                        confirmPressed = true;
                        arrangeOption = -1;
                    }
                }

                if (arrangeOption == -2) //customizing arrangement
                {
                    RectTransform spacerScroll = null;
                    int totalItemsInList = 0;

                    if (itemMode == "Use")
                    {
                        totalItemsInList = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.childCount;
                         spacerScroll = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").GetComponent<RectTransform>();
                    } else if (itemMode == "KeyItems")
                    {
                        totalItemsInList = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.childCount;
                        spacerScroll = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").GetComponent<RectTransform>();
                    }

                    int scrollDiff = totalItemsInList - 10;

                    if (totalItemsInList <= 10)
                    {
                        tempScrollDiff = 0;
                    }
                                       
                    //Debug.Log(cursorOnItem);
                    
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;
                        UseItem();
                    }

                    if (!dpadPressed && arrangeCustomizeItem == 0 && tempScrollDiff == 0)
                    {
                        if (totalItemsInList > 1)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                arrangeCustomizeItem = arrangeCustomizeItem + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            itemMenuCursorState = ItemMenuCursorStates.OPTIONARRANGE;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && arrangeCustomizeItem == 0 && tempScrollDiff > 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            arrangeCustomizeItem = arrangeCustomizeItem + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, (itemListScrollBy * (tempScrollDiff - 1)), 0.0f);

                            tempScrollDiff = tempScrollDiff - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && arrangeCustomizeItem > 0 && arrangeCustomizeItem < 9)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            arrangeCustomizeItem = arrangeCustomizeItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            arrangeCustomizeItem = arrangeCustomizeItem + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && arrangeCustomizeItem == 9 && tempScrollDiff == 0 && scrollDiff > 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            arrangeCustomizeItem = arrangeCustomizeItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, itemListScrollBy, 0.0f);

                            tempScrollDiff = tempScrollDiff + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && arrangeCustomizeItem == 9 && tempScrollDiff == 0 && scrollDiff == 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            arrangeCustomizeItem = arrangeCustomizeItem - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && arrangeCustomizeItem == 9 && tempScrollDiff > 0 && (arrangeCustomizeItem + tempScrollDiff) < (totalItemsInList - 1))
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            arrangeCustomizeItem = 8;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }

                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            spacerScroll.anchoredPosition = new Vector3(0.0f, (itemListScrollBy * (tempScrollDiff + 1)), 0.0f); //and use tempScrollDiff - 1 to go up

                            tempScrollDiff = tempScrollDiff + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (!dpadPressed && arrangeCustomizeItem == 9 && tempScrollDiff > 0 && (arrangeCustomizeItem + tempScrollDiff) == (totalItemsInList - 1))
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            arrangeCustomizeItem = 8;

                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }

                    if (itemMode == "Use")
                    {
                        if (totalItemsInList != 0)
                        {
                            if (arrangeCustomizeItem == totalItemsInList)
                            {
                                arrangeCustomizeItem = totalItemsInList - 1;
                            }

                            if (arrangeCustomizeItem == 0 && tempScrollDiff == 0)
                            {
                                cursor.position = new Vector3(itemListPosX, 645f, 0);
                                itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(0).gameObject;
                                Debug.Log("1) " + itemSelected.GetComponent<ItemMouseEvents>().GetItem(itemSelected.GetComponent<ItemMouseEvents>().GetItemID()).name);
                            }
                            else if (arrangeCustomizeItem == 0 && tempScrollDiff > 0)
                            {
                                cursor.position = new Vector3(itemListPosX, 645f, 0);
                                itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(tempScrollDiff).gameObject;
                                Debug.Log("2) " + itemSelected.GetComponent<ItemMouseEvents>().GetItem(itemSelected.GetComponent<ItemMouseEvents>().GetItemID()).name);
                            }
                            else if (arrangeCustomizeItem > 0 && tempScrollDiff > 0)
                            {
                                cursor.position = new Vector3(itemListPosX, (645f - (itemListSpacerY * arrangeCustomizeItem)), 0);
                                itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(arrangeCustomizeItem + tempScrollDiff).gameObject;
                                Debug.Log("3) " + itemSelected.GetComponent<ItemMouseEvents>().GetItem(itemSelected.GetComponent<ItemMouseEvents>().GetItemID()).name);
                            }
                            else
                            {
                                cursor.position = new Vector3(itemListPosX, (645f - (itemListSpacerY * arrangeCustomizeItem)), 0);
                                itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(arrangeCustomizeItem).gameObject;
                                Debug.Log("4) " + itemSelected.GetComponent<ItemMouseEvents>().GetItem(itemSelected.GetComponent<ItemMouseEvents>().GetItemID()).name); ;
                            }
                            if (itemSelected.GetComponent<ItemMouseEvents>().GetItemType() == "equip")
                            {
                                itemSelected.GetComponent<ItemMouseEvents>().itemDesc.text = itemSelected.GetComponent<ItemMouseEvents>().GetEquipment(itemSelected.GetComponent<ItemMouseEvents>().GetEquipID()).description;
                            }
                            else if (itemSelected.GetComponent<ItemMouseEvents>().GetItemType() == "item")
                            {
                                itemSelected.GetComponent<ItemMouseEvents>().itemDesc.text = itemSelected.GetComponent<ItemMouseEvents>().GetItem(itemSelected.GetComponent<ItemMouseEvents>().GetItemID()).description;
                            }
                        }
                    } else if (itemMode == "KeyItems")
                    {
                        if (totalItemsInList != 0)
                        {
                            if (arrangeCustomizeItem == totalItemsInList)
                            {
                                arrangeCustomizeItem = totalItemsInList - 1;
                            }

                            if (arrangeCustomizeItem == 0 && tempScrollDiff == 0)
                            {
                                cursor.position = new Vector3(itemListPosX, 645f, 0);
                                itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(0).gameObject;
                            }
                            else if (arrangeCustomizeItem == 0 && tempScrollDiff > 0)
                            {
                                cursor.position = new Vector3(itemListPosX, 645f, 0);
                                itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(tempScrollDiff).gameObject;
                            }
                            else if (arrangeCustomizeItem > 0 && tempScrollDiff > 0)
                            {
                                cursor.position = new Vector3(itemListPosX, (645f - (itemListSpacerY * arrangeCustomizeItem)), 0);
                                itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(arrangeCustomizeItem + tempScrollDiff).gameObject;
                            }
                            else
                            {
                                cursor.position = new Vector3(itemListPosX, (645f - (itemListSpacerY * arrangeCustomizeItem)), 0);
                                itemSelected = GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel/ItemScroller/ItemListSpacer").transform.GetChild(arrangeCustomizeItem).gameObject;
                            }
                            if (itemSelected.GetComponent<ItemMouseEvents>().GetItemType() == "equip")
                            {
                                itemSelected.GetComponent<ItemMouseEvents>().itemDesc.text = itemSelected.GetComponent<ItemMouseEvents>().GetEquipment(itemSelected.GetComponent<ItemMouseEvents>().GetEquipID()).description;
                            }
                            else if (itemSelected.GetComponent<ItemMouseEvents>().GetItemType() == "item")
                            {
                                itemSelected.GetComponent<ItemMouseEvents>().itemDesc.text = itemSelected.GetComponent<ItemMouseEvents>().GetItem(itemSelected.GetComponent<ItemMouseEvents>().GetItemID()).description;
                            }
                        }
                    }
                    

                }

                if (Input.GetButtonDown("Cancel") || cancelPressed)
                {
                    itemMenuCursorState = ItemMenuCursorStates.OPTIONARRANGE;
                }
            }

            if (itemMenuCursorState == ItemMenuCursorStates.OPTIONUSE)
            {
                cursor.position = new Vector3(197f, 721f, 0);

                GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "List available items";

                if (!dpadPressed)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        
                        if (itemMode == "Use")
                        {
                            itemMenuCursorState = ItemMenuCursorStates.USEITEMS;
                        } else if (itemMode == "KeyItems")
                        {
                            itemMenuCursorState = ItemMenuCursorStates.CHECKKEYITEMS;
                        }
                    }
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {

                        confirmPressed = true;
                        itemMenuCursorState = ItemMenuCursorStates.USEITEMS;
                        ShowUseItemMenu();
                        itemMode = "Use";
                        itemCustomizeModeOn = false;
                    }

                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        dpadPressed = true;
                        itemMenuCursorState = ItemMenuCursorStates.OPTIONKEYITEMS;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
            }

            if (itemMenuCursorState == ItemMenuCursorStates.OPTIONKEYITEMS)
            {
                cursor.position = new Vector3(360f, 721f, 0);

                GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "List key items";

                if (!dpadPressed)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        if (itemMode == "Use")
                        {
                            itemMenuCursorState = ItemMenuCursorStates.USEITEMS;
                        }
                        else if (itemMode == "KeyItems")
                        {
                            itemMenuCursorState = ItemMenuCursorStates.CHECKKEYITEMS;
                        }
                    }

                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;
                        itemMenuCursorState = ItemMenuCursorStates.CHECKKEYITEMS;
                        cursorOnItem = 0;
                        ShowKeyItemMenu();
                        itemMode = "KeyItems";
                        itemCustomizeModeOn = false;
                    }

                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        arrangeOption = -1;
                        itemMenuCursorState = ItemMenuCursorStates.OPTIONARRANGE;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        dpadPressed = true;
                        itemMenuCursorState = ItemMenuCursorStates.OPTIONUSE;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
            }

            if (itemMenuCursorState == ItemMenuCursorStates.OPTIONARRANGE)
            {
                cursor.position = new Vector3(567f, 721f, 0);

                GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "Organize items";

                if (!dpadPressed)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        if (itemMode == "Use")
                        {
                            itemMenuCursorState = ItemMenuCursorStates.USEITEMS;
                        }
                        else if (itemMode == "KeyItems")
                        {
                            itemMenuCursorState = ItemMenuCursorStates.CHECKKEYITEMS;
                        }
                    }
                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        arrangeOption = 0;
                        ShowArrangeMenu();
                        confirmPressed = true;
                        itemMenuCursorState = ItemMenuCursorStates.ARRANGEITEMS;
                        itemCustomizeModeOn = false;
                    }

                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        dpadPressed = true;
                        itemMenuCursorState = ItemMenuCursorStates.OPTIONKEYITEMS;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
            }
        }
    }

    void UseItem()
    {
        itemSelected.GetComponent<ItemMouseEvents>().UseItem();
    }

    void UpdateMagicCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && magicMenuCursorState != MagicMenuCursorStates.IDLE && !inAnimation)
        {
            if (magicChoosingHero)
            {
                if (!dpadPressed && cursorOnMagicHero == 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnMagicHero = cursorOnMagicHero + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnMagicHero = GameManager.instance.activeHeroes.Count - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnMagicHero > 0 && cursorOnMagicHero < (GameManager.instance.activeHeroes.Count - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnMagicHero = cursorOnMagicHero - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnMagicHero = cursorOnMagicHero + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnMagicHero == (GameManager.instance.activeHeroes.Count - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnMagicHero = cursorOnMagicHero - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnMagicHero = 0;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    selectedMagic.heroToCastOn = GameManager.instance.activeHeroes[cursorOnMagicHero];
                    Debug.Log("casting " + selectedMagic.GetMagic(selectedMagic.GetMagicName()).name + " on " + GameManager.instance.activeHeroes[cursorOnMagicHero].name);
                    magicChoosingHero = false;
                    magicMenuCursorState = MagicMenuCursorStates.USEMAGIC;
                }

                if (Input.GetButtonDown("Cancel") || cancelPressed)
                {
                    //buttonPressed = true;
                    Debug.Log("cancel pressed");
                    magicMenuCursorState = MagicMenuCursorStates.USEMAGIC;
                    magicChoosingHero = false;
                }

                if (cursorOnMagicHero == 0)
                {
                    cursor.position = new Vector3(magicHeroX, 683f, 0);
                }
                else
                {
                    cursor.position = new Vector3(magicHeroX, (683f - (magicHeroSpacerY * cursorOnMagicHero)), 0);
                }
            }

            if (magicMenuCursorState == MagicMenuCursorStates.USEMAGIC && !magicChoosingHero)
            {
                
                GridLayoutGroup glg;
                int magicCount;

                int row = Mathf.FloorToInt(cursorOnMagic / 3);
                int column = (cursorOnMagic - (row * 3));

                string magicMode = "";

                if (GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/WhiteMagicListPanel").GetComponent<CanvasGroup>().alpha == 1)
                {
                    magicMode = "White";
                } else if (GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/BlackMagicListPanel").GetComponent<CanvasGroup>().alpha == 1)
                {
                    magicMode = "Black";
                } else if (GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/SorceryMagicListPanel").GetComponent<CanvasGroup>().alpha == 1)
                {
                    magicMode = "Sorcery";
                }

                glg = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/" + magicMode + "MagicListPanel/" + magicMode + "MagicScroller/" + magicMode + "MagicListSpacer").GetComponent<GridLayoutGroup>();
                magicCount = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/" + magicMode + "MagicListPanel/" + magicMode + "MagicScroller/" + magicMode + "MagicListSpacer").transform.childCount;

                //need to get max column and row count
                int maxColumn = magicCount % 3; //if = 0, max column has been hit
                if (maxColumn == 0)
                    maxColumn = 3;
                maxColumn = maxColumn - 1;

                int maxRow = 0;

                GetMaxRow(glg, out maxRow);

                //note:
                //above variables all start with 0 - ie 0 is first row, 0 is first column
                
                //Debug.Log("-----before-----");
                //Debug.Log("Row: " + row + ", Column: " + column);
                //Debug.Log("cursorOnMagic: " + cursorOnMagic);
                //Debug.Log("magicPosX: " + magicPosX + ", magicPosY: " + magicPosY);
                //Debug.Log(cursor.position);
                //Debug.Log("----------------");

                if (magicCount == 0)
                {
                    if (chosenMagic == "White")
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONWHITE;
                    } else if (chosenMagic == "Black")
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONBLACK;
                    } else if (chosenMagic == "Sorcery")
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONSORC;
                    }
                }

                if (Input.GetButtonDown("Cancel") || cancelPressed)
                {
                    //buttonPressed = true;
                    if (chosenMagic == "White")
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONWHITE;
                    }
                    else if (chosenMagic == "Black")
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONBLACK;
                    }
                    else if (chosenMagic == "Sorcery")
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONSORC;
                    }
                }


                if (!dpadPressed && magicCount > 1)
                {
                    if (row != 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            row = row - 1;
                            cursorOnMagic = cursorOnMagic - 3;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            //Debug.Log("row: " + row);
                        }
                    }

                    if (row != maxRow && row != (maxRow - 1))
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            row = row + 1;
                            cursorOnMagic = cursorOnMagic + 3;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            //Debug.Log("row: " + row);
                        }
                    } else if (row == (maxRow - 1))
                    {
                        int tempCursor = cursorOnMagic + 4; //keeps cursor from moving down if there is no magic available in that space

                        if (Input.GetAxisRaw("DpadVertical") == -1 && tempCursor <= magicCount) //down
                        {
                            row = row + 1;
                            cursorOnMagic = cursorOnMagic + 3;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            //Debug.Log("row: " + row);
                        }
                    }

                    if (column != 0)
                    {
                        if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                        {
                            column = column - 1;
                            cursorOnMagic = cursorOnMagic - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            //Debug.Log("column: " + column);
                        }
                    }

                    if ((row == maxRow && column != maxColumn) || (row != maxRow && maxRow > 0 && column < 2))
                    {
                        if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                        {
                            column = column + 1;
                            cursorOnMagic = cursorOnMagic + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            //Debug.Log("column: " + column);
                        }
                    }
                }

                if (!dpadPressed && magicCount == 1)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONWHITE;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        //Debug.Log("row: " + row);
                    }
                }

                if (magicCount > 0)
                {
                    selectedMagic = GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/" + magicMode + "MagicListPanel/" + magicMode + "MagicScroller/" + magicMode + "MagicListSpacer").
                        transform.GetChild(cursorOnMagic).GetComponent<MagicMouseEvents>();

                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;

                        Debug.Log("confirmed pressed on spell: " + selectedMagic.GetMagic(selectedMagic.GetMagicName()).name);

                        if (selectedMagic.GetMagic(selectedMagic.GetMagicName()).usableInMenu && selectedMagic.GetMagic(selectedMagic.GetMagicName()).enoughMP)
                        {
                            if (selectedMagic.GetMagic(selectedMagic.GetMagicName()).useState == BaseAttack.UseStates.HERO)
                            {
                                selectedMagic.magicUsed = selectedMagic.GetMagic(selectedMagic.GetMagicName());
                                selectedMagic.processMagic = StartCoroutine(selectedMagic.ProcessMagic());

                                selectedMagic.heroToCastOn = null;
                                magicChoosingHero = true;
                            }
                        }
                        else
                        {
                            PlaySE(AudioManager.instance.cantActionSE);
                        }
                    }

                    selectedMagic.magicDesc.text = selectedMagic.GetMagic(selectedMagic.GetMagicName()).description;
                    selectedMagic.cooldownText.text = selectedMagic.GetMagic(selectedMagic.GetMagicName()).cooldown.ToString();
                    selectedMagic.MPCostText.text = selectedMagic.GetMagic(selectedMagic.GetMagicName()).MPCost.ToString();

                    cursor.position = new Vector3((magicPosX + (column * magicSpacerX)), (magicPosY - (row * magicSpacerY)), cursor.position.z); //set x,y
                }                               
            }

            if (magicMenuCursorState == MagicMenuCursorStates.OPTIONWHITE && !magicChoosingHero)
            {
                cursor.position = new Vector3(603f, 714f, 0); //set x,y

                chosenMagic = "White";

                if (!dpadPressed)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONBLACK;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        //Debug.Log("row: " + row);
                    }
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    cursorOnMagic = 0;
                    ShowWhiteMagicListPanel();
                    magicMenuCursorState = MagicMenuCursorStates.USEMAGIC;
                }
            }

            if (magicMenuCursorState == MagicMenuCursorStates.OPTIONBLACK && !magicChoosingHero)
            {
                cursor.position = new Vector3(603f, 692f, 0); //set x,y

                chosenMagic = "Black";

                if (!dpadPressed)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONSORC;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        //Debug.Log("row: " + row);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONWHITE;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        //Debug.Log("row: " + row);
                    }
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    cursorOnMagic = 0;
                    ShowBlackMagicListPanel();
                    magicMenuCursorState = MagicMenuCursorStates.USEMAGIC;
                }
            }

            if (magicMenuCursorState == MagicMenuCursorStates.OPTIONSORC && !magicChoosingHero)
            {
                cursor.position = new Vector3(603f, 670f, 0); //set x,y

                chosenMagic = "Sorcery";

                if (!dpadPressed)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnMagic = 0;
                        magicMenuCursorState = MagicMenuCursorStates.USEMAGIC;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        //Debug.Log("row: " + row);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        magicMenuCursorState = MagicMenuCursorStates.OPTIONBLACK;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        //Debug.Log("row: " + row);
                    }
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    cursorOnMagic = 0;
                    ShowSorceryMagicListPanel();
                    magicMenuCursorState = MagicMenuCursorStates.USEMAGIC;
                }
            }
        }
    }

    void GetMaxRow(GridLayoutGroup glg, out int row)
    {
        row = 0;

        if (glg.transform.childCount == 0)
            return;

        //Column and row are now 1
        row = 1;

        //Get the first child GameObject of the GridLayoutGroup
        RectTransform firstChildObj = glg.transform.
            GetChild(0).GetComponent<RectTransform>();

        Vector2 firstChildPos = firstChildObj.anchoredPosition;

        //Loop through the rest of the child object
        for (int i = 1; i < glg.transform.childCount; i++)
        {
            //Get the next child
            RectTransform currentChildObj = glg.transform.
           GetChild(i).GetComponent<RectTransform>();

            Vector2 currentChildPos = currentChildObj.anchoredPosition;

            //if first child.x == otherchild.x, it is a column, ele it's a row
            if (firstChildPos.x == currentChildPos.x)
            {
                row++;
            }
        }

        row = row - 1;
    }

    void UpdateEquipCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && equipMenuCursorState != EquipMenuCursorStates.IDLE && !inAnimation)
        {
            if (equipMenuCursorState == EquipMenuCursorStates.EQUIPSLOTS)
            {
                if (!dpadPressed && (cursorOnEquipSlot == 0 || cursorOnEquipSlot == 4))
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnEquipSlot = cursorOnEquipSlot + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        equipMenuCursorState = EquipMenuCursorStates.EQUIPOPTION;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnEquipSlot == 3 || cursorOnEquipSlot == 7)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnEquipSlot = cursorOnEquipSlot - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && (cursorOnEquipSlot < 3 || cursorOnEquipSlot > 4) && cursorOnEquipSlot < 7 && cursorOnEquipSlot != 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnEquipSlot = cursorOnEquipSlot + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnEquipSlot = cursorOnEquipSlot - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (!dpadPressed && cursorOnEquipSlot >= 0 && cursorOnEquipSlot <= 3)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        cursorOnEquipSlot = cursorOnEquipSlot + 4;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                } else if (!dpadPressed && cursorOnEquipSlot >= 4 && cursorOnEquipSlot <= 7)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        cursorOnEquipSlot = cursorOnEquipSlot - 4;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (cursorOnEquipSlot >= 0 && cursorOnEquipSlot <= 3)
                {
                    cursor.position = new Vector3(equipRow1X, equipSlotY - (cursorOnEquipSlot * equipSlotSpacerY), 0.0f);
                } else if (cursorOnEquipSlot >= 4 && cursorOnEquipSlot <= 7)
                {
                    cursor.position = new Vector3(equipRow2X, equipSlotY - ((cursorOnEquipSlot - 4) * equipSlotSpacerY), 0.0f);
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    if (cursorOnEquipSlot == 0)
                    {
                        equipButtonClicked = "RightArmButton";
                    } else if (cursorOnEquipSlot == 1)
                    {
                        equipButtonClicked = "HeadButton";
                    } else if (cursorOnEquipSlot == 2)
                    {
                        equipButtonClicked = "ChestButton";
                    } else if (cursorOnEquipSlot == 3)
                    {
                        equipButtonClicked = "FeetButton";
                    } else if (cursorOnEquipSlot == 4)
                    {
                        equipButtonClicked = "LeftArmButton";
                    } else if (cursorOnEquipSlot == 5)
                    {
                        equipButtonClicked = "HandsButton";
                    } else if (cursorOnEquipSlot == 6)
                    {
                        equipButtonClicked = "LegsButton";
                    } else if (cursorOnEquipSlot == 7)
                    {
                        equipButtonClicked = "RelicButton";
                    }

                    if (equipMode == "Equip")
                    {
                        cursorOnEquipList = 0;
                        ListEquipment();
                        equipMenuCursorState = EquipMenuCursorStates.EQUIPLIST;
                    }
                    else if (equipMode == "Remove")
                    {
                        ListEquipment();
                    }                    
                }

            }

            if (equipMenuCursorState == EquipMenuCursorStates.EQUIPLIST)
            {
                int equipListCount = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.childCount;

                int scrollDiff = equipListCount - 7;

                if (equipListCount <= 7)
                {
                    tempScrollDiff = 0;
                }

                //Debug.Log("scrollDiff: " + scrollDiff + ", tempScrollDiff: " + tempScrollDiff + ", equipListCount: " + equipListCount +", cursorOnEquipList: " + cursorOnEquipList);

                RectTransform spacerScroll = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").GetComponent<RectTransform>();

                if (!dpadPressed && cursorOnEquipList == 0 && equipListCount > 1 && tempScrollDiff == 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnEquipList = cursorOnEquipList + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnEquipList == 0 && equipListCount > 1 && tempScrollDiff > 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnEquipList = cursorOnEquipList + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (equipListScrollBy * (tempScrollDiff - 1)), 0.0f);
                        tempScrollDiff = tempScrollDiff - 1;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                }
                else if (!dpadPressed && cursorOnEquipList > 0 && cursorOnEquipList < 6 && equipListCount > 1)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnEquipList = cursorOnEquipList + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnEquipList = cursorOnEquipList - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnEquipList == 6 && tempScrollDiff == 0 && scrollDiff > 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnEquipList = cursorOnEquipList - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, equipListScrollBy, 0.0f);

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnEquipList == 6 && tempScrollDiff > 0 && (cursorOnEquipList + tempScrollDiff) < (equipListCount - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnEquipList = 5;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (equipListScrollBy * (tempScrollDiff + 1)), 0.0f);

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                } else if (!dpadPressed && cursorOnEquipList == 6 && tempScrollDiff > 0 && (cursorOnEquipList + tempScrollDiff) == (equipListCount - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnEquipList = 5;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                cursor.position = new Vector3(equipListX, 529.35f - (cursorOnEquipList * equipListSpacerY), 0.0f);

                if (equipListCount != 0)
                {
                    if (cursorOnEquipList == equipListCount)
                    {
                        cursorOnEquipList = equipListCount - 1;
                    }

                    if (cursorOnEquipList == 0 && tempScrollDiff >= 0)
                    {
                        cursor.position = new Vector3(equipListX, 529.35f, 0);
                    }
                    else if (cursorOnEquipList > 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(equipListX, 529.35f - (cursorOnEquipList * equipListSpacerY), 0);
                    }
                    else
                    {
                        cursor.position = new Vector3(equipListX, 529.35f - (cursorOnEquipList * equipListSpacerY), 0);
                    }

                }

                if (cancelPressed || Input.GetButtonDown("Cancel"))
                {
                    equipMenuCursorState = EquipMenuCursorStates.EQUIPSLOTS;
                }

                equipListItem = GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform.GetChild(cursorOnEquipList).GetComponent<EquipMouseEvents>();
                if (equipListItem.transform.GetChild(0).GetComponent<Text>().text != "None")
                {
                    equipListItem.ShowEquipDetails();
                } else
                {
                    GameObject.Find("EquipMenuCanvas/EquipMenuPanel/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>().text = "";
                    ShowEquipmentStatUpdates(null);
                }
                
                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    if (cursorOnEquipList != 0)
                    {
                        equipListItem.SetEquip();
                    } else
                    {
                        ChangeEquipment(null);
                        PlaySE(AudioManager.instance.equipSE);
                    }

                    equipMenuCursorState = EquipMenuCursorStates.EQUIPSLOTS;
                }                
            }

            if (equipMenuCursorState == EquipMenuCursorStates.EQUIPOPTION)
            {
                cursor.position = new Vector3(477f, 705f, 0);

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    ChangeEquipMode("Equip");

                    cursorOnEquipSlot = 0;
                    equipMenuCursorState = EquipMenuCursorStates.EQUIPSLOTS;
                }
                if (Input.GetAxisRaw("DpadVertical") == -1 && !dpadPressed) //down
                {
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    equipMenuCursorState = EquipMenuCursorStates.EQUIPSLOTS;
                }
                if (Input.GetAxisRaw("DpadHorizontal") == 1 && !dpadPressed) //right
                {
                    equipMenuCursorState = EquipMenuCursorStates.REMOVEOPTION;
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                }
            }

            if (equipMenuCursorState == EquipMenuCursorStates.REMOVEOPTION)
            {
                cursor.position = new Vector3(540f, 705f, 0);

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    ChangeEquipMode("Remove");

                    cursorOnEquipSlot = 0;
                    equipMenuCursorState = EquipMenuCursorStates.EQUIPSLOTS;
                }
                if (Input.GetAxisRaw("DpadVertical") == -1 && !dpadPressed) //down
                {
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    equipMenuCursorState = EquipMenuCursorStates.EQUIPSLOTS;
                }
                if (Input.GetAxisRaw("DpadHorizontal") == 1 && !dpadPressed) //right
                {
                    equipMenuCursorState = EquipMenuCursorStates.REMOVEALLOPTION;
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                }
                if (Input.GetAxisRaw("DpadHorizontal") == -1 && !dpadPressed) //left
                {
                    equipMenuCursorState = EquipMenuCursorStates.EQUIPOPTION;
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                }
            }

            if (equipMenuCursorState == EquipMenuCursorStates.REMOVEALLOPTION)
            {
                cursor.position = new Vector3(613f, 705f, 0);

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    RemoveAllEquipment();
                }
                if (Input.GetAxisRaw("DpadVertical") == -1 && !dpadPressed) //down
                {
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    equipMenuCursorState = EquipMenuCursorStates.EQUIPSLOTS;
                }
                if (Input.GetAxisRaw("DpadHorizontal") == -1 && !dpadPressed) //left
                {
                    equipMenuCursorState = EquipMenuCursorStates.REMOVEOPTION;
                    dpadPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                }
            }
        }

    }

    void UpdateStatusCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && statusMenuCursorState != StatusMenuCursorStates.IDLE && !inAnimation)
        {
            if (statusMenuCursorState == StatusMenuCursorStates.STATS)
            {
                if (!dpadPressed && cursorOnStat >= 0 && cursorOnStat <= 15)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnStat = cursorOnStat + 2;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnStat >= 2 && cursorOnStat <= 17)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnStat = cursorOnStat - 2;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnStat%2 == 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        cursorOnStat = cursorOnStat + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnStat%2 != 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        cursorOnStat = cursorOnStat - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    cursorOnEquip = 0;
                    statusMenuCursorState = StatusMenuCursorStates.EQUIPMENT;
                    PlaySE(AudioManager.instance.confirmSE);
                }

                //set cursor position and details text
                if (cursorOnStat == 0) //STR
                {
                    cursor.position = new Vector3(138f, 620f, 0);

                    int atk = Mathf.RoundToInt(heroToCheck.finalStrength * .5f);
                    int parry = Mathf.FloorToInt(heroToCheck.finalStrength * .1f);

                    StatusPanelDetailsText.text = "Increases physical attack rating by " + statColor + atk.ToString() + "</color> and parry chance by " + statColor + parry.ToString() + "%</color>";
                }
                else if (cursorOnStat == 1) //STA
                {
                    cursor.position = new Vector3(253f, 620f, 0);

                    int def = Mathf.RoundToInt(heroToCheck.finalStamina * .6f);
                    int HP = Mathf.RoundToInt(heroToCheck.finalStamina * .75f);
                    int mdef = Mathf.RoundToInt(heroToCheck.finalStamina * .5f);

                    StatusPanelDetailsText.text = "Increases max HP by " + statColor + HP.ToString() + "</color>, physical defense rating by " + statColor + def.ToString() +
                        "</color>, and magical defense rating by " + statColor + mdef.ToString();
                }
                else if (cursorOnStat == 2) //AGI
                {
                    cursor.position = new Vector3(138f, 604f, 0);

                    int hit = Mathf.FloorToInt(heroToCheck.finalAgility * .12f);
                    int dodge = Mathf.FloorToInt(heroToCheck.finalAgility * .2f);

                    StatusPanelDetailsText.text = "Increases hit chance on physical attacks by " + statColor + hit.ToString() + "%</color> and dodge chance by " + statColor + dodge.ToString() + "%</color>";
                }
                else if (cursorOnStat == 3) //DEX
                {
                    cursor.position = new Vector3(253f, 604f, 0);

                    int crit = Mathf.FloorToInt(heroToCheck.finalDexterity * .12f);
                    int move = Mathf.FloorToInt(heroToCheck.finalDexterity * .02f);
                    int parry = Mathf.FloorToInt(heroToCheck.finalDexterity * .05f);

                    StatusPanelDetailsText.text = "Increases critical chance on physical attacks by " + statColor + crit.ToString() + "%</color>, move rating by " + statColor + 
                        move.ToString() + "</color>, and parry chance by " + statColor + parry.ToString() + "%</color>";
                }
                else if (cursorOnStat == 4) //INT
                {
                    cursor.position = new Vector3(138f, 588f, 0);

                    int matk = Mathf.RoundToInt(heroToCheck.finalIntelligence * .5f);
                    int MP = Mathf.RoundToInt(heroToCheck.finalIntelligence * .75f);

                    StatusPanelDetailsText.text = "Increases max MP by " + statColor + MP.ToString() + "</color> and magic attack rating by " + statColor + matk.ToString() + "</color>";
                }
                else if (cursorOnStat == 5) //SPI
                {
                    cursor.position = new Vector3(253f, 588f, 0);

                    int regen = Mathf.CeilToInt(heroToCheck.finalSpirit * .15f);

                    StatusPanelDetailsText.text = "Increases MP regen by " + statColor + regen.ToString() + "</color>";
                }
                else if (cursorOnStat == 6) //ATK
                {
                    cursor.position = new Vector3(138f, 549f, 0);

                    StatusPanelDetailsText.text = "Increases damage done by physical attacks by " + statColor + heroToCheck.finalATK.ToString() + "</color>";
                }
                else if (cursorOnStat == 7) //MATK
                {
                    cursor.position = new Vector3(251f, 549f, 0);

                    StatusPanelDetailsText.text = "Increases damage done by magical attacks by " + statColor + heroToCheck.finalMATK.ToString() + "</color>";
                }
                else if (cursorOnStat == 8) //DEF
                {
                    cursor.position = new Vector3(138f, 534f, 0);
                    
                    StatusPanelDetailsText.text = "Decreases damage done by physical attacks by " + statColor + heroToCheck.finalDEF.ToString() + "</color>";
                }
                else if (cursorOnStat == 9) //MDEF
                {
                    cursor.position = new Vector3(251f, 534f, 0);

                    StatusPanelDetailsText.text = "Decreases damage done by magical attacks by " + statColor + heroToCheck.finalMDEF.ToString() + "</color>";
                }
                else if (cursorOnStat == 10) //HIT
                {
                    cursor.position = new Vector3(138f, 496f, 0);
                    
                    StatusPanelDetailsText.text = statColor + heroToCheck.finalHitRating.ToString() + "%</color> chance to hit with physical attacks";
                }
                else if (cursorOnStat == 11) //CRIT
                {
                    cursor.position = new Vector3(241f, 496f, 0);

                    StatusPanelDetailsText.text = statColor + heroToCheck.finalCritRating.ToString() + "%</color> chance to critically strike with physical attacks";
                }
                else if (cursorOnStat == 12) //MOVE
                {
                    cursor.position = new Vector3(138f, 479f, 0);

                    StatusPanelDetailsText.text = statColor + heroToCheck.finalMoveRating.ToString() + "</color> spaces available to move per turn";
                }
                else if (cursorOnStat == 13) //REGEN
                {
                    cursor.position = new Vector3(241f, 479f, 0);

                    StatusPanelDetailsText.text = "Recovers " + statColor + heroToCheck.finalRegenRating.ToString() + "</color> MP per turn";
                }
                else if (cursorOnStat == 14) //DODGE
                {
                    cursor.position = new Vector3(138f, 449f, 0);

                    StatusPanelDetailsText.text = statColor + heroToCheck.finalDodgeRating.ToString() + "%</color> chance to avoid a physical attack";
                }
                else if (cursorOnStat == 15) //BLOCK
                {
                    cursor.position = new Vector3(241f, 449f, 0);

                    StatusPanelDetailsText.text = statColor + heroToCheck.finalBlockRating.ToString() + "%</color> chance to reduce a physical attack's damage by half";
                }
                else if (cursorOnStat == 16) //PARRY
                {
                    cursor.position = new Vector3(138f, 432f, 0);

                    StatusPanelDetailsText.text = statColor + heroToCheck.finalParryRating.ToString() + "%</color> chance to reduce a physical attack's damage by 1/3, and immediately counter attack";
                }
                else if (cursorOnStat == 17) //THREAT
                {
                    cursor.position = new Vector3(241f, 432f, 0);

                    StatusPanelDetailsText.text = "Increases threat generated by " + statColor + heroToCheck.finalThreatRating.ToString() + "</color>";
                }
            }

            if (statusMenuCursorState == StatusMenuCursorStates.EQUIPMENT)
            {
                if (!dpadPressed && cursorOnEquip >= 0 && cursorOnEquip <= 5)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnEquip = cursorOnEquip + 2;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnEquip >= 2 && cursorOnEquip <= 7)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnEquip = cursorOnEquip - 2;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnEquip % 2 == 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        cursorOnEquip = cursorOnEquip + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnEquip % 2 != 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        cursorOnEquip = cursorOnEquip - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (cursorOnEquip == 0) //R.Arm
                {
                    cursor.position = new Vector3(352f, 618f, 0);

                    if (heroToCheck.equipment[6] == null) {
                        StatusPanelDetailsText.text = "";
                    } else
                    {
                        StatusPanelDetailsText.text = heroToCheck.equipment[6].description;
                    }                    
                }
                else if (cursorOnEquip == 1) //L.Arm
                {
                    cursor.position = new Vector3(525f, 618f, 0);

                    if (heroToCheck.equipment[7] == null)
                    {
                        StatusPanelDetailsText.text = "";
                    }
                    else
                    {
                        StatusPanelDetailsText.text = heroToCheck.equipment[7].description;
                    }
                }
                else if (cursorOnEquip == 2) //Head
                {
                    cursor.position = new Vector3(352f, 594f, 0);

                    if (heroToCheck.equipment[0] == null)
                    {
                        StatusPanelDetailsText.text = "";
                    }
                    else
                    {
                        StatusPanelDetailsText.text = heroToCheck.equipment[0].description;
                    }
                }
                else if (cursorOnEquip == 3) //Hands
                {
                    cursor.position = new Vector3(525f, 594f, 0);

                    if (heroToCheck.equipment[2] == null)
                    {
                        StatusPanelDetailsText.text = "";
                    }
                    else
                    {
                        StatusPanelDetailsText.text = heroToCheck.equipment[2].description;
                    }
                }
                else if (cursorOnEquip == 4) //Chest
                {
                    cursor.position = new Vector3(352f, 570f, 0);

                    if (heroToCheck.equipment[1] == null)
                    {
                        StatusPanelDetailsText.text = "";
                    }
                    else
                    {
                        StatusPanelDetailsText.text = heroToCheck.equipment[1].description;
                    }
                }
                else if (cursorOnEquip == 5) //Legs
                {
                    cursor.position = new Vector3(525f, 570f, 0);

                    if (heroToCheck.equipment[3] == null)
                    {
                        StatusPanelDetailsText.text = "";
                    }
                    else
                    {
                        StatusPanelDetailsText.text = heroToCheck.equipment[3].description;
                    }
                }
                else if (cursorOnEquip == 6) //Feet
                {
                    cursor.position = new Vector3(352f, 546f, 0);

                    if (heroToCheck.equipment[4] == null)
                    {
                        StatusPanelDetailsText.text = "";
                    }
                    else
                    {
                        StatusPanelDetailsText.text = heroToCheck.equipment[4].description;
                    }
                }
                else if (cursorOnEquip == 7) //Relic
                {
                    cursor.position = new Vector3(525f, 546f, 0);

                    if (heroToCheck.equipment[5] == null)
                    {
                        StatusPanelDetailsText.text = "";
                    }
                    else
                    {
                        StatusPanelDetailsText.text = heroToCheck.equipment[5].description;
                    }
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    cursorOnEle = 0;
                    statusMenuCursorState = StatusMenuCursorStates.ELEMENTS;
                    PlaySE(AudioManager.instance.confirmSE);
                }
            }

            if (statusMenuCursorState == StatusMenuCursorStates.ELEMENTS)
            {
                if (!dpadPressed && cursorOnEle >= 0 && cursorOnEle <= 13)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnEle = cursorOnEle + 2;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnEle >= 2 && cursorOnEle <= 15)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnEle = cursorOnEle - 2;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnEle % 2 == 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        cursorOnEle = cursorOnEle + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnEle % 2 != 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        cursorOnEle = cursorOnEle - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (cursorOnEle == 0) //Fire Attack
                {
                    cursor.position = new Vector3(396f, 508f, 0);
                    
                    StatusPanelDetailsText.text = "Increases effectiveness of " + fireColor + "Fire</color> magic by " + fireColor + heroToCheck.fireEff  + "</color>";
                }
                else if (cursorOnEle == 1) //Fire Defense
                {
                    cursor.position = new Vector3(471f, 508f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + fireColor + "Fire</color> magic by " + fireColor + heroToCheck.fireDef +  "</color>";
                }
                else if (cursorOnEle == 2) //Frost Attack
                {
                    cursor.position = new Vector3(396f, 495f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + frostColor + "Frost</color> magic by " + frostColor + heroToCheck.frostEff +  "</color>";
                }
                else if (cursorOnEle == 3) //Frost Defense
                {
                    cursor.position = new Vector3(471f, 495f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + frostColor + "Frost</color> magic by " + frostColor + heroToCheck.frostDef +  "</color>";
                }
                else if (cursorOnEle == 4) //Lightning Attack
                {
                    cursor.position = new Vector3(396f, 482f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + lightningColor + "Lightning</color> magic by " + lightningColor + heroToCheck.lightningEff + "</color>";
                }
                else if (cursorOnEle == 5) //Lightning Defense
                {
                    cursor.position = new Vector3(471f, 482f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + lightningColor + "Lightning</color> magic by " + lightningColor + heroToCheck.lightningDef + "</color>";
                }
                else if (cursorOnEle == 6) //Water Attack
                {
                    cursor.position = new Vector3(396f, 469f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + waterColor + "Water</color> magic by " + waterColor + heroToCheck.waterEff + "</color>";
                }
                else if (cursorOnEle == 7) //Water Defense
                {
                    cursor.position = new Vector3(471f, 469f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + waterColor + "Water</color> magic by " + waterColor + heroToCheck.waterDef + "</color>";
                }
                else if (cursorOnEle == 8) //Nature Attack
                {
                    cursor.position = new Vector3(396f, 456f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + natureColor + "Nature</color> magic by " + natureColor + heroToCheck.natureEff + "</color>";
                }
                else if (cursorOnEle == 9) //Nature Defense
                {
                    cursor.position = new Vector3(471f, 456f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + natureColor + "Nature</color> magic by " + natureColor + heroToCheck.natureDef + "</color>";
                }
                else if (cursorOnEle == 10) //Earth Attack
                {
                    cursor.position = new Vector3(396f, 443f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + earthColor + "Earth</color> magic by " + earthColor + heroToCheck.earthEff + "</color>";
                }
                else if (cursorOnEle == 11) //Earth Defense
                {
                    cursor.position = new Vector3(471f, 443f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + earthColor + "Earth</color> magic by " + earthColor + heroToCheck.earthDef + "</color>";
                }
                else if (cursorOnEle == 12) //Holy Attack
                {
                    cursor.position = new Vector3(396f, 430f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + holyColor + "Holy</color> magic by " + holyColor + heroToCheck.holyEff + "</color>";
                }
                else if (cursorOnEle == 13) //Holy Defense
                {
                    cursor.position = new Vector3(471f, 430f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + holyColor + "Holy</color> magic by " + holyColor + heroToCheck.holyDef + "</color>";
                }
                else if (cursorOnEle == 14) //Dark Attack
                {
                    cursor.position = new Vector3(396f, 417f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + darknessColor + "Dark</color> magic by " + darknessColor + heroToCheck.darkEff + "</color>";
                }
                else if (cursorOnEle == 15) //Dark Defense
                {
                    cursor.position = new Vector3(471f, 417f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + darknessColor + "Dark</color> magic by " + darknessColor + heroToCheck.darkDef + "</color>";
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    cursorOnRes = 0;
                    statusMenuCursorState = StatusMenuCursorStates.RESISTANCES;
                    PlaySE(AudioManager.instance.confirmSE);
                }
            }

            if (statusMenuCursorState == StatusMenuCursorStates.RESISTANCES)
            {
                if (!dpadPressed && cursorOnRes >= 0 && cursorOnRes <= 13)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnRes = cursorOnRes + 2;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnRes >= 2 && cursorOnRes <= 15)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnRes = cursorOnRes - 2;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnRes % 2 == 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        cursorOnRes = cursorOnRes + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnRes % 2 != 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        cursorOnRes = cursorOnRes - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (cursorOnRes == 0) //Blind Attack
                {
                    cursor.position = new Vector3(565f, 508f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + blindColor + "Blind</color> effects by " + blindColor + heroToCheck.blindEff + "</color>";
                }
                else if (cursorOnRes == 1) //Blind Defense
                {
                    cursor.position = new Vector3(641f, 508f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + blindColor + "Blind</color> effects by " + blindColor + heroToCheck.blindDef + "</color>";
                }
                else if (cursorOnRes == 2) //Silence Attack
                {
                    cursor.position = new Vector3(565f, 495f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + silenceColor + "Silence</color> effects by " + silenceColor + heroToCheck.silenceEff + "</color>";
                }
                else if (cursorOnRes == 3) //Silence Defense
                {
                    cursor.position = new Vector3(641f, 495f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + silenceColor + "Silence</color> effects by " + silenceColor + heroToCheck.silenceDef + "</color>";
                }
                else if (cursorOnRes == 4) //Sleep Attack
                {
                    cursor.position = new Vector3(565f, 482f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + sleepColor + "Sleep</color> effects by " + sleepColor + heroToCheck.sleepEff + "</color>";
                }
                else if (cursorOnRes == 5) //Sleep Defense
                {
                    cursor.position = new Vector3(641f, 482f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + sleepColor + "Sleep</color> effects by " + sleepColor + heroToCheck.sleepDef + "</color>";
                }
                else if (cursorOnRes == 6) //Confuse Attack
                {
                    cursor.position = new Vector3(565f, 469f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + confuseColor + "Confuse</color> effects by " + confuseColor + heroToCheck.confuseEff + "</color>";
                }
                else if (cursorOnRes == 7) //Confuse Defense
                {
                    cursor.position = new Vector3(641f, 469f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + confuseColor + "Confuse</color> effects by " + confuseColor + heroToCheck.confuseDef + "</color>";
                }
                else if (cursorOnRes == 8) //Poison Attack
                {
                    cursor.position = new Vector3(565f, 456f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + poisonColor + "Poison</color> effects by " + poisonColor + heroToCheck.poisonEff + "</color>";
                }
                else if (cursorOnRes == 9) //Poison Defense
                {
                    cursor.position = new Vector3(641f, 456f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + poisonColor + "Poison</color> effects by " + poisonColor + heroToCheck.poisonDef + "</color>";
                }
                else if (cursorOnRes == 10) //Petrify Attack
                {
                    cursor.position = new Vector3(565f, 443f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + petrifyColor + "Petrify</color> effects by " + petrifyColor + heroToCheck.petrifyEff + "</color>";
                }
                else if (cursorOnRes == 11) //Petrify Defense
                {
                    cursor.position = new Vector3(641f, 443f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + petrifyColor + "Petrify</color> effects by " + petrifyColor + heroToCheck.petrifyDef + "</color>";
                }
                else if (cursorOnRes == 12) //Slow Attack
                {
                    cursor.position = new Vector3(565f, 430f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + slowColor + "Slow</color> effects by " + slowColor + heroToCheck.slowEff + "</color>";
                }
                else if (cursorOnRes == 13) //Slow Defense
                {
                    cursor.position = new Vector3(641f, 430f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + slowColor + "Slow</color> effects by " + slowColor + heroToCheck.slowDef + "</color>";
                }
                else if (cursorOnRes == 14) //Zombie Attack
                {
                    cursor.position = new Vector3(565f, 417f, 0);

                    StatusPanelDetailsText.text = "Increases effectiveness of " + zombieColor + "Zombie</color> effects by " + zombieColor + heroToCheck.zombieEff + "</color>";
                }
                else if (cursorOnRes == 15) //Zombie Defense
                {
                    cursor.position = new Vector3(641f, 417f, 0);

                    StatusPanelDetailsText.text = "Increases resistance to " + zombieColor + "Zombie</color> effects by " + zombieColor + heroToCheck.zombieDef + "</color>";
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    inStatusSkills = true;

                    statusMenuCursorState = StatusMenuCursorStates.SKILLS;
                    PlaySE(AudioManager.instance.confirmSE);

                    StartCoroutine(AnimateSecondaryStatusMenu());
                }
            }

            if (statusMenuCursorState == StatusMenuCursorStates.SKILLS)
            {
                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    inStatusSkills = false;

                    StartCoroutine(AnimateSecondaryStatusMenu());

                    cursorOnStat = 0;
                    statusMenuCursorState = StatusMenuCursorStates.STATS;
                }

                if (Input.GetButtonDown("Cancel") || cancelPressed)
                {
                    inStatusSkills = false;

                    StartCoroutine(AnimateSecondaryStatusMenu());

                    cursorOnStat = 0;
                    statusMenuCursorState = StatusMenuCursorStates.STATS;
                    
                }
            }
        }
    }

    void UpdateTalentsCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && talentMenuCursorState != TalentsMenuCursorStates.IDLE && !inAnimation)
        {
            TalentsMouseEvents talentEvent;

            if (talentMenuCursorState == TalentsMenuCursorStates.TALENTS)
            {
                if (!dpadPressed && cursorOnTalent >= 0 && talentRow <= 4)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnTalent = cursorOnTalent + 3;
                        talentRow = talentRow + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && talentRow >= 1 && talentRow <= 5)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnTalent = cursorOnTalent - 3;
                        talentRow = talentRow - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && talentColumn <= 1)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        cursorOnTalent = cursorOnTalent + 1;
                        talentColumn = talentColumn + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && talentColumn >= 1)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        cursorOnTalent = cursorOnTalent - 1;
                        talentColumn = talentColumn - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                talentEvent = GameObject.Find("GameManager/Menus/TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent" +
                        (talentRow + 1).ToString() + "/Talent" + (talentColumn + 1).ToString() + "Button").GetComponent<TalentsMouseEvents>();

                talentEvent.ShowTalentDetails();

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    talentEvent.TalentButtonClicked();
                }

                cursor.position = new Vector3(386f + (talentColumnSpacer * talentColumn), 703f - (talentRowSpacer * talentRow), 0);
            }
        }
    }

    void UpdatePartyCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && partyMenuCursorState != PartyMenuCursorStates.IDLE && !inAnimation)
        {
            int activeHeroes = GameManager.instance.activeHeroes.Count;
            int inactiveHeroes = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/InactiveHeroesSpacer").transform.childCount;

            PartySelectEvents tempPSE;

            if (partyMenuCursorState == PartyMenuCursorStates.ACTIVE)
            {

                if (!dpadPressed && cursorOnPartyActive >= 0 && cursorOnPartyActive <= (activeHeroes - 2))
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnPartyActive = cursorOnPartyActive + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnPartyActive >= 1 && cursorOnPartyActive <= (activeHeroes - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnPartyActive = cursorOnPartyActive - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                
                if (!dpadPressed && inactiveHeroes > 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        dpadPressed = true;

                        cursorOnPartyInactive = 0;

                        partyMenuCursorState = PartyMenuCursorStates.INACTIVE;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (!dpadPressed && cursorOnPartyActive >= 0 && cursorOnPartyActive <= (activeHeroes - 1) && partyChoosing && partyChoosingMode == "Active")
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnPartyActive = cursorOnPartyActive + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (!dpadPressed && cursorOnPartyActive >= 1 && cursorOnPartyActive <= activeHeroes && partyChoosing)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnPartyActive = cursorOnPartyActive - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                
                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    if (partyChoosing)
                    {
                        pse2 = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(cursorOnPartyActive).GetComponent<PartySelectEvents>();
                        pse2.SelectHeroFromActive();

                        partyChoosing = false;
                    } else
                    {
                        pse1 = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(cursorOnPartyActive).GetComponent<PartySelectEvents>();
                        pse1.SelectHeroFromActive();

                        partyChoosingMode = "Inactive";
                        cursorOnPartyInactive = 0;
                        partyChoosing = true;
                        partyMenuCursorState = PartyMenuCursorStates.INACTIVE;
                    }
                }

                if ((Input.GetButtonDown("Cancel") || cancelPressed) && partyChoosing)
                {
                    pse1.UnboldButtons();
                    partyChoosing = false;
                }


                tempPSE = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/ActiveHeroesPanel").transform.GetChild(cursorOnPartyActive).GetComponent<PartySelectEvents>();
                tempPSE.ShowSelectedActiveHeroPanel();

                cursor.position = new Vector3(136f, 696f - (partyActiveSpacer * cursorOnPartyActive), 0);
            }

            if (partyMenuCursorState == PartyMenuCursorStates.INACTIVE)
            {
                GridLayoutGroup glg;

                int row = Mathf.FloorToInt(cursorOnPartyInactive / 3);
                int column = (cursorOnPartyInactive - (row * 3));

                glg = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/InactiveHeroesSpacer").GetComponent<GridLayoutGroup>();

                //need to get max column and row count
                int maxColumn = inactiveHeroes % 3; //if = 0, max column has been hit
                if (maxColumn == 0)
                    maxColumn = 3;
                maxColumn = maxColumn - 1;

                int maxRow = 0;

                GetMaxRow(glg, out maxRow);

                if (!dpadPressed && inactiveHeroes > 1)
                {
                    if (row != 0)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == 1) //up
                        {
                            cursorOnPartyInactive = cursorOnPartyInactive - 3;
                            row = row - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    
                    if (row != maxRow && row != (maxRow - 1))
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnPartyInactive = cursorOnPartyInactive + 3;
                            row = row + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                    else if (row == (maxRow - 1))
                    {
                        int tempCursor = cursorOnPartyInactive + 4; //keeps cursor from moving down if there is no magic available in that space

                        if (Input.GetAxisRaw("DpadVertical") == -1 && tempCursor <= inactiveHeroes) //down
                        {
                            cursorOnPartyInactive = cursorOnPartyInactive + 3;
                            row = row + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }

                    if (column != 0)
                    {
                        if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                        {
                            column = column - 1;
                            cursorOnPartyInactive = cursorOnPartyInactive - 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }

                    if ((row == maxRow && column != maxColumn) || (row != maxRow && maxRow > 0 && column < 2))
                    {
                        if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                        {
                            column = column + 1;
                            cursorOnPartyInactive = cursorOnPartyInactive + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                } else if (!dpadPressed && column == 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        partyMenuCursorState = PartyMenuCursorStates.ACTIVE;
                    }
                }
                
                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    if (partyChoosing)
                    {
                        if (inactiveHeroes == 0)
                        {
                            Debug.Log("needs to be implemented");
                        } else
                        {
                            pse2 = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/InactiveHeroesSpacer").transform.GetChild(cursorOnPartyInactive).GetComponent<PartySelectEvents>();
                            pse2.SelectHeroFromInactive();
                            partyChoosing = false;
                        }                        
                    } else
                    {
                        partyChoosing = true;
                        partyChoosingMode = "Active";
                        pse1 = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/InactiveHeroesSpacer").transform.GetChild(cursorOnPartyInactive).GetComponent<PartySelectEvents>();
                        pse1.SelectHeroFromInactive();
                    }
                    
                    partyMenuCursorState = PartyMenuCursorStates.ACTIVE;
                }

                if ((Input.GetButtonDown("Cancel") || cancelPressed) && partyChoosing)
                {
                    pse1.UnboldButtons();
                    partyChoosing = false;
                    partyMenuCursorState = PartyMenuCursorStates.ACTIVE;
                }

                tempPSE = GameObject.Find("GameManager/Menus/PartyMenuCanvas/PartyMenuPanel/InactiveHeroesPanel/InactiveHeroesSpacer").transform.GetChild(cursorOnPartyInactive).GetComponent<PartySelectEvents>();
                tempPSE.ShowSelectedHeroPanel();

                cursor.position = new Vector3(344f + (inactivePartySpacerX * column), 547f - (inactivePartySpacerY * row), 0);
            }
        }
    }

    void UpdateGridCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && gridMenuCursorState != GridMenuCursorStates.IDLE && !inAnimation)
        {
            GridMenuMouseEvents gme;

            if (gridMenuCursorState == GridMenuCursorStates.HEROES)
            {
                int activeHeroes = GameManager.instance.activeHeroes.Count;

                if (!dpadPressed && cursorOnGridHero >= 0 && cursorOnGridHero <= 3 && cursorOnGridHero < (activeHeroes - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnGridHero = cursorOnGridHero + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && cursorOnGridHero >= 1 && cursorOnGridHero <= 4)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnGridHero = cursorOnGridHero - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        gridColumn = 1;
                        gridRow = 1;

                        gridMenuCursorState = GridMenuCursorStates.GRID;
                    }
                }

                gme = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(cursorOnGridHero).GetComponent<GridMenuMouseEvents>();

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    if (!gridChoosingTile)
                    {
                        gme.SetHero();

                        gridColumn = int.Parse(GameManager.instance.activeHeroes[cursorOnGridHero].spawnPoint[0].ToString());
                        gridRow = int.Parse(GameManager.instance.activeHeroes[cursorOnGridHero].spawnPoint[1].ToString());

                        gridMenuCursorState = GridMenuCursorStates.GRID;
                    } else
                    {
                        gme.SetHero();
                    }
                }

                cursor.position = new Vector3(136f, 696f - (gridHeroSpacer * cursorOnGridHero), 0);
            }

            if (gridMenuCursorState == GridMenuCursorStates.GRID)
            {
                if (!dpadPressed && gridRow >= 1 && gridRow <= 4)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        gridRow = gridRow + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && gridRow >= 2 && gridRow <= 5)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        gridRow = gridRow - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && gridColumn >= 1 && gridColumn <= 4)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                        gridColumn = gridColumn + 1;

                        gridMenuCursorState = GridMenuCursorStates.GRID;
                    }
                }
                if (!dpadPressed && gridColumn >= 2 && gridColumn <= 5)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        gridColumn = gridColumn - 1;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                if (!dpadPressed && gridColumn == 1 && !gridChoosingTile)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        gridMenuCursorState = GridMenuCursorStates.HEROES;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                gme = GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + gridColumn.ToString() + gridRow.ToString()).GetComponent<GridMenuMouseEvents>();

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    gme.ChangeSpawnPoint();
                }

                if ((Input.GetButtonDown("Cancel") || cancelPressed) && gridChoosingTile)
                {
                    gme.EnableButtons();
                    gridChoosingTile = false;
                    gridMenuCursorState = GridMenuCursorStates.HEROES;
                }

                cursor.position = new Vector3(364f + (gridSpacer * (gridColumn - 1)), 696f - (gridSpacer * (gridRow - 1)), 0);
            }
        }
    }

    void UpdateQuestCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && questMenuCursorState != QuestMenuCursorStates.IDLE && !inAnimation)
        {
            int questCount;
            questCount = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/" + QuestOption + "QuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform.childCount;

            if (questCount == 0)
            {
                questMenuCursorState = QuestMenuCursorStates.OPTIONS;
            }

            if (questMenuCursorState == QuestMenuCursorStates.QUESTLIST)
            {
                int scrollDiff = questCount - 7;

                if (questCount <= 7)
                {
                    tempScrollDiff = 0;
                }

                RectTransform spacerScroll = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/" + QuestOption + "QuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").GetComponent<RectTransform>();

                //Debug.Log(cursorOnItem);

                if (!dpadPressed && cursorOnQuest == 0 && tempScrollDiff == 0)
                {
                    if (questCount > 1)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnQuest = cursorOnQuest + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        questMenuCursorState = QuestMenuCursorStates.OPTIONS;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnQuest == 0 && tempScrollDiff > 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnQuest = cursorOnQuest + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (questListScrollBy * (tempScrollDiff - 1)), 0.0f);

                        tempScrollDiff = tempScrollDiff - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnQuest > 0 && cursorOnQuest < 6)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnQuest = cursorOnQuest - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnQuest = cursorOnQuest + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnQuest == 6 && tempScrollDiff == 0 && scrollDiff > 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnQuest = cursorOnQuest - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, questListScrollBy, 0.0f);

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnQuest == 6 && tempScrollDiff == 0 && scrollDiff == 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnQuest = cursorOnQuest - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnQuest == 6 && tempScrollDiff > 0 && (cursorOnQuest + tempScrollDiff) < (questCount - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnQuest = 5;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (questListScrollBy * (tempScrollDiff + 1)), 0.0f);

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnQuest == 6 && tempScrollDiff > 0 && (cursorOnQuest + tempScrollDiff) == (questCount - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnQuest = 5;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (questCount != 0)
                {
                    if (cursorOnQuest == questCount)
                    {
                        cursorOnQuest = questCount - 1;
                    }

                    if (cursorOnQuest == 0 && tempScrollDiff == 0)
                    {
                        cursor.position = new Vector3(133f, 655f, 0);
                        questSelected = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/" + QuestOption + "QuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform.GetChild(0).GetComponent<QuestListButtonMouseEvents>();
                    }
                    else if (cursorOnQuest == 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(133f, 655f, 0);
                        questSelected = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/" + QuestOption + "QuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform.GetChild(tempScrollDiff).GetComponent<QuestListButtonMouseEvents>();
                    }
                    else if (cursorOnQuest > 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(133f, (655f - (questListSpacer * cursorOnQuest)), 0);
                        questSelected = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/" + QuestOption + "QuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform.GetChild(cursorOnQuest + tempScrollDiff).GetComponent<QuestListButtonMouseEvents>();
                    }
                    else
                    {
                        cursor.position = new Vector3(133f, (655f - (questListSpacer * cursorOnQuest)), 0);
                        questSelected = GameObject.Find("GameManager/Menus/QuestsMenuCanvas/" + QuestOption + "QuestsMenuPanel/QuestListPanel/QuestListScroller/QuestListSpacer").transform.GetChild(cursorOnQuest).GetComponent<QuestListButtonMouseEvents>();
                    }

                    questSelected.ShowQuestDetails();
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    questSelected.SetQuest();
                }
            }

            if (questMenuCursorState == QuestMenuCursorStates.OPTIONS)
            {
                if (!dpadPressed && cursorOnQuestOption == 0)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == 1) //right
                    {
                        cursorOnQuestOption = 1;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                } else if (!dpadPressed && cursorOnQuestOption == 1)
                {
                    if (Input.GetAxisRaw("DpadHorizontal") == -1) //left
                    {
                        cursorOnQuestOption = 0;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (!dpadPressed)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnQuest = 0;
                        questMenuCursorState = QuestMenuCursorStates.QUESTLIST;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;
                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    if (cursorOnQuestOption == 0)
                    {
                        ShowActiveQuestsPanel();
                    } else if (cursorOnQuestOption == 1)
                    {
                        ShowCompletedQuestsPanel();
                    }
                    cursorOnQuest = 0;
                    questMenuCursorState = QuestMenuCursorStates.QUESTLIST;
                }

                if (cursorOnQuestOption == 0)
                {
                    cursor.position = new Vector3(131f, 710f, 0);
                } else if (cursorOnQuestOption == 1)
                {
                    cursor.position = new Vector3(195f, 710f, 0);
                }
            }
        }
    }

    void UpdateBestiaryCursor()
    {
        if (mainCursorState == MainCursorStates.IDLE && bestiaryMenuCursorState != BestiaryMenuCursorStates.IDLE && !inAnimation)
        {
            int bestCount;
            bestCount = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform.childCount;

            if (bestiaryMenuCursorState == BestiaryMenuCursorStates.BESTIARYLIST)
            {
                int scrollDiff = bestCount - 9;

                if (bestCount <= 9)
                {
                    tempScrollDiff = 0;
                }

                RectTransform spacerScroll = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").GetComponent<RectTransform>();

                //Debug.Log(cursorOnItem);

                if (!dpadPressed && cursorOnBest == 0 && tempScrollDiff == 0)
                {
                    if (bestCount > 1)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1) //down
                        {
                            cursorOnBest = cursorOnBest + 1;
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        }
                    }
                }
                else if (!dpadPressed && cursorOnBest == 0 && tempScrollDiff > 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnBest = cursorOnBest + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (bestListScrollBy * (tempScrollDiff - 1)), 0.0f);

                        tempScrollDiff = tempScrollDiff - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnBest > 0 && cursorOnBest < 8)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnBest = cursorOnBest - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        cursorOnBest = cursorOnBest + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnBest == 8 && tempScrollDiff == 0 && scrollDiff > 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnBest = cursorOnBest - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, bestListScrollBy, 0.0f);

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnBest == 8 && tempScrollDiff == 0 && scrollDiff == 0)
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnBest = cursorOnBest - 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnBest == 8 && tempScrollDiff > 0 && (cursorOnBest + tempScrollDiff) < (bestCount - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnBest = 7;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1) //down
                    {
                        spacerScroll.anchoredPosition = new Vector3(0.0f, (bestListScrollBy * (tempScrollDiff + 1)), 0.0f);

                        tempScrollDiff = tempScrollDiff + 1;
                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }
                else if (!dpadPressed && cursorOnBest == 8 && tempScrollDiff > 0 && (cursorOnBest + tempScrollDiff) == (bestCount - 1))
                {
                    if (Input.GetAxisRaw("DpadVertical") == 1) //up
                    {
                        cursorOnBest = 7;

                        dpadPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    }
                }

                if (bestCount != 0)
                {
                    if (cursorOnQuest == bestCount)
                    {
                        cursorOnQuest = bestCount - 1;
                    }

                    if (cursorOnBest == 0 && tempScrollDiff == 0)
                    {
                        cursor.position = new Vector3(134f, 684.5f, 0);
                        bestSelected = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform.GetChild(0).GetComponent<BestiaryListButtonMouseEvents>();
                    }
                    else if (cursorOnBest == 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(134f, 684.5f, 0);
                        bestSelected = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform.GetChild(tempScrollDiff).GetComponent<BestiaryListButtonMouseEvents>();
                    }
                    else if (cursorOnBest > 0 && tempScrollDiff > 0)
                    {
                        cursor.position = new Vector3(134f, (684.5f - (bestListSpacer * cursorOnBest)), 0);
                        bestSelected = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform.GetChild(cursorOnBest + tempScrollDiff).GetComponent<BestiaryListButtonMouseEvents>();
                    }
                    else
                    {
                        cursor.position = new Vector3(134f, (684.5f - (bestListSpacer * cursorOnBest)), 0);
                        bestSelected = GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform.GetChild(cursorOnBest).GetComponent<BestiaryListButtonMouseEvents>();
                    }

                    bestSelected.ShowBestiaryEntryDetails();
                }

                if (Input.GetButtonDown("Confirm") && !confirmPressed)
                {
                    confirmPressed = true;

                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                    bestSelected.SetBestEntry();
                }
            }
        }
    }

    #endregion

    #region Main Menu

    /// <summary>
    /// Draws main menu, including hero stats (HP, MP, etc) as well as current location and gold.  Also resets heroToCheck to null
    /// </summary>
    void ShowMainMenu()
    {
        StartCoroutine(AnimateMainMenu());

        mainMenuCanvasGroup.alpha = 1;
        mainMenuCanvasGroup.blocksRaycasts = true;
        mainMenuCanvasGroup.interactable = true;
        DrawHeroStats();
        DisplayLocation();
        DisplayGold();
        heroToCheck = null;

        mainCursorState = MainCursorStates.ITEM;
        
        itemMenuCursorState = ItemMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Coroutine.  Animates the exit of the main menu, hides all main menu elements, returns camera to original position, and unpauses the background
    /// </summary>
    IEnumerator ExitMainMenu()
    {
        yield return (AnimateMainMenu());

        drawingGUI = false;
        mainMenuCanvasGroup.alpha = 0;
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;

        UnboldButton(MagicButton);
        UnboldButton(EquipButton);
        UnboldButton(StatusButton);
        UnboldButton(TalentsButton);
        GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero1Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero2Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero3Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero4Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero5Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
        heroToCheck = null;
        choosingHero = false;

        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        gameCameraObj.transform.parent = GameObject.Find("Player").transform;
        gameCameraObj.transform.position = currentCameraPosition;
        gameCamera.orthographicSize = currentCameraSize;

        //DeleteScreenshot();

        PauseBackground(false);
        menuCalled = false;
        menuState = MenuStates.IDLE;
        mainCursorState = MainCursorStates.IDLE;
    }

    /// <summary>
    /// Displays hero's stats for all active heroes into HeroInfoPanel
    /// </summary>
    void DrawHeroStats()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroPanels(heroCount);

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            DrawHeroFace(GameManager.instance.activeHeroes[i], GameObject.Find("MainMenuCanvas/MainMenuPanel/HeroInfoPanel").transform.GetChild(i).Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            GameObject.Find("MainMenuCanvas/MainMenuPanel/HeroInfoPanel").transform.GetChild(i).Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            GameObject.Find("MainMenuCanvas/MainMenuPanel/HeroInfoPanel").transform.GetChild(i).Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("MainMenuCanvas/MainMenuPanel/HeroInfoPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].finalMaxHP); //HP text
            GameObject.Find("MainMenuCanvas/MainMenuPanel/HeroInfoPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].finalMaxMP); //MP text
            GameObject.Find("MainMenuCanvas/MainMenuPanel/HeroInfoPanel").transform.GetChild(i).Find("EXPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].currentExp + " / " + HeroDB.instance.levelEXPThresholds[(GameManager.instance.activeHeroes[i].currentLevel - 1)]); //Exp text
            DrawHeroSpawnPoint(GameManager.instance.activeHeroes[i], GameObject.Find("MainMenuCanvas/MainMenuPanel/HeroInfoPanel").transform.GetChild(i).Find("GridPanel").gameObject);
        }
    }

    /// <summary>
    /// Shows canvas for each active hero, and hides canvas for inactive heroes
    /// </summary>
    /// <param name="count">Number of active heroes</param>
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

    /// <summary>
    /// Draws HP and MP bars for all active heroes
    /// </summary>
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

    /// <summary>
    /// Draws spawn point in GridPanel for given hero
    /// </summary>
    /// <param name="hero">Hero to obtain spawn point from</param>
    /// <param name="panel">Panel to draw the spawn point</param>
    void DrawHeroSpawnPoint(BaseHero hero, GameObject panel)
    {
        foreach (Transform child in panel.transform)
        {
            child.gameObject.GetComponent<Image>().color = Color.white;
        }

        string spawnPoint = hero.spawnPoint;
        panel.gameObject.transform.Find("Grid - " + spawnPoint).GetComponent<Image>().color = Color.black;
    }

    /// <summary>
    /// Displays sector and city in Location Panel - Sector = Scene name; City = customized in 'DisplayCity' (will likely be updated)
    /// </summary>
    void DisplayLocation()
    {
        string sector = SceneManager.GetActiveScene().name; //sector is the scene name
        DisplayCity(sector);
        DisplaySector(sector); //city is which area the sector is located in
    }

    /// <summary>
    /// Draws sector in the Location Panel
    /// </summary>
    /// <param name="sector">Name of the sector to be drawn</param>
    void DisplaySector(string sector)
    {
        GameObject.Find("LocationPanel").transform.GetChild(1).GetComponent<Text>().text = sector;
    }

    /// <summary>
    /// Displays city based on sector - city is customized in this method
    /// </summary>
    /// <param name="sector">Based on the scene name</param>
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

    /// <summary>
    /// Draws gold value to Gold Panel
    /// </summary>
    void DisplayGold()
    {
        GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/TimeGoldPanel").transform.Find("GoldText").GetComponent<Text>().text = GameManager.instance.gold.ToString();
    }

    /// <summary>
    /// Gets hour/minute/second values from GameManager.instance and draws them to the menu
    /// </summary>
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

            GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/TimeGoldPanel").transform.Find("TimeText").GetComponent<Text>().text = hours + ":" + minutes + ":" + seconds;
        }
    }

    #endregion

    #region Item Menu

    /// <summary>
    /// Public method to call item menu from gui
    /// </summary>
    public void ShowItemMenu()
    {
        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        StartCoroutine(DrawItemMenu());

        mainCursorState = MainCursorStates.IDLE;
        itemMenuCursorState = ItemMenuCursorStates.USEITEMS;
        cursorOnItem = 0;
        cursorOnItemHero = 0;
        itemMode = "Use";
    }

    /// <summary>
    /// Coroutine. Changes menu state to ITEM and draws elements for item menu
    /// </summary>
    IEnumerator DrawItemMenu()
    {
        yield return (AnimateMainMenu());

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(ItemMenuCanvas);
        menuState = MenuStates.ITEM;
        EraseItemDescText();
        DrawHeroItemMenuStats();
        DrawItemList();

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/UseButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/KeyItemsButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

        //HideArrangeMenu();
        itemCustomizeModeOn = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/ArrangeButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

        yield return (AnimateItemMenu());
    }

    /// <summary>
    /// Coroutine. Hides item menu and goes back to main menu
    /// </summary>
    IEnumerator HideItemMenu()
    {
        yield return (AnimateItemMenu());

        ResetItemList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(ItemMenuCanvas);
        menuState = MenuStates.MAIN;
        heroToCheck = null;
        itemCustomizeModeOn = false;

        ShowMainMenu();
        
        mainCursorState = MainCursorStates.ITEM;
        itemMenuCursorState = ItemMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Sets the item description panel to blank ("")
    /// </summary>
    void EraseItemDescText()
    {
        GameObject.Find("ItemMenuCanvas/ItemMenuPanel/ItemDescriptionPanel/ItemDescriptionText").GetComponent<Text>().text = "";
    }

    /// <summary>
    /// Draws items in inventory to item list
    /// </summary>
    public void DrawItemList()
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

                for (int i = 0; i < Inventory.instance.items.Count; i++)
                {
                    if (Inventory.instance.items[i] == item)
                    {
                        itemCount++;
                    }
                }

                NewItemPanel = Instantiate(PrefabManager.Instance.itemPrefab);
                NewItemPanel.transform.GetChild(0).GetComponent<Text>().text = item.name;
                NewItemPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
                NewItemPanel.transform.GetChild(2).GetComponent<Text>().text = itemCount.ToString();
                NewItemPanel.transform.SetParent(KeyItemListSpacer, false);
                itemsAccountedFor.Add(item);
            }
        }

        itemsAccountedFor.Clear();
    }

    /// <summary>
    /// Clears the item and key item list
    /// </summary>
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

    /// <summary>
    /// Draws hero stats for all active heroes in item menu
    /// </summary>
    public void DrawHeroItemMenuStats()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroItemMenuPanels();

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            DrawHeroFace(GameManager.instance.activeHeroes[i], GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].finalMaxHP); //HP text
            GameObject.Find("ItemMenuCanvas/ItemMenuPanel/HeroItemPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].finalMaxMP); //MP text
        }
    }

    /// <summary>
    /// Shows item menu panel for each active hero, and hides item menu panel for inactive heroes
    /// </summary>
    void DrawHeroItemMenuPanels()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;

        if (heroCount == 1)
        {
            DisplayCanvas(Hero1ItemPanel);
            HideCanvas(Hero2ItemPanel);
            HideCanvas(Hero3ItemPanel);
            HideCanvas(Hero4ItemPanel);
            HideCanvas(Hero5ItemPanel);
        }
        else if (heroCount == 2)
        {
            DisplayCanvas(Hero1ItemPanel);
            DisplayCanvas(Hero2ItemPanel);
            HideCanvas(Hero3ItemPanel);
            HideCanvas(Hero4ItemPanel);
            HideCanvas(Hero5ItemPanel);
        }
        else if (heroCount == 3)
        {
            DisplayCanvas(Hero1ItemPanel);
            DisplayCanvas(Hero2ItemPanel);
            DisplayCanvas(Hero3ItemPanel);
            HideCanvas(Hero4ItemPanel);
            HideCanvas(Hero5ItemPanel);
        }
        else if (heroCount == 4)
        {
            DisplayCanvas(Hero1ItemPanel);
            DisplayCanvas(Hero2ItemPanel);
            DisplayCanvas(Hero3ItemPanel);
            DisplayCanvas(Hero4ItemPanel);
            HideCanvas(Hero5ItemPanel);
        }
        else if (heroCount == 5)
        {
            DisplayCanvas(Hero1ItemPanel);
            DisplayCanvas(Hero2ItemPanel);
            DisplayCanvas(Hero3ItemPanel);
            DisplayCanvas(Hero4ItemPanel);
            DisplayCanvas(Hero5ItemPanel);
        }

        DrawHeroItemMenuPanelBars();
    }

    /// <summary>
    /// Draws HP and MP bars in item menu for all active heroes
    /// </summary>
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

    /// <summary>
    /// Displays item use menu
    /// </summary>
    public void ShowUseItemMenu()
    {
        PlaySE(AudioManager.instance.confirmSE);

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/UseButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/KeyItemsButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/ArrangeButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
    }

    /// <summary>
    /// Displays key item menu
    /// </summary>
    public void ShowKeyItemMenu()
    {
        PlaySE(AudioManager.instance.confirmSE);

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/KeyItemListPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/UseButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/KeyItemsButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/ArrangeButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
    }

    /// <summary>
    /// Displays Arrangement menu (when organizing items)
    /// </summary>
    public void ShowArrangeMenu()
    {
        StartCoroutine(DrawArrangeMenu());
    }

    /// <summary>
    /// Coroutine. Displays Arrangement menu (when organizing items)
    /// </summary>
    public IEnumerator DrawArrangeMenu()
    {
        if (!inArrangeMenu)
        {
            PlaySE(AudioManager.instance.confirmSE);
            inArrangeMenu = true;

            GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

            yield return AnimateArrangeOptionsPanel();
        }
    }

    /// <summary>
    /// Coroutine. Hides Arrangement menu
    /// </summary>
    IEnumerator HideArrangeMenu()
    {
        inArrangeMenu = false;

        yield return AnimateArrangeOptionsPanel();

        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ArrangeOptionsPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// Processes the sorting for manual arrange mode and processes item sort if given mode is other than 'Customize'
    /// </summary>
    /// <param name="mode">'Customize' is for manually organizing items, other options are "Field", "Battle", "Type", "Name", "Most", and "Least"</param>
    public void ItemArrange(string mode)
    {
        if (mode == "Customize")
        {
            itemCustomizeModeOn = true;
            GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/ArrangeButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
            GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/UseButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
            GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/KeyItemsButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

            PlaySE(AudioManager.instance.confirmSE);
        } else
        {
            SortItems(mode);
            DrawItemList();
        }

        StartCoroutine(HideArrangeMenu());
        arrangeOption = -1;
    }

    /// <summary>
    /// Processes the sorting for item arrangement
    /// </summary>
    /// <param name="mode">Options are: "Field", "Battle", "Type", "Name", "Most", and "Least"</param>
    void SortItems(string option)
    {
        CancelCustomizeMode();

        PlaySE(AudioManager.instance.confirmSE);

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

    /// <summary>
    /// Turns off customize mode so items can be used
    /// </summary>
    void CancelCustomizeMode()
    {
        GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemOptionsPanel/ArrangeButton").GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
        itemCustomizeModeOn = false;
    }

    /// <summary>
    /// Cancels the hero choosing menu after clicking an item
    /// </summary>
    void CancelItemChoosingHero()
    {
        PlaySE(AudioManager.instance.backSE);

        foreach (Transform child in GameObject.Find("GameManager/Menus/ItemMenuCanvas/ItemMenuPanel/ItemListPanel/ItemScroller/ItemListSpacer").transform)
        {
            child.Find("NewItemNameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        itemChoosingHero = false;
    }

    #endregion

    #region Magic Menu

    /// <summary>
    /// Public method to call magic menu from gui
    /// </summary>
    public void ShowMagicMenu()
    {
        mainCursorState = MainCursorStates.HEROSELECT1;

        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        PlaySE(AudioManager.instance.confirmSE);

        menuToDraw = StartCoroutine(ChooseHeroForMagicMenu());
    }

    /// <summary>
    /// Coroutine. Facilitates drawing magic menu with chosen hero
    /// </summary>
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

        StartCoroutine(DrawMagicMenu());
    }

    /// <summary>
    /// Coroutine. Changes menu state to MAGIC and draws magic menu stats and attacks with chosen hero
    /// </summary>
    IEnumerator DrawMagicMenu()
    {
        yield return (AnimateMainMenu());

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(MagicMenuCanvas);
        DisplayCanvas(WhiteMagicListPanel);

        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDescriptionPanel/MagicDescriptionText").GetComponent<Text>().text = "";
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel/CooldownText").GetComponent<Text>().text = "-";
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicDetailsPanel/MPCostText").GetComponent<Text>().text = "-";

        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/WhiteMagicButton/WhiteMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/BlackMagicButton/BlackMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/SorceryMagicButton/SorceryMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        DrawMagicMenuStats();
        DrawMagicListPanels();
        menuState = MenuStates.MAGIC;

        mainCursorState = MainCursorStates.IDLE;
        magicMenuCursorState = MagicMenuCursorStates.USEMAGIC;
        cursorOnMagic = 0;

        yield return (AnimateMagicMenu());
    }

    /// <summary>
    /// Coroutine. Hides magic menu to go back to main menu
    /// </summary>
    IEnumerator HideMagicMenu()
    {
        yield return (AnimateMagicMenu());

        ResetMagicList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(MagicMenuCanvas);
        HideCanvas(WhiteMagicListPanel);
        HideCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);
        menuState = MenuStates.MAIN;
        heroToCheck = null;

        ShowMainMenu();
        mainCursorState = MainCursorStates.MAGIC;
        magicMenuCursorState = MagicMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Facilitates drawing of chosen hero's attacks
    /// </summary>
    void DrawMagicListPanels()
    {
        foreach (BaseAttack magicAttack in heroToCheck.MagicAttacks)
        {
            NewMagicPanel = Instantiate(PrefabManager.Instance.magicPrefab);
            NewMagicPanel.transform.Find("Name").GetComponent<Text>().text = magicAttack.name;
            NewMagicPanel.transform.Find("Icon").GetComponent<Image>().sprite = magicAttack.icon;

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
            if (heroToCheck.curMP < magicAttack.MPCost)
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

    /// <summary>
    /// Public method to display white magic panel after clicking 'White Magic' button in gui
    /// </summary>
    public void ShowWhiteMagicListPanel()
    {
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/WhiteMagicButton/WhiteMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/BlackMagicButton/BlackMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/SorceryMagicButton/SorceryMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        DisplayCanvas(WhiteMagicListPanel);
        HideCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);

        PlaySE(AudioManager.instance.confirmSE);
    }

    /// <summary>
    /// Public method to display white magic panel after clicking 'Black Magic' button in gui
    /// </summary>
    public void ShowBlackMagicListPanel()
    {
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/WhiteMagicButton/WhiteMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/BlackMagicButton/BlackMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/SorceryMagicButton/SorceryMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        HideCanvas(WhiteMagicListPanel);
        DisplayCanvas(BlackMagicListPanel);
        HideCanvas(SorceryMagicListPanel);

        PlaySE(AudioManager.instance.confirmSE);
    }

    /// <summary>
    /// Public method to display white magic panel after clicking 'Sorcery Magic' button in gui
    /// </summary>
    public void ShowSorceryMagicListPanel()
    {
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/WhiteMagicButton/WhiteMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/BlackMagicButton/BlackMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        GameObject.Find("MagicMenuCanvas/MagicMenuPanel/MagicOptionsPanel/SorceryMagicButton/SorceryMagicButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;

        HideCanvas(WhiteMagicListPanel);
        HideCanvas(BlackMagicListPanel);
        DisplayCanvas(SorceryMagicListPanel);

        PlaySE(AudioManager.instance.confirmSE);
    }

    /// <summary>
    /// Draws hero stats for chosen hero for magic menu 
    /// </summary>
    public void DrawMagicMenuStats()
    {
        DrawHeroFace(heroToCheck, GameObject.Find("HeroMagicPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("HeroMagicPanel/NameText").GetComponent<Text>().text = heroToCheck.name; //Name text
        GameObject.Find("HeroMagicPanel/LevelText").GetComponent<Text>().text = heroToCheck.currentLevel.ToString(); //Level text
        GameObject.Find("HeroMagicPanel/HPText").GetComponent<Text>().text = (heroToCheck.curHP.ToString() + " / " + heroToCheck.finalMaxHP.ToString()); //HP text
        GameObject.Find("HeroMagicPanel/MPText").GetComponent<Text>().text = (heroToCheck.curMP.ToString() + " / " + heroToCheck.finalMaxMP.ToString()); //MP text

        MagicPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(heroToCheck), 0, 1), MagicPanelHPProgressBar.transform.localScale.y);
        MagicPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(heroToCheck), 0, 1), MagicPanelMPProgressBar.transform.localScale.y);
    }

    /// <summary>
    /// Clears magic attacks in gui
    /// </summary>
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

    /// <summary>
    /// Cancels the hero choosing menu after clicking a magic attack to cast on a hero
    /// </summary>
    IEnumerator CancelMagicChoosingHero()
    {
        yield return AnimateMagicHeroSelectPanel();
        PlaySE(AudioManager.instance.backSE);

        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero1SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero2SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero3SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero4SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();
        GameObject.Find("GameManager/Menus/MagicMenuCanvas/MagicMenuPanel/HeroSelectMagicPanel/Hero5SelectMagicPanel").GetComponent<MagicMenuMouseEvents>().HideBorder();

        HideCanvas(HeroSelectMagicPanel);

        choosingHeroForMagicMenu = false;
    }

    #endregion

    #region Equip Menu

    /// <summary>
    /// Public method to call Equip Menu from gui
    /// </summary>
    public void ShowEquipMenu()
    {
        mainCursorState = MainCursorStates.HEROSELECT1;

        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        PlaySE(AudioManager.instance.confirmSE);

        Debug.Log("Equip button clicked - choose a hero");
        menuToDraw = StartCoroutine(ChooseHeroForEquipMenu());
    }

    /// <summary>
    /// Coroutine. Facilitates drawing Equip menu for chosen hero
    /// </summary>
    IEnumerator ChooseHeroForEquipMenu()
    {
        choosingHero = true;
        BoldButton(EquipButton);
        while (heroToCheck == null)
        {
            GetHeroClicked();
            yield return null;
        }
        UnboldButton(EquipButton);
        StartCoroutine(DrawEquipMenu());
    }

    /// <summary>
    /// Coroutine. Changes menu state to EQUIP and draws Equip menu
    /// </summary>
    IEnumerator DrawEquipMenu()
    {
        yield return AnimateMainMenu();

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(EquipMenuCanvas);
        GameObject.Find("EquipMenuCanvas/EquipMenuPanel/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>().text = "";

        equipMode = "Equip";
        GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipOptionsPanel/EquipOptionButton/EquipButtonText").GetComponent<Text>().fontStyle = FontStyle.Bold;
        GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipOptionsPanel/RemoveOptionButton/RemoveButtonText").GetComponent<Text>().fontStyle = FontStyle.Normal;

        heroToCheck.UpdateStats();

        UpdateEquipmentArrowsToNeutral();
        DrawEquipMenuStats();
        DrawCurrentEquipment();
        menuState = MenuStates.EQUIP;

        mainCursorState = MainCursorStates.IDLE;
        equipMenuCursorState = EquipMenuCursorStates.EQUIPSLOTS;
        cursorOnEquipSlot = 0;

        yield return AnimateEquipMenu();
    }

    /// <summary>
    /// Draw equipment name and icon into gui equipment slots for chosen hero
    /// </summary>
    void DrawCurrentEquipment()
    {
        string baseEquipPath = "GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipSlotsPanel/";

        if (heroToCheck.equipment[0] == null)
        {
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotText").GetComponent<Text>().text = heroToCheck.equipment[0].name;
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().sprite = heroToCheck.equipment[0].icon;

            Color temp = GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "HeadSlot/HeadButton/HeadSlotIcon").GetComponent<Image>().color = temp;
        }
        if (heroToCheck.equipment[1] == null)
        {
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotText").GetComponent<Text>().text = heroToCheck.equipment[1].name;
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().sprite = heroToCheck.equipment[1].icon;

            Color temp = GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "ChestSlot/ChestButton/ChestSlotIcon").GetComponent<Image>().color = temp;
        }
        if (heroToCheck.equipment[2] == null)
        {
            GameObject.Find(baseEquipPath + "HandsSlot/HandsButton/HandsSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "HandsSlot/HandsButton/HandsSlotText").GetComponent<Text>().text = heroToCheck.equipment[2].name;
            GameObject.Find(baseEquipPath + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().sprite = heroToCheck.equipment[2].icon;

            Color temp = GameObject.Find(baseEquipPath + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color = temp;
        }
        if (heroToCheck.equipment[3] == null)
        {
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotText").GetComponent<Text>().text = heroToCheck.equipment[3].name;
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().sprite = heroToCheck.equipment[3].icon;

            Color temp = GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "LegsSlot/LegsButton/LegsSlotIcon").GetComponent<Image>().color = temp;
        }
        if (heroToCheck.equipment[4] == null)
        {
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotText").GetComponent<Text>().text = heroToCheck.equipment[4].name;
            GameObject.Find(baseEquipPath + "FeetSlot/FeetButton/FeetSlotIcon").GetComponent<Image>().sprite = heroToCheck.equipment[4].icon;
        }
        if (heroToCheck.equipment[5] == null)
        {
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotText").GetComponent<Text>().text = heroToCheck.equipment[5].name;
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().sprite = heroToCheck.equipment[5].icon;

            Color temp = GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "RelicSlot/RelicButton/RelicSlotIcon").GetComponent<Image>().color = temp;
        }
        if (heroToCheck.equipment[6] == null)
        {
            GameObject.Find(baseEquipPath + "RightArmSlot/RightArmButton/RightArmSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color = temp;
        } else
        {
            GameObject.Find(baseEquipPath + "RightArmSlot/RightArmButton/RightArmSlotText").GetComponent<Text>().text = heroToCheck.equipment[6].name;
            GameObject.Find(baseEquipPath + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().sprite = heroToCheck.equipment[6].icon;

            Color temp = GameObject.Find(baseEquipPath + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color = temp;
        }
        if (heroToCheck.equipment[7] == null)
        {
            GameObject.Find(baseEquipPath + "LeftArmSlot/LeftArmButton/LeftArmSlotText").GetComponent<Text>().text = "";
            GameObject.Find(baseEquipPath + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().sprite = null;

            Color temp = GameObject.Find(baseEquipPath + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color;
            temp.a = 0f;
            GameObject.Find(baseEquipPath + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color = temp;
        }
        else
        {
            GameObject.Find(baseEquipPath + "LeftArmSlot/LeftArmButton/LeftArmSlotText").GetComponent<Text>().text = heroToCheck.equipment[7].name;
            GameObject.Find(baseEquipPath + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().sprite = heroToCheck.equipment[7].icon;

            Color temp = GameObject.Find(baseEquipPath + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color;
            temp.a = 1f;
            GameObject.Find(baseEquipPath + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color = temp;
        }

    }

    /// <summary>
    /// Draws chosen hero's stats after current equipment is taken into account
    /// </summary>
    public void DrawEquipMenuStats()
    {
        //For HeroEquipPanel
        DrawHeroFace(heroToCheck, GameObject.Find("HeroEquipPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("HeroEquipPanel/NameText").GetComponent<Text>().text = heroToCheck.name; //Name text
        GameObject.Find("HeroEquipPanel/LevelText").GetComponent<Text>().text = heroToCheck.currentLevel.ToString(); //Level text
        GameObject.Find("HeroEquipPanel/HPText").GetComponent<Text>().text = (heroToCheck.curHP.ToString() + " / " + heroToCheck.finalMaxHP.ToString()); //HP text
        GameObject.Find("HeroEquipPanel/MPText").GetComponent<Text>().text = (heroToCheck.curMP.ToString() + " / " + heroToCheck.finalMaxMP.ToString()); //MP text

        EquipPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(heroToCheck), 0, 1), EquipPanelHPProgressBar.transform.localScale.y);
        EquipPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(heroToCheck), 0, 1), EquipPanelMPProgressBar.transform.localScale.y);

        //For EquipStatsPanel
        //STR-SPI
        GameObject.Find("EquipStatsPanel/BaseStrengthText").GetComponent<Text>().text = heroToCheck.finalStrength.ToString();
        GameObject.Find("EquipStatsPanel/BaseStaminaText").GetComponent<Text>().text = heroToCheck.finalStamina.ToString();
        GameObject.Find("EquipStatsPanel/BaseAgilityText").GetComponent<Text>().text = heroToCheck.finalAgility.ToString();
        GameObject.Find("EquipStatsPanel/BaseDexterityText").GetComponent<Text>().text = heroToCheck.finalDexterity.ToString();
        GameObject.Find("EquipStatsPanel/BaseIntelligenceText").GetComponent<Text>().text = heroToCheck.finalIntelligence.ToString();
        GameObject.Find("EquipStatsPanel/BaseSpiritText").GetComponent<Text>().text = heroToCheck.finalSpirit.ToString();

        GameObject.Find("EquipStatsPanel/NewStrengthText").GetComponent<Text>().text = heroToCheck.finalStrength.ToString();
        GameObject.Find("EquipStatsPanel/NewStaminaText").GetComponent<Text>().text = heroToCheck.finalStamina.ToString();
        GameObject.Find("EquipStatsPanel/NewAgilityText").GetComponent<Text>().text = heroToCheck.finalAgility.ToString();
        GameObject.Find("EquipStatsPanel/NewDexterityText").GetComponent<Text>().text = heroToCheck.finalDexterity.ToString();
        GameObject.Find("EquipStatsPanel/NewIntelligenceText").GetComponent<Text>().text = heroToCheck.finalIntelligence.ToString();
        GameObject.Find("EquipStatsPanel/NewSpiritText").GetComponent<Text>().text = heroToCheck.finalSpirit.ToString();

        //HP & MP
        GameObject.Find("EquipStatsPanel/BaseHPText").GetComponent<Text>().text = heroToCheck.finalMaxHP.ToString();
        GameObject.Find("EquipStatsPanel/BaseMPText").GetComponent<Text>().text = heroToCheck.finalMaxMP.ToString();

        GameObject.Find("EquipStatsPanel/NewHPText").GetComponent<Text>().text = heroToCheck.finalMaxHP.ToString();
        GameObject.Find("EquipStatsPanel/NewMPText").GetComponent<Text>().text = heroToCheck.finalMaxMP.ToString();

        //ATK-MDEF
        GameObject.Find("EquipStatsPanel/BaseAttackText").GetComponent<Text>().text = heroToCheck.finalATK.ToString();
        GameObject.Find("EquipStatsPanel/BaseMagicAttackText").GetComponent<Text>().text = heroToCheck.finalMATK.ToString();
        GameObject.Find("EquipStatsPanel/BaseDefenseText").GetComponent<Text>().text = heroToCheck.finalDEF.ToString();
        GameObject.Find("EquipStatsPanel/BaseMagicDefenseText").GetComponent<Text>().text = heroToCheck.finalMDEF.ToString();

        GameObject.Find("EquipStatsPanel/NewAttackText").GetComponent<Text>().text = heroToCheck.finalATK.ToString();
        GameObject.Find("EquipStatsPanel/NewMagicAttackText").GetComponent<Text>().text = heroToCheck.finalMATK.ToString();
        GameObject.Find("EquipStatsPanel/NewDefenseText").GetComponent<Text>().text = heroToCheck.finalDEF.ToString();
        GameObject.Find("EquipStatsPanel/NewMagicDefenseText").GetComponent<Text>().text = heroToCheck.finalMDEF.ToString();

        //Other stats
        GameObject.Find("EquipStatsPanel/BaseHitText").GetComponent<Text>().text = heroToCheck.finalHitRating.ToString();
        GameObject.Find("EquipStatsPanel/BaseCritText").GetComponent<Text>().text = heroToCheck.finalCritRating.ToString();
        GameObject.Find("EquipStatsPanel/BaseMPRegenText").GetComponent<Text>().text = heroToCheck.finalRegenRating.ToString();
        GameObject.Find("EquipStatsPanel/BaseMoveText").GetComponent<Text>().text = heroToCheck.finalMoveRating.ToString();
        GameObject.Find("EquipStatsPanel/BaseDodgeText").GetComponent<Text>().text = heroToCheck.finalDodgeRating.ToString();
        GameObject.Find("EquipStatsPanel/BaseBlockText").GetComponent<Text>().text = heroToCheck.finalBlockRating.ToString();
        GameObject.Find("EquipStatsPanel/BaseParryText").GetComponent<Text>().text = heroToCheck.finalParryRating.ToString();
        GameObject.Find("EquipStatsPanel/BaseThreatText").GetComponent<Text>().text = heroToCheck.finalThreatRating.ToString();

        GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = heroToCheck.finalHitRating.ToString();
        GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = heroToCheck.finalCritRating.ToString();
        GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = heroToCheck.finalRegenRating.ToString();
        GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = heroToCheck.finalMoveRating.ToString();
        GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = heroToCheck.finalDodgeRating.ToString();
        GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = heroToCheck.finalBlockRating.ToString();
        GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = heroToCheck.finalParryRating.ToString();
        GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = heroToCheck.finalThreatRating.ToString();
    }

    /// <summary>
    /// Hides all arrows for corresponding stat, and displays the one given in 'arrow' parameter
    /// </summary>
    /// <param name="obj">Given stat arrow parent</param>
    /// <param name="arrow">"Up", "Down", or "Neutral"</param>
    void ChangeArrow(GameObject obj, string arrow)
    {
        foreach (Image arrowImage in obj.GetComponentsInChildren<Image>())
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

    /// <summary>
    /// Hides Equip menu and changes state back to main menu
    /// </summary>
    IEnumerator HideEquipMenu()
    {
        yield return (AnimateEquipMenu());

        DisplayCanvas(MainMenuCanvas);
        HideCanvas(EquipMenuCanvas);
        menuState = MenuStates.MAIN;
        heroToCheck = null;

        ShowMainMenu();
        mainCursorState = MainCursorStates.EQUIP;
        equipMenuCursorState = EquipMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Public method for listing equipment when clicking equipment slot
    /// </summary>
    public void ListEquipment()
    {
        if (equipMode == "Equip")
        {
            PlaySE(AudioManager.instance.confirmSE);

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

            if (equipButtonClicked == "HeadButton")
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

            if (equipButtonClicked == "HandsButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "HANDS")
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

            if (equipButtonClicked == "ChestButton")
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

            if (equipButtonClicked == "LegsButton")
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

            if (equipButtonClicked == "FeetButton")
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

            if (equipButtonClicked == "RelicButton")
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

            if (equipButtonClicked == "RightArmButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "RIGHTARM")
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

            if (equipButtonClicked == "LeftArmButton")
            {
                foreach (Equipment equip in equipmentList)
                {
                    if (equip.equipmentSlot.ToString() == "LEFTARM")
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
            if (equipButtonClicked == "HandsButton")
            {
                heroToCheck.Unequip(2);

                GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color = temp;
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
            if (equipButtonClicked == "RightArmButton")
            {
                heroToCheck.Unequip(6);

                GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "LeftArmButton")
            {
                heroToCheck.Unequip(7);

                GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotText").GetComponent<Text>().text = "";
                GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().sprite = null;

                Color temp = GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color;
                temp.a = 0f;
                GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color = temp;
            }

            heroToCheck.GetCurrentStatsFromEquipment();
            heroToCheck.UpdateStatsFromTalents();

            DrawEquipMenuStats();

            UpdateEquipmentArrowsToNeutral();

            AudioManager.instance.PlaySE(AudioManager.instance.equipSE);
        }
    }

    /// <summary>
    /// Public method to unequip all currently equipped items
    /// </summary>
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

        GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color = temp;

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

        GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.Unequip(7);

        GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotText").GetComponent<Text>().text = "";
        GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().sprite = null;

        temp = GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color;
        temp.a = 0f;
        GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color = temp;

        heroToCheck.GetCurrentStatsFromEquipment();
        heroToCheck.UpdateStatsFromTalents();

        UpdateEquipmentArrowsToNeutral();

        DrawEquipMenuStats();

        inEquipList = false;

        AudioManager.instance.PlaySE(AudioManager.instance.equipSE);
    }

    /// <summary>
    /// Public method to change equipment from choosing a given equipment item in gui
    /// </summary>
    /// <param name="toEquip">Equipment to be equipped.  If equip is null, equipment is unequipped</param>
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
                if (equipButtonClicked == "HandsButton")
                {
                    heroToCheck.Unequip(2);

                    GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color = temp;
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
                if (equipButtonClicked == "RightArmButton")
                {
                    heroToCheck.Unequip(6);

                    GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color = temp;
                }
                if (equipButtonClicked == "LeftArmButton")
                {
                    heroToCheck.Unequip(7);

                    GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotText").GetComponent<Text>().text = "";
                    GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().sprite = null;

                    Color temp = GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color;
                    temp.a = 0f;
                    GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color = temp;
                }

                UpdateEquipmentArrowsToNeutral();

                heroToCheck.GetCurrentStatsFromEquipment();

                heroToCheck.UpdateStatsFromTalents();

                DrawEquipMenuStats();

                inEquipList = false;

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
            if (equipButtonClicked == "HandsButton")
            {
                GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "HandsSlot/HandsButton/HandsSlotIcon").GetComponent<Image>().color = temp;
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
            if (equipButtonClicked == "LeftArmButton")
            {
                GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "LeftArmSlot/LeftArmButton/LeftArmSlotIcon").GetComponent<Image>().color = temp;
            }
            if (equipButtonClicked == "RightArmButton")
            {
                GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotText").GetComponent<Text>().text = toEquip.name;
                GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().sprite = toEquip.icon;
                heroToCheck.Equip(toEquip);

                Color temp = GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color;
                temp.a = 1f;
                GameObject.Find(equipMenuBase + "RightArmSlot/RightArmButton/RightArmSlotIcon").GetComponent<Image>().color = temp;
            }

            foreach (Transform child in GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform)
            {
                Destroy(child.gameObject);
            }

            GameObject.Find("EquipMenuCanvas/EquipMenuPanel/EquipDescriptionPanel/EquipDescriptionText").GetComponent<Text>().text = "";

            UpdateEquipmentArrowsToNeutral();

            heroToCheck.GetCurrentStatsFromEquipment();

            heroToCheck.UpdateStatsFromTalents();

            DrawEquipMenuStats();

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

    /// <summary>
    /// Changes all arrows to neutral
    /// </summary>
    void UpdateEquipmentArrowsToNeutral()
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

    /// <summary>
    /// Returns equipment that is currently equipped in clicked slot from gui
    /// </summary>
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

        if (equipButtonClicked == "HandsButton")
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

        if (equipButtonClicked == "RightArmButton")
        {
            return heroToCheck.equipment[6];
        }

        if (equipButtonClicked == "LeftArmButton")
        {
            return heroToCheck.equipment[7];
        }

        return null;
    }

    /// <summary>
    /// Returns index of equipment button clicked
    /// </summary>
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

        if (equipButtonClicked == "HandsButton")
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

        if (equipButtonClicked == "RightArmButton")
        {
            return 6;
        }

        if (equipButtonClicked == "LeftArmButton")
        {
            return 7;
        }

        Debug.LogWarning("Illegal slot index, returning 0");
        return 0;
    }

    /// <summary>
    /// Returns a temporary hero with same stats as chosen hero for equip menu
    /// </summary>
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

    /// <summary>
    /// Returns a temporary equip with same stats as current equipment
    /// </summary>
    /// <param name="toEquip">Current equipment to be copied</param>
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

    /// <summary>
    /// Public method to show potential stat changes with equipment chosen compared to temporary hero (copied from chosen hero) 
    /// and temporary equipment (copied from current equipment)
    /// </summary>
    /// <param name="toEquip">Potential equipment that has cursor hovered over</param>
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

        tempHero.UpdateStats();

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


        GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = tempHero.finalHitRating.ToString();
        if (tempHero.finalHitRating > heroToCheck.finalHitRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Up");
        }
        else if (tempHero.finalHitRating < heroToCheck.finalHitRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Down");
        }
        else if (tempHero.finalHitRating == heroToCheck.finalHitRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = tempHero.finalCritRating.ToString();
        if (tempHero.finalCritRating > heroToCheck.finalCritRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Up");
        }
        else if (tempHero.finalCritRating < heroToCheck.finalCritRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Down");
        }
        else if (tempHero.finalCritRating == heroToCheck.finalCritRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = tempHero.finalMoveRating.ToString();
        if (tempHero.finalMoveRating > heroToCheck.finalMoveRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Up");
        }
        else if (tempHero.finalMoveRating < heroToCheck.finalMoveRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Down");
        }
        else if (tempHero.finalMoveRating == heroToCheck.finalMoveRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = tempHero.finalRegenRating.ToString();
        if (tempHero.finalRegenRating > heroToCheck.finalRegenRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Up");
        }
        else if (tempHero.finalRegenRating < heroToCheck.finalRegenRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Down");
        }
        else if (tempHero.finalRegenRating == heroToCheck.finalRegenRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = tempHero.finalDodgeRating.ToString();
        if (tempHero.finalDodgeRating > heroToCheck.finalDodgeRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Up");
        }
        else if (tempHero.finalDodgeRating < heroToCheck.finalDodgeRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Down");
        }
        else if (tempHero.finalDodgeRating == heroToCheck.finalDodgeRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = tempHero.finalBlockRating.ToString();
        if (tempHero.finalBlockRating > heroToCheck.finalBlockRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Up");
        }
        else if (tempHero.finalBlockRating < heroToCheck.finalBlockRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Down");
        }
        else if (tempHero.finalBlockRating == heroToCheck.finalBlockRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = tempHero.finalParryRating.ToString();
        if (tempHero.finalParryRating > heroToCheck.finalParryRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Up");
        }
        else if (tempHero.finalParryRating < heroToCheck.finalParryRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Down");
        }
        else if (tempHero.finalParryRating == heroToCheck.finalParryRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
        }

        GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = tempHero.finalThreatRating.ToString();
        if (tempHero.finalThreatRating > heroToCheck.finalThreatRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Up");
        }
        else if (tempHero.finalThreatRating < heroToCheck.finalThreatRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Down");
        }
        else if (tempHero.finalThreatRating == heroToCheck.finalThreatRating)
        {
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
        }
    }

    /// <summary>
    /// Public method to changes all arrows to neutral, and updates equip menu gui with newly updated stats after equipment is equipped
    /// </summary>
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

            GameObject.Find("EquipStatsPanel/NewHitText").GetComponent<Text>().text = heroToCheck.finalHitRating.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/HitArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewCritText").GetComponent<Text>().text = heroToCheck.finalCritRating.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/CritArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewMoveText").GetComponent<Text>().text = heroToCheck.finalMoveRating.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/MoveArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewMPRegenText").GetComponent<Text>().text = heroToCheck.finalRegenRating.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/MPRegenArrow"), "Neutral");

            GameObject.Find("EquipStatsPanel/NewDodgeText").GetComponent<Text>().text = heroToCheck.finalDodgeRating.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/DodgeArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewBlockText").GetComponent<Text>().text = heroToCheck.finalBlockRating.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/BlockArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewParryText").GetComponent<Text>().text = heroToCheck.finalParryRating.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/ParryArrow"), "Neutral");
            GameObject.Find("EquipStatsPanel/NewThreatText").GetComponent<Text>().text = heroToCheck.finalThreatRating.ToString();
            ChangeArrow(GameObject.Find("EquipStatsPanel/ThreatArrow"), "Neutral");
        }

    }

    /// <summary>
    /// Cancels from choosing an equipment after clicking a slot in the gui
    /// </summary>
    void CancelFromEquipList()
    {
        PlaySE(AudioManager.instance.backSE);

        equipButtonClicked = null;

        foreach (Transform child in GameObject.Find("GameManager/Menus/EquipMenuCanvas/EquipMenuPanel/EquipListPanel/EquipScroller/EquipListSpacer").transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Public method to change equip mode when clicking option in gui - sets equipMode to given mode
    /// </summary>
    /// <param name="mode">"Equip" or "Remove"</param>
    public void ChangeEquipMode(string mode)
    {
        PlaySE(AudioManager.instance.confirmSE);

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

    #endregion

    #region Status Menu

    /// <summary>
    /// Public method to call Status Menu from gui
    /// </summary>
    public void ShowStatusMenu()
    {
        mainCursorState = MainCursorStates.HEROSELECT1;

        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        PlaySE(AudioManager.instance.confirmSE);

        Debug.Log("Status button clicked - choose a hero");
        menuToDraw = StartCoroutine(ChooseHeroForStatusMenu());
    }

    /// <summary>
    /// Coroutine. Facilitates drawing Status menu for chosen hero
    /// </summary>
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

        StartCoroutine(DrawStatusMenu());
    }

    /// <summary>
    /// Coroutine. Changes menu state to STATUS and draws Status menu
    /// </summary>
    IEnumerator DrawStatusMenu()
    {
        yield return (AnimateMainMenu());

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(StatusMenuCanvas);

        menuState = MenuStates.STATUS;
        DrawStatusMenuBaseStats();
        DrawStatusMenuStats();
        DrawStatusMenuEquipment();
        DrawStatusElements();
        DrawStatusResistances();

        mainCursorState = MainCursorStates.IDLE;
        statusMenuCursorState = StatusMenuCursorStates.STATS;
        cursorOnEquipSlot = 0;

        yield return (AnimateStatusMenu());
    }

    /// <summary>
    /// Coroutine. Hides status menu and sets menu state to MAIN
    /// </summary>
    IEnumerator HideStatusMenu()
    {
        yield return (AnimateStatusMenu());

        DisplayCanvas(MainMenuCanvas);
        HideCanvas(StatusMenuCanvas);
        menuState = MenuStates.MAIN;
        heroToCheck = null;

        ShowMainMenu();

        mainCursorState = MainCursorStates.STATUS;
        statusMenuCursorState = StatusMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Draws chosen hero's base stats to BaseStatsPanel
    /// </summary>
    void DrawStatusMenuBaseStats()
    {
        heroToCheck.UpdateStats();

        DrawHeroFace(heroToCheck, GameObject.Find("StatusMenuPanel/BaseStatsPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/NameText").GetComponent<Text>().text = heroToCheck.name; //Name text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/LevelText").GetComponent<Text>().text = heroToCheck.currentLevel.ToString(); //Level text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/HPText").GetComponent<Text>().text = (heroToCheck.curHP.ToString() + " / " + heroToCheck.finalMaxHP.ToString()); //HP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/MPText").GetComponent<Text>().text = (heroToCheck.curMP.ToString() + " / " + heroToCheck.finalMaxMP.ToString()); //MP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/EXPText").GetComponent<Text>().text = (heroToCheck.currentExp + " / " + HeroDB.instance.levelEXPThresholds[heroToCheck.currentLevel - 1]); //EXP text
        GameObject.Find("StatusMenuPanel/BaseStatsPanel/ToNextLevelText").GetComponent<Text>().text = (HeroDB.instance.levelEXPThresholds[heroToCheck.currentLevel - 1] - heroToCheck.currentExp).ToString(); //To next level text

        StatusPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(heroToCheck), 0, 1), StatusPanelHPProgressBar.transform.localScale.y);
        StatusPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(heroToCheck), 0, 1), StatusPanelMPProgressBar.transform.localScale.y);
        StatusPanelEXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(heroToCheck), 0, 1), StatusPanelEXPProgressBar.transform.localScale.y);
    }

    /// <summary>
    /// Draws detailed stats to StatsPanel
    /// </summary>
    void DrawStatusMenuStats()
    {
        GameObject.Find("StatusMenuPanel/StatsPanel/StrengthText").GetComponent<Text>().text = heroToCheck.finalStrength.ToString(); //Strength text
        GameObject.Find("StatusMenuPanel/StatsPanel/StaminaText").GetComponent<Text>().text = heroToCheck.finalStamina.ToString(); //Stamina text
        GameObject.Find("StatusMenuPanel/StatsPanel/AgilityText").GetComponent<Text>().text = heroToCheck.finalAgility.ToString(); //Agility text
        GameObject.Find("StatusMenuPanel/StatsPanel/DexterityText").GetComponent<Text>().text = heroToCheck.finalDexterity.ToString(); //Dexterity text
        GameObject.Find("StatusMenuPanel/StatsPanel/IntelligenceText").GetComponent<Text>().text = heroToCheck.finalIntelligence.ToString(); //Intelligence text
        GameObject.Find("StatusMenuPanel/StatsPanel/SpiritText").GetComponent<Text>().text = heroToCheck.finalSpirit.ToString(); //Spirit text

        GameObject.Find("StatusMenuPanel/StatsPanel/AttackText").GetComponent<Text>().text = heroToCheck.finalATK.ToString(); //Attack text
        GameObject.Find("StatusMenuPanel/StatsPanel/MagicAttackText").GetComponent<Text>().text = heroToCheck.finalMATK.ToString(); //Magic Attack text
        GameObject.Find("StatusMenuPanel/StatsPanel/DefenseText").GetComponent<Text>().text = heroToCheck.finalDEF.ToString(); //Defense text
        GameObject.Find("StatusMenuPanel/StatsPanel/MagicDefenseText").GetComponent<Text>().text = heroToCheck.finalMDEF.ToString(); //Magic Defense text

        GameObject.Find("StatusMenuPanel/StatsPanel/HitChanceText").GetComponent<Text>().text = heroToCheck.finalHitRating.ToString(); //Hit Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/CritChanceText").GetComponent<Text>().text = heroToCheck.finalCritRating.ToString(); //Crit Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/MoveRatingText").GetComponent<Text>().text = heroToCheck.finalMoveRating.ToString();
        GameObject.Find("StatusMenuPanel/StatsPanel/MPPerTurnText").GetComponent<Text>().text = heroToCheck.finalRegenRating.ToString(); //MP Regen text
        GameObject.Find("StatusMenuPanel/StatsPanel/DodgeChanceText").GetComponent<Text>().text = heroToCheck.finalDodgeRating.ToString(); //Dodge Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/BlockChanceText").GetComponent<Text>().text = heroToCheck.finalBlockRating.ToString(); //Block Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/ParryChanceText").GetComponent<Text>().text = heroToCheck.finalParryRating.ToString(); //Parry Chance text
        GameObject.Find("StatusMenuPanel/StatsPanel/ThreatText").GetComponent<Text>().text = heroToCheck.finalThreatRating.ToString(); //Threat Rating text
    }

    void DrawStatusMenuEquipment()
    {
        if (heroToCheck.equipment[0] != null)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HeadSlot/HeadIcon").GetComponent<Image>().sprite = heroToCheck.equipment[0].icon;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HeadSlot/HeadIcon").GetComponent<Image>().enabled = true;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HeadSlot/HeadText").GetComponent<Text>().text = heroToCheck.equipment[0].name;
        }
        else
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HeadSlot/HeadIcon").GetComponent<Image>().enabled = false;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HeadSlot/HeadText").GetComponent<Text>().text = "-";
        }

        if (heroToCheck.equipment[1] != null)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/ChestSlot/ChestIcon").GetComponent<Image>().sprite = heroToCheck.equipment[1].icon;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/ChestSlot/ChestIcon").GetComponent<Image>().enabled = true;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/ChestSlot/ChestText").GetComponent<Text>().text = heroToCheck.equipment[1].name;
        } else
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/ChestSlot/ChestIcon").GetComponent<Image>().enabled = false;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/ChestSlot/ChestText").GetComponent<Text>().text = "-";
        }

        if (heroToCheck.equipment[2] != null)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HandsSlot/HandsIcon").GetComponent<Image>().sprite = heroToCheck.equipment[2].icon;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HandsSlot/HandsIcon").GetComponent<Image>().enabled = true;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HandsSlot/HandsText").GetComponent<Text>().text = heroToCheck.equipment[2].name;
        } else
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HandsSlot/HandsIcon").GetComponent<Image>().enabled = false;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/HandsSlot/HandsText").GetComponent<Text>().text = "-";
        }
        
        if (heroToCheck.equipment[3] != null)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LegsSlot/LegsIcon").GetComponent<Image>().sprite = heroToCheck.equipment[3].icon;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LegsSlot/LegsIcon").GetComponent<Image>().enabled = true;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LegsSlot/LegsText").GetComponent<Text>().text = heroToCheck.equipment[3].name;
        } else
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LegsSlot/LegsIcon").GetComponent<Image>().enabled = false;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LegsSlot/LegsText").GetComponent<Text>().text = "-";
        }

        if (heroToCheck.equipment[4] != null)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/FeetSlot/FeetIcon").GetComponent<Image>().sprite = heroToCheck.equipment[4].icon;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/FeetSlot/FeetIcon").GetComponent<Image>().enabled = true;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/FeetSlot/FeetText").GetComponent<Text>().text = heroToCheck.equipment[4].name;
        } else
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/FeetSlot/FeetIcon").GetComponent<Image>().enabled = false;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/FeetSlot/FeetText").GetComponent<Text>().text = "-";
        }
         
        if (heroToCheck.equipment[5] != null)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RelicSlot/RelicIcon").GetComponent<Image>().sprite = heroToCheck.equipment[5].icon;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RelicSlot/RelicIcon").GetComponent<Image>().enabled = true;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RelicSlot/RelicText").GetComponent<Text>().text = heroToCheck.equipment[5].name;
        } else
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RelicSlot/RelicIcon").GetComponent<Image>().enabled = false;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RelicSlot/RelicText").GetComponent<Text>().text = "-";
        }

        if (heroToCheck.equipment[6] != null)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RightArmSlot/RightArmIcon").GetComponent<Image>().sprite = heroToCheck.equipment[6].icon;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RightArmSlot/RightArmIcon").GetComponent<Image>().enabled = true;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RightArmSlot/RightArmText").GetComponent<Text>().text = heroToCheck.equipment[6].name;
        } else
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RightArmSlot/RightArmIcon").GetComponent<Image>().enabled = false;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/RightArmSlot/RightArmText").GetComponent<Text>().text = "-";
        }

        if (heroToCheck.equipment[7] != null)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LeftArmSlot/LeftArmIcon").GetComponent<Image>().sprite = heroToCheck.equipment[7].icon;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LeftArmSlot/LeftArmIcon").GetComponent<Image>().enabled = true;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LeftArmSlot/LeftArmText").GetComponent<Text>().text = heroToCheck.equipment[7].name;
        } else
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LeftArmSlot/LeftArmIcon").GetComponent<Image>().enabled = false;
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/EquipPanel/LeftArmSlot/LeftArmText").GetComponent<Text>().text = "-";
        }

    }

    void DrawStatusElements()
    {
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/FireAttackText").GetComponent<Text>().text = heroToCheck.fireEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/FireDefenseText").GetComponent<Text>().text = heroToCheck.fireDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/FrostAttackText").GetComponent<Text>().text = heroToCheck.frostEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/FrostDefenseText").GetComponent<Text>().text = heroToCheck.frostDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/LightningAttackText").GetComponent<Text>().text = heroToCheck.lightningEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/LightningDefenseText").GetComponent<Text>().text = heroToCheck.lightningDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/WaterAttackText").GetComponent<Text>().text = heroToCheck.waterEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/WaterDefenseText").GetComponent<Text>().text = heroToCheck.waterDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/NatureAttackText").GetComponent<Text>().text = heroToCheck.natureEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/NatureDefenseText").GetComponent<Text>().text = heroToCheck.natureDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/EarthAttackText").GetComponent<Text>().text = heroToCheck.earthEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/EarthDefenseText").GetComponent<Text>().text = heroToCheck.earthDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/HolyAttackText").GetComponent<Text>().text = heroToCheck.holyEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/HolyDefenseText").GetComponent<Text>().text = heroToCheck.holyDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/DarknessAttackText").GetComponent<Text>().text = heroToCheck.darkEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ElePanel/DarknessDefenseText").GetComponent<Text>().text = heroToCheck.darkDef.ToString();
    }

    void DrawStatusResistances()
    {
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/BlindEffText").GetComponent<Text>().text = heroToCheck.blindEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/BlindResText").GetComponent<Text>().text = heroToCheck.blindDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/SilenceEffText").GetComponent<Text>().text = heroToCheck.silenceEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/SilenceResText").GetComponent<Text>().text = heroToCheck.silenceDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/SleepEffText").GetComponent<Text>().text = heroToCheck.sleepEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/SleepResText").GetComponent<Text>().text = heroToCheck.sleepDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/ConfuseEffText").GetComponent<Text>().text = heroToCheck.confuseEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/ConfuseResText").GetComponent<Text>().text = heroToCheck.confuseDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/PoisonEffText").GetComponent<Text>().text = heroToCheck.poisonEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/PoisonResText").GetComponent<Text>().text = heroToCheck.poisonDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/PetrifyEffText").GetComponent<Text>().text = heroToCheck.petrifyEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/PetrifyResText").GetComponent<Text>().text = heroToCheck.petrifyDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/SlowEffText").GetComponent<Text>().text = heroToCheck.slowEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/SlowResText").GetComponent<Text>().text = heroToCheck.slowDef.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/ZombieEffText").GetComponent<Text>().text = heroToCheck.zombieEff.ToString();
        GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/StatusPanel/ZombieResText").GetComponent<Text>().text = heroToCheck.zombieDef.ToString();
    }

    #endregion

    #region Talent Menu

    /// <summary>
    /// Public method to call Talents Menu from gui
    /// </summary>
    public void ShowTalentsMenu()
    {
        mainCursorState = MainCursorStates.HEROSELECT1;

        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        PlaySE(AudioManager.instance.confirmSE);

        Debug.Log("Choose hero for talents menu");
        menuToDraw = StartCoroutine(ChooseHeroForTalentsMenu());
    }

    /// <summary>
    /// Coroutine. Facilitates drawing Talents menu for chosen hero
    /// </summary>
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
        StartCoroutine(DrawTalentsMenu());
        DrawHeroTalents();
    }

    /// <summary>
    /// Coroutine.  Sets menu state to TALENTS and draws talents menu
    /// </summary>
    IEnumerator DrawTalentsMenu()
    {
        yield return (AnimateMainMenu());

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(TalentsMenuCanvas);
        menuState = MenuStates.TALENTS;

        DrawTalentsMenuHeroPanel();
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentNameText").GetComponent<Text>().text = "";
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentDetailsPanel/TalentDescText").GetComponent<Text>().text = "";

        mainCursorState = MainCursorStates.IDLE;
        talentMenuCursorState = TalentsMenuCursorStates.TALENTS;
        cursorOnTalent = 0;
        talentRow = 0;
        talentColumn = 0;

        yield return (AnimateTalentsMenu());
    }

    /// <summary>
    /// Coroutine. Sets menu state to MAIN and hides talents menu
    /// </summary>
    IEnumerator HideTalentsMenu()
    {
        yield return (AnimateTalentsMenu());

        DisplayCanvas(MainMenuCanvas);
        HideCanvas(TalentsMenuCanvas);
        menuState = MenuStates.MAIN;
        heroToCheck = null;

        ShowMainMenu();

        mainCursorState = MainCursorStates.TALENTS;
        talentMenuCursorState = TalentsMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Draws chosen hero stats to talents menu
    /// </summary>
    public void DrawTalentsMenuHeroPanel()
    {
        DrawHeroFace(heroToCheck, GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/FacePanel").GetComponent<Image>()); //Draws face graphic
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/NameText").GetComponent<Text>().text = heroToCheck.name; //Name text
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/LevelText").GetComponent<Text>().text = heroToCheck.currentLevel.ToString(); //Level text
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/HPText").GetComponent<Text>().text = (heroToCheck.curHP.ToString() + " / " + heroToCheck.finalMaxHP.ToString()); //HP text
        GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/HeroPanel/MPText").GetComponent<Text>().text = (heroToCheck.curMP.ToString() + " / " + heroToCheck.finalMaxMP.ToString()); //MP text

        TalentsPanelHPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(heroToCheck), 0, 1), TalentsPanelHPProgressBar.transform.localScale.y);
        TalentsPanelMPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(heroToCheck), 0, 1), TalentsPanelMPProgressBar.transform.localScale.y);
    }

    /// <summary>
    /// Draws talent icons for chosen hero to talents gui
    /// </summary>
    void DrawHeroTalents()
    {
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent1/Talent1Button/TalentIcon"), heroToCheck.level1Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent1/Talent2Button/TalentIcon"), heroToCheck.level1Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent1/Talent3Button/TalentIcon"), heroToCheck.level1Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent2/Talent1Button/TalentIcon"), heroToCheck.level2Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent2/Talent2Button/TalentIcon"), heroToCheck.level2Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent2/Talent3Button/TalentIcon"), heroToCheck.level2Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent3/Talent1Button/TalentIcon"), heroToCheck.level3Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent3/Talent2Button/TalentIcon"), heroToCheck.level3Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent3/Talent3Button/TalentIcon"), heroToCheck.level3Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent4/Talent1Button/TalentIcon"), heroToCheck.level4Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent4/Talent2Button/TalentIcon"), heroToCheck.level4Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent4/Talent3Button/TalentIcon"), heroToCheck.level4Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent5/Talent1Button/TalentIcon"), heroToCheck.level5Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent5/Talent2Button/TalentIcon"), heroToCheck.level5Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent5/Talent3Button/TalentIcon"), heroToCheck.level5Talents[2]);

        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent6/Talent1Button/TalentIcon"), heroToCheck.level6Talents[0]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent6/Talent2Button/TalentIcon"), heroToCheck.level6Talents[1]);
        DrawTalentIcon(GameObject.Find("TalentsMenuCanvas/TalentsMenuPanel/TalentsPanel/Talent6/Talent3Button/TalentIcon"), heroToCheck.level6Talents[2]);
    }

    /// <summary>
    /// Processes the drawing of talent icons to gui
    /// </summary>
    /// <param name="obj">GameObject that image component is attached to that should hold the icon image</param>
    /// <param name="talent">The talent containing the icon to be drawn</param>
    void DrawTalentIcon(GameObject obj, BaseTalent talent)
    {
        obj.GetComponent<Image>().sprite = talent.icon;
        if (talent.isActive)
        {
            DrawActiveTalent(obj.GetComponent<Image>());
        }
        else
        {
            DrawInactiveTalent(obj.GetComponent<Image>());
        }
    }

    /// <summary>
    /// Sets given icon alpha to .25 to show it is inactive
    /// </summary>
    void DrawInactiveTalent(Image icon)
    {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, .25f);
    }

    /// <summary>
    /// Sets given icon alpha to 1 to show it is active
    /// </summary>
    void DrawActiveTalent(Image icon)
    {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
    }

    #endregion

    #region Party Menu

    /// <summary>
    /// Public method to call Party Menu from gui
    /// </summary>
    public void ShowPartyMenu()
    {
        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        PlaySE(AudioManager.instance.confirmSE);

        StartCoroutine(DrawPartyMenu());
    }

    /// <summary>
    /// Coroutine. Changes menu state to PARTY and draws Party menu
    /// </summary>
    IEnumerator DrawPartyMenu()
    {
        yield return (AnimateMainMenu());

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(PartyMenuCanvas);
        menuState = MenuStates.PARTY;

        DrawPartyActiveHeroes();
        DrawInactiveHeroButtons();

        mainCursorState = MainCursorStates.IDLE;
        partyMenuCursorState = PartyMenuCursorStates.ACTIVE;
        partyChoosingMode = "Inactive";

        yield return (AnimatePartyMenu());
    }

    /// <summary>
    /// Coroutine. Hides party menu and sets menu state to MAIN
    /// </summary>
    IEnumerator HidePartyMenu()
    {
        yield return (AnimatePartyMenu());

        DisplayCanvas(MainMenuCanvas);
        HideCanvas(PartyMenuCanvas);
        menuState = MenuStates.MAIN;

        ShowMainMenu();

        mainCursorState = MainCursorStates.PARTY;
        partyMenuCursorState = PartyMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Draws active party members into ActiveHeroesPanel
    /// </summary>
    public void DrawPartyActiveHeroes()
    {
        DrawHeroPartyMenuPanels();

        int heroCount = GameManager.instance.activeHeroes.Count;
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

    /// <summary>
    /// Shows party menu panel for each active hero, and hides party menu panel for inactive heroes
    /// </summary>
    void DrawHeroPartyMenuPanels()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;

        if (heroCount == 1)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawInactivePanel(Hero2PartyPanel);
            DrawInactivePanel(Hero3PartyPanel);
            DrawInactivePanel(Hero4PartyPanel);
            DrawInactivePanel(Hero5PartyPanel);
        }
        else if (heroCount == 2)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawActivePanel(Hero2PartyPanel);
            DrawInactivePanel(Hero3PartyPanel);
            DrawInactivePanel(Hero4PartyPanel);
            DrawInactivePanel(Hero5PartyPanel);
        }
        else if (heroCount == 3)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawActivePanel(Hero2PartyPanel);
            DrawActivePanel(Hero3PartyPanel);
            DrawInactivePanel(Hero4PartyPanel);
            DrawInactivePanel(Hero5PartyPanel);
        }
        else if (heroCount == 4)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawActivePanel(Hero2PartyPanel);
            DrawActivePanel(Hero3PartyPanel);
            DrawActivePanel(Hero4PartyPanel);
            DrawInactivePanel(Hero5PartyPanel);
        }
        else if (heroCount == 5)
        {
            DrawActivePanel(Hero1PartyPanel);
            DrawActivePanel(Hero2PartyPanel);
            DrawActivePanel(Hero3PartyPanel);
            DrawActivePanel(Hero4PartyPanel);
            DrawActivePanel(Hero5PartyPanel);
        }

        DrawHeroPartyMenuPanelBars();
    }

    /// <summary>
    /// Sets alpha value to given panel's CanvasGroup to 1
    /// </summary>
    /// <param name="panel">Panel to be displayed</param>
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

    /// <summary>
    /// Sets alpha value to given panel's CanvasGroup to 0
    /// </summary>
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

    /// <summary>
    /// Draws HP and MP bars for each active hero to party menu
    /// </summary>
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

    /// <summary>
    /// Draws inactive hero buttons to party menu
    /// </summary>
    public void DrawInactiveHeroButtons()
    {
        ResetInactiveHeroList();

        foreach (BaseHero hero in GameManager.instance.inactiveHeroes)
        {
            InactivePartyButton = Instantiate(PrefabManager.Instance.inactiveHeroButton);
            InactivePartyButton.transform.Find("NameText").GetComponent<Text>().text = hero.name;
            DrawHeroFace(hero, InactivePartyButton.transform.Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            InactivePartyButton.name = "Inactive Hero Button - ID " + hero.ID;

            InactivePartyButton.transform.SetParent(PartyInactiveRowSpacer, false);
        }
    }

    /// <summary>
    /// Clears inactive party buttons in inactive party list
    /// </summary>
    public void ResetInactiveHeroList()
    {
        foreach (Transform child in PartyInactiveRowSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

    #region Grid Menu

    /// <summary>
    /// Public method to call Grid Menu from gui
    /// </summary>
    public void ShowGridMenu()
    {
        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        DrawHeroGridMenuStats();

        PlaySE(AudioManager.instance.confirmSE);

        StartCoroutine(DrawGridMenu());
    }

    /// <summary>
    /// Coroutine. Draws Grid menu and sets menu state to GRID
    /// </summary>
    IEnumerator DrawGridMenu()
    {
        yield return (AnimateMainMenu());

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(GridMenuCanvas);
        menuState = MenuStates.GRID;

        cursorOnGridHero = 0;
        mainCursorState = MainCursorStates.IDLE;
        gridMenuCursorState = GridMenuCursorStates.HEROES;

        yield return (AnimateGridMenu());
    }

    /// <summary>
    /// Coroutine. Hides grid menu and sets menu state to MAIN
    /// </summary>
    IEnumerator HideGridMenu()
    {
        yield return (AnimateGridMenu());

        ResetItemList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(GridMenuCanvas);
        menuState = MenuStates.MAIN;

        ShowMainMenu();

        mainCursorState = MainCursorStates.GRID;
        gridMenuCursorState = GridMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Draws grid menu base stats for each active hero
    /// </summary>
    public void DrawHeroGridMenuStats()
    {
        DrawHeroGridMenuPanels();

        for (int i = 0; i < GameManager.instance.activeHeroes.Count; i++) //Display hero stats
        {
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).gameObject.name = "HeroGridPanel - ID: " + GameManager.instance.activeHeroes[i].ID;

            DrawHeroFace(GameManager.instance.activeHeroes[i], GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("FacePanel").GetComponent<Image>()); //Draws face graphic
            GameObject.Find("GameManager/Menus/GridMenuCanvas/GridMenuPanel/GridPanel/Grid - " + GameManager.instance.activeHeroes[i].spawnPoint).GetComponent<Image>().sprite = GameManager.instance.activeHeroes[i].faceImage;
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("HPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curHP + " / " + GameManager.instance.activeHeroes[i].finalMaxHP); //HP text
            GameObject.Find("GridMenuCanvas/GridMenuPanel/HeroGridPanel").transform.GetChild(i).Find("MPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].curMP + " / " + GameManager.instance.activeHeroes[i].finalMaxMP); //MP text
        }
    }

    /// <summary>
    /// Shows grid menu panel for each active hero, and hides grid menu panel for inactive heroes
    /// </summary>
    void DrawHeroGridMenuPanels()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        if (heroCount == 1)
        {
            DisplayCanvas(Hero1GridPanel);
            HideCanvas(Hero2GridPanel);
            HideCanvas(Hero3GridPanel);
            HideCanvas(Hero4GridPanel);
            HideCanvas(Hero5GridPanel);
        }
        else if (heroCount == 2)
        {
            DisplayCanvas(Hero1GridPanel);
            DisplayCanvas(Hero2GridPanel);
            HideCanvas(Hero3GridPanel);
            HideCanvas(Hero4GridPanel);
            HideCanvas(Hero5GridPanel);
        }
        else if (heroCount == 3)
        {
            DisplayCanvas(Hero1GridPanel);
            DisplayCanvas(Hero2GridPanel);
            DisplayCanvas(Hero3GridPanel);
            HideCanvas(Hero4GridPanel);
            HideCanvas(Hero5GridPanel);
        }
        else if (heroCount == 4)
        {
            DisplayCanvas(Hero1GridPanel);
            DisplayCanvas(Hero2GridPanel);
            DisplayCanvas(Hero3GridPanel);
            DisplayCanvas(Hero4GridPanel);
            HideCanvas(Hero5GridPanel);
        }
        else if (heroCount == 5)
        {
            DisplayCanvas(Hero1GridPanel);
            DisplayCanvas(Hero2GridPanel);
            DisplayCanvas(Hero3GridPanel);
            DisplayCanvas(Hero4GridPanel);
            DisplayCanvas(Hero5GridPanel);
        }

        DrawHeroGridMenuPanelBars();
    }

    /// <summary>
    /// Draws HP and MP bars for active heroes
    /// </summary>
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

    #endregion

    #region Quest Menu

    /// <summary>
    /// Public method to call Quest Menu from gui
    /// </summary>
    public void ShowQuestMenu()
    {
        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        PlaySE(AudioManager.instance.confirmSE);

        StartCoroutine(DrawQuestMenu());
    }

    /// <summary>
    /// Coroutine. Sets menu state to QUESTS, and draws active/completed quests to quest menu
    /// </summary>
    IEnumerator DrawQuestMenu()
    {
        yield return (AnimateMainMenu());

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(QuestsMenuCanvas);
        menuState = MenuStates.QUESTS;

        DrawActiveQuestList();
        DrawCompletedQuestList();

        FirstShowActiveQuestsPanel();

        ClearQuestMenuFields();

        cursorOnQuest = 0;
        cursorOnQuestOption = 0;
        QuestOption = "Active";
        mainCursorState = MainCursorStates.IDLE;
        questMenuCursorState = QuestMenuCursorStates.QUESTLIST;

        yield return (AnimateQuestMenu());
    }

    /// <summary>
    /// Coroutine. Sets menu state to MAIN and hides quest menu
    /// </summary>
    IEnumerator HideQuestMenu()
    {
        yield return (AnimateQuestMenu());

        ResetActiveQuestList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(QuestsMenuCanvas);
        menuState = MenuStates.MAIN;

        ShowMainMenu();

        mainCursorState = MainCursorStates.QUESTS;
        questMenuCursorState = QuestMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Draws quests from GameManager.instance.activeQuests to active quest list in gui
    /// </summary>
    public void DrawActiveQuestList()
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

    /// <summary>
    /// Draws quests from GameManager.instance.completedQuests to completed quest list in gui
    /// </summary>
    public void DrawCompletedQuestList()
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

    /// <summary>
    /// Hides completed quests panel and shows active quests panel
    /// </summary>
    public void FirstShowActiveQuestsPanel()
    {
        QuestOption = "Active";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// Hides completed quests panel and shows active quests panel
    /// </summary>
    public void ShowActiveQuestsPanel()
    {
        PlaySE(AudioManager.instance.confirmSE);
        QuestOption = "Active";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// Hides active quests panel and shows completed quests panel
    /// </summary>
    public void ShowCompletedQuestsPanel()
    {
        PlaySE(AudioManager.instance.confirmSE);

        QuestOption = "Completed";
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/ActiveQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/Menus/QuestsMenuCanvas/CompletedQuestsMenuPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// Clears all fields on Active and Completed quests fields
    /// </summary>
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

    /// <summary>
    /// Clears quests in active quest list
    /// </summary>
    public void ResetActiveQuestList()
    {
        foreach (Transform child in ActiveQuestListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Clears quests in completed quest list
    /// </summary>
    public void ResetCompletedQuestList()
    {
        foreach (Transform child in CompletedQuestListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// De-selects chosen quest so another quest can be chosen
    /// </summary>
    void CancelQuestSelect()
    {
        PlaySE(AudioManager.instance.backSE);

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

    #endregion

    #region Bestiary Menu

    /// <summary>
    /// Public method to call Bestiary Menu from gui
    /// </summary>
    public void ShowBestiaryMenu() //displays item menu
    {
        if (menuToDraw != null)
        {
            StopCoroutine(menuToDraw);
        }

        PlaySE(AudioManager.instance.confirmSE);

        StartCoroutine(DrawBestiaryMenu());
    }

    /// <summary>
    /// Coroutine. Changes menu state to BESTIARY and draws Bestiary menu
    /// </summary>
    IEnumerator DrawBestiaryMenu()
    {
        yield return (AnimateMainMenu());

        HideCanvas(MainMenuCanvas);
        DisplayCanvas(BestiaryMenuCanvas);
        menuState = MenuStates.BESTIARY;

        DrawBestiaryEntryList();

        mainCursorState = MainCursorStates.IDLE;
        bestiaryMenuCursorState = BestiaryMenuCursorStates.BESTIARYLIST;

        yield return (AnimateBestiaryMenu());
    }

    /// <summary>
    /// Coroutine. Sets menu state to MAIN and hides Bestiary menu
    /// </summary>
    IEnumerator HideBestiaryMenu()
    {
        yield return (AnimateBestiaryMenu());

        ResetActiveQuestList();
        DisplayCanvas(MainMenuCanvas);
        HideCanvas(BestiaryMenuCanvas);
        menuState = MenuStates.MAIN;

        ShowMainMenu();

        mainCursorState = MainCursorStates.BESTIARY;
        bestiaryMenuCursorState = BestiaryMenuCursorStates.IDLE;
    }

    /// <summary>
    /// Draws entries for bestiary menu for each enemy thats been defeated
    /// </summary>
    public void DrawBestiaryEntryList()
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

    /// <summary>
    /// Clears fields for Bestiary menu when cursor exits an entry or player cancels the enemy selected
    /// </summary>
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

    /// <summary>
    /// Clears enemy selected when cancelled so 'cancel' input doesn't go straight to main menu
    /// </summary>
    void CancelBestiarySelect()
    {
        PlaySE(AudioManager.instance.backSE);

        BestiaryEntryClicked = false;

        foreach (Transform child in GameObject.Find("GameManager/Menus/BestiaryMenuCanvas/BestiaryMenuPanel/EnemyListPanel/EnemyListScroller/EnemyListSpacer").transform)
        {
            child.Find("EnemyNameText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
        ClearBestiaryMenuFields();
    }

    /// <summary>
    /// Clears all bestiary entries
    /// </summary>
    public void ResetBestiaryList()
    {
        foreach (Transform child in BestiaryEnemyListSpacer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

    #region -----Menu Animations-----

    /// <summary>
    /// Plays given sound effect once
    /// </summary>
    /// <param name="SE">Sound effect to play</param>
    public void PlaySE(AudioClip SE)
    {
        menuAudioSource.PlayOneShot(SE);
    }

    /// <summary>
    /// Plays given sound effect once
    /// </summary>
    /// <param name="SE">Sound effect to play</param>
    public void PlayAnimSE(AudioClip SE)
    {
        animAudioSource.PlayOneShot(SE);
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of main menu panel animations
    /// </summary>
    IEnumerator AnimateMainMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (MM_infoPanelAnimator != null)
        {
            bool isOpen = MM_infoPanelAnimator.GetBool("open");

            MM_infoPanelAnimator.SetBool("open", !isOpen);
        }

        if (MM_menuButtonsPanelAnimator != null)
        {
            bool isOpen = MM_menuButtonsPanelAnimator.GetBool("open");

            MM_menuButtonsPanelAnimator.SetBool("open", !isOpen);
        }

        if (MM_timeGoldPanelAnimator != null)
        {
            bool isOpen = MM_timeGoldPanelAnimator.GetBool("open");

            MM_timeGoldPanelAnimator.SetBool("open", !isOpen);
        }

        if (MM_locationPanelAnimator != null)
        {
            bool isOpen = MM_locationPanelAnimator.GetBool("open");

            MM_locationPanelAnimator.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(MM_infoPanelAnimator));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of item menu panel animations
    /// </summary>
    IEnumerator AnimateItemMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Items_itemOptionsPanel != null)
        {
            bool isOpen = Items_itemOptionsPanel.GetBool("open");

            Items_itemOptionsPanel.SetBool("open", !isOpen);
        }

        if (Items_itemDescriptionPanel != null)
        {
            bool isOpen = Items_itemDescriptionPanel.GetBool("open");

            Items_itemDescriptionPanel.SetBool("open", !isOpen);
        }

        if (Items_heroItemPanel != null)
        {
            bool isOpen = Items_heroItemPanel.GetBool("open");

            Items_heroItemPanel.SetBool("open", !isOpen);
        }

        if (Items_itemListPanel != null)
        {
            bool isOpen = Items_itemListPanel.GetBool("open");

            Items_itemListPanel.SetBool("open", !isOpen);
        }

        if (Items_keyItemListPanel != null)
        {
            bool isOpen = Items_keyItemListPanel.GetBool("open");

            Items_keyItemListPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Items_itemOptionsPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of magic menu panel animations
    /// </summary>
    IEnumerator AnimateMagicMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Magic_heroMagicPanel != null)
        {
            bool isOpen = Magic_heroMagicPanel.GetBool("open");

            Magic_heroMagicPanel.SetBool("open", !isOpen);
        }

        if (Magic_magicOptionsPanel != null)
        {
            bool isOpen = Magic_magicOptionsPanel.GetBool("open");

            Magic_magicOptionsPanel.SetBool("open", !isOpen);
        }

        if (Magic_magicDetailsPanel != null)
        {
            bool isOpen = Magic_magicDetailsPanel.GetBool("open");

            Magic_magicDetailsPanel.SetBool("open", !isOpen);
        }

        if (Magic_magicDescriptionPanel != null)
        {
            bool isOpen = Magic_magicDescriptionPanel.GetBool("open");

            Magic_magicDescriptionPanel.SetBool("open", !isOpen);
        }

        if (Magic_whiteMagicListPanel != null)
        {
            bool isOpen = Magic_whiteMagicListPanel.GetBool("open");

            Magic_whiteMagicListPanel.SetBool("open", !isOpen);
        }

        if (Magic_blackMagicListPanel != null)
        {
            bool isOpen = Magic_blackMagicListPanel.GetBool("open");

            Magic_blackMagicListPanel.SetBool("open", !isOpen);
        }

        if (Magic_sorceryMagicListPanel != null)
        {
            bool isOpen = Magic_sorceryMagicListPanel.GetBool("open");

            Magic_sorceryMagicListPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Magic_heroMagicPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of equip menu panel animations
    /// </summary>
    IEnumerator AnimateEquipMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Equip_equipDescriptionPanel != null)
        {
            bool isOpen = Equip_equipDescriptionPanel.GetBool("open");

            Equip_equipDescriptionPanel.SetBool("open", !isOpen);
        }

        if (Equip_heroEquipPanel != null)
        {
            bool isOpen = Equip_heroEquipPanel.GetBool("open");

            Equip_heroEquipPanel.SetBool("open", !isOpen);
        }

        if (Equip_equipOptionsPanel != null)
        {
            bool isOpen = Equip_equipOptionsPanel.GetBool("open");

            Equip_equipOptionsPanel.SetBool("open", !isOpen);
        }

        if (Equip_equipSlotsPanel != null)
        {
            bool isOpen = Equip_equipSlotsPanel.GetBool("open");

            Equip_equipSlotsPanel.SetBool("open", !isOpen);
        }

        if (Equip_equipListPanel != null)
        {
            bool isOpen = Equip_equipListPanel.GetBool("open");

            Equip_equipListPanel.SetBool("open", !isOpen);
        }

        if (Equip_equipStatsPanel != null)
        {
            bool isOpen = Equip_equipStatsPanel.GetBool("open");

            Equip_equipStatsPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Equip_equipDescriptionPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of status menu panel animations
    /// </summary>
    IEnumerator AnimateStatusMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Status_baseStatsPanel != null)
        {
            bool isOpen = Status_baseStatsPanel.GetBool("open");

            Status_baseStatsPanel.SetBool("open", !isOpen);
        }

        if (Status_passivePanel != null)
        {
            bool isOpen = Status_passivePanel.GetBool("open");

            Status_passivePanel.SetBool("open", !isOpen);
        }

        if (Status_detailsPanel != null)
        {
            bool isOpen = Status_detailsPanel.GetBool("open");

            Status_detailsPanel.SetBool("open", !isOpen);
        }

        if (Status_statsPanel != null)
        {
            bool isOpen = Status_statsPanel.GetBool("open");

            Status_statsPanel.SetBool("open", !isOpen);
        }

        if (Status_equipPanel != null)
        {
            bool isOpen = Status_equipPanel.GetBool("open");

            Status_equipPanel.SetBool("open", !isOpen);
        }

        if (Status_elePanel != null)
        {
            bool isOpen = Status_elePanel.GetBool("open");

            Status_elePanel.SetBool("open", !isOpen);
        }

        if (Status_statusPanel != null)
        {
            bool isOpen = Status_statusPanel.GetBool("open");

            Status_statusPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Status_baseStatsPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of status menu panel animations
    /// </summary>
    IEnumerator AnimateSecondaryStatusMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        bool activesWasOpen = Status_activesPanel.GetBool("open");
        bool magicWasOpen = Status_magicPanel.GetBool("open");

        if (Status_activesPanel != null)
        {
            bool isOpen = Status_activesPanel.GetBool("open");

            if (!isOpen)
            {
                GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ActiveEffectsPanel").GetComponent<CanvasGroup>().alpha = 1;
            }

            Status_activesPanel.SetBool("open", !isOpen);
        }

        if (Status_magicPanel != null)
        {
            bool isOpen = Status_magicPanel.GetBool("open");
            
            if (!isOpen)
            {
                GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/MagicPanel").GetComponent<CanvasGroup>().alpha = 1;
            }

            Status_magicPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Status_statsPanel));

        if (magicWasOpen)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/MagicPanel").GetComponent<CanvasGroup>().alpha = 0;
        }

        if (activesWasOpen)
        {
            GameObject.Find("GameManager/Menus/StatusMenuCanvas/StatusMenuPanel/ActiveEffectsPanel").GetComponent<CanvasGroup>().alpha = 0;
        }

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of talents menu panel animations
    /// </summary>
    IEnumerator AnimateTalentsMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Talents_heroPanel != null)
        {
            bool isOpen = Talents_heroPanel.GetBool("open");

            Talents_heroPanel.SetBool("open", !isOpen);
        }

        if (Talents_talentsPanel != null)
        {
            bool isOpen = Talents_talentsPanel.GetBool("open");

            Talents_talentsPanel.SetBool("open", !isOpen);
        }

        if (Talents_talentDetailsPanel != null)
        {
            bool isOpen = Talents_talentDetailsPanel.GetBool("open");

            Talents_talentDetailsPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Talents_heroPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of party menu panel animations
    /// </summary>
    IEnumerator AnimatePartyMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Party_activeHeroesPanel != null)
        {
            bool isOpen = Party_activeHeroesPanel.GetBool("open");

            Party_activeHeroesPanel.SetBool("open", !isOpen);
        }

        if (Party_inactiveHeroesPanel != null)
        {
            bool isOpen = Party_inactiveHeroesPanel.GetBool("open");

            Party_inactiveHeroesPanel.SetBool("open", !isOpen);
        }

        if (Party_selectedHeroPanel != null)
        {
            bool isOpen = Party_selectedHeroPanel.GetBool("open");

            Party_selectedHeroPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Party_activeHeroesPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of grid menu panel animations
    /// </summary>
    IEnumerator AnimateGridMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Grid_heroGridPanel != null)
        {
            bool isOpen = Grid_heroGridPanel.GetBool("open");

            Grid_heroGridPanel.SetBool("open", !isOpen);
        }

        if (Grid_gridPanel != null)
        {
            bool isOpen = Grid_gridPanel.GetBool("open");

            Grid_gridPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Grid_heroGridPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of quest menu panel animations
    /// </summary>
    IEnumerator AnimateQuestMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Quest_questListOptions != null)
        {
            bool isOpen = Quest_questListOptions.GetBool("open");

            Quest_questListOptions.SetBool("open", !isOpen);
        }

        if (Quest_questListPanel != null)
        {
            bool isOpen = Quest_questListPanel.GetBool("open");

            Quest_questListPanel.SetBool("open", !isOpen);
        }

        if (Quest_questNamePanel != null)
        {
            bool isOpen = Quest_questNamePanel.GetBool("open");

            Quest_questNamePanel.SetBool("open", !isOpen);
        }

        if (Quest_questLevelRequirementsPanel != null)
        {
            bool isOpen = Quest_questLevelRequirementsPanel.GetBool("open");

            Quest_questLevelRequirementsPanel.SetBool("open", !isOpen);
        }

        if (Quest_questDetailsPanel != null)
        {
            bool isOpen = Quest_questDetailsPanel.GetBool("open");

            Quest_questDetailsPanel.SetBool("open", !isOpen);
        }

        if (Quest_questRewardsPanel != null)
        {
            bool isOpen = Quest_questRewardsPanel.GetBool("open");

            Quest_questRewardsPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Quest_questListOptions));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of bestiary menu panel animations
    /// </summary>
    IEnumerator AnimateBestiaryMenu()
    {
        inAnimation = true;

        PlayAnimSE(AudioManager.instance.openMenuSE);

        if (Bestiary_enemyListPanel != null)
        {
            bool isOpen = Bestiary_enemyListPanel.GetBool("open");

            Bestiary_enemyListPanel.SetBool("open", !isOpen);
        }

        if (Bestiary_enemyNamePanel != null)
        {
            bool isOpen = Bestiary_enemyNamePanel.GetBool("open");

            Bestiary_enemyNamePanel.SetBool("open", !isOpen);
        }

        if (Bestiary_enemyLevelPanel != null)
        {
            bool isOpen = Bestiary_enemyLevelPanel.GetBool("open");

            Bestiary_enemyLevelPanel.SetBool("open", !isOpen);
        }

        if (Bestiary_enemyGraphicPanel != null)
        {
            bool isOpen = Bestiary_enemyGraphicPanel.GetBool("open");

            Bestiary_enemyGraphicPanel.SetBool("open", !isOpen);
        }

        if (Bestiary_enemyDescriptionPanel != null)
        {
            bool isOpen = Bestiary_enemyDescriptionPanel.GetBool("open");

            Bestiary_enemyDescriptionPanel.SetBool("open", !isOpen);
        }

        if (Bestiary_enemyDetailsPanel != null)
        {
            bool isOpen = Bestiary_enemyDetailsPanel.GetBool("open");

            Bestiary_enemyDetailsPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Bestiary_enemyListPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of magic menu 'HeroSelectMagicPanel' animation
    /// </summary>
    public IEnumerator AnimateMagicHeroSelectPanel()
    {
        inAnimation = true;

        if (Magic_heroSelectMagicPanel != null)
        {
            bool isOpen = Magic_heroSelectMagicPanel.GetBool("open");

            Magic_heroSelectMagicPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Magic_heroSelectMagicPanel));

        inAnimation = false;
    }

    /// <summary>
    /// Coroutine.  Facilitates processing of item menu 'ArrangeOptionsPanel' animation
    /// </summary>
    public IEnumerator AnimateArrangeOptionsPanel()
    {
        inAnimation = true;

        if (Items_arrangeOptionsPanel != null)
        {
            bool isOpen = Items_arrangeOptionsPanel.GetBool("open");

            Items_arrangeOptionsPanel.SetBool("open", !isOpen);
        }

        yield return new WaitForSeconds(GetAnimationTime(Items_arrangeOptionsPanel));

        inAnimation = false;
    }

    #endregion

    #region -----Tools/Methods for above menu methods-----

    /// <summary>
    /// Shows given canvas GameObject and makes it interactable
    /// </summary>
    /// <param name="canvas">Canvas GameObject to be made visible/param>
    public void DisplayCanvas(GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = 1;
        canvas.GetComponent<CanvasGroup>().interactable = true;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// Hides given canvas and makes it non-interactable
    /// </summary>
    /// <param name="canvas">Canvas GameObject to hide</param>
    public void HideCanvas(GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        canvas.GetComponent<CanvasGroup>().interactable = false;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// Keeps background objects from processing
    /// </summary>
    /// <param name="pause">If true, enables 'inMenu' so background processes will not run</param>
    public void PauseBackground(bool pause)
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

    /// <summary>
    /// Makes sure cancel button is only processed once when pressed
    /// </summary>
    void CheckCancelPressed()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //Debug.Log("buttonPressed");
            buttonPressed = true;
            cancelPressed = true;
        }
        if (buttonPressed)
        {
            if (Input.GetButtonUp("Cancel"))
            {
                //Debug.Log("button released");
                buttonPressed = false;
                cancelPressed = false;
            }
        }
    }

    /// <summary>
    /// Used by any menu option that needs a hero chosen (Magic, Equip, Status, Talents) to select the hero to open in the menu - hero chosen is set to 'heroToCheck'
    /// </summary>
    void GetHeroClicked()
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
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero1Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[0];
                }
                if (result.gameObject.name == "Hero2Panel")
                {
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero2Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[1];
                }
                if (result.gameObject.name == "Hero3Panel")
                {
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero3Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[2];
                }
                if (result.gameObject.name == "Hero4Panel")
                {
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero4Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[3];
                }
                if (result.gameObject.name == "Hero5Panel")
                {
                    choosingHero = false;
                    GameObject.Find("GameManager/Menus/MainMenuCanvas/MainMenuPanel/HeroInfoPanel/Hero5Panel").GetComponent<MainMenuMouseEvents>().HideBorder();
                    heroToCheck = GameManager.instance.activeHeroes[4];
                }
            }

            if (!choosingHero)
            {
                PlaySE(AudioManager.instance.confirmSE);
            }
        }
    }

    /// <summary>
    /// Unbolds all menu buttons text, and then bolds the selected button text.  Used for selecting a menu option and choosing a hero
    /// </summary>
    /// <param name="button">Button to bold</param>
    void BoldButton(Button button)
    {
        UnboldButton(ItemButton);
        UnboldButton(MagicButton);
        UnboldButton(EquipButton);
        UnboldButton(StatusButton);
        UnboldButton(TalentsButton);
        UnboldButton(PartyButton);
        UnboldButton(GridButton);
        UnboldButton(QuestsButton);
        UnboldButton(BestiaryButton);

        button.gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
    }

    /// <summary>
    /// Sets given button text to normal
    /// </summary>
    /// <param name="button">Button to set to normal</param>
    void UnboldButton(Button button)
    {
        button.gameObject.GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
    }

    /// <summary>
    /// Calculates the HP for progress bar for given hero - returns their current HP / their max HP
    /// </summary>
    /// <param name="hero">Hero to gather HP data from</param>
    float GetProgressBarValuesHP(BaseHero hero)
    {
        float heroHP = hero.curHP;
        float heroBaseHP = hero.finalMaxHP;
        float calc_HP;

        calc_HP = heroHP / heroBaseHP;

        return calc_HP;
    }

    /// <summary>
    /// Calculates the MP for progress bar for given hero - returns their current MP / their max MP
    /// </summary>
    /// <param name="hero">Hero to gather MP data from</param>
    float GetProgressBarValuesMP(BaseHero hero)
    {
        float heroMP = hero.curMP;
        float heroBaseMP = hero.finalMaxMP;
        float calc_MP;

        calc_MP = heroMP / heroBaseMP;

        return calc_MP;
    }

    /// <summary>
    /// Calculates the EXP for progress bar for given hero - returns a calculation of the EXP needed to reach the next level - their current EXP
    /// </summary>
    /// <param name="hero">Hero to gather EXP data from</param>
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
            heroEXP = (hero.currentExp - HeroDB.instance.levelEXPThresholds[hero.currentLevel - 2]);
            //Debug.Log("baseLine: " + baseLineEXP);
            //Debug.Log(hero.currentExp - HeroDB.instance.levelEXPThresholds[hero.currentLevel - 2]);
        }

        float calcEXP = heroEXP / baseLineEXP;


        return calcEXP;
    }

    /// <summary>
    /// Draws the given hero's face image onto the given image on UI
    /// </summary>
    /// <param name="hero">Hero to gather face image data from</param>
    /// <param name="faceImage">Image component on GameObject that image should be drawn onto</param>
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

    /// <summary>
    /// Returns the hero by given ID based on HeroDB
    /// </summary>
    /// <param name="ID">ID of hero needing to be returned</param>
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

    /// <summary>
    /// Used by GetHeroClicked() to find which panel on main menu is clicked
    /// </summary>
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    /// <summary>
    /// Returns full animation time in seconds by given animator component
    /// </summary>
    /// <param name="anim">Animator component to measure animation time</param>
    float GetAnimationTime(Animator anim)
    {
        float animTime = 0f;
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            animTime = ac.animationClips[i].length;
        }

        return animTime;
    }

    /// <summary>
    /// Takes screenshot of player camera, and sets menu BG image to this screenshot
    /// </summary>
    /*void ScreenshotBG()
    {
        ScreenCapture.CaptureScreenshot("Assets/Resources/Temp/BGScreenshot.png");
        AssetDatabase.ImportAsset("Assets/Resources/Temp/BGScreenshot.png");
        MenuBG.sprite = Resources.Load<Sprite>("Temp/BGScreenshot");

    }

    public IEnumerator ScreenShotCoroutine()
    {
        yield return new WaitForEndOfFrame();
        string path = "Assets/Resources/Temp/BGScreenshot.png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        screenImage.Apply();

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        //Convert to png(Expensive)
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Wait for a long time
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        //Create new thread then save image to file
        new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(100);
            File.WriteAllBytes(path, imageBytes);
        }).Start();

        MenuBG.sprite = Resources.Load<Sprite>("Temp/BGScreenshot");
    }

    void DeleteScreenshot()
    {
        MenuBG.sprite = null;

        File.Delete("Assets/Resources/Temp/BGScreenshot.png");
        File.Delete("Assets/Resources/Temp/BGScreenshot.png.meta");
    }*/

                #endregion

}
