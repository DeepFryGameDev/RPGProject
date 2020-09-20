using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
    //Hero state machine script is attached to each hero to be used in battle state machine

    private BattleStateMachine BSM; //global battle state machine
    public BaseHero hero; //this hero

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    public enum CursorStates
    {
        MOVING,
        ACTION,
        CHOOSINGTARGET,
        IDLE
    }
    public CursorStates cursorState;

    public enum ActionStates
    {
        PREACTION,
        ACTION,
        MAGIC,
        ITEM,
        IDLE
    }
    public ActionStates actionState;

    string chosenAction;

    public int cursorOnAction;
    Transform tileCursor;
    Transform cursor;
    float cursorVisibleScale = .82f;
    float panelItemSpacer = 19.5f;
    float panelItemScrollSpacer = 19.7f;
    int tempScrollDiff;

    Text detailsText;
    GameObject heroDetailsPanel;
    GameObject enemyDetailsPanel;

    GameObject magicSelected;
    GameObject itemSelected;

    bool dpadPressed;
    bool confirmPressed;
    bool cancelPressed;

    public bool waitForDamageToFinish;

    //for ProgressBar
    public float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    private Image ProgressBar;

    public GameObject ActionTarget; //target to be actioned on
    private bool actionStarted = false;
    [HideInInspector] public Vector2 startPosition; //starting position before running to target
    private float animSpeed = 10f; //for moving to target
    //dead
    private bool alive = true;

    //heroPanel
    private HeroPanelStats stats;
    public GameObject HeroPanel;
    private Transform HeroPanelSpacer; //for the spacer on the Hero Panel (for Vertical Layout Group)

    public List<BaseEffect> activeStatusEffects = new List<BaseEffect>();
    public int effectDamage;
    public int magicDamage;
    public int itemDamage;

    List<BaseAddedEffect> checkAddedEffects = new List<BaseAddedEffect>();
    public List<BaseDamage> finalDamages = new List<BaseDamage>();

    private PlayerMove playerMove;
    private bool calculatedTilesToMove;

    public int heroTurn;

    protected List<GameObject> targetsInRange = new List<GameObject>();
    Pattern pattern = new Pattern();
    public List<GameObject> targets = new List<GameObject>();
    List<GameObject> targetsAccountedFor = new List<GameObject>();
    public bool choosingTarget;
    List<Tile> tilesInRange = new List<Tile>();

    Animator heroAnim;

    void Start()
    {
        HeroPanelSpacer = GameObject.Find("BattleCanvas/BattleUI").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer"); //find spacer and make connection

        CreateHeroPanel(); //create panel and fill in info

        if (hero.curHP == 0) //if hero starts battle at 0 HP
        {
            ProgressBar.transform.localScale = new Vector2(0, ProgressBar.transform.localScale.y); //reduces ATB gauge to 0
            cur_cooldown = 0; //Sets ATB gauge to 0
            currentState = HeroStateMachine.TurnState.DEAD; //sets hero in dead state
        } else //if hero is alive
        {
            cur_cooldown = Random.Range(0, 2.5f); //Sets random point for ATB gauge to start
            //currentState = TurnState.PROCESSING; //begins hero processing phase
            currentState = TurnState.WAITING;
        }
        heroTurn = 1;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //make connection to the global battle state manager
        startPosition = transform.position; //sets start position to hero's position for animation purposes
        playerMove = GetComponent<PlayerMove>();
        tileCursor = GameObject.Find("GridMap/TileCursor").transform;
        cursor = GameObject.Find("BattleCanvas/Cursor").transform;
        detailsText = GameObject.Find("BattleCanvas/BattleUI/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
        heroDetailsPanel = GameObject.Find("BattleCanvas/HeroDetailsPanel");
        enemyDetailsPanel = GameObject.Find("BattleCanvas/EnemyDetailsPanel");
        actionState = ActionStates.IDLE;
        HideTileCursor();
    }

    void Update()
    {
        //Debug.Log("HeroStateMachine - currentState: " + currentState);
        switch(currentState)
        {
            case (TurnState.PROCESSING):
                if (!waitForDamageToFinish)
                {
                    if (BSM.activeATB)
                    {
                        UpgradeProgressBar(); //fills hero ATB gauge
                    }
                    else
                    {
                        if (BSM.pendingTurn == false)
                        {
                            UpgradeProgressBar(); //fills hero ATB gauge
                        }
                    }
                }
            break;

            case (TurnState.ADDTOLIST):
                if (!calculatedTilesToMove)
                {
                    playerMove.tilesToMove = playerMove.move;
                    calculatedTilesToMove = true;
                }
                
                playerMove.BeginTurn();
                BSM.HeroesToManage.Add(this.gameObject); //adds hero to heros who can make selection

                playerMove.GetCurrentTile();
                tileCursor.position = playerMove.currentTile.transform.position;
                ShowTileCursor();
                HideBattleUI();
                cursorState = CursorStates.MOVING;

                currentState = TurnState.WAITING;
            break;

            case (TurnState.WAITING):
                //idle
            break;

            case (TurnState.ACTION):
                //Debug.Log("in hero action");
                TimeForAction(); //processes hero action
            break;

            case (TurnState.DEAD): //run after every time enemy takes damage that brings them to or below 0 hp
                if (!alive) //if alive value is set to false, exits the turn state. this is set to false in below code
                {
                    return;
                } else
                {
                    this.gameObject.tag = "DeadHero"; //change tag of hero to DeadHero
                    BSM.HeroesInBattle.Remove(this.gameObject); //not hero attackable by enemy
                    BSM.HeroesToManage.Remove(this.gameObject); //not able to manage hero with player
                    
                    //reset GUI
                    BSM.HidePanel(BSM.actionPanel);
                    BSM.HidePanel(BSM.enemySelectPanel);
                    //remove hero's handleturn from performlist (if there was one)
                    if (BSM.HeroesInBattle.Count > 0)
                    {
                        
                        for (int i = 0; i < BSM.PerformList.Count; i++) //go through all actions in perform list
                        {
                            if (i != 0) //can remove later if heros can kill themselves.  otherwise only checks for items in the perform list after 0 (as 0 would be the enemy's action)
                            {
                                if (BSM.PerformList[i].AttackersGameObject == this.gameObject) //if the attacker in the loop is this hero
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]); //removes this action from the perform list
                                }

                                if (BSM.PerformList[i].AttackersTarget == this.gameObject) //if target in loop in the perform list is the dead hero
                                {
                                    BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];//changes the target from the dead hero to a random hero so dead hero cannot be attacked
                                }
                            }
                        }
                    }

                    //gameObject.GetComponent<SpriteRenderer>().material.color = new Color32(105, 105, 105, 255); //change color/ play animation

                    gameObject.GetComponent<Animator>().SetBool("onDeath", true);

                    Debug.Log(hero.name + " - DEAD");
                    BSM.battleState = battleStates.CHECKALIVE;

                    alive = false;
                }
            break;
        }

        if (BSM.HeroesToManage.Count > 0 && this.gameObject == BSM.HeroesToManage[0])
        {
            switch (cursorState)
            {
                case (CursorStates.MOVING):

                    if (Input.GetAxisRaw("DpadHorizontal") == -1 && !dpadPressed) //left
                    {
                        dpadPressed = true;
                        if (IfTileExists(new Vector3(tileCursor.position.x - 1f, tileCursor.position.y, 0)))
                        {
                            tileCursor.position = new Vector3(tileCursor.position.x - 1f, tileCursor.position.y, 0);
                            AudioManager.instance.PlaySE(AudioManager.instance.moveTileCursorSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadHorizontal") == 1 && !dpadPressed) //right
                    {
                        dpadPressed = true;
                        if (IfTileExists(new Vector3(tileCursor.position.x + 1f, tileCursor.position.y, 0)))
                        {
                            tileCursor.position = new Vector3(tileCursor.position.x + 1f, tileCursor.position.y, 0);
                            AudioManager.instance.PlaySE(AudioManager.instance.moveTileCursorSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1 && !dpadPressed) //up
                    {
                        dpadPressed = true;
                        if (IfTileExists(new Vector3(tileCursor.position.x, tileCursor.position.y + 1f, 0)))
                        {
                            tileCursor.position = new Vector3(tileCursor.position.x, tileCursor.position.y + 1f, 0);
                            AudioManager.instance.PlaySE(AudioManager.instance.moveTileCursorSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1 && !dpadPressed) //down
                    {
                        dpadPressed = true;
                        if (IfTileExists(new Vector3(tileCursor.position.x, tileCursor.position.y - 1f, 0)))
                        {
                            tileCursor.position = new Vector3(tileCursor.position.x, tileCursor.position.y - 1f, 0);
                            AudioManager.instance.PlaySE(AudioManager.instance.moveTileCursorSE);
                        }                        
                    }

                    if (GetUnitOnTile() != null)
                    {
                        if (GetUnitOnTile().tag == "Hero")
                        {
                            heroDetailsPanel.transform.Find("NameText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<HeroStateMachine>().hero.name;
                            heroDetailsPanel.transform.Find("LevelText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<HeroStateMachine>().hero.currentLevel.ToString();

                            heroDetailsPanel.transform.Find("HPText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<HeroStateMachine>().hero.curHP.ToString() + "/" 
                                + GetUnitOnTile().GetComponent<HeroStateMachine>().hero.finalMaxHP.ToString();

                            heroDetailsPanel.transform.Find("HPProgressBarBG/HPProgressBar").transform.localScale = 
                                new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GetUnitOnTile().GetComponent<HeroStateMachine>().hero), 0, 1), 
                                heroDetailsPanel.transform.Find("HPProgressBarBG/HPProgressBar").transform.localScale.y);

                            heroDetailsPanel.transform.Find("MPText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<HeroStateMachine>().hero.curMP.ToString() + "/"
                                + GetUnitOnTile().GetComponent<HeroStateMachine>().hero.finalMaxMP.ToString();
                            heroDetailsPanel.transform.Find("MPProgressBarBG/MPProgressBar").transform.localScale = 
                                new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GetUnitOnTile().GetComponent<HeroStateMachine>().hero), 0, 1),
                                heroDetailsPanel.transform.Find("MPProgressBarBG/MPProgressBar").transform.localScale.y);

                            BSM.HidePanel(enemyDetailsPanel);
                            BSM.ShowPanel(heroDetailsPanel);
                        } else if (GetUnitOnTile().tag == "Enemy")
                        {
                            enemyDetailsPanel.transform.Find("NameText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.name;
                            enemyDetailsPanel.transform.Find("LevelText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.level.ToString();

                            enemyDetailsPanel.transform.Find("HPText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.curHP.ToString() + "/"
                                + GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.baseHP.ToString();

                            enemyDetailsPanel.transform.Find("HPProgressBarBG/HPProgressBar").transform.localScale =
                                new Vector2(Mathf.Clamp(GetEnemyProgressBarValuesHP(GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy), 0, 1),
                                enemyDetailsPanel.transform.Find("HPProgressBarBG/HPProgressBar").transform.localScale.y);

                            enemyDetailsPanel.transform.Find("MPText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.curMP.ToString() + "/"
                                + GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.baseMP.ToString();
                            enemyDetailsPanel.transform.Find("MPProgressBarBG/MPProgressBar").transform.localScale =
                                new Vector2(Mathf.Clamp(GetEnemyProgressBarValuesMP(GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy), 0, 1),
                                enemyDetailsPanel.transform.Find("MPProgressBarBG/MPProgressBar").transform.localScale.y);

                            DrawThreatBar(GetUnitOnTile().GetComponent<EnemyBehavior>());

                            BSM.HidePanel(heroDetailsPanel);
                            BSM.ShowPanel(enemyDetailsPanel);
                        }
                    } else
                    {
                        BSM.HidePanel(heroDetailsPanel);
                        BSM.HidePanel(enemyDetailsPanel);
                    }

                    if (Input.GetButtonDown("Cancel") && !cancelPressed)
                    {
                        cancelPressed = true;
                        /*if (tileCursor.position.x == playerMove.currentTile.transform.position.x && tileCursor.position.y == playerMove.currentTile.transform.position.y)
                        {
                            HideTileCursor();
                            BattleCameraManager.instance.camState = camStates.CHOOSEACTION;
                            ShowBattleUI();
                            cursorOnAction = 0;
                            ShowCursor();
                            cursorState = CursorStates.ACTION;
                            actionState = ActionStates.PREACTION;

                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            BSM.HidePanel(heroDetailsPanel);
                            BSM.HidePanel(enemyDetailsPanel);
                        } else
                        {*/
                        AudioManager.instance.PlaySE(AudioManager.instance.backSE);
                        playerMove.GetCurrentTile();
                        tileCursor.position = new Vector3(playerMove.currentTile.transform.position.x, playerMove.currentTile.transform.position.y, 0);
                        //}
                    }

                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;
                        playerMove.GetCurrentTile();

                        if (tileCursor.position.x == playerMove.currentTile.transform.position.x && tileCursor.position.y == playerMove.currentTile.transform.position.y)
                        {
                            HideTileCursor();
                            BattleCameraManager.instance.camState = camStates.CHOOSEACTION;
                            ShowBattleUI();
                            cursorOnAction = 0;
                            ShowCursor();
                            cursorState = CursorStates.ACTION;
                            actionState = ActionStates.PREACTION;

                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            BSM.HidePanel(heroDetailsPanel);
                            BSM.HidePanel(enemyDetailsPanel);
                        } else
                        {
                            if (GetTileOnCursor().selectable && GetUnitOnTile() == null)
                            {
                                playerMove.currentTile.SelectMoveTile(playerMove);
                            } else
                            {
                                AudioManager.instance.PlaySE(AudioManager.instance.cantActionSE);
                            }                            
                        }
                    }

                    //show unit details window based on cursor position here

                    break;

                case (CursorStates.ACTION):
                    if (actionState == ActionStates.PREACTION)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1 && !dpadPressed && cursorOnAction == 0) //down
                        {
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            cursorOnAction = 1;
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1 && !dpadPressed && cursorOnAction == 1) //up
                        {
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            cursorOnAction = 0;
                        }

                        if (Input.GetButtonDown("Cancel") && !cancelPressed)
                        {
                            cancelPressed = true;

                            AudioManager.instance.PlaySE(AudioManager.instance.backSE);
                            playerMove.GetCurrentTile();
                            tileCursor.position = playerMove.currentTile.transform.position;
                            HideCursor();
                            ShowTileCursor();
                            HideBattleUI();
                            cursorState = CursorStates.MOVING;
                            BattleCameraManager.instance.camState = camStates.HEROTURN;
                        }

                        if (cursorOnAction == 0)
                        {
                            cursor.transform.localPosition = new Vector3(-348f, -145f, 0f);
                            detailsText.text = "Perform an attack, cast magic, or use an item";
                        }
                        else if (cursorOnAction == 1)
                        {
                            cursor.transform.localPosition = new Vector3(-348f, -199f, 0f);
                            detailsText.text = "Take 50% damage until next turn";
                        }

                        if (Input.GetButtonDown("Confirm") && !confirmPressed)
                        {
                            confirmPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            if (cursorOnAction == 0)
                            {
                                BSM.ActionInput();
                                actionState = ActionStates.ACTION;
                            }
                            else if (cursorOnAction == 1)
                            {
                                BSM.DefendInput();
                                detailsText.text = "";
                                cursorState = CursorStates.IDLE;
                                actionState = ActionStates.IDLE;
                                HideCursor();
                            }
                        }
                    }
                    
                    if (actionState == ActionStates.ACTION)
                    {
                        if (Input.GetAxisRaw("DpadVertical") == -1 && !dpadPressed && cursorOnAction >= 0 && cursorOnAction <= 1) //down
                        {
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            cursorOnAction = cursorOnAction + 1;
                        }

                        if (Input.GetAxisRaw("DpadVertical") == 1 && !dpadPressed && cursorOnAction <= 2 && cursorOnAction >= 1) //up
                        {
                            dpadPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            cursorOnAction = cursorOnAction - 1;
                        }

                        if (Input.GetButtonDown("Cancel") && !cancelPressed)
                        {
                            cancelPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.backSE);

                            cursorOnAction = 0;
                            BSM.HidePanel(GameObject.Find("BattleCanvas/BattleUI/ActionPanel"));
                            BSM.ShowPanel(GameObject.Find("BattleCanvas/BattleUI/MoveActionPanel"));
                            actionState = ActionStates.PREACTION;
                        }

                        if (Input.GetButtonDown("Confirm") && !confirmPressed)
                        {
                            confirmPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            if (cursorOnAction == 0) //attack
                            {
                                BSM.AttackInput();
                                HideCursor();
                                ShowTileCursor();
                                HideBattleUI();
                                chosenAction = "Attack";
                                cursorState = CursorStates.CHOOSINGTARGET;
                            }
                            else if (cursorOnAction == 1) //magic
                            {
                                BSM.MagicInput();
                                cursorOnAction = 0;
                                chosenAction = "Magic";
                                actionState = ActionStates.MAGIC;
                            }
                            else if (cursorOnAction == 2) //item
                            {
                                BSM.ItemInput();
                                cursorOnAction = 0;
                                chosenAction = "Item";
                                actionState = ActionStates.ITEM;
                            }
                        }

                        if (cursorOnAction == 0)
                        {
                            cursor.localPosition = new Vector3(-348f, -136f, 0f);
                            detailsText.text = "Perform a physical attack";
                        } else if (cursorOnAction == 1)
                        {
                            cursor.localPosition = new Vector3(-348f, -172f, 0f);
                            detailsText.text = "Perform a magic attack";
                        } else if (cursorOnAction == 2)
                        {
                            cursor.localPosition = new Vector3(-348f, -208f, 0f);
                            detailsText.text = "Use an item from your inventory";
                        }
                    }

                    if (actionState == ActionStates.MAGIC)
                    {
                        int magicCount = GameObject.Find("BattleCanvas/BattleUI/MagicPanel/MagicScroller/MagicSpacer").transform.childCount;

                        int scrollDiff = magicCount - 5;

                        if (magicCount <= 5)
                        {
                            tempScrollDiff = 0;
                        }

                        RectTransform spacerScroll = GameObject.Find("BattleCanvas/BattleUI/MagicPanel/MagicScroller/MagicSpacer").GetComponent<RectTransform>();

                        //Debug.Log(cursorOnItem);

                        if (!dpadPressed && cursorOnAction == 0 && tempScrollDiff == 0)
                        {
                            if (magicCount > 1)
                            {
                                if (Input.GetAxisRaw("DpadVertical") == -1) //down
                                {
                                    cursorOnAction = cursorOnAction + 1;
                                    dpadPressed = true;
                                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                                }
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 0 && tempScrollDiff > 0)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                cursorOnAction = cursorOnAction + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }

                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                spacerScroll.anchoredPosition = new Vector3(0.0f, (panelItemScrollSpacer * (tempScrollDiff - 1)), 0.0f);

                                tempScrollDiff = tempScrollDiff - 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction > 0 && cursorOnAction < 4)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = cursorOnAction - 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }

                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                cursorOnAction = cursorOnAction + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 4 && tempScrollDiff == 0 && scrollDiff > 0)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = cursorOnAction - 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                spacerScroll.anchoredPosition = new Vector3(0.0f, panelItemScrollSpacer, 0.0f);

                                tempScrollDiff = tempScrollDiff + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 4 && tempScrollDiff == 0 && scrollDiff == 0)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = cursorOnAction - 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 4 && tempScrollDiff > 0 && (cursorOnAction + tempScrollDiff) < (magicCount - 1))
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = 3;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }

                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                spacerScroll.anchoredPosition = new Vector3(0.0f, (panelItemScrollSpacer * (tempScrollDiff + 1)), 0.0f); //and use tempScrollDiff - 1 to go up

                                tempScrollDiff = tempScrollDiff + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 4 && tempScrollDiff > 0 && (cursorOnAction + tempScrollDiff) == (magicCount - 1))
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = 3;

                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }

                        if (magicCount != 0)
                        {
                            if (cursorOnAction == magicCount)
                            {
                                cursorOnAction = magicCount - 1;
                            }

                            if (cursorOnAction == 0 && tempScrollDiff == 0)
                            {
                                cursor.localPosition = new Vector3(-377f, -132.5f, 0f);
                                magicSelected = GameObject.Find("BattleCanvas/BattleUI/MagicPanel/MagicScroller/MagicSpacer").transform.GetChild(0).gameObject;
                            }
                            else if (cursorOnAction == 0 && tempScrollDiff > 0)
                            {
                                cursor.localPosition = new Vector3(-377f, -132.5f, 0f);
                                magicSelected = GameObject.Find("BattleCanvas/BattleUI/MagicPanel/MagicScroller/MagicSpacer").transform.GetChild(tempScrollDiff).gameObject;
                            }
                            else if (cursorOnAction > 0 && tempScrollDiff > 0)
                            {
                                cursor.localPosition = new Vector3(-377f, (-132.5f - (panelItemSpacer * cursorOnAction)), 0f);
                                magicSelected = GameObject.Find("BattleCanvas/BattleUI/MagicPanel/MagicScroller/MagicSpacer").transform.GetChild(cursorOnAction + tempScrollDiff).gameObject;
                            }
                            else
                            {
                                cursor.localPosition = new Vector3(-377f, (-132.5f - (panelItemSpacer * cursorOnAction)), 0f);
                                magicSelected = GameObject.Find("BattleCanvas/BattleUI/MagicPanel/MagicScroller/MagicSpacer").transform.GetChild(cursorOnAction).gameObject;
                            }


                            detailsText.text = AttackDB.instance.GetAttack(int.Parse(magicSelected.name.Replace("MagicButton - ID ", ""))).description;
                        }

                        if (Input.GetButtonDown("Cancel") && !cancelPressed)
                        {
                            cancelPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.backSE);

                            cursorOnAction = 1;
                            BSM.HidePanel(GameObject.Find("BattleCanvas/BattleUI/MagicPanel"));
                            BSM.ShowPanel(GameObject.Find("BattleCanvas/BattleUI/ActionPanel"));
                            actionState = ActionStates.ACTION;
                        }

                        if (Input.GetButtonDown("Confirm") && !confirmPressed)
                        {
                            confirmPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            Debug.Log("cast: " + magicSelected.transform.Find("ButtonCanvas/AttackName").GetComponent<Text>().text);
                            magicSelected.GetComponent<MagicAttackButton>().CastMagicAttack();

                            playerMove.GetCurrentTile();
                            tileCursor.position = new Vector3(playerMove.currentTile.transform.position.x, playerMove.currentTile.transform.position.y, 0f);

                            HideCursor();
                            ShowTileCursor();
                            HideBattleUI();

                            cursorState = CursorStates.CHOOSINGTARGET;
                        }

                    }

                    if (actionState == ActionStates.ITEM)
                    {
                        int itemCount = GameObject.Find("BattleCanvas/BattleUI/ItemPanel/ItemScroller/ItemSpacer").transform.childCount;

                        int scrollDiff = itemCount - 5;

                        if (itemCount <= 5)
                        {
                            tempScrollDiff = 0;
                        }

                        RectTransform spacerScroll = GameObject.Find("BattleCanvas/BattleUI/ItemPanel/ItemScroller/ItemSpacer").GetComponent<RectTransform>();

                        //Debug.Log(cursorOnItem);

                        if (!dpadPressed && cursorOnAction == 0 && tempScrollDiff == 0)
                        {
                            if (itemCount > 1)
                            {
                                if (Input.GetAxisRaw("DpadVertical") == -1) //down
                                {
                                    cursorOnAction = cursorOnAction + 1;
                                    dpadPressed = true;
                                    AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                                }
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 0 && tempScrollDiff > 0)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                cursorOnAction = cursorOnAction + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }

                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                spacerScroll.anchoredPosition = new Vector3(0.0f, (panelItemScrollSpacer * (tempScrollDiff - 1)), 0.0f);

                                tempScrollDiff = tempScrollDiff - 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction > 0 && cursorOnAction < 4)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = cursorOnAction - 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }

                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                cursorOnAction = cursorOnAction + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 4 && tempScrollDiff == 0 && scrollDiff > 0)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = cursorOnAction - 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                spacerScroll.anchoredPosition = new Vector3(0.0f, panelItemScrollSpacer, 0.0f);

                                tempScrollDiff = tempScrollDiff + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 4 && tempScrollDiff == 0 && scrollDiff == 0)
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = cursorOnAction - 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 4 && tempScrollDiff > 0 && (cursorOnAction + tempScrollDiff) < (itemCount - 1))
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = 3;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }

                            if (Input.GetAxisRaw("DpadVertical") == -1) //down
                            {
                                spacerScroll.anchoredPosition = new Vector3(0.0f, (panelItemScrollSpacer * (tempScrollDiff + 1)), 0.0f); //and use tempScrollDiff - 1 to go up

                                tempScrollDiff = tempScrollDiff + 1;
                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }
                        else if (!dpadPressed && cursorOnAction == 4 && tempScrollDiff > 0 && (cursorOnAction + tempScrollDiff) == (itemCount - 1))
                        {
                            if (Input.GetAxisRaw("DpadVertical") == 1) //up
                            {
                                cursorOnAction = 3;

                                dpadPressed = true;
                                AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            }
                        }

                        if (itemCount != 0)
                        {
                            if (cursorOnAction == itemCount)
                            {
                                cursorOnAction = itemCount - 1;
                            }

                            if (cursorOnAction == 0 && tempScrollDiff == 0)
                            {
                                cursor.localPosition = new Vector3(-377f, -132.5f, 0f);
                                itemSelected = GameObject.Find("BattleCanvas/BattleUI/ItemPanel/ItemScroller/ItemSpacer").transform.GetChild(0).gameObject;
                            }
                            else if (cursorOnAction == 0 && tempScrollDiff > 0)
                            {
                                cursor.localPosition = new Vector3(-377f, -132.5f, 0f);
                                itemSelected = GameObject.Find("BattleCanvas/BattleUI/ItemPanel/ItemScroller/ItemSpacer").transform.GetChild(tempScrollDiff).gameObject;
                            }
                            else if (cursorOnAction > 0 && tempScrollDiff > 0)
                            {
                                cursor.localPosition = new Vector3(-377f, (-132.5f - (panelItemSpacer * cursorOnAction)), 0f);
                                itemSelected = GameObject.Find("BattleCanvas/BattleUI/ItemPanel/ItemScroller/ItemSpacer").transform.GetChild(cursorOnAction + tempScrollDiff).gameObject;
                            }
                            else
                            {
                                cursor.localPosition = new Vector3(-377f, (-132.5f - (panelItemSpacer * cursorOnAction)), 0f);
                                itemSelected = GameObject.Find("BattleCanvas/BattleUI/ItemPanel/ItemScroller/ItemSpacer").transform.GetChild(cursorOnAction).gameObject;
                            }

                            detailsText.text = ItemDB.instance.GetItem(int.Parse(itemSelected.name.Replace("ItemButton - ID ", ""))).item.description;
                        }

                        if (Input.GetButtonDown("Cancel") && !cancelPressed)
                        {
                            cancelPressed = true;

                            AudioManager.instance.PlaySE(AudioManager.instance.backSE);
                            cursorOnAction = 2;
                            BSM.HidePanel(GameObject.Find("BattleCanvas/BattleUI/ItemPanel"));
                            BSM.ShowPanel(GameObject.Find("BattleCanvas/BattleUI/ActionPanel"));
                            actionState = ActionStates.ACTION;
                        }

                        if (Input.GetButtonDown("Confirm") && !confirmPressed)
                        {
                            confirmPressed = true;
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);

                            Debug.Log("use: " + itemSelected.transform.Find("ButtonCanvas/ItemName").GetComponent<Text>().text);
                            itemSelected.GetComponent<ItemBattleMenuButton>().UseItem();

                            playerMove.GetCurrentTile();
                            tileCursor.position = new Vector3(playerMove.currentTile.transform.position.x, playerMove.currentTile.transform.position.y, 0f);

                            HideCursor();
                            ShowTileCursor();
                            HideBattleUI();

                            cursorState = CursorStates.CHOOSINGTARGET;
                        }
                    }

                    break;

                case (CursorStates.CHOOSINGTARGET):

                    RaycastHit2D[] resetHits = Physics2D.RaycastAll(tileCursor.position, Vector2.zero);

                    Tile tileToAttack = null;

                    if (Input.GetAxisRaw("DpadHorizontal") == -1 && !dpadPressed) //left
                    {
                        dpadPressed = true;
                        foreach (RaycastHit2D hit in resetHits)
                        {
                            if (hit.collider != null && hit.collider.gameObject.tag == "Tile")
                            {
                                hit.collider.gameObject.GetComponent<Tile>().ResetTilesInRange();
                            }
                        }

                        if (IfTileExists(new Vector3(tileCursor.position.x - 1f, tileCursor.position.y, 0)))
                        {
                            tileCursor.position = new Vector3(tileCursor.position.x - 1f, tileCursor.position.y, 0);
                            AudioManager.instance.PlaySE(AudioManager.instance.moveTileCursorSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadHorizontal") == 1 && !dpadPressed) //right
                    {
                        dpadPressed = true;
                        foreach (RaycastHit2D hit in resetHits)
                        {
                            if (hit.collider != null && hit.collider.gameObject.tag == "Tile")
                            {
                                hit.collider.gameObject.GetComponent<Tile>().ResetTilesInRange();
                            }
                        }

                        if (IfTileExists(new Vector3(tileCursor.position.x + 1f, tileCursor.position.y, 0)))
                        {
                            tileCursor.position = new Vector3(tileCursor.position.x + 1f, tileCursor.position.y, 0);
                            AudioManager.instance.PlaySE(AudioManager.instance.moveTileCursorSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadVertical") == 1 && !dpadPressed) //up
                    {
                        dpadPressed = true;
                        foreach (RaycastHit2D hit in resetHits)
                        {
                            if (hit.collider != null && hit.collider.gameObject.tag == "Tile")
                            {
                                hit.collider.gameObject.GetComponent<Tile>().ResetTilesInRange();
                            }
                        }

                        if (IfTileExists(new Vector3(tileCursor.position.x, tileCursor.position.y + 1f, 0)))
                        {
                            tileCursor.position = new Vector3(tileCursor.position.x, tileCursor.position.y + 1f, 0);
                            AudioManager.instance.PlaySE(AudioManager.instance.moveTileCursorSE);
                        }
                    }

                    if (Input.GetAxisRaw("DpadVertical") == -1 && !dpadPressed) //down
                    {
                        dpadPressed = true;
                        foreach (RaycastHit2D hit in resetHits)
                        {
                            if (hit.collider != null && hit.collider.gameObject.tag == "Tile")
                            {
                                hit.collider.gameObject.GetComponent<Tile>().ResetTilesInRange();
                            }
                        }

                        if (IfTileExists(new Vector3(tileCursor.position.x, tileCursor.position.y - 1f, 0)))
                        {
                            tileCursor.position = new Vector3(tileCursor.position.x, tileCursor.position.y - 1f, 0);
                            AudioManager.instance.PlaySE(AudioManager.instance.moveTileCursorSE);
                        }
                    }

                    if (GetUnitOnTile() != null)
                    {
                        if (GetUnitOnTile().tag == "Hero")
                        {
                            heroDetailsPanel.transform.Find("NameText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<HeroStateMachine>().hero.name;
                            heroDetailsPanel.transform.Find("LevelText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<HeroStateMachine>().hero.currentLevel.ToString();

                            heroDetailsPanel.transform.Find("HPText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<HeroStateMachine>().hero.curHP.ToString() + "/"
                                + GetUnitOnTile().GetComponent<HeroStateMachine>().hero.finalMaxHP.ToString();

                            heroDetailsPanel.transform.Find("HPProgressBarBG/HPProgressBar").transform.localScale =
                                new Vector2(Mathf.Clamp(GetProgressBarValuesHP(GetUnitOnTile().GetComponent<HeroStateMachine>().hero), 0, 1),
                                heroDetailsPanel.transform.Find("HPProgressBarBG/HPProgressBar").transform.localScale.y);

                            heroDetailsPanel.transform.Find("MPText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<HeroStateMachine>().hero.curMP.ToString() + "/"
                                + GetUnitOnTile().GetComponent<HeroStateMachine>().hero.finalMaxMP.ToString();
                            heroDetailsPanel.transform.Find("MPProgressBarBG/MPProgressBar").transform.localScale =
                                new Vector2(Mathf.Clamp(GetProgressBarValuesMP(GetUnitOnTile().GetComponent<HeroStateMachine>().hero), 0, 1),
                                heroDetailsPanel.transform.Find("MPProgressBarBG/MPProgressBar").transform.localScale.y);

                            BSM.HidePanel(enemyDetailsPanel);
                            BSM.ShowPanel(heroDetailsPanel);
                        }
                        else if (GetUnitOnTile().tag == "Enemy")
                        {
                            enemyDetailsPanel.transform.Find("NameText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.name;
                            enemyDetailsPanel.transform.Find("LevelText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.level.ToString();

                            enemyDetailsPanel.transform.Find("HPText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.curHP.ToString() + "/"
                                + GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.baseHP.ToString();

                            enemyDetailsPanel.transform.Find("HPProgressBarBG/HPProgressBar").transform.localScale =
                                new Vector2(Mathf.Clamp(GetEnemyProgressBarValuesHP(GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy), 0, 1),
                                enemyDetailsPanel.transform.Find("HPProgressBarBG/HPProgressBar").transform.localScale.y);

                            enemyDetailsPanel.transform.Find("MPText").GetComponent<Text>().text = GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.curMP.ToString() + "/"
                                + GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy.baseMP.ToString();
                            enemyDetailsPanel.transform.Find("MPProgressBarBG/MPProgressBar").transform.localScale =
                                new Vector2(Mathf.Clamp(GetEnemyProgressBarValuesMP(GetUnitOnTile().GetComponent<EnemyStateMachine>().enemy), 0, 1),
                                enemyDetailsPanel.transform.Find("MPProgressBarBG/MPProgressBar").transform.localScale.y);

                            DrawThreatBar(GetUnitOnTile().GetComponent<EnemyBehavior>());

                            BSM.HidePanel(heroDetailsPanel);
                            BSM.ShowPanel(enemyDetailsPanel);
                        }
                    }
                    else
                    {
                        BSM.HidePanel(heroDetailsPanel);
                        BSM.HidePanel(enemyDetailsPanel);
                    }

                    if (Input.GetButtonDown("Cancel") && !cancelPressed)
                    {
                        cancelPressed = true;
                        AudioManager.instance.PlaySE(AudioManager.instance.backSE);
                        if (chosenAction == "Attack")
                        {
                            cursorOnAction = 0;
                            BSM.ShowPanel(GameObject.Find("BattleCanvas/BattleUI/ActionPanel"));

                            playerMove.GetCurrentTile();
                            tileCursor.position = new Vector3(playerMove.currentTile.transform.position.x, playerMove.currentTile.transform.position.y, 0);

                            HideTileCursor();
                            ShowCursor();
                            ShowBattleUI();

                            cursorState = CursorStates.ACTION;
                            actionState = ActionStates.ACTION;
                        }
                        else if (chosenAction == "Magic")
                        {
                            BSM.ShowPanel(GameObject.Find("BattleCanvas/BattleUI/MagicPanel"));

                            HideTileCursor();
                            ShowCursor();
                            ShowBattleUI();

                            cursorState = CursorStates.ACTION;
                            actionState = ActionStates.MAGIC;
                        }
                        else if (chosenAction == "Item")
                        {
                            BSM.ShowPanel(GameObject.Find("BattleCanvas/BattleUI/ItemPanel"));

                            HideTileCursor();
                            ShowCursor();
                            ShowBattleUI();

                            cursorState = CursorStates.ACTION;
                            actionState = ActionStates.ITEM;
                        }

                        break;
                    }

                    RaycastHit2D[] updateHits = Physics2D.RaycastAll(tileCursor.position, Vector2.zero);

                    foreach (RaycastHit2D hit in updateHits)
                    {
                        if (hit.collider != null && hit.collider.gameObject.tag == "Tile")
                        {
                            tileToAttack = hit.collider.gameObject.GetComponent<Tile>();
                            tileToAttack.UpdateTilesInRange();
                        }
                    }

                    if (Input.GetButtonDown("Confirm") && !confirmPressed)
                    {
                        confirmPressed = true;
                        if (!GetTileOnCursor().IfTargetsInRange())
                        {
                            AudioManager.instance.PlaySE(AudioManager.instance.cantActionSE);
                        } else
                        {
                            AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                            tileToAttack.SelectActionTile();

                            BSM.HidePanel(heroDetailsPanel);
                            BSM.HidePanel(enemyDetailsPanel);
                            HideCursor();
                            HideTileCursor();
                            ShowBattleUI();

                            cursorState = CursorStates.IDLE;
                            actionState = ActionStates.IDLE;
                        }
                    }                 

                break;

                case (CursorStates.IDLE):

                break;
            }
        }
        
        if (Input.GetButtonUp("Confirm"))
        {
            confirmPressed = false;
        }

        if (Input.GetButtonUp("Cancel"))
        {
            cancelPressed = false;
        }

        if (Input.GetAxisRaw("DpadVertical") == 0 && Input.GetAxisRaw("DpadHorizontal") == 0)
        {
            dpadPressed = false;
        }
    }

    void HideTileCursor()
    {
        tileCursor.localScale = new Vector3(0.0f, 0.0f, 1.0f);
    }

    void ShowTileCursor()
    {
        tileCursor.localScale = new Vector3(cursorVisibleScale, cursorVisibleScale, 1.0f);
    }

    void HideCursor()
    {
        cursor.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    void ShowCursor()
    {
        cursor.gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }

    void HideBattleUI()
    {
        GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void ShowBattleUI()
    {
        GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    bool IfTileExists(Vector3 checkPos)
    {
        RaycastHit2D[] updateHits = Physics2D.RaycastAll(checkPos, Vector2.zero);

        foreach (RaycastHit2D hit in updateHits)
        {
            if (hit.collider != null && hit.collider.gameObject.tag == "Tile")
            {
                return true;
            }
        }
        return false;
    }

    Tile GetTileOnCursor()
{
        RaycastHit2D[] updateHits = Physics2D.RaycastAll(tileCursor.transform.position, Vector2.zero);

        foreach (RaycastHit2D hit in updateHits)
        {
            if (hit.collider != null && hit.collider.gameObject.tag == "Tile")
            {
                return hit.collider.gameObject.GetComponent<Tile>();
            }
        }

        return null;
    }

    GameObject GetUnitOnTile()
    {
        RaycastHit2D[] updateHits = Physics2D.RaycastAll(tileCursor.transform.position, Vector3.forward);

        foreach (RaycastHit2D hit in updateHits)
        {
            if (hit.collider != null && (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Hero"))
            {
                return hit.collider.gameObject;
            }
        }

        return null;
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
    /// Calculates the HP for progress bar for given enemy - returns their current HP / their max HP
    /// </summary>
    /// <param name="enemy">Enemy to gather HP data from</param>
    float GetEnemyProgressBarValuesHP(BaseEnemy enemy)
    {
        float enemyHP = enemy.curHP;
        float enemyBaseHP = enemy.baseHP;
        float calc_HP;

        calc_HP = enemyHP / enemyBaseHP;

        return calc_HP;
    }

    /// <summary>
    /// Calculates the MP for progress bar for given enemy - returns their current MP / their max MP
    /// </summary>
    /// <param name="enemy">Enemy to gather MP data from</param>
    float GetEnemyProgressBarValuesMP(BaseEnemy enemy)
    {
        float enemyMP = enemy.curMP;
        float enemyBaseMP = enemy.baseMP;
        float calc_MP;

        calc_MP = enemyMP / enemyBaseMP;

        return calc_MP;
    }

    /// <summary>
    /// Sets Threat Bar on enemyDetailsPanel
    /// </summary>
    public void DrawThreatBar(EnemyBehavior eb)
    {
        GameObject hero = BSM.HeroesToManage[0];

        Image threatBar = enemyDetailsPanel.transform.Find("ThreatProgressBarBG/ThreatProgressBar").GetComponent<Image>();

        foreach (BaseThreat threat in eb.threatList)
        {
            if (threat.heroObject == hero && threat.threat > 0)
            {
                float threatVal = threat.threat;
                float calc_Threat = threatVal / eb.maxThreat; //does math of % of ATB gauge to be filled
                threatBar.transform.localScale = new Vector2(Mathf.Clamp(calc_Threat, 0, 1), threatBar.transform.localScale.y); //shows graphic of threat gauge increasing

                if (calc_Threat == 1)
                {
                    threatBar.color = eb.maxThreatColor;
                }
                else if (calc_Threat >= .75f && calc_Threat <= .99f)
                {
                    threatBar.color = eb.highThreatColor;
                }
                else if (calc_Threat >= .50f && calc_Threat <= .75f)
                {
                    threatBar.color = eb.moderateThreatcolor;
                }
                else if (calc_Threat >= .25f && calc_Threat <= .50f)
                {
                    threatBar.color = eb.lowThreatcolor;
                }
                else
                {
                    threatBar.color = eb.veryLowThreatColor;
                }

                enemyDetailsPanel.transform.Find("ThreatText").GetComponent<Text>().text = threat.threat.ToString();

                return;
            }
        }

        //if hero is not in threat list
        threatBar.color = Color.clear;
        enemyDetailsPanel.transform.Find("ThreatText").GetComponent<Text>().text = "0";
    }

    /// <summary>
    /// Instantiates hero panel with hero details
    /// </summary>
    void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject; //creates gameobject of heroPanel prefab (display in BattleCanvas which shows ATB gauge and HP, MP, etc)
        stats = HeroPanel.GetComponent<HeroPanelStats>(); //gets the hero panel's stats script
        stats.HeroName.text = hero.name; //sets hero name in the hero panel to the current hero's name
        stats.HeroHP.text = "HP: " + hero.curHP + "/" + hero.finalMaxHP; //sets HP in the hero panel to the current hero's HP
        stats.HeroMP.text = "MP: " + hero.curMP + "/" + hero.finalMaxMP; //sets MP in the hero panel to the current hero's MP
        ProgressBar = stats.ProgressBar; //sets ATB gauge in the hero panel to the hero's ATB
        HeroPanel.transform.SetParent(HeroPanelSpacer, false); //sets the hero panel to the hero panel's spacer for vertical layout group
        HeroPanel.name = "BattleHeroPanel - ID " + hero.ID;
    }

    /// <summary>
    /// Returns list of gameobjects in the given affect range
    /// </summary>
    /// <param name="affectIndex">Given affect index from attack</param>
    /// <param name="targetChoice">Given target to be in the center of the affect range</param>
    List<GameObject> GetTargetsInAffect(int affectIndex, GameObject targetChoice)
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        
        RaycastHit2D[] tileHits = Physics2D.RaycastAll(targetChoice.transform.position, Vector3.back, 1);

        foreach (RaycastHit2D tile in tileHits)
        {
            if (tile.collider.gameObject.tag == "Tile")
            {
                Tile t = tile.collider.gameObject.GetComponent<Tile>();
                pattern.GetAffectPattern(t, affectIndex);
                tilesInRange = pattern.pattern;
                break;
            }
        }

        foreach (Tile t in tilesInRange)
        {
            RaycastHit2D[] tilesHit = Physics2D.RaycastAll(t.transform.position, Vector3.forward, 1);
            foreach (RaycastHit2D target in tilesHit)
            {
                Debug.Log(target.collider.gameObject.name);
                if (!targets.Contains(target.collider.gameObject) && target.collider.gameObject.tag != "Tile" && target.collider.gameObject.tag != "Shieldable")
                {
                    Debug.Log("adding " + target.collider.gameObject + " to targets");
                    if (!targetsAccountedFor.Contains(target.collider.gameObject))
                    {
                        targets.Add(target.collider.gameObject); //adds all objects inside target range to targets list to be affected
                        targetsAccountedFor.Add(target.collider.gameObject);
                    }
                }
            }
        }

        targetsAccountedFor.Clear();

        return targets;
    }

    /// <summary>
    /// Enables inAffect on given tiles for provided attack
    /// </summary>
    /// <param name="attack">Given attack to gather affect pattern</param>
    /// <param name="parentTile">Parent tile to be the center of the affect pattern</param>
    void ShowAffectPattern(BaseAttack attack, Tile parentTile)
    {
        List<Tile> affectPattern = pattern.GetAffectPattern(parentTile, attack.patternIndex);

        foreach (Tile rangeTile in affectPattern.ToArray())
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(rangeTile.transform.position, Vector3.forward, 1);
            foreach (RaycastHit2D target in hits)
            {
                if (target.collider.gameObject.tag != "Shieldable")
                {
                    Debug.Log(target.collider.gameObject.name + " is shieldable - inAffect");
                    rangeTile.inAffect = true;
                }
            }
        }
    }

    /// <summary>
    /// Enables inRange on given tiles for provided attack
    /// </summary>
    /// <param name="attack">Given attack to gather range pattern</param>
    /// <param name="parentTile">Parent tile to be the center of the range pattern</param>
    void ShowRangePattern(BaseAttack attack, Tile parentTile)
    {
        List<Tile> rangePattern = pattern.GetRangePattern(parentTile, attack.rangeIndex);

        foreach (Tile rangeTile in rangePattern.ToArray())
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(rangeTile.transform.position, Vector3.forward, 1);
            foreach (RaycastHit2D target in hits)
            {
                if (target.collider.gameObject.tag != "Shieldable")
                {
                    Debug.Log(target.collider.gameObject.name + " is shieldable - inRange");
                    rangeTile.inRange = true;
                }
            }
        }
    }

    /// <summary>
    /// Disables inAffect and inRange for all tile gameObjects
    /// </summary>
    void ClearTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tileObj in tiles)
        {
            Tile tile = tileObj.GetComponent<Tile>();
            tile.inAffect = false;
            tile.inRange = false;
        }
    }

    /// <summary>
    /// Called when hero action choice is made to process necessary methods
    /// </summary>
    private void TimeForAction()
    {
        BSM.lastHeroToProcess = gameObject;

        if (BSM.PerformList[0].actionType == ActionType.ATTACK)
        {
            StartCoroutine(AttackAnimation());
        }

        if (BSM.PerformList[0].actionType == ActionType.MAGIC)
        {
            StartCoroutine(MagicAnimation());
        }

        if (BSM.PerformList[0].actionType == ActionType.ITEM)
        {
            StartCoroutine(ItemAnimation());
        }
        
        if (BSM.battleState != battleStates.WIN && BSM.battleState != battleStates.LOSE) //if the battle is still going (didn't win or lose)
        {
            BSM.battleState = battleStates.WAIT; //sets battle state machine back to WAIT

            cur_cooldown = 0f; //reset the hero's ATB gauge to 0
            currentState = TurnState.PROCESSING; //starts the turn over back to filling up the ATB gauge
        } else
        {
            currentState = TurnState.WAITING; //if the battle is in win or lose state, turns the hero back to WAITING (idle) state
        }
    }

    /// <summary>
    /// Coroutine.  Processes attack animation and damage
    /// </summary>
    public IEnumerator AttackAnimation()
    {
        if (actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have gone through it already
        }

        actionStarted = true;

        BattleCameraManager.instance.physAttackObj = targets[0];
        BattleCameraManager.instance.camState = camStates.ATTACK;

        //animate the hero to the enemy (this is where attack animations will go)
        //Vector2 enemyPosition = new Vector2(ActionTarget.transform.position.x + 1.5f, ActionTarget.transform.position.y); //sets enemyPosition to the chosen enemy's position + a few pixels on the x axis
        //while (MoveToTarget(enemyPosition)) { yield return null; } //moves the hero to the calculated position above

        heroAnim = gameObject.GetComponent<Animator>();

        SetHeroFacingDir(targets[0], "atkDirX", "atkDirY");

        while (!BattleCameraManager.instance.physAttackCameraZoomFinished)
        {
            yield return null;
        }

        yield return new WaitForSeconds(.2f); //wait a bit

        heroAnim.SetBool("onPhysAtk", true);

        yield return new WaitForSeconds(.25f);

        AttackAnimation animation = GameObject.Find("BattleManager/AttackAnimationManager").GetComponent<AttackAnimation>();
        animation.attack = BSM.PerformList[0].chosenAttack;
        animation.target = targets[0];
        animation.BuildAnimation();

        StartCoroutine(animation.PlayAnimation());

        yield return new WaitForSeconds(animation.attackDur); //wait a bit

        BattleCameraManager.instance.physAttackAnimFinished = true;

        Debug.Log("addedEffect: " + animation.addedEffectAchieved);

        BaseAddedEffect BAE = new BaseAddedEffect();
        BAE.target = targets[0];
        BAE.addedEffectProcced = animation.addedEffectAchieved;
        checkAddedEffects.Add(BAE);

        animation.addedEffectAchieved = false;

        heroAnim.SetBool("onPhysAtk", false);

        Animator tarAnim = targets[0].GetComponent<Animator>();
        SetTargetFacingDir(targets[0], "rcvDamX", "rcvDamY");

        int hitRoll = GetRandomInt(0, 100);
        if (hitRoll <= hero.GetHitChance(hero.finalHitRating, hero.finalAgility))
        {
            Debug.Log("turning on takeDamage - " + targets[0].name);
            tarAnim.SetBool("onRcvDam", true);

            Debug.Log("Hero hits!");
            int critRoll = GetRandomInt(0, 100);
            if (critRoll <= hero.GetCritChance(hero.finalCritRating, hero.finalDexterity))
            {
                Debug.Log("Hero crits!");
                ProcessAttack(targets[0], true); //do damage with calculations (this will change later)
            }
            else
            {
                Debug.Log("Hero doesn't crit.");
                ProcessAttack(targets[0], false); //do damage with calculations (this will change later)
            }
            Debug.Log(hero.GetCritChance(hero.finalCritRating, hero.finalDexterity) + "% chance to crit, roll was: " + critRoll);
        }
        else
        {
            StartCoroutine(BSM.ShowMiss(ActionTarget));
            Debug.Log(hero.name + " missed!");
        }
        Debug.Log(hero.GetHitChance(hero.finalHitRating, hero.finalAgility) + "% chance to hit, roll was: " + hitRoll);

        //animate the enemy back to start position
        //Vector2 firstPosition = startPosition; //changes the hero's position back to the starting position

        //while (MoveToTarget(firstPosition)) { yield return null; } //move the hero back to the starting position     

        yield return new WaitForSeconds(BSM.damageDisplayTime);

        PostAnimationCleanup();

        actionStarted = false;
    }

    /// <summary>
    /// Coroutine.  Processes magic animation and damage
    /// </summary>
    public IEnumerator MagicAnimation()
    {
        if (actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have gone through it already
        }

        actionStarted = true;

        BattleCameraManager.instance.camState = camStates.ATTACK;

        Debug.Log("Casting: " + BSM.PerformList[0].chosenAttack.name);

        heroAnim = gameObject.GetComponent<Animator>();

        SetHeroFacingDir(targets[0], "atkDirX", "atkDirY");

        yield return new WaitForSeconds(.2f); //wait a bit

        StartCoroutine(BSM.ShowAttackName(BSM.PerformList[0].chosenAttack.name, AudioManager.instance.magicCast.length));

        heroAnim.SetBool("onMagAtk", true);

        AttackAnimation animation = GameObject.Find("BattleManager/AttackAnimationManager").GetComponent<AttackAnimation>();
        animation.attack = BSM.PerformList[0].chosenAttack;

        animation.PlayCastingAnimation(gameObject);

        yield return new WaitForSeconds(AudioManager.instance.magicCast.length);

        BattleCameraManager.instance.magicCastingAnimFinished = true;

        foreach (GameObject target in targets)
        {

            BattleCameraManager.instance.currentMagicTarget = target;

            animation.target = target;
            animation.BuildAnimation();

            StartCoroutine(animation.PlayAnimation());

            yield return new WaitForSeconds(animation.attackDur); //wait a bit

            Debug.Log("addedEffect: " + animation.addedEffectAchieved);

            BaseAddedEffect BAE = new BaseAddedEffect();
            BAE.target = target;
            BAE.addedEffectProcced = animation.addedEffectAchieved;
            checkAddedEffects.Add(BAE);

            animation.addedEffectAchieved = false;
        }

        Destroy(GameObject.Find("Casting animation"));

        if (!targets.Contains(gameObject))
        {
            heroAnim.SetBool("onMagAtk", false);
        }

        foreach (GameObject target in targets)
        {
            Animator tarAnim = target.GetComponent<Animator>();
            SetTargetFacingDir(target, "rcvDamX", "rcvDamY");

            if (BSM.PerformList[0].chosenAttack.magicClass != BaseAttack.MagicClass.WHITE)
            tarAnim.SetBool("onRcvDam", true);

            ProcessAttack(target, false);

            if (BSM.PerformList[0].chosenAttack.magicClass == BaseAttack.MagicClass.WHITE)
            {
                StartCoroutine(BSM.ShowHeal(magicDamage, target));
            }
            else
            {
                foreach (BaseDamage BD in finalDamages)
                {
                    //Debug.Log("BD obj name:" + BD.obj.name);
                    //Debug.Log("BD dmg: " + BD.finalDamage);
                    if (BD.obj == target)
                    {
                        StartCoroutine(BSM.ShowDamage(BD.finalDamage, target));
                        break;
                    }
                }
            }
        }

        yield return new WaitForSeconds(BSM.damageDisplayTime);

        gameObject.GetComponent<HeroStateMachine>().hero.curMP -= BSM.PerformList[0].chosenAttack.MPCost;

        heroAnim.SetBool("onMagAtk", false);

        PostAnimationCleanup();

        actionStarted = false;
    }

    /// <summary>
    /// Coroutine.  Processes item animation and effects
    /// </summary>
    public IEnumerator ItemAnimation()
    {
        if (actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have gone through it already
        }

        actionStarted = true;

        BattleCameraManager.instance.camState = camStates.ATTACK;

        StartCoroutine(BSM.ShowAttackName(BSM.PerformList[0].chosenItem.name, AudioManager.instance.magicCast.length));

        yield return new WaitForSeconds(1.0f); //wait a bit

        AttackAnimation animation = GameObject.Find("BattleManager/AttackAnimationManager").GetComponent<AttackAnimation>();
        animation.item = BSM.PerformList[0].chosenItem;

        animation.PlayUsingItemAnimation(gameObject);

        BattleCameraManager.instance.itemAnimFinished = true;

        PerformItem(); //process the item

        foreach (GameObject target in targets)
        {
            BattleCameraManager.instance.itemUsedUnit = target;

            animation.target = target;
            animation.BuildItemAnimation();

            StartCoroutine(animation.PlayItemAnimation());

            yield return new WaitForSeconds(animation.itemDur); //wait a bit

            if (BSM.PerformList[0].chosenItem.type == Item.Types.RESTORATIVE)
            {
                StartCoroutine(BSM.ShowHeal(itemDamage, target));
            }
        }

        yield return new WaitForSeconds(BSM.damageDisplayTime);

        BSM.ResetItemList();

        PostAnimationCleanup();

        actionStarted = false;
    }

    /// <summary>
    /// Processes post animation methods
    /// </summary>
    void PostAnimationCleanup()
    {
        GetStatusEffectsFromCurrentAttack();

        checkAddedEffects.Clear();

        finalDamages.Clear();

        UpdateHeroStats();

        GameObject[] attackAnims = GameObject.FindGameObjectsWithTag("AttackAnimation");
        foreach (GameObject obj in attackAnims)
        {
            Destroy(obj);
        }

        playerMove.EndTurn(this);
    }

    void SetTargetFacingDir(GameObject target, string paramNameX, string paramNameY)
    {
        Animator tarAnim = target.GetComponent<Animator>();

        float xDiff = gameObject.transform.position.x - target.transform.position.x;
        float yDiff = gameObject.transform.position.y - target.transform.position.y;
        tarAnim.SetFloat(paramNameX, 0);
        tarAnim.SetFloat(paramNameY, 0);

        if (xDiff < 0)
        {
            tarAnim.SetFloat(paramNameX, -1f);
        }
        else if (xDiff > 0)
        {
            tarAnim.SetFloat(paramNameX, 1f);
        }

        if (yDiff < 0)
        {
            tarAnim.SetFloat(paramNameY, -1f);
        }
        else if (yDiff > 0)
        {
            tarAnim.SetFloat(paramNameY, 1f);
        }
    }

    void SetHeroFacingDir(GameObject target, string paramNameX, string paramNameY)
    {
        heroAnim.SetFloat(paramNameX, 0);
        heroAnim.SetFloat(paramNameY, 0);

        float xDiff = gameObject.transform.position.x - target.transform.position.x;
        float yDiff = gameObject.transform.position.y - target.transform.position.y;

        if (xDiff < 0)
        {
            heroAnim.SetFloat(paramNameX, 1f);
            heroAnim.SetFloat("moveX", 1f);
        }
        else if (xDiff > 0)
        {
            heroAnim.SetFloat(paramNameX, -1f);
            heroAnim.SetFloat("moveX", -1f);
        }

        if (yDiff < 0)
        {
            heroAnim.SetFloat(paramNameY, 1f);
            heroAnim.SetFloat("moveY", 1f);
        }
        else if (yDiff > 0)
        {
            heroAnim.SetFloat(paramNameY, -1f);
            heroAnim.SetFloat("moveY", -1f);
        }
    }

    /// <summary>
    /// Processes animation for hero gameObject to run to given target position
    /// </summary>
    /// <param name="target">Target position to run to for action animation</param>
    private bool MoveToTarget(Vector3 target)
    {
        return target != (transform.position = Vector2.MoveTowards(transform.position, target, animSpeed * Time.deltaTime)); //moves toward target until position is same as target position
    }

    /// <summary>
    /// Processes lowering HP for hero based on given value
    /// </summary>
    /// <param name="getDamageAmount">Damage for hero to receive</param>
    public void TakeDamage(int getDamageAmount) //receives damage from enemy
    {
        hero.curHP -= getDamageAmount; //subtracts hero's current HP from getDamageAmount parameter
        if (hero.curHP <= 0)
        {
            hero.curHP = 0; //sets hero's HP to 0 if it is 0 or below
            currentState = TurnState.DEAD; //changes hero's state to DEAD
        }
        UpdateHeroStats(); //updates UI to show current HP and MP
    }

    /// <summary>
    /// Increases given threat value for provided enemy from provided hero
    /// </summary>
    /// <param name="enemy">Enemy to receive threat</param>
    /// <param name="hero">Hero to have threat added from</param>
    /// <param name="threat">Threat value to add</param>
    public void IncreaseThreat(GameObject enemy, BaseHero hero, float threat)
    {
        EnemyBehavior eb = enemy.GetComponent<EnemyBehavior>();
        foreach (BaseThreat t in eb.threatList)
        {
            if (hero.name == t.hero.name)
            {
                int threatToAdd = Mathf.RoundToInt(threat);
                t.threat += threatToAdd;

                if (t.threat > eb.maxThreat)
                {
                    eb.maxThreat = t.threat;
                    Debug.Log("Setting max threat: " + t.threat);
                }

                Debug.Log("Adding " + threat + " threat from hero " + hero.name + " to " + enemy.GetComponent<EnemyStateMachine>().enemy.name);
                break;
            }
        }
    }

    /// <summary>
    /// Lowers HP of target by simulating damage from hero
    /// </summary>
    /// <param name="target">GameObject of who is being attacked</param>
    /// <param name="crit">If true, process critical damage methods</param>
    void ProcessAttack(GameObject target, bool crit) //deals damage to enemy
    {
        int calc_damage = 0;

        if (target.tag == "Enemy")
        {
            if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.PHYSICAL)
            {
                calc_damage = hero.finalATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                if (crit)
                {
                    calc_damage = calc_damage * 2;
                    Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and crits for " + calc_damage + " damage to " + target.GetComponent<EnemyStateMachine>().enemy.name + "!");
                    Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + hero.finalATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " * 2 = " + calc_damage);
                    StartCoroutine(BSM.ShowCrit(calc_damage, target));
                } else
                {
                    Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<EnemyStateMachine>().enemy.name + "!");
                    Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + hero.finalATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);
                    StartCoroutine(BSM.ShowDamage(calc_damage, target));
                }

                float calc_threat = (((calc_damage / 2) + hero.finalThreatRating) * BSM.PerformList[0].chosenAttack.threatMultiplier);
                IncreaseThreat(target, hero, calc_threat);
            }
            else if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.MAGIC) //--process from magic script
            {
                //can check if magic attack should have a flat value, ie gravity spell
                //calc_damage = hero.curMATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's magic attack + the chosen attack's damage
                //Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + ActionTarget.GetComponent<EnemyStateMachine>().enemy.name + "!");
                //Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - magic - hero's MATK: " + hero.curMATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);

                BaseMagicScript magicScript = new BaseMagicScript();
                magicScript.spell = BSM.PerformList[0].chosenAttack; //sets the spell to be cast by the chosen spell
                magicScript.heroPerformingAction = hero; //sets hero performing action to this hero
                magicScript.enemyReceivingAction = target.GetComponent<EnemyStateMachine>().enemy; //sets the enemy receiving action to the target's enemy
                magicScript.hsm = this;

                float calc_threat = (((magicDamage / 2) + hero.finalThreatRating) * BSM.PerformList[0].chosenAttack.threatMultiplier);
                IncreaseThreat(target, hero, calc_threat);

                foreach (BaseAddedEffect BAE in checkAddedEffects)
                {
                    if (BAE.target == target)
                    {
                        magicScript.ProcessMagicHeroToEnemy(BAE.addedEffectProcced); //actually process the magic to hero

                        BaseDamage damage = new BaseDamage();
                        damage.obj = target;
                        damage.finalDamage = magicDamage;
                        finalDamages.Add(damage);

                        break;
                    }
                }

                //StartCoroutine(BSM.ShowDamage(magicDamage, target));
            }
            else //if attack type not found
            {
                calc_damage = hero.finalATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                if (crit)
                {
                    calc_damage = calc_damage * 2;
                }
                Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + ActionTarget.GetComponent<EnemyStateMachine>().enemy.name + "! -- NOTE: ATTACK TYPE NOT FOUND: " + BSM.PerformList[0].chosenAttack.type);
            }
            target.GetComponent<EnemyStateMachine>().enemyBehavior.TakeDamage(calc_damage); //processes enemy take damage by above value
            
        }
        if (target.tag == "Hero")
        {
            if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.PHYSICAL)
            {
                foreach (BaseAddedEffect BAE in checkAddedEffects)
                {
                    if (BAE.target == target)
                    {
                        calc_damage = hero.finalATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                        if (crit)
                        {
                            calc_damage = calc_damage * 2;
                            Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and crits for " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "!");
                            Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + hero.finalATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " * 2 = " + calc_damage);
                            if (BAE.addedEffectProcced)
                            {
                                calc_damage = Mathf.CeilToInt(calc_damage * 2f);
                            }
                            StartCoroutine(BSM.ShowCrit(calc_damage, target));
                        }
                        else
                        {
                            Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "!");
                            Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + hero.finalATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);
                            if (BAE.addedEffectProcced)
                            {
                                calc_damage = Mathf.CeilToInt(calc_damage * 2f);
                            }
                            StartCoroutine(BSM.ShowDamage(calc_damage, target));
                        }
                    }
                }                
            }
            else if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.MAGIC) //--process from magic script
            {
                //can check if magic attack should have a flat value, ie gravity spell
                //calc_damage = hero.curMATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's magic attack + the chosen attack's damage
                //Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + ActionTarget.GetComponent<HeroStateMachine>().hero.name + "!");
                //Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - magic - hero's MATK: " + hero.curMATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);

                BaseMagicScript magicScript = new BaseMagicScript();
                magicScript.spell = BSM.PerformList[0].chosenAttack; //sets the spell to be cast by the chosen spell
                magicScript.heroPerformingAction = hero; //sets hero performing action to this hero
                magicScript.heroReceivingAction = target.GetComponent<HeroStateMachine>().hero; //sets the hero receiving action to the target's hero
                magicScript.hsm = this;

                foreach (BaseAddedEffect BAE in checkAddedEffects)
                {
                    if (BAE.target == target)
                    {
                        magicScript.ProcessMagicHeroToHero(BAE.addedEffectProcced); //actually process the magic to hero

                        BaseDamage damage = new BaseDamage();
                        damage.obj = target;
                        damage.finalDamage = magicDamage;
                        finalDamages.Add(damage);

                        break;
                    }
                }
                //magicScript.ProcessMagicHeroToHero(); //actually process the magic to hero
                //StartCoroutine(BSM.ShowDamage(magicDamage, target));
            }
            else //if attack type not found
            {
                calc_damage = hero.finalATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                if (crit)
                {
                    calc_damage = calc_damage * 2;
                }
                Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "! -- NOTE: ATTACK TYPE NOT FOUND: " + BSM.PerformList[0].chosenAttack.type);
            }
            target.GetComponent<HeroStateMachine>().TakeDamage(calc_damage); //processes enemy take damage by above value
        }
        
    }

    /// <summary>
    /// Processes chosen item
    /// </summary>
    void PerformItem()
    {
        //action item

        BaseItemScript itemScript = new BaseItemScript();
        itemScript.scriptToRun = BSM.PerformList[0].chosenItem.name; //sets which item script to be run
        itemScript.hsm = this;
        foreach (GameObject target in targets)
        {
            if (target.tag == "Hero")
            {
                itemScript.ProcessItemToHero(target.GetComponent<HeroStateMachine>().hero);
            }
            if (target.tag == "Enemy")
            {
                itemScript.ProcessItemToEnemy(target.GetComponent<EnemyStateMachine>().enemy);
            }
        }
        
        Inventory.instance.Remove(BSM.PerformList[0].chosenItem);
    }

    /// <summary>
    /// Updates GUI with new HP/MP values after attack
    /// </summary>
    public void UpdateHeroStats()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (GameObject heroObj in heroes)
        {
            HeroStateMachine heroHSM = heroObj.GetComponent<HeroStateMachine>();
            heroHSM.stats.HeroHP.text = "HP: " + heroHSM.hero.curHP + "/" + heroHSM.hero.finalMaxHP;
            heroHSM.stats.HeroMP.text = "MP: " + heroHSM.hero.curMP + "/" + heroHSM.hero.finalMaxMP;

            foreach (BaseHero hero in GameManager.instance.activeHeroes)
            {
                if (hero.name == heroHSM.hero.name)
                {
                    hero.curHP = heroHSM.hero.curHP;
                    hero.curMP = heroHSM.hero.curMP;
                }
            }
        }
    }

    /// <summary>
    /// Recovers MP based on spirit and calculations
    /// </summary>
    public void RecoverMPAfterTurn()
    {
        if (hero.curMP < hero.baseMP)
        {
            hero.curMP += hero.GetRegen(hero.finalRegenRating, hero.finalSpirit);
            Debug.Log(hero.name + " recovering " + hero.GetRegen(hero.finalRegenRating, hero.finalSpirit) + " MP");
        }

        if (hero.curMP > hero.finalMaxMP)
        {
            hero.curMP = hero.finalMaxMP;
        }
        UpdateHeroStats();
    }

    /// <summary>
    /// Adds status effect(s) from current attack to current target
    /// </summary>
    public void GetStatusEffectsFromCurrentAttack()
    {
        //if target is ally, add to ally's active status effects. if target is enemy, add to enemy's list

        if (BSM.PerformList[0].chosenAttack != null && BSM.PerformList[0].chosenAttack.statusEffects.Count > 0)
        {
            foreach (BaseStatusEffect statusEffect in BSM.PerformList[0].chosenAttack.statusEffects)
            {
                foreach (GameObject target in targets)
                {
                    BaseEffect effectToApply = new BaseEffect();
                    effectToApply.effectName = statusEffect.name;
                    effectToApply.effectType = statusEffect.effectType.ToString();
                    effectToApply.turnsRemaining = statusEffect.turnsApplied;
                    effectToApply.baseValue = statusEffect.baseValue;
                    Debug.Log("Status effect: " + effectToApply.effectName + ", type: " + effectToApply.effectType + " - applied to " + target.name);
                    if (target.tag == "Enemy")
                    {
                        target.GetComponent<EnemyBehavior>().activeStatusEffects.Add(effectToApply);
                    }
                    else if (target.tag == "Hero")
                    {
                        target.GetComponent<HeroStateMachine>().activeStatusEffects.Add(effectToApply);
                    }

                }
            }
        } else if (BSM.PerformList[0].chosenItem != null && BSM.PerformList[0].chosenItem.statusEffects.Count > 0)
        {
            foreach (BaseStatusEffect statusEffect in BSM.PerformList[0].chosenItem.statusEffects)
            {
                foreach (GameObject target in targets)
                {
                    BaseEffect effectToApply = new BaseEffect();
                    effectToApply.effectName = statusEffect.name;
                    effectToApply.effectType = statusEffect.effectType.ToString();
                    effectToApply.turnsRemaining = statusEffect.turnsApplied;
                    effectToApply.baseValue = statusEffect.baseValue;
                    Debug.Log("Status effect: " + effectToApply.effectName + ", type: " + effectToApply.effectType + " - applied to " + target.name);
                    if (target.tag == "Enemy")
                    {
                        target.GetComponent<EnemyBehavior>().activeStatusEffects.Add(effectToApply);
                    }
                    else if (target.tag == "Hero")
                    {
                        target.GetComponent<HeroStateMachine>().activeStatusEffects.Add(effectToApply);
                    }

                }
            }
        }
    }

    /// <summary>
    /// Processes the status effect(s) from current attack to current target, lowers the turns remaining, and removs the status effect if needed
    /// </summary>
    public void ProcessStatusEffects()
    {
        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            StatusEffect effectToProcess = new StatusEffect();

            effectToProcess.hsm = this;

            effectToProcess.ProcessEffect(activeStatusEffects[i].effectName, activeStatusEffects[i].effectType, activeStatusEffects[i].baseValue, this.gameObject);
            
            if (effectDamage > 0)
            {
                StartCoroutine(BSM.ShowElementalDamage(effectDamage, this.gameObject, effectToProcess.elementColor));
            }

            activeStatusEffects[i].turnsRemaining--; //lowers turns remaining by 1

            Debug.Log(hero.name + " - turns remaining on " + activeStatusEffects[i].effectName + ": " + activeStatusEffects[i].turnsRemaining);
            if (activeStatusEffects[i].turnsRemaining == 0) //removes status effect if no more turns remaining
            {
                Debug.Log(activeStatusEffects[i].effectName + " removed from " + hero.name);
                activeStatusEffects.RemoveAt(i);
            }
        }
        UpdateHeroStats();
    }

    /// <summary>
    /// Returns random integer between provided values
    /// </summary>
    /// <param name="min">Minimum value for random value</param>
    /// <param name="max">Maximum value for random value</param>
    int GetRandomInt(int min, int max)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int rand = Random.Range(min, max);
        return rand;
    }

    /// <summary>
    /// Used in update method to increase the hero's ATB progress bar
    /// </summary>
    void UpgradeProgressBar()
    {
        cur_cooldown = (cur_cooldown + (Time.deltaTime / 1f)) + (hero.finalDexterity * .000055955f); //increases ATB gauge, first float dictates how slowly gauge fills (default 1f), while second float dictates how effective dexterity is
        float calc_cooldown = cur_cooldown / max_cooldown; //does math of % of ATB gauge to be filled each frame
        ProgressBar.transform.localScale = new Vector2(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y); //shows graphic of ATB gauge increasing
        if (cur_cooldown >= max_cooldown) //if hero turn is ready
        {
            BSM.pendingTurn = true;
            calculatedTilesToMove = false;
            currentState = TurnState.ADDTOLIST;
        }
    }
}
