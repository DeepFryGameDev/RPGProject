using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuScript : MonoBehaviour
{
    [MenuItem("Dev Tools/Dialogue Canvas/Display")] //displays dialogue canvas in Unity editor
    public static void DisplayDialogueCanvas()
    {
        GameObject.Find("DialogueCanvas").GetComponent<CanvasGroup>().alpha = 1;
    }

    [MenuItem("Dev Tools/Dialogue Canvas/Hide")] //hides dialogue canvas in Unity editor
    public static void HideDialogueCanvas()
    {
        GameObject.Find("DialogueCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    //for Menu
    [MenuItem("Dev Tools/Menu Canvas/Display Main Menu")] //displays menu canvas in Unity editor
    public static void DisplayMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Item Menu")] //displays item menu canvas in Unity editor
    public static void DisplayItemMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Magic Menu")] //displays magic menu canvas in Unity editor
    public static void DisplayMagicMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Equip Menu")] //displays equip menu canvas in Unity editor
    public static void DisplayEquipMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Status Menu")] //displays status menu canvas in Unity editor
    public static void DisplayStatusMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Party Menu")] //displays party menu canvas in Unity editor
    public static void DisplayPartyMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Order Menu")] //displays order menu canvas in Unity editor
    public static void DisplayOrderMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Grid Menu")] //displays grid menu canvas in Unity editor
    public static void DisplayGridMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Config Menu")] //displays config menu canvas in Unity editor
    public static void DisplayConfigMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }

    [MenuItem("Dev Tools/Menu Canvas/Display Quit Menu")] //displays quit menu canvas in Unity editor
    public static void DisplayQuitMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 1;
    }

    [MenuItem("Dev Tools/Menu Canvas/Hide Menus")] //hides menu canvases in Unity editor
    public static void HideMenuCanvas()
    {
        GameObject.Find("MainMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ItemMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("MagicMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("EquipMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("StatusMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("PartyMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("OrderMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GridMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ConfigMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("QuitMenuCanvas").GetComponent<CanvasGroup>().alpha = 0;
    }


    //for Troops
    [MenuItem("Dev Tools/List Troops")] //lists out each troop with index for easy identifying
    public static void ListTroops()
    {
        List<BaseTroop> troops = GameObject.Find("GameManager").GetComponent<GameManager>().troops;
        Debug.Log("-----GameManager Troops-----");
        for (int i = 0; i < troops.Count; i++)
        {
            Debug.Log(i + ") " + troops[i]._Name);
        }
        Debug.Log("-----End GameManager Troops-----");
    }
}
