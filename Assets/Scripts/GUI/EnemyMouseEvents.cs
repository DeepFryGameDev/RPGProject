using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    BattleStateMachine BSM; //for the active battle state manager
    public Text detailsText;

    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        detailsText = GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
    }

    void Update()
    {
        if (BSM.cancelledEnemySelect)
        {
            this.gameObject.GetComponent<EnemyStateMachine>().Selector.SetActive(false); //removes selector from being active if ChooseTarget is cancelled
        }
    }

    public void OnPointerClick(PointerEventData eventData) //to set the target
    {
        if (BSM.choosingTarget)
        {
            BSM.chosenTarget = this.gameObject; //sets BSM target to this gameObject
            this.gameObject.GetComponent<EnemyStateMachine>().Selector.SetActive(false); //hides selector after being chosen
            detailsText.GetComponent<Text>().text = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //shows selector
    {
        if (BSM.choosingTarget)
        {
            this.gameObject.GetComponent<EnemyStateMachine>().Selector.SetActive(true);
        }
        detailsText.GetComponent<Text>().text = this.gameObject.GetComponent<EnemyStateMachine>().enemy.name;
    }

    public void OnPointerExit(PointerEventData eventData) //hides selector
    {
        Debug.Log("should hide selector");

        if (BSM.choosingTarget)
        {
            this.gameObject.GetComponent<EnemyStateMachine>().Selector.SetActive(false);
        }
        detailsText.GetComponent<Text>().text = "";
    }
}
