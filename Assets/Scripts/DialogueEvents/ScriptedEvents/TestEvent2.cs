using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent2 : BaseScriptedEvent
{
    public void QuestTest()
    {
        if (messageFinished)
        StartCoroutine(QuestCoroutine());
    }

    IEnumerator QuestCoroutine()
    {
        if (IfQuestIsActive(GetQuest(2)))
        {
            if (QuestObjectivesFulfilled(GetActiveQuest(2)))
            {
                Debug.Log("Yay it's complete!");
                StartCoroutine(ShowMessage("Woot!  Quest complete!", null, true, true));
                CompleteQuest(GetActiveQuest(2));
                yield break;
            }
            else
            {
                StartCoroutine(ShowMessage("You need to talk to the guy next to me!", null, true, true));
                Debug.Log("You still need to talk to the guy next to me!");
                yield break;
            }
        }
        else if (!IfQuestIsCompleted(GetQuest(2)))
        {
            StartCoroutine(ShowMessage("Go pick up the bool quest!", null, true, true));
            yield break;
        }

        else if (IfQuestIsActive(GetQuest(0)))
        {
            if (QuestObjectivesFulfilled(GetActiveQuest(0)))
            {
                Debug.Log("Yay it's complete!");
                StartCoroutine(ShowMessage("Thank you for the potions!", null, true, true));
                CompleteQuest(GetActiveQuest(0));
                yield break;
            }
            else
            {
                StartCoroutine(ShowMessage("You still need to gather potions!", null, true, true));
                Debug.Log("You still need to gather potions!");
                yield break;
            }
        }
        else if (!IfQuestIsCompleted(GetQuest(0)))
        {
            StartCoroutine(ShowMessage("Go pick up the gather quest!", null, true, true));
            Debug.Log("You don't have the gather quest!");
            yield break;
        }

        else if (IfQuestIsActive(GetQuest(1)))
        {
            if (QuestObjectivesFulfilled(GetActiveQuest(1)))
            {
                StartCoroutine(ShowMessage("The test enemy has been killed, congrats!", null, true, true));
                Debug.Log("Yay it's complete!");
                CompleteQuest(GetActiveQuest(1));
                yield break;
            }
            else
            {
                StartCoroutine(ShowMessage("You still need to kill the test enemy!", null, true, true));
                Debug.Log("You still need to kill the test enemy!");
                yield break;
            }
        }
        else if (!IfQuestIsCompleted(GetQuest(1)))
        {
            StartCoroutine(ShowMessage("You don't have the kill quest yet!", null, true, true));
            Debug.Log("You don't have the kill quest!");
            yield break;
        }
    }
}
