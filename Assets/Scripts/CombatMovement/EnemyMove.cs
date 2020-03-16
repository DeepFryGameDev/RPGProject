using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : BaseMove
{
    protected GameObject target;
    protected bool readyForAction;
    public bool readyToAnimateAction;

    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
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
            if (Vector2.Distance(transform.position, target) >= 0.05f)
            {
                //Debug.Log("need to move");
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.rotation = Quaternion.Euler(Vector3.zero);
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
            //Debug.Log("end of path");
            readyForMove = false;
            EndTurn();
            //GetComponent<EnemyBehavior>().ChooseAction(); //enemy chooses random attack from their available attacks (this is where enemy behavior will likely need to go)
            readyForAction = true;
            transform.gameObject.GetComponent<EnemyStateMachine>().inMove = false;
        }
    }

    public void FindSelectableTiles()
    {
        //GetWalkableTiles();
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

            RaycastHit2D[] selectableHits = Physics2D.RaycastAll(t.transform.position, Vector3.forward, 1);
            
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

    protected void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        Debug.Log("Calculate path target: " + targetTile.gameObject.name);
        FindPath(targetTile);
    }
}
