using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour //for processing enemy turns
{
    //Enemy state machine script is attached to each enemy to be used in battle state machine

    private BattleStateMachine BSM; //global battle state machine

    public EnemyBehavior enemyBehavior;

    [HideInInspector] public BaseEnemy enemy;

    public bool waitForDamageToFinish;

    public TurnState currentState;

    //for ProgressBar
    public float cur_cooldown = 0f; //starting point for enemy ATB gauge (not fully needed as it is set to random value in Start())
    private float max_cooldown = 10f; //how long it takes for enemy ATB gauge to fill

    //this GameObject
    [HideInInspector] public Vector2 startPosition; //to store enemy's starting position for movement
    //TimeforAction() stuff
    [HideInInspector] public bool actionStarted = false; //used for knowing whether to execute or exit the ieNumerator

    //is enemy alive?
    private bool alive = true;

    //for movement
    public bool inMove;
    bool runMoveOnce;

    //for death animation
    bool deathAnimationStarted;
    bool deathAnimationFinished;
    float deathFadeTime = 1.0f;

    void Start()
    {
        //currentState = TurnState.PROCESSING;
        currentState = TurnState.WAITING;

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //sets battle state machine to active battle state machine in BattleManager (in scene)
        startPosition = transform.position; //sets startPosition to the enemy's position at the start of battle
        cur_cooldown = Random.Range(0, 2.5f); //Sets random point for enemy ATB gauge to start
        
        enemyBehavior = GetComponent<EnemyBehavior>();

        enemy.curHP = enemy.baseHP;
        enemy.curMP = enemy.baseMP;
    }

    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                if (!waitForDamageToFinish && BattleCameraManager.instance.camState != camStates.LOSS)
                {
                    if (BSM.activeATB)
                    {
                        UpgradeProgressBar(); //fills enemy ATB gauge
                    }
                    else
                    {
                        if (BSM.pendingTurn == false)
                        {
                            UpgradeProgressBar(); //fills enemy ATB gauge
                        }
                    }
                }
            break;

            case (TurnState.CHOOSEACTION):
                BSM.enemyToManage = gameObject;

                BattleCameraManager.instance.currentUnit = gameObject;

                if (runMoveOnce)
                {
                    BattleCameraManager.instance.camState = camStates.ENEMYTURN;

                    enemyBehavior.BeginEnemyTurn();
                    runMoveOnce = false;
                }

                while (inMove)
                {
                    return;
                }

                currentState = TurnState.WAITING;
            break;

            case (TurnState.WAITING):
                //idle state
            break;

            case (TurnState.DEAD): //run after every time enemy takes damage that brings them to or below 0 hp
                if (!alive) //if alive value is set to false, exits the turn state. this is set to false in below code
                {
                    return;
                } else
                {           
                    if (gameObject.GetComponent<Animator>().GetBool("onDeath") == false)
                    {
                        gameObject.GetComponent<Animator>().SetBool("onDeath", true);

                        BSM.expPool += enemy.earnedEXP; //increases enemy's exp to exp pool to take after battle
                        this.gameObject.tag = "DeadEnemy"; //change tag of enemy to DeadEnemy
                        BSM.EnemiesInBattle.Remove(this.gameObject); //Makes this enemy not attackable by heroes

                        if (BSM.EnemiesInBattle.Count > 0) //remove all enemyAttacks inputs from active perform list if there are still enemies on the field
                        {
                            for (int i = 0; i < BSM.PerformList.Count; i++) //go through all actions in perform list
                            {
                                //if (i != 0) //can remove later if enemies can kill themselves. otherwise only checks for items in the perform list after 0 (as 0 would be the hero's action)
                                //{
                                if (BSM.PerformList[i].AttackersGameObject == this.gameObject) //if the attacker in the loop is this enemy
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]); //removes this action from the perform list
                                }
                                if (BSM.PerformList[i].AttackersTarget == this.gameObject) //if target in loop in the perform list is the dead enemy
                                {
                                    BSM.PerformList[i].AttackersTarget = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)]; //changes the target from the dead enemy to a random enemy so dead enemy cannot be attacked
                                }
                                //}
                            }
                        }
                    }

                    if (!deathAnimationStarted)
                    {
                        deathAnimationStarted = true;
                        ProcessDeathFade();
                    }

                    if (deathAnimationFinished)
                    {
                        alive = false; //set alive to false to exit out of the turn state

                        Debug.Log("setting BSM Battlestate to checkalive");
                        BSM.battleState = battleStates.CHECKALIVE; //changes battle state to check alive

                        Destroy(gameObject);
                    }
                }
            break;
        }
    }

    /// <summary>
    /// Used in update method to increase the enemy's ATB progress bar
    /// </summary>
    void UpgradeProgressBar()
    {
        cur_cooldown = (cur_cooldown + (Time.deltaTime / 1)) + (enemy.baseDexterity * .000055955f); //increases enemy ATB gauge over time

        Debug.Log(enemy.name + " Cooldown: " + cur_cooldown + "/" + max_cooldown);
        if (cur_cooldown >= max_cooldown) //if enemy ATB gauge meets threshold for choosing an action
        {
            BSM.pendingTurn = true;
            inMove = true;
            runMoveOnce = true;
            currentState = TurnState.CHOOSEACTION; //choose the action
        }
    }

    void ProcessDeathFade()
    {
        StartCoroutine(FadeOutObject());
    }

    IEnumerator FadeOutObject()
    {
        float counter = 0;
        Color spriteColor = gameObject.GetComponent<Renderer>().material.color;

        AudioManager.instance.PlaySE(AudioManager.instance.enemyDeath);

        while (counter < deathFadeTime)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / deathFadeTime);

            gameObject.GetComponent<Renderer>().material.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);

            yield return null;
        }

        deathAnimationFinished = true;
    }

}
