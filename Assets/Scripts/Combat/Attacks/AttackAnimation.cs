using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    public GameObject target;
    public BaseAttack attack;

    public float attackDur;

    GameObject attackAnim;
    GameObject addedEffectAnim;

    List<ParticleSystem> playingAnims = new List<ParticleSystem>();
    List<BaseAttackAudio> attackSoundEffects = new List<BaseAttackAudio>();
    List<BaseAttackAudio> addedEffectSoundEffects = new List<BaseAttackAudio>();

    public bool addedEffectAchieved;
    bool buttonPressed;
    bool enemyAttemptedAE;

    public int enemyAEChance;

    int frame = 0;
    int frameAEThresholdMin = 0;
    int frameAEThresholdMax = 0;
    int AEframe = 0;
    bool inAnimation;

    public void Update()
    {
        if (inAnimation)
        {
            frame++;
            //Debug.Log("Attack frame: " + frame);

            if (GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().PerformList[0].attackerType == Types.HERO)
            {
                if (ButtonPressedDuringAnimation(frame))
                {
                    Debug.Log("Button pressed on frame " + frame);
                }
            } else
            {
                if (!enemyAttemptedAE)
                {
                    EnemyAttackAddedEffect(frame);
                }
            }

            PlayAttackSE(frame, attackSoundEffects);
        }

        if (addedEffectAchieved)
        {
            AEframe++;
            //Debug.Log("AE frame: " + AEframe);

            PlayAttackSE(AEframe, addedEffectSoundEffects);
        }
    }

    /// <summary>
    /// Generates new animation GameObject, and builds specific animation based on attack on the new GameObject
    /// </summary>
    public void BuildAnimation()
    {
        attackAnim = new GameObject
        {
            tag = "AttackAnimation",

            name = attack.name + " animation"
        };

        //Physical attacks
        if (attack.name == "Slash")
            Slash();

        //Magic attacks
        if (attack.name == "Fire 1")
            Fire1();

        if (attack.name == "Bio 1")
            Bio1();

        if (attack.name == "Cure 1")
            Cure1();
    }

    /// <summary>
    /// Coroutine.  Plays the animation and begins 'inAnimation' process in Update method for checking if user gets added effect
    /// </summary>
    public IEnumerator PlayAnimation()
    {
        Debug.Log("beginning animation");

        foreach (Transform child in attackAnim.transform)
        {
            child.GetComponent<ParticleSystem>().Play();
            playingAnims.Add(child.GetComponent<ParticleSystem>());
        }

        foreach (ParticleSystem ps in playingAnims)
        {
            if (!ps.isPlaying)
            {
                yield return new WaitForSeconds(.01f);
            }
        }

        playingAnims.Clear();

        inAnimation = true;

        Debug.Log("duration: " + attackDur);

        yield return new WaitForSeconds(attackDur);

        Debug.Log("ending animation");

        attackDur = 0;
        frame = 0;
        AEframe = 0;

        enemyAEChance = 0;

        frameAEThresholdMin = 0;
        frameAEThresholdMax = 0;

        inAnimation = false;
        buttonPressed = false;
        enemyAttemptedAE = false;

        attackSoundEffects.Clear();
        addedEffectSoundEffects.Clear();
    }

    /// <summary>
    /// Plays animation for added effect for attack
    /// </summary>
    void PlayAddedEffectAnimation()
    {
        addedEffectAnim = new GameObject
        {
            tag = "AttackAnimation",

            name = attack.name + " added effect animation"
        };

        if (attack.name == "Fire 1")
        {
            Fire1AddedEffect();
        }

        if (attack.name == "Bio 1")
        {
            Bio1AddedEffect();
        }

        if (attack.name == "Cure 1")
        {
            Cure1AddedEffect();
        }

        if (attack.name == "Slash")
        {
            SlashAddedEffect();
        }

        foreach (Transform child in addedEffectAnim.transform)
        {
            child.GetComponent<ParticleSystem>().Play();
        }
    }

    #region -----Physical Attack Animations-----

    void Slash()
    {
        //Animation
        GameObject piece1 = Instantiate(AttackPrefabManager.Instance.slash, attackAnim.transform);
        attackAnim.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
        piece1.transform.localScale = new Vector3(1f, 1f);
        piece1.GetComponent<Renderer>().sortingLayerName = "Foreground";

        attackDur = piece1.GetComponent<ParticleSystem>().main.duration;

        //Audio
        BaseAttackAudio se = new BaseAttackAudio
        {
            frame = 1,
            SE = AudioManager.instance.slash
        };
        attackSoundEffects.Add(se);

        //Added effect threshold
        frameAEThresholdMin = 10;
        frameAEThresholdMax = 40;
    }

    void SlashAddedEffect()
    {
        //Animation
        GameObject piece1 = Instantiate(AttackPrefabManager.Instance.slash, addedEffectAnim.transform);
        addedEffectAnim.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
        piece1.transform.localScale = new Vector3(1f, 1f);
        piece1.transform.rotation = new Quaternion(180f, 180f, 1f, 1f);
        piece1.GetComponent<Renderer>().sortingLayerName = "Foreground";

        //Audio
        BaseAttackAudio se = new BaseAttackAudio
        {
            frame = 1,
            SE = AudioManager.instance.slashAE
        };
        addedEffectSoundEffects.Add(se);
    }

    #endregion

    #region -----Magic Attack Animations-----

    public void PlayCastingAnimation(GameObject caster)
    {
        //Animation
        GameObject castAnimation = Instantiate(AttackPrefabManager.Instance.magicCast, caster.transform);
        castAnimation.name = "Casting animation";
        castAnimation.transform.position = new Vector3(caster.transform.position.x, caster.transform.position.y, 0f);
        castAnimation.transform.localScale = new Vector3(3f, 3f);
        castAnimation.GetComponent<Renderer>().sortingLayerName = "Foreground";

        AudioManager.instance.PlaySE(AudioManager.instance.magicCast);
    }

    void Fire1()
    {
        //Animation
        GameObject piece1 = Instantiate(AttackPrefabManager.Instance.fire, attackAnim.transform);
        attackAnim.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
        piece1.transform.localScale = new Vector3(3f, 3f);
        piece1.GetComponent<Renderer>().sortingLayerName = "Foreground";

        attackDur = piece1.GetComponent<ParticleSystem>().main.duration;

        //Audio
        BaseAttackAudio se = new BaseAttackAudio
        {
            frame = 1,
            SE = AudioManager.instance.fire1
        };
        attackSoundEffects.Add(se);

        //Added effect threshold
        frameAEThresholdMin = 10;
        frameAEThresholdMax = 40;
    }

    void Fire1AddedEffect()
    {
        //Animation
        GameObject piece1 = Instantiate(AttackPrefabManager.Instance.fire, addedEffectAnim.transform);
        addedEffectAnim.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
        piece1.transform.localScale = new Vector3(8f, 8f);
        piece1.GetComponent<Renderer>().sortingLayerName = "Foreground";

        //Audio
        BaseAttackAudio se = new BaseAttackAudio
        {
            frame = 1,
            SE = AudioManager.instance.fire1AE
        };
        addedEffectSoundEffects.Add(se);
    }

    void Bio1()
    {
        //Animation
        GameObject piece1 = Instantiate(AttackPrefabManager.Instance.bio, attackAnim.transform);
        attackAnim.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
        piece1.transform.localScale = new Vector3(3f, 3f);
        piece1.GetComponent<Renderer>().sortingLayerName = "Foreground";

        attackDur = piece1.GetComponent<ParticleSystem>().main.duration;

        //Audio
        BaseAttackAudio se = new BaseAttackAudio
        {
            frame = 1,
            SE = AudioManager.instance.bio1
        };
        attackSoundEffects.Add(se);

        //Added effect threshold
        frameAEThresholdMin = 10;
        frameAEThresholdMax = 40;
    }

    void Bio1AddedEffect()
    {

    }

    void Cure1()
    {
        //Animation
        GameObject piece1 = Instantiate(AttackPrefabManager.Instance.cure, attackAnim.transform);
        attackAnim.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, 0f);
        piece1.transform.localScale = new Vector3(3f, 3f);
        piece1.GetComponent<Renderer>().sortingLayerName = "Foreground";

        attackDur = piece1.GetComponent<ParticleSystem>().main.duration;

        //Audio
        BaseAttackAudio se = new BaseAttackAudio
        {
            frame = 1,
            SE = AudioManager.instance.cure1
        };
        attackSoundEffects.Add(se);

        //Added effect threshold
        frameAEThresholdMin = 10;
        frameAEThresholdMax = 40;
    }

    void Cure1AddedEffect()
    {

    }

    #endregion

    //Tools for above methods

    /// <summary>
    /// Adds given GameObject to the attack animation object
    /// </summary>
    /// <param name="prefab">GameObject to be added to the attack animation</param>
    void AddPrefabToAnimation(GameObject prefab)
    {
        GameObject prefabToAdd = new GameObject();
        prefabToAdd.transform.SetParent(attackAnim.transform);
    }

    /// <summary>
    /// Returns true if button is pressed during animation, and checks if it was pressed during correct frame threshold
    /// </summary>
    /// <param name="frame">Frame the button was pressed on</param>
    bool ButtonPressedDuringAnimation(int frame)
    {
        if (Input.GetButtonDown("Confirm") && !buttonPressed)
        {
            buttonPressed = true;

            if (frame >= frameAEThresholdMin && frame <= frameAEThresholdMax)
            {
                Debug.Log("Added effect!");
                addedEffectAchieved = true;
                PlayAddedEffectAnimation();
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    void EnemyAttackAddedEffect(int frame)
    {
        if (frame >= frameAEThresholdMin && frame <= frameAEThresholdMax)
        {
            int rand = Random.Range(1, 100);

            if (rand <= enemyAEChance)
            {
                Debug.Log("Enemy added effect!");
                addedEffectAchieved = true;
                PlayAddedEffectAnimation();
            }
            enemyAttemptedAE = true;
        }
    }

    /// <summary>
    /// Plays sound effect at correct frame configured in attack build
    /// </summary>
    /// <param name="frame">Frame to play the sound effect</param>
    /// <param name="BAAs">List of BaseAttackAudio to check</param>
    void PlayAttackSE(int frame, List<BaseAttackAudio> BAAs)
    {
        foreach (BaseAttackAudio BAA in BAAs)
        {
            if (BAA.frame == frame)
            {
                GameObject.Find("GameManager/BGS").GetComponent<AudioSource>().PlayOneShot(BAA.SE);
            }
        }
    }
}
