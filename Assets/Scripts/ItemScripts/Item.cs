using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")] //to create scriptable object for items. used to display in menus
public class Item : ScriptableObject
{
    public Sprite icon = null;
    new public string name = "New Item";
    public string description = "Item Description";
    public bool usableInMenu;

    public enum UseStates
    {
        HERO,
        ALLHEROES,
        ENEMY,
        ALLENEMIES,
    }

    public UseStates useState;

    public void RemoveFromInventory()
    {
        Inventory.instance.items.Reverse();
        Inventory.instance.items.Remove(this);
        Inventory.instance.items.Reverse();
    }
}
