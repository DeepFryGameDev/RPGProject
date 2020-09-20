using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : BaseMove
{
    HeroStateMachine HSM;
    public int tilesToMove;
    bool ignoreMoveOnce;

    Vector3 lastPos;

    Camera battleCam;

    void Start()
    {
        InitMove();
        HSM = GetComponent<HeroStateMachine>();
        move = HSM.hero.finalMoveRating;
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        if (!readyForMove)
        {            
            FindSelectableTiles();
        }
        else
        {
            Move();
            HSM.startPosition = transform.position;
        }
    }

    /// <summary>
    /// Algorithm for movement processes when moving along tiles
    /// </summary>
    public void Move()
    {
        if (path.Count > 0)
        {
            ProcessWalkingAnimation();

            Tile t = path.Peek();

            Vector3 target = t.transform.position;
            
            target.z = 1;

            //calculate the unit's position on top of the target tile
            if (Vector2.Distance(transform.position, target) >= 0.05f)
            {
                CalculateHeading(target);
                SetMoveVelocity();
                transform.forward = heading;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //tile center reached

                transform.position = target;
                path.Pop();

                //this is at the end of every tile moved                
                if (!ignoreMoveOnce)
                {
                    ignoreMoveOnce = true;
                } else
                {
                    tilesToMove -= 1;
                }
            }
        }
        else
        {
            ToggleMoveActionPanel(true);

            GameObject.Find("Main Camera").transform.SetParent(null);

            if (tilesToMove > 0)
            {
                ignoreMoveOnce = false;
                readyForMove = false;
            } else
            {
                ignoreMoveOnce = false;
                readyForMove = false;
                //EndTurn();
            }
        }
    }

    void ProcessWalkingAnimation()
    {
        Vector3 newPos = gameObject.transform.position;

        if (lastPos == new Vector3(0.0f, 0.0f, 0.0f))
        {
            lastPos = newPos;
        }

        //Debug.Log("last position: " + lastPos);
        //Debug.Log("current position: " + newPos);

        if (lastPos != newPos)
        {
            Animator heroAnim = gameObject.GetComponent<Animator>();

            heroAnim.SetFloat("moveX", 0.0f);
            heroAnim.SetFloat("moveY", 0.0f);

            if (lastPos.x != newPos.x)
            {
                float xDiff = lastPos.x - newPos.x;
                if (xDiff > 0)
                {
                    //Debug.Log("moving left");
                    heroAnim.SetFloat("moveX", -1.0f);
                } else
                {
                    //Debug.Log("moving right");
                    heroAnim.SetFloat("moveX", 1.0f);
                }
            }

            if (lastPos.y != newPos.y)
            {
                float yDiff = lastPos.y - newPos.y;
                if (yDiff > 0)
                {
                    //Debug.Log("moving down");
                    heroAnim.SetFloat("moveY", -1.0f);
                } else
                {
                    //Debug.Log("moving up");
                    heroAnim.SetFloat("moveY", 1.0f);
                }
            }
        }

        if (lastPos != null && lastPos != newPos)
        {
            lastPos = newPos;
        }
    }

    /// <summary>
    /// Detects which tiles can be selected to choose a tile to move to
    /// </summary>
    public void FindSelectableTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tileObj in tiles)
        {
            tileObj.GetComponent<Tile>().ClearPathable();
        }

        ComputeAdjacencyLists(null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = ?? leave as null

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;
            t.pathable = true;

            RaycastHit2D[] selectableHits = Physics2D.RaycastAll(t.transform.position, Vector3.forward, 1);
            
            foreach (RaycastHit2D checkIfSelectable in selectableHits) //Allows active hero to move past all heroes, but not land on them.
            {
                if (checkIfSelectable.collider.gameObject.tag == "Hero")
                {
                    t.selectable = false;
                }  else
                {
                    t.selectable = true;
                }

                if (checkIfSelectable.collider.gameObject.tag == "Enemy" || checkIfSelectable.collider.gameObject.tag == "Shieldable")
                {
                    t.pathable = false;
                    t.selectable = false;
                } else
                {
                    t.pathable = true;
                    t.selectable = true;
                }
            }

            if (t.distance < tilesToMove)
            {
                foreach (Tile tile in t.adjecencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }
}
