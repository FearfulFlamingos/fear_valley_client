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
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse button clicked");
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast (myRay, out hitInfo, 100, whatCanBeClickedOn))
            {
                float hitX = hitInfo.point[0];
                Debug.Log($"hitX {hitX}");
                float hitY = hitInfo.point[2];
                Debug.Log($"hitY {hitY}");
                float floating = 0.2F;
                float playX = gameObject.transform.position[0];
                Debug.Log($"playX {playX}");
                float playY = gameObject.transform.position[2];
                Debug.Log($"play Y {playY}");
                float squaredX = (hitX - playX) * (hitX - playX);
                Debug.Log($"squaredX {squaredX}");
                float squaredY = (hitY - playY) * (hitY - playY);
                Debug.Log($"squaredY {squaredY}");
                float result = Mathf.Sqrt(squaredX + squaredY);
                Debug.Log($"result: {result}");
                Debug.Log("Hello");
                Debug.Log(hitX);
                ///Debug.Log(myAgent.nextPosition[0]);
                if (result < 11) 
                    {
                    GameLoop actionPoints = scripts.GetComponent<GameLoop>();
                    actionPoints.actionPoints = System.Convert.ToInt32(actionPoints.actionPoints) - 1;
                    gameObject.GetComponent<CharacterFeatures>().isFocused = false;
                    MoveMe(hitInfo.point);
                    scripts.GetComponent<Client>().SendMoveData(gameObject.GetComponent<CharacterFeatures>().troopId, hitInfo.point[0], hitInfo.point[2]);
                    DeactivateMovement();
                }
                    
            }
        }

    }
    public void MoveMe(Vector3 newPos) {
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
