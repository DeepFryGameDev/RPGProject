﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent2 : BaseScriptedEvent
{
    public void QuestTest()
    {
        if (IfQuestIsActive(GetQuest(2)))
        { 
            if (QuestObjectivesFulfilled(GetActiveQuest(2)))
            {
                Debug.Log("Yay it's complete!");
                CompleteQuest(GetActiveQuest(2));
            }
            else
            {
                Debug.Log("You still need to talk to the guy next to me!");
            }
        } else
        {
            Debug.Log("You don't have the quest!");
        }
        
    }
}
