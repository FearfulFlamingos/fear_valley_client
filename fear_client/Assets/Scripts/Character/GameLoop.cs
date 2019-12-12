using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
	public int currentPlayer;
    public int actionPoints;
    public GameObject uiCanvas;
    public GameObject attackcanvas,victorycanvas;
    public GameObject lastClicked;
    public Text victoryStatement;
    public List<GameObject> p0Chars, p1Chars;
    private int numAttacks;
    private GameObject scripts;
    
    // Start is called before the first frame update
    void Start()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        List<GameObject> p1Chars = new List<GameObject>();
        List<GameObject> p0Chars = new List<GameObject>();

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
        actionPoints--;
    }
    public void CancelAttack()
    {
        attackcanvas.SetActive(false);
        lastClicked.GetComponent<PlayerAttack>().DeactivateAttack();
        lastClicked.GetComponent<PlayerAttack>().enabled = false;
    }
    public void EndAttack()
    {
        attackcanvas.SetActive(false);
        lastClicked.GetComponent<PlayerAttack>().DeactivateAttack();
        lastClicked.GetComponent<PlayerAttack>().enabled = false;
        numAttacks++;
        actionPoints++;
    }
    public void endGame()
    {
        SceneManager.LoadScene("ArmyBuilder");
    }
    public void Leave()
    {
        if (scripts.GetComponent<GameLoop>().actionPoints >= 3)
        {
            uiCanvas.SetActive(false);
            Destroy(lastClicked.GetComponent<CharacterFeatures>().myCircle);
            Destroy(lastClicked.GetComponent<CharacterFeatures>().attackRange);
            PlayerRemoval("Retreat", lastClicked);
            Destroy(lastClicked);

            scripts.GetComponent<GameLoop>().actionPoints = 0;

        }
    }
    public void PlayerRemoval(string action, GameObject deleteThis)
    {
        if (action == "Retreat")
        {
            if (currentPlayer == 0)
            {
                p0Chars.Remove(deleteThis);
                if (p0Chars.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Victory has been acheived for \nplayer 2 after player 1 retreated ";
                }
            }
            else
            {
                p1Chars.Remove(deleteThis);
                if (p1Chars.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Victory has been acheived for \nplayer 1 after player 2 retreated ";
                }
            }
        }
        else
        {
            if (currentPlayer == 0)
            {
                p1Chars.Remove(deleteThis);
                if (p1Chars.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Victory has been acheived for \nplayer 1 after defeating player 2 ";
                }
            }
            else
            {
                p0Chars.Remove(deleteThis);
                if (p0Chars.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Victory has been acheived for \nplayer 2 after defeating player 1 ";
                }
            }
        }
    }
}
