using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFeatures : MonoBehaviour
{
    public int team;
    public int health;
    public string charclass;
    public string armclass;
    public string shield;
    public string weapon;
    public bool isLeader;

    public int GetTeam() => this.team;
    public int GetHealth() => this.health;
    public string GetClass() => this.charclass;
    public string GetArmor() => this.armclass;
    public string GetShield() => this.shield;
    public string GetWeapon() => this.weapon;
    public bool IsLeader() => this.isLeader;


}
