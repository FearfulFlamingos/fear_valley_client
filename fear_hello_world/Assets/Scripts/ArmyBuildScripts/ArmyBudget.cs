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


    /// <summary>
    /// This adds functionality to the buttons, fills dropdown menus,
    /// and reads costs into a dict for later comparison.
    /// </summary>
    private void Start()
    {

    }

    private void Update()
    {

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