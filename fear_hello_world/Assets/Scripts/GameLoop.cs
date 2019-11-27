using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
	public int currentPlayer;
    public int actionPoints;
    public GameObject uiCanvas;
    public GameObject attackcanvas;
    public GameObject lastClicked;
    private int numAttacks;
    private GameObject scripts;
    // Start is called before the first frame update
    void Start()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
    }

    // Update is called once per frame
    void Update()
    {
        if (actionPoints == 0)
        {
            currentPlayer = 1 - currentPlayer;
            actionPoints = 3;
            numAttacks = 0;
        }
    }
    public void Attack()
    {
        lastClicked.GetComponent<PlayerAttack>().Attack();
    }
    public void CancelAttack()
    {
        attackcanvas.SetActive(false);
        Color mycolor = new Color32(223, 210, 194, 255);
        var cubeRenderer = lastClicked.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", mycolor);
        lastClicked.GetComponent<PlayerAttack>().DeactivateAttack();
        lastClicked.GetComponent<PlayerAttack>().enabled = false;
    }
    public void EndAttack()
    {
        attackcanvas.SetActive(false);
        Color mycolor = new Color32(223, 210, 194, 255);
        var cubeRenderer = lastClicked.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", mycolor);
        lastClicked.GetComponent<PlayerAttack>().DeactivateAttack();
        lastClicked.GetComponent<PlayerAttack>().enabled = false;
        numAttacks++;
        actionPoints++;
    }
}
