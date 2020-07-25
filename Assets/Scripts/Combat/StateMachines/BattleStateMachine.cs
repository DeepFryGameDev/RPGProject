using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour //for processing phases of battle between enemies and heroes
{
    [HideInInspector] public HandleTurn HeroChoice; //the variable to store current hero's selection
    
    [HideInInspector] public GameObject moveActionPanel;
    [HideInInspector] public GameObject actionPanel; //panel that displays attack, magic, etc.
    [HideInInspector] public GameObject enemySelectPanel; //panel that displays list of enemy targets
    [HideInInspector] public GameObject magicPanel; //panel that lists magic attacks
    [HideInInspector] public GameObject itemPanel;
    [HideInInspector] public GameObject battleDetailsPanel;
    
    Transform moveActionSpacer;
    Transform actionSpacer;
    Transform magicSpacer;
    Transform itemSpacer;

    GameObject actionButton;
    GameObject magicButton;
    GameObject itemButton;

    //for victory screen
    bool victoryConfirmButtonPressed;
    bool runVictory;
    int goldDropped;
    List<Item> itemsDropped = new List<Item>();
    GameObject victoryCanvas;
    GameObject hero1Panel;
    GameObject hero2Panel;
    GameObject hero3Panel;
    GameObject hero4Panel;
    GameObject hero5Panel;
    Image hero1EXPProgressBar;
    Image hero2EXPProgressBar;
    Image hero3EXPProgressBar;
    Image hero4EXPProgressBar;
    Image hero5EXPProgressBar;
    GameObject newItemPanel;
    Transform itemListSpacer;
    float expGainSpeed = .0125f;
    public AudioClip expTick;
    AudioSource audioSource;
    public float victoryPoseTime;

    //for displaying damage
    public float damageTextDistance;
    public float damageDisplayTime;
    GameObject damageText;

    public battleStates battleState;

    public HeroGUI HeroInput;

    public bool choosingTarget;
    public GameObject chosenTarget;
    List<Tile> tilesInRange = new List<Tile>();

    private List<GameObject> atkBtns = new List<GameObject>();

    //enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject>();
    
    //for adding EXP after battle
    public int expPool;

    //for Active/Wait Time Battle
    public bool activeATB = false; //change for active or wait ATB;
    public bool pendingTurn = false;

    //for moving 'back' in menus with Cancel button (escape by default)
    bool enemySelectAfterAttackCancel = false;
    bool magicAttackCancel = false;
    bool enemySelectAfterMagicCancel = false;
    bool itemBattleMenuCancel = false;
    bool enemySelectAfterItemMenuCancel = false;
    bool moveMenuCancel = false;
    public bool cancelledEnemySelect = false;

    //for MP calculations
    [HideInInspector] public List<BaseAttack> attacksWithinMPThreshold = new List<BaseAttack>();

    //for finding which targets are in range
    protected List<GameObject> targetsInRange = new List<GameObject>();
    Pattern pattern = new Pattern();

    //for finding which attacks are in range
    protected List<BaseAttack> attacksWithinRange = new List<BaseAttack>();

    int lowestRangeForAttack = 0;

    bool buttonPressed;

    public List<HandleTurn> PerformList = new List<HandleTurn>(); //to store the turns that have been chosen between enemies and heroes
    public List<GameObject> HeroesInBattle = new List<GameObject>(); //to store the gameobjects for all living heroes in battle
    public List<GameObject> AllHeroesInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>(); //to store the gameobjects for all enemies in battle

    public List<GameObject> HeroesToManage = new List<GameObject>(); //to store the gameobjects for all heros available to make a selection
    public GameObject enemyToManage;

    public List<GameObject> targets = new List<GameObject>();

    private PlayerMove playerMove;

    void Start()
    {
        moveActionPanel = GameObject.Find("BattleCanvas/BattleUI/MoveActionPanel");
        actionPanel = GameObject.Find("BattleCanvas/BattleUI/ActionPanel");
        enemySelectPanel = GameObject.Find("BattleCanvas/BattleUI/SelectTargetPanel");
        magicPanel = GameObject.Find("BattleCanvas/BattleUI/MagicPanel");
        itemPanel = GameObject.Find("BattleCanvas/BattleUI/ItemPanel");
        battleDetailsPanel = GameObject.Find("BattleCanvas/BattleUI/BattleDetailsPanel");

        moveActionSpacer = GameObject.Find("BattleCanvas/BattleUI/MoveActionPanel/MoveActionSpacer").transform;
        actionSpacer = GameObject.Find("BattleCanvas/BattleUI/ActionPanel/ActionSpacer").transform;
        magicSpacer = GameObject.Find("BattleCanvas/BattleUI/MagicPanel/MagicScroller/MagicSpacer").transform;
        itemSpacer = GameObject.Find("BattleCanvas/BattleUI/ItemPanel/ItemScroller/ItemSpacer").transform;

        actionButton = PrefabManager.Instance.battleActionButton;
        magicButton = PrefabManager.Instance.battleMagicButton;
        itemButton = PrefabManager.Instance.battleItemButton;

        victoryCanvas = GameObject.Find("BattleCanvas/VictoryCanvas");
        hero1Panel = victoryCanvas.transform.Find("VictoryPanel/HeroEXPPanel/Hero1Panel").gameObject;
        hero2Panel = victoryCanvas.transform.Find("VictoryPanel/HeroEXPPanel/Hero2Panel").gameObject;
        hero3Panel = victoryCanvas.transform.Find("VictoryPanel/HeroEXPPanel/Hero3Panel").gameObject;
        hero4Panel = victoryCanvas.transform.Find("VictoryPanel/HeroEXPPanel/Hero4Panel").gameObject;
        hero5Panel = victoryCanvas.transform.Find("VictoryPanel/HeroEXPPanel/Hero5Panel").gameObject;
        hero1EXPProgressBar = hero1Panel.transform.Find("LevelProgressBarBG/LevelProgressBar").gameObject.GetComponent<Image>();
        hero2EXPProgressBar = hero2Panel.transform.Find("LevelProgressBarBG/LevelProgressBar").gameObject.GetComponent<Image>();
        hero3EXPProgressBar = hero3Panel.transform.Find("LevelProgressBarBG/LevelProgressBar").gameObject.GetComponent<Image>();
        hero4EXPProgressBar = hero4Panel.transform.Find("LevelProgressBarBG/LevelProgressBar").gameObject.GetComponent<Image>();
        hero5EXPProgressBar = hero5Panel.transform.Find("LevelProgressBarBG/LevelProgressBar").gameObject.GetComponent<Image>();
        itemListSpacer = victoryCanvas.transform.Find("VictoryPanel/ItemsEarnedPanel/ItemSpacer");

        //-------------------------

        battleState = battleStates.WAIT; //battle starts in Battle State "WAIT"
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero")); //adds all heros with Hero tag to Heroes in Battle list
        for (int i = 0; i < HeroesInBattle.Count; i++) //copies all available heroes in battle to 'AllHeroesInBattle' so both lists can be used differently
        {
            AllHeroesInBattle.Add(HeroesInBattle[i]);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyBehavior eb = obj.GetComponent(typeof(EnemyBehavior)) as EnemyBehavior;
            eb.InitMove();
            eb.InitBehavior();
        }

        //insert here to tie heroes in battle to new hero manager stuff to maintain exp, hp, mp, etc. can loop through heroes in battle to assign.
        HeroInput = HeroGUI.ACTIVATE; //battle starts with hero interface in state "ACTIVATE"

        HidePanel(moveActionPanel);
        HidePanel(actionPanel);
        HidePanel(enemySelectPanel);
        HidePanel(magicPanel);
        HidePanel(itemPanel);

        audioSource = GameObject.Find("BattleManager").GetComponent<AudioSource>();

        HidePanel(victoryCanvas);
        HidePanel(GameObject.Find("BattleCanvas/BattleUI")); //for battle transition
    }

    private void Awake()
    {
        for (int i = 0; i < GameManager.instance.activeHeroes.Count; i++)
        {
            //GameObject NewHero = Instantiate(GameManager.instance.heroesToBattle[i], heroSpawnPoints[i].position, Quaternion.identity) as GameObject;
            GameObject NewHero = Instantiate(GameManager.instance.heroesToBattle[i], GetHeroSpawnPoint(GameManager.instance.heroesToBattle[i]).transform.position, Quaternion.identity) as GameObject;
            NewHero.name = "BattleHero - ID " + GameManager.instance.heroesToBattle[i].GetComponent<HeroStateMachine>().hero.ID;
            //NewHero.GetComponent<Canvas>().sortingLayerName = "Sprites";
            //NewHero.AddComponent<PlayerMove>();
        }
        for(int i=0; i < GameManager.instance.enemyAmount; i++)
        {
            string spawnPoint = (GameManager.instance.enemySpawnPoints[i]);  //need to set spawn point by troop and use it below
            GameObject spawnPointObject = GameObject.Find("EnemySpawnPoints/EnemySP" + spawnPoint);
            GameObject NewEnemy = Instantiate(GameManager.instance.enemiesToBattle[i].prefab, spawnPointObject.transform.position, Quaternion.identity) as GameObject; //uses enemy prefabs in Encounter region list and creates them as gameobjects
            NewEnemy.name = "BattleEnemy - ID " + GameManager.instance.enemiesToBattle[i].ID;
            //NewEnemy.GetComponent<Canvas>().sortingLayerName = "Sprites";

            NewEnemy.GetComponent<EnemyStateMachine>().enemy = GetEnemy(GameManager.instance.enemiesToBattle[i].ID); //sets the created enemy's name in the state machine
            EnemiesInBattle.Add(NewEnemy); //adds the created enemy to enemies in battle list
        }

        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = true;
    }
    
    void Update()
    {
        switch(battleState) //phases of battle
        {
            case (battleStates.WAIT): //checks if actions are to be taken
                //Debug.Log("waiting for action");
                if (PerformList.Count > 0) //if there are actions to be taken (from enemy or hero)
                {
                    //Debug.Log("found in perform list");
                    battleState = battleStates.TAKEACTION;
                }
            break;

            case (battleStates.TAKEACTION): //checks for hero/enemy and processes action
                GameObject performer = GameObject.Find(PerformList[0].Attacker); //creates game object = enemy or hero that is attacking as "performer" which is used as current attacker (hero or enemy)
                if (PerformList[0].attackerType == Types.HERO) //if attacker is a hero
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>(); //gets hero's state machine
                    //HSM.ActionTarget = PerformList[0].AttackersTarget; //changes hero state machine enemy to attack to the hero's target
                    HSM.currentState = HeroStateMachine.TurnState.ACTION; //tells hero state machine to start ACTION phase
                }
                battleState = battleStates.PERFORMACTION; //changes battle state to PERFORMACTION
            break;

            case (battleStates.PERFORMACTION): //idle state while action is chosn

            break;

            case (battleStates.CHECKALIVE): //checks when hero or enemy dies if win or loss conditions have been met
                Debug.Log("checking alive");

                if (HeroesInBattle.Count < 1)
                {
                    battleState = battleStates.LOSE;
                    //lose game
                } else if (EnemiesInBattle.Count < 1)
                {
                    battleState = battleStates.WIN;
                    //win the battle
                } else
                {
                    ClearAttackPanel(); //resets/clears enemySelect, actionPanel, magicPanel, and attack buttons
                    HeroInput = HeroGUI.ACTIVATE;
                    battleState = battleStates.WAIT;
                }
            break;

            case (battleStates.LOSE): //lose battle
                Debug.Log("Game lost"); //things to go here later - retry battle, go back to world map, load from save
            break;

            case (battleStates.WIN): //win battle
                StartCoroutine(RunVictory());
                                
            break;
        }

        switch (HeroInput) //phases of hero inputs when hero's turn is available
        {
            case (HeroGUI.ACTIVATE): //hero's turn is available
                if (HeroesToManage.Count > 0) //if there is a hero's turn available (ATB gauge filled up and pending input)
                {
                    ShowSelector(HeroesToManage[0].transform.Find("Selector").gameObject); //Show hero's selector cursor
                    HeroChoice = new HandleTurn(); //new handle turn instance as HeroChoice

                    ShowPanel(moveActionPanel);
                    CreateMoveActionButtons();
                    CreateActionButtons(); //populate action buttons

                    SetCancelButton(0); //Cancel button does nothing
                    
                    foreach (GameObject enObj in EnemiesInBattle)
                    {
                        EnemyBehavior eb = enObj.GetComponent<EnemyBehavior>();
                        eb.DrawThreatBar();
                    }

                    HeroInput = HeroGUI.WAITING;
                }
            break;

            case (HeroGUI.WAITING):
                //idle state
            break;

            case (HeroGUI.DONE):
                ClearActionLists();

                ClearThreatBars();

                HeroInputDone();
            break;

        }

        GetCancelButton();
    }

    /// <summary>
    /// Used by enemy state machine to add enemy's chosen attack to the perform list
    /// </summary>
    /// <param name="input">Enemy's chosen attack to add to the perform list</param>
    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }

    /// <summary>
    /// Procedure for finishing hero's input. (Adds choice to perform list, clears attack panel, hides selector, removes from HeroesToManage, and places HeroInput to ACTIVATE)
    /// </summary>
    void HeroInputDone() 
    {
        PerformList.Add(HeroChoice); //adds the details of the current hero making selection's choice to the perform list
        ClearAttackPanel(); //cleans the attackpanel
        HideSelector(HeroesToManage[0].transform.Find("Selector").gameObject); //hides the current hero making selection's selector cursor
        HeroesToManage.RemoveAt(0); //removes the hero making selection from the heroesToManage list
        HeroInput = HeroGUI.ACTIVATE; //resets the HeroGUI switch back to the beginning to await the next hero's choice
    }

    /// <summary>
    /// Hides enemySelect, action, and magic panels, then clears attack choice buttons
    /// </summary>
    void ClearAttackPanel()
    {
        HidePanel(enemySelectPanel);
        HidePanel(actionPanel);
        HidePanel(magicPanel);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    /// <summary>
    /// Hides move action panel and clears attack buttons
    /// </summary>
    void ClearMoveActionPanel()
    {
        HidePanel(moveActionPanel);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    /// <summary>
    /// Hides action panel and clears attack buttons
    /// </summary>
    void ClearActionPanel()
    {
        HidePanel(actionPanel);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    /// <summary>
    /// Insantiates action and defend button for when movement phase is complete
    /// </summary>
    void CreateMoveActionButtons()
    {
        //Create Action button
        GameObject ActionButton = Instantiate(actionButton) as GameObject; //instantiates prefab assigned to actionButton as a gameobject for attack command
        ActionButton.name = "ActionButton";
        Text ActionButtonText = ActionButton.transform.Find("Text").gameObject.GetComponent<Text>(); //initializes text object as the text attached to above actionButton
        ActionButtonText.text = "Action"; //changes the text of the actionButton to 'Attack'
        ActionButton.GetComponent<Button>().onClick.AddListener(() => ActionInput()); //assigns action when clicking the button to AttackInput() function
        ActionButton.transform.SetParent(moveActionSpacer, false); //sets the parent of this button to the action panel spacer
        atkBtns.Add(ActionButton); //adds the action button to atkBtns list for organizational purposes

        //Create Defend button
        GameObject DefendButton = Instantiate(actionButton) as GameObject; //instantiates prefab assigned to actionButton as a gameobject for magic command
        DefendButton.name = "DefendButton";
        Text MagicActionButtonText = DefendButton.transform.Find("Text").gameObject.GetComponent<Text>(); //initializes text object as the text attached to above actionButton
        MagicActionButtonText.text = "Defend"; //changes the text of the actionButton to 'Magic'
        DefendButton.GetComponent<Button>().onClick.AddListener(() => DefendInput()); //assigns action when clicking the button to MagicInput() function
        DefendButton.transform.SetParent(moveActionSpacer, false); //sets the parent of this button to the action panel spacer
        atkBtns.Add(DefendButton); //adds the magic button to atkBtns list for organizational purposes
    }

    /// <summary>
    /// Enables action buttons depending on what actions are available (with enemies/heroes in range taken into account)
    /// </summary>
    void GetButtonsByRange()
    {
        Transform[] actionSpacerChildren = actionSpacer.GetComponentsInChildren<Transform>();
        //Transform[] magicSpacerChildren = magicSpacer.GetComponentsInChildren<Transform>();
        //Transform[] itemSpacerChildren = itemSpacer.GetComponentsInChildren<Transform>();

        foreach (Transform child in actionSpacerChildren)
        {
            if (child.gameObject.name == "AttackButton") //add the other buttons later -  || child.gameObject.name == "MagicAttackButton" || child.gameObject.name == "ItemActionButton"
            {
                DisableButton(child.gameObject);
            }
        }
        /*
        foreach (Transform child in magicSpacerChildren)
        {
            DisableButton(child.gameObject);
        }
        foreach (Transform child in itemSpacerChildren)
        {
            DisableButton(child.gameObject);
        }*/

        BuildActionLists();

        if (attacksWithinRange.Count > 0)
        {
            foreach (BaseAttack attack in attacksWithinRange)
            {
                if (attack.type == BaseAttack.Type.PHYSICAL)
                {
                    EnableButton(GameObject.Find("ActionSpacer/AttackButton"));
                }
            }
        }
    }

    /// <summary>
    /// Generates attacks that hero has MP for
    /// </summary>
    void BuildActionLists()
    {
        ClearActionLists();

        //check which attacks are available based on MP cost of all attacks, and enemy's current MP, and adds them to 'attacksWithinMPThreshold' list.
        foreach (BaseAttack atk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks)
        {
            if (atk.MPCost <= HeroesToManage[0].GetComponent<HeroStateMachine>().hero.curMP)
            {
                attacksWithinMPThreshold.Add(atk);
            }
        }

        foreach (GameObject hero in HeroesInBattle)
        {
            if (hero != HeroesToManage[0]) //ignores current hero
            {
                //Get lowest range
                if (lowestRangeForAttack == 0)
                {
                    lowestRangeForAttack = GetRangeFromTarget(hero);
                }
                else
                {
                    int range = GetRangeFromTarget(hero);
                    if (range < lowestRangeForAttack)
                    {
                        lowestRangeForAttack = range;
                    }
                }
            }
        }

        foreach (GameObject enemy in EnemiesInBattle)
        {
            //Get lowest range
            if (lowestRangeForAttack == 0)
            {
                lowestRangeForAttack = GetRangeFromTarget(enemy);
            }
            else
            {
                int range = GetRangeFromTarget(enemy);
                if (range < lowestRangeForAttack)
                {
                    lowestRangeForAttack = range;
                }
            }
        }

        foreach (BaseAttack attackInRange in attacksWithinMPThreshold)
        {
            if (lowestRangeForAttack <= attackInRange.rangeIndex)
            {
                attacksWithinRange.Add(attackInRange);
            }
        }
    }

    /// <summary>
    /// Clears action list for next enemy to use
    /// </summary>
    void ClearActionLists()
    {
        attacksWithinMPThreshold.Clear();
        attacksWithinRange.Clear();
        lowestRangeForAttack = 0;
        foreach (Tile tile in tilesInRange)
        {
            tile.inRange = false;
        }
        tilesInRange.Clear();
    }

    /// <summary>
    /// Calls ClearThreatBar for each EnemyBehavior in EnemiesInBattle
    /// </summary>
    void ClearThreatBars()
    {
        foreach (GameObject enObj in EnemiesInBattle)
        {
            EnemyBehavior eb = enObj.GetComponent<EnemyBehavior>();
            eb.ClearThreatBar();
        }
    }

    /// <summary>
    /// Returns range from current hero to the target
    /// </summary>
    /// <param name="target">Target to check how many tiles away</param>
    int GetRangeFromTarget(GameObject target)
    {
        HeroesToManage[0].GetComponent<PlayerMove>().GetCurrentTile();

        Tile targetTile = HeroesToManage[0].GetComponent<PlayerMove>().GetTargetTile(target);

        float x = Mathf.Abs(HeroesToManage[0].GetComponent<PlayerMove>().currentTile.transform.position.x - targetTile.transform.position.x);
        float y = Mathf.Abs(HeroesToManage[0].GetComponent<PlayerMove>().currentTile.transform.position.y - targetTile.transform.position.y);

        //Debug.Log("GetRangeFromTarget " + target.name + " - Distance: " + x + "," + y);

        int range = Mathf.RoundToInt(x) + Mathf.RoundToInt(y);
        //Debug.Log("Range from target " + target.name + ": " + range);

        return range;
    }

    /// <summary>
    /// Returns coordinates of given tile
    /// </summary>
    /// <param name="tile">Tile to return coordinates for</param>
    string[] GetTileCoordinates(Tile tile)
    {
        string tileCoords = tile.gameObject.name.Replace("Tile (", "");
        tileCoords = tileCoords.Replace(")", "");
        string[] coords = tileCoords.Split(',');
        return coords;
    }

    /// <summary>
    /// Enables given button by setting button component to 'interactable = true'
    /// </summary>
    /// <param name="button">Button to enable</param>
    void EnableButton(GameObject button)
    {
        //Debug.Log("enabling " + button.name);
        button.GetComponent<Button>().interactable = true;
    }

    /// <summary>
    /// Disables given button by setting button component to 'interactable = false'
    /// </summary>
    /// <param name="button">Button to disable</param>
    void DisableButton(GameObject button)
    {
        //Debug.Log("disabling" + button.name);
        button.GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// Instantiates action buttons (attack, magic, item)
    /// </summary>
    void CreateActionButtons()
    {
        //Create attack button
        GameObject AttackButton = Instantiate(actionButton) as GameObject; //instantiates prefab assigned to actionButton as a gameobject for attack command
        AttackButton.name = "AttackButton";
        Text ActionButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>(); //initializes text object as the text attached to above actionButton
        ActionButtonText.text = "Attack"; //changes the text of the actionButton to 'Attack'
        AttackButton.GetComponent<Button>().onClick.AddListener(() => AttackInput()); //assigns action when clicking the button to AttackInput() function
        AttackButton.transform.SetParent(actionSpacer, false); //sets the parent of this button to the action panel spacer
        atkBtns.Add(AttackButton); //adds the action button to atkBtns list for organizational purposes

        //Create magic button
        GameObject MagicAttackButton = Instantiate(actionButton) as GameObject; //instantiates prefab assigned to actionButton as a gameobject for magic command
        MagicAttackButton.name = "MagicAttackButton";
        Text MagicActionButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>(); //initializes text object as the text attached to above actionButton
        MagicActionButtonText.text = "Magic"; //changes the text of the actionButton to 'Magic'
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => MagicInput()); //assigns action when clicking the button to MagicInput() function
        MagicAttackButton.transform.SetParent(actionSpacer, false); //sets the parent of this button to the action panel spacer
        atkBtns.Add(MagicAttackButton); //adds the magic button to atkBtns list for organizational purposes

        //create item button
        GameObject ItemActionButton = Instantiate(actionButton) as GameObject; //instantiates prefab assigned to actionButton as a gameobject for item command
        ItemActionButton.name = "ItemActionButton";
        Text ItemActionButtonText = ItemActionButton.transform.Find("Text").gameObject.GetComponent<Text>(); //initializes text object as the text attached to above actionButton
        ItemActionButtonText.text = "Item"; //changes the text of the actionButton to 'Item'
        ItemActionButton.GetComponent<Button>().onClick.AddListener(() => ItemInput()); //assigns action when clicking the button to ItemInput() function
        ItemActionButton.transform.SetParent(actionSpacer, false); //sets the parent of this button to the action panel spacer
        atkBtns.Add(ItemActionButton); //adds the item button to atkBtns list for organizational purposes

        CreateMagicButtons(MagicAttackButton); //creates magic buttons for each spell that hero can cast
        CreateItemButtons(); //creates item buttons from items in inventory
    }

    /// <summary>
    /// Instantiates magic buttons for each magic attack for current hero to manage
    /// </summary>
    void CreateMagicButtons(GameObject MagicActionButton)
    {
        if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks.Count > 0) //if the current hero has any magic attacks
        {
            foreach (BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks) //for each magic attack
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject; //instantiate prefab assigned to magicButton as a gameobject for selecting which magic to use after clicking magic in action panel
                Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>(); //initializes text object as the text attached to above magicButton
                MagicButtonText.text = magicAtk.name; //changes the text of the magicButton to the current magic attack in the loop
                MagicAttackButton MATB = MagicButton.GetComponent<MagicAttackButton>(); //initializes Magic Attack Button script from the Magic Attack Button assigned to the magic button prefab
                MATB.magicAttackToPerform = magicAtk; //sets the magic attack button's attack to perform to the current magic attack in the loop
                MagicButton.transform.SetParent(magicSpacer, false); //sets the parent of this button to the magic panel spacer
                if (magicAtk.MPCost > HeroesToManage[0].GetComponent<HeroStateMachine>().hero.curMP)
                {
                    MagicButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    MagicButton.GetComponent<Button>().interactable = true;
                }
                atkBtns.Add(MagicButton); //adds the magic button to atkBtns list for organizational purposes
            }
        }
        else //if the current hero does not have any magic attacks
        {
            MagicActionButton.GetComponent<Button>().interactable = false; //keeps the magic attack button from being available. could also hide it. will likely change this in the future
        }
    }

    /// <summary>
    /// Called after choosing a magic attack
    /// </summary>
    /// <param name="chosenMagic">BaseAttack that is chosen to be cast</param>
    public void SetChosenMagic(BaseAttack chosenMagic)
    {
        HeroChoice.Attacker = HeroesToManage[0].name; //sets the hero choice attacker to the current hero who selected magic
        HeroChoice.AttackersGameObject = HeroesToManage[0]; //sets the hero choice attacker's game object to the current hero
        HeroChoice.attackerType = Types.HERO; //as hero choice is a HandleTurn, sets the type to Hero

        HeroChoice.chosenAttack = chosenMagic; //sets the hero choice's chosen attack to the chosenMagic in the parameter
        HeroChoice.actionType = ActionType.MAGIC;

        HidePanel(magicPanel); //hides the magic panel
        ShowPanel(enemySelectPanel); //opens the enemy select panel
        StartCoroutine(ChooseTarget()); //select which gameObject to be targetted
        SetCancelButton(3);
    }

    /// <summary>
    /// Displays all tiles in given range index
    /// </summary>
    /// <param name="rangeIndex">Given range index from attack to display tiles</param>
    void ShowTilesInRange(int rangeIndex)
    {
        RaycastHit2D[] tileHits = Physics2D.RaycastAll(HeroesToManage[0].transform.position, Vector3.back, 1);
        
        foreach (RaycastHit2D tile in tileHits)
        {
            if (tile.collider.gameObject.tag == "Tile")
            {
                Tile t = tile.collider.gameObject.GetComponent<Tile>();
                pattern.GetRangePattern(t, rangeIndex);
                tilesInRange = pattern.pattern;
                break;
            }
        }

        foreach(Tile tile in tilesInRange)
        {
            tile.inRange = true;
        }

        /*HeroesToManage[0].GetComponent<PlayerMove>().GetCurrentTile();
        Tile currentTile = HeroesToManage[0].GetComponent<PlayerMove>().currentTile;
        string[] tileCoordinates = GetTileCoordinates(currentTile);
        int currentTileX = int.Parse(tileCoordinates[0]);
        int currentTileY = int.Parse(tileCoordinates[1]);*/
        //Debug.Log(currentTileX);
        //Debug.Log(currentTileY);
    }

    /// <summary>
    /// Shows range for chosen attack's range index (or 1 space for items)
    /// </summary>
    void ShowRange()
    {
        if (HeroChoice.chosenItem != null)
        {
            ShowTilesInRange(0); //shows tiles 1 space away, and current tile
        } else
        {
            ShowTilesInRange(HeroChoice.chosenAttack.rangeIndex);
        }
    }

    /// <summary>
    /// Coroutine.  Sets current hero to manage target to whichever gameObject is clicked (enemy or hero)
    /// </summary>
    IEnumerator ChooseTarget()
    {
        ShowRange();

        choosingTarget = true; //to ensure clicking on gameObjects are only processed when choosing target - used by Tile script as tiles are the clicked object
        //Debug.Log(choosingTarget);

        while (HeroesToManage[0].GetComponent<HeroStateMachine>().targets.Count == 0)
        {
            if (cancelledEnemySelect) //if choosing enemy is cancelled
            {
                //Debug.Log("cancelled enemy select");
                GameObject.Find("GameManager/BGS").GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.backSE);
                choosingTarget = false; //disables ability to click/hover gameObjects
                cancelledEnemySelect = false; //resets cancelledEnemySelect

                GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile"); //clear all tiles
                foreach (GameObject tile in tiles)
                {
                    tile.GetComponent<Tile>().Reset();
                }

                yield break;
            }
            yield return null;
        }

        GameObject.Find("GameManager/BGS").GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.confirmSE);

        //HeroChoice.AttackersTarget = chosenEnemy;
        //HeroChoice.targetType = HandleTurn.Types.ENEMY;
        battleDetailsPanel.transform.Find("BattleDetailsText").GetComponent<Text>().text = "";
        HeroInput = HeroGUI.DONE;

        choosingTarget = false;
    }

    /// <summary>
    /// When enemy is selected - sets hero choice target type to enemy
    /// </summary>
    public void EnemySelection(GameObject chosenEnemy)
    {
        if (choosingTarget)
        {
            HeroChoice.AttackersTarget = chosenEnemy;
            HeroChoice.targetType = Types.ENEMY;
            battleDetailsPanel.transform.Find("BattleDetailsText").GetComponent<Text>().text = "";
            HeroInput = HeroGUI.DONE;
        }
    }

    /// <summary>
    /// When hero is selected - sets hero choice to hero
    /// </summary>
    public void HeroSelection(GameObject chosenHero)
    {
        if (choosingTarget)
        {
            HeroChoice.AttackersTarget = chosenHero;
            HeroChoice.targetType = Types.HERO;
            HeroInput = HeroGUI.DONE;
        }
    }

    /// <summary>
    /// Creates item buttons for each item in inventory (to click after choosing 'item' option)
    /// </summary>
    public void CreateItemButtons()
    {
        List<Item> itemsAccountedFor = new List<Item>();

        foreach (Item item in Inventory.instance.items) //for each item in inventory
        {
            int itemCount = 0;
            if (!itemsAccountedFor.Contains(item)) //if item has not already been added to list
            {
                for (int i = 0; i < Inventory.instance.items.Count; i++)
                {
                    if (Inventory.instance.items[i] == item) //to set how many of the same item are in inventory
                    {
                        itemCount++;
                    }
                }
                GameObject ItemButton = Instantiate(itemButton) as GameObject; //instantiate prefab assigned to itemButton as a gameobject for selecting which item to use after clicking item in action panel
                Text ItemButtonText = ItemButton.transform.Find("Text").gameObject.GetComponent<Text>(); //initializes text object as the text attached to above itemButton
                Text ItemButtonQuantity = ItemButton.transform.Find("Quantity").gameObject.GetComponent<Text>();
                ItemButtonText.text = item.name; //changes the text of the itemButton to the current item in the loop
                ItemButtonQuantity.text = itemCount.ToString();
                ItemBattleMenuButton ItemToUseButton = ItemButton.GetComponent<ItemBattleMenuButton>(); //initializes Item Button script
                ItemToUseButton.itemToUse = item;
                ItemButton.transform.SetParent(itemSpacer, false); //sets the parent of this button to the item panel spacer

                atkBtns.Add(ItemButton); //adds the item button to atkBtns list for organizational purposes

                itemsAccountedFor.Add(item);

                if (!item.usableInMenu)
                {
                    ItemButton.transform.GetChild(0).GetComponent<Text>().color = Color.gray;
                    ItemButton.transform.GetChild(1).GetComponent<Text>().color = Color.gray;
                }
            }
        }

        itemsAccountedFor.Clear();
    }

    /// <summary>
    /// Clears items in item list so it can be rebuilt
    /// </summary>
    public void ResetItemList()
    {
        ShowPanel(itemPanel);
        foreach (Transform child in GameObject.Find("BattleCanvas/BattleUI/ItemPanel/ItemScroller/ItemSpacer").transform)
        {
            Destroy(child.gameObject);
        }
        HidePanel(itemPanel);
    }

    /// <summary>
    /// Called after choosing an item - sets hero choice chosen item to the item button clicked
    /// </summary>
    /// <param name="chosenItem">Item to be set to chosen item</param>
    public void SetChosenItem(Item chosenItem)
    {
        HeroChoice.Attacker = HeroesToManage[0].name; //sets the hero choice attacker to the current hero who selected magic
        HeroChoice.AttackersGameObject = HeroesToManage[0]; //sets the hero choice attacker's game object to the current hero
        HeroChoice.attackerType = Types.HERO; //as hero choice is a HandleTurn, sets the type to Hero

        HeroChoice.chosenItem = chosenItem; //sets the hero choice's chosen attack to the chosenMagic in the parameter
        HeroChoice.actionType = ActionType.ITEM;

        HidePanel(itemPanel); //hides the magic panel
        ShowPanel(enemySelectPanel); //opens the enemy select panel
        StartCoroutine(ChooseTarget());
        SetCancelButton(5);
    }

    /// <summary>
    /// Displays action panel and disables moveable tiles
    /// </summary>
    public void ActionInput()
    {
        playerMove = HeroesToManage[0].GetComponent<PlayerMove>();

        if (playerMove.canChooseAction)
        {
            SetCancelButton(0);
            ShowPanel(actionPanel); //enables actionPanel (attack, magic, etc)
            HidePanel(moveActionPanel);
            //GetButtonsByRange();
            playerMove.RemoveSelectableTiles();
            playerMove.canMove = false;
        }
    }

    /// <summary>
    /// Input processing for defend (method still need to be implemented to reduce incoming damage by 50%)
    /// </summary>
    public void DefendInput()
    {
        playerMove = HeroesToManage[0].GetComponent<PlayerMove>();

        if (playerMove.canChooseAction)
        {
            ClearMoveActionPanel();
            ClearActionLists();
            ClearThreatBars();

            HeroStateMachine HSM = HeroesToManage[0].GetComponent<HeroStateMachine>();

            //HSM.ProcessStatusEffects();

            //HSM.RecoverMPAfterTurn(); //slowly recover MP based on spirit value

            pendingTurn = false;

            HSM.heroTurn++;
            HSM.cur_cooldown = 0f; //reset the hero's ATB gauge to 0
            HeroesToManage[0].GetComponent<PlayerMove>().EndTurn();

            if (HSM.activeStatusEffects.Count > 0)
            {
                HSM.ProcessStatusEffects();
            }

            HSM.currentState = HeroStateMachine.TurnState.PROCESSING; //starts the turn over back to filling up the ATB gauge
            HideSelector(HeroesToManage[0].transform.Find("Selector").gameObject); //hides the current hero making selection's selector cursor
            HeroesToManage.RemoveAt(0); //removes the hero making selection from the heroesToManage list
            HeroInput = HeroGUI.ACTIVATE; //resets the HeroGUI switch back to the beginning to await the next hero's choice
        }       
    }

    /// <summary>
    /// When clicking 'attack' in action panel
    /// </summary>
    public void AttackInput()
    {
        HeroChoice.Attacker = HeroesToManage[0].name; //sets heroChoice attacker to current hero making selection
        HeroChoice.AttackersGameObject = HeroesToManage[0]; //sets heroChoice attacker's game object to the current hero making selection's game object
        HeroChoice.attackerType = Types.HERO; //as HeroChoice is of class HandleTurn, sets type to Hero
        HeroChoice.chosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attack; //sets heroChoice chosen attack to the current hero's hero state machine to the attack at top of their attack list (likely change later)
        HidePanel(actionPanel); //hides attack panel as action has been chosen
        ShowPanel(enemySelectPanel); //displays enemy select panel to process chosen attack to
        StartCoroutine(ChooseTarget());
        SetCancelButton(1);
    }

    /// <summary>
    /// After clicking 'magic' on the action panel
    /// </summary>
    public void MagicInput()
    {
        HidePanel(actionPanel); //hides the action panel
        ShowPanel(magicPanel); //displays the magic panel showing current heros magic attacks
        SetCancelButton(2);
    }

    /// <summary>
    /// After clicking 'item' on the action panel
    /// </summary>
    public void ItemInput() 
    {
        HidePanel(actionPanel); //hides the action panel
        ShowPanel(itemPanel); //displays the magic panel showing current items in inventory
        SetCancelButton(4);
    }

    /// <summary>
    /// Setting the bools for hitting the cancel button so the functionality changes depending on which menu the player is in
    /// </summary>
    /// <param name="option">Which button was pressed for processing cancel</param>
    void SetCancelButton(int option) 
    {
        if (option == 0) //in action menu
        {
            enemySelectAfterAttackCancel = false;
            magicAttackCancel = false;
            enemySelectAfterMagicCancel = false;
            itemBattleMenuCancel = false;
            enemySelectAfterItemMenuCancel = false;
            moveMenuCancel = true;
        }

        if (option == 1) //in enemy select menu after choosing attack
        {
            enemySelectAfterAttackCancel = true;
            magicAttackCancel = false;
            enemySelectAfterMagicCancel = false;
            itemBattleMenuCancel = false;
            enemySelectAfterItemMenuCancel = false;
            moveMenuCancel = false;
        }

        if (option == 2) //in magic select menu
        {
            enemySelectAfterAttackCancel = false;
            magicAttackCancel = true;
            enemySelectAfterMagicCancel = false;
            itemBattleMenuCancel = false;
            enemySelectAfterItemMenuCancel = false;
            moveMenuCancel = false;
        }

        if (option == 3) //in enemy select menu after choosing magic
        {
            enemySelectAfterAttackCancel = false;
            magicAttackCancel = false;
            enemySelectAfterMagicCancel = true;
            itemBattleMenuCancel = false;
            enemySelectAfterItemMenuCancel = false;
            moveMenuCancel = false;
        }

        if (option == 4) //in item select menu
        {
            enemySelectAfterAttackCancel = false;
            magicAttackCancel = false;
            enemySelectAfterMagicCancel = false;
            itemBattleMenuCancel = true;
            enemySelectAfterItemMenuCancel = false;
            moveMenuCancel = false;
        }

        if (option == 5) //in enemy select menu after choosing item
        {
            enemySelectAfterAttackCancel = false;
            magicAttackCancel = false;
            enemySelectAfterMagicCancel = false;
            itemBattleMenuCancel = false;
            enemySelectAfterItemMenuCancel = true;
            moveMenuCancel = false;
        }

        if (option == 6) //in move menu
        {
            enemySelectAfterAttackCancel = false;
            magicAttackCancel = false;
            enemySelectAfterMagicCancel = false;
            itemBattleMenuCancel = false;
            enemySelectAfterItemMenuCancel = false;
            moveMenuCancel = false;
        }
    }

    /// <summary>
    /// Detects if Cancel button is hit, and moves 'back' in the menus
    /// </summary>
    void GetCancelButton() 
    {
        if (Input.GetButtonDown("Cancel") && !buttonPressed)
        {
            GameObject.Find("GameManager/BGS").GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.backSE);
            if (moveMenuCancel) //in movement phase
            {
                HidePanel(actionPanel);
                ShowPanel(moveActionPanel);
                SetCancelButton(6);

                HeroesToManage[0].GetComponent<PlayerMove>().FindSelectableTiles();
                HeroesToManage[0].GetComponent<PlayerMove>().canMove = true;
            }
            if (enemySelectAfterAttackCancel)
            {
                ShowPanel(actionPanel); //shows action panel
                HidePanel(moveActionPanel);
                HidePanel(enemySelectPanel); //hides enemy select panel
                cancelledEnemySelect = true;
                SetCancelButton(0);
                ClearActionLists();
            }
            if (magicAttackCancel)
            {
                ShowPanel(actionPanel); //shows the action panel
                HidePanel(magicPanel); //hides the magic panel showing current heros magic attacks
                SetCancelButton(0);
            }
            if (enemySelectAfterMagicCancel)
            {
                ShowPanel(magicPanel); //shows the magic panel
                cancelledEnemySelect = true;
                HidePanel(enemySelectPanel); //hides the enemy select panel
                SetCancelButton(2);
            }
            if (itemBattleMenuCancel)
            {
                ShowPanel(actionPanel); //shows the action panel
                HidePanel(itemPanel);
                SetCancelButton(0);
            }
            if (enemySelectAfterItemMenuCancel)
            {
                ShowPanel(itemPanel); //shows the item panel
                HidePanel(enemySelectPanel); //hides the enemy select panel
                HeroChoice.chosenItem = null;
                cancelledEnemySelect = true;
                SetCancelButton(4);
            }
        }
        CheckCancelPressed();
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

    /// <summary>
    /// Shows panels depending on how many heroes are in party
    /// </summary>
    /// <param name="count">count of heroes in party</param>
    void DrawHeroPanels(int count)
    {
        if (count == 1)
        {
            ShowPanel(hero1Panel);
            HidePanel(hero2Panel);
            HidePanel(hero3Panel);
            HidePanel(hero4Panel);
            HidePanel(hero5Panel);
        }
        else if (count == 2)
        {
            ShowPanel(hero1Panel);
            ShowPanel(hero2Panel);
            HidePanel(hero3Panel);
            HidePanel(hero4Panel);
            HidePanel(hero5Panel);
        }
        else if (count == 3)
        {
            ShowPanel(hero1Panel);
            ShowPanel(hero2Panel);
            ShowPanel(hero3Panel);
            HidePanel(hero4Panel);
            HidePanel(hero5Panel);
        }
        else if (count == 4)
        {
            ShowPanel(hero1Panel);
            ShowPanel(hero2Panel);
            ShowPanel(hero3Panel);
            ShowPanel(hero4Panel);
            HidePanel(hero5Panel);
        }
        else if (count == 5)
        {
            ShowPanel(hero1Panel);
            ShowPanel(hero2Panel);
            ShowPanel(hero3Panel);
            ShowPanel(hero4Panel);
            ShowPanel(hero5Panel);
        }

        DrawHeroPanelBars();
    }

    /// <summary>
    /// Draws EXP progress bars for all active heroes in party
    /// </summary>
    void DrawHeroPanelBars()
    {
        if (GameManager.instance.activeHeroes.Count == 1)
        {
            hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), hero1EXPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 2)
        {
            hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), hero1EXPProgressBar.transform.localScale.y);

            hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), hero2EXPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 3)
        {
            hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), hero1EXPProgressBar.transform.localScale.y);

            hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), hero2EXPProgressBar.transform.localScale.y);

            hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), hero3EXPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 4)
        {
            hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), hero1EXPProgressBar.transform.localScale.y);

            hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), hero2EXPProgressBar.transform.localScale.y);

            hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), hero3EXPProgressBar.transform.localScale.y);

            hero4EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[3]), 0, 1), hero4EXPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 5)
        {
            hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), hero1EXPProgressBar.transform.localScale.y);

            hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), hero2EXPProgressBar.transform.localScale.y);

            hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), hero3EXPProgressBar.transform.localScale.y);

            hero4EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[3]), 0, 1), hero4EXPProgressBar.transform.localScale.y);

            hero5EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[4]), 0, 1), hero5EXPProgressBar.transform.localScale.y);
        }

    }

    /// <summary>
    /// Coroutine.  Displays damage above GameObject to visually show the health thats been lost after an attack
    /// </summary>
    /// <param name="damage">Damage value to be displayed</param>
    /// <param name="target">GameObject for damage to be displayed above</param>
    public IEnumerator ShowDamage(int damage, GameObject target)
    {
        PauseATBWhileDamageFinishes(true);
        Debug.Log("ShowDamage: " + damage + ", " + target.name);
        damageText = Instantiate(PrefabManager.Instance.damagePrefab);
        damageText.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + damageTextDistance);
        damageText.transform.GetComponent<TextMeshPro>().color = Color.red;
        damageText.transform.GetComponent<TextMeshPro>().text = damage.ToString();
        
        damageText.gameObject.tag = "DamageText";

        yield return new WaitForSeconds(damageDisplayTime);
        PauseATBWhileDamageFinishes(false);
        //Destroy(damageText);
        DestroyDamageTexts();

        if (target != null)
        target.GetComponent<Animator>().SetBool("onRcvDam", false);
    }

    /// <summary>
    /// Coroutine.  Displays colored damage above GameObject to visually show the health thats been lost after a magic attack
    /// </summary>
    /// <param name="damage">Damage to be displayed</param>
    /// <param name="target">GameObject for damage to be displayed above</param>
    /// <param name="color">Color of font to be displayed</param>
    public IEnumerator ShowElementalDamage(int damage, GameObject target, Color color)
    {
        PauseATBWhileDamageFinishes(true);
        Debug.Log("ShowElementalDamage: " + damage + ", " + target.name + ", Color: " + color.ToString());
        damageText = Instantiate(PrefabManager.Instance.damagePrefab);
        damageText.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + damageTextDistance);
        damageText.transform.GetComponent<TextMeshPro>().color = color;
        damageText.transform.GetComponent<TextMeshPro>().text = damage.ToString();

        damageText.gameObject.tag = "DamageText";

        yield return new WaitForSeconds(damageDisplayTime);
        PauseATBWhileDamageFinishes(false);
        //Destroy(damageText);
        DestroyDamageTexts();

        if (target != null)
        target.GetComponent<Animator>().SetBool("onRcvDam", false);
    }

    /// <summary>
    /// Coroutine.  Displays critical damage above GameObject to visually show the health thats been lost after a critical attack
    /// </summary>
    /// <param name="damage">Damage to be displayed</param>
    /// <param name="target">GameObject for damage to be displayed above</param>
    public IEnumerator ShowCrit(int damage, GameObject target)
    {
        PauseATBWhileDamageFinishes(true);
        Debug.Log("ShowCritDamage: " + damage + ", " + target.name);
        damageText = Instantiate(PrefabManager.Instance.damagePrefab);
        damageText.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + damageTextDistance);
        damageText.transform.GetComponent<TextMeshPro>().color = Color.red;
        damageText.transform.GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
        damageText.transform.GetComponent<TextMeshPro>().fontSize = 8f;
        damageText.transform.GetComponent<TextMeshPro>().text = damage.ToString();

        damageText.gameObject.tag = "DamageText";

        yield return new WaitForSeconds(damageDisplayTime);
        PauseATBWhileDamageFinishes(false);
        //Destroy(damageText);
        DestroyDamageTexts();

        if (target != null)
        target.GetComponent<Animator>().SetBool("onRcvDam", false);
    }

    /// <summary>
    /// Coroutine.  Displays heal value to visually show the health thats been gained after a heal
    /// </summary>
    /// <param name="healVal">Heal value to be displayed</param>
    /// <param name="target">GameObject for heal to be displayed above</param>
    public IEnumerator ShowHeal(int healVal, GameObject target)
    {
        PauseATBWhileDamageFinishes(true);
        Debug.Log("ShowHeal: " + healVal + ", " + target.name);
        damageText = Instantiate(PrefabManager.Instance.damagePrefab);
        damageText.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + damageTextDistance);
        damageText.transform.GetComponent<TextMeshPro>().color = Color.blue;
        damageText.transform.GetComponent<TextMeshPro>().text = healVal.ToString();

        damageText.gameObject.tag = "DamageText";

        yield return new WaitForSeconds(damageDisplayTime);
        PauseATBWhileDamageFinishes(false);
        //Destroy(damageText);
        DestroyDamageTexts();
    }

    /// <summary>
    /// Causes damage text to disappear
    /// </summary>
    void DestroyDamageTexts()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("DamageText");
        foreach (GameObject obj in toDestroy)
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// Coroutine.  Displays miss text to visually show that the action has missed the target
    /// </summary>
    /// <param name="target">GameObject for the text to be displayed above</param>
    public IEnumerator ShowMiss(GameObject target)
    {
        PauseATBWhileDamageFinishes(true);
        damageText = Instantiate(PrefabManager.Instance.damagePrefab);
        damageText.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + damageTextDistance);
        damageText.transform.GetComponent<TextMeshPro>().color = Color.white;
        damageText.transform.GetComponent<TextMeshPro>().text = "Miss";
        AudioManager.instance.PlaySE(AudioManager.instance.attackMiss);

        yield return new WaitForSeconds(damageDisplayTime);
        PauseATBWhileDamageFinishes(false);
        Destroy(damageText);
    }

    /// <summary>
    /// Pauses ATB for heroes and enemies in battle while damage is displayed
    /// </summary>
    /// <param name="pause">True if ATBs should be paused</param>
    void PauseATBWhileDamageFinishes(bool pause)
    {
        if (pause)
        {
            foreach (GameObject obj in HeroesInBattle)
            {
                obj.GetComponent<HeroStateMachine>().waitForDamageToFinish = true;
            }
            foreach (GameObject obj in EnemiesInBattle)
            {
                obj.GetComponent<EnemyStateMachine>().waitForDamageToFinish = true;
            }
        } else
        {
            foreach (GameObject obj in HeroesInBattle)
            {
                obj.GetComponent<HeroStateMachine>().waitForDamageToFinish = false;
            }
            foreach (GameObject obj in EnemiesInBattle)
            {
                obj.GetComponent<EnemyStateMachine>().waitForDamageToFinish = false;
            }
        }
    }

    /// <summary>
    /// For each active hero, draw EXP Panel details
    /// </summary>
    void DrawHeroStats()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroPanels(heroCount);

        for (int i = 0; i < heroCount; i++) //Display hero stats
        {
            DrawHeroFace(GameManager.instance.activeHeroes[i].ID, GameObject.Find("VictoryCanvas/VictoryPanel/HeroEXPPanel/Hero" + (i + 1) + "Panel").transform.Find("FacePanel").GetComponent<Image>());
            GameObject.Find("VictoryCanvas/VictoryPanel/HeroEXPPanel/Hero" + (i + 1) + "Panel").transform.Find("NameText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].name; //Name text
            GameObject.Find("VictoryCanvas/VictoryPanel/HeroEXPPanel/Hero" + (i + 1) + "Panel").transform.Find("LevelText").GetComponent<Text>().text = GameManager.instance.activeHeroes[i].currentLevel.ToString(); //Level text
            GameObject.Find("VictoryCanvas/VictoryPanel/HeroEXPPanel/Hero" + (i + 1) + "Panel").transform.Find("EXPText").GetComponent<Text>().text = (GameManager.instance.activeHeroes[i].currentExp.ToString()); //Exp text
            GameObject.Find("VictoryCanvas/VictoryPanel/HeroEXPPanel/Hero" + (i + 1) + "Panel").transform.Find("NextLevelText").GetComponent<Text>().text = (HeroDB.instance.levelEXPThresholds[(GameManager.instance.activeHeroes[i].currentLevel - 1)] - (GameManager.instance.activeHeroes[i].currentExp)).ToString(); //Exp text
        }
    }

    /// <summary>
    /// Draws hero face image based on ID and where the image should be placed
    /// </summary>
    /// <param name="ID">ID of the hero to have their face image drawn</param>
    /// <param name="faceImage">Image component for the face image to be drawn to</param>
    void DrawHeroFace(int ID, Image faceImage)
    {
        faceImage.sprite = GameManager.instance.activeHeroes[ID].faceImage;
    }

    public IEnumerator ShowAttackName(string attackName, float displayTime)
    {
        GameObject attackNamePanel = GameObject.Find("BattleCanvas/BattleUI/AttackNamePanel");

        attackNamePanel.transform.Find("AttackNameText").GetComponent<Text>().text = attackName;
        attackNamePanel.GetComponent<CanvasGroup>().alpha = 1;
        attackNamePanel.GetComponent<CanvasGroup>().interactable = true;
        attackNamePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        yield return new WaitForSeconds(displayTime);

        attackNamePanel.transform.Find("AttackNameText").GetComponent<Text>().text = "";
        attackNamePanel.GetComponent<CanvasGroup>().alpha = 0;
        attackNamePanel.GetComponent<CanvasGroup>().interactable = false;
        attackNamePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    //POST BATTLE METHODS

    /// <summary>
    /// Coroutine.  Handles all post-battle methods
    /// </summary>
    /// 
    IEnumerator RunVictory()
    {
        if (!runVictory)
        {
            AudioSource BGM = GameObject.Find("BGM").GetComponent<AudioSource>();
            BGM.Stop();
            BGM.clip = Resources.Load<AudioClip>("Music/11. Fanfare");
            BGM.Play();

            runVictory = true;

            victoryConfirmButtonPressed = false;

            DoPostBattleStuff();

            yield return ShowVictoryPoses();

            DisplayVictoryCanvas();

            ShowPostBattleGains();

            yield return TallyEXP();

            //show quests completed graphic here

            UpdateQuests();

            UpdateBestiary();

            Debug.Log("ready to leave victory screen");

            while (!victoryConfirmButtonPressed)
            {
                CheckForVictoryConfirmButtonPress();
                yield return null;
            }

            Debug.Log("Confirm pressed, leaving victory screen");

            BattleFinished();
        }
    }

    /// <summary>
    /// Handles obtaining dropped items and gold
    /// </summary>
    void DoPostBattleStuff()
    {
        HidePanel(GameObject.Find("BattleCanvas/BattleUI"));

        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (GameObject hero in heroes)
        {
            hero.GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
        }
        
        GetItemsDropped();

        GetGoldDropped();
    }

    /// <summary>
    /// If confirm button is pressed, changes victoryConfirmButtonPressed to true
    /// </summary>
    void CheckForVictoryConfirmButtonPress()
    {
        if (Input.GetButtonDown("Confirm"))
        {
            victoryConfirmButtonPressed = true;
        }
    }

    /// <summary>
    /// Coroutine.  Displays victory poses - not yet implemented
    /// </summary>
    IEnumerator ShowVictoryPoses()
    {
        victoryPoseTime = 4.8f;

        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (GameObject hero in heroes)
        {
            if (!hero.GetComponent<Animator>().GetBool("onVictory"))
            hero.GetComponent<Animator>().SetBool("onVictory", true);
        }

        yield return new WaitForSeconds(victoryPoseTime);
    }

    /// <summary>
    /// Shows victory canvas panel
    /// </summary>
    void DisplayVictoryCanvas()
    {
        ShowPanel(victoryCanvas);
    }

    /// <summary>
    /// Draws GUI elements for earned EXP/AP, items, and gold - AP not yet implemented
    /// </summary>
    void DrawEarnedStuff()
    {
        GameObject.Find("VictoryCanvas/VictoryPanel/EXPEarnedPanel/EXPText").GetComponent<Text>().text = expPool.ToString();
        GameObject.Find("VictoryCanvas/VictoryPanel/APEarnedPanel/APText").GetComponent<Text>().text = "0"; //not yet implemented
        GameObject.Find("VictoryCanvas/VictoryPanel/GoldEarnedPanel/GoldText").GetComponent<Text>().text = goldDropped.ToString();

        List<Item> itemsAccountedFor = new List<Item>();

        foreach (Item item in itemsDropped)
        {
            int itemCount = 0;
            if (!itemsAccountedFor.Contains(item))
            {
                for (int i = 0; i < itemsDropped.Count; i++)
                {
                    if (itemsDropped[i] == item)
                    {
                        itemCount++;
                    }
                }

                newItemPanel = Instantiate(PrefabManager.Instance.itemVictoryPrefab);
                newItemPanel.transform.GetChild(0).GetComponent<Text>().text = item.name;
                newItemPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
                newItemPanel.transform.GetChild(2).GetComponent<Text>().text = itemCount.ToString();
                newItemPanel.transform.SetParent(itemListSpacer, false);
                itemsAccountedFor.Add(item);
            }
        }
    }

    /// <summary>
    /// Resets GUI elements for earned EXP/AP, items, and gold
    /// </summary>
    void ClearEarnedStuff()
    {
        GameObject.Find("VictoryCanvas/VictoryPanel/EXPEarnedPanel/EXPText").GetComponent<Text>().text = "";
        GameObject.Find("VictoryCanvas/VictoryPanel/APEarnedPanel/APText").GetComponent<Text>().text = ""; //not yet implemented
        GameObject.Find("VictoryCanvas/VictoryPanel/GoldEarnedPanel/GoldText").GetComponent<Text>().text = "";

        foreach (Transform child in itemSpacer)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Adds bestiary entries for enemies from the battle if they have not yet been added
    /// </summary>
    void UpdateBestiary()
    {
        bool found;
        foreach (BaseBattleEnemy battleEnemy in GameManager.instance.enemiesToBattle)
        {
            found = false;
            foreach (BaseBestiaryEntry entry in GameManager.instance.bestiaryEntries)
            {
                if (entry.enemy == GetEnemy(battleEnemy.ID))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Debug.Log("Enemy not found, adding to bestiary");
                BaseBestiaryEntry newEntry = new BaseBestiaryEntry();
                newEntry.enemy = GetEnemy(battleEnemy.ID);
                newEntry.scanned = false; //change later when we add 'scan' ability
                GameManager.instance.bestiaryEntries.Add(newEntry);
            }
        }
    }

    /// <summary>
    /// If any active quests require enemies from the battle, quest is updated with number of killed (required) enemies
    /// </summary>
    void UpdateQuests()
    {
        foreach (BaseQuest quest in GameManager.instance.activeQuests)
        {
            if (quest.type == BaseQuest.types.KILLTARGETS)
            {
                foreach (BaseBattleEnemy battleEnemy in GameManager.instance.enemiesToBattle)
                {
                    foreach (BaseQuestKillRequirement killReq in quest.killReqs)
                    {
                        int enemyCount = 0;
                        if (killReq.enemyID == battleEnemy.ID)
                        {
                            enemyCount++;
                        }
                        killReq.targetsKilled = killReq.targetsKilled + enemyCount;
                    }
                }
            }
        }

       QuestDB.instance.UpdateQuestObjectives();
    }

    /// <summary>
    /// Draws panels for victory screen, and draws the GUI elements for earned EXP/AP and gold
    /// </summary>
    void ShowPostBattleGains()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroPanels(heroCount);
        DrawHeroStats();
        DrawEarnedStuff();
    }

    /// <summary>
    /// Coroutine. Shows EXP gained on hero panels on victory screen
    /// </summary>
    IEnumerator TallyEXP()
    {
        yield return new WaitForSeconds(1f);

        int expRemaining = expPool;
        int heroCount = GameManager.instance.activeHeroes.Count;

        Text expText = GameObject.Find("VictoryCanvas/VictoryPanel/EXPEarnedPanel/EXPText").GetComponent<Text>();

        int tempExpPool = expPool;
        while (tempExpPool > 0)
        {
            expText.text = (tempExpPool - 1).ToString();

            for (int i = 0; i < heroCount; i++)
            {
                Text heroExpText = GameObject.Find("VictoryCanvas/VictoryPanel/HeroEXPPanel/Hero" + (i + 1) + "Panel/EXPText").GetComponent<Text>();
                heroExpText.text = (int.Parse(heroExpText.text) + 1).ToString();

                Text heroNextLevelText = GameObject.Find("VictoryCanvas/VictoryPanel/HeroEXPPanel/Hero" + (i + 1) + "Panel/NextLevelText").GetComponent<Text>();
                heroNextLevelText.text = (int.Parse(heroNextLevelText.text) - 1).ToString();

                Text heroLevel = GameObject.Find("VictoryCanvas/VictoryPanel/HeroEXPPanel/Hero" + (i + 1) + "Panel/LevelText").GetComponent<Text>();

                if (heroNextLevelText.text == 0.ToString()) //Levelup
                {
                    heroLevel.text = ((int.Parse(heroLevel.text) + 1).ToString());
                    heroNextLevelText.text = (HeroDB.instance.levelEXPThresholds[(int.Parse(heroLevel.text) - 1)] - int.Parse(heroExpText.text)).ToString();
                }

                int expNeededToLevel;

                if (heroLevel.text == "1")
                {
                    expNeededToLevel = 0;
                }
                else
                {
                    expNeededToLevel = HeroDB.instance.levelEXPThresholds[int.Parse(heroLevel.text) - 2];
                }

                int tempLevel = int.Parse(heroLevel.text);
                if (i == 0)
                {
                    hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), hero1EXPProgressBar.transform.localScale.y);
                }
                else if (i == 1)
                {
                    hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), hero2EXPProgressBar.transform.localScale.y);
                }
                else if (i == 2)
                {
                    hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), hero3EXPProgressBar.transform.localScale.y);
                }
                else if (i == 3)
                {
                    hero4EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), hero4EXPProgressBar.transform.localScale.y);
                }
                else if (i == 4)
                {
                    hero5EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), hero5EXPProgressBar.transform.localScale.y);
                }
            }

            tempExpPool -= 1;

            audioSource.PlayOneShot(expTick, 0.3f);

            yield return new WaitForSeconds(expGainSpeed);
        }
    }

    /// <summary>
    /// Not yet implemented, but will display level up animation on victory screen
    /// </summary>
    void ShowLevelUp()
    {
        //show level up graphic here
        Debug.Log("Leveled up!");
    }

    /// <summary>
    /// Facilitates resetting all victory variables so they can be reused next time
    /// </summary>
    void BattleFinished()
    {
        GameManager.instance.heroesToBattle.Clear(); //clears heroesToBattle list to be generated again next battle

        //ClearEarnedStuff();

        for (int i = 0; i < HeroesInBattle.Count; i++) //for each hero in battle
        {
            GameManager.instance.activeHeroes[i].currentExp += expPool; //adds the expPool to each active hero's current exp 
            HeroesInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING; //set each hero to WAITING state
        }

        Debug.Log(expPool + " - expPool");

        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().heroToCheck = null;
        GameManager.instance.ProcessExp();

        expPool = 0; //reset exp pool
        goldDropped = 0; //reset gold dropped
        itemsDropped.Clear(); //reset items dropped list

        UpdateActiveHeroes(); //keeps hero's parameters (HP, MP, and any persistent buffs/debuffs) consistent through each battle

        GameManager.instance.LoadSceneAfterBattle(); //load scene from before battle
        GameManager.instance.gameState = GameStates.HOSTILE_STATE; //puts game manager back to hostile state
        GameManager.instance.enemiesToBattle.Clear(); //clears enemies to battle list to be used from scratch on next battle
        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = false;
    }

    /// <summary>
    /// Hides victory screen
    /// </summary>
    void HideVictoryCanvas()
    {
        HidePanel(victoryCanvas);
    }

    /// <summary>
    /// Determines which items to be dropped after battle and adds them to inventory
    /// </summary>
    void GetItemsDropped()
    {
        foreach (BaseBattleEnemy battleEnemy in GameManager.instance.enemiesToBattle)
        {
            foreach (BaseItemDrop itemDrop in GetEnemy(battleEnemy.ID).itemsDropped)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                int dropChance = Random.Range(1, 100); //get random value out of 100
                if (itemDrop.dropChance >= dropChance)
                {
                    Debug.Log(GetEnemy(battleEnemy.ID).name + " dropped " + itemDrop.item.name + " - drop chance is " + itemDrop.dropChance + ", and roll was " + dropChance);
                    Inventory.instance.Add(itemDrop.item);
                    itemsDropped.Add(itemDrop.item);
                }
                else
                {
                    Debug.Log(GetEnemy(battleEnemy.ID).name + " did not drop " + itemDrop.item.name + " - drop chance is " + itemDrop.dropChance + ", and roll was " + dropChance);
                }
            }
        }
    }

    /// <summary>
    /// Adds gold from battle to player's gold inventory
    /// </summary>
    void GetGoldDropped()
    {
        foreach (BaseBattleEnemy battleEnemy in GameManager.instance.enemiesToBattle)
        {
            Debug.Log(GetEnemy(battleEnemy.ID).name + " dropped " + GetEnemy(battleEnemy.ID).goldDropped + " gold.");
            GameManager.instance.gold += GetEnemy(battleEnemy.ID).goldDropped;
            goldDropped += GetEnemy(battleEnemy.ID).goldDropped;
        }
    }

    /// <summary>
    /// Updates game manager hero parameters upon winning the battle
    /// </summary>
    void UpdateActiveHeroes() //updates game manager hero parameters upon winning the battle
    {
        for (int i = 0; i < GameManager.instance.heroAmount; i++)
        {
            BaseHero heroToUpdate = GameManager.instance.activeHeroes[i];
            BaseHero fromHero = AllHeroesInBattle[i].GetComponent<HeroStateMachine>().hero;
            heroToUpdate.curHP = fromHero.curHP;
            heroToUpdate.curMP = fromHero.curMP;
        }
    }

    /// <summary>
    /// Returns enemy entry from DB by given ID
    /// </summary>
    /// <param name="ID">ID of the enemy to be returned</param>
    BaseEnemy GetEnemy(int ID)
    {
        foreach (BaseEnemyDBEntry entry in EnemyDB.instance.enemies)
        {
            if (entry.enemy.ID == ID)
            {
                return entry.enemy;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns hero spawn point object from given hero GameObject
    /// </summary>
    /// <param name="heroObj">Given hero GameObject to return the spawn point object</param>
    GameObject GetHeroSpawnPoint(GameObject heroObj)
    {
        BaseHero checkID = heroObj.GetComponent<HeroStateMachine>().hero;
        BaseHero heroToCheck = null;

        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            if (checkID.ID == hero.ID)
            {
                heroToCheck = hero;
                break;
            }
        }

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("HeroSpawnPoint");

        foreach (GameObject spawnPoint in spawnPoints)
        {
            string spawnPointName = "SP" + heroToCheck.spawnPoint;
            if (spawnPointName == spawnPoint.name)
            {
                return spawnPoint;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns percent for exp bar by given hero current EXP and baseline EXP for next level
    /// </summary>
    /// <param name="hero">Hero to get EXP details from</param>
    float GetProgressBarValuesEXP(BaseHero hero)
    {
        float baseLineEXP;
        float heroEXP;

        if (hero.currentLevel == 1)
        {
            baseLineEXP = (HeroDB.instance.levelEXPThresholds[hero.currentLevel - 1]);
            heroEXP = hero.currentExp;
        }
        else
        {
            baseLineEXP = (HeroDB.instance.levelEXPThresholds[hero.currentLevel - 1] - HeroDB.instance.levelEXPThresholds[hero.currentLevel - 2]);
            heroEXP = (hero.currentExp - baseLineEXP);
            //Debug.Log("baseLine: " + baseLineEXP);
        }

        //Debug.Log(heroEXP + " / " + baseLineEXP + ": " + calcEXP);

        float calcEXP = heroEXP / baseLineEXP;

        return calcEXP;
    }

    /// <summary>
    /// Returns percent for exp bar by given EXP value and level
    /// </summary>
    /// <param name="heroEXP">Current hero EXP to process</param>
    /// <param name="heroLevel">Current hero level to process</param>
    float GetProgressBarValuesEXP(float heroEXP, int heroLevel)
    {
        float baseLineEXP;


        if (heroLevel == 1)
        {
            baseLineEXP = (HeroDB.instance.levelEXPThresholds[heroLevel - 1]);
            //Debug.Log("baseLine level 1: " + baseLineEXP);
        }
        else
        {
            baseLineEXP = (HeroDB.instance.levelEXPThresholds[heroLevel - 1] - HeroDB.instance.levelEXPThresholds[heroLevel - 2]);
            //Debug.Log("baseLine > level 1: " + baseLineEXP);
        }

        float calcEXP = heroEXP / baseLineEXP;

        //Debug.Log(heroEXP + " / " + baseLineEXP + ": " + calcEXP);

        return calcEXP;
    }

    /// <summary>
    /// Hides given panel by manipulating canvas group
    /// </summary>
    /// <param name="panel">UI Panel to hide</param>
    public void HidePanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 0;
        panel.GetComponent<CanvasGroup>().interactable = false;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// Displays given panel by manipulating canvas group
    /// </summary>
    /// <param name="panel">UI Panel to show</param>
    public void ShowPanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 1;
        panel.GetComponent<CanvasGroup>().interactable = true;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// Hides given selector by disabling attached sprite renderer
    /// </summary>
    /// <param name="selector">Selector GameObject to hide</param>
    public void HideSelector(GameObject selector)
    {
        selector.GetComponent<SpriteRenderer>().enabled = false;
    }

    /// <summary>
    /// Shows given selector by enabling attached sprite renderer
    /// </summary>
    /// <param name="selector">Selector GameObject to show</param>
    public void ShowSelector(GameObject selector)
    {
        selector.GetComponent<SpriteRenderer>().enabled = true;
    }
}
