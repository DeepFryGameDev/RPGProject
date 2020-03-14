using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    Vector2 curPos, lastPos; //for determining if player is moving

    private void Start()
    {
        if(GameManager.instance.nextSpawnPoint != "") //if no spawn point has been set
        {
            GameObject spawnPoint = GameObject.Find(GameManager.instance.nextSpawnPoint); //create spawn point gameobject
            transform.position = spawnPoint.transform.position; //sets player's position to the spawn point's position. -- this is where player will start the game --

            GameManager.instance.nextSpawnPoint = ""; //sets the next spawn point to blank (might not need this?) do some testing
        } else if (GameManager.instance.lastHeroPosition != Vector2.zero) //if lastHeroPosition has been set
        {
            transform.position = GameManager.instance.lastHeroPosition; //sets player's position to lastHeroPosition
            GameManager.instance.lastHeroPosition = Vector2.zero; //sets lastHeroPosition to 0 (might not need this? do some testing)
        }
    }
    
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //sets movement input vector2 horizontal to move along x axis
        movement.y = Input.GetAxisRaw("Vertical"); //sets movement input vector2 vertical to move along y axis

        animator.SetFloat("Horizontal", movement.x); //sets animator horizontal axis to movement.x
        animator.SetFloat("Vertical", movement.y); //sets animator vertical axis to movement.y
        animator.SetFloat("Speed", movement.sqrMagnitude); //sets animator speed to square magnitude of movement
    }



    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); //moves player's rigidbody based on movement input

        curPos = transform.position; //change current position to player's position
        if (curPos == lastPos) //if current position is the same as last position
        {
            GameManager.instance.isWalking = false; //player is not walking
        }
        else //if current position is different from last position
        {
            GameManager.instance.isWalking = true; //player is walking
        }
        lastPos = curPos; //sets last position to current position for tracking if player is moving
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Portal") //if entering a new scene transition collider
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>(); //initiates new collision handler as the transition's collision handler
            GameManager.instance.nextSpawnPoint = col.spawnPointName; //sets next spawn point to the collider's spawn point
            GameManager.instance.sceneToLoad = col.sceneToLoad; //sets scene to load to the collider's scene to be loaded
            //can add some scene transitioning animation here
            GameManager.instance.LoadScene(); //loads the appropriate scene
        }

        if (other.tag == "CombatRegion") //if entering a combat region
        {
            RegionData region = other.gameObject.GetComponent<RegionData>(); //creates a regionData and assigns it to the collider's regionData
            GameManager.instance.curRegion = region; //assigns the game manager's current region to this collider's region
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "CombatRegion" && GameManager.instance.isWalking) //if walking through a combat region
        {
            Debug.Log("In CombatRegion: " + other.name);

            GameManager.instance.canGetEncounter = true; //possible to begin an encounter
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "CombatRegion") //if exiting a combat region
        {
            Debug.Log("Exiting CombatRegion: " + other.name);

            GameManager.instance.curRegion = null; //remove current region
            GameManager.instance.canGetEncounter = false; //not possible to begin an encounter
        }
    }
}
