using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        thisGameObject = this.gameObject; //sets thisGameObject to game object this script is attached to
        thisTransform = this.gameObject.transform; //sets thisTransform to transform of game object this script is attached to
        playerGameObject = GameObject.Find("Player"); //sets playerGameObject to gameobject of player
        playerTransform = playerGameObject.transform; //sets playerTransform to transform of gameobject of player
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); //sets gameManager to the game manager object in scene
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

    public IEnumerator MoveLeft(GameObject GO, float timeToMove, float spacesToMove) //moves object left
    {
        TurnLeft(GO);

        Rigidbody2D rb = GO.GetComponent<Rigidbody2D>(); //uses rigidbody as transform.move was causing collision issues

        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        if (GO == playerGameObject)
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

        if (GO == playerGameObject)
        {
            EnablePlayerMovement();
        }       
    }  

    public IEnumerator MoveRight(GameObject GO, float timeToMove, float spacesToMove) //moves object right
    {
        TurnRight(GO);

        Rigidbody2D rb = GO.GetComponent<Rigidbody2D>();
        
        
        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        if (GO == playerGameObject)
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

        if (GO == playerGameObject)
        {
            EnablePlayerMovement();
        }
    } 

    public IEnumerator MoveUp(GameObject GO, float timeToMove, float spacesToMove) //moves object up
    {
        TurnUp(GO);

        Rigidbody2D rb = GO.GetComponent<Rigidbody2D>();

        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        if (GO == playerGameObject)
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

        if (GO == playerGameObject)
        {
            EnablePlayerMovement();
        }
    } 

    public IEnumerator MoveDown(GameObject GO, float timeToMove, float spacesToMove) //moves object down
    {
        TurnDown(GO);
        Rigidbody2D rb = GO.GetComponent<Rigidbody2D>();

        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        if (GO == playerGameObject)
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

        if (GO == playerGameObject)
        {
            EnablePlayerMovement();
        }
    } 

    public IEnumerator MoveLeftUp(GameObject GO, float timeToMove, int spacesToMove) //moves object diagonally left and up - needs rigidbody to be implemented
    {
        Transform transform = GO.transform;
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

    public IEnumerator MoveRightUp(GameObject GO, float timeToMove, int spacesToMove) //moves object diagonally right and up - needs rigidbody to be implemented
    {
        Transform transform = GO.transform;
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

    public IEnumerator MoveLeftDown(GameObject GO, float timeToMove, int spacesToMove) //moves object diagonally left and down - needs rigidbody to be implemented
    {
        Transform transform = GO.transform;
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

    public IEnumerator MoveRightDown(GameObject GO, float timeToMove, int spacesToMove) //moves object diagonally right and down - needs rigidbody to be implemented
    {
        Transform transform = GO.transform;
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

    public IEnumerator MoveToTarget(GameObject GO, Vector2 target, float timeToMove) //moves object to target position - needs rigidbody to be implemented
    {
        Transform transform = GO.transform;
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

    public IEnumerator MoveRandom(GameObject GO, float timeToMove, int spacesToMove) //moves object randomly - needs rigidbody to be implemented
    {
        Transform transform = GO.transform;

        if (timeToMove == baseMoveSpeed)
        {
            timeToMove = GetBaseMoveSpeed(spacesToMove);
        }

        Random.InitState(System.DateTime.Now.Millisecond);
        int randomDirection = Random.Range(0, 4);

        if (randomDirection == 0)
        {
            yield return MoveLeft(GO, timeToMove, spacesToMove); //left
        }
        if (randomDirection == 1)
        {
            yield return MoveRight(GO, timeToMove, spacesToMove); //right
        }
        if (randomDirection == 2)
        {

            yield return MoveDown(GO, timeToMove, spacesToMove); //down
        }
        if (randomDirection == 3)
        {
            yield return MoveUp(GO, timeToMove, spacesToMove); //up
        }
    } 

    public IEnumerator MoveTowardPlayer(GameObject GO, float timeToMove, int spacesToMove)  //moves object toward player
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
                        TurnLeft(GO);
                        yield return MoveLeft(GO, timeToMove, spacesToMove);
                    }
                    else //player is right of object
                    {
                        TurnRight(GO);
                        yield return MoveRight(GO, timeToMove, spacesToMove);
                    }
                }
                else if (dimToUse == "y")
                {
                    if (hit.point.y < 0) //player is below object
                    {
                        TurnDown(GO);
                        yield return MoveDown(GO, timeToMove, spacesToMove);
                    }
                    else //player is above object
                    {
                        TurnUp(GO);
                        yield return MoveUp(GO, timeToMove, spacesToMove);
                    }
                }
                else
                {
                    Debug.Log("MoveTowardPlayer - dimToUse not found!");
                }
            }
        }
    }

   public IEnumerator MoveAwayFromPlayer(GameObject GO, float timeToMove, int spacesToMove) //moves object away from player
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
                        TurnRight(GO);
                        yield return MoveRight(GO, timeToMove, spacesToMove);
                    }
                    else //player is right of object
                    {
                        TurnLeft(GO);
                        yield return MoveLeft(GO, timeToMove, spacesToMove);
                    }
                }
                else if (dimToUse == "y")
                {
                    if (hit.point.y < 0) //player is below object
                    {
                        TurnUp(GO);
                        yield return MoveUp(GO, timeToMove, spacesToMove);
                    }
                    else //player is above object
                    {
                        TurnDown(GO);
                        yield return MoveDown(GO, timeToMove, spacesToMove);
                    }
                }
                else
                {
                    Debug.Log("MoveTowardPlayer - dimToUse not found!");
                }
            }
        }
    }

    public IEnumerator StepForward(GameObject GO, float timeToMove, int spacesToMove) //moves object in direction they are facing
    {
        if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            yield return MoveUp(GO, timeToMove, spacesToMove);
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            yield return MoveDown(GO, timeToMove, spacesToMove);
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            yield return MoveLeft(GO, timeToMove, spacesToMove);
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            yield return MoveRight(GO, timeToMove, spacesToMove);
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = GO.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                yield return MoveUp(GO, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                yield return MoveDown(GO, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                yield return MoveLeft(GO, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                yield return MoveRight(GO, timeToMove, spacesToMove);
            }
        }
    }

    public IEnumerator StepBackward(GameObject GO, float timeToMove, int spacesToMove) //moves object in opposite direction they are facing
    {
        if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            yield return MoveDown(GO, timeToMove, spacesToMove);
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            yield return MoveUp(GO, timeToMove, spacesToMove);
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            yield return MoveRight(GO, timeToMove, spacesToMove);
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            yield return MoveLeft(GO, timeToMove, spacesToMove);
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = GO.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                yield return MoveDown(GO, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                yield return MoveUp(GO, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                yield return MoveRight(GO, timeToMove, spacesToMove);
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                yield return MoveLeft(GO, timeToMove, spacesToMove);
            }
        }
    }

    public void TurnDown(GameObject GO) //makes object face downward
    {
        GO.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
    }

    public void TurnLeft(GameObject GO) //makes object face left
    {
        GO.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
    }

    public void TurnRight(GameObject GO) //makes object face right
    {
        GO.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
    }

    public void TurnUp(GameObject GO) //makes object face upward
    {
        GO.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
    }

    public void TurnDefault(GameObject GO) //makes object face their default state
    {
        GO.GetComponent<FacingState>().faceState = FacingState.FaceState.DEFAULT;
    }

    public void TurnTowardPlayer(GameObject GO) //makes object face toward player
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
                        TurnLeft(GO);
                    } else //player is right of object
                    {
                        TurnRight(GO);
                    }
                }
                else if (dimToUse == "y")
                {
                    if (hit.point.y < 0) //player is below object
                    {
                        TurnDown(GO);
                    }
                    else //player is above object
                    {
                        TurnUp(GO);
                    }
                } else
                {
                    Debug.Log("TurnTowardPlayer - dimToUse not found!");
                }
            }
        }
    }

    public void TurnAwayFromPlayer(GameObject GO) //makes object face away from player
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
                        TurnRight(GO);
                    }
                    else //player is right of object
                    {
                        TurnLeft(GO);
                    }
                }
                else if (dimToUse == "y")
                {
                    if (hit.point.y < 0) //player is below object
                    {
                        TurnUp(GO);
                    }
                    else //player is above object
                    {
                        TurnDown(GO);
                    }
                }
                else
                {
                    Debug.Log("TurnAwayFromPlayer - dimToUse not found!");
                }
            }
        }
    }

    public void Turn90Right (GameObject GO) //turns object 90 degrees to the right
    {
        if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
        } else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
        } else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
        } else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
        } else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = GO.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
            }
        }
    }

    public void Turn90Left (GameObject GO) //turns object 90 degrees to the left
    {
        
        if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
        } else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
        } else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
        } else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
        } else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = GO.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
            }
        }
    }

    public void Turn180 (GameObject GO) //turns object 180 degrees (turns them around)
    {
        if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.UP)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DOWN)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.LEFT)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.RIGHT)
        {
            GO.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
        }
        else if (GO.GetComponent<FacingState>().faceState == FacingState.FaceState.DEFAULT)
        {
            string defaultDirection = GO.GetComponent<FacingState>().defaultDirection;
            if (defaultDirection == "Up" || defaultDirection == "up")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.DOWN;
            }
            else if (defaultDirection == "Down" || defaultDirection == "down")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.UP;
            }
            else if (defaultDirection == "Left" || defaultDirection == "left")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.RIGHT;
            }
            else if (defaultDirection == "Right" || defaultDirection == "right")
            {
                GO.GetComponent<FacingState>().faceState = FacingState.FaceState.LEFT;
            }
        }
    }

    public void Turn90RightOrLeft(GameObject GO) //randomly chooses left or right, and turns object 90 degrees to that direction
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int randDir = Random.Range(0, 2);
        if (randDir == 0)
        {
            Turn90Right(GO);
        } else if (randDir == 1)
        {
            Turn90Left(GO);
        }
    }

    public void TurnRandom(GameObject GO) //turns object in random direction
    {
        bool sameDir = true;
        FacingState fs = GO.GetComponent<FacingState>();
        while (sameDir == true)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int randDir = Random.Range(0, 4);
            if (randDir == 0)
            {
                if (fs.faceState != FacingState.FaceState.UP || (fs.faceState != FacingState.FaceState.DEFAULT && (fs.defaultDirection == "Up" || fs.defaultDirection == "up")))
                {
                    sameDir = false;
                    TurnUp(GO);
                }
            }
            else if (randDir == 1)
            {
                if (fs.faceState != FacingState.FaceState.DOWN || (fs.faceState != FacingState.FaceState.DEFAULT && (fs.defaultDirection == "Down" || fs.defaultDirection == "down")))
                {
                    sameDir = false;
                    TurnDown(GO);
                }
            }
            else if (randDir == 2)
            {
                if (fs.faceState != FacingState.FaceState.LEFT || (fs.faceState != FacingState.FaceState.DEFAULT && (fs.defaultDirection == "Left" || fs.defaultDirection == "left")))
                {
                    sameDir = false;
                    TurnLeft(GO);
                }
            }
            else if (randDir == 3)
            {
                if (fs.faceState != FacingState.FaceState.RIGHT || (fs.faceState != FacingState.FaceState.DEFAULT && (fs.defaultDirection == "Right" || fs.defaultDirection == "right")))
                {
                    sameDir = false;
                    TurnRight(GO);
                }
            }
        }
    }

    public void ChangeDefaultMoveSpeed(float newMoveSpeed) //changes default move speed
    {
        playerGameObject.GetComponent<PlayerController2D>().moveSpeed = newMoveSpeed;
    }

    public void EnableWalkingAnimation() //enables walking animation for player
    {
        playerGameObject.GetComponent<Animator>().SetBool("isMoving", true); 
    }

    public void DisableWalkingAnimation() //disables walking animation for player
    {
        playerGameObject.GetComponent<Animator>().SetBool("isMoving", false); 
    }

    public void EnableForceDirection(GameObject GO) //keeps player from being able to change facing direction
    {
        GO.GetComponent<FacingState>().forceDirection = true;
    }

    public void DisableForceDirection(GameObject GO) //allows player to change facing direction
    {
        GO.GetComponent<FacingState>().forceDirection = false;
    }

    public void DisablePlayerMovement() //disables player's movement
    {
        playerGameObject.GetComponent<PlayerController2D>().enabled = false;
    } 

    public void EnablePlayerMovement() //enables player's movement
    {
        playerGameObject.GetComponent<PlayerController2D>().enabled = true;
    }

    public void SavePosition(GameObject objectToSave) //saves the position of player for loadPosition method
    {
        BasePositionSave thePosition = new BasePositionSave();
        thePosition._Name = objectToSave.name;
        thePosition.newPosition = gameObject.transform.position;
        thePosition.newDirection = "Test"; //not yet implemented
        thePosition.Scene = SceneManager.GetActiveScene().name;
        GameManager.instance.positionSaves.Add(thePosition);
        Debug.Log("Position saved: " + thePosition._Name);
    }

    #endregion

    #region ---BATTLE MANAGEMENT---

    public void CallBattle(int troopIndex, string scene) //calls battle from script using the troop index and battle scene to be loaded
    {
        GameManager.instance.GetBattleFromScript(troopIndex, scene);
    } 

    public void ChangeBattleFrequency(int battleChance, int maxBattleChance) //changes global battle frequency
    {
        GameManager.instance.battleChance = battleChance;
        GameManager.instance.maxBattleChance = maxBattleChance;
    }

    public void ChangeEncounterChanceFromRegion(GameObject region, int index, float newEncounterChance) //changes encounter chance from given encounter region
    {
        region.GetComponent<RegionData>().troopEncounters[index].encounterChance = newEncounterChance;
    }

    #endregion

    #region ---SCENE MANAGEMENT---

    //void TransitionToScene

    public void OpenItemShop()
    {
        ItemShop itemShop = GetComponent<ItemShop>();
        GameManager.instance.itemShopList = itemShop.itemShopList;
        itemShop.ShowItemListInBuyGUI();
        itemShop.DisplayItemShopGUI();
        DisablePlayerMovement();
    }

    public void OpenEquipShop()
    {
        EquipShop equipShop = GetComponent<EquipShop>();
        GameManager.instance.equipShopList = equipShop.equipShopList;
        equipShop.ShowEquipListInBuyGUI();
        equipShop.DisplayEquipShopGUI();
        DisablePlayerMovement();
    }

    public void OpenMenu() //forces menu to be opened
    {
        GameObject.Find("GameManager").GetComponent<GameMenu>().menuCalled = true;
    }

    //void OpenSave

    //void GameOver

    //void ReturnToTitle

    #endregion

    #region ---GAME MANAGEMENT---

    public void ChangeSwitch(GameObject whichObject, int whichEvent, int whichSwitch, bool whichBool) //changes switch value
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

    public bool GetSwitchBool(GameObject whichObject, int whichEvent, int whichSwitch) //returns if switch is true or false (not yet fully implemented)
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

    public void ChangeGlobalBool(int index, bool boolean) //changes value of global event bools
    {
        GameManager.instance.globalBools[index] = boolean;
    }

    #endregion

    #region ---MUSIC/SOUNDS---

    //void PlaySE

    //void PlayBGM

    //ienumerator FadeOutBGM

    //ienumerator FadeInBGM

    //void PlayBGS

    //ienumerator FadeOutBGS

    //void StopBGM

    //void StopSE

    #endregion

    #region ---TIMING---

    public IEnumerator WaitForSeconds(float waitTime) //pause for period of time
    {
        yield return new WaitForSeconds(waitTime);
    }

    #endregion

    #region ---DIALOGUE---

    public IEnumerator ShowMessage(string message, float textSpeed, bool waitForEnd, bool lockPlayerMovement) //Displays any custom message
    {
        if (lockPlayerMovement)
        {
            Debug.Log("Disabling player movement");
            DisablePlayerMovement();
        }
        if (waitForEnd)
        {
            yield return (StartCoroutine(WriteToMessagePanel(message, textSpeed)));
        }
        else
        {
            StartCoroutine(WriteToMessagePanel(message, textSpeed));
        }
        if (lockPlayerMovement)
        {
            Debug.Log("Enabling player movement");
            EnablePlayerMovement();
        }
    }

    public IEnumerator ShowDialogueChoices(string message, string button1Text, RunDialogueChoice choice1, string button2Text, RunDialogueChoice choice2, string button3Text, RunDialogueChoice choice3, string button4Text, RunDialogueChoice choice4)
    {
        DisablePlayerMovement();
        bool messageFinished;
        Text messageText = GameManager.instance.DialogueCanvas.GetComponentInChildren<Text>(); //assigns messageText to dialogue canvas text component
        messageFinished = false; //starting the dialogue text, sets to true once all text is added
        string text = message; //gets the text to be put in dialogue UI
        string fullText = ""; //sets current fullText to blank as letters will be added individually

        GameManager.instance.DisplayPanel(true); //shows dialogue panel

        for (int i = 0; i < text.Length; i++) //for each letter in the text
        {
            fullText += text[i]; //adds current letter to fullText
            messageText.text = fullText; //sets dialogue UI text to the current fullText
            yield return new WaitForSecondsRealtime(baseTextSpeed);
        }

        if (fullText == text) //if full message has been added to the dialogue text
        {
            messageFinished = true;
        }

        if (messageFinished)
        {
            choiceMade = "";
        }
        
        SetChoiceButtons(button1Text, button2Text, button3Text, button4Text);

        yield return new WaitUntil(() => choiceMade != "");

        DialogueChoicePanel.GetComponent<CanvasGroup>().alpha = 0;
        GameManager.instance.DisplayPanel(false); //hides dialogue panel

        EnablePlayerMovement();
        
        if (choiceMade == "button1")
        {
            choice1();
        } else if (choiceMade == "button2")
        {
            choice2();
        } else if (choiceMade == "button3")
        {
            choice3();
        } else if (choiceMade == "button4")
        {
            choice4();
        }
    }

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

    #region ---SYSTEM SETTINGS---

    //void ChangeBattleBGM

    //void ChangeSaveAccess

    //void ChangeMenuAccess

    #endregion

    #region ---SPRITES---

    //void ChangeGraphic

    //void ChangeOpacity

    //void AddSprite

    //void RemoveSprite

    #endregion

    #region ---ACTORS---

    public void FullHeal()
    {
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            hero.curHP = hero.maxHP;
            hero.curMP = hero.maxMP;
        }
    }

    //void ChangeHP

    //void ChangeMP

    //void ChangeEXP

    //void ChangeLevel

    //void ChangeParameter

    //void AddSkill

    //void RemoveSkill

    //void ChangeEquipment

    //void ChangeName

    public IEnumerator NameInput(BaseHero hero)
    {
        DisablePlayerMovement();
        DisplayInputPanel("Name");
        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/NameEnteredPanel/NameEnteredText").GetComponent<Text>().text = hero._Name;

        GameManager.instance.nameInput = "";

        NameResetCaps();

        GameObject.Find("GameManager/TextInputCanvas/NameInputPanel/FacePanel").GetComponent<Image>().sprite = hero.faceImage;

        while (GameManager.instance.nameInput == "")
        {
            yield return null;
        }

        hero._Name = GameManager.instance.nameInput;

        EnablePlayerMovement();
        HideInputPanels();
    }

    #endregion

    #region ---PARTY---

    public void ChangeGold(int gold)
    {
        GameManager.instance.gold += gold;
    }

    public void AddItem(Item item) //adds item to inventory
    {
        Inventory.instance.Add(item);
        Debug.Log("Added to inventory: " + item.name);
    }

    public void RemoveItem(Item item) //removes item from inventory
    {
        Inventory.instance.Remove(item);
        Debug.Log("Removed from inventory: " + item.name);
    }

    //void AddWeapon

    //void RemoveWeapon

    //void AddArmor

    //void RemoveArmor

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

    float GetBaseMoveSpeed(float spaces) //calculates move speed based on number of spaces to be moved, to keep base move speed consistent
    {
        float tempMoveSpeed = baseMoveSpeed * spaces;
        return tempMoveSpeed;
    }

    public IEnumerator WriteToMessagePanel(string message, float textSpeed) //Tool for ShowMessage to facilitate dialogue operation
    {
        bool messageFinished;
        Text messageText = GameManager.instance.DialogueCanvas.GetComponentInChildren<Text>(); //assigns messageText to dialogue canvas text component
        messageFinished = false; //starting the dialogue text, sets to true once all text is added
        string text = message; //gets the text to be put in dialogue UI
        string fullText = ""; //sets current fullText to blank as letters will be added individually

        GameManager.instance.DisplayPanel(true); //shows dialogue panel

        for (int i = 0; i < text.Length; i++) //for each letter in the text
        {
            fullText += text[i]; //adds current letter to fullText
            messageText.text = fullText; //sets dialogue UI text to the current fullText
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        if (fullText == text) //if full message has been added to the dialogue text
        {
            messageFinished = true;
        }

        if (messageFinished)
        {
            yield return new WaitUntil(() => Input.GetButtonDown("Confirm")); //wait until confirm button pressed before continuing
        }

        GameManager.instance.DisplayPanel(false); //hides dialogue panel
    }

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

    public void Choice1ButtonClicked()
    {
        if (choiceMade == "")
        {
            Debug.Log("Choice 1 button clicked");
            choiceMade = "button1";
        }
    }

    public void Choice2ButtonClicked()
    {
        if (choiceMade == "")
        {
            Debug.Log("Choice 2 button clicked");
            choiceMade = "button2";
        }
    }

    public void Choice3ButtonClicked()
    {
        if (choiceMade == "")
        {
            Debug.Log("Choice 3 button clicked");
            choiceMade = "button3";
        }
    }

    public void Choice4ButtonClicked()
    {
        if (choiceMade == "")
        {
            Debug.Log("Choice 4 button clicked");
            choiceMade = "button4";
        }
    }

    void DisplayInputPanel(string option) //option should be 'Text', 'Name', or 'Number'
    {
        HideInputPanels();

        GameObject.Find("GameManager/TextInputCanvas/" + option + "InputPanel").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameManager/TextInputCanvas/" + option + "InputPanel").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("GameManager/TextInputCanvas/" + option + "InputPanel").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

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

    #endregion
}


