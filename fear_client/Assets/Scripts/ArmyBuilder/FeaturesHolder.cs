using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeaturesHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public string fname, troop, weapon, armor;
    public bool isactive;
    public GameObject uicontrol,gamepiece;

    void Start()
    {
        //isactive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isactive == true){
            uicontrol.GetComponent<PopulateGrid>().ChangeChar(gamepiece);
        }
    }
}
