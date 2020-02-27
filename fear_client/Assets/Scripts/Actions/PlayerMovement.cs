using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    private NavMeshAgent myAgent;
    public GameObject gameObject;
    private GameObject uiCanvas;
    private GameObject scripts;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        uiCanvas = GameObject.FindGameObjectWithTag("PlayerAction");
        scripts = GameObject.FindGameObjectWithTag("scripts");
    }
        
    
        
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Client.Instance.hasControl)
        {
            Debug.Log("Mouse button clicked");
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast (myRay, out hitInfo, 100, whatCanBeClickedOn) && gameObject.GetComponent<CharacterFeatures>().team == 1)
            {
                float hitX = hitInfo.point[0];
                float hitY = hitInfo.point[2];
                float floating = 0.2F;
                float playX = gameObject.transform.position[0];
                float playY = gameObject.transform.position[2];
                float squaredX = (hitX - playX) * (hitX - playX);
                float squaredY = (hitY - playY) * (hitY - playY);
                float result = Mathf.Sqrt(squaredX + squaredY);

                ///Debug.Log(myAgent.nextPosition[0]);
                if (result < 11) 
                    {
                    GameLoop actionPoints = scripts.GetComponent<GameLoop>();
                    actionPoints.actionPoints = System.Convert.ToInt32(actionPoints.actionPoints) - 1;
                    gameObject.GetComponent<CharacterFeatures>().isFocused = false;
                    MoveMe(hitInfo.point);
                    Client.Instance.SendMoveData(gameObject.GetComponent<CharacterFeatures>().troopId, hitInfo.point[0], hitInfo.point[2]);
                    DeactivateMovement();
                }
                    
            }
        }

    }
    public void MoveMe(Vector3 newPos) {
        if (gameObject.GetComponent<CharacterFeatures>().team == 2)
        {
            newPos.z = 8- newPos.z;
            newPos.x = 8 - newPos.x;
        }
        myAgent.SetDestination(newPos);
        Debug.Log(newPos);
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        GameObject Circle = referenceScript.myCircle;
        GameObject Circle2 = referenceScript.attackRange;
        Circle.transform.position = new Vector3(newPos[0], 0.2F, newPos[2]);
        Circle2.transform.position = new Vector3(newPos[0], 0.2F, newPos[2]);
    }
    public void ActivateMovement()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = false;

    }
    public void DeactivateMovement()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = true;
        uiCanvas.SetActive(false);
        gameObject.GetComponent<PlayerMovement>().enabled = false;
    }
}
