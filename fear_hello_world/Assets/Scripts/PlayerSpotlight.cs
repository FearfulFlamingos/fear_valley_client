using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpotlight : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public GameObject lastClicked;
    public GameObject uiCanvas;
    public GameObject attackcanvas;
    public GameObject xbutton, attackbutton, cancelbutton;
    public Text leftText, rightText;
    public Text yourChar, attackChar;
    private GameObject scripts;

    private void Start()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
        uiCanvas.SetActive(false);
        attackcanvas.SetActive(false);
        scripts = GameObject.FindGameObjectWithTag("scripts");

    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("Frame");
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Physics.Raycast(ray, out hit, 100.0f));
            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                if (hit.transform != null)
                {
                    GameLoop currentPlayer = scripts.GetComponent<GameLoop>();
                    GameObject gameObject = hit.transform.gameObject;
                    lastClicked = gameObject;
                    currentPlayer.lastClicked = gameObject;
                    CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
                    
                    if (currentPlayer.currentPlayer == referenceScript.team)
                    {


                        referenceScript.isFocused = true;
                        Debug.Log(referenceScript.isFocused);
                        GameObject circle = referenceScript.myCircle;
                        var cubeRenderer = gameObject.GetComponent<Renderer>();
                        if (circle.GetComponent<Renderer>().enabled == false)
                        {
                            referenceScript.isFocused = true;
                            circle.GetComponent<Renderer>().enabled = true;
                            cubeRenderer.material.SetColor("_Color", Color.red);

                            uiCanvas.SetActive(true);
                            
                            leftText.text = $"Name: Roman\nAttack:+4\nAction Points:{currentPlayer.actionPoints}";
                            rightText.text = $"Class: {referenceScript.charclass}\nDefense:13\nMovement:6";

                        }
                        else
                        {
                            referenceScript.isFocused = false;
                            circle.GetComponent<Renderer>().enabled = false;
                            Color mycolor = new Color32(223, 210, 194, 255);
                            cubeRenderer.material.SetColor("_Color", mycolor);
                            uiCanvas.SetActive(false);
                        }
                    }


                    // Populate Panel
                    // left string = 

                }
            }
        }

        

    }


    public void ActivateMovement()
    {
        lastClicked.GetComponent<PlayerMovement>().enabled = true;
    }
    public void ActivateAttack()
    {
        yourChar.text = $"You are attacking with: {lastClicked.GetComponent<CharacterFeatures>().charclass}";
        uiCanvas.SetActive(false);
        attackcanvas.SetActive(true);
        xbutton.SetActive(false);
        lastClicked.GetComponent<PlayerAttack>().enabled = true;
        lastClicked.GetComponent<PlayerAttack>().ActivateAttack();
    }
    public void Leave()
    {
        if (scripts.GetComponent<GameLoop>().actionPoints >= 3)
        {
            uiCanvas.SetActive(false);
            Destroy(lastClicked.GetComponent<CharacterFeatures>().myCircle);
            Destroy(lastClicked.GetComponent<CharacterFeatures>().attackRange);
            Destroy(lastClicked);
            scripts.GetComponent<GameLoop>().actionPoints = 0;

        }
    }

    //private void switchCamera(int camPosition)
    //{
    //    if (camPosition > 1)
    //    {
    //        camPosition = 0;
    //    }

    //    PlayerPrefs.SetInt("CameraPosition", camPosition);
    //    if (camPosition == 0)
    //    {
    //        camera1.SetActive(true);
    //        camera2.SetActive(false);
    //    }
    //    if (camPosition == 1)
    //    {
    //        camera1.SetActive(false);
    //        camera2.SetActive(true);
    //    }
    }

