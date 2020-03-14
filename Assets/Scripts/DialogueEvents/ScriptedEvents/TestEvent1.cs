using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent1 : BaseScriptedEvent
{
    public void TestMethod()
    {
        //StartCoroutine(StartMoving());
        //Debug.Log("TestEvent1");
    }

    IEnumerator StartMoving()
    {
        yield return (StartCoroutine(MoveRight(this.gameObject, .5f, 1)));
    }
}
