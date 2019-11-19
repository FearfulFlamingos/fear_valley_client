using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    public GameObject gameObject;
    public Camera camera1;
    public Text attackChar;
    public GameObject uiCanvas;
    private GameObject scripts;

    // Start is called before the first frame update
    void Start()
    {
        
        //uiCanvas = GameObject.FindGameObjectWithTag("PlayerAction");
        scripts = GameObject.FindGameObjectWithTag("scripts");
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
                    GameObject gameObject = hit.transform.gameObject;
                    
                    CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
                    GameLoop currentPlayer = scripts.GetComponent<GameLoop>();
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
    public void withinRange(int newPointX, int newPointZ, int curPointX, int curPointZ, bool ranged)
    {

        float floating = 0.2F;
        float playX = gameObject.transform.position[0];
        float playY = gameObject.transform.position[2];
        float squaredX = (newPointX - playX) * (newPointX - playX);
        float squaredY = (newPointZ - playY) * (newPointZ- playY);
        float result = Mathf.Sqrt(squaredX + squaredY);
    }
    public void activateAttack()
    {
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        GameObject Circle = referenceScript.myCircle;
        Circle.GetComponent<Renderer>().enabled = false;
        GameObject Circle2 = referenceScript.attackRange;
        Circle2.GetComponent<Renderer>().enabled = true;
    }
}

