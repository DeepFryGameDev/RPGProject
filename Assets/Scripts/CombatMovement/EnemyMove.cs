using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : BaseMove
{
    protected GameObject target;
    protected bool readyForAction;
    public bool readyToAnimateAction;

    //for finding which targets are in range
    protected List<GameObject> targetsInRange = new List<GameObject>();
    protected Pattern pattern = new Pattern();
    protected List<Tile> tilesInRange = new List<Tile>();

    protected BaseAttack chosenAttack;
    protected GameObject chosenTarget;
    protected GameObject actionTarget;
    protected List<GameObject> targets = new List<GameObject>();

    Vector3 lastPos;

    protected Tile startingTile;

    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Algorithm for enemy GameObject moving to target tile
    /// </summary>
    public void MoveToTarget()
    {
        if (path.Count > 0)
        {
            ProcessWalkingAnimation();

            //Debug.Log("found path");
            Tile t = path.Peek();
            Vector3 target = t.transform.position;
            target.z = 1;

            //Debug.Log("transform position: " + transform.position);
            //Debug.Log("target position: " + target);
            //calculate the unit's position on top of the target tile
            //target.z = target.z + t.GetComponent<Collider>().bounds.extents.z;

            if (Vector2.Distance(transform.position, target) >= 0.05f)
            {
                //Debug.Log("need to move to " + target);
                CalculateHeading(target);
                SetMoveVelocity();

                transform.forward = heading;
                transform.rotation = Quaternion.Euler(Vector3.zero); //fixes weird rotation behavior
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //tile center reached
                Debug.Log("tile center reached");
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            Debug.Log("end of path");
            readyForMove = false;
            EndTurn();

            readyForAction = true;
            transform.gameObject.GetComponent<EnemyStateMachine>().inMove = false;
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
            Animator enemyAnim = gameObject.GetComponent<Animator>();

            enemyAnim.SetFloat("moveX", 0.0f);
            enemyAnim.SetFloat("moveY", 0.0f);

            if (lastPos.x != newPos.x)
            {
                float xDiff = lastPos.x - newPos.x;
                if (xDiff > 0)
                {
                    //Debug.Log("moving left");
                    enemyAnim.SetFloat("moveX", -1.0f);
                }
                else
                {
                    //Debug.Log("moving right");
                    enemyAnim.SetFloat("moveX", 1.0f);
                }
            }

            if (lastPos.y != newPos.y)
            {
                float yDiff = lastPos.y - newPos.y;
                if (yDiff > 0)
                {
                    //Debug.Log("moving down");
                    enemyAnim.SetFloat("moveY", -1.0f);
                }
                else
                {
                    //Debug.Log("moving up");
                    enemyAnim.SetFloat("moveY", 1.0f);
                }
            }
        }

        if (lastPos != null && lastPos != newPos)
        {
            lastPos = newPos;
        }
    }

    /// <summary>
    /// Returns current tile for attached enemy GameObject
    /// </summary>
    new protected Tile GetCurrentTile()
    {
        RaycastHit2D[] tileHits = Physics2D.RaycastAll(transform.position, Vector3.back, 1);

        foreach (RaycastHit2D tile in tileHits)
        {
            if (tile.collider.gameObject.tag == "Tile")
            {
                currentTile = tile.collider.gameObject.GetComponent<Tile>();
                currentTile.current = true;
                return tile.collider.gameObject.GetComponent<Tile>();
            }
        }

        Debug.Log("Current tile not found, returning null");
        return null;
    }

    /// <summary>
    /// Detects which tiles can be selected for enemy to move
    /// Primary function of BFS algorithm - explanation on tutorial: https://youtu.be/2NVEqBeXdBk?t=760
    /// </summary>
    public void FindSelectableTiles()
    {
        GetWalkableTiles();
        ComputeAdjacencyLists(null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = ?? leave as null

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            if (!IfShieldable(t))
            {
                selectableTiles.Add(t);
                t.pathable = true;

                if (t.distance < move)
                {
                    foreach (Tile tile in t.adjecencyList)
                    {
                        //Debug.Log("adjencencyList - " + tile);
                        if (!tile.visited)
                        {
                            //Debug.Log("tile not visited");
                            tile.parent = t;
                            tile.visited = true;
                            tile.distance = 1 + t.distance;
                            process.Enqueue(tile);
                        }
                    }
                }
            }
            /*selectableTiles.Add(t);
            t.pathable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjecencyList)
                {
                    //Debug.Log("adjencencyList - " + tile);
                    if (!tile.visited)
                    {
                        //Debug.Log("tile not visited");
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }*/
        }
    }

    /// <summary>
    /// Sets all selectable tiles that aren't held by a hero or enemy to walkable
    /// </summary>
    void GetWalkableTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject obj in tiles)
        {
            RaycastHit2D[] selectableHits = Physics2D.RaycastAll(obj.transform.position, Vector3.forward, 1);
            foreach (RaycastHit2D hit in selectableHits)
            {
                if (hit.collider.gameObject.tag == "Hero" || hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Shieldable")
                {
                    obj.GetComponent<Tile>().walkable = false;
                    break;
                } else if (hit.collider.gameObject.tag == "Tile")
                {
                    obj.GetComponent<Tile>().walkable = true;
                }
            }
        }
    }

    /// <summary>
    /// Gets target tile for enemy's target, then finds the path to that tile
    /// </summary>
    protected void CalculatePath(Tile targetTile)
    {        
        //Debug.Log("Calculate path target: " + targetTile.gameObject.name);
        FindPath(targetTile);
    }
}
