using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    //new public string name = "New Equipment";
    //public string description = "Equipment Description";
    //public Sprite icon = null;

    public EquipmentSlot equipmentSlot;

    public List<int> equippableByPlayers = new List<int>();

    public int Strength;
    public int Stamina;
    public int Agility;
    public int Dexterity;
    public int Intelligence;
    public int Spirit;

    public int ATK;
    public int DEF;

    public int MATK;
    public int MDEF;

    public int threat;

    public int hit;
    public int crit;

    public int move;
    public int regen;

    public int dodge;
    public int parry;
    public int block;
}

public enum EquipmentSlot { HEAD, CHEST, WRISTS, LEGS, FEET, RELIC, RIGHTHAND, LEFTHAND }
