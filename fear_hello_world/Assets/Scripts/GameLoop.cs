using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
	public int currentPlayer;
    public int actionPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (actionPoints == 0)
        {
            currentPlayer = 1 - currentPlayer;
            actionPoints = 3;
        }
    }
}
