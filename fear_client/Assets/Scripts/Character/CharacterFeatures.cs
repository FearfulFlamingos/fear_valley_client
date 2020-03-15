using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is largely used to keep track of information on each of the game objects vital to making the game work
/// </summary>
public class CharacterFeatures : MonoBehaviour
{

    public int team;
    public int health;
    public int troopId;
    public string charclass;
    //public string armclass;
    //public string weapon;
    public int attack;
    public int damageBonus;
    public int movement;
    public int perception;
    public int magicattack;
    public int magicdamage;
    public int armorBonus;
    public int stealth;
    public int attackRange;
    public int damage;
    public int isLeader;
    //public GameObject attackRange;
    public bool isFocused, isAttacking;
    public int numMagAtk;
 

    void Start()
    {
        isAttacking = false;
    }

    // Update is called once per frame
    /// <summary>
    /// This function called once per frame is used to update the colors and activation of the circles as they
    /// are changed by functions like player attack and player spotlight. This allows us to simplify code in other functions
    /// </summary>
    void Update()
    {
        var cubeRenderer = gameObject.GetComponent<Renderer>();
        if (isFocused)
        {
            cubeRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            if (!isAttacking)
            {
                Color mycolor = new Color32(223, 210, 194, 255);
                cubeRenderer.material.SetColor("_Color", mycolor);
            }
        }
        if (isAttacking)
        {
            isFocused = false;
            cubeRenderer.material.SetColor("_Color", Color.red);
            //attackRange.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            if (isFocused)
            {
                //attackRange.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                Color mycolor = new Color32(223, 210, 194, 255);
                cubeRenderer.material.SetColor("_Color", mycolor);
                //attackRange.GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
