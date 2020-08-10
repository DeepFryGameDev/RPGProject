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

    public bool waitForDamageToFinish;

    //for ProgressBar
    public float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    private Image ProgressBar;
    public GameObject Selector;

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
        BSM.HideSelector(Selector); //hide hero selector cursor
        startPosition = transform.position; //sets start position to hero's position for animation purposes
        playerMove = GetComponent<PlayerMove>();
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
                    
                    BSM.HideSelector(Selector); //hide the selector cursor for the hero
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
                if (!targets.Contains(target.collider.gameObject) && target.collider.gameObject.tag != "Tile")
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
            rangeTile.inAffect = true;
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
            rangeTile.inRange = true;
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
