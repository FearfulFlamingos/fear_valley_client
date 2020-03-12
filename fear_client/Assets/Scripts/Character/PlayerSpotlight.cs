using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerSpotlight : MonoBehaviour
{
    public Camera camera1;
    public GameObject lastClicked, board;
    public GameObject uiCanvas;
    public GameObject attackcanvas;
    public GameObject xbutton, attackbutton, cancelbutton;
    public GameObject battleUI;
    private GameObject scripts;

    private void Start()
    {
        camera1.gameObject.SetActive(true);
        uiCanvas.SetActive(false);
        attackcanvas.SetActive(false);
        scripts = GameObject.FindGameObjectWithTag("scripts");
        lastClicked = null;

    }

    // Update is called once per frame
    /// <summary>
    /// This function is constantly checking, what has been clicked on the screen. This is used
    /// by the player to select a figure which is on their team. Once the figure has been selected
    /// they are given a menu with options on what they would like to do with the figure. They
    /// select the option they would like to pursue by clicking the button on the menu which is
    /// linked to the functions below.
    /// </summary>
    private void Update()
    {
        //Debug.Log("Frame");
        if (Input.GetMouseButtonDown(0) && Client.Instance.hasControl)
        {
            RaycastHit hit;
            Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Physics.Raycast(ray, out hit, 100.0f));
            if (Physics.Raycast(ray, out hit, 100.0f,layerMask:1<<10))
            {

                if (hit.transform != null)
                {
                    GameLoop currentPlayer = scripts.GetComponent<GameLoop>();
                    GameObject gameObject = hit.transform.gameObject;
                    Debug.Log($"Last clicked: {lastClicked}");
                    if (lastClicked != null)
                    {
                        if (lastClicked != board)
                        {
                            Debug.Log("Last clicked != board");
                            lastClicked.GetComponent<CharacterFeatures>().isFocused = false;
                        }
                    }
                    if (currentPlayer != board)
                    {
                        Debug.Log("Currentplayer != board");
                        lastClicked = gameObject;
                        currentPlayer.lastClicked = gameObject;
                    }
                    else
                    {
                        Debug.Log("Currentplayer == board");
                        uiCanvas.SetActive(false);
                    }

                    CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
                    if (gameObject.GetComponent<CharacterFeatures>().team == 1)
                        SpotlightChar(gameObject);

                }
            }
        }



    }
    /// <summary>
    /// This functions is called after a viable object has been selected and it focuses the
    /// object and then populates the ui canvas. 
    /// </summary>
    /// <param name="current"></param>
    public void SpotlightChar(GameObject current)
    {
        CharacterFeatures referenceScript = current.GetComponent<CharacterFeatures>();
        GameLoop currentPlayer = scripts.GetComponent<GameLoop>();
        lastClicked = current;
        scripts.GetComponent<GameLoop>().lastClicked = current;
        if (referenceScript.isFocused)
        {
            referenceScript.isFocused = false;
            uiCanvas.SetActive(false);
        }
        else
        {
            referenceScript.isFocused = true;
            uiCanvas.SetActive(true);

            string leftText = $"Name: Roman\nAttack:+4\nAction Points:{currentPlayer.actionPoints}";
            string rightText = $"Class: {referenceScript.charclass}\nDefense:13\nMovement:6";
            gameObject.GetComponent<BfieldUIControl>().ChangeText(
                gameObject.GetComponent<BfieldUIControl>().actionsPanelLeftHandText, leftText);
            gameObject.GetComponent<BfieldUIControl>().ChangeText(
                 gameObject.GetComponent<BfieldUIControl>().actionsPanelRightHandText, rightText);
        }
    }
    /// <summary>
    /// This function is used after the move buttion is clicked and it activates the movement script
    /// </summary>
    public void ActivateMovement()
    {
        //uiCanvas.SetActive(false);
        lastClicked.GetComponent<PlayerMovement>().enabled = true;
        lastClicked.GetComponent<PlayerMovement>().ActivateMovement();
    }

    /// <summary>
    /// This function is used after the attack buttion is clicked and it activates the attack script
    /// </summary>
    public void ActivateAttack()
    {
        string text = $"You are attacking with: {lastClicked.GetComponent<CharacterFeatures>().charclass}";
        gameObject.GetComponent<BfieldUIControl>().ChangeText(
            gameObject.GetComponent<BfieldUIControl>().attackButtonText, text);

        uiCanvas.SetActive(false);
        attackcanvas.SetActive(true);
        xbutton.SetActive(false);
        lastClicked.GetComponent<PlayerAttack>().enabled = true;
        lastClicked.GetComponent<PlayerAttack>().ActivateAttack();
    }

    public void PlaceExplosion()
    {
        //lastClicked.GetComponent<PlayerMagic>().enabled = true;
        uiCanvas.SetActive(false);
        lastClicked.GetComponent<PlayerMagic>().PlaceExplosion();

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
