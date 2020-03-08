using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionScrollView;
    private Stack<GameObject> explosions;

    public GameObject selection,lastclicked;
    public GameObject UIcontrol,troopinfo, CurrentPanel;
    public Camera camera;
    [SerializeField]
    private TMP_Text remainingBudget;
    [SerializeField]
    private TMP_Text projectedCost;
    [SerializeField]
    private TMP_Text Budget, weapon, classid, fname, armor, nocash;
    public int budget= 300;
    public int rollingbudget;
    public int numTroops=1;
    private Dictionary<string, GameObject> activetroops = new Dictionary<string, GameObject>();
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
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            RemovePlacedObject();
        if (selection != null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit,100.0f,1<<9))
            {
                Debug.Log("Moving");
                selection.transform.position = hit.point;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            selection = null;
        }

        Budget.text = $"Budget: {budget}";
        projectedCost.text = $"Estimated Cost: {rollingbudget}";
        remainingBudget.text = $"Estimated Cost: {budget-rollingbudget}";
    }

    public void additem(string item)
    {
        rollingbudget += costs[item];
    }
    //void Populate()
    //{

    //    GameObject newObj;
    //    int maxX = 8, maxZ = 2;
    //    Button[] buttons = new Button[numberOfButtons];

    //    for (int i = 0; i< numberOfButtons; i++)
    //    {
    //        newObj = (GameObject) Instantiate(Resources.Load("UI/ArmyBuild/GUIButton"), transform) as GameObject;
    //        buttons[i] = newObj.GetComponent<Button>();
    //    }

    #region button setting
    //    buttons[0].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(0, 1); buttons[0].interactable = false; });
    //    buttons[1].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(1, 1); buttons[1].interactable = false; });
    //    buttons[2].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(2, 1); buttons[2].interactable = false; });
    //    buttons[3].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(3, 1); buttons[3].interactable = false; });
    //    buttons[4].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(4, 1); buttons[4].interactable = false; });
    //    buttons[5].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(5, 1); buttons[5].interactable = false; });
    //    buttons[6].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(6, 1); buttons[6].interactable = false; });
    //    buttons[7].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(7, 1); buttons[7].interactable = false; });
    //    buttons[8].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(0, 0); buttons[8].interactable = false; });
    //    buttons[9].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(1, 0); buttons[9].interactable = false; });
    //    buttons[10].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(2, 0); buttons[10].interactable = false; });
    //    buttons[11].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(3, 0); buttons[11].interactable = false; });
    //    buttons[12].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(4, 0); buttons[12].interactable = false; });
    //    buttons[13].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(5, 0); buttons[13].interactable = false; });
    //    buttons[14].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(6, 0); buttons[14].interactable = false; });
    //    buttons[15].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(7, 0); buttons[15].interactable = false; });
    //int halfway = buttons.Length / 2;
    //for (int i = 0; i< halfway;i++)
    //{
    //    int temp = i;
    //    //Debug.Log(temp);
    //    buttons[i].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(temp, 0); buttons[i].interactable = false; });
    //}

    //for (int i = halfway; i < buttons.Length; i++)
    //{
    //    int temp = i - halfway;
    //    //Debug.Log(temp);
    //    buttons[i].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(temp, 1); buttons[i].interactable = false; });
    //}

    #endregion

    #region Upper Panel
    public void SetSelection(string resourceName)
    {
        GameObject resource = (GameObject) Instantiate(Resources.Load(resourceName)) as GameObject;
        selection = resource;
        selection.transform.position = new Vector3(0,0.2f,10);
        selection.GetComponent<FeaturesHolder>().uicontrol = UIcontrol;
        selection.GetComponent<FeaturesHolder>().gamepiece = selection;
        selection.GetComponent<FeaturesHolder>().isactive = true;
        Debug.Log(selection.GetComponent<FeaturesHolder>().isactive);
        troopinfo.SetActive(true);
        lastclicked = selection;

    }
    public void GetArmor(string ArmorClass)
    {
        //selection.armorclass
        selection.GetComponent<FeaturesHolder>().armor = ArmorClass;
    }
    public void GetWeapon(string WeaponClass)
    {
        //selection.armorclass
        selection.GetComponent<FeaturesHolder>().weapon = WeaponClass;
        additem(WeaponClass);

    }
    public void GetCurrentPanel(GameObject panel)
    {
        CurrentPanel = panel;
    }
    public void ChangeChar(GameObject character)
    {
        FeaturesHolder reference = character.GetComponent<FeaturesHolder>();
        classid.text = $"{reference.troop}";
        fname.text = $"{reference.fname}";
        weapon.text = $"{reference.weapon}";
        armor.text = $"{reference.armor}";
        CurrentPanel.SetActive(false);

    }
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
    public void CancelPurchase()
    {
        rollingbudget = 0;
        
        troopinfo.SetActive(false);
        nocash.text = "";
        Destroy(lastclicked);
        ActivateButtons();

    }
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
            string dictName = lastclicked.name;
            activetroops[dictName] = selection;
            numTroops++;
            troopinfo.SetActive(false);
            budget -= rollingbudget;
            rollingbudget = 0;
            ActivateButtons();

        }


    }
    public void RemovePlacedObject()
    {
        Debug.Log("Checking");
        RaycastHit checkGameObject;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out checkGameObject, 100.0f,1<<10) && checkGameObject.transform != null)
        {
            Debug.Log("Deleting placed");
            Destroy(checkGameObject.transform.gameObject);
        }
    }

    public void FinalizeArmy()
    {
        for (int i=0; i <= numTroops; i++)
        {
            if (activetroops[$"troop{i}"])
            {
                FeaturesHolder reference = activetroops[$"troop{i}"].GetComponent<FeaturesHolder>();

                Client.Instance.SendTroopRequest(reference.troop, reference.weapon, reference.armor, (int)activetroops[$"troop{i}"].transform.position[0], (int)activetroops[$"troop{i}"].transform.position[1]);
            }
        }
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
