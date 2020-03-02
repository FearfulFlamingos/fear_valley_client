﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerSpotlight : MonoBehaviour
{
    public Camera camera1;
    public GameObject lastClicked, board;
    public GameObject uiCanvas;
    public GameObject attackcanvas;
    public GameObject xbutton, attackbutton, cancelbutton;
    public TMP_Text leftText, rightText;
    public TMP_Text yourChar, attackChar;
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
    private void Update()
    {
        //Debug.Log("Frame");
        if (Input.GetMouseButtonDown(0) && Client.Instance.hasControl)
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
                    //try
                    //{
                    //    Debug.Log($"currentplayer {currentPlayer} == {referenceScript.team}?");
                    //    if (currentPlayer.currentPlayer == referenceScript.team)
                    //    {
                            
                            

                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Debug.Log($"Non-troop object selected;\n{ex}");
                    //}


                    // Populate Panel
                    // left string = 

                }
            }
        }



    }

    public void SpotlightChar(GameObject current)
    {
        CharacterFeatures referenceScript = current.GetComponent<CharacterFeatures>();
        GameLoop currentPlayer = scripts.GetComponent<GameLoop>();
        if (referenceScript.isFocused)
        {
            referenceScript.isFocused = false;
            uiCanvas.SetActive(false);
        }
        else
        {
            referenceScript.isFocused = true;
            uiCanvas.SetActive(true);

            leftText.text = $"Name: Roman\nAttack:+4\nAction Points:{currentPlayer.actionPoints}";
            rightText.text = $"Class: {referenceScript.charclass}\nDefense:13\nMovement:6";
        }
    }

    public void ActivateMovement()
    {
        //uiCanvas.SetActive(false);
        lastClicked.GetComponent<PlayerMovement>().enabled = true;
        lastClicked.GetComponent<PlayerMovement>().ActivateMovement();
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