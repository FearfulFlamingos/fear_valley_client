﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleUIControl : MonoBehaviour
{
    // Info Panel
    [SerializeField]
    private TMP_Text actionsPanelLeftHandText, actionsPanelRightHandText, attackPanelAttackInfo, attackPanelEnemyInfo;

    // Victory Panel
    [SerializeField]
    private TMP_Text victoryText, exitButtonText;

    // Panel References
    [SerializeField]
    private GameObject infoPanel, victoryPanel, stdActionPanel, magicActionPanel, attackPanel, magicInstructionPanel;

    #region Monobehavior
    private void Start()
    {
        
    }

    private void Update()
    {
        if (ActionPanelActive())
        {
            CheckForKeyboardInput();
        }
        else if (attackPanel.activeSelf)
        {
            AttackKeyboardInput();
        }
    }

    private void CheckForKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Move");
            gameObject.GetComponent<PlayerSpotlight>().ActivateMovement();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Attack");
            gameObject.GetComponent<PlayerSpotlight>().ActivateAttack();
            SwitchToAttackPanel();
        }
        else if (Input.GetKeyDown(KeyCode.E) && magicActionPanel.activeSelf)
        {
            Debug.Log("Magic");
            gameObject.GetComponent<PlayerSpotlight>().PlaceExplosion();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Retreat");
            gameObject.GetComponent<GameLoop>().Leave();

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Cancel");
            CancelButton();
        }
    }

    private void AttackKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<GameLoop>().Attack();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.GetComponent<GameLoop>().CancelAttack();
        }
    }

    private bool ActionPanelActive() => stdActionPanel.activeSelf || magicActionPanel.activeSelf;

    #endregion

    #region Panel toggling

    public void SwitchToAttackPanel()
    {
        stdActionPanel.SetActive(false);
        magicActionPanel.SetActive(false);
        attackPanel.SetActive(true);
    }

    public void CancelAttackPanel()
    {
        attackPanel.SetActive(false);
        ToggleActionPanel(true,
            gameObject.GetComponent<PlayerSpotlight>()
            .lastClicked.GetComponent<CharacterFeatures>().charclass);
        
    }

    public void ToggleActionPanel(bool state, string charClass="")
    {
        if (charClass == "Magic User")
        {
            magicActionPanel.SetActive(state);
        }
        else
        {
            stdActionPanel.SetActive(state);
        }
    }

    public void ToggleInfoPanel(bool state)
    {
        infoPanel.SetActive(state);   
    }

    /// <summary>
    /// Toggles all relevant panels based on whether the Magic Instructions should be visible.
    /// 
    /// </summary>
    /// <param name="state">Boolean value of whether magic instructions should be visible.</param>
    public void ToggleMagicInstructions(bool state)
    {
        magicInstructionPanel.SetActive(state);
        ToggleInfoPanel(!state);
        ToggleActionPanel(!state, "Magic User");
    }

    public void DeactivateAllPanels()
    {
        infoPanel.SetActive(false);
        victoryPanel.SetActive(false);
        stdActionPanel.SetActive(false);
        magicActionPanel.SetActive(false);
        attackPanel.SetActive(false);
        magicInstructionPanel.SetActive(false);
    }

    public void CancelButton()
    {
        gameObject.GetComponent<PlayerSpotlight>().DeactivateFocus(
                gameObject.GetComponent<PlayerSpotlight>().
                lastClicked.GetComponent<CharacterFeatures>());
    }

    public void ToggleVictoryPanel(bool state)
    {
        victoryPanel.SetActive(state);
    }

    #endregion

    #region Button assignment

    #endregion


    public void SetInfoPanelFriendlyText(string left, string right)
    {
        actionsPanelLeftHandText.SetText(left);
        actionsPanelRightHandText.SetText(right);
    }

    public void SetAttackPanelAttackerInfo(string text)
    {
        attackPanelAttackInfo.SetText(text);
    }

    public void SetAttackPanelEnemyInfo(string text)
    {
        attackPanelEnemyInfo.SetText(text);
    }
    
    public void SetVictoryPanelText(string text)
    {
        victoryText.SetText(text);
    }

}