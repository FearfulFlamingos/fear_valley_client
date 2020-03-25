using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Scripts.Networking;
using Scripts.Character;

namespace Scripts.Controller
{
    public class PlayerSpotlight : MonoBehaviour, IPlayerSpotlight
    {
        public Camera camera1;
        public GameObject lastClicked, board;
        public GameObject battleUI;
        private bool selectingCharacter = true;
        public bool testing;

        private BattleUIControl uiController;
        private GameLoop gameLoop;

        #region Monobehavior
        private void Start()
        {
            camera1.gameObject.SetActive(true);
            uiController = gameObject.GetComponent<BattleUIControl>();
            gameLoop = gameObject.GetComponent<GameLoop>();
            lastClicked = null;

        }

        // This function is constantly checking, what has been clicked on the screen. This is used
        // by the player to select a figure which is on their team. Once the figure has been selected
        // they are given a menu with options on what they would like to do with the figure. They
        // select the option they would like to pursue by clicking the button on the menu which is
        // linked to the functions below.
        private void Update()
        {
            if (Client.Instance.HasControl() && selectingCharacter && InputManager.GetLeftMouseClick()  )
            {
                RaycastHit hit;
                Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask: 1 << 10))
                {
                    CheckSelection(hit.transform.gameObject, lastClicked);
                }
                else
                {
                    // Nothing was hit by a ray on character level
                    if (Physics.Raycast(ray, out hit, 100.0f, layerMask: 1 << 9) && lastClicked != null)
                    {
                        Debug.Log("Unselecting characters");
                        DeactivateCurrentFocus();
                        lastClicked = null;
                    }
                }
            }
        }

        #endregion

        #region Spotlight Checking

        void CheckSelection(object clicked, object lastClicked)
        {
            GameObject selection = (GameObject)clicked;
            Debug.Log($"Last clicked: {lastClicked}");

            // something has been selected before & we want to not focus it
            // but also focus the new one
            if (lastClicked != null)
            {
                Debug.Log("Unfocusing last character");
                DeactivateCurrentFocus();

                Debug.Log("Focusing new character");
                lastClicked = selection;
                Debug.Log($"Last clicked object: {selection.gameObject.name}");

                if (selection.GetComponent<CharacterFeatures>().Team == 1)
                {
                    gameLoop.lastClicked = selection;
                    SpotlightChar(selection);
                }
            }
            else
            {
                lastClicked = selection;
                Debug.Log($"Last clicked object: {selection.gameObject.name}");

                if (selection.GetComponent<CharacterFeatures>().Team == 1)
                {
                    gameLoop.lastClicked = selection;
                    SpotlightChar(selection);
                }
            }
        }


        #endregion
        /// <summary>
        /// Toggles whether or not the LMB should select a character.
        /// </summary>
        /// <param name="state">Desired state of selection.</param>
        public void SetCharacterSelect(bool state)
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
            gameLoop.lastClicked = current;
            if (referenceScript.IsFocused)
            {
                DeactivateCurrentFocus();
            }
            else
            {
                //referenceScript.isFocused = true;
                ChangeFocusedColor(Color.red);
                uiController.ToggleInfoPanel(true);
                uiController.ToggleActionPanel(true, referenceScript.Charclass);

                string leftText = $"Name: Roman\nAttack:+4\nAction Points:{gameLoop.actionPoints}";
                string rightText = $"Class: {referenceScript.Charclass}\nDefense:13\nMovement:6";
                uiController.SetInfoPanelFriendlyText(leftText, rightText);
            }
        }

        /// <summary>
        /// Unfocuses the selected character.
        /// </summary>
        /// <param name="referenceScript">The CharacterFeatures component of the selected character.</param>
        public void DeactivateCurrentFocus()
        {
            ChangeFocusedColor(223, 210, 194, 255);
            CharacterFeatures referenceScript = lastClicked.GetComponent<CharacterFeatures>();
            referenceScript.IsFocused = false;
            uiController.SetAttackPanelAttackerInfo("");
            uiController.SetAttackPanelEnemyInfo("");
            uiController.DeactivateAllPanels();

            lastClicked = null;
            gameLoop.lastClicked = null;
            selectingCharacter = true;
        }

        private void ChangeFocusedColor(Color color)
        {
            lastClicked.GetComponent<Renderer>().material.SetColor("_Color", color);
        }

        private void ChangeFocusedColor(byte red, byte green, byte blue, byte alpha)
        {
            lastClicked.GetComponent<Renderer>().material.SetColor("_Color", new Color32(red, green, blue, alpha));
        }
    }

}