using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //TOOLS FOR DEBUGGING
    bool canBattle = true; //for debugging purposes, change to true to allow battles

    //TOOLS FOR DEBUGGING THAT WILL STILL BE USED
    public int battleChance = 10; //lower number is lower chance battle will occur.  set higher for battle debugging purposes so battle is encountered instantly - up to maxBattleChance
    public int maxBattleChance = 1000; //higher number is lower chance the battle will occur depending on battleChance, default is 1000

    //DEBUGGING NOTES
    //Each hero needs it's own prefab

    //-----------------------------------
    public static GameManager instance;

    //HERO
    public GameObject heroCharacter;

    //GOLD
    public int gold;

    [ReadOnly] public RegionData curRegion; //region of encounter zone

    [HideInInspector] public GameObject DialogueCanvas;

    //SPAWN POINTS
    [HideInInspector] public string nextSpawnPoint;

    //POSITIONS
    [ReadOnly] public Vector2 nextHeroPosition; //is set for loading player after battle
    [ReadOnly] public Vector2 lastHeroPosition; //is set for loading player after battle

    //SCENES
    [ReadOnly] public string sceneToLoad; //to load on collisions
    [ReadOnly] public string lastScene; //to load after battle

    //TEXT INPUTS
    [ReadOnly] public string textInput;
    [ReadOnly] public int numberInput;
    [ReadOnly] public string nameInput;
    [ReadOnly] public bool capsOn = false;
    [HideInInspector] public bool letterButtonPressed = false;

    //BOOLS
    [ReadOnly] public bool isWalking = false; //if player is currently walking
    [ReadOnly] public bool canGetEncounter = false; //if player is able to encounter enemies
    [HideInInspector] public bool gotAttacked = false; //if player actually enters combat
    [ReadOnly] bool receivedAllExp = false;

    //TEMP OBJECTS FOR SHOPS
    [HideInInspector] public List<BaseShopItem> itemShopList = new List<BaseShopItem>();
    [HideInInspector] public Item itemShopItem;
    [HideInInspector] public int itemShopCost;
    [HideInInspector] public List<BaseShopEquipment> equipShopList = new List<BaseShopEquipment>();
    [HideInInspector] public Equipment equipShopItem;
    [HideInInspector] public int equipShopCost;
    [HideInInspector] public bool inConfirmation;

    //ACTIVE HEROES
    [ReadOnly] public List<BaseHero> activeHeroes = new List<BaseHero>();

    //INACTIVE HEROES
    [ReadOnly] public List<BaseHero> inactiveHeroes = new List<BaseHero>();

    //QUESTS
    [ReadOnly] public List<BaseQuest> activeQuests = new List<BaseQuest>();
    [ReadOnly] public List<BaseQuest> completedQuests = new List<BaseQuest>();

    //POSITION SAVES
    [HideInInspector] public List<BasePositionSave> positionSaves = new List<BasePositionSave>();
    
    //FROM SCRIPT STUFF
    [HideInInspector] public string battleSceneFromScript;

    //TIME TRACKING
    [HideInInspector] public int seconds;
    [HideInInspector] public int minutes;
    [HideInInspector] public int hours;

    //BESTIARY
    [ReadOnly] public List<BaseBestiaryEntry> bestiaryEntries = new List<BaseBestiaryEntry>();
    
    public GameStates gameState;
    
    [HideInInspector] public List<BaseBattleEnemy> enemiesToBattle = new List<BaseBattleEnemy>(); //for adding enemies in encounter to the battle
    [HideInInspector] public List<GameObject> heroesToBattle = new List<GameObject>();
    [HideInInspector] public int enemyAmount; //for how many enemies can be encountered in one battle
    [HideInInspector] public List<string> enemySpawnPoints = new List<string>();
    [HideInInspector] public int heroAmount;

    [HideInInspector] public bool startBattleFromScript; //if battle is being started from script

    [HideInInspector] public string battleSceneToLoad;
    

    void Awake()
    {
        GameObject.Find("DebugCamera").SetActive(false); //Disable debugging camera so main camera attached to player can be used
        DialogueCanvas = GameObject.Find("DialogueCanvas");
        DialogueCanvas.GetComponent<CanvasGroup>().alpha = 0; //hides dialogue UI upon starting
        
        if (instance == null) //check if instance exists
        {
            instance = this; //if not set the instance to this
        }
        else if (instance != this) //if it exists but is not this instance
        {
            Destroy(gameObject); //destroy it
        }
        DontDestroyOnLoad(gameObject); //set this to be persistable across scenes

        if (!GameObject.Find("Player")) //if player gameobject doesnt exist
        {
            GameObject Hero = Instantiate(heroCharacter, nextHeroPosition, Quaternion.identity) as GameObject; //create player gameobject from prefab
            Hero.name = "Player"; //name player gameobject to "Player"
        }

        LoadPositionSaves(); //when loading each scene, check for any saved positions and load them

        StartCoroutine(UpdateTime());
    }

    private void Update()
    {
        switch (gameState)
        {
            case (GameStates.HOSTILE_STATE): //in the world, dungeon, etc. encounter possible
                if (isWalking) //if player is walking, encounter may be possible
                {
                    RandomEncounter(); //check for random encounter
                }
                if (gotAttacked) //if player is attacked
                {
                    gameState = GameStates.BATTLE_STATE; //transition to battle state
                }
                break;

            case (GameStates.PEACEFUL_STATE): //in town or area with no encounters

                break;

            case (GameStates.BATTLE_STATE):
                if (gotAttacked) //if detected encounter from region
                {
                    StartBattle(true); //Loads battle scene from region
                }
                else
                {
                    StartBattle(false); //Loads battle scene from script
                }

                gameState = GameStates.IDLE;
                break;

            case (GameStates.IDLE):

                break;
        }
    }

    /// <summary>
    /// Loads saved positions for all NPCs from exiting/entering the map
    /// </summary>
    void LoadPositionSaves()
    {
        if (instance.positionSaves.Count > 0) //if any positions are saved
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(); //get all gameobjects in scene
            for (int j = 0; j < allObjects.Length; j++)
            {
                for (int i = 0; i < instance.positionSaves.Count; i++)
                {
                    if (instance.positionSaves[i].name == allObjects[j].name && instance.positionSaves[i].Scene == SceneManager.GetActiveScene().name) //if gameobject name matches positionSave name, and scene from positionSave is same as current scene
                    {
                        allObjects[j].transform.position = instance.positionSaves[i].newPosition; //sets the gameobject's position to the saved position
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Calls battle scene with provided troops
    /// </summary>
    /// <param name="fromRegion">If true, loads scene from current region.  If false, loads scene set from script</param>
    void StartBattle(bool fromRegion)
    {
        heroAmount = activeHeroes.Count; //number of active heroes in the game manager


        for (int i = 0; i < heroAmount; i++) //loop through all active heroes in the game manager
        {
            heroesToBattle.Add(activeHeroes[i].heroPrefab); //spawn them in battle
        }

        GenerateHeroesToBattle(); //sets all parameters for the hero combatants to the values from their corresponding values in active hero list in game manager


        if (fromRegion) //if encounter is started from encounter zone
        {
            GetTroopsFromRegion(); //load troops based on encounter zone
        }

        //HERO
        lastHeroPosition = GameObject.Find("Player").gameObject.transform.position; //sets player's gameObject in the world position to lastHeroPosition
        nextHeroPosition = lastHeroPosition; //next hero position should be the last hero position, to spawn back into the world after battle
        lastScene = SceneManager.GetActiveScene().name; //sets last scene to the scene before battle
        //LOAD LEVEL
        if (fromRegion)
        {
            //SceneManager.LoadScene(curRegion.BattleScene); //loads battle scene -- Disable when not testing
            GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadBattle(battleSceneToLoad);
        } else
        {
            //SceneManager.LoadScene(battleSceneFromScript); //loads battle scene from script -- Disable when not testing
            GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadBattle(battleSceneFromScript);
        }
        
        //RESET HERO ENCOUNTER BOOLS
        isWalking = false;
        gotAttacked = false;
        canGetEncounter = false;
    }

    /// <summary>
    /// Loads scene set from sceneToLoad
    /// </summary>
    public void LoadScene()
    {
        //SceneManager.LoadScene(sceneToLoad); //loads scene from collisions
        GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadScene(sceneToLoad); //loads scene from collisions
    }

    /// <summary>
    /// Loads last scene loaded before battle was called
    /// </summary>
    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene); //loads last scene saved from before battle
    }

    /// <summary>
    /// Checks if player is walking in encounterable zone, then chooses random value from max battle chance to potentially call battle
    /// </summary>
    void RandomEncounter()
    {
        if(isWalking && canGetEncounter && canBattle) //if player is walking in an encounterable zone
        {   Random.InitState(System.DateTime.Now.Millisecond);
            int battleValue = Random.Range(0, maxBattleChance); //get random value between 0 and maxBattleChance
            if(battleValue < battleChance)
            {
                Debug.Log("GameManager - RandomEncounter called");
                Debug.Log("isWalking: " + isWalking + ", canGetEncounter: " + canGetEncounter);
                Debug.Log("BattleChance: " + battleChance + " out of " + maxBattleChance + ", battleValue: " + battleValue);
                gotAttacked = true; //allows for battle to start. if this turns true during 'HOSTILE_STATE', battle is started
            }
        }
    }

    /// <summary>
    /// Loads heroes into newly generated heroes for battle
    /// </summary>
    void GenerateHeroesToBattle() //when heros are instantiated in battle, the stats from game manager heroes are copied over to the combatants
    {
        for (int i = 0; i < heroAmount; i++)
        {
            BaseHero heroToAdd = heroesToBattle[i].GetComponent<HeroStateMachine>().hero;
            BaseHero fromHero = activeHeroes[i];
            heroToAdd.name = fromHero.name;
            heroToAdd.ID = fromHero.ID;
            heroToAdd.currentLevel = fromHero.currentLevel;
            heroToAdd.currentExp = fromHero.currentExp;
            heroToAdd.baseHP = fromHero.baseHP;
            heroToAdd.curHP = fromHero.curHP;
            heroToAdd.baseMP = fromHero.baseMP;
            heroToAdd.curMP = fromHero.curMP;
            heroToAdd.finalMaxHP = fromHero.finalMaxHP;
            heroToAdd.finalMaxMP = fromHero.finalMaxMP;
            heroToAdd.baseATK = fromHero.baseATK;
            heroToAdd.finalATK = fromHero.finalATK;
            heroToAdd.baseMATK = fromHero.baseMATK;
            heroToAdd.finalMATK = fromHero.finalMATK;
            heroToAdd.baseDEF = fromHero.baseDEF;
            heroToAdd.finalDEF = fromHero.finalDEF;
            heroToAdd.baseSTR = fromHero.baseSTR;
            heroToAdd.baseSTA = fromHero.baseSTA;
            heroToAdd.baseINT = fromHero.baseINT;
            heroToAdd.baseDEX = fromHero.baseDEX;
            heroToAdd.baseAGI = fromHero.baseAGI;
            heroToAdd.baseSPI = fromHero.baseSPI;
            heroToAdd.baseMove = fromHero.baseMove;
            heroToAdd.finalStrength = fromHero.finalStrength;
            heroToAdd.finalStamina = fromHero.finalStamina;
            heroToAdd.finalIntelligence = fromHero.finalIntelligence;
            heroToAdd.finalDexterity = fromHero.finalDexterity;
            heroToAdd.finalAgility = fromHero.finalAgility;
            heroToAdd.finalSpirit = fromHero.finalSpirit;
            heroToAdd.strengthMod = fromHero.strengthMod;
            heroToAdd.staminaMod = fromHero.staminaMod;
            heroToAdd.intelligenceMod = fromHero.intelligenceMod;
            heroToAdd.dexterityMod = fromHero.dexterityMod;
            heroToAdd.agilityMod = fromHero.agilityMod;
            heroToAdd.baseHit = fromHero.baseHit;
            heroToAdd.finalHitRating = fromHero.finalHitRating;
            heroToAdd.baseCrit = fromHero.baseCrit;
            heroToAdd.finalCritRating = fromHero.finalCritRating;
            heroToAdd.attack = fromHero.attack;
            heroToAdd.MagicAttacks = fromHero.MagicAttacks;
            heroToAdd.finalMoveRating = fromHero.finalMoveRating;
        }
    }

    /// <summary>
    /// Adds exp for all active heroes and levels them up if needed
    /// </summary>
    public void ProcessExp()
    {
        foreach (BaseHero hero in activeHeroes)
        {
            Debug.Log("currentExp: " + hero.currentExp + ", currentLevel: " + hero.currentLevel);
            while(receivedAllExp == false)
            {
                if (hero.currentExp >= HeroDB.instance.levelEXPThresholds[(hero.currentLevel - 1)])
                {
                    hero.levelBeforeExp = hero.currentLevel; //wrong place for this, will update later
                    hero.LevelUp();
                } else
                {
                    receivedAllExp = true;
                }
            }
            receivedAllExp = false;
        }
    }

    /// <summary>
    /// Enables panels by displaying via CanvasGroup
    /// </summary>
    public void DisplayPanel(bool display)
    {
        if (display)
        {
            DialogueCanvas.GetComponent<CanvasGroup>().alpha = 1;
            DialogueCanvas.GetComponent<CanvasGroup>().interactable = true;
            DialogueCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
            //Debug.Log("displaying canvas");
        }
        else
        {
            DialogueCanvas.GetComponent<CanvasGroup>().alpha = 0;
            DialogueCanvas.GetComponent<CanvasGroup>().interactable = false;
            DialogueCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
            //Debug.Log("hiding canvas");
        }
    }

    /// <summary>
    /// Sets troops from encounter zone
    /// </summary>
    void GetTroopsFromRegion()
    {
        //int whichTroop = Random.Range(0, curRegion.possibleTroops.Count);
        //enemyAmount = troops[whichTroop].enemies.Count;
        //Debug.Log(whichTroop);
        //Debug.Log(troops[whichTroop].enemies.Count);
        bool gotEncounterChance = false;
        float encounterChance;
        int randomVal;
        Random.InitState(System.DateTime.Now.Millisecond);
        int randomStart = Random.Range(0, curRegion.troopEncounters.Count);
        bool runRandomStartOnce = true;
        int whichTroop = 0;
        enemiesToBattle.Clear();
        enemySpawnPoints.Clear();

        while (!gotEncounterChance)
        {
            for (int i = 0; i < curRegion.troopEncounters.Count; i++)
            {
                if (runRandomStartOnce)
                {
                    i = randomStart;
                    runRandomStartOnce = false;
                }
                Random.InitState(System.DateTime.Now.Millisecond);
                randomVal = Random.Range(1, 100);
                Debug.Log("randomVal: " + randomVal);
                encounterChance = curRegion.troopEncounters[i].encounterChance;
                if (encounterChance == 0)
                {
                    encounterChance = 50;
                }
                if (randomVal <= encounterChance)
                {
                    Debug.Log("Chose: " + curRegion.troopEncounters[i].index + ", Encounter chance: " + encounterChance);
                    whichTroop = curRegion.troopEncounters[i].index;
                    battleSceneToLoad = curRegion.troopEncounters[i].BattleScene;
                    gotEncounterChance = true;
                    break;
                }
            }
         }
        for (int i = 0; i < TroopDB.instance.troops[whichTroop].enemies.Count; i++)
        {
            BaseBattleEnemy newBattleEnemy = new BaseBattleEnemy();
            newBattleEnemy.ID = TroopDB.instance.troops[whichTroop].enemies[i].enemyID;
            newBattleEnemy.prefab = GetEnemyDBEntry(TroopDB.instance.troops[whichTroop].enemies[i].enemyID).prefab;

            enemiesToBattle.Add(newBattleEnemy);

            enemySpawnPoints.Add(TroopDB.instance.troops[whichTroop].enemies[i].spawnPoint);
        }
        enemyAmount = TroopDB.instance.troops[whichTroop].enemies.Count;
        }

    /// <summary>
    /// Sets troops from script and load battle
    /// </summary>
    /// <param name="troopIndex">Given troop index to search</param>
    /// <param name="scene">Battle scene to load</param>
    public void GetBattleFromScript(int troopIndex, string scene)
    {
        for (int i = 0; i < TroopDB.instance.troops[troopIndex].enemies.Count; i++)
        {
            BaseBattleEnemy newBattleEnemy = new BaseBattleEnemy();
            newBattleEnemy.ID = TroopDB.instance.troops[troopIndex].enemies[i].enemyID;
            newBattleEnemy.prefab = GetEnemyDBEntry(TroopDB.instance.troops[troopIndex].enemies[i].enemyID).prefab;

            enemiesToBattle.Add(newBattleEnemy);
            
            enemySpawnPoints.Add(TroopDB.instance.troops[troopIndex].enemies[i].spawnPoint);
        }
        battleSceneFromScript = scene;
        enemyAmount = TroopDB.instance.troops[troopIndex].enemies.Count;
        gameState = GameStates.BATTLE_STATE;
    }

    /// <summary>
    /// Returns enemy DB entry by ID
    /// </summary>
    /// <param name="ID">ID of enemy in entry DB to return</param>
    BaseEnemyDBEntry GetEnemyDBEntry(int ID)
    {
        foreach (BaseEnemyDBEntry entry in EnemyDB.instance.enemies)
        {
            if (entry.enemy.ID == ID)
            {
                return entry;
            }
        }
        return null;
    }

    /// <summary>
    /// Consistently keeps game time updated
    /// </summary>
    IEnumerator UpdateTime()
    {
        while (hours != 99 && minutes != 60 && seconds != 60)
        {
            yield return new WaitForSecondsRealtime(1);
            seconds++;

            if (seconds == 60)
            {
                minutes++;
                seconds = 0;
            }

            if (minutes == 60)
            {
                hours++;
                minutes = 0;
            }
        }
    }

}
