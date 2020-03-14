using UnityEngine;
using System.Collections;

public class PlayerController2D : MonoBehaviour {
	private bool moveEnabled = true; //Bool used to check if movement is enabled or not. Use ToggleMovement() to set.
	[Range (0.0f, 10.0f)]
	[Tooltip("The movement speed of the controller.")]
	public float moveSpeed = 5.0f;
	private Vector2 moveVector; //The vector used to apply movement to the controller.
	private float origSpeed; //Temp variable that stores the original speed upon start. Used to set speed back when running stops.
	private float moveSense = 0.2f; //An axis value above this is considered movement.

	private enum MoveState { Stand, Walk, Run } //States for standing, walking and running.
	private MoveState moveState	= MoveState.Stand; //Create and set a MoveState variable for the controller.
	private Animator anim; //The parent animator.

    Vector2 curPos, lastPos; //for determining if player is moving

    void Start()
    {
        origSpeed = moveSpeed;
        anim = transform.GetComponent<Animator>();

        {
            if (GameManager.instance.nextSpawnPoint != "") //if no spawn point has been set
            {
                GameObject spawnPoint = GameObject.Find(GameManager.instance.nextSpawnPoint); //create spawn point gameobject
                transform.position = spawnPoint.transform.position; //sets player's position to the spawn point's position. -- this is where player will start the game --

                GameManager.instance.nextSpawnPoint = ""; //sets the next spawn point to blank (might not need this?) do some testing
            }
            else if (GameManager.instance.lastHeroPosition != Vector2.zero) //if lastHeroPosition has been set
            {
                transform.position = GameManager.instance.lastHeroPosition; //sets player's position to lastHeroPosition
                GameManager.instance.lastHeroPosition = Vector2.zero; //sets lastHeroPosition to 0 (might not need this? do some testing)
            }
        }
    }

	/// <summary>
	/// Toggles the controller's movement.
	/// </summary>
	/// <param name="enable">If set to <c>true</c>, enable movement. If set to <c>false</c>, disable movement.</param>
	public void ToggleMovement(bool enable)
	{
		moveEnabled = enable;
	}

	void FixedUpdate()
	{
		if (moveEnabled == true)
		{
			if (moveVector.x > moveSense || moveVector.x < -moveSense || moveVector.y > moveSense || moveVector.y < -moveSense)
			{
				transform.Translate(moveVector * (moveSpeed / 100)); //If movement is enabled and any movement above the threshold (sense) is detected, move controller.
			}
		}

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

	void Update()
	{
		//Only check for movement if the movement bool is set to true.
		if (moveEnabled == true)
		{
			//Set the move vector to horizontal and vertical input axis values.
			moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			//If horizontal or vertical axis is above the threshold value (moveSense), set the move state to Walk.
			if (Input.GetAxisRaw("Horizontal") > moveSense || Input.GetAxisRaw("Horizontal") < (-moveSense) || Input.GetAxisRaw("Vertical") > moveSense || Input.GetAxisRaw("Vertical") < (-moveSense))
			{
				moveState = MoveState.Walk;

				//Pass the moveVector axes to the animators move variables and set animator's isMoving to true.
				anim.SetFloat("moveX", moveVector.x);
				anim.SetFloat("moveY", moveVector.y);
				anim.SetBool("isMoving", true);
			}
			else
			{
				//If there's no input, set the state to stand again and change Animator's isMoving to false.
				moveState = MoveState.Stand;

				anim.SetBool("isMoving", false);
			}

			if (Input.GetButton("Fire3") && moveState == MoveState.Walk)
			{
				//If the controller is moving and we're holding the run button, double the moveSpeed and change to Run state. Also tell animator to display running animation.
				moveSpeed = origSpeed * 2;
				moveState = MoveState.Run;

				anim.SetBool("isRunning", true);
			}
			else if (Input.GetButtonUp("Fire3") || moveState == MoveState.Stand)
			{
				//Set the speed and Animator running bool back when we're not running.
				moveSpeed = origSpeed;

				anim.SetBool("isRunning", false);
			}
		}
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
