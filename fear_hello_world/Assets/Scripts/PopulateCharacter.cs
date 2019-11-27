using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class PopulateCharacter : MonoBehaviour
{
    Dictionary<string, string> objectReference = new Dictionary<string, string>();
    
    

    List<(object teamNum, object charClass, object armor, object weapon, object health,
        object leader, object xpos, object zpos, object attack, object damageBonus, object movement,
        object perception, object magicAttack, object magicDamage, object armorBonus, object armorStealth, object damage)> armyList =
        new List<(object teamNum, object charClass, object armor, object weapon, object health,
        object leader, object xpos, object zpos, object attack, object damageBonus, object movement,
        object perception, object magicAttack, object magicDamage, object armorBonus, object armorStealth, object damage)>();

    private void Start()
    {
        objectReference.Add("Peasant", "peasantprefab");
        objectReference.Add("Trained Warrior", "warriorprefab");
        objectReference.Add("Magic User", "wizardprefab");
        
        //DuplicateObjects();
        string connection = "URI=file:" + Application.dataPath + "/Data/fearful_data.sqlite";
        Debug.Log(connection);
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT a.teamNumber, t.class, ar.armor, w.name, " +
            "t.health, a.isLeader, a.pos_x,a.pox_z,t.attack, t.damage, t.movement,t.perception " +
            ", t.magicattack, t.magicDamage, ar.bonus,ar.stealth, w.damage FROM Army a, Armor ar, Troop t, Weapon w Where a.class = t.class " +
            " and a.armor = ar.armor and a.weapon = w.name";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        while (reader.Read())
        {

            var competitor = (teamNum: reader[0], charClass: reader[1], armor: reader[2],
                weapon: reader[3], health: reader[4], leader: reader[5],
                xpos: reader[6], zpos: reader[7], attack: reader[8], damageBonus: reader[9],
                movement: reader[10], perception: reader[11], magicAttack: reader[12], magicDamage: reader[13],
                armorBonus: reader[14], armorStealth: reader[15], damage: reader[16]);
            armyList.Add(competitor);
            Debug.Log(objectReference[competitor.charClass.ToString()]);
        }


        dbcon.Close();
        for (var i = 0; i < armyList.Count; i++)
        {
            Debug.Log("Amount is "+armyList[i].xpos+" and type is " + armyList[i].zpos);
            DuplicateObjects(objectReference[armyList[i].charClass.ToString()], armyList[i]);
            if (System.Convert.ToInt32(armyList[i].zpos) == -21)
            {
                var newArmy = (teamNum: 1, charClass: armyList[i].charClass, armor: armyList[i].armor,
                    weapon: armyList[i].weapon, health: armyList[i].health, leader: armyList[i].leader, xpos: armyList[i].xpos,
                    zpos: 0, attack: armyList[i].attack, damageBonus: armyList[i].damageBonus, movement: armyList[i].movement, perception: armyList[i].perception,
                    magicAttack: armyList[i].magicAttack, magicDamage: armyList[i].magicDamage, armorBonus: armyList[i].armorBonus, armorStealth: armyList[i].armorStealth, damage: armyList[i].damage);
                DuplicateObjects(objectReference[armyList[i].charClass.ToString()], newArmy);
            }
            else
            {
                var newArmy = (teamNum: 1, charClass: armyList[i].charClass, armor: armyList[i].armor,
                                    weapon: armyList[i].weapon, health: armyList[i].health, leader: armyList[i].leader, xpos: armyList[i].xpos,
                                    zpos: 0, attack: armyList[i].attack, damageBonus: armyList[i].damageBonus, movement: armyList[i].movement, perception: armyList[i].perception,
                                    magicAttack: armyList[i].magicAttack, magicDamage: armyList[i].magicDamage, armorBonus: armyList[i].armorBonus, armorStealth: armyList[i].armorStealth, damage: armyList[i].damage);
                DuplicateObjects(objectReference[armyList[i].charClass.ToString()], newArmy);
            }
            
        }

    }

    //This function is desgined to create each instance of the game objects. It is called for every character created by the game and populates all of the characteristics of the character.
    private void DuplicateObjects(string prefab, (object teamNum, object charClass, object armor, object weapon, object health,
        object leader, object xpos, object zpos, object attack, object damageBonus, object movement,
        object perception, object magicAttack, object magicDamage, object armorBonus, object armorStealth, object damage) characterInfo)
    {
        //GameObject referenceTile = (GameObject)Instantiate(Resources.Load(prefab));
        GameObject tile = (GameObject)Instantiate(Resources.Load(prefab));
        GameObject circle = (GameObject)Instantiate(Resources.Load("circleprefab"));
        GameObject circle2 = (GameObject)Instantiate(Resources.Load("circleprefab"));
        int xPos = System.Convert.ToInt32(characterInfo.xpos);
        int yPos = 0;
        int zPos = System.Convert.ToInt32(characterInfo.zpos);
        Debug.Log(zPos);
        float floating = 0.2F;
        tile.transform.position = new Vector3(xPos,yPos,zPos);
        circle.transform.position = new Vector3(xPos,floating, zPos);
        circle.transform.localScale = new Vector3(21, 21, 21);
        circle2.transform.position = new Vector3(xPos, floating, zPos);
        circle2.transform.localScale = new Vector3(9, 9, 9);
        circle.GetComponent<Renderer>().enabled = false;
        circle2.GetComponent<Renderer>().enabled = false;
        CharacterFeatures referenceScript = tile.GetComponent<CharacterFeatures>();
        referenceScript.team = System.Convert.ToInt32(characterInfo.teamNum);
        referenceScript.health = System.Convert.ToInt32(characterInfo.health);
        referenceScript.attack = System.Convert.ToInt32(characterInfo.attack);
        referenceScript.weapon = characterInfo.weapon.ToString();
        referenceScript.armclass = characterInfo.armor.ToString();
        referenceScript.damageBonus = System.Convert.ToInt32(characterInfo.damageBonus);
        referenceScript.movement = System.Convert.ToInt32(characterInfo.movement);
        referenceScript.perception = System.Convert.ToInt32(characterInfo.perception);
        //referenceScript.magicattack = System.Convert.ToInt32(characterInfo.magicAttack);
        //referenceScript.magicdamage = System.Convert.ToInt32(characterInfo.magicDamage);
        referenceScript.bonus = System.Convert.ToInt32(characterInfo.armorBonus);
        referenceScript.stealth = System.Convert.ToInt32(characterInfo.armorStealth);
        referenceScript.damage = System.Convert.ToInt32(characterInfo.damage);
        referenceScript.isLeader = System.Convert.ToInt32(characterInfo.leader);
        referenceScript.charclass = characterInfo.charClass.ToString();
        referenceScript.myCircle = circle;
        referenceScript.attackRange = circle2;
        referenceScript.isFocused = false;


        tile.GetComponent<PlayerMovement>().enabled = false;
        tile.GetComponent<PlayerAttack>().enabled = false;


    }


}
