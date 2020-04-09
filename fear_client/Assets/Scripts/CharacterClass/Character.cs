using UnityEngine;
using System.Collections;
using Scripts.Actions;

namespace Scripts.CharacterClass
{
    public class Character : MonoBehaviour
    {
        public ICharacterFeatures Features { set; get; }
        public IPlayerMovement PlayerMovement { set; get; }
        public PlayerAttack PlayerAttack {set; get;}
        public PlayerMagic PlayerMagic { set; get; }

        public enum State
        {
            None,
            Moving,
            Attacking,
            CastingSpell,
            Selected,
            Waiting
        }

        public State CurrentState { set; get; }

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
                    PlayerMagic.PlaceExplosion();
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


    }
}