using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern
{
    [HideInInspector] public List<Tile> pattern = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Tile> GetAffectPattern(Tile parentTile, int index)
    {
        pattern.Clear();

        if (index == 0) //single tile
        {
            pattern.Add(parentTile);
        }

        if (index == 1) //cross shaped
        {
            pattern.Add(parentTile);
            pattern.Add(UpTile(parentTile, 1));
            pattern.Add(DownTile(parentTile, 1));
            pattern.Add(LeftTile(parentTile, 1));
            pattern.Add(RightTile(parentTile, 1));
        }

        if (index == 2)
        {
            pattern.Add(parentTile);
            pattern.Add(UpTile(parentTile, 1));
            pattern.Add(DownTile(parentTile, 1));
            pattern.Add(LeftTile(parentTile, 1));
            pattern.Add(RightTile(parentTile, 1));
            pattern.Add(UpTile(parentTile, 2));
            pattern.Add(DownTile(parentTile, 2));
            pattern.Add(LeftTile(parentTile, 2));
            pattern.Add(RightTile(parentTile, 2));
            pattern.Add(UpLeftTile(parentTile, 1));
            pattern.Add(UpRightTile(parentTile, 1));
            pattern.Add(DownLeftTile(parentTile, 1));
            pattern.Add(DownRightTile(parentTile, 1));
        }


        return pattern;
    }

    public List<Tile> GetRangePattern(Tile parentTile, int index)
    {
        pattern.Clear();

        if (index == 0) //cross shaped
        {
            pattern.Add(parentTile);
            pattern.Add(UpTile(parentTile, 1));
            pattern.Add(DownTile(parentTile, 1));
            pattern.Add(LeftTile(parentTile, 1));
            pattern.Add(RightTile(parentTile, 1));
        }

        if (index == 1) //cross shaped without middle
        {
            pattern.Add(UpTile(parentTile, 1));
            pattern.Add(DownTile(parentTile, 1));
            pattern.Add(LeftTile(parentTile, 1));
            pattern.Add(RightTile(parentTile, 1));
        }

        if (index == 2) //diamond shaped
        {
            pattern.Add(parentTile);
            pattern.Add(UpTile(parentTile, 1));
            pattern.Add(DownTile(parentTile, 1));
            pattern.Add(LeftTile(parentTile, 1));
            pattern.Add(RightTile(parentTile, 1));
            pattern.Add(UpTile(parentTile, 2));
            pattern.Add(DownTile(parentTile, 2));
            pattern.Add(LeftTile(parentTile, 2));
            pattern.Add(RightTile(parentTile, 2));
            pattern.Add(UpLeftTile(parentTile, 1));
            pattern.Add(UpRightTile(parentTile, 1));
            pattern.Add(DownLeftTile(parentTile, 1));
            pattern.Add(DownRightTile(parentTile, 1));
        }

        return pattern;
    }

    Tile UpTile(Tile parentTile, int range)
    {
        //shoot a ray from the parent tile to tile(s) above parent tile.  if it lands on a space with a tile, and tile is not already in pattern list, AND it is the last tile hit, add it to pattern list.
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(parentTile.transform.position, Vector2.up, range);

        if (ReturnTile(hits) != null)
        {
            return ReturnTile(hits);
        } else
        {
            return parentTile;
        }
    }

    Tile DownTile(Tile parentTile, int range)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(parentTile.transform.position, Vector2.down, range);

        if (ReturnTile(hits) != null)
        {
            return ReturnTile(hits);
        }
        else
        {
            return parentTile;
        }
    }

    Tile LeftTile(Tile parentTile, int range)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(parentTile.transform.position, Vector2.left, range);

        if (ReturnTile(hits) != null)
        {
            return ReturnTile(hits);
        }
        else
        {
            return parentTile;
        }
    }

    Tile RightTile(Tile parentTile, int range)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(parentTile.transform.position, Vector2.right, range);

        if (ReturnTile(hits) != null)
        {
            return ReturnTile(hits);
        }
        else
        {
            return parentTile;
        }
    }

    Tile UpLeftTile(Tile parentTile, int range)
    {
        RaycastHit2D[] upHits = Physics2D.RaycastAll(parentTile.transform.position, Vector2.up, range);
        for (int i = 0; i < upHits.Length; i++)
        {
            RaycastHit2D upHit = upHits[i];
            RaycastHit2D[] leftHits = Physics2D.RaycastAll(upHit.collider.transform.position, Vector2.left, range);
            for (int j = 0; j < leftHits.Length; j++)
            {
                RaycastHit2D leftHit = leftHits[j];
                if (leftHit.collider.gameObject.tag == "Tile" && (!pattern.Contains(leftHit.collider.gameObject.GetComponent<Tile>())))
                {
                    //Debug.Log("returning " + leftHit.collider.gameObject);
                    return leftHit.collider.gameObject.GetComponent<Tile>();
                }
            }
        }
        return parentTile;
    }

    Tile UpRightTile(Tile parentTile, int range)
    {
        RaycastHit2D[] upHits = Physics2D.RaycastAll(parentTile.transform.position, Vector2.up, range);
        for (int i = 0; i < upHits.Length; i++)
        {
            RaycastHit2D upHit = upHits[i];
            RaycastHit2D[] rightHits = Physics2D.RaycastAll(upHit.collider.transform.position, Vector2.right, range);
            for (int j = 0; j < rightHits.Length; j++)
            {
                RaycastHit2D rightHit = rightHits[j];
                if (rightHit.collider.gameObject.tag == "Tile" && (!pattern.Contains(rightHit.collider.gameObject.GetComponent<Tile>())))
                {
                    //Debug.Log("returning " + rightHit.collider.gameObject);
                    return rightHit.collider.gameObject.GetComponent<Tile>();
                }
            }
        }
        return parentTile;
    }

    Tile DownLeftTile(Tile parentTile, int range)
    {
        RaycastHit2D[] downHits = Physics2D.RaycastAll(parentTile.transform.position, Vector2.down, range);
        for (int i = 0; i < downHits.Length; i++)
        {
            RaycastHit2D downHit = downHits[i];
            RaycastHit2D[] leftHits = Physics2D.RaycastAll(downHit.collider.transform.position, Vector2.left, range);
            for (int j = 0; j < leftHits.Length; j++)
            {
                RaycastHit2D leftHit = leftHits[j];
                if (leftHit.collider.gameObject.tag == "Tile" && (!pattern.Contains(leftHit.collider.gameObject.GetComponent<Tile>())))
                {
                    //Debug.Log("returning " + leftHit.collider.gameObject);
                    return leftHit.collider.gameObject.GetComponent<Tile>();
                }
            }
        }
        return parentTile;
    }

    Tile DownRightTile(Tile parentTile, int range)
    {
        RaycastHit2D[] downHits = Physics2D.RaycastAll(parentTile.transform.position, Vector2.down, range);
        for (int i = 0; i < downHits.Length; i++)
        {
            RaycastHit2D downHit = downHits[i];
            RaycastHit2D[] rightHits = Physics2D.RaycastAll(downHit.collider.transform.position, Vector2.right, range);
            for (int j = 0; j < rightHits.Length; j++)
            {
                RaycastHit2D rightHit = rightHits[j];
                if (rightHit.collider.gameObject.tag == "Tile" && (!pattern.Contains(rightHit.collider.gameObject.GetComponent<Tile>())))
                {
                    //Debug.Log("returning " + rightHit.collider.gameObject);
                    return rightHit.collider.gameObject.GetComponent<Tile>();
                }
            }
        }
        return parentTile;
    }

    Tile ReturnTile(RaycastHit2D[] hits)
    {
        List<Tile> tilesInRaycast = new List<Tile>();

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (hit.collider.gameObject.tag == "Tile" && (!pattern.Contains(hit.collider.gameObject.GetComponent<Tile>())))
            {
                tilesInRaycast.Add(hit.collider.gameObject.GetComponent<Tile>());
            }
        }

        for (int i = 0; i < tilesInRaycast.Count; i++)
        {
            if (i == tilesInRaycast.Count - 1)
            {
                //Debug.Log("returning " + tilesInRaycast[i].gameObject.name);
                return tilesInRaycast[i];
            }
        }

        return null;
    }

}
