﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //TOOLS FOR DEBUGGING
    bool canBattle = true; //for debugging purposes, change to true to allow battles

    //TOOLS FOR DEBUGGING THAT WILL STILL BE USED
    [HideInInspector] public int battleChance = 10; //lower number is lower chance battle will occur.  set higher for battle debugging purposes so battle is encountered instantly - up to maxBattleChance
    [HideInInspector] public int maxBattleChance = 10; //higher number is lower chance the battle will occur depending on battleChance, default is 1000

    //DEBUGGING NOTES
    //Each hero needs it's own prefab

    //-----------------------------------
    public static GameManager instance;

    public RegionData curRegion; //region of encounter zone

    [HideInInspector] public GameObject DialogueCanvas;

    //SPAWN POINTS
    [HideInInspector] public string nextSpawnPoint;

    //HERO
    public GameObject heroCharacter;

    //POSITIONS
    public Vector2 nextHeroPosition; //is set for loading player after battle
    public Vector2 lastHeroPosition; //is set for loading player after battle

    //SCENES
    public string sceneToLoad; //to load on collisions
    public string lastScene; //to load after battle

    //BOOLS
    public bool isWalking = false; //if player is currently walking
    public bool canGetEncounter = false; //if player is able to encounter enemies
    [HideInInspector] public bool gotAttacked = false; //if player actually enters combat
    bool receivedAllExp = false;

    //GOLD
    public int gold;

    //ACTIVE HEROES
    public List<BaseHero> activeHeroes = new List<BaseHero>();

    //LEVELING BASES
    public int[] toLevelUp;

    //Global Game Bools
    public List<bool> globalBools = new List<bool>();

    //POSITION SAVES
    [HideInInspector] public List<BasePositionSave> positionSaves = new List<BasePositionSave>();

    //TROOPS
    public List<BaseTroop> troops = new List<BaseTroop>();

    //FROM SCRIPT STUFF
    [HideInInspector] public string battleSceneFromScript;

    //TIME TRACKING
    [HideInInspector] public int seconds;
    [HideInInspector] public int minutes;
    [HideInInspector] public int hours;
    

    //ENUM
    public enum GameStates
    {
        HOSTILE_STATE,
        PEACEFUL_STATE,
        BATTLE_STATE,
        IDLE
    }
    public GameStates gameState;

    
    [HideInInspector] public List<GameObject> enemiesToBattle = new List<GameObject>(); //for adding enemies in encounter to the battle
    [HideInInspector] public List<GameObject> heroesToBattle = new List<GameObject>();
    [HideInInspector] public int enemyAmount; //for how many enemies can be encountered in one battle
    [HideInInspector] public int heroAmount;

    [HideInInspector] public bool startBattleFromScript; //if battle is being started from script
    

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

    void LoadPositionSaves()
    {
        if (instance.positionSaves.Count > 0) //if any positions are saved
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(); //get all gameobjects in scene
            for (int j = 0; j < allObjects.Length; j++)
            {
                for (int i = 0; i < instance.positionSaves.Count; i++)
                {
                    if (instance.positionSaves[i]._Name == allObjects[j].name && instance.positionSaves[i].Scene == SceneManager.GetActiveScene().name) //if gameobject name matches positionSave name, and scene from positionSave is same as current scene
                    {
                        allObjects[j].transform.position = instance.positionSaves[i].newPosition; //sets the gameobject's position to the saved position
                        break;
                    }
                }
            }
        }
    }
    

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
            SceneManager.LoadScene(curRegion.BattleScene); //loads battle scene
        } else
        {
            SceneManager.LoadScene(battleSceneFromScript); //loads battle scene from script
        }
        
        //RESET HERO ENCOUNTER BOOLS
        isWalking = false;
        gotAttacked = false;
        canGetEncounter = false;
    }

    private void Update()
    {
        switch(gameState)
        {
            case (GameStates.HOSTILE_STATE): //in the world, dungeon, etc. encounter possible
                if(isWalking) //if player is walking, encounter may be possible
                {
                    RandomEncounter(); //check for random encounter
                }
                if(gotAttacked) //if player is attacked
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
                } else
                {
                    StartBattle(false); //Loads battle scene from script
                }
                
                gameState = GameStates.IDLE;
            break;

            case (GameStates.IDLE):

            break;

        }
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad); //loads scene from collisions
    }

    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene); //loads last scene saved from before battle
    }

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

    void GenerateHeroesToBattle() //when heros are instantiated in battle, the stats from game manager heros are copied over to the combatants
    {
        for (int i = 0; i < heroAmount; i++)
        {
            BaseHero heroToAdd = heroesToBattle[i].GetComponent<HeroStateMachine>().hero;
            BaseHero fromHero = activeHeroes[i];
            heroToAdd._Name = fromHero._Name;
            heroToAdd.currentLevel = fromHero.currentLevel;
            heroToAdd.currentExp = fromHero.currentExp;
            heroToAdd.baseHP = fromHero.baseHP;
            heroToAdd.curHP = fromHero.curHP;
            heroToAdd.baseMP = fromHero.baseMP;
            heroToAdd.curMP = fromHero.curMP;
            heroToAdd.maxHP = fromHero.maxHP;
            heroToAdd.maxMP = fromHero.maxMP;
            heroToAdd.baseATK = fromHero.baseATK;
            heroToAdd.currentATK = fromHero.currentATK;
            heroToAdd.baseMATK = fromHero.baseMATK;
            heroToAdd.currentMATK = fromHero.currentMATK;
            heroToAdd.baseDEF = fromHero.baseDEF;
            heroToAdd.currentDEF = fromHero.currentDEF;
            heroToAdd.baseStrength = fromHero.baseStrength;
            heroToAdd.baseStamina = fromHero.baseStamina;
            heroToAdd.baseIntelligence = fromHero.baseIntelligence;
            heroToAdd.baseDexterity = fromHero.baseDexterity;
            heroToAdd.baseAgility = fromHero.baseAgility;
            heroToAdd.baseSpirit = fromHero.baseSpirit;
            heroToAdd.currentStrength = fromHero.currentStrength;
            heroToAdd.currentStamina = fromHero.currentStamina;
            heroToAdd.currentIntelligence = fromHero.currentIntelligence;
            heroToAdd.currentDexterity = fromHero.currentDexterity;
            heroToAdd.currentAgility = fromHero.currentAgility;
            heroToAdd.currentSpirit = fromHero.currentSpirit;
            heroToAdd.strengthModifier = fromHero.strengthModifier;
            heroToAdd.staminaModifier = fromHero.staminaModifier;
            heroToAdd.intelligenceModifier = fromHero.intelligenceModifier;
            heroToAdd.dexterityModifier = fromHero.dexterityModifier;
            heroToAdd.agilityModifier = fromHero.agilityModifier;
            heroToAdd.baseHitRating = fromHero.baseHitRating;
            heroToAdd.currentHitRating = fromHero.currentHitRating;
            heroToAdd.baseCritRating = fromHero.baseCritRating;
            heroToAdd.currentCritRating = fromHero.currentCritRating;
            heroToAdd.attacks = fromHero.attacks;
            heroToAdd.MagicAttacks = fromHero.MagicAttacks;
        }
    }

    public void ProcessExp()
    {
        foreach (BaseHero hero in activeHeroes)
        {
            Debug.Log("currentExp: " + hero.currentExp + ", currentLevel: " + hero.currentLevel);
            while(receivedAllExp == false)
            {
                if (hero.currentExp >= toLevelUp[(hero.currentLevel - 1)])
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

    public void DisplayPanel(bool display)
    {
        if (display)
        {
            DialogueCanvas.GetComponent<CanvasGroup>().alpha = 1;
            DialogueCanvas.SetActive(true);
            //Debug.Log("displaying canvas");
        }
        else
        {
            DialogueCanvas.GetComponent<CanvasGroup>().alpha = 0;
            DialogueCanvas.SetActive(false);
            //Debug.Log("hiding canvas");
        }
    }

    void GetTroopsFromRegion() //sets troops from encounter zone
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
                    gotEncounterChance = true;
                    break;
                }
            }
         }
        for (int i = 0; i < troops[whichTroop].enemies.Count; i++)
            {
                enemiesToBattle.Add(troops[whichTroop].enemies[i]);
            }
        enemyAmount = troops[whichTroop].enemies.Count;
        }

    public void GetBattleFromScript(int troopIndex, string scene) //sets troops from script
    {
        for (int i = 0; i < troops[troopIndex].enemies.Count; i++)
        {
            enemiesToBattle.Add(troops[troopIndex].enemies[i]);
        }
        battleSceneFromScript = scene;
        enemyAmount = troops[troopIndex].enemies.Count;
        gameState = GameManager.GameStates.BATTLE_STATE;
    }

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