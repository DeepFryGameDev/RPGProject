using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float crossfadeTransitionTime = .5f;
    public Animator crossfadeTransition;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
    }

    IEnumerator SceneTransition(string sceneName)
    {
        crossfadeTransition.SetTrigger("Start");        

        yield return new WaitForSeconds(crossfadeTransitionTime);

        SceneManager.LoadScene(sceneName);
    }

}
