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
                //Debug.Log("Building action lists");
                BuildActionLists();
                //Debug.Log("getting action");
                GetChosenAction();
                behaviorStates = BehaviorStates.BEFOREMOVE;

            break;

            case (BehaviorStates.BEFOREMOVE):
                
                if (AttackInRangeOfTarget(chosenTarget, chosenAttack))
                {
                    //Debug.Log("Attack is in range");
                    behaviorStates = BehaviorStates.ACTION;
                }
                else
                {
                    //Debug.Log("attack is NOT in range. moving");
                    behaviorStates = BehaviorStates.MOVE;
                }

                break;

            case (BehaviorStates.MOVE):
                if (!readyForAction)
                {
                    if (foundTarget)
                    {
                        //Debug.Log("attack not in range, moving enemy");
                        //Debug.Log("chosen action: " + chosenAttack);
                        //Debug.Log("chosen target: " + chosenTarget);
                        MoveEnemy(true);
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
                    //Debug.Log("attack is in range");
                    behaviorStates = BehaviorStates.ACTION;
                }
                else
                {
                    Debug.Log("No attack within range. Skipping turn");
                    SkipTurn();
                }

                break;

            case (BehaviorStates.ACTION):
                
                if (chosenAttack == ESM.enemy.attacks[1]) //Cure 1
                {
                    RunAction(chosenAttack, GetTargets(chosenAttack.patternIndex, "Enemy", chosenTarget));
                } else if (chosenAttack == ESM.enemy.attacks[2]) //Bio 1
                {
                    RunAction(chosenAttack, GetTargets(chosenAttack.patternIndex, "Hero", chosenTarget));
                } else if (chosenAttack == ESM.enemy.attacks[0]) //Hammer Swing
                {
                    RunAction(chosenAttack, GetTargets(chosenAttack.patternIndex, "Hero", chosenTarget));
                }
                //Debug.Log("changing back to idle");
                behaviorStates = BehaviorStates.IDLE;

            break;
        }
    }

    void GetChosenAction()
    {
        // 1) Check HP of all allies - if lowest ally is < 50% hp, cast cure 1 on them.
        chosenAttack = ESM.enemy.attacks[1];

        GameObject lowestHPAlly = GetLowestHPPercent("Enemy");
        chosenTarget = lowestHPAlly;
        
        float lowestHPPercent = GetHPPercent(lowestHPAlly);

        if (lowestHPPercent <= 99.0f && HasEnoughMP(chosenAttack))
        {
            Debug.Log("Choosing to heal " + lowestHPAlly.name + " as their HP is " + lowestHPPercent + "%");

            return;
        }

        // 2) Check if Poison debuff is on any hero.  If not, cast Bio 1 on them
        chosenAttack = ESM.enemy.attacks[2];

        foreach (GameObject hero in BSM.AllHeroesInBattle)
        {
            chosenTarget = hero;
            if (!IfStatusApplied(hero, "Poison") && HasEnoughMP(chosenAttack))
            {
                Debug.Log("Choosing to cast Bio 1 on " + hero.name);
                return;
            }
        }

        // 3) Attack target (Hammer Swing) with highest threat
        Debug.Log("Don't need to heal or cast bio, attacking highest threat target");
        chosenAttack = ESM.enemy.attacks[0];
        chosenTarget = GetHeroWithHighestThreat();


        //example:
        //chosenAttack = ESM.enemy.attacks[0]; //chooses attack
        //chosenTarget = GetHeroWithHighestThreat(); //chooses target
    }
}
