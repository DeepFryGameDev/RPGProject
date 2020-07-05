using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackButton : MonoBehaviour
{
    //Facilitates sets magic attack to battle state machine's chosen magic attack when panel object is clicked
    public BaseAttack magicAttackToPerform;

    /// <summary>
    /// Used by battle state machine to perform casting magic attack
    /// </summary>
    public void CastMagicAttack()
    {
        //Debug.Log("chose magic " + magicAttackToPerform.name);
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().SetChosenMagic(magicAttackToPerform);
    }
}
