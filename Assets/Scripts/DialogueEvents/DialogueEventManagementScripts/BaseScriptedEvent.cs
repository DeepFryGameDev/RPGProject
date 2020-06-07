using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RichTextSubstringHelper;

public class BaseScriptedEvent : MonoBehaviour
{
    public string method; //name of the method to be run

    [System.NonSerialized]public float baseTextSpeed = 0.030225f; //textSpeed in DialogueEvent (update this if text speed is updated there)

    [System.NonSerialized] public float baseMoveSpeed = 0.5f; //base move speed for movement methods

    [System.NonSerialized] public float collisionDistance = 1;

    [System.NonSerialized] public Transform thisTransform; //transform of the game object this script is attached to
    [System.NonSerialized] public GameObject thisGameObject; //game object that this script is attached to
    [System.NonSerialized] public Transform playerTransform; //transform of the player game object
    [System.NonSerialized] public GameObject playerGameObject; //game object of the player
    [System.NonSerialized] public GameManager gameManager; //the game manager

    [System.NonSerialized] public bool otherEventRunning = false;
    [System.NonSerialized] public bool inMenu = false;

    //For Dialog Choice
    public delegate void RunDialogueChoice();
    GameObject DialogueChoicePanel;
    Button Choice1Button;
    Button Choice2Button;
    Button Choice3Button;
    Button Choice4Button;
    string choiceMade;

    bool confirmPressed;
    bool checkForConfirmPressed;

    AudioSource audioSource;
    AudioClip confirmSE;

    AudioSource voiceAudioSource;

    //Enums
    public enum MenuButtons
    {
        Item,
        Magic,
        Equip,
        Status,
        Talents,
        Party,
        Grid,
        Quests,
        Bestiary
    }
    public MenuButtons menuButton;

    private void Start()
    {
        thisGameObject = this.gameObject; //sets thisGameObject to game object this script is attached to
        thisTransform = this.gameObject.transform; //sets thisTransform to transform of game object this script is attached to
        playerGameObject = GameObject.Find("Player"); //sets playerGameObject to gameobject of player
        playerTransform = playerGameObject.transform; //sets playerTransform to transform of gameobject of player
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); //sets gameManager to the game manager object in scene

        audioSource = GameObject.Find("GameManager/DialogueCanvas").GetComponent<AudioSource>();
        confirmSE = Resources.Load<AudioClip>("Sounds/000 - Cursor Move");
        voiceAudioSource = GameObject.Find("GameManager/DialogueCanvas/VoiceAudio").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (checkForConfirmPressed) //Keeps confirm button considered 'pressed' once when player presses it
        {
            CheckConfirmButtonStatus();
        }        
    }

    //DIFFERENT FUNCTIONS THAT CAN BE RUN BY ANY EVENT SCRIPT

    public IEnumerator MoveTest (GameObject GO, float timeToMove, float spacesToMove) //for testing
    {
        Rigidbody2D rb = GO.GetComponent<Rigidbody2D>();

        float t = 0;
        Vector2 start = transform.position;
        Vector2 target = new Vector2(transform.position.x, transform.position.y + 1); 

        while (t <= 1)
        {
            t += Time.fixedDeltaTime / baseMoveSpeed;
            rb.MovePosition(Vector2.Lerp(start, target, t));

            yield return new WaitForFixedUpdate();
        }
    }

    #region ---MOVEMENT---

    /// <summary>
    /// Coroutine.  Moves given GameObject to the left.  FacingState script is required to be attached to GameObject as well as rigidbody. Further changes needed.
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveLeft(GameObject obj, float timeToMove, float spacesToMove) //moves object left
    {
        FaceLeft(obj);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>(); //uses rigidbody as transform.move was causing collision issues

        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        if (obj == playerGameObject)
        {
            DisablePlayerMovement();
        }

        Vector2 currentPos = transform.position;
        Vector2 position = new Vector2(currentPos.x - spacesToMove, currentPos.y);
        float t = 0f;
        while (t < 1)
        {
            if (!inMenu)
            {
                t += Time.deltaTime / timeToMove;
                rb.MovePosition(Vector2.Lerp(currentPos, position, t));
                yield return new WaitForFixedUpdate();
            } else
            {
                yield return null;
            }
        }

        if (obj == playerGameObject)
        {
            EnablePlayerMovement();
        }       
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject to the right.  FacingState script is required to be attached to GameObject as well as rigidbody. Further changes needed.
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveRight(GameObject obj, float timeToMove, float spacesToMove) //moves object right
    {
        FaceRight(obj);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        
        
        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        if (obj == playerGameObject)
        {
            DisablePlayerMovement();
        }

        Vector2 currentPos = transform.position;
        Vector2 position = new Vector2(currentPos.x + spacesToMove, currentPos.y);
        float t = 0f;
        while (t < 1)
        {
            if (!inMenu)
            {
                t += Time.deltaTime / timeToMove;
                rb.MovePosition(Vector2.Lerp(currentPos, position, t));
                yield return new WaitForFixedUpdate();
            } else
            {
                yield return null;
            }
        }

        if (obj == playerGameObject)
        {
            EnablePlayerMovement();
        }
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject upward.  FacingState script is required to be attached to GameObject as well as rigidbody. Further changes needed.
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveUp(GameObject obj, float timeToMove, float spacesToMove)
    {
        FaceUp(obj);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        if (obj == playerGameObject)
        {
            DisablePlayerMovement();
        }

        Vector2 currentPos = transform.position;
        Vector2 position = new Vector2(currentPos.x, currentPos.y + spacesToMove);
        float t = 0f;
        while (t < 1)
        {
            if (!inMenu)
            {
                t += Time.deltaTime / timeToMove;
                rb.MovePosition(Vector2.Lerp(currentPos, position, t));
                yield return new WaitForFixedUpdate();
            } else
            {
                yield return null;
            }
        }

        if (obj == playerGameObject)
        {
            EnablePlayerMovement();
        }
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject downward.  FacingState script is required to be attached to GameObject as well as rigidbody. Further changes needed.
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveDown(GameObject obj, float timeToMove, float spacesToMove)
    {
        FaceDown(obj);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        if (obj == playerGameObject)
        {
            DisablePlayerMovement();
        }

        Vector2 currentPos = transform.position;
        Vector2 position = new Vector2(currentPos.x, currentPos.y - spacesToMove);
        float t = 0f;
        while (t < 1)
        {
            if (!inMenu)
            {
                t += Time.deltaTime / timeToMove;
                rb.MovePosition(Vector2.Lerp(currentPos, position, t));
                yield return new WaitForFixedUpdate();
            } else
            {
                yield return null;
            }
        }

        if (obj == playerGameObject)
        {
            EnablePlayerMovement();
        }
    }

    /// <summary>
    /// Coroutine.  Moves object diagonally left and up.  Rigidbody is required.  Further changes needed (pathfinding to target, etc to include direction facing)
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveLeftUp(GameObject obj, float timeToMove, int spacesToMove)
    {
        Transform transform = obj.transform;
        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }
        if (transform == playerTransform)
        {
            DisablePlayerMovement();
        }
        Vector2 currentPos = transform.position;
        Vector2 position = new Vector2(currentPos.x - spacesToMove, currentPos.y + spacesToMove);
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        if (transform == playerTransform)
        {
            EnablePlayerMovement();
        }
    }

    /// <summary>
    /// Coroutine.  Moves object diagonally right and up.  Rigidbody is required.  Further changes needed (pathfinding to target, etc to include direction facing)
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveRightUp(GameObject obj, float timeToMove, int spacesToMove)
    {
        Transform transform = obj.transform;
        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }
        if (transform == playerTransform)
        {
            DisablePlayerMovement();
        }
        Vector2 currentPos = transform.position;
        Vector2 position = new Vector2(currentPos.x + spacesToMove, currentPos.y + spacesToMove);
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        if (transform == playerTransform)
        {
            EnablePlayerMovement();
        }
    }

    /// <summary>
    /// Coroutine.  Moves object diagonally left and down.  Rigidbody is required.  Further changes needed (pathfinding to target, etc to include direction facing)
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveLeftDown(GameObject obj, float timeToMove, int spacesToMove)
    {
        Transform transform = obj.transform;
        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }
        if (transform == playerTransform)
        {
            DisablePlayerMovement();
        }
        Vector2 currentPos = transform.position;
        Vector2 position = new Vector2(currentPos.x - spacesToMove, currentPos.y - spacesToMove);
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        if (transform == playerTransform)
        {
            EnablePlayerMovement();
        }
    }

    /// <summary>
    /// Coroutine.  Moves object diagonally right and down.  Rigidbody is required.  Further changes needed (pathfinding to target, etc to include direction facing)
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveRightDown(GameObject obj, float timeToMove, int spacesToMove)
    {
        Transform transform = obj.transform;
        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }
        if (transform == playerTransform)
        {
            DisablePlayerMovement();
        }
        Vector2 currentPos = transform.position;
        Vector2 position = new Vector2(currentPos.x + spacesToMove, currentPos.y - spacesToMove);
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        if (transform == playerTransform)
        {
            EnablePlayerMovement();
        }
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject in to target position.  Rigidbody is required.  Further changes needed (pathfinding to target, etc to include direction facing)
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="target">Target coordinates to move target to</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    public IEnumerator MoveToTarget(GameObject obj, Vector2 target, float timeToMove)
    {
        Transform transform = obj.transform;
        if (transform == playerTransform)
        {
            DisablePlayerMovement();
        }
        Vector2 currentPos = transform.position;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, target, t);
            yield return null;
        }
        if (transform == playerTransform)
        {
            EnablePlayerMovement();
        }
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject in random direction by choosing random value 0-3.  FacingState script is required to be attached to GameObject as well as rigidbody
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveRandom(GameObject obj, float timeToMove, int spacesToMove)
    {
        Transform transform = obj.transform;

        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        Random.InitState(System.DateTime.Now.Millisecond);
        int randomDirection = Random.Range(0, 4);

        if (randomDirection == 0)
        {
            yield return MoveLeft(obj, timeToMove, spacesToMove); //left
        }
        if (randomDirection == 1)
        {
            yield return MoveRight(obj, timeToMove, spacesToMove); //right
        }
        if (randomDirection == 2)
        {

            yield return MoveDown(obj, timeToMove, spacesToMove); //down
        }
        if (randomDirection == 3)
        {
            yield return MoveUp(obj, timeToMove, spacesToMove); //up
        }
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject toward player.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveTowardPlayer(GameObject obj, float timeToMove, int spacesToMove)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(thisTransform.position, 100, Vector2.one); //radius may need to be adjusted if player isn't being picked up

        string dimToUse = "";

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (Mathf.Abs(hit.point.x) > Mathf.Abs(hit.point.y))
                {
                    dimToUse = "x";
                }
                else
                {
                    dimToUse = "y";
                }

                if (dimToUse == "x")
                {
                    if (hit.point.x < 0) //player is left of object
                    {
                        FaceLeft(obj);
                        yield return MoveLeft(obj, timeToMove, spacesToMove);
                    }
                    else //player is right of object
                    {
                        FaceRight(obj);
                        yield return MoveRight(obj, timeToMove, spacesToMove);
                    }
                }
                else if (dimToUse == "y")
                {
                    if (hit.point.y < 0) //player is below object
                    {
                        FaceDown(obj);
                        yield return MoveDown(obj, timeToMove, spacesToMove);
                    }
                    else //player is above object
                    {
                        FaceUp(obj);
                        yield return MoveUp(obj, timeToMove, spacesToMove);
                    }
                }
                else
                {
                    Debug.Log("MoveTowardPlayer - dimToUse not found!");
                }
            }
        }
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject away from the player.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator MoveAwayFromPlayer(GameObject obj, float timeToMove, int spacesToMove)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(thisTransform.position, 100, Vector2.one); //radius may need to be adjusted if player isn't being picked up

        string dimToUse = "";

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (Mathf.Abs(hit.point.x) > Mathf.Abs(hit.point.y))
                {
                    dimToUse = "x";
                }
                else
                {
                    dimToUse = "y";
                }

                if (dimToUse == "x")
                {
                    if (hit.point.x < 0) //player is left of object
                    {
                        FaceRight(obj);
                        yield return MoveRight(obj, timeToMove, spacesToMove);
                    }
                    else //player is right of object
                    {
                        FaceLeft(obj);
                        yield return MoveLeft(obj, timeToMove, spacesToMove);
                    }
                }
                else if (dimToUse == "y")
                {
                    if (hit.point.y < 0) //player is below object
                    {
                        FaceUp(obj);
                        yield return MoveUp(obj, timeToMove, spacesToMove);
                    }
                    else //player is above object
                    {
                        FaceDown(obj);
                        yield return MoveDown(obj, timeToMove, spacesToMove);
                    }
                }
                else
                {
                    Debug.Log("MoveTowardPlayer - dimToUse not found!");
                }
            }
        }
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject in direction they are facing.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator StepForward(GameObject obj, float timeToMove, int spacesToMove)
    {
        if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            yield return MoveUp(obj, timeToMove, spacesToMove);
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            yield return MoveDown(obj, timeToMove, spacesToMove);
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            yield return MoveLeft(obj, timeToMove, spacesToMove);
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            yield return MoveRight(obj, timeToMove, spacesToMove);
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = obj.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                yield return MoveUp(obj, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                yield return MoveDown(obj, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                yield return MoveLeft(obj, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                yield return MoveRight(obj, timeToMove, spacesToMove);
            }
        }
    }

    /// <summary>
    /// Coroutine.  Moves given GameObject backwards from direction they are currently facing.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to move</param>
    /// <param name="timeToMove">How quickly to move the object (smaller value is quicker, can use "baseMoveSpeed")</param>
    /// <param name="spacesToMove">How many spaces/tiles to move</param>
    public IEnumerator StepBackward(GameObject obj, float timeToMove, int spacesToMove)
    {
        if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            yield return MoveDown(obj, timeToMove, spacesToMove);
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            yield return MoveUp(obj, timeToMove, spacesToMove);
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            yield return MoveRight(obj, timeToMove, spacesToMove);
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            yield return MoveLeft(obj, timeToMove, spacesToMove);
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = obj.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                yield return MoveDown(obj, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                yield return MoveUp(obj, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                yield return MoveRight(obj, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                yield return MoveLeft(obj, timeToMove, spacesToMove);
            }
        }
    }

    /// <summary>
    /// Faces given GameObject downward.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void FaceDown(GameObject obj)
    {
        obj.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
    }

    /// <summary>
    /// Faces given GameObject to the left.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void FaceLeft(GameObject obj)
    {
        obj.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
    }

    /// <summary>
    /// Faces given GameObject to the right.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void FaceRight(GameObject obj)
    {
        obj.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
    }

    /// <summary>
    /// Faces given GameObject upward.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void FaceUp(GameObject obj)
    {
        obj.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
    }

    /// <summary>
    /// Faces given GameObject to their default direction.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void FaceDefault(GameObject obj)
    {
        obj.GetComponent<FacingState>().faceState = FacingState.FaceState.DEFAULT;
    }

    /// <summary>
    /// Faces given GameObject toward the player.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void FaceTowardPlayer(GameObject obj)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(thisTransform.position, 100, Vector2.one); //radius may need to be adjusted if player isn't being picked up

        string dimToUse = "";

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (Mathf.Abs(hit.point.x) > Mathf.Abs(hit.point.y))
                {
                    dimToUse = "x";
                } else
                {
                    dimToUse = "y";
                }

                if (dimToUse == "x")
                {
                    if (hit.point.x < 0) //player is left of object
                    {
                        FaceLeft(obj);
                    } else //player is right of object
                    {
                        FaceRight(obj);
                    }
                }
                else if (dimToUse == "y")
                {
                    if (hit.point.y < 0) //player is below object
                    {
                        FaceDown(obj);
                    }
                    else //player is above object
                    {
                        FaceUp(obj);
                    }
                } else
                {
                    Debug.Log("FaceTowardPlayer - dimToUse not found!");
                }
            }
        }
    }

    /// <summary>
    /// Faces given GameObject away from the player.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void FaceAwayFromPlayer(GameObject obj)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(thisTransform.position, 100, Vector2.one); //radius may need to be adjusted if player isn't being picked up

        string dimToUse = "";

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (Mathf.Abs(hit.point.x) > Mathf.Abs(hit.point.y))
                {
                    dimToUse = "x";
                }
                else
                {
                    dimToUse = "y";
                }

                if (dimToUse == "x")
                {
                    if (hit.point.x < 0) //player is left of object
                    {
                        FaceRight(obj);
                    }
                    else //player is right of object
                    {
                        FaceLeft(obj);
                    }
                }
                else if (dimToUse == "y")
                {
                    if (hit.point.y < 0) //player is below object
                    {
                        FaceUp(obj);
                    }
                    else //player is above object
                    {
                        FaceDown(obj);
                    }
                }
                else
                {
                    Debug.Log("FaceAwayFromPlayer - dimToUse not found!");
                }
            }
        }
    }

    /// <summary>
    /// Faces given GameObject 90 degrees to the right.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void Face90Right (GameObject obj)
    {
        if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
        } else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
        } else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
        } else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
        } else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = obj.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
            }
        }
    }

    /// <summary>
    /// Faces given GameObject 90 degrees to the left.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void Face90Left (GameObject obj)
    {
        
        if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
        } else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
        } else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
        } else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
        } else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = obj.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
            }
        }
    }

    /// <summary>
    /// Faces given GameObject 180 degrees (faces them around to opposite direction).  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void Face180 (GameObject obj)
    {
        if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            obj.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
        }
        else if (obj.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = obj.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                obj.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
            }
        }
    }

    /// <summary>
    /// Randomly chooses left or right, and faces given GameObject 90 degrees to that direction.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void Face90RightOrLeft(GameObject obj)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int randDir = Random.Range(0, 2);
        if (randDir == 0)
        {
            Face90Right(obj);
        } else if (randDir == 1)
        {
            Face90Left(obj);
        }
    }

    /// <summary>
    /// Faces given GameObject in random direction by picking random value 0-3.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to change facing direction</param>
    public void FaceRandom(GameObject obj)
    {
        bool sameDir = true;
        FacingState fs = obj.GetComponent<FacingState>();
        while (sameDir == true)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int randDir = Random.Range(0, 4);
            if (randDir == 0)
            {
                if (fs.faceState != FacingState.FaceState.UP || (fs.faceState != FacingState.FaceState.DEFAULT && (fs.defaultDirection == "Up" || fs.defaultDirection == "up")))
                {
                    sameDir = false;
                    FaceUp(obj);
                }
            }
            else if (randDir == 1)
            {
                if (fs.faceState != FacingState.FaceState.DOWN || (fs.faceState != FacingState.FaceState.DEFAULT && (fs.defaultDirection == "Down" || fs.defaultDirection == "down")))
                {
                    sameDir = false;
                    FaceDown(obj);
                }
            }
            else if (randDir == 2)
            {
                if (fs.faceState != FacingState.FaceState.LEFT || (fs.faceState != FacingState.FaceState.DEFAULT && (fs.defaultDirection == "Left" || fs.defaultDirection == "left")))
                {
                    sameDir = false;
                    FaceLeft(obj);
                }
            }
            else if (randDir == 3)
            {
                if (fs.faceState != FacingState.FaceState.RIGHT || (fs.faceState != FacingState.FaceState.DEFAULT && (fs.defaultDirection == "Right" || fs.defaultDirection == "right")))
                {
                    sameDir = false;
                    FaceRight(obj);
                }
            }
        }
    }

    /// <summary>
    /// Changes default move speed for player
    /// </summary>
    /// <param name="newMoveSpeed">Move speed to be set (higher = faster)</param>
    public void ChangeDefaultMoveSpeed(float newMoveSpeed)
    {
        playerGameObject.GetComponent<PlayerController2D>().moveSpeed = newMoveSpeed;
    }

    /// <summary>
    /// Turns player walking animation on
    /// </summary>
    public void EnableWalkingAnimation()
    {
        playerGameObject.GetComponent<Animator>().SetBool("isMoving", true); 
    }

    /// <summary>
    /// Turns player walking animation off
    /// </summary>
    public void DisableWalkingAnimation()
    {
        playerGameObject.GetComponent<Animator>().SetBool("isMoving", false); 
    }

    /// <summary>
    /// Keeps given GameObject from being able to change facing direction.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to keep from being able to change facing direction</param>
    public void EnableForceDirection(GameObject obj)
    {
        obj.GetComponent<FacingState>().forceDirection = true;
    }

    /// <summary>
    /// Allows given GameObject to be able to change facing direction.  FacingState script is required to be attached to GameObject
    /// </summary>
    /// <param name="obj">GameObject to allow changing facing direction</param>
    public void DisableForceDirection(GameObject obj)
    {
        obj.GetComponent<FacingState>().forceDirection = false;
    }

    /// <summary>
    /// Disables player's movement
    /// </summary>
    public void DisablePlayerMovement()
    {
        playerGameObject.GetComponent<PlayerController2D>().enabled = false;
    }

    /// <summary>
    /// Enables player's movement
    /// </summary>
    public void EnablePlayerMovement()
    {
        playerGameObject.GetComponent<PlayerController2D>().enabled = true;
    }

    /// <summary>
    /// Saves the position of player for loadPosition method (Not yet implemented)
    /// </summary>
    /// <param name="objectToSave">GameObject to save position</param>
    public void SavePosition(GameObject objectToSave)
    {
        BasePositionSave thePosition = new BasePositionSave();
        thePosition.name = objectToSave.name;
        thePosition.newPosition = gameObject.transform.position;
        thePosition.newDirection = "Test"; //not yet implemented
        thePosition.Scene = SceneManager.GetActiveScene().name;
        GameManager.instance.positionSaves.Add(thePosition);
        Debug.Log("Position saved: " + thePosition.name);
    }

    #endregion

    #region ---BATTLE MANAGEMENT---

    /// <summary>
    /// Changes encounter chance from given encounter region
    /// </summary>
    /// <param name="troopIndex">Index of troop in TroopDB</param>
    /// <param name="scene">Name of scene to open</param>
    public void CallBattle(int troopIndex, string scene)
    {
        GameManager.instance.GetBattleFromScript(troopIndex, scene);
    }

    /// <summary>
    /// Changes global battle frequency (The greater the difference, the longer it will take to enter battle)
    /// </summary>
    /// <param name="minBattleChance">Minimum value to check</param>
    /// <param name="maxBattleChance">Maximum value to check</param>
    public void ChangeBattleFrequency(int minBattleChance, int maxBattleChance)
    {
        GameManager.instance.battleChance = minBattleChance;
        GameManager.instance.maxBattleChance = maxBattleChance;
    }

    /// <summary>
    /// Changes encounter chance from given encounter region
    /// </summary>
    /// <param name="region">GameObject which the region is held</param>
    /// <param name="index">Index of the encounter</param>
    /// <param name="newEncounterChance">New encounter chance to be set (0-100)</param>
    public void ChangeEncounterChanceFromRegion(GameObject region, int index, float newEncounterChance)
    {
        region.GetComponent<RegionData>().troopEncounters[index].encounterChance = newEncounterChance;
    }

    #endregion

    #region ---SCENE MANAGEMENT---

    //void TransitionToScene

    /// <summary>
    /// Opens Item or Equipment shop
    /// </summary>
    /// <param name="type">Use "Item" or "Equip"</param>
    public void OpenShop(string type)
    {
        if (type == "Item")
        {
            ItemShop itemShop = GetComponent<ItemShop>();
            GameManager.instance.itemShopList = itemShop.itemShopList;
            itemShop.ShowItemListInBuyGUI();
            itemShop.DisplayItemShopGUI();
        }

        if (type == "Equip")
        {
            EquipShop equipShop = GetComponent<EquipShop>();
            GameManager.instance.equipShopList = equipShop.equipShopList;
            equipShop.ShowEquipListInBuyGUI();
            equipShop.DisplayEquipShopGUI();
        }
        DisablePlayerMovement();
    }

    /// <summary>
    /// Forces menu to be opened
    /// </summary>
    public void OpenMenu()
    {
        GameObject.Find("GameManager").GetComponent<GameMenu>().menuCalled = true;
    }

    //void OpenSave

    //void GameOver

    //void ReturnToTitle

    #endregion

    #region ---GAME MANAGEMENT---

    /// <summary>
    /// Changes switch value (not yet implemented)
    /// </summary>
    /// <param name="whichObject">GameObject with the switch</param>
    /// <param name="whichEvent">Switch's event index</param>
    /// <param name="whichSwitch">The switch to be changed</param>
    /// <param name="whichBool">To change the switch to true or false</param>
    public void ChangeSwitch(GameObject whichObject, int whichEvent, int whichSwitch, bool whichBool)
    {
        BaseDialogueEvent e = whichObject.GetComponent<DialogueEvents>().eventOrDialogue[whichEvent];

        if (whichSwitch == 1)
        {
            e.switch1 = whichBool;
        } else if (whichSwitch == 2)
        {
            e.switch2 = whichBool;
        }
    }

    /// <summary>
    /// Returns if switch is true or false on given object (not yet fully implemented)
    /// </summary>
    /// <param name="whichObject">GameObject with the switch</param>
    /// <param name="whichEvent">Switch's event index</param>
    /// <param name="whichSwitch">The switch to be returned</param>
    public bool GetSwitchBool(GameObject whichObject, int whichEvent, int whichSwitch)
    {
        BaseDialogueEvent e = whichObject.GetComponent<DialogueEvents>().eventOrDialogue[whichEvent];

        if (whichSwitch == 1)
        {
            return e.switch1;
        } else if (whichSwitch == 2)
        {
            return e.switch2;
        } else
        {
            Debug.Log("GetSwitchBool - invalid switch: " + whichSwitch);
            return false;
        }
    }

    /// <summary>
    /// Changes value of global event bools
    /// </summary>
    /// <param name="index">Index of the global bool</param>
    /// <param name="boolean">Change bool to true or false</param>
    public void ChangeGlobalBool(int index, bool boolean)
    {
       GlobalBoolsDB.instance.globalBools[index] = boolean;
    }

    #endregion

    #region ---MUSIC/SOUNDS---

    /// <summary>
    /// Plays given sound effect once
    /// </summary>
    /// <param name="SE">Sound effect to play</param>
    void PlaySE(AudioClip SE)
    {
        audioSource.PlayOneShot(SE);
    }

    //void PlayBGM

    //ienumerator FadeOutBGM

    //ienumerator FadeInBGM

    //void PlayBGS

    //ienumerator FadeOutBGS

    //void StopBGM

    //void StopSE

    #endregion

    #region ---TIMING---

    /// <summary>
    /// Halt processing for given seconds
    /// </summary>
    /// <param name="waitTime">Time to move in seconds</param>
    public IEnumerator WaitForSeconds(float waitTime) //pause for period of time
    {
        yield return new WaitForSeconds(waitTime);
    }

    #endregion

    #region ---DIALOGUE---
    
    /// <summary>
    /// Coroutine.  Displays given message via dialogue
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="voiceTone">Voice tone to be played during message</param>
    /// <param name="textSpeed">Speed to display text</param>
    /// <param name="waitForEnd">Halt other processing until message is completed</param>
    /// <param name="lockPlayerMovement">Disable player movement until message is completed</param>
    public IEnumerator ShowMessage(string message, AudioClip voiceTone, float textSpeed, bool waitForEnd, bool lockPlayerMovement)
    {
        checkForConfirmPressed = true;
        if (!confirmPressed)
        {
            if (lockPlayerMovement)
            {
                Debug.Log("Disabling player movement");
                DisablePlayerMovement();
            }
            if (waitForEnd)
            {
                yield return (StartCoroutine(WriteToMessagePanel(message, textSpeed, voiceTone)));
            }
            else
            {
                StartCoroutine(WriteToMessagePanel(message, textSpeed, voiceTone));
            }
            if (lockPlayerMovement)
            {
                Debug.Log("Enabling player movement");
                EnablePlayerMovement();
            }
            //CheckConfirmButtonStatus();
        }
    }

    /// <summary>
    /// Coroutine.  Displays given message via dialogue
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="voiceTone">Voice Tone for message</param>
    /// <param name="waitForEnd">Halt other processing until message is completed</param>
    /// <param name="lockPlayerMovement">Disable player movement until message is completed</param>
    public IEnumerator ShowMessage(string message, AudioClip voiceTone, bool waitForEnd, bool lockPlayerMovement)
    {
        checkForConfirmPressed = true;
        if (!confirmPressed)
        {
            if (lockPlayerMovement)
            {
                Debug.Log("Disabling player movement");
                DisablePlayerMovement();
            }
            if (waitForEnd)
            {
                yield return (StartCoroutine(WriteToMessagePanel(message, baseTextSpeed, voiceTone)));
            }
            else
            {
                StartCoroutine(WriteToMessagePanel(message, baseTextSpeed, voiceTone));
            }
            if (lockPlayerMovement)
            {
                Debug.Log("Enabling player movement");
                EnablePlayerMovement();
            }
            //CheckConfirmButtonStatus();
        }
    }

    /// <summary>
    /// Coroutine.  Displays given message via dialogue and displays choices for user input that can be assigned by any method.  Minimum 2 options, others can be set with 'null'
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="voiceTone">Voice Tone for message</param>
    /// <param name="textSpeed">Speed to display text.  Can use "baseTextSpeed"</param>
    /// <param name="button1Text">Text for choice 1</param>
    /// <param name="choice1">Name of method to assign for choice 1</param>
    /// <param name="button2Text">Text for choice 2</param>
    /// <param name="choice2">Name of method to assign for choice 2</param>
    /// <param name="button3Text">Text for choice 3.  Use 'null' if there is no 3rd option</param>
    /// <param name="choice3">Name of method to assign for choice 3.  Use 'null' if there is no 3rd option</param>
    /// <param name="button4Text">Text for choice 4. Use 'null' if there is no 4th option</param>
    /// <param name="choice4">Name of method to assign for choice 4.  Use 'null' if there is no 4th option</param>
    public IEnumerator ShowDialogueChoices(string message, AudioClip voiceTone, float textSpeed, string button1Text, RunDialogueChoice choice1, string button2Text, RunDialogueChoice choice2, string button3Text, RunDialogueChoice choice3, string button4Text, RunDialogueChoice choice4)
    {
        checkForConfirmPressed = true;
        if (!confirmPressed)
        {
            if (voiceTone != null)
            {
                PlayVoice(voiceTone);
            }

            DisablePlayerMovement();
            Text messageText = GameManager.instance.DialogueCanvas.GetComponentInChildren<Text>(); //assigns messageText to dialogue canvas text component
            string text = message; //gets the text to be put in dialogue UI

            GameManager.instance.DisplayPanel(true); //shows dialogue panel

            for (int i = 0; i < text.RichTextLength(); i++) //for each letter in the text
            {
                CheckConfirmButtonStatus(); //checks if confirm button is pressed to be able to skip message (not yet implemented)
                                            //fullText += text[i]; //adds current letter to fullText
                                            //messageText.text = fullText; //sets dialogue UI text to the current fullText
                                            //fullText += text.RichTextSubString(i);
                messageText.text = text.RichTextSubString(i + 1);
                yield return new WaitForSecondsRealtime(textSpeed); //waits before entering next letter, based on textSpeed
            }

            choiceMade = "";

            if (voiceTone != null)
            {
                StopVoice();
            }

            SetChoiceButtons(button1Text, button2Text, button3Text, button4Text);

            yield return new WaitUntil(() => choiceMade != "");

            DialogueChoicePanel.GetComponent<CanvasGroup>().alpha = 0;
            GameManager.instance.DisplayPanel(false); //hides dialogue panel

            EnablePlayerMovement();

            if (choiceMade == "button1")
            {
                choice1();
            }
            else if (choiceMade == "button2")
            {
                choice2();
            }
            else if (choiceMade == "button3")
            {
                choice3();
            }
            else if (choiceMade == "button4")
            {
                choice4();
            }
        }
    }

    /// <summary>
    /// Coroutine.  Displays given message via dialogue and displays choices for user input that can be assigned by any method.  Minimum 2 options, others can be set with 'null'
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="voiceTone">Voice Tone for message</param>
    /// <param name="button1Text">Text for choice 1</param>
    /// <param name="choice1">Name of method to assign for choice 1</param>
    /// <param name="button2Text">Text for choice 2</param>
    /// <param name="choice2">Name of method to assign for choice 2</param>
    /// <param name="button3Text">Text for choice 3.  Use 'null' if there is no 3rd option</param>
    /// <param name="choice3">Name of method to assign for choice 3.  Use 'null' if there is no 3rd option</param>
    /// <param name="button4Text">Text for choice 4. Use 'null' if there is no 4th option</param>
    /// <param name="choice4">Name of method to assign for choice 4.  Use 'null' if there is no 4th option</param>
    public IEnumerator ShowDialogueChoices(string message, AudioClip voiceTone, string button1Text, RunDialogueChoice choice1, string button2Text, RunDialogueChoice choice2, string button3Text, RunDialogueChoice choice3, string button4Text, RunDialogueChoice choice4)
    {
        checkForConfirmPressed = true;
        if (!confirmPressed)
        {
            if (voiceTone != null)
            {
                PlayVoice(voiceTone);
            }

            DisablePlayerMovement();
            Text messageText = GameManager.instance.DialogueCanvas.GetComponentInChildren<Text>(); //assigns messageText to dialogue canvas text component
            string text = message; //gets the text to be put in dialogue UI

            GameManager.instance.DisplayPanel(true); //shows dialogue panel

            for (int i = 0; i < text.RichTextLength(); i++) //for each letter in the text
            {
                CheckConfirmButtonStatus(); //checks if confirm button is pressed to be able to skip message (not yet implemented)
                                            //fullText += text[i]; //adds current letter to fullText
                                            //messageText.text = fullText; //sets dialogue UI text to the current fullText
                                            //fullText += text.RichTextSubString(i);
                messageText.text = text.RichTextSubString(i + 1);
                yield return new WaitForSecondsRealtime(baseTextSpeed); //waits before entering next letter, based on textSpeed
            }

            choiceMade = "";

            if (voiceTone != null)
            {
                StopVoice();
            }

            SetChoiceButtons(button1Text, button2Text, button3Text, button4Text);

            yield return new WaitUntil(() => choiceMade != "");

            DialogueChoicePanel.GetComponent<CanvasGroup>().alpha = 0;
            GameManager.instance.DisplayPanel(false); //hides dialogue panel

            EnablePlayerMovement();

            if (choiceMade == "button1")
            {
                choice1();
            }
            else if (choiceMade == "button2")
            {
                choice2();
            }
            else if (choiceMade == "button3")
            {
                choice3();
            }
            else if (choiceMade == "button4")
            {
                choice4();
            }
        }
    }

    /// <summary>
    /// Coroutine.  Input a number with the number input GUI that is set to Gamemanager.instance.numberInput
    /// </summary>
    public IEnumerator NumberInput()
    {
        DisablePlayerMovement();
        DisplayInputPanel("Number");
        GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel/NumberEnteredPanel/NumberEnteredText").GetComponent<Text>().text = "0";

        GameManager.instance.numberInput = 0;

        while (GameManager.instance.numberInput == 0)
        {
            yield return null;
        }

        EnablePlayerMovement();
        HideInputPanels();
    }

    /// <summary>
    /// Coroutine.  Input text with the text input GUI that is set to GameManager.instance.textInput
    /// </summary>
    public IEnumerator TextInput()
    {
        DisablePlayerMovement();
        DisplayInputPanel("Text");
        GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/TextEnteredPanel/TextEnteredText").GetComponent<Text>().text = "";

        GameManager.instance.textInput = "";

        TextResetCaps();

        while (GameManager.instance.textInput == "")
        {
            yield return null;
        }

        EnablePlayerMovement();
        HideInputPanels();
    }

    #endregion

    #region ---QUESTS---

    /// <summary>
    /// Adds given quest to active quests list
    /// </summary>
    /// <param name="ID">ID of quest in QuestDB</param>
    public void StartQuest(int ID)
    {
        QuestDB.instance.AddToActiveQuests(QuestDB.instance.quests[ID]);
    }

    /// <summary>
    /// Returns quest.fulfilled for given quest
    /// </summary>
    /// <param name="quest">Quest to check</param>
    public bool QuestObjectivesFulfilled(BaseQuest quest)
    {
        QuestDB.instance.UpdateQuestObjectives();

        if (quest.fulfilled)
        {
            return true;
        } else
        {
            return false;
        }
    }

    /// <summary>
    /// Marks given quest as complete
    /// </summary>
    /// <param name="ID">Quest to check</param>
    public void CompleteQuest(BaseQuest quest)
    {
        QuestDB.instance.CompleteQuest(quest);
    }

    /// <summary>
    /// Marks bool of given quest as given value if quest type is 'bool'
    /// </summary>
    /// <param name="quest">Quest to check</param>
    /// <param name="index">Index of bool in quest</param>
    /// <param name="value">To mark the bool as true or false</param>
    public void MarkQuestBool(BaseQuest quest, int index, bool value)
    {
        quest.boolReqs[index].objectiveFulfilled = value;
    }

    /// <summary>
    /// Returns given quest and index bool value
    /// </summary>
    /// <param name="quest">Quest to check</param>
    /// <param name="index">Index of bool in quest</param>
    public bool QuestBool(BaseQuest quest, int index)
    {
        return quest.boolReqs[index].objectiveFulfilled;
    }

    /// <summary>
    /// Returns Quest by ID   	
    /// </summary>
    /// <param name="type">Use DB, Active, or Complete</param>
    /// <param name="ID">ID of quest</param>
    public BaseQuest GetQuestByID(string type, int ID)
    {
        if (type == "DB")
        {
            foreach (BaseQuest quest in QuestDB.instance.quests)
            {
                if (quest.ID == ID)
                {
                    return quest;
                }
            }
        }

        if (type == "Active")
        {
            foreach (BaseQuest quest in GameManager.instance.activeQuests)
            {
                if (quest.ID == ID)
                {
                    return quest;
                }
            }
        }

        if (type == "Complete")
        {
            foreach (BaseQuest quest in GameManager.instance.completedQuests)
            {
                if (quest.ID == ID)
                {
                    return quest;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Returns given quest by ID from QuestDB
    /// </summary>
    /// <param name="ID">ID of quest in QuestDB</param>
    public BaseQuest GetQuest(int ID)
    {
        foreach (BaseQuest quest in QuestDB.instance.quests)
        {
            if (quest.ID == ID)
            {
                return quest;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns given quest by ID from active quests
    /// </summary>
    /// <param name="ID">ID of quest in active quests</param>
    public BaseQuest GetActiveQuest(int ID)
    {
        foreach (BaseQuest quest in GameManager.instance.activeQuests)
        {
            if (quest.ID == ID)
            {
                return quest;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns if given quest is in active quest list
    /// </summary>
    /// <param name="quest">Quest to check</param>
    public bool IfQuestIsActive(BaseQuest quest)
    {
        foreach (BaseQuest activeQuest in GameManager.instance.activeQuests)
        {
            if (activeQuest == quest)
            {
                return true;
            } else
            {
                return false;
            }
        }
        return false;
    }

    #endregion

    #region ---SYSTEM SETTINGS---

    //void ChangeBattleBGM

    /// <summary>
    /// Enables or disables ability to open menu
    /// </summary>
    /// <param name="canOpen">If true, menu can be opened</param>
    public void ChangeMenuAccess(bool canOpen)
    {
        if (canOpen)
        {
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = false;
        } else
        {
            GameObject.Find("GameManager/Menus").GetComponent<GameMenu>().disableMenu = true;
        }
    }

    /// <summary>
    /// Enables or disables ability to access menu buttons (Items, Magic, etc)
    /// </summary>
    /// <param name="button">Button to change access</param>
    /// <param name="canAccess">If true, button can be accessed</param>
    public void ChangeMenuButtonAccess(MenuButtons button, bool canAccess)
    {
        Button buttonToChange = GameObject.Find("GameManager/Menus/MainMenuCanvas/MenuButtonsPanel/" + button.ToString() + "Button").GetComponent<Button>();
        
        if (canAccess)
        {
            buttonToChange.interactable = true;
            buttonToChange.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);
        }

        if (!canAccess)
        {
            buttonToChange.interactable = false;
            buttonToChange.GetComponentInChildren<Text>().color = new Color(1, 1, 1, .5f);
        }
    }

    #endregion

    #region ---SPRITES---

    //void ChangeGraphic

    //void ChangeOpacity

    //void AddSprite

    //void RemoveSprite

    #endregion

    #region ---ACTORS---

    /// <summary>
    /// Fully restores HP and MP of all active heroes
    /// </summary>
    public void FullHeal()
    {
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            hero.curHP = hero.finalMaxHP;
            hero.curMP = hero.finalMaxMP;
        }
    }

    /// <summary>
    /// Adds or subtracts current HP of given hero
    /// </summary>
    /// <param name="hero">Hero to modify HP</param>
    /// <param name="hp">CurrentHP to add/subtract (subtract with negative value)</param>
    public void ChangeHP(BaseHero hero, int hp)
    {
        hero.curHP += hp;

        if (hero.curHP > hero.finalMaxHP)
        {
            hero.curHP = hero.finalMaxHP;
        }
    }

    /// <summary>
    /// Sets current HP or given hero to given value
    /// </summary>
    /// <param name="hero">Hero to modify HP</param>
    /// <param name="hp">HP value to set current HP</param>
    public void SetHP(BaseHero hero, int hp)
    {
        hero.curHP = hp;

        if (hero.curHP > hero.finalMaxHP)
        {
            hero.curHP = hero.finalMaxHP;
        }
    }

    /// <summary>
    /// Adds or subtracts current MP of given hero
    /// </summary>
    /// <param name="hero">Hero to modify MP</param>
    /// <param name="mp">MP value to set current MP</param>
    public void ChangeMP(BaseHero hero, int mp)
    {
        hero.curMP += mp;

        if (hero.curMP > hero.finalMaxMP)
        {
            hero.curMP = hero.finalMaxMP;
        }
    }

    /// <summary>
    /// Sets current MP of given hero to given value
    /// </summary>
    /// <param name="hero">Hero to modify MP</param>
    /// <param name="mp">MP value to set current MP</param>
    public void SetMP(BaseHero hero, int mp)
    {
        hero.curMP = mp;

        if (hero.curMP > hero.finalMaxMP)
        {
            hero.curMP = hero.finalMaxMP;
        }
    }

    /// <summary>
    /// Adds or subtracts current EXP of given hero (de-leveling not yet supported)
    /// </summary>
    public void ChangeEXP(BaseHero hero, int exp)
    {
        hero.currentExp += exp;

        GameManager.instance.ProcessExp();
    }

    /// <summary>
    /// Adds or subtracts given base stat by given value to given hero
    /// </summary>
    /// <param name="hero">Hero to modify parameter</param>
    /// <param name="parameter">Use: "Strength", "Stamina", "Agility", "Dexterity", "Intelligence" or "Spirit"</param>
    /// <param name="paramChange">Value to be added/subtracted</param>
    public void ChangeParameter(BaseHero hero, string parameter, int paramChange)
    {
        if (parameter == "Strength")
        {
            hero.baseSTR += paramChange;
        }
        if (parameter == "Stamina")
        {
            hero.baseSTA += paramChange;
        }
        if (parameter == "Agility")
        {
            hero.baseAGI += paramChange;
        }
        if (parameter == "Dexterity")
        {
            hero.baseDEX += paramChange;
        }
        if (parameter == "Intelligence")
        {
            hero.baseINT += paramChange;
        }
        if (parameter == "Spirit")
        {
            hero.baseSPI += paramChange;
        }

        hero.UpdateStats();
    }

    //void AddSkill

    //void RemoveSkill

    /// <summary>
    /// Equip given equipment for given hero
    /// </summary>
    /// <param name="hero">Hero to change equipment</param>
    /// <param name="equipName">Name of the equipment to be equipped</param>
    public void ChangeEquipment(BaseHero hero, string equipName)
    {
        bool equipFound = false;

        foreach (Item equipment in Inventory.instance.items)
        {
            if (equipment.name == equipName)
            {
                hero.Equip((Equipment)equipment);
                equipFound = true;
                break;
            }
        }

        if (!equipFound)
        {
            Debug.Log("Equipment not in inventory");
        }
    }

    /// <summary>
    /// Change name of given hero with given string
    /// </summary>
    /// <param name="hero">Hero to modify name</param>
    /// <param name="name">New name of the given hero</param>
    public void ChangeName(BaseHero hero, string name)
    {
        hero.name = name;
    }

    /// <summary>
    /// Change name of given hero with name input GUI
    /// </summary>
    /// <param name="hero">Hero to modify name</param>
    public IEnumerator NameInput(BaseHero hero)
    {
        DisablePlayerMovement();
        DisplayInputPanel("Name");
        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text = hero.name;

        GameManager.instance.nameInput = "";

        NameResetCaps();

        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/FacePanel").GetComponent<Image>().sprite = hero.faceImage;

        while (GameManager.instance.nameInput == "")
        {
            yield return null;
        }

        hero.name = GameManager.instance.nameInput;

        EnablePlayerMovement();
        HideInputPanels();
    }

    #endregion

    #region ---PARTY---

    /// <summary>
    /// Adds or subtracts given gold
    /// </summary>
    /// <param name="gold">Number of gold to be added/subtracted</param>
    public void ChangeGold(int gold)
    {
        GameManager.instance.gold += gold;
    }

    /// <summary>
    /// Adds given item to inventory
    /// </summary>
    /// <param name="ID">ID of item from ItemDB to add</param>
    /// <param name="numberToAdd">Number of the given item to add to inventory</param>
    public void AddItem(int ID, int numberToAdd)
    {
        for (int i=0; i < numberToAdd; i++)
        {
            Inventory.instance.Add(ItemDB.instance.items[ID].item);
            Debug.Log("Added to inventory: " + ItemDB.instance.items[ID].item.name);
        }
    }

    /// <summary>
    /// Adds 1 of given item to inventory
    /// </summary>
    /// <param name="ID">ID of item from ItemDB to add</param>
    public void AddItem(int ID)
    {
        Inventory.instance.Add(ItemDB.instance.items[ID].item);
        Debug.Log("Added to inventory: " + ItemDB.instance.items[ID].item.name);
    }

    /// <summary>
    /// Removes given item from inventory
    /// </summary>
    /// <param name="ID">ID of item from ItemDB to remove</param>
    /// <param name="numberToRemove">Number of the given item to remove from inventory</param>
    public void RemoveItem(int ID, int numberToRemove)
    {
        for (int i=0; i < numberToRemove; i++)
        {
            Inventory.instance.Remove(ItemDB.instance.items[ID].item);
            Debug.Log("Removed from inventory: " + ItemDB.instance.items[ID].item.name);
        }
    }

    /// <summary>
    /// Removes 1 of given item from inventory
    /// </summary>
    /// <param name="ID">ID of item from ItemDB to remove</param>
    public void RemoveItem(int ID)
    {
        Inventory.instance.Remove(ItemDB.instance.items[ID].item);
        Debug.Log("Removed from inventory: " + ItemDB.instance.items[ID].item.name);
    }

    /// <summary>
    /// Adds given equipment to inventory
    /// </summary>
    /// <param name="ID">ID of equipment from EquipmentDB to add</param>
    /// <param name="numberToAdd">Number of the given equipment to add to inventory</param>
    public void AddEquipment(int ID, int numberToAdd)
    {
        for (int i = 0; i < numberToAdd; i++)
        {
            Inventory.instance.Add(EquipmentDB.instance.equipment[ID].equipment);
            Debug.Log("Added to inventory: " + EquipmentDB.instance.equipment[ID].equipment.name);
        }
    }

    /// <summary>
    /// Adds 1 of given equipment to inventory
    /// </summary>
    /// <param name="ID">ID of equipment from EquipmentDB to add</param>
    public void AddEquipment(int ID)
    {
        Inventory.instance.Add(EquipmentDB.instance.equipment[ID].equipment);
        Debug.Log("Added to inventory: " + EquipmentDB.instance.equipment[ID].equipment.name);
    }

    /// <summary>
    /// Removes given equipment from inventory
    /// </summary>
    /// <param name="ID">ID of equipment from EquipmentDB to add</param>
    /// <param name="numberToRemove">Number of the given equipment to add to inventory</param>
    public void RemoveEquipment(int ID, int numberToRemove) //removes equipment from inventory
    {
        for (int i = 0; i < numberToRemove; i++)
        {
            Inventory.instance.Remove(EquipmentDB.instance.equipment[ID].equipment);
            Debug.Log("Removed from inventory: " + EquipmentDB.instance.equipment[ID].equipment.name);
        }
    }

    /// <summary>
    /// Removes 1 of given equipment from inventory
    /// </summary>
    /// <param name="ID">ID of equipment from EquipmentDB to add</param>
    public void RemoveEquipment(int ID) //removes equipment from inventory
    {
        Inventory.instance.Remove(EquipmentDB.instance.equipment[ID].equipment);
        Debug.Log("Removed from inventory: " + EquipmentDB.instance.equipment[ID].equipment.name);
    }

    //void ChangePartyMember

    #endregion

    #region ---IMAGES---

    //void ShowPicture

    //void MovePicture

    //void RotatePicture

    //void TintPicture

    //void RemovePicture

    #endregion

    #region ---WEATHER/EFFECTS---

    //void FadeInScreen

    //void FadeOutScreen

    //void TintScreen

    //void FlashScreen

    //void ShakeScreen

    #endregion

    #region ---TOOLS FOR EVENTS---

    /// <summary>
    /// Calculates move speed based on number of spaces to be moved based on baseMoveSpeed
    /// </summary>
    /// <param name="spaces">Spaces to calculate move speed</param>
    float GetBaseMoveSpeed(float spaces)
    {
        float tempMoveSpeed = baseMoveSpeed * spaces;
        return tempMoveSpeed;
    }

    /// <summary>
    /// Ensures confirm button is only considered 'pressed' once
    /// </summary>
    void CheckConfirmButtonStatus()
    {
        //Debug.Log(confirmPressed);
        if (Input.GetButtonDown("Confirm"))
        {
            //Debug.Log("buttonPressed");
            confirmPressed = true;
        }
        if (confirmPressed)
        {
            if (Input.GetButtonUp("Confirm"))
            {
                //Debug.Log("button released");
                confirmPressed = false;
                checkForConfirmPressed = false;
            }
        }
    }

    /// <summary>
    /// Tool for ShowMessage to facilitate dialogue operation
    /// </summary>
    /// <param name="message">Spaces to calculate move speed</param>
    /// <param name="textSpeed">How quickly the text can be filled; can use "baseTextSpeed"</param>
    /// <param name="voiceTone">Voice to be played during message</param>
    public IEnumerator WriteToMessagePanel(string message, float textSpeed, AudioClip voiceTone)
    {
        if (voiceTone != null)
        {
            PlayVoice(voiceTone);
        }
        
        Text messageText = GameManager.instance.DialogueCanvas.GetComponentInChildren<Text>(); //assigns messageText to dialogue canvas text component
        string text = message; //gets the text to be put in dialogue UI

        GameManager.instance.DisplayPanel(true); //shows dialogue panel

        for (int i = 0; i < text.RichTextLength(); i++) //for each letter in the text
        {
            CheckConfirmButtonStatus(); //checks if confirm button is pressed to be able to skip message (not yet implemented)
                                        //fullText += text[i]; //adds current letter to fullText
                                        //messageText.text = fullText; //sets dialogue UI text to the current fullText
                                        //fullText += text.RichTextSubString(i);
            messageText.text = text.RichTextSubString(i + 1);
            yield return new WaitForSecondsRealtime(textSpeed); //waits before entering next letter, based on textSpeed
        }

            if (voiceTone != null)
            {
                StopVoice();
            }

            yield return new WaitUntil(() => Input.GetButtonDown("Confirm")); //wait until confirm button pressed before continuing
            PlaySE(confirmSE);

        GameManager.instance.DisplayPanel(false); //hides dialogue panel
    }

    /// <summary>
    /// Disables dialogue choice panels
    /// </summary>
    void DisableChoicePanels()
    {
        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue2ChoicePanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue3ChoicePanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue4ChoicePanel").GetComponent<CanvasGroup>().interactable = false;

        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue2ChoicePanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue3ChoicePanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue4ChoicePanel").GetComponent<CanvasGroup>().alpha = 0;

        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue2ChoicePanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue3ChoicePanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue4ChoicePanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// Enables dialogue choice panel
    /// </summary>
    void EnableChoicePanel(int choices)
    {
        if (choices == 2)
        {
            DialogueChoicePanel = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue2ChoicePanel");
            Choice1Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue2ChoicePanel/Choice1Button").GetComponent<Button>();
            Choice2Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue2ChoicePanel/Choice2Button").GetComponent<Button>();
        }
        else if (choices == 3)
        {
            DialogueChoicePanel = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue3ChoicePanel");
            Choice1Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue3ChoicePanel/Choice1Button").GetComponent<Button>();
            Choice2Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue3ChoicePanel/Choice2Button").GetComponent<Button>();
            Choice3Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue3ChoicePanel/Choice3Button").GetComponent<Button>();
        }
        else if (choices == 4)
        {
            DialogueChoicePanel = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue4ChoicePanel");
            Choice1Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue4ChoicePanel/Choice1Button").GetComponent<Button>();
            Choice2Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue4ChoicePanel/Choice2Button").GetComponent<Button>();
            Choice3Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue4ChoicePanel/Choice3Button").GetComponent<Button>();
            Choice4Button = GameObject.Find("GameManager/DialogueCanvas/DialogueChoicePanel/Dialogue4ChoicePanel/Choice4Button").GetComponent<Button>();
        }

        DialogueChoicePanel.GetComponent<CanvasGroup>().interactable = true;
        DialogueChoicePanel.GetComponent<CanvasGroup>().alpha = 1;
        DialogueChoicePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// Sets text on choice buttons; Minimum of 2 options, 3 or 4 can be null
    /// </summary>
    /// <param name="button1txt">Text for first button</param>
    /// <param name="button2txt">Text for second button</param>
    /// <param name="button3txt">Text for third button</param>
    /// <param name="button4txt">Text for fourth button</param>
    void SetChoiceButtons(string button1txt, string button2txt, string button3txt, string button4txt)
    {
        DisableChoicePanels();

        if (button4txt == null && button3txt == null)
        {
            EnableChoicePanel(2);
        }
        else if (button4txt == null)
        {
            EnableChoicePanel(3);
        }
        else
        {
            EnableChoicePanel(4);
        }

        //Should always have a minimum of 2 choices
        Choice1Button.transform.Find("Text").GetComponent<Text>().text = button1txt;
        Choice1Button.onClick.AddListener(delegate { Choice1ButtonClicked(); });

        Choice2Button.transform.Find("Text").GetComponent<Text>().text = button2txt;
        Choice2Button.onClick.AddListener(delegate { Choice2ButtonClicked(); });

        if (button3txt != null)
        {
            Choice3Button.transform.Find("Text").GetComponent<Text>().text = button3txt;
            Choice3Button.onClick.AddListener(delegate { Choice3ButtonClicked(); });
        }
        if (button4txt != null)
        {
            Choice4Button.transform.Find("Text").GetComponent<Text>().text = button4txt;
            Choice4Button.onClick.AddListener(delegate { Choice4ButtonClicked(); });
        }
    }

    /// <summary>
    /// Sets choiceMade to "button1"
    /// </summary>
    public void Choice1ButtonClicked()
    {
        if (choiceMade == "")
        {
            Debug.Log("Choice 1 button clicked");
            PlaySE(confirmSE);
            choiceMade = "button1";
        }
    }

    /// <summary>
    /// Sets choiceMade to "button2"
    /// </summary>
    public void Choice2ButtonClicked()
    {
        if (choiceMade == "")
        {
            Debug.Log("Choice 2 button clicked");
            PlaySE(confirmSE);
            choiceMade = "button2";
        }
    }

    /// <summary>
    /// Sets choiceMade to "button3"
    /// </summary>
    public void Choice3ButtonClicked()
    {
        if (choiceMade == "")
        {
            Debug.Log("Choice 3 button clicked");
            PlaySE(confirmSE);
            choiceMade = "button3";
        }
    }

    /// <summary>
    /// Sets choiceMade to "button4"
    /// </summary>
    public void Choice4ButtonClicked()
    {
        if (choiceMade == "")
        {
            Debug.Log("Choice 4 button clicked");
            PlaySE(confirmSE);
            choiceMade = "button4";
        }
    }

    /// <summary>
    /// Displays the various input panels by using 'option' parameter to get the panel GameObject
    /// </summary>
    /// <param name="option">Use "Text", "Name", or "Number"</param>
    void DisplayInputPanel(string option)
    {
        HideInputPanels();

        GameObject.Find("GameManager/TextInputCanvas/" + option + "InputPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/TextInputCanvas/" + option + "InputPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/TextInputCanvas/" + option + "InputPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// Hides all input panels
    /// </summary>
    void HideInputPanels()
    {
        GameObject.Find("GameManager/TextInputCanvas/TextInputPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/TextInputCanvas/TextInputPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/TextInputCanvas/TextInputPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel").GetComponent<CanvasGroup>().interactable = false;
        GameObject.Find("GameManager/TextInputCanvas/NumberInputPanel").GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// Turns GameManager.instance.capsOn to false, and sets all letter buttons on TextInputPanel to lowercase
    /// </summary>
    void TextResetCaps()
    {
        GameManager.instance.capsOn = false;

        GameObject panel = GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/LetterButtonsPanel");

        GameObject.Find("GameManager/TextInputCanvas/TextInputPanel/OptionsPanel/capsButton/capsText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        foreach (Transform childButton in panel.transform)
        {
            if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
            {
                childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToLower();
            }
            else
            {
                if (childButton.name == "1Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "1";
                }
                else if (childButton.name == "2Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "2";
                }
                else if (childButton.name == "3Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "3";
                }
                else if (childButton.name == "4Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "4";
                }
                else if (childButton.name == "5Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "5";
                }
                else if (childButton.name == "6Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "6";
                }
                else if (childButton.name == "7Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "7";
                }
                else if (childButton.name == "8Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "8";
                }
                else if (childButton.name == "9Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "9";
                }
                else if (childButton.name == "0Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "0";
                }
                else if (childButton.name == "char1Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "-";
                }
                else if (childButton.name == "char2Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "'";
                }
            }
        }
    }

    /// <summary>
    /// Turns GameManager.instance.capsOn to false, and sets all letter buttons on NameInputPanel to lowercase
    /// </summary>
    void NameResetCaps()
    {
        GameManager.instance.capsOn = false;

        GameObject panel = GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/LetterButtonsPanel");

        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/OptionsPanel/capsButton/capsText").GetComponent<Text>().fontStyle = FontStyle.Normal;
        foreach (Transform childButton in panel.transform)
        {
            if (childButton.name != "1Button" && childButton.name != "2Button" && childButton.name != "3Button" && childButton.name != "4Button" && childButton.name != "5Button" &&
                childButton.name != "6Button" && childButton.name != "7Button" && childButton.name != "8Button" && childButton.name != "9Button" && childButton.name != "0Button" &&
                childButton.name != "char1Button" && childButton.name != "char2Button" && childButton.name != "spaceButton")
            {
                childButton.GetChild(0).GetComponent<Text>().text = childButton.GetChild(0).GetComponent<Text>().text.ToLower();
            }
            else
            {
                if (childButton.name == "1Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "1";
                }
                else if (childButton.name == "2Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "2";
                }
                else if (childButton.name == "3Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "3";
                }
                else if (childButton.name == "4Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "4";
                }
                else if (childButton.name == "5Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "5";
                }
                else if (childButton.name == "6Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "6";
                }
                else if (childButton.name == "7Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "7";
                }
                else if (childButton.name == "8Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "8";
                }
                else if (childButton.name == "9Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "9";
                }
                else if (childButton.name == "0Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "0";
                }
                else if (childButton.name == "char1Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "-";
                }
                else if (childButton.name == "char2Button")
                {
                    childButton.GetChild(0).GetComponent<Text>().text = "'";
                }
            }
        }
    }

    /// <summary>
    /// Returns enemy by ID
    /// </summary>
    /// <param name="ID">ID of enemy from EnemyDB to be returned</param>
    protected BaseEnemy GetEnemy(int ID)
    {
        foreach (BaseEnemyDBEntry entry in GameObject.Find("GameManager/EnemyDB").GetComponent<EnemyDB>().enemies)
        {
            if (entry.enemy.ID == ID)
            {
                return entry.enemy;
            }
        }
        return null;
    }

    void PlayVoice(AudioClip voice)
    {
        voiceAudioSource.clip = voice;
        voiceAudioSource.Play();
    }

    void StopVoice()
    {
        voiceAudioSource.Stop();
    }

    #endregion
}


