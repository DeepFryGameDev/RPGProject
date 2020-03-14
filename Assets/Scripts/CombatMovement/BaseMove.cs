using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    public bool canMove = false;

    protected List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    protected Stack<Tile> path = new Stack<Tile>();
    public Tile currentTile;

    public bool readyForMove = false;
    public int move = 3;
    public float moveSpeed = 2;

    protected Vector3 velocity = new Vector3();
    protected Vector3 heading = new Vector3();

    float halfHeight = 0; //might not need

    public Tile actualTargetTile;
    
    protected int turn;

    public void InitMove()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<BoxCollider2D>().bounds.extents.z;

        turn = 0;
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile (GameObject target)
    {
        RaycastHit2D hit = Physics2D.Raycast(target.transform.position, Vector3.back, 1);

        Tile tile = null;
        
        if (hit.collider != null)
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyLists(Tile target)
    {
        //tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(target);
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        readyForMove = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    protected void CalculateHeading(Vector2 target)
    {
        heading = (target - (Vector2)transform.position);
        heading.Normalize();
    }

    protected void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while(next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move) //if target is in range
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        //currentTile.parent = ??
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjecencyList)
            {
                if (closedList.Contains(tile))
                {
                    //do nothing, already processed
                } else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g) //found quicker way to target
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                } else //never processed the tile
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }

        }

        //todo: what do you do if there is no path to the target tile?
        Debug.Log("Path not found");

    }

    public void BeginTurn()
    {
        canMove = true;
        turn++;
        //Debug.Log(transform.gameObject.name + " starting turn " + turn);
    }

    public void EndTurn()
    {
        RemoveSelectableTiles();
        canMove = false;
        //Debug.Log(transform.gameObject.name + " ending turn " + turn);
    }
}
