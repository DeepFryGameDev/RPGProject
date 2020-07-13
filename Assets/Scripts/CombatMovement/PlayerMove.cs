using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : BaseMove
{
    HeroStateMachine HSM;
    public int tilesToMove;
    bool ignoreMoveOnce;
    
    void Start()
    {
        InitMove();
        HSM = GetComponent<HeroStateMachine>();
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
            CheckMouse();
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
                }
                else
                {
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

    /// <summary>
    /// If mouse button is clicked, moves to selected tile
    /// </summary>
    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            
         RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
        if (hit.collider != null)
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        AudioManager.instance.PlaySE(AudioManager.instance.confirmSE);
                        MoveToTile(t);
                    }
                }
            }
        }
    }
}
