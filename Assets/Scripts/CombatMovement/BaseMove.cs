using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseMove : MonoBehaviour
{
    //This class facilitates the unit algorithms for combat; algorithm used: Breadth First Search (BFS) - tutorial where this was implemented can be found here:
    //https://www.youtube.com/watch?v=cX_KrK8RQ2o
    //Note: As the combat system from this game does not use 3D movement, jumpHeight and halfExtents were not used

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
    
    public Tile actualTargetTile;
    
    protected int turn;

    protected BattleStateMachine BSM;

    public bool canChooseAction;

    /// <summary>
    /// Facilitates setting the tiles array to tile GameObjects, sets BSM to the battle state machine, and sets turn count to 0
    /// Explanation on tutorial: https://youtu.be/2NVEqBeXdBk?t=38
    /// </summary>
    public void InitMove()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        turn = 0;
    }

    /// <summary>
    /// Sets current tile to the hero/enemy's current tile GameObject
    /// Explanation on tutorial: https://youtu.be/2NVEqBeXdBk?t=455
    /// </summary>
    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
        //Debug.Log("GetCurrentTile: " + currentTile.gameObject);
    }

    /// <summary>
    /// Returns tile that given target is standing on using raycast
    /// Explanation on tutorial: https://youtu.be/2NVEqBeXdBk?t=473
    /// </summary>
    /// <param name="target">Given target to return tile</param>
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

    /// <summary>
    /// Part of BFS algorithm - gets all adjacent tiles to given target tile
    /// Explanation on tutorial: https://youtu.be/2NVEqBeXdBk?t=660
    /// </summary>
    /// <param name="target">Target tile to calculate adjacent tiles</param>
    public void ComputeAdjacencyLists(Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(target);
        }
    }

    /// <summary>
    /// Algorithm for moving to tile in path
    /// Ran once when clicking new tile to be moved to
    /// Explanation on tutorial: https://youtu.be/2NVEqBeXdBk?t=1580
    /// </summary>
    /// <param name="tile">Tile to be moved to in path</param>
    public void MoveToTile(Tile tile)
    {
        if (gameObject.tag == "Hero")
        {
            ToggleMoveActionPanel(false);
        }

        GameObject.Find("Main Camera").transform.SetParent(gameObject.transform);

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

    /// <summary>
    /// Clears pathable tiles and resets all selectable tile parameters
    /// </summary>
    public void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        Debug.Log("removing selectable tiles");

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
            tile.ClearPathable();
        }

        selectableTiles.Clear();
    }

    /// <summary>
    /// Calculates direction to head in path
    /// </summary>
    /// <param name="target">Position to calculate direction from current tile</param>
    protected void CalculateHeading(Vector2 target)
    {
        heading = (target - (Vector2)transform.position);
        heading.Normalize();
    }

    /// <summary>
    /// Sets move speed for hero/enemy to path through tiles
    /// </summary>
    protected void SetMoveVelocity()
    {
        velocity = heading * moveSpeed;
    }

    /// <summary>
    /// Algorithm for finding quickest route to target tile and returning the next tile in the path
    /// </summary>
    /// <param name="list">Given list of tiles to find quickest route</param>
    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            //Debug.Log("FindLowestF: " + t);
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    /// <summary>
    /// Algorithm for returning end tile in movement path
    /// </summary>
    /// <param name="t">Tile to locate parent</param>
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

    /// <summary>
    /// Algorithm for building path to given target tile
    /// </summary>
    /// <param name="target">Tile to find path to from current tile</param>
    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(target);
        GetCurrentTile();
        
        List<Tile> openList = new List<Tile>(); //any tile that has not been processed
        List<Tile> closedList = new List<Tile>(); //any tile that has been processed
        //when the target tile is added to the closedList, we have found the closest path to the target tile

        openList.Add(currentTile);
        //currentTile.parent = ??
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);
            //Debug.Log("1: " + t.gameObject.name);
            closedList.Add(t);
            foreach (Tile tile in closedList)
            {
                //Debug.Log(tile.gameObject.name);
            }
            //Debug.Log("t: " + t.gameObject.name);
            //Debug.Log("target: " + target.gameObject.name);
            if (t == target) //<---- t is never the target tile
            {
                //Debug.Log("if t == target: true");
                actualTargetTile = FindEndTile(t);
                //Debug.Log("actual target file: " + actualTargetTile.gameObject.name);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjecencyList)
            {
                //Debug.Log("in foreach loop: " + tile.gameObject.name);
                if (closedList.Contains(tile))
                {
                    //Debug.Log("do nothing");
                    //do nothing, already processed
                } else if (openList.Contains(tile))
                {
                    //Debug.Log("if openlist contains tile in adjecency list");
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    //Debug.Log("tempG: " + tempG);
                    if (tempG < tile.g) //found quicker way to target
                    {
                        Debug.Log("tempG < tile.g");
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                } else //never processed the tile
                {
                    //Debug.Log("never processed the tile");
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }

        }

        //todo: what do you do if there is no path to the target tile?  Likely just skip turn I'm thinking
        Debug.Log("Path not found");
    }

    /// <summary>
    /// Increases turn count and allows unit to move - only heroes use this method
    /// </summary>
    public void BeginTurn()
    {
        canMove = true;
        turn++;

        canChooseAction = true;

        Debug.Log("turning on animation - onTurn");

        gameObject.GetComponent<Animator>().SetBool("onTurn", true);

        Animator heroAnim = gameObject.GetComponent<Animator>();

        heroAnim.SetFloat("moveX", 0.0f);
        heroAnim.SetFloat("moveY", 0.0f);

        BattleCameraManager.instance.camState = camStates.HEROTURN;

        //Debug.Log(transform.gameObject.name + " starting turn " + turn);
    }

    /// <summary>
    /// Handles all methods for terminating turn to move forward with battle state machine for given hero
    /// </summary>
    /// <param name="HSM">HeroStateMachine for hero to end the turn</param>
    public void EndTurn(HeroStateMachine HSM)
    {
        RemoveSelectableTiles();
        canMove = false;
        
        HSM.ProcessStatusEffects(); //when adding ability for spells that affect allies, - MOVE THIS TO BASE MAGIC SCRIPT

        BSM.PerformList.RemoveAt(0); //remove this performer from the list in BSM

        HSM.RecoverMPAfterTurn(); //slowly recover MP based on spirit value

        BSM.pendingTurn = false;

        HSM.heroTurn++;

        HSM.targets.Clear();

        BSM.chosenTarget = null;

        HSM.ActionTarget = null;

        //BSM.battleState = battleStates.WAIT;
        BSM.battleState = battleStates.CHECKALIVE;
        //Debug.Log(transform.gameObject.name + " ending turn " + turn);

        Debug.Log("turning off animation - onTurn");
        gameObject.GetComponent<Animator>().SetBool("onTurn", false);

        BattleCameraManager.instance.camState = camStates.IDLE;
    }

    /// <summary>
    /// Handles terminating turn for unit
    /// </summary>
    public void EndTurn()
    {
        Debug.Log("ending turn");
        RemoveSelectableTiles();
        canMove = false;

        if (gameObject.tag == "Hero")
        {
            Debug.Log("turning off animation - onTurn");
            gameObject.GetComponent<Animator>().SetBool("onTurn", false);
        }
        BattleCameraManager.instance.camState = camStates.IDLE;
    }

    protected void ToggleMoveActionPanel(bool toggle)
    {
        Text actionText = GameObject.Find("BattleCanvas/BattleUI/MoveActionPanel/MoveActionSpacer/ActionButton/Text").GetComponent<Text>();
        Text defendText = GameObject.Find("BattleCanvas/BattleUI/MoveActionPanel/MoveActionSpacer/DefendButton/Text").GetComponent<Text>();
        if (toggle)
        {
            canChooseAction = true;
            actionText.color = new Color(1.0f, 1.0f, 1.0f);
            defendText.color = new Color(1.0f, 1.0f, 1.0f);
        } else
        {
            canChooseAction = false;
            actionText.color = new Color(0.5f, 0.5f, 0.5f);
            defendText.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
