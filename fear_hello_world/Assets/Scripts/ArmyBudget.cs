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
    private DBStandin newDB = new DBStandin();
    private DatabaseController gameDB = new DatabaseController();
    public Text displayBudget;
    public Dropdown characters;
    public Dropdown weapons;
    public Dropdown armors;



    private void Start()
    {
        PopulateDropdown(characters, newDB.getArmy());
        PopulateDropdown(weapons, newDB.getWeapons());
        PopulateDropdown(armors, newDB.getArmor());
        
    }

    private void Update()
    {
        displayBudget.text = $"Current Resources: {currentBudget}";
    }

    private int calculateCost()
    {
        int totalCost = 0;
        string troop = characters.options[characters.value].text;
        totalCost += newDB.getArmy()[troop];

        string weap = weapons.options[weapons.value].text;
        totalCost += newDB.getWeapons()[weap];

        string armor = armors.options[weapons.value].text;
        totalCost += newDB.getArmor()[armor];

        return totalCost; 
    }

    private void PopulateDropdown(Dropdown drpdwn, Dictionary<string, int> optionsArray)
    {
        List<string> options = new List<string>();
        foreach (var option in optionsArray.Keys)
        {
            options.Add(option);
        }
        drpdwn.ClearOptions();
        drpdwn.AddOptions(options);
    }

    public void addPersonOnClick()
    {
        int cost = calculateCost();
        currentBudget -= cost;
    }
}

class DBStandin
{
    Dictionary<string, int> armyType = new Dictionary<string, int>()
        { {"None",0 }, {"Peasant",10}, {"Warrior",50},{"Mage", 100}  };
    Dictionary<string, int> weaponType = new Dictionary<string, int>()
        { {"None",0 }, {"1H",15 },{"2H", 20},{"Polearm",10},{"Ranged",25} };
    Dictionary<string, int> armorType = new Dictionary<string, int>()
        {{"None",0 },
        { "Light Mundane",20},
        { "Light Magic",40},
        { "Heavy Mundane",30},
        { "Heavy Magic",50},
        { "Mundane Shield", 10},
        { "Magic Shield",30 }};
    
    public Dictionary<string,int> getArmy()
    {
        return armyType;
    }
    public Dictionary<string, int> getWeapons()
    {
        return weaponType;
    }
    public Dictionary<string, int> getArmor()
    {
        return armorType;
    }
}
