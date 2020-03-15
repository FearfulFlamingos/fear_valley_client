using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    public GameObject movementSelector;
    private Vector3 startingPosition;
    private NavMeshAgent myAgent;
    private GameObject scripts;


    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        scripts = GameObject.FindGameObjectWithTag("scripts");
    }
        
    
        
    // Update is called once per frame
    /// <summary>
    /// This function called once per frame and takes in the information upon the mouse click. Once the mouse is clicked
    /// it is processed to see if the hit point is valid. Once this is confirmed Moveme is called to move the game object,
    /// and then the movement is sent to the server. The function also takes away action points.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.GetComponent<CharacterFeatures>().team == 1)//TODO: && Client.Instance.hasControl)
        {
            CheckMovement();
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f, 1 << 9)) //Only look on ground layer
        {
            if (Vector3.Distance(hit.point, startingPosition) < gameObject.GetComponent<CharacterFeatures>().movement)
            {
                movementSelector.transform.position = hit.point;
            }

        }

    }

    private void CheckMovement()
    {
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();

        if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn))
        {
            if (Vector3.Distance(gameObject.transform.position, hitInfo.point) < referenceScript.movement)
            {
                GameLoop actionPoints = scripts.GetComponent<GameLoop>();
                actionPoints.actionPoints = System.Convert.ToInt32(actionPoints.actionPoints) - 1;
                scripts.GetComponent<PlayerSpotlight>().DeactivateFocus(referenceScript);

                MoveMe(hitInfo.point);
                DeactivateMovement();
                //TODO: Client.Instance.SendMoveData(gameObject.GetComponent<CharacterFeatures>().troopId, hitInfo.point[0], hitInfo.point[2]);
            }

        }
    }

    /// <summary>
    /// This function takes the new destination of the player and activates the agent to move the object there.
    /// </summary>
    /// <param name="newPos">This is the new vector where the object is put</param>
    public void MoveMe(Vector3 newPos) {
        if (gameObject.GetComponent<CharacterFeatures>().team == 2)
        {
            newPos.z = 8- newPos.z;
            newPos.x = 8 - newPos.x;
        }
        myAgent.SetDestination(newPos);
        Debug.Log(newPos);
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
    }

    /// <summary>
    /// This function stops PlayerSpotlight from selecting a new character. Everything else needed
    /// to activate movement is done before we get to this script.
    /// </summary>
    public void ActivateMovement()
    {        
        movementSelector = Instantiate(Resources.Load("MovementCursor")) as GameObject;
        startingPosition = transform.position;
        movementSelector.transform.position = startingPosition;

    }
    /// <summary>
    /// This function activates the player spotlight script and deactivates this script.
    /// </summary>
    public void DeactivateMovement()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().DeactivateFocus(
            gameObject.GetComponent<CharacterFeatures>());

        gameObject.GetComponent<PlayerMovement>().enabled = false;
        Destroy(movementSelector);
    }
}
