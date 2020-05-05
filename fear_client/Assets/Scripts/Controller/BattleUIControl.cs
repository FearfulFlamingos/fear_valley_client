using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Scripts.CharacterClass;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scripts.Controller
{
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

        [SerializeField]
        private UnityEngine.UI.Button exitButton;
        //[SerializeField]
        //private GameObject inGameOptionsMenu;

        // Text References
        [SerializeField]
        private TMP_Text enemyName;
        private IInputManager InputManager { set; get; }
        public static BattleUIControl Instance { set; get; }

        #region Monobehavior
        private void Start()
        {
            if (InputManager == null)
                InputManager = gameObject.GetComponent<InputManager>();
            if (Instance == null)
                Instance = this;
            exitButton.onClick.AddListener(() => SceneManager.LoadScene(0));

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
        #endregion

        // Main loop of the BattleUIController script. Intercepts keyboard input.
        public void CheckForKeyboardInput()
        {
            if (InputManager.GetMoveButtonDown())
            {
                Debug.Log("Move");
                Move();
            }
            else if (InputManager.GetAttackButtonDown())
            {
                Debug.Log("Attack");
                Attack();
            }
            else if (InputManager.GetMagicButtonDown() && magicActionPanel.activeSelf)
            {
                Debug.Log("Magic");
                Magic();
            }
            else if (InputManager.GetRetreatButtonDown())
            {
                Debug.Log("Retreat");
                Retreat();

            }
            else if (InputManager.GetCancelButtonDown())
            {
                Debug.Log("Cancel");
                Cancel();
            }
        }

        // Secondary loop of the BattleUIController. While attack panel is active, intercepts keyboard input.
        private void AttackKeyboardInput()
        {
            if (InputManager.GetSpaceKeyDown())
            {
                ExecuteAttack();
            }
            else if (InputManager.GetCancelButtonDown())
            {
                CancelAttack();
            }
        }

        // Check whether any action panels are active.
        private bool ActionPanelActive() => stdActionPanel.activeSelf || magicActionPanel.activeSelf;

        #region Panel toggling
        // Disables action panels and enables the Attack panel.
        public void SwitchToAttackPanel()
        {
            stdActionPanel.SetActive(false);
            magicActionPanel.SetActive(false);
            attackPanel.SetActive(true);
        }

        // Disables the Attack panel and enables the appropriate action panel.
        public void CancelAttackPanel()
        {
            attackPanel.SetActive(false);
            ToggleActionPanel(true,
                GameLoop
                .SelectedCharacter
                .GetComponent<Character>()
                .Features
                .Charclass);

        }

        // Enables the appropriate action panel based on the character class.
        public void ToggleActionPanel(bool state, string charClass = "")
        {
            if (charClass == "MagicUser")
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

        ///<summary>Toggles all relevant panels based on whether the Magic Instructions should be visible.</summary>
        public void ToggleMagicInstructions(bool state)
        {
            magicInstructionPanel.SetActive(state);
        }

        /// <summary>
        /// Deactivates all panels.
        /// </summary>
        public void DeactivateAllPanels()
        {
            infoPanel.SetActive(false);
            //victoryPanel.SetActive(false);
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
            Debug.Log($"Setting Victory Panel {state}");
            victoryPanel.SetActive(state);
            Debug.Log(victoryPanel.activeSelf);
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
            GameLoop.Instance.Move();
        }

        /// <summary>
        /// Allows the selected character to attack.
        /// </summary>
        public void Attack()
        {
            SwitchToAttackPanel();
            GameLoop.Instance.Attack();
        }

        /// <summary>
        /// Allows the selected character to cast a spell.
        /// </summary>
        public void Magic()
        {
            ToggleMagicInstructions(true);
            ToggleInfoPanel(false);
            ToggleActionPanel(false, "MagicUser");
            GameLoop.Instance.CastSpell();
        }

        /// <summary>
        /// Allows the selected character to retreat.
        /// </summary>
        public void Retreat()
        {
            DeactivateAllPanels();
            GameLoop.Instance.Leave();
        }

        /// <summary>
        /// Deactivates all panels and unfocuses all characters.
        /// </summary>
        public void Cancel()
        {
            DeactivateAllPanels();
            PlayerSpotlight.Instance.DeactivateCurrentFocus();
        }
        #endregion

        #region Attack Panel Actions
        /// <summary>
        /// Causes the selected character to attack.
        /// </summary>
        public void ExecuteAttack()
        {
            GameLoop.Instance.ConfirmAttack();
        }

        /// <summary>
        /// Causes the selected character to cancel the attack.
        /// </summary>
        public void CancelAttack()
        {
            CancelAttackPanel();
            GameLoop.Instance.CancelAttack();
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
            ToggleActionPanel(true, "MagicUser");
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
        /// Updates the enemy name.
        /// </summary>
        /// <param name="name">New name to update.</param>
        public void UpdateEnemyName(string name)
        {
            enemyName.SetText(name);
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

        #region Testing
        public string TESTGETATTACKPANELENEMYINFO() => attackPanelEnemyInfo.text;
        public bool TESTGETVICTORYPANELSTATUS() => victoryPanel.activeSelf;
        #endregion
    }
}