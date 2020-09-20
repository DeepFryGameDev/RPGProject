using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDB : MonoBehaviour
{
    public List<BaseAttack> attacks = new List<BaseAttack>();

    #region Singleton
    public static AttackDB instance; //call instance to get the single active AttackDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of ItemDB found!");
            return;
        }

        instance = this;
    }
    #endregion

    public BaseAttack GetAttack(int ID)
    {
        return attacks[ID];
    }
}
