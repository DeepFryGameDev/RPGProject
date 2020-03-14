using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BaseAttack
{
    public Slash()
    {
        name = "Slash";
        description = "standard sword slash";
        damage = 10;
        MPCost = 0;
        type  = Type.PHYSICAL;
    }
}
