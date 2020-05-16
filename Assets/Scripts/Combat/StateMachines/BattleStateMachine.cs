using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour //for processing phases of battle between enemies and heroes
{
    public enum PerformAction //phases of battle
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE
    }
    public PerformAction battleStates;
    
    public enum HeroGUI //phases of a hero selecting input
    {
        ACTIVATE,
        WAITING,
        DONE
    }
    public HeroGUI HeroInput;

    public List<HandleTurn> PerformList = new List<HandleTurn>(); //to store the turns that have been chosen between enemies and heros
    public List<GameObject> HeroesInBattle = new List<GameObject>(); //to store the gameobjects for all living heroes in battle
    public List<GameObject> AllHeroesInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>(); //to store the gameobjects for all enemies in battle

    public List<GameObject> HeroesToManage = new List<GameObject>(); //to store the gameobjects for all heros available to make a selection
    [HideInInspector] public HandleTurn HeroChoice; //the variable to store current hero's selection

    public GameObject EnemyButton; //button to click to use attack on enemy
    public GameObject HeroButton;
    public Transform EnemySelectSpacer; //for enemy select panel vertical layout group

    public GameObject moveActionPanel;
    public GameObject actionPanel; //panel that displays attack, magic, etc.
    public GameObject EnemySelectPanel; //panel that displays list of enemy targets
    public GameObject MagicPanel; //panel that lists magic attacks
    public GameObject ItemPanel;
    public GameObject BattleDetailsPanel;
    
    public Transform moveActionSpacer;
    public Transform actionSpacer; //to be assigned to the action panel's spacer
    public Transform magicSpacer; //to be assigned to the magic panel's spacer
    public Transform itemSpacer;
    public GameObject actionButton; //to be assigned to the attack button in action panel
    public GameObject magicButton; //to be assigned to the magic button in action panel
    public GameObject itemButton;

    //for showing damage
    public GameObject damageText;
    float damageTextDistance = .75f;
    public float damageDisplayTime = 1.5f;

    //for victory screen
    public GameObject victoryCanvas;
    bool victoryConfirmButtonPressed;
    bool runVictory;
    int goldDropped;
    List<Item> itemsDropped = new List<Item>();
    public GameObject Hero1Panel;
    public GameObject Hero2Panel;
    public GameObject Hero3Panel;
    public GameObject Hero4Panel;
    public GameObject Hero5Panel;
    public Image Hero1EXPProgressBar;
    public Image Hero2EXPProgressBar;
    public Image Hero3EXPProgressBar;
    public Image Hero4EXPProgressBar;
    public Image Hero5EXPProgressBar;
    public GameObject NewItemPanel;
    public Transform ItemListSpacer;
    float expGainSpeed = .0125f;
    public AudioClip expTick;
    AudioSource audioSource;

    public List<GameObject> targets = new List<GameObject>();
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

    private void Awake()
    {
        for (int i = 0; i < GameManager.instance.activeHeroes.Count; i++)
        {
            //GameObject NewHero = Instantiate(GameManager.instance.heroesToBattle[i], heroSpawnPoints[i].position, Quaternion.identity) as GameObject;
            GameObject NewHero = Instantiate(GameManager.instance.heroesToBattle[i], GetHeroSpawnPoint(GameManager.instance.heroesToBattle[i]).transform.position, Quaternion.identity) as GameObject;
            //NewHero.AddComponent<PlayerMove>();
        }
        for(int i=0; i < GameManager.instance.enemyAmount; i++)
        {
            string spawnPoint = (GameManager.instance.enemySpawnPoints[i]);  //need to set spawn point by troop and use it below
            GameObject spawnPointObject = GameObject.Find("EnemySpawnPoints/EnemySP" + spawnPoint);
            GameObject NewEnemy = Instantiate(GameManager.instance.enemiesToBattle[i].prefab, spawnPointObject.transform.position, Quaternion.identity) as GameObject; //uses enemy prefabs in Encounter region list and creates them as gameobjects
            if (i == 0)
            {
                NewEnemy.name = NewEnemy.GetComponent<EnemyStateMachine>().enemy.name; //sets the created enemy's name based on prefab enemy's name
            } else
            {
                NewEnemy.name = NewEnemy.GetComponent<EnemyStateMachine>().enemy.name + " " + (i + 1); //if there are more than 1 of the enemy, add a number to it.  This will need to be updated as separate enemies will not be taken into account
            }
            NewEnemy.GetComponent<EnemyStateMachine>().enemy = GetEnemy(GameManager.instance.enemiesToBattle[i].ID); //sets the created enemy's name in the state machine
            EnemiesInBattle.Add(NewEnemy); //adds the created enemy to enemies in battle list
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = true;
        }
    }

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

    void Start()
    {
        battleStates = PerformAction.WAIT; //battle starts in Battle State "WAIT"
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

        //insert here to tie heros in battle to new hero manager stuff to maintain exp, hp, mp, etc. can loop through heroes in battle to assign.
        HeroInput = HeroGUI.ACTIVATE; //battle starts with hero interface in state "ACTIVATE"

        moveActionPanel.SetActive(false);
        actionPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        MagicPanel.SetActive(false);
        ItemPanel.SetActive(false);
        //BattleDetailsPanel.SetActive(false);

        //EnemyButtons(); //creates enemy buttons from EnemiesInBattle list

        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        switch(battleStates) //phases of battle
        {
            case (PerformAction.WAIT): //checks if actions are to be taken
                //Debug.Log("waiting for action");
                if (PerformList.Count > 0) //if there are actions to be taken (from enemy or hero)
                {
                    //Debug.Log("found in perform list");
                    battleStates = PerformAction.TAKEACTION;
                }
            break;

            case (PerformAction.TAKEACTION): //checks for hero/enemy and processes action
                GameObject performer = GameObject.Find(PerformList[0].Attacker); //creates game object = enemy or hero that is attacking as "performer" which is used as current attacker (hero or enemy)
                if (PerformList[0].attackerType == HandleTurn.Types.HERO) //if attacker is a hero
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>(); //gets hero's state machine
                    //HSM.ActionTarget = PerformList[0].AttackersTarget; //changes hero state machine enemy to attack to the hero's target
                    HSM.currentState = HeroStateMachine.TurnState.ACTION; //tells hero state machine to start ACTION phase
                }
                battleStates = PerformAction.PERFORMACTION; //changes battle state to PERFORMACTION
            break;

            case (PerformAction.PERFORMACTION): //idle state while action is chosn

            break;

            case (PerformAction.CHECKALIVE): //checks when hero or enemy dies if win or loss conditions have been met
                if (HeroesInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                    //lose game
                } else if (EnemiesInBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                    //win the battle
                } else
                {
                    ClearAttackPanel(); //resets/clears enemySelect, actionPanel, magicPanel, and attack buttons
                    HeroInput = HeroGUI.ACTIVATE;
                }
            break;

            case (PerformAction.LOSE): //lose battle
                Debug.Log("Game lost"); //things to go here later - retry battle, go back to world map, load from save
            break;

            case (PerformAction.WIN): //win battle
                StartCoroutine(RunVictory());
                                
            break;
        }

        switch (HeroInput) //phases of hero inputs when hero's turn is available
        {
            case (HeroGUI.ACTIVATE): //hero's turn is available
                if (HeroesToManage.Count > 0) //if there is a hero's turn available (ATB gauge filled up and pending input)
                {
                    HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true); //Show hero's selector cursor
                    HeroChoice = new HandleTurn(); //new handle turn instance as HeroChoice

                    moveActionPanel.SetActive(true);
                    CreateMoveActionButtons();
                    CreateActionButtons(); //populate action buttons
                    //BattleDetailsPanel.SetActive(true);

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

                //HeroesToManage[0].GetComponent<HeroStateMachine>().ProcessStatusEffects();
            break;

        }

        GetCancelButton();
    }

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input); //used by enemy state machine to add enemy's chosen attack to the perform list
    }

    void HeroInputDone() 
    {
        PerformList.Add(HeroChoice); //adds the details of the current hero making selection's choice to the perform list
        ClearAttackPanel(); //cleans the attackpanel
        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false); //hides the current hero making selection's selector cursor
        HeroesToManage.RemoveAt(0); //removes the hero making selection from the heroesToManage list
        HeroInput = HeroGUI.ACTIVATE; //resets the HeroGUI switch back to the beginning to await the next hero's choice
    }

    void ClearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        actionPanel.SetActive(false);
        //BattleDetailsPanel.SetActive(false);
        MagicPanel.SetActive(false);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    void ClearMoveActionPanel()
    {
        moveActionPanel.SetActive(false);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    void ClearActionPanel()
    {
        actionPanel.SetActive(false);

        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

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

    void ClearActionLists()
    {
        attacksWithinMPThreshold.Clear(); //clears list for next enemy to use
        attacksWithinRange.Clear();
        lowestRangeForAttack = 0;
        foreach (Tile tile in tilesInRange)
        {
            tile.inRange = false;
        }
        tilesInRange.Clear();
    }

    void ClearThreatBars()
    {
        foreach (GameObject enObj in EnemiesInBattle)
        {
            EnemyBehavior eb = enObj.GetComponent<EnemyBehavior>();
            eb.ClearThreatBar();
        }
    }

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

    string[] GetTileCoordinates(Tile tile)
    {
        string tileCoords = tile.gameObject.name.Replace("Tile (", "");
        tileCoords = tileCoords.Replace(")", "");
        string[] coords = tileCoords.Split(',');
        return coords;
    }

    void EnableButton(GameObject button)
    {
        //Debug.Log("enabling " + button.name);
        button.GetComponent<Button>().interactable = true;
    }

    void DisableButton(GameObject button)
    {
        //Debug.Log("disabling" + button.name);
        button.GetComponent<Button>().interactable = false;
    }

    //create actionbuttons (attack, magic, etc)
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

    public void SetChosenMagic(BaseAttack chosenMagic) //called after choosing a magic attack
    {
        HeroChoice.Attacker = HeroesToManage[0].name; //sets the hero choice attacker to the current hero who selected magic
        HeroChoice.AttackersGameObject = HeroesToManage[0]; //sets the hero choice attacker's game object to the current hero
        HeroChoice.attackerType = HandleTurn.Types.HERO; //as hero choice is a HandleTurn, sets the type to Hero

        HeroChoice.chosenAttack = chosenMagic; //sets the hero choice's chosen attack to the chosenMagic in the parameter
        HeroChoice.actionType = HandleTurn.ActionType.MAGIC;

        MagicPanel.SetActive(false); //hides the magic panel
        EnemySelectPanel.SetActive(true); //opens the enemy select panel
        StartCoroutine(ChooseTarget()); //select which gameObject to be targetted
        SetCancelButton(3);
    }

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

    IEnumerator ChooseTarget() //gets target by which gameObject is clicked (enemy or hero)
    {
        //EnemyButtons();

        ShowRange();

        choosingTarget = true; //to ensure clicking on gameObjects are only processed when choosing target
        //Debug.Log(choosingTarget);

        while (HeroesToManage[0].GetComponent<HeroStateMachine>().targets.Count == 0)
        {
            if (cancelledEnemySelect) //if choosing enemy is cancelled
            {
                //Debug.Log("cancelled enemy select");
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

        //HeroChoice.AttackersTarget = chosenEnemy;
        //HeroChoice.targetType = HandleTurn.Types.ENEMY;
        GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>().text = "";
        HeroInput = HeroGUI.DONE;

        choosingTarget = false;
    }
    
    public void EnemySelection(GameObject chosenEnemy) //for selecting enemy
    {
        if (choosingTarget)
        {
            HeroChoice.AttackersTarget = chosenEnemy;
            HeroChoice.targetType = HandleTurn.Types.ENEMY;
            GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>().text = "";
            HeroInput = HeroGUI.DONE;
        }
    }

    public void HeroSelection(GameObject chosenHero) //for selecting hero
    {
        if (choosingTarget)
        {
            HeroChoice.AttackersTarget = chosenHero;
            HeroChoice.targetType = HandleTurn.Types.HERO;
            HeroInput = HeroGUI.DONE;
        }
    }

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

    public void ResetItemList() //clears items in item list so it can be rebuilt
    {
        ItemPanel.gameObject.SetActive(true);
        foreach (Transform child in GameObject.Find("BattleCanvas/ItemPanel/ItemScroller/ItemSpacer").transform)
        {
            Destroy(child.gameObject);
        }
        ItemPanel.gameObject.SetActive(false);
    }

    public void SetChosenItem(Item chosenItem) //called after choosing an item
    {
        HeroChoice.Attacker = HeroesToManage[0].name; //sets the hero choice attacker to the current hero who selected magic
        HeroChoice.AttackersGameObject = HeroesToManage[0]; //sets the hero choice attacker's game object to the current hero
        HeroChoice.attackerType = HandleTurn.Types.HERO; //as hero choice is a HandleTurn, sets the type to Hero

        HeroChoice.chosenItem = chosenItem; //sets the hero choice's chosen attack to the chosenMagic in the parameter
        HeroChoice.actionType = HandleTurn.ActionType.ITEM;

        ItemPanel.SetActive(false); //hides the magic panel
        EnemySelectPanel.SetActive(true); //opens the enemy select panel
        StartCoroutine(ChooseTarget());
        SetCancelButton(5);
    }

    public void ActionInput()
    {
        SetCancelButton(0);
        actionPanel.SetActive(true); //enables actionPanel (attack, magic, etc)
        moveActionPanel.SetActive(false);
        //GetButtonsByRange();
        HeroesToManage[0].GetComponent<PlayerMove>().RemoveSelectableTiles();
        HeroesToManage[0].GetComponent<PlayerMove>().canMove = false;
    }

    public void DefendInput()
    {
        //add defend method to reduce incoming damage by 50%

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
        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false); //hides the current hero making selection's selector cursor
        HeroesToManage.RemoveAt(0); //removes the hero making selection from the heroesToManage list
        HeroInput = HeroGUI.ACTIVATE; //resets the HeroGUI switch back to the beginning to await the next hero's choice
    }

    public void AttackInput() //when clicking 'attack' in actionPanel
    {
        HeroChoice.Attacker = HeroesToManage[0].name; //sets heroChoice attacker to current hero making selection
        HeroChoice.AttackersGameObject = HeroesToManage[0]; //sets heroChoice attacker's game object to the current hero making selection's game object
        HeroChoice.attackerType = HandleTurn.Types.HERO; //as HeroChoice is of class HandleTurn, sets type to Hero
        HeroChoice.chosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attack; //sets heroChoice chosen attack to the current hero's hero state machine to the attack at top of their attack list (likely change later)
        actionPanel.SetActive(false); //hides attack panel as action has been chosen
        EnemySelectPanel.SetActive(true); //displays enemy select panel to process chosen attack to
        StartCoroutine(ChooseTarget());
        SetCancelButton(1);
    }

    public void MagicInput() //after clicking 'Magic' on the action panel
    {
        actionPanel.SetActive(false); //hides the action panel
        MagicPanel.SetActive(true); //displays the magic panel showing current heros magic attacks
        SetCancelButton(2);
    }

    public void ItemInput() //after clicking 'Item' on the action panel
    {
        actionPanel.SetActive(false); //hides the action panel
        ItemPanel.SetActive(true); //displays the magic panel showing current items in inventory
        SetCancelButton(4);
    }

    void SetCancelButton(int option) //setting the bools for hitting the cancel button so the functionality changes depending on which menu the player is in
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

    void GetCancelButton() //detects if Cancel button is hit, and moves 'back' in the menus
    {
        if (Input.GetButtonDown("Cancel") && !buttonPressed)
        {
            if (moveMenuCancel) //in movement phase
            {
                actionPanel.SetActive(false);
                moveActionPanel.SetActive(true);
                SetCancelButton(6);

                HeroesToManage[0].GetComponent<PlayerMove>().FindSelectableTiles();
                HeroesToManage[0].GetComponent<PlayerMove>().canMove = true;
            }
            if (enemySelectAfterAttackCancel)
            {
                actionPanel.SetActive(true); //shows action panel
                moveActionPanel.SetActive(false);
                EnemySelectPanel.SetActive(false); //hides enemy select panel
                cancelledEnemySelect = true;
                SetCancelButton(0);
                ClearActionLists();
            }
            if (magicAttackCancel)
            {
                actionPanel.SetActive(true); //shows the action panel
                MagicPanel.SetActive(false); //hides the magic panel showing current heros magic attacks
                SetCancelButton(0);
            }
            if (enemySelectAfterMagicCancel)
            {
                MagicPanel.SetActive(true); //shows the magic panel
                cancelledEnemySelect = true;
                EnemySelectPanel.SetActive(false); //hides the enemy select panel
                SetCancelButton(2);
            }
            if (itemBattleMenuCancel)
            {
                actionPanel.SetActive(true); //shows the action panel
                ItemPanel.SetActive(false);
                SetCancelButton(0);
            }
            if (enemySelectAfterItemMenuCancel)
            {
                ItemPanel.SetActive(true); //shows the item panel
                EnemySelectPanel.SetActive(false); //hides the enemy select panel
                HeroChoice.chosenItem = null;
                cancelledEnemySelect = true;
                SetCancelButton(4);
            }
        }
        CheckCancelPressed();
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
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 2)
        {
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);

            Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2EXPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 3)
        {
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);

            Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2EXPProgressBar.transform.localScale.y);

            Hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3EXPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 4)
        {
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);

            Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2EXPProgressBar.transform.localScale.y);

            Hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3EXPProgressBar.transform.localScale.y);

            Hero4EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4EXPProgressBar.transform.localScale.y);
        }
        else if (GameManager.instance.activeHeroes.Count == 5)
        {
            Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[0]), 0, 1), Hero1EXPProgressBar.transform.localScale.y);

            Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[1]), 0, 1), Hero2EXPProgressBar.transform.localScale.y);

            Hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[2]), 0, 1), Hero3EXPProgressBar.transform.localScale.y);

            Hero4EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[3]), 0, 1), Hero4EXPProgressBar.transform.localScale.y);

            Hero5EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(GameManager.instance.activeHeroes[4]), 0, 1), Hero5EXPProgressBar.transform.localScale.y);
        }

    }

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
    }

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
    }

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
    }

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

    void DestroyDamageTexts()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("DamageText");
        foreach (GameObject obj in toDestroy)
        {
            Destroy(obj);
        }
    }

    public IEnumerator ShowMiss(GameObject target)
    {
        PauseATBWhileDamageFinishes(true);
        damageText = Instantiate(PrefabManager.Instance.damagePrefab);
        damageText.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + damageTextDistance);
        damageText.transform.GetComponent<TextMeshPro>().color = Color.white;
        damageText.transform.GetComponent<TextMeshPro>().text = "Miss";
        yield return new WaitForSeconds(damageDisplayTime);
        PauseATBWhileDamageFinishes(false);
        Destroy(damageText);
    }

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

    void DrawHeroFace(int ID, Image faceImage)
    {
        faceImage.sprite = GameManager.instance.activeHeroes[ID].faceImage;
    }

    //POST BATTLE METHODS

    void DoPostBattleStuff()
    {
        Debug.Log("You win!");

        GameObject.Find("BattleCanvas").GetComponent<CanvasGroup>().alpha = 0f;

        GetItemsDropped();

        GetGoldDropped();
    }

    void CheckForVictoryConfirmButtonPress()
    {
        if (Input.GetButtonDown("Confirm"))
        {
            victoryConfirmButtonPressed = true;
        }
    }

    IEnumerator ShowVictoryPoses()
    {
        Debug.Log("~~Simulating victory poses~~");

        yield return new WaitForSeconds(1f);

        Debug.Log("~~Victory pose end~~");
    }

    void DisplayVictoryCanvas()
    {
        victoryCanvas.SetActive(true);
    }

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

                NewItemPanel = Instantiate(PrefabManager.Instance.itemVictoryPrefab);
                NewItemPanel.transform.GetChild(0).GetComponent<Text>().text = item.name;
                NewItemPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
                NewItemPanel.transform.GetChild(2).GetComponent<Text>().text = itemCount.ToString();
                NewItemPanel.transform.SetParent(ItemListSpacer, false);
                itemsAccountedFor.Add(item);
            }
        }
    }

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

    IEnumerator RunVictory()
    {
        if (!runVictory)
        {
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

    void ShowPostBattleGains()
    {
        int heroCount = GameManager.instance.activeHeroes.Count;
        DrawHeroPanels(heroCount);
        DrawHeroStats();
        DrawEarnedStuff();
    }

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
                    Hero1EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), Hero1EXPProgressBar.transform.localScale.y);
                }
                else if (i == 1)
                {
                    Hero2EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), Hero2EXPProgressBar.transform.localScale.y);
                }
                else if (i == 2)
                {
                    Hero3EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), Hero3EXPProgressBar.transform.localScale.y);
                }
                else if (i == 3)
                {
                    Hero4EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), Hero4EXPProgressBar.transform.localScale.y);
                }
                else if (i == 4)
                {
                    Hero5EXPProgressBar.transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP((int.Parse(heroExpText.text) - expNeededToLevel), tempLevel), 0, 1), Hero5EXPProgressBar.transform.localScale.y);
                }
            }

            tempExpPool -= 1;

            audioSource.PlayOneShot(expTick, 0.3f);

            yield return new WaitForSeconds(expGainSpeed);
        }
    }

    void ShowLevelUp()
    {
        //show level up graphic here
        Debug.Log("Leveled up!");
    }

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
        GameManager.instance.ProcessExp();

        expPool = 0; //reset exp pool
        goldDropped = 0; //reset gold dropped
        itemsDropped.Clear(); //reset items dropped list

        UpdateActiveHeroes(); //keeps hero's parameters (HP, MP, and any persistent buffs/debuffs) consistent through each battle

        GameManager.instance.LoadSceneAfterBattle(); //load scene from before battle
        GameManager.instance.gameState = GameManager.GameStates.HOSTILE_STATE; //puts game manager back to hostile state
        GameManager.instance.enemiesToBattle.Clear(); //clears enemies to battle list to be used from scratch on next battle
        GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = false;
    }

    void HideVictoryCanvas()
    {
        victoryCanvas.GetComponent<CanvasGroup>().alpha = 0;
        victoryCanvas.GetComponent<CanvasGroup>().interactable = false;
        victoryCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void GetItemsDropped() //to determine which items to be dropped after battle, and add them
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

    void GetGoldDropped()
    {
        foreach (BaseBattleEnemy battleEnemy in GameManager.instance.enemiesToBattle)
        {
            Debug.Log(GetEnemy(battleEnemy.ID).name + " dropped " + GetEnemy(battleEnemy.ID).goldDropped + " gold.");
            GameManager.instance.gold += GetEnemy(battleEnemy.ID).goldDropped;
            goldDropped += GetEnemy(battleEnemy.ID).goldDropped;
        }
    }

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
}
