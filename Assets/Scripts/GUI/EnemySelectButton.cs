using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectButton : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Text detailsText;

    public void Start()
    {
        detailsText = GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>();
    }

    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().EnemySelection(EnemyPrefab); //Save input of enemy selection to enemy prefab
    }

    public void HideSelector() //hides selector cursor over enemy
    {
            EnemyPrefab.transform.Find("Selector").gameObject.SetActive(false);
    }

    public void ShowSelector() //shows selector cursor over enemy
    {
            EnemyPrefab.transform.Find("Selector").gameObject.SetActive(true);
    }

    public void HideDetails() //hides selector cursor over enemy
    {
        detailsText.text = "";
    }

    public void ShowDetails() //shows selector cursor over enemy
    {
        string enemyName = this.gameObject.GetComponentInChildren<Text>().text;
        foreach (GameObject enemy in GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().EnemiesInBattle)
        {
            if (enemyName == enemy.GetComponent<EnemyStateMachine>().enemy._Name)
            {
                detailsText.GetComponent<Text>().text = enemyName;
                break;
            }
        }
    }
}
