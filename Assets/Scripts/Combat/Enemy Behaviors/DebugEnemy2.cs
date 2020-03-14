using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemy2 : EnemyBehavior
{
    //This enemy only searches for the hero with highest threat (or closest if threat is 0), and attacks them with "Slash"

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
                        //Debug.Log("moving enemy");
                        MoveEnemy();
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
                
                if (chosenAttack == ESM.enemy.attacks[0])
                {
                    //Debug.Log("run attack");
                    RunAction(chosenAttack, GetTargetsInAffect(chosenAttack.patternIndex,"Hero",chosenTarget));
                }

                Debug.Log("changing back to idle");
                behaviorStates = BehaviorStates.IDLE;

            break;
        }
    }

    void GetChosenAction()
    {
        chosenAttack = ESM.enemy.attacks[0]; //chooses attack
        chosenTarget = GetHeroWithHighestThreat(); //chooses target
    }
}
