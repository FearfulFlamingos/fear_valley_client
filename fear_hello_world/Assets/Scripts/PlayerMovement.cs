using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    private NavMeshAgent myAgent;
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        
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
                float hitY = hitInfo.point[2];
                float playX = gameObject.transform.position[0];
                float playY = gameObject.transform.position[2];
                float squaredX = (hitX - playX) * (hitX - playX);
                float squaredY = (hitY - playY) * (hitY - playY);
                float result = Mathf.Sqrt(squaredX + squaredY);

                Debug.Log(squaredX);
                Debug.Log(result);
                ///Debug.Log(myAgent.nextPosition[0]);
                if (result < 20) 
                    {
                    myAgent.SetDestination(hitInfo.point);
                }
                    
            }
        }

    }
}
