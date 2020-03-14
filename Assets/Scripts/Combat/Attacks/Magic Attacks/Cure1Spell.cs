using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure1Spell : BaseAttack
{
    public Cure1Spell()
    {
        name = "Cure 1";
        description = "Basic heal";
        damage = 20;
        MPCost = 5;
        type = Type.MAGIC;
        magicClass = MagicClass.WHITE;
    }
}