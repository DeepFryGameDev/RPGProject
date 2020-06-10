using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : EnemyMove
{
    protected List<BaseAttack> enemySkills = new List<BaseAttack>();
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

    Tile bestTargetTile;
    Tile bestMoveTile;

    //for status effects
    public List<BaseEffect> activeStatusEffects = new List<BaseEffect>();
    public int effectDamage;
    public int magicDamage;
    public int itemDamage;

    //for threat
    public List<BaseThreat> threatList = new List<BaseThreat>();
    [HideInInspector] public int maxThreat;
    public Image threatBar;
    protected Color maxThreatColor = Color.red;
    protected Color highThreatColor = Color.yellow;
    protected Color moderateThreatcolor = Color.green;
    protected Color lowThreatcolor = Color.blue;
    protected Color veryLowThreatColor = Color.grey;


    protected enum BehaviorStates
    {
        IDLE,
        CHOOSEACTION,
        BEFOREMOVE,
        MOVE,
        AFTERMOVE,
        ACTION
    }

    protected BehaviorStates behaviorStates;

    protected bool foundTarget;

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

    protected void BuildActionLists()
    {
        ClearActionLists();

        //check which attacks are available based on MP cost of all attacks, and enemy's current MP, and adds them to 'attacksWithinMPThreshold' list.
        foreach (BaseAttack atk in self.attacks)
        {
            if (atk.MPCost <= self.curMP)
            {
                attacksWithinMPThreshold.Add(atk);
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

    void ClearActionLists()
    {
        attacksWithinMPThreshold.Clear(); //clears list for next enemy to use
        attacksWithinRange.Clear();
        targets.Clear();
    }

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

    public IEnumerator AttackAnimation()
    {
        if (ESM.actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have already gone through it
        }

        ESM.actionStarted = true;

        ShowRangePattern(chosenAttack, GetCurrentTile());
        ShowAffectPattern(chosenAttack, bestTargetTile);

        //this is where actual attack animation would go
        Vector2 targetPosition = new Vector2(actionTarget.transform.position.x - .5f, actionTarget.transform.position.y); //gets hero's position (minus a few pixels on the x axis) to move to for attack animation
        while (MoveToTarget(targetPosition)) { yield return null; } //move towards the target
        yield return new WaitForSeconds(0.5f); //wait a bit

        DoDamage(); //do damage with calculations (this will change later)

        //animate the enemy back to start position
        Vector2 firstPosition = ESM.startPosition; //changes the first position of the animation back to the starting position of the enemy
        while (MoveToTarget(firstPosition)) { yield return null; } //moves back towards the starting position

        FinishAction();
    }

    public IEnumerator MagicAnimation()
    {
        if (ESM.actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have already gone through it
        }

        ESM.actionStarted = true;

        ShowRangePattern(chosenAttack, GetCurrentTile());
        ShowAffectPattern(chosenAttack, bestTargetTile);

        //this is where actual attack animation would go
        Vector2 targetPosition = new Vector2(actionTarget.transform.position.x - .5f, actionTarget.transform.position.y); //gets hero's position (minus a few pixels on the x axis) to move to for attack animation
        while (MoveToTarget(targetPosition)) { yield return null; } //move towards the target
        yield return new WaitForSeconds(0.5f); //wait a bit

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

        //animate the enemy back to start position
        Vector2 firstPosition = ESM.startPosition; //changes the first position of the animation back to the starting position of the enemy
        while (MoveToTarget(firstPosition)) { yield return null; } //moves back towards the starting position

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

        yield return new WaitForSeconds(BSM.damageDisplayTime);

        FinishAction();
    }

    protected void RunAction(BaseAttack action, List<GameObject> targets)
    {
        HandleTurn myAttack = new HandleTurn(); //new handleturn for the enemy's attack
        myAttack.Attacker = self.name; //enemy's name set for the handleturn's attacker
        myAttack.attackerType = HandleTurn.Types.ENEMY; //sets handleturn's type to enemy
        myAttack.AttackersGameObject = this.gameObject; //sets the handleturn's attacker game object to this enemy
        
        myAttack.chosenAttack = action;

        BSM.CollectActions(myAttack); //adds chosen attack to the perform list
        
        ProcessAnimation();
    }

    private bool MoveToTarget(Vector3 target) //using Vector2 causes an error, but Vector3 translates without issues
    {
        return target != (transform.position = Vector2.MoveTowards(transform.position, target, animSpeed * Time.deltaTime)); //moves towards the target parameter until position is same as the target position
    }

    void DoDamage() //deals damage to hero
    {
        int calc_damage = self.baseATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by enemy's current attack + the attack's damage
        
        foreach (GameObject target in targets)
        {
            if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.PHYSICAL)
            {
                calc_damage = self.baseATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                target.GetComponent<HeroStateMachine>().TakeDamage(calc_damage); //processes enemy take damage by above value
                Debug.Log(self.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "!");
                Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + self.baseATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);
            }
            else if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.MAGIC)
            {
                //can check if magic attack should have a flat value, ie gravity spell
                calc_damage = self.baseMATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's magic attack + the chosen attack's damage
                target.GetComponent<HeroStateMachine>().TakeDamage(calc_damage); //processes enemy take damage by above value
                Debug.Log(self.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "!");
                Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - magic - hero's MATK: " + self.baseMATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);
            }
            else //if attack type not found
            {
                calc_damage = self.baseATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                target.GetComponent<HeroStateMachine>().TakeDamage(calc_damage); //processes enemy take damage by above value
                Debug.Log(self.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "! -- NOTE: ATTACK TYPE NOT FOUND: " + BSM.PerformList[0].chosenAttack.type);
            }
            //HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage); //processes the damage to the hero from the enemy
            //Debug.Log(this.gameObject.name + " has chosen " + BSM.PerformList[0].chosenAttack.attackName + " and does " + calc_damage + " damage to " + HeroToAttack.GetComponent<HeroStateMachine>().hero.name + "!");

            //enemy.curMP -= BSM.PerformList[0].chosenAttack.attackCost; //remove MP from enemy

            StartCoroutine(BSM.ShowDamage(calc_damage, target));
        }
    }

    protected List<GameObject> GetTargets(int affectIndex, string tag, GameObject targetChoice)
    {
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

        Tile targetTile = GetBestTargetTile(tag);
        pattern.GetAffectPattern(targetTile, affectIndex);
        tilesInRange = pattern.pattern;

        foreach (Tile t in tilesInRange)
        {
            RaycastHit2D[] tilesHit = Physics2D.RaycastAll(t.transform.position, Vector3.forward, 1);
            foreach (RaycastHit2D target in tilesHit)
            {
                if (!targets.Contains(target.collider.gameObject) && (target.collider.gameObject.tag == "Hero" || target.collider.gameObject.tag == "Enemy"))
                {
                    //Debug.Log("adding " + target.collider.gameObject + " to targets");
                    targets.Add(target.collider.gameObject); //adds all objects inside target range to targets list to be affected
                }
            }
        }

        return targets;
    }

    public void TakeDamage(int getDamageAmount) //receives damage from hero
    {
        self.curHP -= getDamageAmount; //lowers current HP from damageAmount parameter
        if (self.curHP <= 0) //checks if enemy is dead
        {
            self.curHP = 0; //sets HP to 0 if lower than 0
            ESM.currentState = EnemyStateMachine.TurnState.DEAD; //changes enemy state to DEAD
        }
    }

    void RecoverMPAfterTurn() //slowly recovers MP based on spirit value and below math
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

    void GetStatusEffectsFromCurrentAttack() //gather status effects from selected attack
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

    void ProcessStatusEffects() //processes the status effects applied by selected attack
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

    protected int GetRandomNumber(int min, int max)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int rand = Random.Range(min, max);
        return rand;
    }

    public void BeginEnemyTurn()
    {
        turn++;
        behaviorStates = BehaviorStates.CHOOSEACTION;
    }

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
        ESM.currentState = EnemyStateMachine.TurnState.PROCESSING; //starts the turn over from waiting for the enemy ATB gauge to fill

        foundTarget = false;
        readyForAction = false;

        ClearTiles();
        RemoveSelectableTiles();
    }

    protected void SkipTurn()
    {
        RemoveSelectableTiles();

        RecoverMPAfterTurn(); //slowly recover MP based on spirit value

        BSM.pendingTurn = false;

        BSM.battleState = battleStates.WAIT; //reset battle state manager back to wait state

        ESM.actionStarted = false; //end the coroutine

        ESM.cur_cooldown = 0f; //reset the enemy ATB to 0
        ESM.currentState = EnemyStateMachine.TurnState.PROCESSING; //starts the turn over from waiting for the enemy ATB gauge to fill

        foundTarget = false;
        readyForAction = false;
        
        behaviorStates = BehaviorStates.IDLE;
    }

    public void DrawThreatBar()
    {
        GameObject hero = BSM.HeroesToManage[0];

        foreach (BaseThreat threat in threatList)
        {
            if (threat.heroObject == hero && threat.threat > 0)
            {
                float threatVal = threat.threat;
                float calc_Threat = threatVal / maxThreat; //does math of % of ATB gauge to be filled
                threatBar.transform.localScale = new Vector2(Mathf.Clamp(calc_Threat, 0, 1), threatBar.transform.localScale.y); //shows graphic of threat gauge increasing

                if (calc_Threat == 1)
                {
                    threatBar.color = maxThreatColor;
                } else if (calc_Threat >= .75f && calc_Threat <= .99f)
                {
                    threatBar.color = highThreatColor;
                } else if (calc_Threat >= .50f && calc_Threat <= .75f)
                {
                    threatBar.color = moderateThreatcolor;
                } else if (calc_Threat >= .25f && calc_Threat <= .50f)
                {
                    threatBar.color = lowThreatcolor;
                } else
                {
                    threatBar.color = veryLowThreatColor;
                }

                return;
            }
        }

        //if hero is not in threat list
        threatBar.color = Color.clear;

    }

    public void ClearThreatBar()
    {
        threatBar.color = Color.clear;
    }

    void ShowAffectPattern(BaseAttack attack, Tile parentTile)
    {
        List<Tile> affectPattern = pattern.GetAffectPattern(parentTile, attack.patternIndex);

        foreach (Tile rangeTile in affectPattern.ToArray())
        {
            rangeTile.inAffect = true;
        }
    }

    void ShowRangePattern(BaseAttack attack, Tile parentTile)
    {
        List<Tile> rangePattern = pattern.GetRangePattern(parentTile, attack.rangeIndex);

        foreach (Tile rangeTile in rangePattern.ToArray())
        {
            rangeTile.inRange = true;
        }
    }

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

    protected void MoveEnemy(bool moveToRange)
    {
        if (!moveToRange)
        {
            MoveToTarget();
        } else
        {
            MoveToRange();
        }

        GetComponent<EnemyStateMachine>().startPosition = transform.position;
    }

    Tile GetBestTargetTile(string tag)
    {
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

                    foreach (Tile targetTile in affectTiles)
                    {
                        RaycastHit2D[] affectHits = Physics2D.RaycastAll(targetTile.transform.position, Vector3.forward, 1);
                        foreach (RaycastHit2D affectedTarget in affectHits)
                        {
                            if (affectedTarget.collider.gameObject.tag == tag)
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
        bestTargetTile = tempTile;

        return bestTargetTile;
    }

    Tile GetBestMoveTile()
    {
        Tile tempTile = null;
        List<Tile> moveTiles = new List<Tile>();
        List<Tile> rangeTiles = new List<Tile>();
        List<Tile> affectTiles = new List<Tile>();
        List<GameObject> possibleTargets = new List<GameObject>();
        int targetCount = 0;

        RaycastHit2D[] moveHits = Physics2D.RaycastAll(transform.position, Vector3.back, 1);

        bestMoveTile = null;

        foreach (RaycastHit2D tile in moveHits) //sets parent tile
        {
            if (tile.collider.gameObject.tag == "Tile")
            {
                GameObject[] allTiles = GameObject.FindGameObjectsWithTag("Tile");
                foreach (GameObject tileObj in allTiles)
                {
                    Tile t = tileObj.GetComponent<Tile>();
                    if (t.pathable && !moveTiles.Contains(t))
                    {
                        moveTiles.Add(t); //all pathable tiles are added here
                    }
                }
                //rangeTiles = pattern.pattern;
                break;
            }
        }

        foreach (Tile moveTile in moveTiles) //for all pathable tiles
        {
            rangeTiles.Clear();

            RaycastHit2D[] rangeHits = Physics2D.RaycastAll(moveTile.transform.position, Vector3.zero, 1);

            foreach (RaycastHit2D tile in rangeHits) //sets parent tile to the current moveTile
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

            foreach (Tile t in rangeTiles) //for each range tile on the current move tile
            {
                RaycastHit2D[] rangeTilesToCheck = Physics2D.RaycastAll(t.transform.position, Vector3.zero, 1);
                foreach (RaycastHit2D tileToCheck in rangeTilesToCheck)
                {
                    possibleTargets.Clear();
                    if (tileToCheck.collider.gameObject.tag == "Tile")
                    {
                        Tile rangeTileToCheck = tileToCheck.collider.gameObject.GetComponent<Tile>();
                        pattern.GetAffectPattern(rangeTileToCheck, chosenAttack.patternIndex);
                        affectTiles = pattern.pattern;

                        foreach (Tile targetTile in affectTiles)
                        {
                            RaycastHit2D[] affectHits = Physics2D.RaycastAll(targetTile.transform.position, Vector3.forward, 1);
                            foreach (RaycastHit2D affectedTarget in affectHits)
                            {
                                if (affectedTarget.collider.gameObject.tag == target.tag)
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
        }

        bestMoveTile = tempTile;

        return bestMoveTile;
    }

    protected void CalculateEnemyMove()
    {
        /*if (chosenTarget != null)
        {
            actualTargetTile = GetTargetTile(chosenTarget);
            target = chosenTarget;
        }*/

        //actualTargetTile = GetTargetTile(target);

        FindSelectableTiles();
        CalculatePath(GetBestMoveTile());
        
        //Debug.Log(actualTargetTile);
        actualTargetTile.target = true;
    }

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

        if (!AttackInRangeOfTarget(target, chosenAttack)) //sets up enemy for moving if not in range
        {
            CalculateEnemyMove();
        }

        //CalculateEnemyMove();
        foundTarget = true;
        return target;
    }

    protected void FindHighestThreatTarget()
    {
        target = GetHeroWithHighestThreat();
    }

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

        if (!AttackInRangeOfTarget(target, chosenAttack)) //sets up enemy for moving if not in range
        {
            CalculateEnemyMove();
        }

        //CalculateEnemyMove();
        foundTarget = true;
        actionTarget = target;

        return target;
    }

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

        if (!AttackInRangeOfTarget(target, chosenAttack)) //sets up enemy for moving if not in range
        {
            CalculateEnemyMove();
        }

        foundTarget = true;
        actionTarget = target;
        return target;
    }

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
                if (!AttackInRangeOfTarget(target, chosenAttack)) //sets up enemy for moving if not in range
                {

                    CalculateEnemyMove();
                }

                ifStatusExists = true;
                foundTarget = true;
                break;
            }
        }
        
        if (!AttackInRangeOfTarget(target, chosenAttack)) //sets up enemy for moving if not in range
        {
            CalculateEnemyMove();
        }

        foundTarget = true;
        actionTarget = target;

        return ifStatusExists;
    }
}
