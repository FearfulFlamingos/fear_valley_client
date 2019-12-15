using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    public GameObject gameObject;
    private GameObject attackObject;
    private bool canAttack, timeToDistroy;
    private Camera camera1;
    private GameObject xbutton, attackbutton, cancelbutton;
    private TMP_Text attackChar;
    private GameObject uiCanvas;
    private GameObject scripts;

    // Start is called before the first frame update
    void Start()
    {
        
        //uiCanvas = GameObject.FindGameObjectWithTag("PlayerAction");
        scripts = GameObject.FindGameObjectWithTag("scripts");
        attackChar = scripts.GetComponent<PlayerSpotlight>().attackChar;
        uiCanvas = scripts.GetComponent<PlayerSpotlight>().attackcanvas;
        camera1 = scripts.GetComponent<PlayerSpotlight>().camera1;
        xbutton = scripts.GetComponent<PlayerSpotlight>().xbutton;
        attackbutton = scripts.GetComponent<PlayerSpotlight>().attackbutton;
        cancelbutton = scripts.GetComponent<PlayerSpotlight>().cancelbutton;
        timeToDistroy = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Physics.Raycast(ray, out hit, 100.0f));
            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                if (hit.transform != null)
                {
                    attackObject = hit.transform.gameObject;
                    
                    CharacterFeatures referenceScript = attackObject.GetComponent<CharacterFeatures>();

                    if (referenceScript.team != gameObject.GetComponent<CharacterFeatures>().team)
                    {
                        if(WithinRange(3.5F, attackObject.transform.position[0],attackObject.transform.position[2], gameObject.transform.position[0], gameObject.transform.position[2], false))
                        {
                            attackChar.text = $"Name: Roman\nHealth: {referenceScript.health}\nClass: {referenceScript.charclass}\nDefense: {referenceScript.damageBonus}\nWithin Range: Yes";
                            canAttack = true;
                        }
                        else
                        {
                            attackChar.text = $"Name: Roman\nHealth: {referenceScript.health}\nClass: {referenceScript.charclass}\nDefense: {referenceScript.damageBonus}\nWithin Range: No";
                            canAttack = false;
                        }

                    }
                    else
                    {
                        attackChar.text = $"You can not attack\nyour own team.";
                    }
                    
                }
            }
        }

    }
    public bool WithinRange(float maxDistance, float newPointX, float newPointZ, float curPointX, float curPointZ, bool ranged)
    {

        float squaredX = (newPointX - curPointX) * (newPointX - curPointX);
        float squaredZ = (newPointZ - curPointZ) * (newPointZ - curPointZ);
        float result = Mathf.Sqrt(squaredX + squaredZ);
        Debug.Log("IAMHERE");
        Debug.Log(result);
        if (maxDistance >= result)
        {
            return true;
        }

        return false;
    }

    public void ActivateAttack()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = false;
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        referenceScript.isAttacking = true;

    }
    public void Attack()
    {
        System.Random random = new System.Random();
        CharacterFeatures referenceScript = attackObject.GetComponent<CharacterFeatures>();
        CharacterFeatures referenceScript2 = gameObject.GetComponent<CharacterFeatures>();
        GameLoop gamevars = scripts.GetComponent<GameLoop>();
        if (canAttack)
        {
            
            if (random.Next(0,20)+referenceScript2.attack >= 10+referenceScript.bonus)
            {
                int damageTaken = random.Next(1, (referenceScript2.damage))+referenceScript2.damageBonus;
                if ((referenceScript.health - damageTaken) <= 0)
                {
                    attackChar.text = $"You have dealt fatal damage\nto the player named Roman ";
                    timeToDistroy = true;

                    gamevars.PlayerRemoval("Attack", attackObject.GetComponent<CharacterFeatures>().troopId);
                    Destroy(attackObject);
                    Destroy(attackObject.GetComponent<CharacterFeatures>().myCircle);
                    Destroy(attackObject.GetComponent<CharacterFeatures>().attackRange);
                }
                else
                {
                    referenceScript.health = System.Convert.ToInt32(referenceScript.health - damageTaken);
                    scripts.GetComponent<Client>().SendAttackData(referenceScript.troopId,damageTaken);
                    attackChar.text = $"You attack was a success \nand you have dealt {damageTaken} damage\nto the player named Roman ";
                }
                
            }
            else
            {
                attackChar.text = $"You could not get passed their armor\nyour attack has failed";
            }
            xbutton.SetActive(true);
            attackbutton.SetActive(false);
            cancelbutton.SetActive(false);
        }
        else
        {
            attackChar.text = $"You can not attack this target\nthey are not in range. Select \nanother fighter to attack.";
        }
    }


    public void DeactivateAttack()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = true;
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        attackbutton.SetActive(true);
        cancelbutton.SetActive(true);
        referenceScript.isAttacking = false;
        attackChar.text = $"Name: Health: \nClass: \nDefense: \nWithin Range: ";
        if (timeToDistroy)
        {
            Debug.Log("__________________________________________________________" + timeToDistroy);
            timeToDistroy = false;
            Destroy(attackObject.GetComponent<CharacterFeatures>().myCircle);
            Destroy(attackObject.GetComponent<CharacterFeatures>().attackRange);
            Destroy(attackObject);
        }
    }
}

