﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
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

    private PlayerMove playerMove;
    private bool calculatedTilesToMove;

    public int heroTurn;

    protected List<GameObject> targetsInRange = new List<GameObject>();
    Pattern pattern = new Pattern();
    public List<GameObject> targets = new List<GameObject>();
    public bool choosingTarget;
    List<Tile> tilesInRange = new List<Tile>();

    void Start()
    {
        HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer"); //find spacer and make connection

        CreateHeroPanel(); //create panel and fill in info

        if (hero.curHP == 0) //if hero starts battle at 0 HP
        {
            ProgressBar.transform.localScale = new Vector2(0, ProgressBar.transform.localScale.y); //reduces ATB gauge to 0
            cur_cooldown = 0; //Sets ATB gauge to 0
            currentState = HeroStateMachine.TurnState.DEAD; //sets hero in dead state
        } else //if hero is alive
        {
            cur_cooldown = Random.Range(0, 2.5f); //Sets random point for ATB gauge to start
            currentState = TurnState.PROCESSING; //begins hero processing phase
        }
        heroTurn = 1;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>(); //make connection to the global battle state manager
        Selector.SetActive(false); //hide hero selector cursor
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
                    
                    Selector.SetActive(false); //hide the selector cursor for the hero
                    //reset GUI
                    BSM.actionPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
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
                    
                    this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color32(105, 105, 105, 255); //change color/ play animation
                    Debug.Log(hero.name + " - DEAD");
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;

                    alive = false;
                }
            break;
        }
    }

    //create hero panel
    void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject; //creates gameobject of heroPanel prefab (display in BattleCanvas which shows ATB gauge and HP, MP, etc)
        stats = HeroPanel.GetComponent<HeroPanelStats>(); //gets the hero panel's stats script
        stats.HeroName.text = hero.name; //sets hero name in the hero panel to the current hero's name
        stats.HeroHP.text = "HP: " + hero.curHP + "/" + hero.maxHP; //sets HP in the hero panel to the current hero's HP
        stats.HeroMP.text = "MP: " + hero.curMP + "/" + hero.maxMP; //sets MP in the hero panel to the current hero's MP
        ProgressBar = stats.ProgressBar; //sets ATB gauge in the hero panel to the hero's ATB
        HeroPanel.transform.SetParent(HeroPanelSpacer, false); //sets the hero panel to the hero panel's spacer for vertical layout group
    }

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
                    targets.Add(target.collider.gameObject); //adds all objects inside target range to targets list to be affected
                }
            }
        }

        return targets;
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

    private void TimeForAction()
    {
        if (BSM.PerformList[0].actionType == HandleTurn.ActionType.ATTACK)
        {
            StartCoroutine(AttackAnimation());
        }

        if (BSM.PerformList[0].actionType == HandleTurn.ActionType.MAGIC)
        {
            StartCoroutine(MagicAnimation());
        }

        if (BSM.PerformList[0].actionType == HandleTurn.ActionType.ITEM)
        {
            StartCoroutine(ItemAnimation());
        }
        
        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE) //if the battle is still going (didn't win or lose)
        {
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT; //sets battle state machine back to WAIT

            cur_cooldown = 0f; //reset the hero's ATB gauge to 0
            currentState = TurnState.PROCESSING; //starts the turn over back to filling up the ATB gauge
        } else
        {
            currentState = TurnState.WAITING; //if the battle is in win or lose state, turns the hero back to WAITING (idle) state
        }
    }

    public IEnumerator AttackAnimation()
    {
        if (actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have gone through it already
        }

        actionStarted = true;

        //animate the hero to the enemy (this is where attack animations will go)
        Vector2 enemyPosition = new Vector2(ActionTarget.transform.position.x + 1.5f, ActionTarget.transform.position.y); //sets enemyPosition to the chosen enemy's position + a few pixels on the x axis
        while (MoveToTarget(enemyPosition)) { yield return null; } //moves the hero to the calculated position above

        yield return new WaitForSeconds(0.5f); //wait a bit

        foreach (GameObject target in targets)
        {
            int hitRoll = GetRandomInt(0, 100);
            if (hitRoll <= hero.GetHitChance(hero.currentHitRating, hero.currentAgility))
            {
                Debug.Log("Hero hits!");
                int critRoll = GetRandomInt(0, 100);
                if (critRoll <= hero.GetCritChance(hero.currentCritRating, hero.currentDexterity))
                {
                    Debug.Log("Hero crits!");
                    DoDamage(target, true); //do damage with calculations (this will change later)
                }
                else
                {
                    Debug.Log("Hero doesn't crit.");
                    DoDamage(target, false); //do damage with calculations (this will change later)
                }
                Debug.Log(hero.GetCritChance(hero.currentCritRating, hero.currentDexterity) + "% chance to crit, roll was: " + critRoll);

            }
            else
            {
                StartCoroutine(BSM.ShowMiss(ActionTarget));
                Debug.Log(hero.name + " missed!");
            }
            Debug.Log(hero.GetHitChance(hero.currentHitRating, hero.currentAgility) + "% chance to hit, roll was: " + hitRoll);
        }

        //animate the enemy back to start position
        Vector2 firstPosition = startPosition; //changes the hero's position back to the starting position
        while (MoveToTarget(firstPosition)) { yield return null; } //move the hero back to the starting position     

        PostAnimationCleanup();

        actionStarted = false;
    }

    public IEnumerator MagicAnimation()
    {
        if (actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have gone through it already
        }

        actionStarted = true;

        //animate the hero to the enemy (this is where attack animations will go)
        Vector2 enemyPosition = new Vector2(ActionTarget.transform.position.x + 1.5f, ActionTarget.transform.position.y); //sets enemyPosition to the chosen enemy's position + a few pixels on the x axis
        while (MoveToTarget(enemyPosition)) { yield return null; } //moves the hero to the calculated position above

        yield return new WaitForSeconds(0.5f); //wait a bit

        foreach (GameObject target in targets)
        {
            DoDamage(target, false);
        }

        //animate the enemy back to start position
        Vector2 firstPosition = startPosition; //changes the first position of the animation back to the starting position of the enemy
        while (MoveToTarget(firstPosition)) { yield return null; } //moves back towards the starting position
        
        foreach (GameObject target in targets)
        {
            if (BSM.PerformList[0].chosenAttack.magicClass == BaseAttack.MagicClass.WHITE)
            {
                StartCoroutine(BSM.ShowHeal(magicDamage, target));
            } else
            {
                StartCoroutine(BSM.ShowDamage(magicDamage, target));
            }
        }

        yield return new WaitForSeconds(BSM.damageDisplayTime);

        PostAnimationCleanup();

        actionStarted = false;
    }

    public IEnumerator ItemAnimation()
    {
        if (actionStarted)
        {
            yield break; //breaks from the IEnumerator if we have gone through it already
        }

        actionStarted = true;

        //animate the hero to the enemy (this is where attack animations will go)
        Vector2 enemyPosition = new Vector2(ActionTarget.transform.position.x + 1.5f, ActionTarget.transform.position.y); //sets enemyPosition to the chosen enemy's position + a few pixels on the x axis
        while (MoveToTarget(enemyPosition)) { yield return null; } //moves the hero to the calculated position above

        yield return new WaitForSeconds(0.5f); //wait a bit

        PerformItem(); //process the item

        //animate the enemy back to start position
        Vector2 firstPosition = startPosition; //changes the hero's position back to the starting position
        while (MoveToTarget(firstPosition)) { yield return null; } //move the hero back to the starting position

        foreach (GameObject target in targets)
        {
            if (BSM.PerformList[0].chosenItem.type == Item.Types.RESTORATIVE)
            {
                StartCoroutine(BSM.ShowHeal(itemDamage, target));
            }
        }
        
        BSM.ResetItemList();

        PostAnimationCleanup();

        actionStarted = false;
    }

    void PostAnimationCleanup()
    {
        GetStatusEffectsFromCurrentAttack();

        UpdateHeroStats();

        playerMove.EndTurn(this);
    }

    private bool MoveToTarget(Vector3 target)
    {
        return target != (transform.position = Vector2.MoveTowards(transform.position, target, animSpeed * Time.deltaTime)); //moves toward target until position is same as target position
    }

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
    
    void DoDamage(GameObject target, bool crit) //deals damage to enemy
    {
        int calc_damage = 0;

        if (target.tag == "Enemy")
        {
            if (BSM.PerformList[0].chosenAttack.type == BaseAttack.Type.PHYSICAL)
            {
                calc_damage = hero.currentATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                if (crit)
                {
                    calc_damage = calc_damage * 2;
                    Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and crits for " + calc_damage + " damage to " + target.GetComponent<EnemyStateMachine>().enemy.name + "!");
                    Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + hero.currentATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " * 2 = " + calc_damage);
                    StartCoroutine(BSM.ShowCrit(calc_damage, target));
                } else
                {
                    Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<EnemyStateMachine>().enemy.name + "!");
                    Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + hero.currentATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);
                    StartCoroutine(BSM.ShowDamage(calc_damage, target));
                }

                float calc_threat = (((calc_damage / 2) + hero.currentThreatRating) * BSM.PerformList[0].chosenAttack.threatMultiplier);
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
                magicScript.ProcessMagicHeroToEnemy(); //actually process the magic to enemy

                float calc_threat = (((magicDamage / 2) + hero.currentThreatRating) * BSM.PerformList[0].chosenAttack.threatMultiplier);
                IncreaseThreat(target, hero, calc_threat);
                //StartCoroutine(BSM.ShowDamage(magicDamage, target));
            }
            else //if attack type not found
            {
                calc_damage = hero.currentATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
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
                calc_damage = hero.currentATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                if (crit)
                {
                    calc_damage = calc_damage * 2;
                    Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and crits for " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "!");
                    Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + hero.currentATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " * 2 = " + calc_damage);
                    StartCoroutine(BSM.ShowCrit(calc_damage, target));
                } else
                {
                    Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "!");
                    Debug.Log(BSM.PerformList[0].chosenAttack.name + " calc_damage - physical - hero's ATK: " + hero.currentATK + " + chosenAttack's damage: " + BSM.PerformList[0].chosenAttack.damage + " = " + calc_damage);
                    StartCoroutine(BSM.ShowDamage(calc_damage, target));
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
                magicScript.ProcessMagicHeroToHero(); //actually process the magic to hero
                //StartCoroutine(BSM.ShowDamage(magicDamage, target));
            }
            else //if attack type not found
            {
                calc_damage = hero.currentATK + BSM.PerformList[0].chosenAttack.damage; //calculates damage by hero's attack + the chosen attack's damage
                if (crit)
                {
                    calc_damage = calc_damage * 2;
                }
                Debug.Log(hero.name + " has chosen " + BSM.PerformList[0].chosenAttack.name + " and does " + calc_damage + " damage to " + target.GetComponent<HeroStateMachine>().hero.name + "! -- NOTE: ATTACK TYPE NOT FOUND: " + BSM.PerformList[0].chosenAttack.type);
            }
            target.GetComponent<HeroStateMachine>().TakeDamage(calc_damage); //processes enemy take damage by above value
        }
        
    }

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
    
    //Update stats on damage / heal
    public void UpdateHeroStats() //can maybe make this public to process when losing MP
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (GameObject heroObj in heroes)
        {
            HeroStateMachine heroHSM = heroObj.GetComponent<HeroStateMachine>();
            heroHSM.stats.HeroHP.text = "HP: " + heroHSM.hero.curHP + "/" + heroHSM.hero.maxHP;
            heroHSM.stats.HeroMP.text = "MP: " + heroHSM.hero.curMP + "/" + heroHSM.hero.maxMP;

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

    public void RecoverMPAfterTurn() //slowly recover MP based on spirit and math below
    {
        if (hero.curMP < hero.baseMP)
        {
            hero.curMP += hero.GetRegen(hero.currentRegenRating, hero.currentSpirit);
            Debug.Log(hero.name + " recovering " + hero.GetRegen(hero.currentRegenRating, hero.currentSpirit) + " MP");
        }

        if (hero.curMP > hero.maxMP)
        {
            hero.curMP = hero.maxMP;
        }
        UpdateHeroStats();
    }

    public void GetStatusEffectsFromCurrentAttack() //gets status effects to be processed from chosen attack
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

    int GetRandomInt(int min, int max)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int rand = Random.Range(min, max);
        return rand;
    }

    void UpgradeProgressBar()
    {
        cur_cooldown = (cur_cooldown + (Time.deltaTime / 1f)) + (hero.currentDexterity * .000055955f); //increases ATB gauge, first float dictates how slowly gauge fills (default 1f), while second float dictates how effective dexterity is
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
