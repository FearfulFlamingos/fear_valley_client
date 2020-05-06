using UnityEngine;
using Scripts.Actions;

/// <summary>
/// All scripts related to just the properties of the characters.
/// </summary>
namespace Scripts.CharacterClass
{
    /// <summary>
    /// The wrapper class for each character. Determines what state the character is in.
    /// </summary>
    public class Character : MonoBehaviour
    {
        /// <summary>Properties of the character.See <see cref="CharacterFeatures"/>.</summary>
        public ICharacterFeatures Features { set; get; }
        
        /// <summary>Movement script for the character. See <see cref="Actions.PlayerMovement"/>.</summary>
        public IPlayerMovement PlayerMovement { set; get; }
        
        /// <summary>Attack script for the character. See <see cref="Actions.PlayerAttack"/>.</summary>
        public PlayerAttack PlayerAttack {set; get;}
        
        /// <summary>Magic script for the character, if applicable. 
        /// <see cref="Actions.PlayerMagic"/>.</summary>
        public PlayerMagic PlayerMagic { set; get; }

        /// <summary>Current state of the character. See <see cref="State"/>.</summary>
        public State CurrentState { set; get; }
        
        /// <summary>
        /// All possible states of the character.
        /// </summary>
        public enum State
        {
            /// <summary>Default state.</summary>
            None,
            
            /// <summary>Currently moving.</summary>
            Moving,
            
            /// <summary>Currently attacking.</summary>
            Attacking,
            
            /// <summary>Casting a spell.</summary>
            CastingSpell,
            
            /// <summary>Selected by the player.</summary>
            Selected,
            
            /// <summary>Inter-state waiting location.</summary>
            Waiting
        }

        #region Monobehaviour
        // Use this for initialization
        void Start()
        {
            CurrentState = State.None;
            if (Features == null)
                Features = new CharacterFeatures();
            if (PlayerMovement == null) 
                PlayerMovement = gameObject.GetComponent<PlayerMovement>();
            if (PlayerAttack == null) 
                PlayerAttack = gameObject.GetComponent<PlayerAttack>();
            if (PlayerMagic == null)
                PlayerMagic = gameObject.GetComponent<PlayerMagic>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (CurrentState)
            {
                case State.Moving:
                    PlayerMovement.ActivateMovement();
                    CurrentState = State.Waiting;
                    break;
                case State.Attacking:
                    PlayerAttack.ActivateAttack();
                    break;
                case State.CastingSpell:
                    PlayerMagic.StartExplosionSelector();
                    CurrentState = State.Waiting;
                    break;
                case State.Selected:
                    Features.IsFocused = true;
                    break;
                case State.Waiting:
                case State.None:
                default:
                    return;
            }
        }
        #endregion
    }
}