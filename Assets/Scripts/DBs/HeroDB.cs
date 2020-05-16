using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroDB : MonoBehaviour
{
    public List<BaseHero> heroes = new List<BaseHero>();

    //LEVELING BASES
    public int[] levelEXPThresholds;

    #region Singleton
    public static HeroDB instance; //call instance to get the single active HeroDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of HeroDB found!");
            return;
        }

        instance = this;
    }
    #endregion

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
