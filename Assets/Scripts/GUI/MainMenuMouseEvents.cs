using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Facilitates mouse cursor interaction with main menu panel objects

    /// <summary>
    /// Displays border around menu panel object
    /// </summary>
    public void ShowBorder()
    {
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().alpha = 1;
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().interactable = true;
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// Hides border around menu panel object
    /// </summary>
    public void HideBorder()
    {
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().alpha = 0;
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().interactable = false;
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// If hero is being chosen for main menu, hides the border around hero panel object
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().choosingHero)
        {
            HideBorder();
        }
    }

    /// <summary>
    /// If hero is being chosen for main menu, shows border around hero panel object
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().choosingHero)
        {
            ShowBorder();
        }
    }
}
