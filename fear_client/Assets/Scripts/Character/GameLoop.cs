using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameLoop : MonoBehaviour
{
	public int currentPlayer;
    public int actionPoints;
    public GameObject uiCanvas;
    public GameObject attackcanvas,victorycanvas;
    public GameObject lastClicked;
    public TMP_Text victoryStatement;
    public Dictionary<int,GameObject> p1CharsDict, p2CharsDict;
    private int numAttacks;
    private GameObject scripts;
    
    // Start is called before the first frame update
    void Start()
    {
        actionPoints = 3;
        scripts = GameObject.FindGameObjectWithTag("scripts");
        p1CharsDict = new Dictionary<int,GameObject>();
        p2CharsDict = new Dictionary<int,GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (actionPoints == 0)
        {
            currentPlayer = 1;// - currentPlayer;
            actionPoints = 3;
            numAttacks = 0;
            Client.Instance.SendEndTurn();
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
        //Debug.Log(newX);
        //Debug.Log(newZ);
        //float adjX = 4 + (4 - newX);
        //float adjZ = 4 + (4 - newZ);
        Vector3 newVect = new Vector3(newX, 0, newZ);
        Debug.Log(newVect);
        changing.GetComponent<PlayerMovement>().MoveMe(newVect);
    }
    public void IveBeenAttacked(float troopid, float damage)
    {
        GameObject changing = p1CharsDict[System.Convert.ToInt32(troopid)];
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
        //actionPoints++;
    }
    public void endGame()
    {
        SceneManager.LoadScene("ArmyBuilder");
    }
    public void OtherLeaves(int troopId,int TeamNum)
    {
        Debug.Log($"Retreat message received with {TeamNum} and {troopId}");
        if (TeamNum == 1)
        {
            GameObject destroy = p1CharsDict[troopId];
            PlayerRemoval("Attack", troopId, TeamNum);
            Destroy(destroy);
        }
        
        else {
            GameObject destroy = p2CharsDict[troopId];
            PlayerRemoval("Retreat", troopId, TeamNum);
            Destroy(destroy);
        }
            
    }
    public void Leave()
    {
        if (scripts.GetComponent<GameLoop>().actionPoints >= 3)
        {
            uiCanvas.SetActive(false);
            Destroy(lastClicked.GetComponent<CharacterFeatures>().myCircle);
            Destroy(lastClicked.GetComponent<CharacterFeatures>().attackRange);
            PlayerRemoval("Retreat", lastClicked.GetComponent<CharacterFeatures>().troopId, 1);
            Destroy(lastClicked);
            Client.Instance.SendRetreatData(lastClicked.GetComponent<CharacterFeatures>().troopId,2);
            scripts.GetComponent<GameLoop>().actionPoints = 0;

        }
    }
    public void PlayerRemoval(string action, int troopId, int playerInQuestion)
    {
        if (action == "Retreat")
        {
            if (playerInQuestion == 1)
            {
                p1CharsDict.Remove(troopId);
                if (p1CharsDict.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"You have fled the battlefield. ";
                }
            }
            else
            {
                p2CharsDict.Remove(troopId);
                if (p2CharsDict.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Your enemy has retreated!";
                }
            }
        }
        else
        {
            if (playerInQuestion == 2)
            {
                p2CharsDict.Remove(troopId);
                if (p2CharsDict.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"Your foes lie defeated! Victory!";
                }
            }
            else
            {
                p1CharsDict.Remove(troopId);
                if (p1CharsDict.Count == 0)
                {
                    victorycanvas.SetActive(true);
                    attackcanvas.SetActive(false);
                    victoryStatement.text = $"You have suffered a terrible defeat.";
                }
            }
        }
    }
}
