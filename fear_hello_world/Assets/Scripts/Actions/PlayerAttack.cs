using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    public GameObject gameObject;
    private GameObject attackObject;
    private bool canAttack;
    private Camera camera1;
    private Text attackChar;
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
                    GameLoop currentPlayer = scripts.GetComponent<GameLoop>();
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
    public void Attack()
    {
        if (canAttack)
        {
            Debug.Log("worked");
        }
        else
        {
            attackChar.text = $"You can not attack this target\nthey are not in range. Select \nanother fighter to attack."; 
        }
    }
    public void ActivateAttack()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = false;
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        GameObject Circle = referenceScript.myCircle;
        Circle.GetComponent<Renderer>().enabled = false;
        GameObject Circle2 = referenceScript.attackRange;
        Circle2.GetComponent<Renderer>().enabled = true;
        
    }
    public void DeactivateAttack()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        scripts.GetComponent<PlayerSpotlight>().enabled = true;
        CharacterFeatures referenceScript = gameObject.GetComponent<CharacterFeatures>();
        GameObject Circle2 = referenceScript.attackRange;
        Circle2.GetComponent<Renderer>().enabled = false;
        attackChar.text = $"Name: Health: \nClass: \nDefense: \nWithin Range: ";
    }
}

