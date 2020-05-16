using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent1 : BaseScriptedEvent
{
    public GameObject enemy;

    public void BattleTest()
    {
        //CallBattle(0, "Battle");

        //GameManager.instance.completedQuests.Add(GetQuest(0));
        //GameManager.instance.activeQuests.Add(GetQuest(1));

        AddItem(0); //2 potions
        AddItem(0);
        AddItem(1); //5 ethers
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(2); //1 key item
        AddEquipment(0);
        AddEquipment(1);
        AddEquipment(2);
        AddEquipment(3);
        AddEquipment(4);
        AddEquipment(5);
        AddEquipment(6);
        AddEquipment(7);
    }
}
