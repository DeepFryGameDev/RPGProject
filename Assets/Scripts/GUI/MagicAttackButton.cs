using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackButton : MonoBehaviour
{
    public BaseAttack magicAttackToPerform;

    public void CastMagicAttack() //used by battle state machine to perform casting magic attack
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().SetChosenMagic(magicAttackToPerform);
    }
}
