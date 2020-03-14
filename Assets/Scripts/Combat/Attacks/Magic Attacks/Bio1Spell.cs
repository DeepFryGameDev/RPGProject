using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bio1Spell : BaseAttack
{
    public Bio1Spell()
    {
        name = "Bio 1";
        description = "Basic Poison attack";
        damage = 5;
        MPCost = 5;
        type = Type.MAGIC;
    }
}
