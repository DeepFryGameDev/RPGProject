using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentDB : MonoBehaviour
{
    public List<BaseEquipment> equipment = new List<BaseEquipment>();

    #region Singleton
    public static EquipmentDB instance; //call instance to get the single active EquipmentDB for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of EquipmentDB found!");
            return;
        }

        instance = this;
    }
    #endregion
}
