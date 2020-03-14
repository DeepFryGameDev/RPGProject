using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour //contains all items in active inventory
{

    #region Singleton
    public static Inventory instance; //call instance to get the single active inventory for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void OnItemchanged();

    public OnItemchanged onItemChangedCallback;

    public List<Item> items = new List<Item>(); //active items

    public void Add (Item item) //adds item to inventory
    {
        items.Add(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public void Remove (Item item) //removes item from inventory
    {
        items.Reverse();
        items.Remove(item);
        items.Reverse();

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
    
}
