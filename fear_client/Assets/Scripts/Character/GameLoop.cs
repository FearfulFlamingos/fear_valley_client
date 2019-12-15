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
    public Dictionary<int,GameObject> p1CharsDict, p2CharsDict;
    private int numAttacks;
    private GameObject scripts;
    
    // Start is called before the first frame update
    void Start()
    {
        scripts = GameObject.FindGameObjectWithTag("scripts");
        p1CharsDict = new Dictionary<int,GameObject>();
        p2CharsDict = new Dictionary<int,GameObject>();

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
    public void AddtoDict(int team,int troopid, GameObject reference)
    {
        if (team == 1)
        {
            p1CharsDict.Add(troopid, reference);
        }
        else
        {
            p2CharsDict.Add(troopid, reference);
        }
    }
    public void MoveOther(int troopid, float newX, float newZ)
    {
        GameObject changing = p2CharsDict[troopid];
        Vector3 newVect = new Vector3(newX, 0, newZ);
        changing.GetComponent<PlayerMovement>().MoveMe(newVect);
    }
    public void IveBeenAttacked(int troopid, int damage)
    {
        GameObject changing = p1CharsDict[troopid];
        CharacterFeatures reference = changing.GetComponent<CharacterFeatures>();
        reference.health = System.Convert.ToInt32(reference.health - damage);
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
            PlayerRemoval("Retreat", lastClicked.GetComponent<CharacterFeatures>().troopId);
            Destroy(lastClicked);

            scripts.GetComponent<GameLoop>().actionPoints = 0;

        }
    }
    public void PlayerRemoval(string action, int troopId)
    {
        if (action == "Retreat")
        {
            if (currentPlayer == 0)
            {
                p1CharsDict.Remove(troopId);
                if (p1CharsDict.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Victory has been acheived for \nplayer 2 after player 1 retreated ";
                }
            }
            else
            {
                p2CharsDict.Remove(troopId);
                if (p2CharsDict.Count == 0)
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
                p2CharsDict.Remove(troopId);
                if (p2CharsDict.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Victory has been acheived for \nplayer 1 after defeating player 2 ";
                }
            }
            else
            {
                p1CharsDict.Remove(troopId);
                if (p1CharsDict.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Victory has been acheived for \nplayer 2 after defeating player 1 ";
                }
            }
        }
    }
}
