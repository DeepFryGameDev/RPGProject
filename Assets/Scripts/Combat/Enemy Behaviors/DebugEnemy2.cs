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
                behaviorStates = BehaviorStates.THINK;

            break;

            case (BehaviorStates.THINK):
                if (!thinking)
                {
                    thinking = true;
                    StartCoroutine(Think());
                }

                if (doneThinking)
                {
                    thinking = false;
                    behaviorStates = BehaviorStates.BEFOREMOVE;
                }
            break;

            case (BehaviorStates.BEFOREMOVE):

                GetCurrentTile();

                if (currentTile == bestMoveTile)
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
                        MoveEnemy();
                    }
                } else
                {
                    foundPath = false;
                    behaviorStates = BehaviorStates.AFTERMOVE;
                }

            break;

            case (BehaviorStates.AFTERMOVE):
                
                if (bestTargetTile != null)
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

                RunAction(chosenAttack, targets);

                Debug.Log("changing back to idle");
                behaviorStates = BehaviorStates.IDLE;

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
