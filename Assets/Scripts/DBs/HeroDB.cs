using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroDB : MonoBehaviour
{
    public List<BaseHero> heroes = new List<BaseHero>();

    //LEVELING BASES
    public int[] levelEXPThresholds;

    public BaseHero GetHeroByID(int ID)
    {
        foreach (BaseHero hero in heroes)
        {
            if (hero.ID == ID)
            {
                return hero;
            }
        }
        return null;
    }
}
