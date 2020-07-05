using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn
{
    public string Attacker; //name of attacker

    public Types attackerType;
    public Types targetType;
    public GameObject AttackersGameObject; //GameObject of attacker
    public GameObject AttackersTarget; //Who will be attacked
    
    public BaseAttack chosenAttack; //which attack is performed
    public Item chosenItem; //which attack is performed

    public ActionType actionType; //to determine how turn should be processed when applying damage/effect

    List<BaseStatusEffect> statusEffects = new List<BaseStatusEffect>(); //any status effects to be stored in the turn
}
