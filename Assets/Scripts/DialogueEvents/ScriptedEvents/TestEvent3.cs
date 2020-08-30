using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent3 : BaseScriptedEvent
{
    public void QuestTest()
    {
        CallBattle(0, "Battle Test");
    }

    void Temp() //use above for testing quests
    {
        if (messageFinished)
        {
            if (IfQuestIsActive(GetQuest(2)))
            {
                if (QuestBool(GetActiveQuest(2), 0))
                {
                    Debug.Log("You already talked to me!");
                    StartCoroutine(ShowMessage("You already talked to me!", null, true, true));
                    return;
                }
                else
                {
                    Debug.Log("Marking quest complete!");
                    StartCoroutine(ShowMessage("Good job!  Talk to the guy to my right again to mark the quest complete!", null, true, true));
                    MarkQuestBool(GetActiveQuest(2), 0, true);
                    return;
                }
            }
            else
            {
                StartCoroutine(ShowMessage("Talk to me when you pick up the bool test quest!", null, true, true));
                Debug.Log("You don't have the quest!");
            }
        }
    }
}
