using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Controller;


namespace Scripts.ArmyBuilder
{
    public class PopulateGrid : MonoBehaviour
    {
        private Build build;
        private GameObject selection;
        
        [SerializeField]
        private GameObject explosionScrollView;
        [SerializeField]
        private TMP_Text remainingBudget;
        [SerializeField]
        private TMP_Text projectedCost;
        [SerializeField]
        private TMP_Text BudgetText, weapon, classid, fname, armor, nocash;
        [SerializeField]
        private GameObject troopInfoPanel;
        [SerializeField]
        private Button peasantTroopButton;
        [SerializeField]
        private Button warriorTroopButton;
        [SerializeField]
        private Button magicUserTroopButton;
        
        public int RollingBudget { set; get; }
        public int Budget { set; get; }
        public HashSet<GameObject> ActiveTroops { set; get; }
        public static Stack<GameObject> Explosions { set; get; }
        public static GameObject LastClicked { set; get; }
        public IInputManager InputManager { set; get; }

        #region Class Info
        /// <summary>
        /// Keep track of the player's choices so far.
        /// </summary>
        public struct Build
        {
            public Armor ChosenArmor { set; get; }
            public Weapon ChosenWeapon { set; get; }
            public Troop ChosenTroop { set; get; }

            public Build DeepCopy()
            {
                Build copy = new Build
                {
                    ChosenArmor = ChosenArmor,
                    ChosenWeapon = ChosenWeapon,
                    ChosenTroop = ChosenTroop
                };
                return copy;
            }
            public override string ToString()
            {
                return $"<Build: Troop={ChosenTroop}, Armor={ChosenArmor}, Weapon={ChosenWeapon}>";
            }
        }

        /// <summary>
        /// All armor options and their costs.
        /// </summary>
        public enum Armor
        {
            Unarmored = 0,
            LightMundaneArmor = 20,
            LightMagicalArmor = 30,
            HeavyMundaneArmor = 40,
            HeavyMagicalArmor = 50
        }

        /// <summary>
        /// All weapon choices and thier costs.
        /// </summary>
        public enum Weapon
        {
            Unarmed = 0,
            Polearm = 10,
            TwoHandedWeapon = 20,
            OneHandedWeapon = 15,
            RangedAttack = 25
        }

        /// <summary>
        /// All Troop choices and thier costs.
        /// </summary>
        public enum Troop
        {
            None = 0,
            Peasant = 10,
            TrainedWarrior = 50,
            MagicUser = 100
        }
        #endregion

        #region Monobehaviour
        // Start is called before the first frame update
        void Start()
        {
            Explosions = new Stack<GameObject>();
            ActiveTroops = new HashSet<GameObject>();
            build = new Build();
            Debug.Log(build.ToString());
            RollingBudget = 0;
            Budget = 300;
            if (InputManager == null)
                InputManager = GameObject.FindGameObjectWithTag("scripts").GetComponent<InputManager>();
        }

        private void Update()
        {
            if (selection != null && selection.GetComponent<PlaceableCollision>().shouldMove)
            {
                MoveUnplacedSelection();
            }
            
            if (InputManager.GetRightMouseClick())
            {
                Debug.Log("Deleting");
                RemovePlacedObject();
            }

            //Cancel a potential purchase with escape after clicking through the menus
            if (InputManager.GetCancelButtonDown())
            {
                CancelPurchase();
            }

            //Execute a purchase upon click
            if (InputManager.GetLeftMouseClick() && selection!=null)
            {
                ExecutePurchase();
            }

            BudgetText.text = $"Budget: {Budget}";
            projectedCost.text = $"Estimated Cost: {RollingBudget}";
            remainingBudget.text = $"Remaining Budget: {Budget - RollingBudget}";
        }

        #endregion

        // This is only called in Update() when selection != null. Nowhere else.
        private void MoveUnplacedSelection()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition());
            if (Physics.Raycast(ray, out hit, 100.0f, 1<<9 ))
            {
                if (!Physics.Raycast(ray, out RaycastHit hit2, 100.0f, layerMask: 1<<10))
                {
                    Debug.Log("Moving");
                    selection.transform.SetPositionAndRotation(
                        new Vector3(hit.point.x, 0.2f, hit.point.z),
                        Quaternion.identity);
                }
                
            }
        }

        #region Update Troop
        /// <summary>
        /// Update all of the features for the selection and add resources to the map. 
        /// </summary>
        /// <param name="resourceName">Prefab name to instantiate.</param>
        public void InstantiateTroop(string resourceName)
        {
            // Instantiate the troop
            GameObject resource = (GameObject)Instantiate(Resources.Load("PlaceableCharacters/" + resourceName));
            selection = resource;
            Debug.Log("Setting Selection");
            selection.transform.position = new Vector3(0, 0.2f, 5);
            
            // Set values
            FeaturesHolder features = selection.GetComponent<FeaturesHolder>();
            features.TroopInfo = build.DeepCopy();
            features.Cost = RollingBudget;
            features.IsActive = true;
            Debug.Log(selection);
            
            // Update internal parameters
            troopInfoPanel.SetActive(true);
            LastClicked = selection;
        }
        
        /* I've elected to set the Setters in thier own region, because they're cumbersome to look at.
         * All you're missing is a long switch statement about which enum value it should be, since
         * APPARENTLY you can't use a public enum from a button if it's not attached to said button 
         * by a script. The other option is strings (prone to misspelling), or attach 28 little scripts 
         * to buttons in the scene, or make another script which just assigns all of the functions on awake().
         */
        #region Setters For build struct
        /// <summary>
        /// Set the troop type for the current troop being purchased.
        /// </summary>
        /// <param name="type">Troop enum value.</param>
        public void SetTroop(int type)
        {
            Troop actual;
            switch(type)
            {
                case 10:
                    actual = Troop.Peasant;
                    break;
                case 50:
                    actual = Troop.TrainedWarrior;
                    break;
                case 100:
                    actual = Troop.MagicUser;
                    break;
                default:
                    actual = Troop.None;
                    break;
            }
            build.ChosenTroop = actual;
            RollingBudget += type;
        }

        /// <summary>
        /// Set the armor for the current troop being purchased.
        /// </summary>
        /// <param name="type">Armor enum value.</param>
        public void SetArmor(int type)
        {
            Armor actual;
            switch(type)
            {
                case 20:
                    actual = Armor.LightMundaneArmor;
                    break;
                case 30:
                    actual = Armor.LightMagicalArmor;
                    break;
                case 40:
                    actual = Armor.HeavyMundaneArmor;
                    break;
                case 50:
                    actual = Armor.HeavyMagicalArmor;
                    break;
                default:
                    actual = Armor.Unarmored;
                    break;
            }
            build.ChosenArmor = actual;
            RollingBudget += type;
        }

        /// <summary>
        /// Set the weapon fro the current troop being purchased.
        /// </summary>
        /// <param name="type">Weapon enum value.</param>
        public void SetWeapon(int type)
        {
            Weapon actual;
            switch(type)
            {
                case 10:
                    actual = Weapon.Polearm;
                    break;
                case 20:
                    actual = Weapon.TwoHandedWeapon;
                    break;
                case 15:
                    actual = Weapon.OneHandedWeapon;
                    break;
                case 25:
                    actual = Weapon.RangedAttack;
                    break;
                default:
                    actual = Weapon.Unarmed;
                    break;
            }
            build.ChosenWeapon = actual;
            RollingBudget += type;
        }
        #endregion
        
        #endregion

        #region UI Controls
        /// <summary>
        /// Toggle the troop buttons interactable quality.
        /// </summary>
        /// <param name="state">Desired state for button interactibility.</param>
        public void ToggleClassButtons(bool state)
        {
            peasantTroopButton.interactable = state;
            warriorTroopButton.interactable = state;
            magicUserTroopButton.interactable = state;
        }

        // Disables all of the active subpanels.
        // This is used when the Cancel button is pressed.
        private void DeactivateAllSubPanels()
        {
            GameObject subMenus = GameObject.Find("/MainCanvas/SubmenuHolder");
            int subMenuCount = subMenus.transform.childCount;
            for (int i = 0; i < subMenuCount; i++)
            {
                if (subMenus.transform.GetChild(i).gameObject.activeSelf)
                    subMenus.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        #endregion

        #region Purchasing and Returns
        /// <summary>
        /// Helper function used with the escape button to back out of a purchase.
        /// </summary>
        public void CancelPurchase()
        {
            RollingBudget = 0;

            troopInfoPanel.SetActive(false);
            if (LastClicked != null)
                Destroy(LastClicked);
            nocash.text = "";
            DeactivateAllSubPanels();
            ToggleClassButtons(true);
        }

        /// <summary>
        /// Helper function called after a click.
        /// </summary>
        public void ExecutePurchase()
        {
            selection = null;
            Debug.Log("Purchasing");

            if (Budget - RollingBudget < 0)
            {
                Debug.Log("No Money");
                nocash.text = $"Not Enough Money";
                CancelPurchase();
            }
            else
            {
                LastClicked.GetComponent<FeaturesHolder>().Cost = RollingBudget;
                LastClicked.GetComponent<FeaturesHolder>().IsActive = false;
                ActiveTroops.Add(LastClicked);
                Budget -= RollingBudget;
                RollingBudget = 0;
                ToggleClassButtons(true);
                DeactivateAllSubPanels();
                troopInfoPanel.SetActive(false);
                build = new Build();
            }
        }

        /// <summary>
        /// Remove an obect that has been already been placed called from the right click.
        /// </summary>
        public void RemovePlacedObject()
        {
            Debug.Log("Checking");
            Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition());
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, 1<<10))
            {
                Debug.Log("Deleting placed");
                Debug.Log(hit.transform.gameObject);
                LastClicked = hit.transform.gameObject;
                if (!LastClicked.GetComponent<FeaturesHolder>().IsActive)
                {
                    Budget += LastClicked.GetComponent<FeaturesHolder>().Cost;
                    ActiveTroops.Remove(LastClicked);
                }
                Destroy(LastClicked);
                CancelPurchase();
            }
        }

        /// <summary>
        /// Finalize the army and send it to the client. 
        /// </summary>
        public void FinalizeArmy()
        {
            foreach (GameObject troop in ActiveTroops)
            {
                Debug.Log(troop);
                FeaturesHolder reference = troop.GetComponent<FeaturesHolder>();
                Debug.Log(reference);
                Networking.MonoClient.Instance.SendTroopRequest(
                    reference.TroopInfo.ChosenTroop.ToString(), 
                    reference.TroopInfo.ChosenWeapon.ToString(), 
                    reference.TroopInfo.ChosenArmor.ToString(), 
                    troop.transform.position[0], 
                    troop.transform.position[1]);

            }
            Networking.MonoClient.Instance.SendFinishBuild(Explosions.Count);
        }
        #endregion

        #region Manual Test Functions
        public void TESTQUICKARMY()
        {
            Networking.MonoClient.Instance.SendTroopRequest("Peasant", "RangedAttack", "LightMundaneArmor", 0, 0);
            Networking.MonoClient.Instance.SendTroopRequest("TrainedWarrior", "OneHandedWeapon", "HeavyMundaneArmor", 1, 0);
            Networking.MonoClient.Instance.SendTroopRequest("MagicUser", "Unarmed", "Unarmored", 2, 0);
            Networking.MonoClient.Instance.SendTroopRequest("Peasant", "Polearm", "Unarmored", 3, 0);
            Networking.MonoClient.Instance.SendFinishBuild(3);
        }
        #endregion

        #region Lower Panel
        /// <summary>
        /// Adds an explosion to the bottom panel in the army build visual scene.
        /// </summary>
        public void AddExplosion() 
        {
            GameObject newExplosion = (GameObject)Instantiate(Resources.Load("UI/ArmyBuild/ExplosionImage"), explosionScrollView.transform);
            Explosions.Push(newExplosion);
            Budget -= 10;
        }

        /// <summary>
        /// Removes an explosion from the bottom panel, if there are still explosions there.
        /// </summary>
        public void RemoveExplosion() 
        {
            if (Explosions.Count != 0)
            {
                GameObject oldExplosion = Explosions.Pop();
                Destroy(oldExplosion);
                Budget += 10;
            }

        }
        #endregion
    }
}