using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : BaseScriptedEvent
{
    public AudioClip voice;

    public void TestMethod()
    {
        StartCoroutine(ShowDialogueChoices(
            "WHATCHU WANT??", voice,
            "Give 100,000 golds", GiveGold,
            "Give 300 gold pls", GiveSmallGold,
            "Fully heal everyone", HealAll,
            "Nothing", DoNothing
            ));
    }

    void GiveGold()
    {
        Debug.Log("Giving 100k gold");
        ChangeGold(100000);
    }

    void HealAll()
    {
        Debug.Log("Healing everyone");
        FullHeal();
    }

    void GiveSmallGold()
    {
        Debug.Log("Giving 300 gold");
        ChangeGold(300);
    }
    
    void DoNothing()
    {

    }
}
