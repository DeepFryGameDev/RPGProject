using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent1 : BaseScriptedEvent
{
    public GameObject enemy;

    public AudioClip voice;

    public void BattleTest()
    {
        //GameManager.instance.completedQuests.Add(GetQuest(0));
        //GameManager.instance.activeQuests.Add(GetQuest(1));

        /*AddItem(0, 2);
        AddItem(1, 5);
        AddItem(2); //key item
        AddEquipment(0);
        AddEquipment(1);
        AddEquipment(2);
        AddEquipment(3);
        AddEquipment(4);
        AddEquipment(5);
        AddEquipment(6);
        AddEquipment(7);

        StartCoroutine(ShowMessage("Here you go dude!", voice, true, true));*/

        CallBattle(0, "Battle");
    }
}
