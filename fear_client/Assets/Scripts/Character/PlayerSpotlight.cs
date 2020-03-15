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

    private void Start()
    {
        camera1.gameObject.SetActive(true);
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
        if (selectingCharacter )//TODO: && Client.Instance.hasControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask: 1 << 10)) // if you hit something
                {
                    GameLoop currentPlayerGameloop = scripts.GetComponent<GameLoop>();
                    GameObject selection = hit.transform.gameObject;
                    Debug.Log($"Last clicked: {lastClicked}");
                    if (lastClicked != null) // something has been selected before & we want to not focus it
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
                else // Nothing was hit by a ray on character level
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

    public void ActivateCharacterSelect(bool state)
    {
        selectingCharacter = state;
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
    /// Unfocuses a selected character and disables all panels.
    /// </summary>
    /// <param name="referenceScript">The CharacterFeatures script of the selected character.</param>
    public void DeactivateFocus(CharacterFeatures referenceScript)
    {
        referenceScript.isFocused = false;
        gameObject.GetComponent<BattleUIControl>().DeactivateAllPanels();
        lastClicked = null;
        gameObject.GetComponent<GameLoop>().lastClicked = null;
        selectingCharacter = true;
    }
    
    /// <summary>
    /// This function is used after the move buttion is clicked and it activates the movement script
    /// </summary>
    public void ActivateMovement()
    {
        //gameObject.GetComponent<BfieldUIControl>().ToggleInfoPanel(false);
        selectingCharacter = false;
        lastClicked.GetComponent<PlayerMovement>().enabled = true;
        lastClicked.GetComponent<PlayerMovement>().ActivateMovement();
    }

    /// <summary>
    /// This function is used after the attack buttion is clicked and it activates the attack script
    /// </summary>
    public void ActivateAttack()
    {
        string text = $"You are attacking with: {lastClicked.GetComponent<CharacterFeatures>().charclass}";
        gameObject.GetComponent<BattleUIControl>().SetAttackPanelAttackerInfo(text);
        gameObject.GetComponent<BattleUIControl>().SwitchToAttackPanel();


        lastClicked.GetComponent<PlayerAttack>().enabled = true;
        lastClicked.GetComponent<PlayerAttack>().ActivateAttack();
    }

    /// <summary>
    /// </summary>
    public void PlaceExplosion()
    {
        //lastClicked.GetComponent<PlayerMagic>().enabled = true;
        gameObject.GetComponent<BattleUIControl>().ToggleMagicInstructions(true);
        lastClicked.GetComponent<PlayerMagic>().PlaceExplosion();
    }
}
