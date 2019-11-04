using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class ArmyBudget : MonoBehaviour
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

    private void Start()
    {
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
        displayBudget.text = $"Current Resources: {currentBudget}";

    }

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

    private void PopulateDropdown(Dropdown drpdwn, Dictionary<string, int> optionsArray)
    {
        List<string> options = new List<string>();
        foreach (var option in optionsArray.Keys)
        {
            options.Add(option);
            costs.Add(option, optionsArray[option]);
        }
        drpdwn.ClearOptions();
        drpdwn.AddOptions(options);
    }

    public void AddPersonOnClick(string posx, string posy)
    {
        int cost = CalculateCost();
        Debug.Log("POS:" + posx + "," + posy);
        Debug.Log("COST:" + cost);
        if (cost <= currentBudget)
        {
            currentBudget -= cost;
            DatabaseController dbCont = new DatabaseController();
            dbCont.update("INSERT INTO Army " +
                "(teamNumber,class,armor,shield,weapon," +
                "isLeader,pos_x,pox_z) " +
                "VALUES " +
                "(0," +
                $"'{characters.options[characters.value].text}'," +
                $"'{armors.options[armors.value].text}'," +
                "'Mundane shield'," +
                $"'{weapons.options[weapons.value].text}'," +
                "'0'," +
                $"{posx}," +
                $"{posy}); ");
            dbCont.CloseDB();
        }
        else
        {
            warning.SetActive(true);
        }
    }




}