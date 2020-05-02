using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStuff : BaseScriptedEvent
{
    public Item testItem;
    public Item testItem2;

    public Equipment testHelmet1;
    public Equipment testHelmet2;
    public Equipment testChest1;
    public Equipment testChest2;
    public Equipment testWrists1;
    public Equipment testWrists2;
    public Equipment testLegs1;
    public Equipment testLegs2;
    public Equipment testFeet1;
    public Equipment testFeet2;
    public Equipment testRelic1;
    public Equipment testRelic2;
    public Equipment testLeftHand1;
    public Equipment testLeftHand2;
    public Equipment testRightHand1;
    public Equipment testRightHand2;
    public Equipment testEquip1;
    public Equipment testEquip2;
    public Equipment testEquip3;

    public void TestMethod()
    {
        //if (!otherEventRunning)
        //{
        //StartCoroutine(DoTest());
        //}
        //StartCoroutine(DoTest());

        /*AddItem(testItem);
        AddItem(testItem);
        AddItem(testItem);
        AddItem(testItem);
        AddItem(testItem2);
        AddItem(testItem2);
        AddItem(testItem2);
        AddItem(testItem2);
        AddItem(testItem2);

        AddItem(testHelmet1);
        AddItem(testHelmet2);
        AddItem(testChest1);
        AddItem(testChest2);
        AddItem(testWrists1);
        AddItem(testWrists2);
        AddItem(testLegs1);
        AddItem(testLegs2);
        AddItem(testFeet1);
        AddItem(testFeet2);
        AddItem(testRelic1);
        AddItem(testRelic2);
        AddItem(testLeftHand1);
        AddItem(testLeftHand2);
        AddItem(testRightHand1);
        AddItem(testRightHand2);
        AddItem(testEquip1);
        AddItem(testEquip2);
        AddItem(testEquip3);*/


        StartCoroutine(ShowDialogueChoices(
            "WHATCHU WANT??",
            "Start Gather Quest", QuestTest1,
            "Start Kill Quest", QuestTest2,
            "Start Bool Quest", QuestTest3,
            "Nothing", DoNothing
            ));


        //OpenMenu();
    }

    public void QuestTest1()
    {
        BaseQuest questToStart = GameObject.Find("GameManager/QuestDB").GetComponent<QuestDB>().quests[0];

        StartCoroutine(ShowMessage("Go forth and bring me 5 potions!", baseTextSpeed, true, true));

        Debug.Log("Quest started!");
        StartQuest(questToStart);
    }

    public void QuestTest2()
    {
        BaseQuest questToStart = GameObject.Find("GameManager/QuestDB").GetComponent<QuestDB>().quests[1];

        StartCoroutine(ShowMessage("Go forth and kill 1 test enemy!", baseTextSpeed, true, true));

        Debug.Log("Quest started!");
        StartQuest(questToStart);
    }

    public void QuestTest3()
    {
        BaseQuest questToStart = GameObject.Find("GameManager/QuestDB").GetComponent<QuestDB>().quests[2];

        StartCoroutine(ShowMessage("Go talk to that guy over there!", baseTextSpeed, true, true));

        Debug.Log("Quest started!");
        StartQuest(questToStart);
    }

    void RenameCharacters()
    {
        StartCoroutine(ChangeHeroNames());
    }

    IEnumerator ChangeHeroNames()
    {
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            Debug.Log("Changing " + hero._Name);
            yield return StartCoroutine(NameInput(hero));
        }
    }

    void TestTextInput()
    {
        StartCoroutine(DoTextInput());
    }

    IEnumerator DoTextInput()
    {
        yield return StartCoroutine(TextInput());
        Debug.Log(GameManager.instance.textInput);
    }

    void TestNumberInput()
    {
        StartCoroutine(DoNumberInput());
    }

    IEnumerator DoNumberInput()
    {
        yield return StartCoroutine(NumberInput());
        Debug.Log(GameManager.instance.numberInput);
    }

    IEnumerator DoTest()
    {
        for (int i = 0; i < 300; i++)
        {
            yield return MoveRandom(thisGameObject, baseMoveSpeed, 1);
            //yield return MoveRight(thisGameObject, baseMoveSpeed, 1);
        }
    }

    void DoNothing()
    {

    }
}
