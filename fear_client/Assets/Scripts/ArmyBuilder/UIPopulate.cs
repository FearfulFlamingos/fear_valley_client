using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopulate : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown troopsDropdown;
    [SerializeField]
    private TMP_Dropdown weaponsDropdown;
    [SerializeField]
    private TMP_Dropdown armorsDropdown;

    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private TMP_Text remainingBudget;
    [SerializeField]
    private TMP_Text projectedCost;

    private int budget = 300;

    #region Dropdown values
    // Keys in costs MUST MATCH List values!
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
    private List<string> troopVals = new List<string>()
    {
        "No troop",
        "Peasant",
        "Trained Warrior",
        "Magic User"
    };
    private List<string> weaponVals = new List<string>()
    {
        "Unarmed",
        "Polearm",
        "Two-handed weapon",
        "One-handed weapon",
        "Ranged attack",
        "Magical Explosion"
    };
    private List<string> armorVals = new List<string>()
    {
        "Unarmored",
        "Light mundane armor",
        "Light magical armor",
        "Heavy mundane armor",
        "Heavy magical armor"
    };
    #endregion
    
    #region MonoBehavior
    // Start is called before the first frame update
    void Start()
    {
        troopsDropdown.ClearOptions();
        weaponsDropdown.ClearOptions();
        armorsDropdown.ClearOptions();
        
        troopsDropdown.AddOptions(troopVals);
        weaponsDropdown.AddOptions(weaponVals);
        armorsDropdown.AddOptions(armorVals);

        warning.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        remainingBudget.text = $"Budget: {budget}";
        projectedCost.text = $"Projected Cost: {CalculateCost()}";
    }
    #endregion
    
    /// <summary>
    /// This function checks the value of the dropdown menus against the cost,
    /// which is stored in a local dict.
    /// </summary>
    /// <returns></returns>
    private int CalculateCost()
    {
        
        int totalCost = 0;
        
        string troop = troopsDropdown.options[troopsDropdown.value].text;
        totalCost += costs[troop];
        //Debug.Log($"COST for {troop} = {costs[troop]}, TOTAL={totalCost}");

        string weap = weaponsDropdown.options[weaponsDropdown.value].text;
        totalCost += costs[weap];
        //Debug.Log($"COST for {weap} = {costs[weap]}, TOTAL={totalCost}");

        string armor = armorsDropdown.options[armorsDropdown.value].text;
        totalCost += costs[armor];
        //Debug.Log($"COST for {armor} = {costs[armor]}, TOTAL={totalCost}");

        return totalCost;
    }

    public void OnClickAddTroop(int posx, int posz)
    {
        int cost = CalculateCost();
        if (cost <= budget)
        {
            budget -= cost;
            string troop = troopsDropdown.options[troopsDropdown.value].text;
            string weap = weaponsDropdown.options[weaponsDropdown.value].text;
            string armor = armorsDropdown.options[armorsDropdown.value].text;
            Client.Instance.AddTroopRequest(troop, weap, armor, 0, 0);
        }
        else
        {
            warning.SetActive(true);
        }
    }

}
