using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerSpotlight : MonoBehaviour
{
    public Camera camera1;
    public GameObject lastClicked, board;
    public GameObject battleUI;
    private GameObject scripts;
    private bool selectingCharacter = true;
    public bool testing;

    #region Monobehavior
    private void Start()
    {
        camera1.gameObject.SetActive(true);
        scripts = GameObject.FindGameObjectWithTag("scripts");
        lastClicked = null;

    }

    // This function is constantly checking, what has been clicked on the screen. This is used
    // by the player to select a figure which is on their team. Once the figure has been selected
    // they are given a menu with options on what they would like to do with the figure. They
    // select the option they would like to pursue by clicking the button on the menu which is
    // linked to the functions below.
    private void Update()
    {
        if (selectingCharacter && Client.Instance.hasControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
                // if you hit something
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask: 1 << 10)) 
                {
                    GameLoop currentPlayerGameloop = scripts.GetComponent<GameLoop>();
                    GameObject selection = hit.transform.gameObject;
                    Debug.Log($"Last clicked: {lastClicked}");
                    
                    // something has been selected before & we want to not focus it
                    if (lastClicked != null) 
                    {
                        Debug.Log("Unfocusing last character");
                        DeactivateFocus(lastClicked.GetComponent<CharacterFeatures>());
                    }
                    else
                    {
                        lastClicked = selection;
                        Debug.Log($"Last clicked object: {selection.gameObject.name}");
                        
                        if (selection.GetComponent<CharacterFeatures>().team == 1)
                        {
                            currentPlayerGameloop.lastClicked = selection;
                            SpotlightChar(selection);
                        }
                    }
                }
                // Nothing was hit by a ray on character level
                else
                {
                    if (Physics.Raycast(ray, out hit, 100.0f, layerMask: 1 <<9) && lastClicked != null)
                    {
                        Debug.Log("Unselecting characters");
                        DeactivateFocus(lastClicked.GetComponent<CharacterFeatures>());
                        lastClicked = null;
                    }
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Toggles whether or not the LMB should select a character.
    /// </summary>
    /// <param name="state">Desired state of selection.</param>
    public void ActivateCharacterSelect(bool state)
    {
        selectingCharacter = state;
    }

    /// <summary>
    /// This functions is called after a viable object has been selected and it focuses the
    /// object and then populates the ui canvas. 
    /// </summary>
    /// <param name="current">Selected character.</param>
    public void SpotlightChar(GameObject current)
    {
        CharacterFeatures referenceScript = current.GetComponent<CharacterFeatures>();
        GameLoop currentPlayer = scripts.GetComponent<GameLoop>();
        scripts.GetComponent<GameLoop>().lastClicked = current;
        if (referenceScript.isFocused)
        {
            DeactivateFocus(referenceScript);
        }
        else
        {
            referenceScript.isFocused = true;
            gameObject.GetComponent<BattleUIControl>().ToggleInfoPanel(true);
            gameObject.GetComponent<BattleUIControl>().ToggleActionPanel(true, referenceScript.charclass);

            string leftText = $"Name: Roman\nAttack:+4\nAction Points:{currentPlayer.actionPoints}";
            string rightText = $"Class: {referenceScript.charclass}\nDefense:13\nMovement:6";
            gameObject.GetComponent<BattleUIControl>().SetInfoPanelFriendlyText(leftText, rightText);
        }
    }
    
    /// <summary>
    /// Unfocuses a selected character.
    /// </summary>
    /// <param name="referenceScript">The CharacterFeatures component of the selected character.</param>
    public void DeactivateFocus(CharacterFeatures referenceScript)
    {
        referenceScript.isFocused = false;
        gameObject.GetComponent<BattleUIControl>().SetAttackPanelAttackerInfo("");
        gameObject.GetComponent<BattleUIControl>().SetAttackPanelEnemyInfo("");

        lastClicked = null;
        gameObject.GetComponent<GameLoop>().lastClicked = null;
        selectingCharacter = true;
    }
    
    /// <summary>
    /// This function is used after the move buttion is clicked and it activates the movement script
    /// </summary>
    public void ActivateMovement()
    {
        selectingCharacter = false;
        lastClicked.GetComponent<PlayerMovement>().enabled = true;
        lastClicked.GetComponent<PlayerMovement>().ActivateMovement();
    }

    /// <summary>
    /// </summary>
    public void PlaceExplosion()
    {
        //lastClicked.GetComponent<PlayerMagic>().enabled = true;
        selectingCharacter = false;
        lastClicked.GetComponent<PlayerMagic>().PlaceExplosion();
    }
}
