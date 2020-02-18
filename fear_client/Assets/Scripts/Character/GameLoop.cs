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
    /// <summary>
    /// This function is constantly checking if action points have dipped below 0 at which point the next turn is triggered
    /// </summary>
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
    /// <summary>
    /// This function is used to create dictionaries for each player. The dicitionaries are used to track
    /// the number of active figures each player currently has and it tracks a reference to the gameobjects.
    /// </summary>
    /// <param name="team"></param>
    /// <param name="troopid">Incremental Id goes up by 1 for each new troop</param>
    /// <param name="reference">Reference to the GameObject</param>
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
    /// <summary>
    /// This is a networking function that is called by the client script eachtime the other player moves.
    /// Once this message is received the gameobject is retrieved from the dictionary and then the move
    /// function is triggered.
    /// </summary>
    /// <param name="troopid"></param>
    /// <param name="newX"></param>
    /// <param name="newZ"></param>
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
    /// <summary>
    /// This is another networking function and it currently is used to remove health from a specified
    /// troop after another player succesfully attacks them.
    /// </summary>
    /// <param name="troopid"></param>
    /// <param name="damage"></param>
    public void IveBeenAttacked(float troopid, float damage)
    {
        GameObject changing = p1CharsDict[System.Convert.ToInt32(troopid)];
        CharacterFeatures reference = changing.GetComponent<CharacterFeatures>();
        reference.health = System.Convert.ToInt32(reference.health - damage);
    }
    /// <summary>
    /// This is called after an attack is triggered and sends the message to the player attack script.
    /// </summary>
    public void Attack()
    {
        lastClicked.GetComponent<PlayerAttack>().Attack();
        actionPoints--;
    }

    /// <summary>
    /// This function is called after the cancel attack button is pressed and it deactivates attack mode.
    /// </summary>
    public void CancelAttack()
    {
        attackcanvas.SetActive(false);
        lastClicked.GetComponent<PlayerAttack>().DeactivateAttack();
        lastClicked.GetComponent<PlayerAttack>().enabled = false;
    }
    /// <summary>
    /// This function is used after the end of an attack to deactivate attack mode and track the number
    /// of attacks.
    /// </summary>
    public void EndAttack()
    {
        attackcanvas.SetActive(false);
        lastClicked.GetComponent<PlayerAttack>().DeactivateAttack();
        lastClicked.GetComponent<PlayerAttack>().enabled = false;
        numAttacks++;
        //actionPoints++;
    }
    /// <summary>
    /// This function is called when the game is finished and the player clicks the x button.
    /// </summary>
    public void endGame()
    {
        SceneManager.LoadScene("ArmyBuilder");
    }
    /// <summary>
    /// This is a networking function used when other players call a retreat in the game.
    /// </summary>
    /// <param name="troopId">Troop retreating</param>
    /// <param name="TeamNum">The team that is leaving</param>
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
    /// <summary>
    /// This function is called when the local player calls a retreat on their figure.
    /// </summary>
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

    /// <summary>
    /// This function is checked each time any damage is done. It checks to see if the player's health is
    /// 0. Following this the function checks if the endgame has been trigged.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="troopId"></param>
    /// <param name="playerInQuestion"></param>
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
            if (playerInQuestion == 2)
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
