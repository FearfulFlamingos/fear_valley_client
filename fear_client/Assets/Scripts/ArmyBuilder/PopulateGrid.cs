using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Networking;
using Scripts.Controller;


namespace Scripts.ArmyBuilder
{
    public class PopulateGrid : MonoBehaviour
    {
        [SerializeField]
        private GameObject explosionScrollView;
        public Stack<GameObject> explosions;

        public GameObject selection, lastclicked;
        public GameObject UIcontrol, troopinfo;
        public Camera camera;
        [SerializeField]
        private TMP_Text remainingBudget;
        [SerializeField]
        private TMP_Text projectedCost;
        [SerializeField]
        private TMP_Text Budget, weapon, classid, fname, armor, nocash;
        public int budget = 300;
        public int rollingbudget;
        public int numTroops = 1;
        public string current_armor;
        public IInputManager InputManager { set; get; }
        public HashSet<GameObject> activetroops = new HashSet<GameObject>();

        Dictionary<string, int> costs = new Dictionary<string, int>()
        {
            {"Unarmored", 0 },
            {"Light mundane armor", 20 },
            {"Light magical armor", 30 },
            {"Heavy mundane armor", 40 },
            {"Heavy magical armor",50 },

            { "Unarmed", 0 },
            {"Polearm", 10 },
            {"Two-handed weapon", 20 },
            {"One-handed weapon", 15 },
            {"Ranged attack", 25 },
            {"Magical Explosion", 10 },

            {"No troop",0 },
            {"Peasant", 10 },
            {"Trained Warrior", 50 },
            {"Magic User", 100 }
        };
        // Start is called before the first frame update
        void Start()
        {
            explosions = new Stack<GameObject>();
            if (InputManager == null)
                InputManager = GameObject.FindGameObjectWithTag("scripts").GetComponent<InputManager>();
        }

        private void Update()
        {
            if (InputManager.GetRightMouseClick())
            {
                Debug.Log("Deleting");
                RemovePlacedObject();
            }

            if (selection != null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition());
                if (Physics.Raycast(ray, out hit, 100.0f, 1 << 9))
                {
                    Debug.Log("Moving");
                    selection.transform.position = hit.point;
                }
            }
            //Cancel a potential purchase with escape after clicking through the menus
            if (InputManager.GetCancelButtonDown())
            {
                Debug.Log("Escaping");
                Destroy(selection);
                selection = null;
                lastclicked = null;
                troopinfo.SetActive(false);
                rollingbudget = 0;
                DeactivateAllSubPanels();
                ActivateButtons();
            }
            //Execute a purchase upon click
            if (InputManager.GetLeftMouseClick() && selection!=null)
            {
                selection = null;
                Debug.Log("Purchasing");
                ExecutePurchase();
            }
            //Select a troop after the troop has been placed
            //if (InputManager.GetLeftMouseClick() && selection == null)
            //{
            //    RaycastHit checkGameObject;
            //    Ray ray = Camera.main.ScreenPointToRay(InputManager.MousePosition());
            //    if (Physics.Raycast(ray, out checkGameObject, 100.0f, 1 << 10) && checkGameObject.transform != null)
            //    {
            //        GameObject target = checkGameObject.transform.gameObject;
            //        target.GetComponent<FeaturesHolder>().isactive = true;
            //        troopinfo.SetActive(true);
            //        //start coroutine to deactivate the ui

            //        //StartCoroutine(Unhighlight(target));
            //    }
            //}

            Budget.text = $"Budget: {budget}";
            projectedCost.text = $"Estimated Cost: {rollingbudget}";
            remainingBudget.text = $"Remaining Budget: {budget - rollingbudget}";
        }
        //public bool InputDetected() => InputManager.GetLeftMouseClick() && Client.Instance.HasControl();
        /// <summary>
        /// Add the cost of an item to the adjusted budget
        /// </summary>
        /// <param name="item">This is the name of the item being added.</param>
        public void additem(string item)
        {
            rollingbudget += costs[item];
        }

        /// <summary>
        /// Deacitvate the canvas after a set amount of time. 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        //IEnumerator Unhighlight(GameObject target)
        //{

        //    yield return 20000;    //Wait one frame
        //    troopinfo.SetActive(false);
        //    target.GetComponent<FeaturesHolder>().isactive = false;
        //}


        #region Update Troop

        /// <summary>
        /// Update all of the features for the selection and add resources to the map. 
        /// </summary>
        /// <param name="resourceName"></param>
        public void SetSelection(string resourceName)
        {
            GameObject resource = (GameObject)Instantiate(Resources.Load("PlaceableCharacters/" + resourceName));
            selection = resource;
            Debug.Log("Setting Selection");
            selection.transform.position = new Vector3(0, 0.2f, 5);
            selection.GetComponent<FeaturesHolder>().uicontrol = UIcontrol;
            selection.GetComponent<FeaturesHolder>().gamepiece = selection;
            selection.GetComponent<FeaturesHolder>().isactive = true;
            selection.GetComponent<FeaturesHolder>().armor = current_armor;
            Debug.Log(selection);
            troopinfo.SetActive(true);
            lastclicked = selection;

        }
        /// <summary>
        /// Update the armor for an individual object. 
        /// </summary>
        /// <param name="ArmorClass"></param>
        public void GetArmor(string ArmorClass)
        {
            //selection.armorclass
            current_armor = ArmorClass;
            additem(ArmorClass);
            
        }

        /// <summary>
        /// Update weapon for an individual object.
        /// </summary>
        /// <param name="WeaponClass"></param>
        public void GetWeapon(string WeaponClass)
        {
            //selection.armorclass
            selection.GetComponent<FeaturesHolder>().weapon = WeaponClass;

        }


        /// <summary>
        /// Update the ui canvas for current player. 
        /// </summary>
        /// <param name="character"></param>
        public void ChangeChar(GameObject character)
        {
            FeaturesHolder reference = character.GetComponent<FeaturesHolder>();
            classid.text = $"{reference.troop}";
            fname.text = $"{reference.fname}";
            weapon.text = $"{reference.weapon}";
            armor.text = $"{reference.armor}";
            //CurrentPanel.SetActive(false);

        }
        #endregion

        #region Button Functions
        public void ActivateButtons()
        {
            GameObject.Find("PeasantButton").GetComponent<Button>().interactable = true;
            GameObject.Find("WizardButton").GetComponent<Button>().interactable = true;
            GameObject.Find("WarriorButton").GetComponent<Button>().interactable = true;
        }
        public void DeactivateButtons()
        {
            GameObject.Find("PeasantButton").GetComponent<Button>().interactable = false;
            GameObject.Find("WizardButton").GetComponent<Button>().interactable = false;
            GameObject.Find("WarriorButton").GetComponent<Button>().interactable = false;
        }
        /// <summary>
        /// Helper funciton used with the escape button to back out of a purchase.
        /// </summary>
        public void CancelPurchase()
        {
            rollingbudget = 0;

            troopinfo.SetActive(false);
            Debug.Log(lastclicked);
            budget += lastclicked.GetComponent<FeaturesHolder>().cost;
            activetroops.Remove(lastclicked);
            nocash.text = "";
            Destroy(lastclicked);
            DeactivateAllSubPanels();
            ActivateButtons();

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

        /// <summary>
        /// Helper function called after a click.
        /// </summary>
        public void ExecutePurchase()
        {
            if (budget - rollingbudget < 0)
            {
                Debug.Log("No Money");
                nocash.text = $"Not Enough Money";
            }
            else
            {
                lastclicked.name = $"troop{numTroops}";
                lastclicked.GetComponent<FeaturesHolder>().cost = rollingbudget;
                activetroops.Add(lastclicked);
                numTroops++;
                budget -= rollingbudget;
                rollingbudget = 0;
                ActivateButtons();
                DeactivateAllSubPanels();
                troopinfo.SetActive(false);
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
                lastclicked = hit.transform.gameObject;
                CancelPurchase();
            }
        }

        /// <summary>
        /// Finalize the army and send it to the client. 
        /// </summary>
        public void FinalizeArmy()
        {
            foreach (GameObject troop in activetroops)
            {
                Debug.Log(troop);
                FeaturesHolder reference = troop.GetComponent<FeaturesHolder>();
                Debug.Log(reference);
                MonoClient.Instance.SendTroopRequest(reference.troop, reference.weapon, reference.armor, (int)troop.transform.position[0], (int)troop.transform.position[1]);

            }
            MonoClient.Instance.SendFinishBuild(explosions.Count);
        }


        public void TESTQUICKARMY()
        {
            MonoClient.Instance.SendTroopRequest("Peasant", "Ranged attack", "Light mundane armor", 0, 0);
            MonoClient.Instance.SendTroopRequest("Trained Warrior", "One-handed weapon", "Heavy mundane armor", 1, 0);
            MonoClient.Instance.SendTroopRequest("Magic User", "Unarmed", "Unarmored", 2, 0);
            MonoClient.Instance.SendTroopRequest("Peasant", "Polearm", "Unarmored", 3, 0);
            MonoClient.Instance.SendFinishBuild(3);
        }
        #endregion


        #region Lower Panel
        /// <summary>
        /// Adds an explosion to the bottom panel in the army build visual scene.
        /// </summary>
        public void AddExplosion() 
        {
            GameObject newExplosion = (GameObject)Instantiate(Resources.Load("UI/ArmyBuild/ExplosionImage"), explosionScrollView.transform);
            explosions.Push(newExplosion);
            budget -= 10;
        }

        /// <summary>
        /// Removes an explosion from the bottom panel, if there are still explosions there.
        /// </summary>
        public void RemoveExplosion() 
        {
            if (explosions.Count != 0)
            {
                GameObject oldExplosion = explosions.Pop();
                Destroy(oldExplosion);
                budget += 10;
            }

        }
        #endregion
    }
}