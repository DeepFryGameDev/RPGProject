using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MagicMenuMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Facilitates mouse cursor interaction with magic menu panel objects

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
    /// Hides border when mouse cursor exits the menu object
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        HideBorder();
    }

    /// <summary>
    /// Displays border when mouse cursor enters menu object
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowBorder();
    }
}
