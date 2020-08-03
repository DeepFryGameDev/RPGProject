using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioManager : MonoBehaviour
{
    public AudioClip confirmSE;
    public AudioClip backSE;
    public AudioClip healSE;
    public AudioClip cantActionSE;
    public AudioClip equipSE; //not the correct equip SE but ok for now
    public AudioClip openMenuSE;

    public AudioClip battleTransition;

    public AudioClip turnReady;
    public AudioClip magicCast;
    public AudioClip attackMiss;
    public AudioClip enemyDeath;

    public AudioClip fire1;
    public AudioClip fire1AE;
    public AudioClip bio1;
    public AudioClip bio1AE;
    public AudioClip cure1;
    public AudioClip cure1AE;

    public AudioClip slash;
    public AudioClip slashAE;
    
    #region Singleton
    public static AudioManager instance; //call instance to get the single active inventory for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    public void PlaySE(AudioClip audio)
    {
        GameObject.Find("GameManager/BGS").GetComponent<AudioSource>().PlayOneShot(audio);
    }

}
