using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent1 : BaseScriptedEvent
{
    public GameObject enemy;

    public void BattleTest()
    {
        //CallBattle(0, "Battle");
        
        GameManager.instance.completedQuests.Add(GetQuest(0));
        GameManager.instance.activeQuests.Add(GetQuest(1));
    }
}
