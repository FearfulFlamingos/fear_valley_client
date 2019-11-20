using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    public GameObject gameObject;
    private Camera camera1;
    private Text attackChar;
    private GameObject uiCanvas;
    private GameObject scripts;

    // Start is called before the first frame update
    void Start()
    {
        
        //uiCanvas = GameObject.FindGameObjectWithTag("PlayerAction");
        scripts = GameObject.FindGameObjectWithTag("scripts");
        attackChar = scripts.GetComponent<PlayerSpotlight>().attackChar;
        uiCanvas = scripts.GetComponent<PlayerSpotlight>().attackcanvas;
        camera1 = scripts.GetComponent<PlayerSpotlight>().camera1;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Physics.Raycast(ray, out hit, 100.0f));
            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                if (hit.transform != null)
                {
                    GameObject newObject = hit.transform.gameObject;
                    
                    CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
                    GameLoop currentPlayer = scripts.GetComponent<GameLoop>();
                    Debug.Log(WithinRange(3.5F, newObject.transform.position[0], newObject.transform.position[2], gameObject.transform.position[0], gameObject.transform.position[2], false));
                    //if (currentPlayer.currentPlayer == referenceScript.team)
                    //{


                    //    referenceScript.isFocused = true;
                    //    Debug.Log(referenceScript.isFocused);
                    //    GameObject circle = referenceScript.myCircle;
                    //    var cubeRenderer = gameObject.GetComponent<Renderer>();
                    //    if (circle.GetComponent<Renderer>().enabled == false)
                    //    {
                    //        referenceScript.isFocused = true;
                    //        circle.GetComponent<Renderer>().enabled = true;
                    //        cubeRenderer.material.SetColor("_Color", Color.red);

                    //        uiCanvas.SetActive(true);
                    //        leftText.text = $"Name: Roman\nAttack:+4\nAction Points:{currentPlayer.actionPoints}";
                    //        rightText.text = $"Class: {referenceScript.charclass}\nDefense:13\nMovement:6";

                    //    }
                    //    else
                    //    {
                    //        referenceScript.isFocused = false;
                    //        circle.GetComponent<Renderer>().enabled = false;
                    //        Color mycolor = new Color32(223, 210, 194, 255);
                    //        cubeRenderer.material.SetColor("_Color", mycolor);
                    //        uiCanvas.SetActive(false);
                    //    }
                    //}


                    // Populate Panel
                    // left string = 

                }
            }
        }

    }
    public bool WithinRange(float maxDistance, float newPointX, float newPointZ, float curPointX, float curPointZ, bool ranged)
    {

        float squaredX = (newPointX - curPointX) * (newPointX - curPointX);
        float squaredZ = (newPointZ - curPointZ) * (newPointZ - curPointZ);
        float result = Mathf.Sqrt(squaredX + squaredZ);
        Debug.Log("IAMHERE");
        Debug.Log(result);
        if (maxDistance >= result)
        {
            return true;
        }

        return false;
    }
    public void ActivateAttack()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = false;
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        GameObject Circle = referenceScript.myCircle;
        Circle.GetComponent<Renderer>().enabled = false;
        GameObject Circle2 = referenceScript.attackRange;
        Circle2.GetComponent<Renderer>().enabled = true;
        
    }
}

