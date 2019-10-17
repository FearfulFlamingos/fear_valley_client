using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class Character : MonoBehaviour
{
    List<(object teamNum, object charClass, object armor, object shield, object weapon, object health, object leader)> armyList = new List<(object teamNum, object charClass, object armor, object shield, object weapon, object health, object leader)>();
    private void Start()
    {
        string connection = "URI=file:" + Application.dataPath + "/Data/fearful_data";
        Debug.Log(connection);
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        cmnd_read.CommandText = "INSERT INTO Army (teamNumber,class,armor,shield,weapon,currentHealth,isLeader) VALUES (2,1,3,5,2,16,FALSE)";
        cmnd_read.ExecuteNonQuery();
        IDataReader reader;
        string query = "SELECT a.id, teamNumber, t.class, ar.armor, ar.armor, w.name, a.currentHealth, a.isLeader FROM Army a, Weapon w, Armor ar, Troop t WHERE a.class = t.id AND a.armor = ar.id AND a.weapon = w.id;";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        dbcon.Close();
        while (reader.Read())
        {
            
            var competitor = (teamNum: reader[1], charClass: reader[2], armor: reader[3], shield: reader[4], weapon: reader[5], health: reader[6], leader: reader[7]);
            armyList.Add(competitor);
            Debug.Log(armyList[0]);
            Debug.Log(reader[1]);
            Debug.Log("in");
        }


        dbcon.Close();

    }


}
