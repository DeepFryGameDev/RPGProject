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

    //----MOVEMENT---- (Needs animation to be built in)
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


    //----------------


    //----BATTLE MANAGEMENT----

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

    //----------------


    //----SCENE MANAGEMENT----

    //void TransitionToScene

    //void ShowShop

    public void OpenMenu() //forces menu to be opened
    {
        GameObject.Find("GameManager").GetComponent<GameMenu>().menuCalled = true;
    }

    //void OpenSave

    //void GameOver

    //void ReturnToTitle

    //----------------


    //----GAME MANAGEMENT----
    
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

    //----------------


    //----MUSIC/SOUNDS----

    //void PlaySE

    //void PlayBGM

    //ienumerator FadeOutBGM

    //ienumerator FadeInBGM

    //void PlayBGS

    //ienumerator FadeOutBGS

    //void StopBGM

    //void StopSE

    //----------------


    //----TIMING----

    public IEnumerator WaitForSeconds(float waitTime) //pause for period of time
    {
        yield return new WaitForSeconds(waitTime);
    }

    //----------------


    //----DIALOGUE----

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

    //void ShowChoices

    //void InputNumber

    //----------------


    //----SYSTEM SETTINGS----

    //void ChangeBattleBGM

    //void ChangeSaveAccess

    //void ChangeMenuAccess

    //----------------


    //----SPRITES----

    //void ChangeGraphic

    //void ChangeOpacity

    //void AddSprite

    //void RemoveSprite

    //----------------


    //----ACTORS----

    //void ChangeHP

    //void ChangeMP

    //void ChangeEXP

    //void ChangeLevel

    //void ChangeParameter

    //void AddSkill

    //void RemoveSkill

    //void ChangeEquipment

    //void ChangeName

    //void InputName

    //----------------


    //----PARTY----

    //void ChangeGold

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

    //----------------


    //----IMAGES----

    //void ShowPicture

    //void MovePicture

    //void RotatePicture

    //void TintPicture

    //void RemovePicture

    //----------------


    //----SCREEN EFFECTS/WEATHER----

    //void FadeInScreen

    //void FadeOutScreen

    //void TintScreen

    //void FlashScreen

    //void ShakeScreen

    //----------------

    //----FOR EVENTS----
    float GetBaseMoveSpeed(float spaces) //calculates move speed based on number of spaces to be moved, to keep base move speed consistent
    {
        float tempMoveSpeed = baseMoveSpeed * spaces;
        return tempMoveSpeed;
    } 
}


