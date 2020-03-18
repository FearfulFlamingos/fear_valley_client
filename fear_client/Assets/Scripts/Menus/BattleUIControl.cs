using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleUIControl : MonoBehaviour
{
    // Info Panel
    [SerializeField]
    private TMP_Text actionsPanelLeftHandText;
    [SerializeField]
    private TMP_Text actionsPanelRightHandText;
    [SerializeField]
    private TMP_Text attackPanelAttackInfo;
    [SerializeField]
    private TMP_Text attackPanelEnemyInfo;

    // Victory Panel
    [SerializeField]
    private TMP_Text victoryText;

    // Panel References
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private GameObject victoryPanel;
    [SerializeField]
    private GameObject stdActionPanel;
    [SerializeField]
    private GameObject magicActionPanel;
    [SerializeField]
    private GameObject attackPanel;
    [SerializeField]
    private GameObject magicInstructionPanel;

    #region Monobehavior
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
    #endregion
 
    // Main loop of the BattleUIController script. Intercepts keyboard input.
    private void CheckForKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Move");
            Move();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Attack");
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.E) && magicActionPanel.activeSelf)
        {
            Debug.Log("Magic");
            Magic();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Retreat");
            Retreat();

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Cancel");
            Cancel();
        }
    }

    // Secondary loop of the BattleUIController. While attack panel is active, intercepts keyboard input.
    private void AttackKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelAttack();
        }
    }

    // Check whether any action panels are active.
    private bool ActionPanelActive() => stdActionPanel.activeSelf || magicActionPanel.activeSelf;

    #region Panel toggling
    // Disables action panels and enables the Attack panel.
    private void SwitchToAttackPanel()
    {
        stdActionPanel.SetActive(false);
        magicActionPanel.SetActive(false);
        attackPanel.SetActive(true);
    }

    // Disables the Attack panel and enables the appropriate action panel.
    private void CancelAttackPanel()
    {
        attackPanel.SetActive(false);
        ToggleActionPanel(true,
            gameObject.GetComponent<PlayerSpotlight>()
            .lastClicked.GetComponent<CharacterFeatures>().charclass);
        
    }

    // Enables the appropriate action panel based on the character class.
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

    /// <summary>
    /// Turns the Information Panel on or off.
    /// </summary>
    /// <param name="state">Desired state of the info panel.</param>
    public void ToggleInfoPanel(bool state)
    {
        infoPanel.SetActive(state);   
    }

    // Toggles all relevant panels based on whether the Magic Instructions should be visible.
    private void ToggleMagicInstructions(bool state)
    {
        magicInstructionPanel.SetActive(state);
    }

    // Deactivates all of the panels.
    private void DeactivateAllPanels()
    {
        infoPanel.SetActive(false);
        victoryPanel.SetActive(false);
        stdActionPanel.SetActive(false);
        magicActionPanel.SetActive(false);
        attackPanel.SetActive(false);
        magicInstructionPanel.SetActive(false);
    }

    /// <summary>
    /// Toggles the Victory panel on and off.
    /// </summary>
    /// <param name="state">Desired state of the Victory panel.</param>
    public void ToggleVictoryPanel(bool state)
    {
        victoryPanel.SetActive(state);
    }

    #endregion

    #region Button assignment
    #region Basic Panel Actions
    /// <summary>
    /// Allows the selected character to move.
    /// </summary>
    public void Move()
    {
        DeactivateAllPanels();
        gameObject.GetComponent<PlayerSpotlight>().ActivateMovement(); // TODO: move to gameloop
    }

    /// <summary>
    /// Allows the selected character to attack.
    /// </summary>
    public void Attack()
    {
        SwitchToAttackPanel();
        gameObject.GetComponent<PlayerSpotlight>().ActivateAttack(); // TODO: move to gameloop
    }

    /// <summary>
    /// Allows the selected character to cast a spell.
    /// </summary>
    public void Magic()
    {
        ToggleMagicInstructions(true);
        ToggleInfoPanel(false);
        ToggleActionPanel(false, "Magic User");
        gameObject.GetComponent<PlayerSpotlight>().PlaceExplosion(); // TODO: move to gameloop
    }

    /// <summary>
    /// Allows the selected character to retreat.
    /// </summary>
    public void Retreat()
    {
        DeactivateAllPanels();
        gameObject.GetComponent<GameLoop>().Leave();
    }

    /// <summary>
    /// Deactivates all panels and unfocuses all characters.
    /// </summary>
    public void Cancel()
    {
        DeactivateAllPanels();
        gameObject
            .GetComponent<PlayerSpotlight>()
            .DeactivateFocus(
            gameObject
            .GetComponent<PlayerSpotlight>()
            .lastClicked
            .GetComponent<CharacterFeatures>()
            );
    }
    #endregion

    #region Attack Panel Actions
    /// <summary>
    /// Causes the selected character to attack.
    /// </summary>
    public void ExecuteAttack()
    {
        gameObject.GetComponent<GameLoop>().Attack();
    }

    /// <summary>
    /// Causes the selected character to cancel the attack.
    /// </summary>
    public void CancelAttack()
    {
        CancelAttackPanel();
        gameObject.GetComponent<GameLoop>().CancelAttack();
    }
    #endregion

    #region Magic Explosion Actions
    // Not really buttons, but these should still be publicly exposed.

    /// <summary>
    /// Disables magical instructions and enables the action panels.
    /// </summary>
    public void CancelMagicExplosion()
    {
        ToggleMagicInstructions(false);
        ToggleInfoPanel(true);
        ToggleActionPanel(true, "Magic User");
    }

    #endregion
    #endregion

    #region Set Text
    /// <summary>
    /// Sets the character info text.
    /// </summary>
    /// <param name="left">Text to put in the left text box.</param>
    /// <param name="right">Text to put in the right text box.</param>
    public void SetInfoPanelFriendlyText(string left, string right)
    {
        actionsPanelLeftHandText.SetText(left);
        actionsPanelRightHandText.SetText(right);
    }

    /// <summary>
    /// Sets the character stats in the info box.
    /// </summary>
    /// <param name="text">Text to display.</param>
    public void SetAttackPanelAttackerInfo(string text)
    {
        attackPanelAttackInfo.SetText(text);
    }

    /// <summary>
    /// Sets the enemy stats in the info box.
    /// </summary>
    /// <param name="text">Text to display.</param>
    public void SetAttackPanelEnemyInfo(string text)
    {
        attackPanelEnemyInfo.SetText(text);
    }
    
    /// <summary>
    /// Sets the victory panel text.
    /// </summary>
    /// <param name="text">Text to display.</param>
    public void SetVictoryPanelText(string text)
    {
        victoryText.SetText(text);
    }
    #endregion
}
