using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FacingState : MonoBehaviour
{
    public Sprite facingUp; //set the sprite for this gameObject to face up
    public Sprite facingDown; //set the sprite for this gameObject to face down
    public Sprite facingLeft; //set the sprite for this gameObject to face left
    public Sprite facingRight; //set the sprite for this gameObject to face right

    [System.NonSerialized] public Sprite defaultFacing; //for the gameObject's base sprite
    public string defaultDirection; //set which direction the base sprite is facing

    [System.NonSerialized] public bool forceDirection; //for keeping the player from changing facing direction

    [System.NonSerialized] public FaceState faceState; //the state of which direction they are facing

    // Start is called before the first frame update
    void Start()
    {
        defaultFacing = this.gameObject.GetComponent<SpriteRenderer>().sprite; //sets defaultFacing to base sprite
        faceState = FaceState.DEFAULT; //sets the facing state to default
        forceDirection = false; //sets force direction to false by default
    }

    void Update() //sets the facing direction sprites set above based on the facing state
    {
        if (!forceDirection)
        {
            switch (faceState)
            {
                case (FaceState.UP):
                    if (this.gameObject.GetComponent<SpriteRenderer>().sprite != facingUp)
                    {
                        this.gameObject.GetComponent<SpriteRenderer>().sprite = facingUp;
                    }
                    break;

                case (FaceState.DOWN):
                    if (this.gameObject.GetComponent<SpriteRenderer>().sprite != facingDown)
                    {
                        this.gameObject.GetComponent<SpriteRenderer>().sprite = facingDown;
                    }
                    break;

                case (FaceState.LEFT):
                    if (this.gameObject.GetComponent<SpriteRenderer>().sprite != facingLeft)
                    {
                        this.gameObject.GetComponent<SpriteRenderer>().sprite = facingLeft;
                    }
                    break;

                case (FaceState.RIGHT):
                    if (this.gameObject.GetComponent<SpriteRenderer>().sprite != facingRight)
                    {
                        this.gameObject.GetComponent<SpriteRenderer>().sprite = facingRight;
                    }
                    break;

                case (FaceState.DEFAULT):
                    if (this.gameObject.GetComponent<SpriteRenderer>().sprite != defaultFacing)
                    {
                        this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultFacing;
                    }
                    break;
            }
        }
    }
}
