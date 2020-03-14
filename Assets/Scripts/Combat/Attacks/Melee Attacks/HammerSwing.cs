using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSwing : BaseAttack
{
    public HammerSwing()
    {
        name = "Hammer Swing";
        description = "powerful hammer swing";
        damage = 20;
        MPCost = 0;
        type = Type.PHYSICAL;
    }
}
