using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MagicMenuMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void ShowBorder()
    {
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().alpha = 1;
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().interactable = true;
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void HideBorder()
    {
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().alpha = 0;
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().interactable = false;
        gameObject.transform.Find("Border").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideBorder();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowBorder();
    }
}
