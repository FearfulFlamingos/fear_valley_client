using UnityEngine;
using System.Collections;
using Scripts.Actions;

namespace Scripts.Character
{
    public class Character : MonoBehaviour
    {
        public CharacterFeatures Features { set; get; }
        private PlayerMovement _playerMovement;
        private PlayerAttack _playerAttack;
        private PlayerMagic _playerMagic;

        public enum State
        {
            None,
            Moving,
            Attacking,
            CastingSpell,
            Selected
        }

        public State CurrentState { set; get; }

        // Use this for initialization
        void Start()
        {
            if (Features == null)
                Features = new CharacterFeatures();
            CurrentState = State.None;
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAttack = GetComponent<PlayerAttack>();
            _playerMagic = GetComponent<PlayerMagic>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (CurrentState)
            {
                case State.Moving:
                    Features.IsFocused = false;
                    _playerMovement.ActivateMovement();
                    break;
                case State.Attacking:
                    Features.IsFocused = false;
                    break;
                case State.CastingSpell:
                    Features.IsFocused = false;
                    break;
                case State.Selected:
                    Features.IsFocused = true;
                    break;
                case State.None:
                default:
                    Features.IsFocused = false;
                    return;
            }
        }


    }
}