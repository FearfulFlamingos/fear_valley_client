using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.Networking;
/// <summary>
///     This is the main script to keep track of all army building
///     related things. 
/// </summary>
public class ArmyBudget : NetworkBehaviour
{

    private int currentBudget = 300;
    private Dictionary<string, int> costs;
    public Text displayBudget;
    public Dropdown characters;
    public Dropdown weapons;
    public Dropdown armors;

    [SerializeField]
    private Button button0;
    [SerializeField]
    private Button button1;
    [SerializeField]
    private Button button2;
    [SerializeField]
    private Button button3;
    [SerializeField]
    private Button button4;
    [SerializeField]
    private Button button5;
    [SerializeField]
    private Button button6;
    [SerializeField]
    private Button button7;
    [SerializeField]
    private Button button8;
    [SerializeField]
    private Button button9;
    [SerializeField]
    private Button button10;
    [SerializeField]
    private Button button11;
    [SerializeField]
    private Button button12;
    [SerializeField]
    private Button button13;
    [SerializeField]
    private Button button14;
    [SerializeField]
    private Button button15;
    [SerializeField]
    private GameObject warning;

    public GameObject commandMgr;
    public int connectionID;

    /// <summary>
    /// This adds functionality to the buttons, fills dropdown menus,
    /// and reads costs into a dict for later comparison.
    /// </summary>
    private void Start()
    {
        connectionID = PlayerPrefs.GetInt("connID");
        // Grid workaroud
        button0.onClick.AddListener(() => { AddPersonOnClick("0", "-18"); button0.interactable = false; });
        button1.onClick.AddListener(() => { AddPersonOnClick("3", "-18"); button1.interactable = false; });
        button2.onClick.AddListener(() => { AddPersonOnClick("0", "-21"); button2.interactable = false; });
        button3.onClick.AddListener(() => { AddPersonOnClick("3", "-21"); button3.interactable = false; });
        button4.onClick.AddListener(() => { AddPersonOnClick("6", "-18"); button4.interactable = false; });
        button5.onClick.AddListener(() => { AddPersonOnClick("9", "-18"); button5.interactable = false; });
        button6.onClick.AddListener(() => { AddPersonOnClick("6", "-21"); button6.interactable = false; });
        button7.onClick.AddListener(() => { AddPersonOnClick("9", "-21"); button7.interactable = false; });
        button8.onClick.AddListener(() => { AddPersonOnClick("12", "-18"); button8.interactable = false; });
        button9.onClick.AddListener(() => { AddPersonOnClick("15", "-18"); button9.interactable = false; });
        button10.onClick.AddListener(() => { AddPersonOnClick("12", "-21"); button10.interactable = false; });
        button11.onClick.AddListener(() => { AddPersonOnClick("15", "-21"); button11.interactable = false; });
        button12.onClick.AddListener(() => { AddPersonOnClick("18", "-18"); button12.interactable = false; });
        button13.onClick.AddListener(() => { AddPersonOnClick("21", "-18"); button13.interactable = false; });
        button14.onClick.AddListener(() => { AddPersonOnClick("18", "-21"); button14.interactable = false; });
        button15.onClick.AddListener(() => { AddPersonOnClick("21", "-21"); button15.interactable = false; });
        warning.SetActive(false);
        // Everything else
        DatabaseController dbCont = new DatabaseController();
        costs = new Dictionary<string, int>();
        PopulateDropdown(characters, dbCont.ReadDB("class, cost", "Troop"));
        PopulateDropdown(weapons, dbCont.ReadDB("name, cost", "Weapon"));
        PopulateDropdown(armors, dbCont.ReadDB("armor, cost", "Armor"));
        dbCont.update("DELETE FROM Army;");
        dbCont.CloseDB();
        

    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        displayBudget.text = $"Current Resources: {currentBudget}";

    }

    /// <summary>
    /// This function checks the value of the dropdown menus against the cost,
    /// which is stored in a local dict.
    /// </summary>
    /// <returns></returns>
    private int CalculateCost()
    {
        //foreach (var thing in costs) { Debug.Log(thing); }
        int totalCost = 0;
        string troop = characters.options[characters.value].text;
        totalCost += costs[troop];

        string weap = weapons.options[weapons.value].text;
        totalCost += costs[weap];

        string armor = armors.options[weapons.value].text;
        totalCost += costs[armor];

        return totalCost;
    }

    /// <summary>
    /// Given a menu object and a dictionary of options, will add all
    /// options to the DD menu and to the costs dict.
    /// </summary>
    /// <param name="drpdwn">Dropdown menu object to be filled.</param>
    /// <param name="options">Dict object of values.</param>
    private void PopulateDropdown(Dropdown drpdwn, Dictionary<string, int> options)
    {
        List<string> values = new List<string>();
        foreach (var option in options.Keys)
        {
            values.Add(option);
            costs.Add(option, options[option]);
        }
        drpdwn.ClearOptions();
        drpdwn.AddOptions(values);
    }

    /// <summary>
    /// Main button behavior. Will read values from DD menus and create
    /// SQL query to insert into ARMY table. Buttons provide coordinates.
    /// Unity is wierd, Y is straight up and XZ plane is ground.
    /// </summary>
    /// <param name="posx">X position of troop.</param>
    /// <param name="posz">Z position of troop.</param>
    public void AddPersonOnClick(string posx, string posz)
    {

        int cost = CalculateCost();
        if (connectionID > 0)
        {
            posz = "0";
        }

        Debug.Log("POS:" + posx + "," + posz);
        Debug.Log("COST:" + cost);
        if (cost <= currentBudget)
        {
            currentBudget -= cost;
            //DatabaseController dbCont = new DatabaseController();
            string sql = "INSERT INTO Army " +
                "(teamNumber,class,armor,shield,weapon," +
                "isLeader,pos_x,pox_z) " +
                "VALUES " +
                $"({connectionID}," +
                $"'{characters.options[characters.value].text}'," +
                $"'{armors.options[armors.value].text}'," +
                "'Mundane shield'," +
                $"'{weapons.options[weapons.value].text}'," +
                "'0'," +
                $"{posx}," +
                $"{posz}); ";
            //dbCont.CloseDB();
            commandMgr.GetComponent<NetworkArmyQueueManager>().Add(sql);

            
        }
        else
        {
            warning.SetActive(true);
        }
    }


    //[Command]
    //private void CmdUpdateOnServer(string sql)
    //{
    //    DatabaseController dbCont = new DatabaseController();
    //    dbCont.update(sql);
    //    dbCont.CloseDB();
    //}

}