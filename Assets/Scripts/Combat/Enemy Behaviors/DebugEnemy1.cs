using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemy1 : EnemyBehavior
{
    //This enemy only casts 'Bio' on hero targets without Poison debuff, and heals allies if they are below 50% hp

    void Start()
    {

    }
    
    void Update()
    {
        switch(behaviorStates)
        {
            case (BehaviorStates.IDLE):

            break;

            case (BehaviorStates.CHOOSEACTION):

                BuildActionLists();
                GetChosenAction();
                behaviorStates = BehaviorStates.BEFOREMOVE;

            break;

            case (BehaviorStates.THINK):
                Debug.Log("Thinking...");
                //foreach tile in walkable range (inRange) <-- range loop
                //simulate range for chosen attack using tile in loop as parent tile
                //foreach tile in attack range <-- affect loop
                //simulate affect pattern using tile in above loop as parent tile
                //check for shielded tiles
                //get count of how many targets affected
                //when count is highest, set best tile to move as the tile from range loop and best tile to attack as the tile from affect loop
                //set the shielded tiles based on best tile to attack as center tile
                //go to BEFOREMOVE
            break;

            case (BehaviorStates.BEFOREMOVE):
                
                if (AttackInRangeOfTarget(chosenTarget, chosenAttack))
                {
                    behaviorStates = BehaviorStates.ACTION;
                }
                else
                {
                    behaviorStates = BehaviorStates.MOVE;
                }

                break;

            case (BehaviorStates.MOVE):

                if (!readyForAction)
                {
                    if (foundTarget)
                    {
                        MoveEnemy(true); //move algorithm should use best tile to move from THINK phase
                    }
                } else
                {
                    behaviorStates = BehaviorStates.AFTERMOVE;
                }

            break;

            case (BehaviorStates.AFTERMOVE):

                BuildActionLists();
                
                if (AttackInRangeOfTarget(chosenTarget, chosenAttack))
                {
                    behaviorStates = BehaviorStates.ACTION;
                }
                else
                {
                    Debug.Log("No attack within range. Skipping turn");
                    SkipTurn();
                }

                break;

            case (BehaviorStates.ACTION):
                
                if (!gettingTarget)
                {
                    StartCoroutine(GetTargets(chosenAttack.patternIndex, targetType.ToString()));
                }

                if (targets.Count != 0 && !gettingTarget)
                {
                    RunAction(chosenAttack, targets);

                    Debug.Log("changing back to idle");
                    behaviorStates = BehaviorStates.IDLE;
                }

            break;
        }
    }

    void GetChosenAction()
    {
        // 1) Check HP of all allies - if lowest ally is < 50% hp, cast cure 1 on them.
        chosenAttack = ESM.enemy.attacks[1].attack;

        GameObject lowestHPAlly = GetLowestHPPercent("Enemy");
        chosenTarget = lowestHPAlly;
        
        float lowestHPPercent = GetHPPercent(lowestHPAlly);

        if (lowestHPPercent <= 99.0f && HasEnoughMP(chosenAttack))
        {
            Debug.Log("Choosing to heal " + lowestHPAlly.name + " as their HP is " + lowestHPPercent + "%");
            targetType = targetTypes.Enemy;
            return;
        }

        // 2) Check if Poison debuff is on any hero.  If not, cast Bio 1 on them
        chosenAttack = ESM.enemy.attacks[2].attack;

        foreach (GameObject hero in BSM.AllHeroesInBattle)
        {
            chosenTarget = hero;
            if (!IfStatusApplied(hero, "Poison") && HasEnoughMP(chosenAttack))
            {
                Debug.Log("Choosing to cast Bio 1 on " + hero.name);
                targetType = targetTypes.Hero;
                return;
            }
        }

        // 3) Attack target (Slash) with highest threat
        Debug.Log("Don't need to heal or cast bio, attacking highest threat target");
        chosenAttack = ESM.enemy.attacks[0].attack;
        chosenTarget = GetHeroWithHighestThreat();
        targetType = targetTypes.Hero;

        //example:
        //chosenAttack = ESM.enemy.attacks[0]; //chooses attack
        //chosenTarget = GetHeroWithHighestThreat(); //chooses target
    }
}
