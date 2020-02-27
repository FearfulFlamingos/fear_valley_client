using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int bonus;
    public int stealth;
    public int damage;
    public int isLeader;
    public GameObject myCircle;
    public GameObject attackRange;
    public GameObject character;
    public bool isFocused, isAttacking;
 

    void Start()
    {
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        var cubeRenderer = character.GetComponent<Renderer>();
        if (isFocused)
        {
            myCircle.GetComponent<Renderer>().enabled = true;
            cubeRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            if (isAttacking)
            {
                myCircle.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                Color mycolor = new Color32(223, 210, 194, 255);
                cubeRenderer.material.SetColor("_Color", mycolor);
                myCircle.GetComponent<Renderer>().enabled = false;
            }
        }
        if (isAttacking)
        {
            isFocused = false;
            myCircle.GetComponent<Renderer>().enabled = false;
            cubeRenderer.material.SetColor("_Color", Color.red);
            attackRange.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            if (isFocused)
            {
                attackRange.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                Color mycolor = new Color32(223, 210, 194, 255);
                cubeRenderer.material.SetColor("_Color", mycolor);
                attackRange.GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
