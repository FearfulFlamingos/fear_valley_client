using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.CharacterClass;

namespace Scripts.Actions
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        public LayerMask whatCanBeClickedOn;
        public GameObject movementSelector;
        private Vector3 startingPosition;
        private Vector3 targetPosition;
        public NavMeshAgent myAgent;
        public bool selectingMovement;
        private bool movement;
        public IInputManager InputManager { set; get; }

        void Start()
        {
            myAgent = gameObject.GetComponent<NavMeshAgent>();
            if (InputManager == null)
                InputManager = GameObject.FindGameObjectWithTag("scripts").GetComponent<InputManager>();
        }
        
        void Update()
        {
            if (movement)
            {
                MoveSelector();
                
                if (InputDetected())
                    FinishMovementAndReturn();
            }
        }

        /// <summary>Moves the movement cursor around the screen to follow the actual cursor.</summary>
        private void MoveSelector()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition());

            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, 1 << 9) && selectingMovement) //Only look on ground layer
            {
                if (Vector3.Distance(hit.point, startingPosition) < gameObject.GetComponent<Character>().Features.Movement)
                {
                    movementSelector.transform.position = hit.point;
                    targetPosition = hit.point;
                }
            }
        }

        /// <summary>Checks for player input and ability to move a character</summary>
        /// <returns>Boolean value of input.</returns>
        public bool InputDetected() => InputManager.GetLeftMouseClick() && MonoClient.Instance.HasControl();

        /// <summary>
        /// This is a private function that triggers movement, sends a client request, and toggles the checks for
        /// this script. It also resets the character state via a call to PlayerSpotlight.
        /// </summary>
        public void FinishMovementAndReturn()
        {
            selectingMovement = false;
            movement = false;
            PlayerSpotlight.Instance.DeactivateCurrentFocus();
            Destroy(movementSelector);
            Move(targetPosition);
            GameLoop.ActionPoints--;
            MonoClient.Instance.SendMoveData(
                gameObject.GetComponent<Character>().Features.TroopId,
                targetPosition[0],
                targetPosition[2]);
        }

        /// <summary>Moves the gameobject to <paramref name="newPos"/> with the navmeshagent.</summary>
        /// <param name="newPos">This is the new vector where the object is put</param>
        public void Move(Vector3 newPos)
        {
            if (gameObject.GetComponent<Character>().Features.Team == 2)
            {
                newPos.z = 8 - newPos.z;
                newPos.x = 8 - newPos.x;
            }
            Debug.Log(myAgent);
            myAgent.SetDestination(newPos);
            Debug.Log(newPos);
        }

        /// <summary>
        /// Creates a movement selector from a prefab and sets its position 
        /// to the character's position.
        /// </summary>
        public void ActivateMovement()
        {
            movement = true;
            selectingMovement = true;
            movementSelector = (GameObject) Instantiate(Resources.Load("MovementCursor"));
            startingPosition = transform.position;
            movementSelector.transform.position = startingPosition;

        }
    }
}
