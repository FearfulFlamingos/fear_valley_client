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
        private Stack<GameObject> explosions;

        public GameObject selection, lastclicked;
        public GameObject UIcontrol, troopinfo, CurrentPanel;
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
        private HashSet<GameObject> activetroops = new HashSet<GameObject>();

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
                RemovePlacedObject();
            }

            if (selection != null)
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, 1 << 9))
                {
                    Debug.Log("Moving");
                    selection.transform.position = hit.point;
                }
            }
            //Cancel a potential purchase with escape after clicking through the menus
            if (InputManager.GetEscapeKeyDown())
            {
                Destroy(selection);
                selection = null;
                troopinfo.SetActive(false);
                rollingbudget = 0;
                ActivateButtons();
            }
            //Execute a purchase upon click
            if (InputManager.GetLeftMouseClick() && selection!=null)
            {
                selection = null;
                ExecutePurchase();
            }
            //Select a troop after the troop has been placed
            if (InputManager.GetLeftMouseClick() && selection == null)
            {
                RaycastHit checkGameObject;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out checkGameObject, 100.0f, 1 << 10) && checkGameObject.transform != null)
                {
                    Debug.Log("Highlighting player");
                    GameObject target = checkGameObject.transform.gameObject;
                    target.GetComponent<FeaturesHolder>().isactive = true;
                    troopinfo.SetActive(true);
                    //start coroutine to deactivate the ui

                    StartCoroutine(Unhighlight(target));
                }
            }

            Budget.text = $"Budget: {budget}";
            projectedCost.text = $"Estimated Cost: {rollingbudget}";
            remainingBudget.text = $"Estimated Cost: {budget - rollingbudget}";
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
        IEnumerator Unhighlight(GameObject target)
        {

            yield return 20000;    //Wait one frame
            troopinfo.SetActive(false);
            target.GetComponent<FeaturesHolder>().isactive = false;
        }


        #region Update Troop

        /// <summary>
        /// Update all of the features for the selection and add resources to the map. 
        /// </summary>
        /// <param name="resourceName"></param>
        public void SetSelection(string resourceName)
        {
            GameObject resource = (GameObject)Instantiate(Resources.Load(resourceName)) as GameObject;
            selection = resource;
            selection.transform.position = new Vector3(0, 0.2f, 10);
            selection.GetComponent<FeaturesHolder>().uicontrol = UIcontrol;
            selection.GetComponent<FeaturesHolder>().gamepiece = selection;
            selection.GetComponent<FeaturesHolder>().isactive = true;
            Debug.Log(selection.GetComponent<FeaturesHolder>().isactive);
            selection.GetComponent<FeaturesHolder>().armor = current_armor;
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
        /// Set the panel that is currently being used. 
        /// </summary>
        /// <param name="panel"></param>
        public void GetCurrentPanel(GameObject panel)
        {
            CurrentPanel = panel;
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
            budget += System.Convert.ToInt32(lastclicked.GetComponent<FeaturesHolder>().troop);
            activetroops.Remove(lastclicked);
            nocash.text = "";
            Destroy(lastclicked);
            ActivateButtons();

        }
        /// <summary>
        /// Helper function called after a click.
        /// </summary>
        public void ExecutePurchase()
        {
            if (budget - rollingbudget < 0)
            {
                Debug.Log("in");
                nocash.text = $"Not Enough Money";
            }
            else
            {
                lastclicked.name = $"troop{numTroops}";
                activetroops.Add(lastclicked);
                numTroops++;
                budget -= rollingbudget;
                rollingbudget = 0;
                ActivateButtons();
                troopinfo.SetActive(false);
            }
        }
        /// <summary>
        /// Remove an obect that has been already been placed called from the right click.
        /// </summary>
        public void RemovePlacedObject()
        {
            Debug.Log("Checking");
            RaycastHit checkGameObject;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out checkGameObject, 100.0f, 1 << 10) && checkGameObject.transform != null)
            {
                Debug.Log("Deleting placed");
                Destroy(checkGameObject.transform.gameObject);
                lastclicked = checkGameObject.transform.gameObject;
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

        #endregion


        #region Lower Panel
        /// <summary>
        /// Adds an explosion to the bottom panel in the army build visual scene.
        /// </summary>
        public void AddExplosion() //TODO: Update cost string
        {
            GameObject newExplosion = (GameObject)Instantiate(Resources.Load("UI/ArmyBuild/ExplosionImage"), explosionScrollView.transform);
            explosions.Push(newExplosion);
            budget -= 10;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveExplosion() //TODO: Update Cost string
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