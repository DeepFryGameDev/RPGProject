using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1Spell : BaseAttack
{
    public Fire1Spell()
    {
        name = "Fire 1";
        description = "Basic fireball which burns a lil bit";
        damage = 20;
        MPCost = 10;
        type = Type.MAGIC;
    }
}
