using UnityEngine;
using Scripts.Networking;
using Scripts.CharacterClass;

namespace Scripts.Controller
{
    /// <summary>
    /// Highlights a character after the player clicks on them.
    /// </summary>
    public class PlayerSpotlight : MonoBehaviour
    {
        private bool selectingCharacter = true;
        private BattleUIControl uiController;
        public Camera camera1;
        public GameObject board;
        public GameObject battleUI;
        public bool testing;

        /// <summary>Static instance of the Player Spotlight, as there will only ever be one.</summary>
        public static PlayerSpotlight Instance { private set; get; }
        /// <inheritdoc cref="Actions.IPlayerMovement.InputManager"/>
        public IInputManager InputManager { set; get; }

        #region Monobehavior
        private void Start()
        {
            camera1.gameObject.SetActive(true);
            uiController = gameObject.GetComponent<BattleUIControl>();

            if (InputManager == null)
            {
                InputManager = gameObject.GetComponent<InputManager>();
            }
            Instance = this;

        }

        // This function is constantly checking, what has been clicked on the screen. This is used
        // by the player to select a figure which is on their team. Once the figure has been selected
        // they are given a menu with options on what they would like to do with the figure. They
        // select the option they would like to pursue by clicking the button on the menu which is
        // linked to the functions below.
        private void Update()
        {
            if (selectingCharacter && InputManager.GetLeftMouseClick() && MonoClient.Instance.HasControl())
            {
                Ray ray = camera1.ScreenPointToRay(InputManager.MousePosition());
                if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, layerMask: 1 << 10 ))
                {
                    ChangeSelection(hit.transform.gameObject);
                }
                else
                {
                    if (Physics.Raycast(ray, out RaycastHit uiHit, 100.0f, layerMask: 1<< 9) && GameLoop.SelectedCharacter != null)
                    {
                        DeactivateCurrentFocus();
                        GameLoop.SelectedCharacter = null;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Changes the selected object to match the one just clicked on.
        /// </summary>
        /// <param name="selection">GameObject to select.</param>
        public void ChangeSelection(GameObject selection)
        {
            Debug.Log($"Last clicked: {GameLoop.SelectedCharacter}");

            // Something was already focused and we want to switch it
            if (GameLoop.SelectedCharacter != null)
            {
                Debug.Log("Unfocusing last character");
                DeactivateCurrentFocus();
            }
            Debug.Log("Focusing new character");
            GameLoop.SelectedCharacter = selection;
            Debug.Log($"Last clicked object: {GameLoop.SelectedCharacter.gameObject.name}");
            SpotlightChar();
        }

        /// <summary>
        /// Disables the LMB to prevent the <see cref="Update"/> loop from running.
        /// </summary>
        public void DisableCharacterSelect()
        {
            selectingCharacter = false;
        }

        /// <summary>
        /// This functions is called after a viable object has been selected and it focuses the
        /// object and then populates the ui canvas. 
        /// </summary>
        /// <param name="current">Selected character.</param>
        private void SpotlightChar()
        {
            Character character = GameLoop.SelectedCharacter.GetComponent<Character>();
            if (character.CurrentState == Character.State.Selected)
            {
                DeactivateCurrentFocus();
            }
            else
            {
                character.CurrentState = Character.State.Selected;
                ChangeFocusedColor(Color.red);
                uiController.ToggleInfoPanel(true);
                uiController.ToggleActionPanel(true, character.Features.Charclass);

                string leftText = $"Name: Roman\nAttack:+4\nAction Points:{GameLoop.ActionPoints}";
                string rightText = $"Class: {character.Features.Charclass}\nDefense:13\nMovement:6";
                uiController.SetInfoPanelFriendlyText(leftText, rightText);
            }
        }

        /// <summary>
        /// Unfocuses the selected character.
        /// </summary>
        /// <param name="referenceScript">The CharacterFeatures component of the selected character.</param>
        public void DeactivateCurrentFocus()
        {
            ChangeFocusedColor(Color.white);
            GameLoop.SelectedCharacter.GetComponent<Character>().CurrentState = Character.State.None;
            uiController.SetAttackPanelAttackerInfo("");
            uiController.SetAttackPanelEnemyInfo("");
            uiController.DeactivateAllPanels();

            GameLoop.SelectedCharacter = null;
            selectingCharacter = true;
        }

        // Changes the color of a character's base.
        private void ChangeFocusedColor(Color color)
        {
            GameLoop.SelectedCharacter.transform.GetChild(0).Find("Base").gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }

}