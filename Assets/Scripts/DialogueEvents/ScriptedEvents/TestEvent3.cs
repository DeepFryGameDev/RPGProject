using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent3 : BaseScriptedEvent
{
    public void QuestTest()
    {
        if (IfQuestIsActive(GetQuest(2)))
        { 
            if (QuestBool(GetActiveQuest(2), 0))
            {
                Debug.Log("You already talked to me!");
            } else
            {
                Debug.Log("Ayyyy! Poop! Poop is funny lol!");
                MarkQuestBool(GetActiveQuest(2), 0, true);
            }
        } else
        {
            Debug.Log("You don't have the quest!");
        }
        
    }
}
