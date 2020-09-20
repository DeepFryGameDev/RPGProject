using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : EnemyMove
{
    //All enemies have a behavior attached which is derived from this class
    protected List<BaseEnemyAttack> enemySkills = new List<BaseEnemyAttack>();
    protected List<BaseEnemy> enemyParty = new List<BaseEnemy>();
    protected List<BaseHero> heroParty = new List<BaseHero>();

    [HideInInspector] public EnemyStateMachine ESM;
    protected BaseEnemy self;

    public GameObject HeroToAttack; //the hero to be attacked by enemy

    private float animSpeed = 10f; //speed at which enemy moves to target for attack animation

    //for MP calculations
    [HideInInspector] public List<BaseAttack> attacksWithinMPThreshold = new List<BaseAttack>();

    //for finding which attacks are in range
    protected List<BaseAttack> attacksWithinRange = new List<BaseAttack>();

    protected Tile bestTargetTile;
    protected Tile bestMoveTile;

    Tile bestPositionTile;
    int tempPosition;
    
    protected bool gettingTarget;
    protected bool thinking;
    protected bool doneThinking;
    //bool gettingBestTargetTile;

    bool showingThinkIcon;

    protected enum targetTypes
    {
        Hero,
        Enemy
    }

    protected targetTypes targetType;

    //for status effects
    public List<BaseEffect> activeStatusEffects = new List<BaseEffect>();
    public int effectDamage;
    public int magicDamage;
    public int itemDamage;

    //for threat
    public List<BaseThreat> threatList = new List<BaseThreat>();
    [HideInInspector] public int maxThreat;
    public Image threatBar;
    public Color maxThreatColor = Color.red;
    public Color highThreatColor = Color.yellow;
    public Color moderateThreatcolor = Color.green;
    public Color lowThreatcolor = Color.blue;
    public Color veryLowThreatColor = Color.grey;

    protected BehaviorStates behaviorStates;

    protected bool foundTarget;
    protected bool foundPath;

    Animator enemyAnim;

    /// <summary>
    /// Sets initial values for attached enemy's state machine, as well as threat and threatlist
    /// </summary>
    public void InitBehavior()
    {
        ESM = GetComponent<EnemyStateMachine>();
        self = ESM.enemy;
        enemySkills = self.attacks;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyParty.Add(self);
        }

        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        for (int i = 0; i < heroes.Length; i++)
        {
            heroParty.Add(heroes[i].GetComponent<HeroStateMachine>().hero);

            BaseThreat threat = new BaseThreat();
            threat.threat = 0;
            threat.hero = (heroes[i].GetComponent<HeroStateMachine>().hero);
            threat.heroObject = heroes[i];
            threatList.Add(threat);
        }
    }

    /// <summary>
    /// Generates available actions based on learned attacks within MP availability and in range
    /// </summary>
    protected void BuildActionLists()
    {
        ClearActionLists();

        //check which attacks are available based on MP cost of all attacks, and enemy's current MP, and adds them to 'attacksWithinMPThreshold' list.
        foreach (BaseEnemyAttack atk in self.attacks)
        {
            if (atk.attack.MPCost <= self.curMP)
            {
                attacksWithinMPThreshold.Add(atk.attack);
            }
        }

        /*foreach (GameObject hero in BSM.HeroesInBattle)
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
        }*/

        foreach (BaseAttack attackInRange in attacksWithinMPThreshold)
        {
            /*if (lowestRangeForAttack <= attackInRange.rangeIndex)
            {
                Debug.Log("Adding " + attackInRange.name + " to attacksInRange - Range " + attackInRange.rangeIndex);
                attacksWithinRange.Add(attackInRange);
            }*/
            Pattern pattern = new Pattern();

            RaycastHit2D[] tilesHit = Physics2D.RaycastAll(transform.position, Vector3.back, 1);
            foreach (RaycastHit2D tile in tilesHit)
            {
                if (tile.collider.gameObject.tag == "Tile")
                {
                    Tile parentTile = tile.collider.gameObject.GetComponent<Tile>();
                    pattern.GetRangePattern(parentTile, attackInRange.rangeIndex);
                    break;
                }
            }

            List<Tile> tilesInPattern = pattern.pattern;

            foreach (Tile tile in tilesInPattern)
            {
                RaycastHit2D[] targetsHit = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);
                foreach (RaycastHit2D target in targetsHit)
                {
                    if (target.collider.gameObject.tag == "Hero" && !attacksWithinRange.Contains(attackInRange)) //add enemy later
                    {
                        //Debug.Log("adding " + target.collider.gameObject + " to targets");
                        attacksWithinRange.Add(attackInRange);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Resets attacks within MP threshold and within range, as well as any possible targets
    /// </summary>
    void ClearActionLists()
    {
        attacksWithinMPThreshold.Clear(); //clears list for next enemy to use
        attacksWithinRange.Clear();
        targets.Clear();
    }

    /// <summary>
    /// Facilitates animation for chosen magic or physical attack
    /// </summary>
    protected void ProcessAnimation()
    {
        readyToAnimateAction = false;

        //need to add animation for magic casting, as well as ranged physical damage
        
        if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.PHYSICAL)
        {
            StartCoroutine(AttackAnimation()); //the actual action being performed (including animation and damage calculation)
        }

        if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.MAGIC)
        {
            StartCoroutine(MagicAnimation()); //the actual action being performed (including animation and damage calculation)
            //Need to implement magic animation and damage calculations here - use BaseMagicScript
        }
    }

    /// <summary>
    /// Coroutine.  Processes attack animation and handles damage process
    /// </summary>
    public IEnumerator AttackAnimation()
    {
        if (ESM.actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have already gone through it
        }

        ESM.actionStarted = true;

        BSM.PerformList[0].actionType = ActionType.ATTACK;

        BattleCameraManager.instance.physAttackObj = targets[0];
        BattleCameraManager.instance.camState = camStates.ATTACK;

        while (!BattleCameraManager.instance.physAttackCameraZoomFinished)
        {
            yield return null;
        }

        ShowRangePattern(chosenAttack, GetCurrentTile());
        ShowAffectPattern(chosenAttack, bestTargetTile);

        //this is where actual attack animation would go
        //Vector2 targetPosition = new Vector2(actionTarget.transform.position.x - .5f, actionTarget.transform.position.y); //gets hero's position (minus a few pixels on the x axis) to move to for attack animation
        //while (MoveToTarget(targetPosition)) { yield return null; } //move towards the target
        
        enemyAnim = gameObject.GetComponent<Animator>();

        foreach (GameObject target in targets)
        {
            SetEnemyFacingDir(target, "atkDirX", "atkDirY");

            yield return new WaitForSeconds(.2f); //wait a bit

            enemyAnim.SetBool("onPhysAtk", true);

            yield return new WaitForSeconds(0.5f); //wait a bit

            AttackAnimation animation = GameObject.Find("BattleManager/AttackAnimationManager").GetComponent<AttackAnimation>();
            animation.attack = BSM.PerformList[0].chosenAttack;
            animation.target = target;

            foreach (BaseEnemyAttack BEA in self.attacks)
            {
                if (BEA.attack == BSM.PerformList[0].chosenAttack)
                {
                    animation.enemyAEChance = BEA.addedEffectChance;
                    break;
                }
            }

            animation.BuildAnimation();

            StartCoroutine(animation.PlayAnimation());

            yield return new WaitForSeconds(animation.attackDur); //wait a bit

            /*Debug.Log("addedEffect: " + animation.addedEffectAchieved);

            BaseAddedEffect BAE = new BaseAddedEffect();
            BAE.target = target;
            BAE.addedEffectProcced = animation.addedEffectAchieved;
            checkAddedEffects.Add(BAE);

            animation.addedEffectAchieved = false;*/ //will update later when calculating if enemy succeeds added effect
        }

        enemyAnim.SetBool("onPhysAtk", false);

        foreach (GameObject target in targets)
        {
            Animator tarAnim = target.GetComponent<Animator>();
            SetTargetFacingDir(target, "rcvDamX", "rcvDamY");
            tarAnim.SetBool("onRcvDam", true);

            ProcessAttack(target);
        }

        yield return new WaitForSeconds(BSM.damageDisplayTime);

        //animate the enemy back to start position
        //Vector2 firstPosition = ESM.startPosition; //changes the first position of the animation back to the starting position of the enemy
        //while (MoveToTarget(firstPosition)) { yield return null; } //moves back towards the starting position

        FinishAction();
    }

    /// <summary>
    /// Coroutine.  Processes magic animation and handles damage process
    /// </summary>
    public IEnumerator MagicAnimation()
    {
        if (ESM.actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have already gone through it
        }

        ESM.actionStarted = true;

        BSM.PerformList[0].actionType = ActionType.MAGIC;

        BattleCameraManager.instance.camState = camStates.ATTACK;

        ShowRangePattern(chosenAttack, GetCurrentTile());
        ShowAffectPattern(chosenAttack, bestTargetTile);
        
        Debug.Log("Casting: " + BSM.PerformList[0].chosenAttack.name);

        enemyAnim = gameObject.GetComponent<Animator>();

        SetEnemyFacingDir(targets[0], "atkDirX", "atkDirY");

        yield return new WaitForSeconds(.2f); //wait a bit

        StartCoroutine(BSM.ShowAttackName(BSM.PerformList[0].chosenAttack.name, AudioManager.instance.magicCast.length));

        enemyAnim.SetBool("onMagAtk", true);

        AttackAnimation animation = GameObject.Find("BattleManager/AttackAnimationManager").GetComponent<AttackAnimation>();
        animation.attack = BSM.PerformList[0].chosenAttack;

        animation.PlayCastingAnimation(gameObject);

        yield return new WaitForSeconds(AudioManager.instance.magicCast.length);

        BattleCameraManager.instance.magicCastingAnimFinished = true;

        //show spell animation

        foreach (BaseEnemyAttack BEA in self.attacks)
        {
            if (BEA.attack == BSM.PerformList[0].chosenAttack)
            {
                animation.enemyAEChance = BEA.addedEffectChance;
                break;
            }
        }

        foreach (GameObject target in targets)
        {
            BattleCameraManager.instance.currentMagicTarget = target;

            animation.target = target;

            animation.BuildAnimation();

            StartCoroutine(animation.PlayAnimation());

            yield return new WaitForSeconds(animation.attackDur); //wait a bit
        }

        //process spell

        BaseMagicScript magicScript = new BaseMagicScript();
        magicScript.spell = chosenAttack;
        magicScript.enemyPerformingAction = self;
        magicScript.eb = this;

        foreach (GameObject tar in targets)
        {
            if (tar.tag == "Hero")
            {
                magicScript.heroReceivingAction = tar.GetComponent<HeroStateMachine>().hero;
                magicScript.ProcessMagicEnemyToHero();

            }
            else if (tar.tag == "Enemy")
            {
                magicScript.enemyReceivingAction = tar.GetComponent<EnemyStateMachine>().enemy;
                magicScript.ProcessMagicEnemyToEnemy();
            }
        }

        Destroy(GameObject.Find("Casting animation"));

        if (!targets.Contains(gameObject))
        {
            enemyAnim.SetBool("onMagAtk", false);
        }

        foreach (GameObject target in targets)
        {
            Animator tarAnim = target.GetComponent<Animator>();
            SetTargetFacingDir(target, "rcvDamX", "rcvDamY");

            if (BSM.PerformList[0].chosenAttack.magicClass != BaseAttack.MagicClass.WHITE)
                tarAnim.SetBool("onRcvDam", true);
        }

        foreach (GameObject target in targets)
        {
            if (chosenAttack.magicClass == BaseAttack.MagicClass.WHITE)
            {
                StartCoroutine(BSM.ShowHeal(magicDamage, target));
            }
            else
            {
                StartCoroutine(BSM.ShowDamage(magicDamage, target));
            }
        }

        gameObject.GetComponent<EnemyStateMachine>().enemy.curMP -= BSM.PerformList[0].chosenAttack.MPCost;

        yield return new WaitForSeconds(BSM.damageDisplayTime);

        enemyAnim.SetBool("onMagAtk", false);

        FinishAction();
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

    void SetEnemyFacingDir(GameObject target, string paramNameX, string paramNameY)
    {
        enemyAnim.SetFloat(paramNameX, 0);
        enemyAnim.SetFloat(paramNameY, 0);

        float xDiff = gameObject.transform.position.x - target.transform.position.x;
        float yDiff = gameObject.transform.position.y - target.transform.position.y;

        if (xDiff < 0)
        {
            enemyAnim.SetFloat(paramNameX, 1f);
            enemyAnim.SetFloat("moveX", 1f);
        }
        else if (xDiff > 0)
        {
            enemyAnim.SetFloat(paramNameX, -1f);
            enemyAnim.SetFloat("moveX", -1f);
        }

        if (yDiff < 0)
        {
            enemyAnim.SetFloat(paramNameY, 1f);
            enemyAnim.SetFloat("moveY", 1f);
        }
        else if (yDiff > 0)
        {
            enemyAnim.SetFloat(paramNameY, -1f);
            enemyAnim.SetFloat("moveY", -1f);
        }
    }

    /// <summary>
    /// Facilitates processing of chosen action
    /// </summary>
    /// <param name="action">Sets chosen attack to the given BaseAttack</param>
    /// <param name="targets">Given list of GameObjects that are the targets for the chosen action</param>
    protected void RunAction(BaseAttack action, List<GameObject> targets)
    {
        HandleTurn myAttack = new HandleTurn(); //new handleturn for the enemy's attack
        myAttack.Attacker = self.name; //enemy's name set for the handleturn's attacker
        myAttack.attackerType = Types.ENEMY; //sets handleturn's type to enemy
        myAttack.AttackersGameObject = this.gameObject; //sets the handleturn's attacker game object to this enemy
        
        myAttack.chosenAttack = action;

        BSM.CollectActions(myAttack); //adds chosen attack to the perform list
        
        ProcessAnimation();
    }

    /// <summary>
    /// Facilitates movement to target position for attack animation
    /// </summary>
    /// <param name="target">Target position to move to for animation</param>
    private bool MoveToTarget(Vector3 target) //using Vector2 causes an error, but Vector3 translates without issues
    {
        return target != (transform.position = Vector2.MoveTowards(transform.position, target, animSpeed * Time.deltaTime)); //moves towards the target parameter until position is same as the target position
    }

    /// <summary>
    /// Processes dealing damage to chosen action's targets
    /// </summary>
    void ProcessAttack(GameObject target)
    {
        int calc_damage = self.baseATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by enemy's current attack + the attack's damage
        
        if (target.tag == "Hero")
        {
            calc_damage = self.baseATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
            target.GetComponent<HeroStateMachine>().TakeDamage(calc_damage); //processes enemy take damage by above value
            Debug.Log(self.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "!");
            Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - enemy's ATK: " + self.baseATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);
        }

        if (target.tag == "Enemy")
        {
            calc_damage = self.baseATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
            target.GetComponent<EnemyBehavior>().TakeDamage(calc_damage); //processes enemy take damage by above value
            Debug.Log(self.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<EnemyStateMachine>().enemy.name + "!");
            Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - enemy's ATK: " + self.baseATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);

            //HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage); //processes the damage to the hero from the enemy
            //Debug.Log(this.gameObject.name + " has chosen " + BSM.PerformList[0].chosenAttack.attackName + " and does " + calc_damage + " damage to " + HeroToAttack.GetComponent<HeroStateMachine>().hero.name + "!");

            //enemy.curMP -= BSM.PerformList[0].chosenAttack.attackCost; //remove MP from enemy
        }

        StartCoroutine(BSM.ShowDamage(calc_damage, target));
    }

    /// <summary>
    /// Returns list of GameObjects to be set as targets for the chosen action
    /// </summary>
    /// <param name="affectIndex">Affect index of the chosen attack/action</param>
    /// <param name="tag">to determine if target for chosen action is a hero or enemy</param>
    protected IEnumerator GetTargets(int affectIndex, string tag)
    {
        gettingTarget = true;

        targets.Clear();

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        /*Tile targetTile = null;
        foreach (GameObject obj in tiles)
        {
            Tile tile = obj.GetComponent<Tile>();
            if (tile.inRange && tile.inAffect)
            {
                targetTile = tile;
                break;
            }
        }*/

        //RaycastHit2D[] tileHits;

        //if (targetTile == null)
        //{
        //    Debug.Log("using targetChoice");
        //tileHits = Physics2D.RaycastAll(targetChoice.transform.position, Vector3.back, 1);
        //} else
        //{
        //    Debug.Log("using targetTile");
        //    tileHits = Physics2D.RaycastAll(targetTile.transform.position, Vector3.zero, 1);
        //}


        /*foreach (RaycastHit2D tile in tileHits)
        {
            if (tile.collider.gameObject.tag == "Tile")
            {
                Tile t = tile.collider.gameObject.GetComponent<Tile>();
                pattern.GetAffectPattern(t, affectIndex);
                tilesInRange = pattern.pattern;
                break;
            }
        }*/

        //gettingBestTargetTile = true;
        //GetBestTargetTile();

        Tile targetTile = bestTargetTile;
        BattleCameraManager.instance.parentTile = targetTile.gameObject;

        BSM.centerTile = targetTile.gameObject;

        pattern.GetAffectPattern(targetTile, affectIndex);
        tilesInRange = pattern.pattern;

        //need to wait until tiles are shielded before continuing..
        while (bestTargetTile == null)
        {
            Debug.Log("waiting...");
            yield return new WaitForSeconds(0.1f);
        }


        foreach (Tile t in tilesInRange)
        {
            //Debug.Log(t.gameObject.name);
            RaycastHit2D[] tilesHit = Physics2D.RaycastAll(t.transform.position, Vector3.forward, 1);

            foreach (RaycastHit2D target in tilesHit)
            {
                if (!targets.Contains(target.collider.gameObject) && (target.collider.gameObject.tag == "Hero" || target.collider.gameObject.tag == "Enemy"))
                {
                    Debug.Log(t.gameObject.name + " - is shielded: " + t.shielded);
                    Debug.Log("adding " + target.collider.gameObject + " to targets");
                    targets.Add(target.collider.gameObject); //adds all objects inside target range to targets list to be affected
                }
            }
        }

        gettingTarget = false;
    }

    /// <summary>
    /// Calculates the best target tile for processing the action using all possible target tiles, and returns the tile with the most targets that also includes the priority target -- might not need
    /// </summary>
    void GetBestTargetTile()
    {
        bestTargetTile = null;

        Tile tempTile = null;
        List<Tile> rangeTiles = new List<Tile>();
        List<Tile> affectTiles = new List<Tile>();
        List<GameObject> possibleTargets = new List<GameObject>();
        int targetCount = 0;

        RaycastHit2D[] rangeHits = Physics2D.RaycastAll(transform.position, Vector3.back, 1);

        foreach (RaycastHit2D tile in rangeHits) //sets parent tile
        {
            if (tile.collider.gameObject.tag == "Tile")
            {
                Tile t = tile.collider.gameObject.GetComponent<Tile>();
                pattern.GetRangePattern(t, chosenAttack.rangeIndex);
                foreach (Tile rangeTile in pattern.pattern)
                {
                    rangeTiles.Add(rangeTile);
                }
                //rangeTiles = pattern.pattern;
                break;
            }
        }

        foreach (Tile t in rangeTiles)
        {
            RaycastHit2D[] rangeTilesToCheck = Physics2D.RaycastAll(t.transform.position, Vector3.zero, 1);
            foreach (RaycastHit2D target in rangeTilesToCheck)
            {
                possibleTargets.Clear();
                if (target.collider.gameObject.tag == "Tile")
                {
                    Tile rangeTileToCheck = target.collider.gameObject.GetComponent<Tile>();
                    pattern.GetAffectPattern(rangeTileToCheck, chosenAttack.patternIndex);
                    affectTiles = pattern.pattern;

                    //check shielded for affect tiles using t as center

                    BSM.centerTile = t.gameObject;

                    foreach (Tile targetTile in affectTiles)
                    {
                        //Debug.Log("checking " + targetTile.gameObject.name + " for shielded");
                        //t.CheckIfTileIsShielded(targetTile);
                    }

                    foreach (Tile targetTile in affectTiles)
                    {
                        //Debug.Log("if " + targetTile.gameObject.name + " is shielded: " + targetTile.shielded);

                        RaycastHit2D[] affectHits = Physics2D.RaycastAll(targetTile.transform.position, Vector3.forward, 1);
                        foreach (RaycastHit2D affectedTarget in affectHits)
                        {
                            //Debug.Log("checking " + affectedTarget.collider.gameObject.tag + ", chosen target tag: " + targetType.ToString() + ", chosenTarget: " + chosenTarget.name);

                            int checkIfValuesMatch = string.Compare(affectedTarget.collider.gameObject.tag, targetType.ToString());

                            if (targetTile.shielded)
                            {
                                Debug.Log("shielded: " + targetTile.gameObject.name);
                            }

                            if (checkIfValuesMatch == 0)
                            {
                                possibleTargets.Add(affectedTarget.collider.gameObject);
                            }
                        }
                    }
                    if (possibleTargets.Count > targetCount && possibleTargets.Contains(chosenTarget))
                    {
                        tempTile = t;
                        targetCount = possibleTargets.Count;
                    }
                }
            }
        }

        Debug.Log("reached the end");
        bestTargetTile = tempTile;
        //gettingBestTargetTile = false;
    }

    //----- for THINK phase-----

    protected IEnumerator Think()
    {
        Debug.Log("-----THINKING-----");

        showingThinkIcon = true;
        StartCoroutine(ShowThinkingIcon());

        doneThinking = false;
        bestMoveTile = null;
        bestTargetTile = null;
        targets.Clear();

        FindSelectableTiles();

        //foreach tile in walkable range (pathable) <-- pathable loop
        List<Tile> pathableTiles = GetPathableTiles();
        List<Tile> possibleAttackTiles = new List<Tile>();
        List<Tile> fPossibleAttackTiles = new List<Tile>();
        List<BaseEnemyBehaviorMove> possibleFinalTiles = new List<BaseEnemyBehaviorMove>();
        
        startingTile = currentTile;

        int highCount = 0;

        foreach (Tile pathableTile in pathableTiles)
        {
            //Debug.Log("pathableTile: " + pathableTile.gameObject.name);

            //simulate range for chosen attack using tile in loop as parent tile
            //foreach tile in attack range <-- range loop
            foreach (Tile attackTile in GetTilesInAttackRange(pathableTile))
            {
                //Debug.Log("attackTile: " + attackTile.gameObject.name);

                //simulate affect pattern using tile in above loop as parent tile
                //foreach tile in affect range <-- affect loop
                int targetCount = 0;
                foreach (Tile affectTile in GetTilesInAffectRange(attackTile))
                {
                    //Debug.Log("affectTile: " + affectTile.gameObject.name);

                    //this is every affected tile after simulating all attackable tiles after simulating all available pathable tiles.
                    //get count of how many targets affected
                    if (IfPossibleTargetOnTile(affectTile))
                    {
                        targetCount++;
                    }
                }

                if (targetCount > 0 && !possibleAttackTiles.Contains(attackTile) && !attackTile.shieldable)
                {
                    //Debug.Log("adding possible attack tile: " + attackTile.gameObject.name);
                    possibleAttackTiles.Add(attackTile);

                    BaseEnemyBehaviorMove newTileGroup = new BaseEnemyBehaviorMove
                    {
                        moveTile = pathableTile,
                        attackTile = attackTile
                    };
                    possibleFinalTiles.Add(newTileGroup);
                }
            }
        }

        foreach (Tile possibleAttackTile in possibleAttackTiles)
        {
            BSM.centerTile = possibleAttackTile.gameObject;
            List<GameObject> tempTargetList = new List<GameObject>();

            int count = 0;

            //Debug.Log("possible attack tile: " + possibleAttackTile.gameObject.name);

            //clear shielded for all potential tiles if they are marked already
            foreach (Tile affectTile in GetTilesInAffectRange(possibleAttackTile))
            {
                affectTile.shielded = false;
            }

            //check for shielded tiles
            foreach (Tile affectTile in GetTilesInAffectRange(possibleAttackTile))
            {
                if (possibleAttackTile != affectTile)
                {
                    yield return GetShieldableTiles(affectTile);
                }
            }

            //get count of how many targets affected
            foreach (Tile affectTile in GetTilesInAffectRange(possibleAttackTile))
            {
                if (IfPossibleTargetOnTile(affectTile) && !tempTargetList.Contains(ObjectOnTile(affectTile)) && affectTile.shielded == false)
                {
                    tempTargetList.Add(ObjectOnTile(affectTile));
                    count++;
                }
            }

            //Get possible attack tiles with highest count of targets
            if (count > 0)
            {
                if (count > highCount) //new high count
                {
                    fPossibleAttackTiles.Clear();

                    highCount = count;

                    //Debug.Log("Adding to final list: " + possibleAttackTile.gameObject.name + " with count of " + count);
                    fPossibleAttackTiles.Add(possibleAttackTile);
                } else if (count == highCount) //same as high count
                {
                    //Debug.Log("Adding to final list: " + possibleAttackTile.gameObject.name + " with count of " + count);
                    fPossibleAttackTiles.Add(possibleAttackTile);
                }
            }
        }

        //simulate affect tiles for all possible attack tiles
        foreach (Tile possibleFinalAttackTile in fPossibleAttackTiles)
        {
            //Debug.Log("possible final attack tile: " + possibleFinalAttackTile.gameObject.name);
            foreach (Tile affectTile in GetTilesInAffectRange(possibleFinalAttackTile))
            {
                affectTile.shielded = false;
            }

            //check for shielded tiles
            foreach (Tile affectTile in GetTilesInAffectRange(possibleFinalAttackTile))
            {
                if (possibleFinalAttackTile != affectTile)
                {
                    yield return GetShieldableTiles(affectTile);
                }
            }
            SetBestPositionTile(GetTilesInAffectRange(possibleFinalAttackTile));
        }

        //set best tile to move as the tile from range loop and best tile to attack as the tile from affect loop
        tempPosition = 0; //reset tempPosition

        Debug.Log("-----------------------------");
        //Debug.Log("Best target tile: " + bestTargetTile.gameObject.name);

        foreach (BaseEnemyBehaviorMove posTiles in possibleFinalTiles)
        {
            //Debug.Log("posTiles - move tile: " + posTiles.moveTile.gameObject.name + ", attackTile: " + posTiles.attackTile.gameObject.name);
            if (bestPositionTile.gameObject == posTiles.attackTile.gameObject)
            {
                Debug.Log("Best move tile: " + posTiles.moveTile.gameObject.name);
                Debug.Log("Best attack tile: " + posTiles.attackTile.gameObject.name);
                
                bestTargetTile = posTiles.attackTile;
                bestMoveTile = posTiles.moveTile;
            }
        }

        Debug.Log("-----------------------------");

        //finally set the shielded tiles based on best tile to attack as center tile and gather targets
        if (bestTargetTile != null)
        {
            BSM.centerTile = bestTargetTile.gameObject;

            //set the shielded tiles
            foreach (Tile affectTile in GetTilesInAffectRange(bestTargetTile))
            {
                yield return GetShieldableTiles(affectTile);
            }

            //set targets
            foreach (Tile affectTile in GetTilesInAffectRange(bestTargetTile))
            {
                if (!affectTile.shielded && IfPossibleTargetOnTile(affectTile))
                {
                    targets.Add(ObjectOnTile(affectTile));
                }
            }
        }

        Debug.Log("-----DONE THINKING-----");

        showingThinkIcon = false;
        doneThinking = true;
    }

    IEnumerator ShowThinkingIcon()
    {
        GameObject icon = gameObject.transform.Find("ThinkingIcon").gameObject;

        float rotateSpeed = 2.0f;
        
        if (!icon.GetComponent<SpriteRenderer>().enabled)
        {
            icon.GetComponent<SpriteRenderer>().enabled = true;
        }

        while (showingThinkIcon)
        {
            icon.transform.Rotate(0.0f, 0.0f, (-1 * rotateSpeed));

            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForEndOfFrame();
        }

        icon.transform.rotation = new Quaternion(0, 0, 0, 0);
        icon.GetComponent<SpriteRenderer>().enabled = false;
    }

    List<Tile> GetPathableTiles()
    {
        List<Tile> pathableTiles = new List<Tile>();

        GameObject[] allTiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tileObj in allTiles)
        {
            if (IfPossibleTargetOnTile(tileObj.GetComponent<Tile>()) && ObjectOnTile(tileObj.GetComponent<Tile>()).tag == "Hero")
            {
                tileObj.GetComponent<Tile>().pathable = false;
            }

            if (tileObj.GetComponent<Tile>().pathable)
            {
                pathableTiles.Add(tileObj.GetComponent<Tile>());
            }
        }

        return pathableTiles;
    }

    List<Tile> GetTilesInAttackRange(Tile parentTile)
    {
        Pattern attackRangePattern = new Pattern();
        List<Tile> tilesInAttackRange = attackRangePattern.GetRangePattern(parentTile, chosenAttack.rangeIndex);

        return tilesInAttackRange;
    }

    List<Tile> GetTilesInAffectRange(Tile parentTile)
    {
        Pattern affectRangePattern = new Pattern();
        List<Tile> tilesInAffectRange = affectRangePattern.GetAffectPattern(parentTile, chosenAttack.patternIndex);

        return tilesInAffectRange;
    }

    bool IfPossibleTargetOnTile(Tile tileToCheck)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.forward, 1);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == chosenTarget.tag)
            {
                return true;
            }
        }

            return false;
    }

    void SetBestPositionTile(List<Tile> affectTiles)
    {
        int position = 0;

        foreach (Tile affectTile in affectTiles)
        {
            position++;
            if (!affectTile.shielded && ObjectOnTile(affectTile) == chosenTarget)
            {
                if (tempPosition == 0)
                {
                    bestPositionTile = affectTile;
                    tempPosition = position;
                    break;
                } else if (position < tempPosition)
                {
                    bestPositionTile = affectTile;
                    break;
                }

            }
        }
    }

    GameObject ObjectOnTile(Tile tileToCheck)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.forward, 1);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Hero" || hit.collider.gameObject.tag == "Enemy")
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    IEnumerator GetShieldableTiles(Tile tile)
    {
        //check each tile in each direction 1 tile - done
        //if tile is shieldable or shielded - done
        //get direction of that tile from center tile - done
        //check tile 1 over in that direction - done
        //if in affect - done
        //mark tile as shielded - done

        yield return new WaitForEndOfFrame();

        string dir = "null";
        Tile tileToCheck = null;

        //Debug.Log(BSM.centerTile.name);
        //Debug.Log("checking for shieldable tiles from: " + BSM.centerTile.name + " to: " + tile.gameObject.name);

        //up
        RaycastHit2D[] upHits = Physics2D.RaycastAll(tile.transform.position, Vector3.up, 1);
        foreach (RaycastHit2D target in upHits)
        {
            if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>() != tile)
            {
                if (target.collider.gameObject.GetComponent<Tile>().shieldable || target.collider.gameObject.GetComponent<Tile>().shielded)
                {
                    if (DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) != "null")
                    {
                        //Debug.Log(target.collider.gameObject.name + " is shieldable/shielded and is " + DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) + " from center tile");
                        tileToCheck = target.collider.gameObject.GetComponent<Tile>();
                        dir = DirectionFromCenterTile(tileToCheck);
                    }
                }
            }
        }
        //down
        RaycastHit2D[] downHits = Physics2D.RaycastAll(tile.transform.position, Vector3.down, 1);
        foreach (RaycastHit2D target in downHits)
        {
            if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>() != tile)
            {
                if (target.collider.gameObject.GetComponent<Tile>().shieldable || target.collider.gameObject.GetComponent<Tile>().shielded)
                {
                    if (DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) != "null")
                    {
                        //Debug.Log(target.collider.gameObject.name + " is shieldable/shielded and is " + DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) + " from center tile");
                        tileToCheck = target.collider.gameObject.GetComponent<Tile>();
                        dir = DirectionFromCenterTile(tileToCheck);
                    }
                }
            }
        }
        //left
        RaycastHit2D[] leftHits = Physics2D.RaycastAll(tile.transform.position, Vector3.left, 1);
        foreach (RaycastHit2D target in leftHits)
        {
            if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>() != tile)
            {
                if (target.collider.gameObject.GetComponent<Tile>().shieldable || target.collider.gameObject.GetComponent<Tile>().shielded)
                {
                    if (DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) != "null")
                    {
                        //Debug.Log(target.collider.gameObject.name + " is shieldable/shielded and is " + DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) + " from center tile");
                        tileToCheck = target.collider.gameObject.GetComponent<Tile>();
                        dir = DirectionFromCenterTile(tileToCheck);
                    }
                }
            }
        }
        //right
        RaycastHit2D[] rightHits = Physics2D.RaycastAll(tile.transform.position, Vector3.right, 1);
        foreach (RaycastHit2D target in rightHits)
        {
            if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>() != tile)
            {
                if (target.collider.gameObject.GetComponent<Tile>().shieldable || target.collider.gameObject.GetComponent<Tile>().shielded)
                {
                    if (DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) != "null")
                    {
                        //Debug.Log(target.collider.gameObject.name + " is shieldable/shielded and is " + DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) + " from center tile");
                        tileToCheck = target.collider.gameObject.GetComponent<Tile>();
                        dir = DirectionFromCenterTile(tileToCheck);
                    }
                }
            }
        }

        if (dir != "null" && tileToCheck != null)
        {
            if (dir == "up" && tileToCheck.gameObject != BSM.centerTile)
            {
                //Debug.Log("checking 1 tile up from " + tileToCheck.gameObject.name);
                RaycastHit2D[] dirHits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.up, 1);
                foreach (RaycastHit2D target in dirHits)
                {
                    if (target.collider.gameObject.tag == "Tile")
                    {
                        //Debug.Log("shielding " + target.collider.gameObject.name);
                        SetShielded(target.collider.gameObject);
                    }
                }
            }
            if (dir == "down" && tileToCheck.gameObject != BSM.centerTile)
            {
                //Debug.Log("checking 1 tile down from " + tileToCheck.gameObject.name);
                RaycastHit2D[] dirHits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.down, 1);
                foreach (RaycastHit2D target in dirHits)
                {
                    if (target.collider.gameObject.tag == "Tile")
                    {
                        SetShielded(target.collider.gameObject);
                    }
                }
            }
            if (dir == "left" && tileToCheck.gameObject != BSM.centerTile)
            {
                //Debug.Log("checking 1 tile left from " + tileToCheck.gameObject.name);
                RaycastHit2D[] dirHits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.left, 1);
                foreach (RaycastHit2D target in dirHits)
                {
                    if (target.collider.gameObject.tag == "Tile")
                    {
                        SetShielded(target.collider.gameObject);
                    }
                }
            }
            if (dir == "right" && tileToCheck.gameObject != BSM.centerTile)
            {
                //Debug.Log("checking 1 tile right from " + tileToCheck.gameObject.name);
                RaycastHit2D[] dirHits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.right, 1);
                foreach (RaycastHit2D target in dirHits)
                {
                    if (target.collider.gameObject.tag == "Tile")
                    {
                        SetShielded(target.collider.gameObject);
                    }
                }
            }
        }
    }

    void SetShielded(GameObject tileObj)
    {
        //Debug.Log("shielding " + tileObj.name);
        tileObj.GetComponent<Tile>().shielded = true;
        tileObj.GetComponent<Tile>().inAffect = false;
    }

    string DirectionFromCenterTile(Tile tileToCheck)
    {
        Vector3 centerTilePos = BSM.centerTile.gameObject.transform.position;
        Vector3 tileToCheckPos = tileToCheck.gameObject.transform.position;

        //Debug.Log(centerTilePos + " - " + tileToCheckPos);

        if (centerTilePos.x != tileToCheckPos.x && centerTilePos.y != tileToCheckPos.y)
        {
            return "null";
        }
        else
        {
            if (centerTilePos.x > tileToCheckPos.x)
            {
                //Debug.Log(BSM.centerTile.gameObject.name + " is right of " + tileToCheck.gameObject.name);
                return "left";
            }

            if (centerTilePos.x < tileToCheckPos.x)
            {
                //Debug.Log(BSM.centerTile.gameObject.name + " is left of " + tileToCheck.gameObject.name);
                return "right";
            }

            if (centerTilePos.y > tileToCheckPos.y)
            {
                //Debug.Log(BSM.centerTile.gameObject.name + " is up of " + tileToCheck.gameObject.name);
                return "down";
            }

            if (centerTilePos.y < tileToCheckPos.y)
            {
                //Debug.Log(BSM.centerTile.gameObject.name + " is down of " + tileToCheck.gameObject.name);
                return "up";
            }
        }

        return "null";
    }

    void UpdateShieldable(Tile tile)
    {
        RaycastHit2D[] tilesHit = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);
        foreach (RaycastHit2D target in tilesHit)
        {
            if (target.collider.gameObject.tag == "Shieldable")
            {
                //Debug.Log("Setting " + tile.gameObject.name + " as shieldable");
                tile.shieldable = true;
            }
        }
    }

    //-----------------------------------------

    /// <summary>
    /// Facilitates receiving damage from hero/enemy
    /// </summary>
    /// <param name="getDamageAmount">Damage value to be received</param>
    public void TakeDamage(int getDamageAmount)
    {
        self.curHP -= getDamageAmount; //lowers current HP from damageAmount parameter
        if (self.curHP <= 0) //checks if enemy is dead
        {
            self.curHP = 0; //sets HP to 0 if lower than 0
            ESM.currentState = TurnState.DEAD; //changes enemy state to DEAD
        }
    }

    /// <summary>
    /// Recovers MP based on spirit value and calculations
    /// </summary>
    void RecoverMPAfterTurn()
    {
        if (self.curMP < self.baseMP)
        {
            self.curMP += Mathf.CeilToInt(self.baseSpirit * .15f);
            Debug.Log(self.name + " recovering " + Mathf.Ceil(self.baseSpirit * .15f) + " MP");
        }

        if (self.curMP > self.baseMP)
        {
            self.curMP = self.baseMP;
        }
    }

    /// <summary>
    /// Gather status effects from selected attack
    /// </summary>
    void GetStatusEffectsFromCurrentAttack()
    {
        //if target is ally, add to ally's active status effects. if target is hero, add to hero's list
        foreach (BaseStatusEffect statusEffect in BSM.PerformList[0].chosenAttack.statusEffects)
        {
            BaseEffect effectToApply = new BaseEffect();
            effectToApply.effectName = statusEffect.name;
            effectToApply.effectType = statusEffect.effectType.ToString();
            effectToApply.turnsRemaining = statusEffect.turnsApplied;
            effectToApply.baseValue = statusEffect.baseValue + self.baseMATK;

            foreach (GameObject statusTarget in targets)
            {
                if (statusTarget.tag == "Hero")
                {
                    Debug.Log("Status effect: " + effectToApply.effectName + ", type: " + effectToApply.effectType + " - applied to " + statusTarget.GetComponent<HeroStateMachine>().hero.name);
                    statusTarget.GetComponent<HeroStateMachine>().activeStatusEffects.Add(effectToApply);
                }
                else if (statusTarget.tag == "Enemy")
                {
                    Debug.Log("Status effect: " + effectToApply.effectName + ", type: " + effectToApply.effectType + " - applied to " + statusTarget.GetComponent<EnemyStateMachine>().enemy.name);
                    statusTarget.GetComponent<EnemyBehavior>().activeStatusEffects.Add(effectToApply);
                }
            }
        }

        ProcessStatusEffects();
    }

    /// <summary>
    /// Processes the status effects applied by the selected attack
    /// </summary>
    void ProcessStatusEffects()
    {
        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            StatusEffect effectToProcess = new StatusEffect();

            effectToProcess.eb = this;

            effectToProcess.ProcessEffect(activeStatusEffects[i].effectName, activeStatusEffects[i].effectType, activeStatusEffects[i].baseValue, this.gameObject);

            StartCoroutine(BSM.ShowDamage(effectDamage, this.gameObject)); //displays calculated effect damage

            activeStatusEffects[i].turnsRemaining--; //lowers turns remaining by 1

            Debug.Log(self.name + " - turns remaining on " + activeStatusEffects[i].effectName + ": " + activeStatusEffects[i].turnsRemaining);

            if (activeStatusEffects[i].turnsRemaining == 0) //removes status effect if no more turns remaining
            {
                Debug.Log(activeStatusEffects[i].effectName + " removed from " + self.name);
                activeStatusEffects.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Returns random value between provided min and max values
    /// </summary>
    /// <param name="min">Minimum value for random value</param>
    /// <param name="max">Maximum value for random value</param>
    protected int GetRandomNumber(int min, int max)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int rand = Random.Range(min, max);
        return rand;
    }

    /// <summary>
    /// Increases turn count and sets up enemy to choose an action
    /// </summary>
    public void BeginEnemyTurn()
    {
        turn++;
        behaviorStates = BehaviorStates.CHOOSEACTION;

        Debug.Log("turning on animation - onTurn");
        gameObject.GetComponent<Animator>().SetBool("onTurn", true);
    }

    /// <summary>
    /// Facilitates ending the enemy's turn and resets variables so new turn can be started
    /// </summary>
    void FinishAction()
    {
        GetStatusEffectsFromCurrentAttack();

        if (chosenTarget.tag == "Hero")
        {
            chosenTarget.gameObject.GetComponent<HeroStateMachine>().UpdateHeroStats();
        }

        BSM.PerformList.RemoveAt(0); //removes this performer from the perform list in battle state manager
        RecoverMPAfterTurn(); //slowly recover MP based on spirit value

        BSM.pendingTurn = false;

        BSM.battleState = battleStates.WAIT; //reset battle state manager back to wait state

        ESM.actionStarted = false; //end the coroutine

        ESM.cur_cooldown = 0f; //reset the enemy ATB to 0
        ESM.currentState = TurnState.PROCESSING; //starts the turn over from waiting for the enemy ATB gauge to fill

        foundTarget = false;
        readyForAction = false;

        ClearTiles();
        RemoveSelectableTiles();

        Debug.Log("turning off animation - onTurn");
        gameObject.GetComponent<Animator>().SetBool("onTurn", false);

        if (BattleCameraManager.instance.camState != camStates.LOSS)
            BattleCameraManager.instance.camState = camStates.IDLE;
    }

    /// <summary>
    /// Resets variables so new turn can be started, however does not process status effects from attack as turn was skipped (no available actions)
    /// </summary>
    protected void SkipTurn()
    {
        RemoveSelectableTiles();

        RecoverMPAfterTurn(); //slowly recover MP based on spirit value

        BSM.pendingTurn = false;

        BSM.battleState = battleStates.WAIT; //reset battle state manager back to wait state

        ESM.actionStarted = false; //end the coroutine

        ESM.cur_cooldown = 0f; //reset the enemy ATB to 0
        ESM.currentState = TurnState.PROCESSING; //starts the turn over from waiting for the enemy ATB gauge to fill

        foundTarget = false;
        readyForAction = false;
        
        behaviorStates = BehaviorStates.IDLE;

        Debug.Log("turning off animation - onTurn");
        gameObject.GetComponent<Animator>().SetBool("onTurn", false);

        if (BattleCameraManager.instance.camState != camStates.LOSS)
            BattleCameraManager.instance.camState = camStates.IDLE;
    }

    /// <summary>
    /// Sets inAffect for all tiles in affect pattern for given attack
    /// </summary>
    /// <param name="attack">Attack to gather affect pattern</param>
    /// <param name="parentTile">Center tile for the pattern to be displayed</param>
    void ShowAffectPattern(BaseAttack attack, Tile parentTile)
    {
        List<Tile> affectPattern = pattern.GetAffectPattern(parentTile, attack.patternIndex);

        foreach (Tile rangeTile in affectPattern.ToArray())
        {
            //rangeTile.CheckIfTileIsShielded(rangeTile);

            /*if (!rangeTile.shielded)
            {
                rangeTile.inAffect = true;
            }*/

            rangeTile.inAffect = true;
        }
    }

    /// <summary>
    /// Sets inRange for all tiles in range pattern for given attack
    /// </summary>
    /// <param name="attack">Attack to gather range pattern</param>
    /// <param name="parentTile">Center tile for the pattern to be displayed</param>
    void ShowRangePattern(BaseAttack attack, Tile parentTile)
    {
        List<Tile> rangePattern = pattern.GetRangePattern(parentTile, attack.rangeIndex);

        foreach (Tile rangeTile in rangePattern.ToArray())
        {
            rangeTile.inRange = true;
        }
    }

    /// <summary>
    /// Sets all tiles inAffect and inRange to false
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
    /// Processes if enemy graphic should move to target and moves to them if needed.  If true, enemy moves to range. (Mage)  If false, enemy moves to target. (Warrior) - disabled for now
    /// </summary>
    protected void MoveEnemy()
    {
        if (!foundPath)
        {
            Debug.Log("calculating enemy move");
            CalculateEnemyMove();
            foundPath = true;
        }

        Debug.Log("moving to target");
        MoveToTarget();

        GetComponent<EnemyStateMachine>().startPosition = transform.position;
    }

    /// <summary>
    /// Facilitates which tile to move to using CalculatePath and finding the best move tile for the path to be calculated
    /// </summary>
    protected void CalculateEnemyMove()
    {
        if (bestMoveTile == null) //target not in range, need to move to closest tile to target
        {
            CalculatePath(GetTargetTile(chosenTarget));

            for (int i = 2; i < gameObject.GetComponent<EnemyStateMachine>().enemy.baseMoveRating; i++)
            {
                path.Pop();
                Debug.Log(path.Peek());
            }
            bestMoveTile = path.Peek();
            path.Clear();
        }

        CalculatePath(bestMoveTile);
    }

    /// <summary>
    /// Returns true if given attack is within enemy's MP threshold
    /// </summary>
    /// <param name="magic">Attack to gather MP cost from to check if enemy can cast it</param>
    protected bool HasEnoughMP(BaseAttack magic)
    {
        bool hasEnoughMP = false;

        if (self.curMP >= magic.MPCost)
        {
            return true;
        }

        return hasEnoughMP;
    }

    //----------------------------------------Target seeking algorithms------------------------------------------

    /// <summary>
    /// Returns closest hero GameObject to enemy
    /// </summary>
    protected GameObject FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Hero");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
        
        foundTarget = true;
        return target;
    }

    /// <summary>
    /// Sets target to hero with highest threat
    /// </summary>
    protected void FindHighestThreatTarget()
    {
        target = GetHeroWithHighestThreat();
    }

    /// <summary>
    /// Returns hero GameObject with highest threat to attached enemy
    /// </summary>
    protected GameObject GetHeroWithHighestThreat()
    {
        Debug.Log("checking for highest threat");
        GameObject highestThreatTarget = null;
        foreach (BaseThreat threat in threatList)
        {
            if (threat.threat == maxThreat && maxThreat != 0)
            {
                highestThreatTarget = threat.heroObject;
            }
        }

        if (maxThreat == 0)
        {
            Debug.Log("Nobody has highest threat - returning nearest enemy");
            target = FindNearestTarget();
        } else
        {
            Debug.Log("Highest threat target is " + highestThreatTarget);
            target = highestThreatTarget;
        }

        foundTarget = true;
        actionTarget = target;

        return target;
    }

    /// <summary>
    /// Returns % of HP for given GameObject hero/enemy
    /// </summary>
    protected float GetHPPercent(GameObject obj)
    {
        float percent = 0.0f;
        float curHP = 0;
        float maxHP = 0;

        if (obj.tag == "Hero")
        {
            BaseHero hero = obj.GetComponent<HeroStateMachine>().hero;
            curHP = hero.curHP;
            maxHP = hero.finalMaxHP;
        } else if (obj.tag == "Enemy")
        {
            BaseEnemy enemy = obj.GetComponent<EnemyStateMachine>().enemy;
            curHP = enemy.curHP;
            maxHP = enemy.baseHP;
        }

        percent = curHP / maxHP * 100;

        return percent;
    }

    /// <summary>
    /// Returns GameObject with lowest HP % with given enemy/hero tag
    /// </summary>
    /// <param name="tag">Use Hero or Enemy</param>
    protected GameObject GetLowestHPPercent(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        float lowestHPPercent = 0;
        float tempHPPercent = 0;

        foreach (GameObject tar in targets)
        {
            tempHPPercent = GetHPPercent(tar);
            
            if (lowestHPPercent == 0 || tempHPPercent < lowestHPPercent)
            {
                lowestHPPercent = tempHPPercent;
                target = tar;
            }
        }

        foundTarget = true;
        actionTarget = target;
        return target;
    }

    /// <summary>
    /// Returns if given GameObject hero/enemy has given status effect applied
    /// </summary>
    /// <param name="obj">GameObject hero/enemy to check</param>
    /// <param name="effectName">Status effect to check</param>
    protected bool IfStatusApplied(GameObject obj, string effectName)
    {
        bool ifStatusExists = false;

        List<BaseEffect> effectsOnObj = new List<BaseEffect>();

        if (obj.tag == "Hero")
        {
            effectsOnObj = obj.GetComponent<HeroStateMachine>().activeStatusEffects;
        } else if (obj.tag == "Enemy")
        {
            effectsOnObj = activeStatusEffects;
        }

        target = obj;

        foreach (BaseEffect checkEffect in effectsOnObj)
        {
            if (checkEffect.effectName == effectName)
            {
                ifStatusExists = true;
                foundTarget = true;
                break;
            }
        }
        
        foundTarget = true;
        actionTarget = target;

        return ifStatusExists;
    }
}
