using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool pathable = false;
    public bool inRange = false;
    public bool inAffect = false;

    public List<Tile> adjecencyList = new List<Tile>();

    //Needed BFS algorithm (Breadth First Search)
    public bool visited = false;
    public Tile parent = null;
    [HideInInspector] public int distance = 0;

    //For A*
    [HideInInspector] public float f = 0; //g + h
    [HideInInspector] public float g = 0; //cost from parent to current tile
    [HideInInspector] public float h = 0; //heuristic cost (cost from processed tile to destination)

    [HideInInspector] public Color selectableColor = Color.red;
    [HideInInspector] public Color pathableColor = Color.red;
    [HideInInspector] public Color targetColor = Color.gray;
    [HideInInspector] public Color currentColor = Color.magenta;
    [HideInInspector] public Color inRangeColor = Color.black;
    [HideInInspector] public Color baseColor = Color.white;
    [HideInInspector] public Color emptyAffectColor = Color.grey;
    [HideInInspector] public Color attackableColor = Color.blue;

    [HideInInspector] public List<Tile> tilesInRange = new List<Tile>();
    

    BattleStateMachine BSM;
    Pattern pattern = new Pattern();

    // Start is called before the first frame update
    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && !inAffect)
        {
            GetComponent<SpriteRenderer>().material.color = inRangeColor;
        }
        //else if (inRange && inAffect)
        //{
        //    GetComponent<SpriteRenderer>().material.color = attackableColor;
        //}
        else if (inAffect)
        {
            GetComponent<SpriteRenderer>().material.color = attackableColor; //set back to emptyAffectColor if needed
        }
        else if (current)
        {
            GetComponent<SpriteRenderer>().material.color = currentColor;
        }
        else if (target)
        {
            GetComponent<SpriteRenderer>().material.color = targetColor;
        }
        else if (pathable)
        {
            GetComponent<SpriteRenderer>().material.color = pathableColor;
        }
        //else if (selectable)
        //{
        //    GetComponent<SpriteRenderer>().material.color = selectableColor;
        //}
        else
        {
            GetComponent<SpriteRenderer>().material.color = baseColor;
        }
    }

    public void Reset()
    {
        adjecencyList.Clear();

        walkable = false;

        current = false;
        target = false;
        selectable = false;

        inAffect = false;
        inRange = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void ClearPathable()
    {
        pathable = false;
    }

    public void FindNeighbors(Tile target)
    {
        Reset();

        CheckTile(Vector2.up, target);
        CheckTile(Vector2.down, target);
        CheckTile(Vector2.right, target);
        CheckTile(Vector2.left, target);
    }

    public void CheckTile(Vector2 direction, Tile target)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + direction, new Vector2(.5f, .5f), 0);

        foreach (Collider2D item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();

            if (tile != null)
            {
                //Make these blockable when incorporating tiles that block movement.  Then it will not add these to adjency list.
                //Really I just need to add a new tag for "unwalkable" or "blockable" objects, and use the new tag below.
                RaycastHit2D[] walkableHits = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);

                foreach (RaycastHit2D checkIfWalkable in walkableHits)
                {
                    if (checkIfWalkable.collider.gameObject.tag == "Enemy" && BSM.HeroesToManage.Count > 0)
                    {
                        tile.walkable = false;
                    }
                    else
                    {
                        tile.walkable = true;
                    }
                }
                
                if (tile.walkable)
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(tile.transform.position, Vector3.forward, out hit, 1) || (tile == target))
                    {
                        adjecencyList.Add(tile);
                    }
                }
            }
        }
    }

    public void CheckStuff()
    {
        foreach (Tile tile in adjecencyList)
        {
            Debug.Log(tile.gameObject.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //shows enemy name on hover
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Tile")
                {
                    RaycastHit2D[] tilesHit = Physics2D.RaycastAll(hit.collider.gameObject.transform.position, Vector3.forward, 1);
                    foreach (RaycastHit2D target in tilesHit)
                    {
                        if (target.collider.gameObject.tag == "Enemy" && !BSM.targets.Contains(target.collider.gameObject))
                        {
                            GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>().text = target.collider.gameObject.GetComponent<EnemyStateMachine>().enemy.name;
                        }
                    }
                }
            }
        }

        //shows hero border and name on hover
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Tile")
                {
                    RaycastHit2D[] tilesHit = Physics2D.RaycastAll(hit.collider.gameObject.transform.position, Vector3.forward, 1);
                    foreach (RaycastHit2D target in tilesHit)
                    {
                        if (target.collider.gameObject.tag == "Hero" && !BSM.targets.Contains(target.collider.gameObject))
                        {
                            GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>().text = target.collider.gameObject.GetComponent<HeroStateMachine>().hero.name;

                            foreach (Transform child in GameObject.Find("BattleCanvas/HeroPanel/HeroPanelSpacer").transform)
                            {
                                string ID = target.collider.gameObject.name.Replace("BattleHero - ID ", "");
                                if (child.name.Replace("BattleHeroPanel - ID ","") == ID)
                                {
                                    HeroStateMachine HSM = target.collider.gameObject.GetComponent<HeroStateMachine>();
                                    HSM.HeroPanel.transform.Find("BorderCanvas").GetComponent<CanvasGroup>().alpha = 1.0f;
                                }
                            }

                        }
                    }
                }
            }
        }


        if (BSM.choosingTarget)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Tile")
                    {
                        if (hit.collider.gameObject.GetComponent<Tile>().inRange)
                        {
                            foreach (Tile tile in tilesInRange)
                            {
                                RaycastHit2D[] tilesHit = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);
                                foreach (RaycastHit2D target in tilesHit)
                                {
                                    if ((target.collider.gameObject.tag == "Enemy" || target.collider.gameObject.tag == "Hero"))
                                    {
                                        BSM.ShowSelector(target.collider.gameObject.transform.Find("Selector").gameObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //facilitates display of if the tile can be selected (if it is in the attack pattern and is in range)
            if (BSM.HeroChoice.chosenItem != null)
            {
                pattern.GetAffectPattern(this, 0);
            } else
            {
                pattern.GetAffectPattern(this, BSM.HeroChoice.chosenAttack.patternIndex);
            }
            tilesInRange = pattern.pattern;
            foreach (Tile tile in tilesInRange)
            {
                //Debug.Log(tile.gameObject.name + " is in range");
                tile.inAffect = true;
            }
            //Debug.Log(gameObject.name + " entered");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //resets enemy name on hover to blank when exiting tile
        GameObject.Find("BattleCanvas/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>().text = "";

        foreach (Transform child in GameObject.Find("BattleCanvas/HeroPanel/HeroPanelSpacer").transform)
        {
            child.transform.Find("BorderCanvas").GetComponent<CanvasGroup>().alpha = 0.0f;
        }

        if (BSM.choosingTarget)
        {
            GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject heroObj in heroes)
            {
                BSM.HideSelector(heroObj.transform.Find("Selector").gameObject);
            }

            foreach (GameObject enemyObj in enemies)
            {
                BSM.HideSelector(enemyObj.transform.Find("Selector").gameObject);
            }

            foreach (Tile tile in tilesInRange)
            {
                tile.inAffect = false;
            }
            //tilesInRange.Clear();
            //Debug.Log(gameObject.name + " exited");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (BSM.choosingTarget)
        {
            //this.inAffect = false;

            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Tile")
                    {
                        if (hit.collider.gameObject.GetComponent<Tile>().inRange)
                        {
                            BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().ActionTarget = hit.collider.gameObject; //sets the primary target for animation to occur on the tile clicked

                            foreach (Tile tile in tilesInRange)
                            {
                                RaycastHit2D[] tilesHit = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);
                                foreach (RaycastHit2D target in tilesHit)
                                {
                                    if ((target.collider.gameObject.tag == "Enemy" || target.collider.gameObject.tag == "Hero") && !BSM.targets.Contains(target.collider.gameObject))
                                    {
                                        //Debug.Log("adding " + target.collider.gameObject + " to targets");
                                        //BSM.targets.Add(target.collider.gameObject); //adds all objects inside target range to targets list to be affected
                                        BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().targets.Add(target.collider.gameObject);
                                    }
                                }
                            }

                            if (BSM.HeroesToManage[0].GetComponent<HeroStateMachine>().targets.Count > 0)
                            {
                                tilesInRange.Clear();
                                ClearTiles();
                            }
                        }
                    }
                }
            }
        }
    }

    void ClearTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tileObj in tiles)
        {
            Tile tile = tileObj.GetComponent<Tile>();
            tile.inAffect = false;
            tile.inRange = false;
        }
    }

    /// <summary>
    /// Returns the hero by given ID based on HeroDB
    /// </summary>
    /// <param name="ID">ID of hero needing to be returned</param>
    BaseHero GetHeroByID(int ID)
    {
        foreach (BaseHero hero in GameManager.instance.activeHeroes)
        {
            if (hero.ID == ID)
            {
                return hero;
            }
        }

        foreach (BaseHero hero in GameManager.instance.inactiveHeroes)
        {
            if (hero.ID == ID)
            {
                return hero;
            }
        }

        return null;
    }
}
