using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopDB : MonoBehaviour
{
    public List<BaseTroop> troops = new List<BaseTroop>();

    #region Singleton
    public static TroopDB instance; //call instance to get the single active TroopDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of TroopDB found!");
            return;
        }

        instance = this;
    }
    #endregion
}
