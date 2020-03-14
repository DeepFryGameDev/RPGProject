using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBattleMenuButton : MonoBehaviour
{
    public Item itemToUse;

    public void UseItem() //used by battle state machine to perform using an item
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().SetChosenItem(itemToUse);
    }
}
