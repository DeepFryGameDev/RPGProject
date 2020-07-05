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
    /// Algorithm for enemy GameObject moving to target
    /// </summary>
    public void MoveToTarget()
    {
        //Debug.Log("Path count " + path.Count);
        if (path.Count > 0)
        {
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
                //Debug.Log("need to move");
                CalculateHeading(target);
                SetMoveVelocity();

                transform.forward = heading;
                transform.rotation = Quaternion.Euler(Vector3.zero); //fixes weird rotation behavior
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //tile center reached
                //Debug.Log("tile center reached");
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            Debug.Log("end of path");
            readyForMove = false;
            EndTurn();
            //GetComponent<EnemyBehavior>().ChooseAction(); //enemy chooses random attack from their available attacks (this is where enemy behavior will likely need to go)
            readyForAction = true;
            transform.gameObject.GetComponent<EnemyStateMachine>().inMove = false;
        }
    }

    /// <summary>
    /// Algorithm for enemy GameObject moving to a range without a target GameObject involved
    /// </summary>
    public void MoveToRange()
    {
        //Debug.Log("Path count " + path.Count);
        if (path.Count > 0)
        {
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
                //Debug.Log("need to move");
                CalculateHeading(target);
                SetMoveVelocity();

                transform.forward = heading;
                transform.rotation = Quaternion.Euler(Vector3.zero); //fixes weird rotation behavior
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //tile center reached
                //Debug.Log("tile center reached");
                if (AttackInRangeOfTarget(chosenTarget, chosenAttack))
                {
                    path.Clear();
                } else
                {
                    path.Pop();
                }
                transform.position = target;
            }
        }
        else
        {
            Debug.Log("end of path");
            readyForMove = false;
            EndTurn();
            //GetComponent<EnemyBehavior>().ChooseAction(); //enemy chooses random attack from their available attacks (this is where enemy behavior will likely need to go)
            readyForAction = true;
            transform.gameObject.GetComponent<EnemyStateMachine>().inMove = false;
        }
    }

    /// <summary>
    /// Returns if given attack is in range of given target.  Will possibly need to be adjusted later to allow off-centering of targets. ie target is not in selectable tiles, but still in range via affect pattern
    /// </summary>
    /// <param name="target">Target GameObject to check if in range</param>
    /// <param name="attack">Attack to gather range and pattern index to determine if target is in range of attack</param>
    protected bool AttackInRangeOfTarget(GameObject target, BaseAttack attack)
    {
        bool inRange = false;

        List<Tile> attackRange = pattern.GetRangePattern(GetCurrentTile(), attack.rangeIndex);

        foreach (Tile rangeTile in attackRange.ToArray())
        {
            //Debug.Log("RangeTile: " + rangeTile.gameObject.name);
            RaycastHit2D[] rangeTilesHit = Physics2D.RaycastAll(rangeTile.transform.position, Vector3.zero, 1);
            foreach (RaycastHit2D targetTile in rangeTilesHit)
            {
                //Debug.Log("TargetTile: " + targetTile.collider.gameObject.name);
                if (targetTile.collider.gameObject.tag == "Tile")
                {
                    //Debug.Log("Found tile here");
                    Tile thisTile = targetTile.collider.gameObject.GetComponent<Tile>();
                    List<Tile> affectPattern = pattern.GetAffectPattern(thisTile, attack.patternIndex);
                    foreach (Tile affectTile in affectPattern)
                    {
                        //Debug.Log("AffectTile: " + affectTile.gameObject.name);
                        RaycastHit2D[] targetsHit = Physics2D.RaycastAll(affectTile.transform.position, Vector3.forward, 1);
                        foreach (RaycastHit2D targettedTile in targetsHit)
                        {
                            //Debug.Log("TargettedTile: " + targettedTile.collider.gameObject.name + ", Target: " + target.name);
                            if ((targettedTile.collider.gameObject == target))
                            {
                                //Debug.Log(target + " is in range. Returning true");
                                return true;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log(target + " is NOT in range. Returning false");
        return inRange;
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
                if (hit.collider.gameObject.tag == "Hero" || hit.collider.gameObject.tag == "Enemy")
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
        //Set target tile to best tile to move to. if no tile available:
        if (targetTile == null)
        {
            targetTile = GetTargetTile(target);
        }
        
        //Debug.Log("Calculate path target: " + targetTile.gameObject.name);
        FindPath(targetTile);
    }
}
