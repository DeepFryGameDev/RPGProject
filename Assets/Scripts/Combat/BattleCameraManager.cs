using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraManager : MonoBehaviour
{
    Camera cam;
    Vector3 targetPos;
    
    float toTargetTime = 3.0f;
    float zoomSpeed = 2.0f;

    public camStates camState;

    Vector3 homePos = new Vector3(-4.1f, 0.49f, -10.0f);
    float homeSize = 4.305343f;

    float targetZoomSize = 2.0f;

    float baseMoveTime = 7.5f;
    float baseZoomTime = 7.5f;

    float baseZ = -10f;

    float canvasAdj = -1.0f;

    BattleStateMachine BSM;

    #region Singleton
    public static BattleCameraManager instance; //call instance 

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of BattleCameraManager found!");
            return;
        }

        instance = this;

        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        homeSize = cam.orthographicSize;
        homePos = cam.transform.position;
    }
    #endregion

    private void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    void Update()
    {
        //Debug.Log("Cam state: " + camState);

        switch(camState)
        {
            //When transitioning into battle and loading the scene (during fade in)
            case (camStates.BATTLESTART):


            break;

            //Show the battlefield
            case (camStates.IDLE):
                
                if (cam.transform.position != homePos)
                {
                    Debug.Log("Moving camera: " + cam.transform.position);
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, homePos, baseMoveTime * Time.deltaTime);
                }

                if (cam.orthographicSize != homeSize)
                {
                    Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, homeSize, baseZoomTime * Time.deltaTime);
                }

            break;

            //When hero's turn begins / during movement phase
            case (camStates.HEROTURN):

                //Hover camera over unit, but don't zoom in. - Zooms out to home camera size if it isn't already
                if (cam.orthographicSize != homeSize)
                {
                    Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, homeSize, baseZoomTime * Time.deltaTime);
                }

                //Follow unit through movement phase
                if (cam.transform.position != new Vector3(BSM.HeroesToManage[0].transform.position.x, (BSM.HeroesToManage[0].transform.position.y + canvasAdj), baseZ))
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(BSM.HeroesToManage[0].transform.position.x, (BSM.HeroesToManage[0].transform.position.y + canvasAdj), baseZ), baseMoveTime * Time.deltaTime);
                }

                break;

            //When enemy's turn begins and they are moving - Zoom into enemy
            case (camStates.ENEMYTURN):


                //Zoom into enemy
                if (cam.orthographicSize != targetZoomSize)
                {
                    Debug.Log("Zooming camera: " + cam.orthographicSize);
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoomSize, baseZoomTime * Time.deltaTime);
                }

                //Follow unit through movement phase
                if (cam.transform.position != new Vector3(BSM.enemyToManage.transform.position.x, (BSM.enemyToManage.transform.position.y), baseZ))
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(BSM.enemyToManage.transform.position.x, (BSM.enemyToManage.transform.position.y), baseZ), baseMoveTime * Time.deltaTime);
                }

                break;

            //When hero can choose an action - Zoom into unit and slowly rotate camera
            case (camStates.CHOOSEACTION):


            break;

            //When hero can choose a unit for action - Zoom out to show range of tiles for attack, and follow cursor until unit/tile chosen
            case (camStates.CHOOSETARGET):


            break;

            //When hero has chosen unit and animation is triggered - If Magic, zoom into attacker for animation, then hover over each target.  If physical, zoom into center tile between attacker and target
            case (camStates.ATTACK):


            break;

            //When heroes have won the battle - Zoom into last hero to process attack
            case (camStates.VICTORY):


            break;

            //When heroes have lost the battle - Fade out to black and show restart options (not yet implemented)
            case (camStates.LOSS):


            break;
        }
    }
}
