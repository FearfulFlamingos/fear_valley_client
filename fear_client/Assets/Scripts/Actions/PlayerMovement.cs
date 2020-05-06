using UnityEngine;
using UnityEngine.AI;
using Scripts.Controller;
using Scripts.Networking;
using Scripts.CharacterClass;


namespace Scripts.Actions
{
    /// <inheritdoc cref="IPlayerMovement"/>
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        private bool movement;
        private Vector3 startingPosition;
        private Vector3 targetPosition;
        public LayerMask whatCanBeClickedOn;
        public GameObject movementSelector;
        public NavMeshAgent myAgent;
        public bool selectingMovement;
        
        /// <inheritdoc cref="IPlayerMovement.InputManager"/>
        public IInputManager InputManager { set; get; }

        #region Monobehaviour
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
        #endregion 

        // Moves the movement cursor around the screen to follow the actual cursor.
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

        /// <inheritdoc cref="IPlayerMovement.InputDetected"/>
        public bool InputDetected() => InputManager.GetLeftMouseClick() && MonoClient.Instance.HasControl();

        /// <inheritdoc cref="IPlayerMovement.FinishMovementAndReturn"/>
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

        /// <inheritdoc cref="IPlayerMovement.Move(Vector3)"/>
        public void Move(Vector3 newPos)
        {
            if (gameObject.GetComponent<Character>().Features.Team == 2)
            {
                newPos.z = 9.0f - newPos.z;
                newPos.x = 9.0f - newPos.x;
            }
            Debug.Log(myAgent);
            myAgent.SetDestination(newPos);
            Debug.Log(newPos);
        }

        /// <inheritdoc cref="IPlayerMovement.ActivateMovement"/>
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
