using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Used for combat interaction between mouse cursor and enemy unit game objects

    BattleStateMachine BSM; //for the active battle state manager
    public Text detailsText;

    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        detailsText = GameObject.Find("BattleCanvas/BattleUI/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
    }

    /// <summary>
    /// Sets chosen target for BSM
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (BSM.choosingTarget)
        {
            GameObject.Find("GameManager/BGS").GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.confirmSE);
            BSM.chosenTarget = gameObject; //sets BSM target to this gameObject
            detailsText.GetComponent<Text>().text = "";
        }
    }

    /// <summary>
    /// Displays selector
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        detailsText.GetComponent<Text>().text = this.gameObject.GetComponent<EnemyStateMachine>().enemy.name;
    }

    /// <summary>
    /// Hides selector
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        detailsText.GetComponent<Text>().text = "";
    }
}
