using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBattleMenuButton : MonoBehaviour
{
    //Facilitates interaction with mouse cursor on items instantiated in item menu in battle
    public Item itemToUse;

    /// <summary>
    /// Used by battle state machine to perform using an item
    /// </summary>
    public void UseItem()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().SetChosenItem(itemToUse);
    }
}
