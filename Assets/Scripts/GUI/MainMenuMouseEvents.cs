using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().choosingHero)
        {
            HideBorder();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().choosingHero)
        {
            ShowBorder();
        }
    }
}
