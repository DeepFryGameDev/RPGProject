using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraManager : MonoBehaviour
{
    public Camera cam;
    Vector3 targetPos;
    
    public camStates camState;

    Vector3 homePos = new Vector3(-4.1f, 0.49f, -10.0f);
    float homeSize = 4.305343f;

    float targetZoomSize = 2.0f;

    float baseMoveTime = 7.5f;
    float baseZoomTime = 7.5f;

    float battleStartZoomTime = 2.0f;
    float battleStartZoomSize = 1.5f;
    int battleStartTotalHoverFrames = 60;
    int battleStartCurrentHoverFrames = 0;
    int battleStartZoomWaitTotalFrames = 40;
    int battleStartZoomWaitCurrentFrames = 0;
    List<GameObject> enemiesAccountedFor = new List<GameObject>();
    GameObject battleStartEnemyToZoom;
    bool enemyZoomReady;

    float baseZ = -10f;

    float canvasAdj = -1.0f;

    public bool startBattleZoom;
    bool battleReady;

    float baseChooseTargetZoomSize = 1.5f;
    
    GameObject[] heroesInBattle;
    GameObject[] enemiesInBattle;

    public bool magicCastingAnimFinished;
    public GameObject currentMagicTarget;

    public GameObject physAttackObj;
    public bool physAttackAnimFinished;
    public bool physAttackCameraZoomFinished;

    public GameObject itemUsedUnit;
    public bool itemAnimFinished;

    float lossFadeTime = 0.25f;

    public GameObject currentUnit;

    //Set externally
    public bool showingDamage;
    public GameObject parentTile;
    BattleStateMachine BSM;

    #region Singleton
    public static BattleCameraManager instance; //call instance 

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of BattleCameraManager found!");
            return;
        }

        instance = this;

        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        homeSize = cam.orthographicSize;
        homePos = cam.transform.position;

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startBattleZoom = false;
        battleReady = false;
        enemyZoomReady = false;
        magicCastingAnimFinished = false;
        currentMagicTarget = null;

        heroesInBattle = GameObject.FindGameObjectsWithTag("Hero");
        enemiesInBattle = GameObject.FindGameObjectsWithTag("Enemy");
    }
    #endregion
    
    void Update()
    {
        //Debug.Log(camState);

        switch(camState)
        {
            //When transitioning into battle and loading the scene (during fade in)
            case (camStates.BATTLESTART):

                //-----
                //if debugging, use below to skip pre-battle zoom, otherwise comment out

                startBattleZoom = false;
                battleReady = true;

                //------
                if (startBattleZoom)
                {
                    //Zoom in
                    if (cam.orthographicSize != homeSize && !enemyZoomReady)
                    {
                        //Debug.Log("Zooming camera: " + cam.orthographicSize);
                        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, homeSize, battleStartZoomTime * Time.deltaTime);
                    }

                    //Pause for a second to show battlefield
                    if (cam.orthographicSize == homeSize && !enemyZoomReady)
                    {
                        battleStartZoomWaitCurrentFrames++;

                        if (battleStartZoomWaitCurrentFrames == battleStartZoomWaitTotalFrames)
                        {
                            enemyZoomReady = true;
                        }
                    }

                    if (enemyZoomReady)
                    {
                        //zoom into each enemy unit
                        foreach (GameObject enemy in enemiesInBattle)
                        {
                            if (!enemiesAccountedFor.Contains(enemy))
                            {
                                if (battleStartEnemyToZoom == null)
                                {
                                    battleStartEnemyToZoom = enemy;
                                }

                                if (cam.orthographicSize != targetZoomSize)
                                {
                                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, battleStartZoomSize, baseZoomTime * Time.deltaTime);
                                }

                                if (cam.transform.position != new Vector3(battleStartEnemyToZoom.transform.position.x, battleStartEnemyToZoom.transform.position.y, baseZ))
                                {
                                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(battleStartEnemyToZoom.transform.position.x, battleStartEnemyToZoom.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                                }

                                if (cam.transform.position == new Vector3(battleStartEnemyToZoom.transform.position.x, battleStartEnemyToZoom.transform.position.y, baseZ) && battleStartCurrentHoverFrames < battleStartTotalHoverFrames)
                                {
                                    battleStartCurrentHoverFrames++;
                                }

                                if (battleStartCurrentHoverFrames == battleStartTotalHoverFrames)
                                {
                                    battleStartCurrentHoverFrames = 0;
                                    enemiesAccountedFor.Add(enemy);
                                    battleStartEnemyToZoom = null;
                                }
                            }
                        }
                    }

                    if (enemiesAccountedFor.Count == BSM.EnemiesInBattle.Count)
                    {
                        battleReady = true;
                    }

                }

                if (battleReady)
                {
                    GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().alpha = 1;
                    GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().interactable = true;
                    GameObject.Find("BattleCanvas/BattleUI").GetComponent<CanvasGroup>().blocksRaycasts = true;
                    
                    foreach (GameObject hero in heroesInBattle)
                    {
                        HeroStateMachine HSM = hero.GetComponent<HeroStateMachine>();
                        HSM.currentState = HeroStateMachine.TurnState.PROCESSING;
                    }

                    foreach (GameObject enemy in enemiesInBattle)
                    {
                        EnemyStateMachine ESM = enemy.GetComponent<EnemyStateMachine>();
                        ESM.currentState = TurnState.PROCESSING;
                    }

                    ResetVars();
                    camState = camStates.IDLE;
                }


            break;

            //Show the battlefield
            case (camStates.IDLE):
                
                if (cam.transform.position != homePos)
                {
                    //Debug.Log("Moving camera: " + cam.transform.position);
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, homePos, baseMoveTime * Time.deltaTime);
                }

                if (cam.orthographicSize != homeSize)
                {
                    //Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, homeSize, baseZoomTime * Time.deltaTime);
                }

            break;

            //When hero's turn begins / during movement phase
            case (camStates.HEROTURN):

                //Hover camera over unit, zoom out if needed
                if (cam.orthographicSize != homeSize)
                {
                    //Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, homeSize, baseZoomTime * Time.deltaTime);
                }

                //Follow unit through movement phase
                if (cam.transform.position != new Vector3(currentUnit.transform.position.x, (currentUnit.transform.position.y + canvasAdj), baseZ))
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(currentUnit.transform.position.x, (currentUnit.transform.position.y + canvasAdj), baseZ), baseMoveTime * Time.deltaTime);
                }

                break;

            //When enemy's turn begins and they are moving
            case (camStates.ENEMYTURN):

                //Zoom into enemy
                if (cam.orthographicSize != targetZoomSize)
                {
                    //Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                }

                //Follow unit through movement phase
                if (cam.transform.position != new Vector3(BSM.enemyToManage.transform.position.x, (BSM.enemyToManage.transform.position.y), baseZ))
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(BSM.enemyToManage.transform.position.x, (BSM.enemyToManage.transform.position.y), baseZ), baseMoveTime * Time.deltaTime);
                }

                break;

            //When hero can choose an action
            case (camStates.CHOOSEACTION):

                //Zoom into hero
                if (cam.orthographicSize != targetZoomSize)
                {
                    //Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                }

                //Center camera to unit
                if (cam.transform.position != new Vector3(currentUnit.transform.position.x, currentUnit.transform.position.y, baseZ))
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(currentUnit.transform.position.x, currentUnit.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                }

                break;

            //When hero can choose a unit for action
            case (camStates.CHOOSETARGET):
                
                //Zoom out to show range of tiles for attack
                if (cam.orthographicSize != (baseChooseTargetZoomSize + (BSM.tileRange + 1)))
                {
                    //Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, (baseChooseTargetZoomSize + (BSM.tileRange + 1)), baseZoomTime * Time.deltaTime);
                }

                //Center camera to unit position
                if (cam.transform.position != new Vector3(currentUnit.transform.position.x, (currentUnit.transform.position.y + canvasAdj), baseZ))
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(currentUnit.transform.position.x, (currentUnit.transform.position.y + canvasAdj), baseZ), baseMoveTime * Time.deltaTime);
                }

                break;

            //When unit has chosen target and animation is triggered
            case (camStates.ATTACK):

                //If magic
                if (BSM.PerformList[0].actionType == ActionType.MAGIC)
                {
                    if (!magicCastingAnimFinished)
                    {
                        //Zoom into attacker
                        if (cam.orthographicSize != targetZoomSize)
                        {
                            //Debug.Log("Zooming camera: " + cam.orthographicSize);
                            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                        }

                        //Center camera to unit
                        if (cam.transform.position != new Vector3(currentUnit.transform.position.x, currentUnit.transform.position.y, baseZ))
                        {
                            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(currentUnit.transform.position.x, currentUnit.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                        }
                    }
                    else
                    {
                        if (currentMagicTarget != null && !showingDamage)
                        {
                            //Center camera to unit
                            if (cam.transform.position != new Vector3(currentMagicTarget.transform.position.x, currentMagicTarget.transform.position.y, baseZ))
                            {
                                cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(currentMagicTarget.transform.position.x, currentMagicTarget.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                            }
                        }
                    }

                    if (showingDamage)
                    {
                        //Zoom out to show damage
                        if (cam.orthographicSize != (targetZoomSize + BSM.tileRange + 1))
                        {
                            //Debug.Log("Zooming camera: " + cam.orthographicSize);
                            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, (targetZoomSize + BSM.tileRange + 1), baseZoomTime * Time.deltaTime);
                        }

                        //Move to parent tile
                        if (cam.transform.position != new Vector3(parentTile.transform.position.x, parentTile.transform.position.y, baseZ))
                        {
                            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(parentTile.transform.position.x, parentTile.transform.position.y, baseZ), 5.0f * Time.deltaTime);
                        }
                    }
                }

                //If physical
                if (BSM.PerformList[0].actionType == ActionType.ATTACK)
                {
                    if (!physAttackAnimFinished)
                    {
                        //Zoom into attacker
                        if (cam.orthographicSize != targetZoomSize)
                        {
                            //Debug.Log("Zooming camera: " + cam.orthographicSize);
                            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                        }

                        //Center camera to unit
                        /*if (cam.transform.position != new Vector3(currentUnit.transform.position.x, currentUnit.transform.position.y, baseZ))
                        {
                            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(currentUnit.transform.position.x, currentUnit.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                        } else
                        {
                            physAttackCameraZoomFinished = true;
                        }*/

                        if (cam.transform.position != new Vector3(physAttackObj.transform.position.x, physAttackObj.transform.position.y, baseZ))
                        {
                            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(physAttackObj.transform.position.x, physAttackObj.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                        }
                        else
                        {
                            physAttackCameraZoomFinished = true;
                        }

                    }

                    if (showingDamage)
                    {
                        //Zoom in to target
                        if (cam.orthographicSize != targetZoomSize)
                        {
                            //Debug.Log("Zooming camera: " + cam.orthographicSize);
                            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                        }

                        //Center camera to unit
                        if (physAttackObj != null && cam.transform.position != new Vector3(physAttackObj.transform.position.x, physAttackObj.transform.position.y, baseZ))
                        {
                            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(physAttackObj.transform.position.x, physAttackObj.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                        }
                    }
                }

                //If item
                if (BSM.PerformList[0].actionType == ActionType.ITEM)
                {
                    if (!itemAnimFinished)
                    {
                        //Zoom into attacker
                        if (cam.orthographicSize != targetZoomSize)
                        {
                            //Debug.Log("Zooming camera: " + cam.orthographicSize);
                            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                        }

                        //Center camera to unit
                        if (cam.transform.position != new Vector3(currentUnit.transform.position.x, currentUnit.transform.position.y, baseZ))
                        {
                            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(currentUnit.transform.position.x, currentUnit.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                        }
                    }

                    if (showingDamage)
                    {
                        //Zoom in to target
                        if (cam.orthographicSize != targetZoomSize)
                        {
                            //Debug.Log("Zooming camera: " + cam.orthographicSize);
                            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                        }

                        //Center camera to unit
                        if (itemUsedUnit != null && cam.transform.position != new Vector3(itemUsedUnit.transform.position.x, itemUsedUnit.transform.position.y, baseZ))
                        {
                            cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(itemUsedUnit.transform.position.x, itemUsedUnit.transform.position.y, baseZ), baseMoveTime * Time.deltaTime);
                        }
                    }
                }

            break;

            //When heroes have won the battle
            case (camStates.VICTORY):

                //Zoom into last hero to process
                if (cam.orthographicSize != targetZoomSize)
                {
                    Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                }

                //Move camera to last hero to process
                if (cam.transform.position != new Vector3(BSM.lastHeroToProcess.transform.position.x, (BSM.lastHeroToProcess.transform.position.y), baseZ))
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(BSM.lastHeroToProcess.transform.position.x, (BSM.lastHeroToProcess.transform.position.y), baseZ), baseMoveTime * Time.deltaTime);
                }

                break;

            //When heroes have lost the battle
            case (camStates.LOSS):
                //Fade out to black
                if (!GameObject.Find("BattleCanvas/LossCanvas").GetComponent<CanvasGroup>().blocksRaycasts)
                {
                    GameObject.Find("BattleCanvas/LossCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;
                }

                if (GameObject.Find("BattleCanvas/LossCanvas").GetComponent<CanvasGroup>().alpha != 1.0f)
                {
                    GameObject.Find("BattleCanvas/LossCanvas").GetComponent<CanvasGroup>().alpha = Mathf.MoveTowards(GameObject.Find("BattleCanvas/LossCanvas").GetComponent<CanvasGroup>().alpha, 1.0f, lossFadeTime * Time.deltaTime);
                }

            break;
        }
    }

    public void ResetVars()
    {
        startBattleZoom = false;
        battleReady = false;

        magicCastingAnimFinished = false;
        currentMagicTarget = null;

        physAttackObj = null;
        physAttackAnimFinished = false;
        physAttackCameraZoomFinished = false;

        itemUsedUnit = null;
        itemAnimFinished = false;

        currentUnit = null;
}

}
