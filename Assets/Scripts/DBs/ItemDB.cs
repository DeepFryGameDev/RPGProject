using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDB : MonoBehaviour
{
    public List<BaseItem> items = new List<BaseItem>();

    #region Singleton
    public static ItemDB instance; //call instance to get the single active ItemDB for the game

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

    public BaseItem GetItem(int ID)
    {
        return items[ID];
    }
}
