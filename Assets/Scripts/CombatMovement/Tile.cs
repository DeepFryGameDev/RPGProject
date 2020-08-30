using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Script is attached to all tile gameObjects
    //Explanation of variables are found on the Tactics Movement tutorial: https://www.youtube.com/watch?v=cX_KrK8RQ2o


    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool pathable = false;
    public bool inRange = false;
    public bool inAffect = false;

    public bool shieldable = false;
    public bool shielded = false;

    public bool settingShieldedComplete = false;

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
    
    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        UpdateShieldable(this);
    }
    
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

    /// <summary>
    /// Sets attached tile's variables to false and clears the adjency list for it
    /// Implemented here: https://youtu.be/cK2wzBCh9cg?t=1038
    /// </summary>
    public void Reset()
    {
        adjecencyList.Clear();

        walkable = false;

        current = false;
        target = false;
        selectable = false;

        inAffect = false;
        inRange = false;

        shielded = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    /// <summary>
    /// Sets pathable to false for attached tile
    /// </summary>
    public void ClearPathable()
    {
        pathable = false;
    }

    /// <summary>
    /// Calls CheckTile for tiles (up, down, left, and right) surrounding the attached tile
    /// Explanation found on tutorial: https://youtu.be/cK2wzBCh9cg?t=1076
    /// </summary>
    /// <param name="target">Target tile to check neighbor details</param>
    public void FindNeighbors(Tile target)
    {
        Reset();

        CheckTile(Vector2.up, target);
        CheckTile(Vector2.down, target);
        CheckTile(Vector2.right, target);
        CheckTile(Vector2.left, target);
    }

    /// <summary>
    /// Checks if tile should be walkable or if it is adjacent to attached tile
    /// Explanation found on tutorial: https://youtu.be/cK2wzBCh9cg?t=1115
    /// </summary>
    /// <param name="direction">Position of tile to check</param>
    /// <param name="target">Tile to check if it should be added to adjency list</param>
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
                RaycastHit2D[] tileHits = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);

                foreach (RaycastHit2D checkIfWalkable in tileHits)
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

                foreach (RaycastHit2D checkIfShieldable in tileHits)
                {
                    if (checkIfShieldable.collider.gameObject.tag == "Shieldable" && BSM.HeroesToManage.Count > 0)
                    {
                        //Debug.Log("Shieldable: " + checkIfShieldable.collider.gameObject.name);
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

    /// <summary>
    /// Test method
    /// </summary>
    public void CheckStuff()
    {
        foreach (Tile tile in adjecencyList)
        {
            Debug.Log(tile.gameObject.name);
        }
    }

    /// <summary>
    /// Processes methods when cusor enters attached tile
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BSM.centerTile == null)
        {
            BSM.centerTile = this.gameObject;
        }

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
                            GameObject.Find("BattleCanvas/BattleUI/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>().text = target.collider.gameObject.GetComponent<EnemyStateMachine>().enemy.name;
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
                            GameObject.Find("BattleCanvas/BattleUI/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>().text = target.collider.gameObject.GetComponent<HeroStateMachine>().hero.name;

                            foreach (Transform child in GameObject.Find("BattleCanvas/BattleUI/HeroPanel/HeroPanelSpacer").transform)
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
            //facilitates display of if the tile can be selected (if it is in the attack pattern and is in range)
            if (BSM.HeroChoice.chosenItem != null)
            {
                pattern.GetAffectPattern(this, 0); //gets pattern for 1 tile for item use
            }
            else
            {
                pattern.GetAffectPattern(this, BSM.HeroChoice.chosenAttack.patternIndex); //gets attack pattern
            }
            tilesInRange = pattern.pattern;
            foreach (Tile tile in tilesInRange)
            {
                //Debug.Log(tile.gameObject.name + " is in affect");
                RaycastHit2D[] shieldableTiles = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);
                foreach (RaycastHit2D target in shieldableTiles)
                {
                    if (target.collider.gameObject.tag != "Shieldable" && !tile.shielded)
                    {
                        tile.inAffect = true;
                    }
                    else
                    {
                        tile.inAffect = false;
                    }
                }

                CheckIfTileIsShielded(tile);
            }
            //Debug.Log(gameObject.name + " entered");

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
                                    if ((target.collider.gameObject.tag == "Enemy" || target.collider.gameObject.tag == "Hero") && tile.inAffect)
                                    {
                                        BSM.ShowSelector(target.collider.gameObject.transform.Find("Selector").gameObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Processes methods when cusor exits attached tile
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        //resets enemy name on hover to blank when exiting tile
        GameObject.Find("BattleCanvas/BattleUI/BattleDetailsPanel/BattleDetailsText").GetComponent<Text>().text = "";

        foreach (Transform child in GameObject.Find("BattleCanvas/BattleUI/HeroPanel/HeroPanelSpacer").transform)
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
                tile.shielded = false;
            }
            //tilesInRange.Clear();
            //Debug.Log(gameObject.name + " exited");
        }
    }

    /// <summary>
    /// Processes methods when cusor clicks on attached tile
    /// </summary>
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

                            BattleCameraManager.instance.parentTile = hit.collider.gameObject;

                            foreach (Tile tile in tilesInRange)
                            {
                                RaycastHit2D[] tilesHit = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);
                                foreach (RaycastHit2D target in tilesHit)
                                {
                                    if ((target.collider.gameObject.tag == "Enemy" || target.collider.gameObject.tag == "Hero") && !BSM.targets.Contains(target.collider.gameObject) && tile.inAffect)
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

    /// <summary>
    /// Sets all tiles inAffect and inRange to false
    /// </summary>
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

    public void CheckIfTileIsShielded(Tile tile)
    {
        StartCoroutine(GetShieldableTiles(tile));
    }

    public IEnumerator GetShieldableTiles(Tile tile)
    {
        settingShieldedComplete = false;

        //check each tile in each direction 1 tile - done
        //if tile is shieldable or shielded - done
        //get direction of that tile from center tile - done
        //check tile 1 over in that direction - done
        //if in affect - done
        //mark tile as shielded - done

        yield return new WaitForEndOfFrame();

        string dir = "null";
        Tile tileToCheck = null;

        //Debug.Log(BSM.centerTile.name);

        //up
        RaycastHit2D[] upHits = Physics2D.RaycastAll(tile.transform.position, Vector3.up, 1);
        foreach (RaycastHit2D target in upHits)
        {
            if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>() != tile && BSM.centerTile.GetComponent<Tile>().inRange)
            {
                if (target.collider.gameObject.GetComponent<Tile>().shieldable || target.collider.gameObject.GetComponent<Tile>().shielded)
                {
                    if (DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) != "null")
                    {
                        //Debug.Log(target.collider.gameObject.name + " is shieldable/shielded and is " + DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) + " from center tile");
                        tileToCheck = target.collider.gameObject.GetComponent<Tile>();
                        dir = DirectionFromCenterTile(tileToCheck);
                    }
                }
            }
        }
        //down
        RaycastHit2D[] downHits = Physics2D.RaycastAll(tile.transform.position, Vector3.down, 1);
        foreach (RaycastHit2D target in downHits)
        {
            if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>() != tile && BSM.centerTile.GetComponent<Tile>().inRange)
            {
                if (target.collider.gameObject.GetComponent<Tile>().shieldable || target.collider.gameObject.GetComponent<Tile>().shielded)
                {
                    if (DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) != "null")
                    {
                        //Debug.Log(target.collider.gameObject.name + " is shieldable/shielded and is " + DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) + " from center tile");
                        tileToCheck = target.collider.gameObject.GetComponent<Tile>();
                        dir = DirectionFromCenterTile(tileToCheck);
                    }
                }
            }
        }
        //left
        RaycastHit2D[] leftHits = Physics2D.RaycastAll(tile.transform.position, Vector3.left, 1);
        foreach (RaycastHit2D target in leftHits)
        {
            if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>() != tile && BSM.centerTile.GetComponent<Tile>().inRange)
            {
                if (target.collider.gameObject.GetComponent<Tile>().shieldable || target.collider.gameObject.GetComponent<Tile>().shielded)
                {
                    if (DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) != "null")
                    {
                        //Debug.Log(target.collider.gameObject.name + " is shieldable/shielded and is " + DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) + " from center tile");
                        tileToCheck = target.collider.gameObject.GetComponent<Tile>();
                        dir = DirectionFromCenterTile(tileToCheck);
                    }
                }
            }
        }
        //right
        RaycastHit2D[] rightHits = Physics2D.RaycastAll(tile.transform.position, Vector3.right, 1);
        foreach (RaycastHit2D target in rightHits)
        {
            if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>() != tile && BSM.centerTile.GetComponent<Tile>().inRange)
            {
                if (target.collider.gameObject.GetComponent<Tile>().shieldable || target.collider.gameObject.GetComponent<Tile>().shielded)
                {
                    if (DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) != "null")
                    {
                        //Debug.Log(target.collider.gameObject.name + " is shieldable/shielded and is " + DirectionFromCenterTile(target.collider.gameObject.GetComponent<Tile>()) + " from center tile");
                        tileToCheck = target.collider.gameObject.GetComponent<Tile>();
                        dir = DirectionFromCenterTile(tileToCheck);
                    }
                }
            }
        }

        if (dir != "null" && tileToCheck != null)
        {
            if (dir == "up" && tileToCheck.gameObject != BSM.centerTile)
            {
                //Debug.Log("checking 1 tile up from " + tileToCheck.gameObject.name);
                RaycastHit2D[] dirHits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.up, 1);
                foreach (RaycastHit2D target in dirHits)
                {
                    if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>().inAffect)
                    {
                        //Debug.Log("shielding " + target.collider.gameObject.name);
                        SetShielded(target.collider.gameObject);
                    }
                }
            }
            if (dir == "down" && tileToCheck.gameObject != BSM.centerTile)
            {
                //Debug.Log("checking 1 tile down from " + tileToCheck.gameObject.name);
                RaycastHit2D[] dirHits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.down, 1);
                foreach (RaycastHit2D target in dirHits)
                {
                    if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>().inAffect)
                    {
                        SetShielded(target.collider.gameObject);
                    }
                }
            }
            if (dir == "left" && tileToCheck.gameObject != BSM.centerTile)
            {
                //Debug.Log("checking 1 tile left from " + tileToCheck.gameObject.name);
                RaycastHit2D[] dirHits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.left, 1);
                foreach (RaycastHit2D target in dirHits)
                {
                    if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>().inAffect)
                    {
                        SetShielded(target.collider.gameObject);
                    }
                }
            }
            if (dir == "right" && tileToCheck.gameObject != BSM.centerTile)
            {
                //Debug.Log("checking 1 tile right from " + tileToCheck.gameObject.name);
                RaycastHit2D[] dirHits = Physics2D.RaycastAll(tileToCheck.transform.position, Vector3.right, 1);
                foreach (RaycastHit2D target in dirHits)
                {
                    if (target.collider.gameObject.tag == "Tile" && target.collider.gameObject.GetComponent<Tile>().inAffect)
                    {
                        SetShielded(target.collider.gameObject);
                    }
                }
            }
        }

        settingShieldedComplete = true;
    }

    void SetShielded(GameObject tileObj)
    {
        Debug.Log("shielding " + tileObj.name);
        tileObj.GetComponent<Tile>().shielded = true;
        tileObj.GetComponent<Tile>().inAffect = false;

        RaycastHit2D[] hits = Physics2D.RaycastAll(tileObj.transform.position, Vector3.forward, 1);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Hero")
            {
                BSM.HideSelector(hit.collider.gameObject.transform.Find("Selector").gameObject);
            }
        }
    }

    string DirectionFromCenterTile(Tile tileToCheck)
    {
        Vector3 centerTilePos = BSM.centerTile.gameObject.transform.position;
        Vector3 tileToCheckPos = tileToCheck.gameObject.transform.position;

        //Debug.Log(centerTilePos + " - " + tileToCheckPos);

        if (centerTilePos.x != tileToCheckPos.x && centerTilePos.y != tileToCheckPos.y)
        {
            return "null";
        }
        else
        {
            if (centerTilePos.x > tileToCheckPos.x)
            {
                //Debug.Log(BSM.centerTile.gameObject.name + " is right of " + tileToCheck.gameObject.name);
                return "left";
            }

            if (centerTilePos.x < tileToCheckPos.x)
            {
                //Debug.Log(BSM.centerTile.gameObject.name + " is left of " + tileToCheck.gameObject.name);
                return "right";
            }

            if (centerTilePos.y > tileToCheckPos.y)
            {
                //Debug.Log(BSM.centerTile.gameObject.name + " is up of " + tileToCheck.gameObject.name);
                return "down";
            }

            if (centerTilePos.y < tileToCheckPos.y)
            {
                //Debug.Log(BSM.centerTile.gameObject.name + " is down of " + tileToCheck.gameObject.name);
                return "up";
            }
        }

        return "null";
    }

    void UpdateShieldable(Tile tile)
    {
        RaycastHit2D[] tilesHit = Physics2D.RaycastAll(tile.transform.position, Vector3.forward, 1);
        foreach (RaycastHit2D target in tilesHit)
        {
            if (target.collider.gameObject.tag == "Shieldable")
            {
                Debug.Log("Setting " + tile.gameObject.name + " as shieldable");
                tile.shieldable = true;
            }
        }
    }
}
