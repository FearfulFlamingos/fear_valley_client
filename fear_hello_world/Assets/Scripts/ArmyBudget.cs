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



    private void Start()
    {
        DatabaseController dbCont = new DatabaseController();
        costs = new Dictionary<string, int>();
        PopulateDropdown(characters, dbCont.ReadDB("class, cost","Troop"));
        PopulateDropdown(weapons, dbCont.ReadDB("name, cost","Weapon"));
        PopulateDropdown(armors, dbCont.ReadDB("armor, cost","Armor"));
        dbCont.CloseDB();
        
    }

    private void Update()
    {
        displayBudget.text = $"Current Resources: {currentBudget}";
    }

    private int CalculateCost()
    {
        foreach (var thing in costs) { Debug.Log(thing); }
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

    public void AddPersonOnClick()
    {
        int cost = CalculateCost();
        Debug.Log(cost);
        currentBudget -= cost;
    }

}