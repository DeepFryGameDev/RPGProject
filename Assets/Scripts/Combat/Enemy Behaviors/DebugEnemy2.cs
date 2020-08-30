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
                        MoveEnemy(false); //move algorithm should use best tile to move from THINK phase
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
                    GetTargets(chosenAttack.patternIndex, targetType.ToString()); //update as its coroutine
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
        chosenAttack = self.attacks[0].attack; //chooses attack
        Debug.Log("Chosen attack: " + chosenAttack);
        chosenTarget = GetHeroWithHighestThreat(); //chooses target
        Debug.Log("Chosen target: " + chosenTarget);
        targetType = targetTypes.Hero;
    }
}
