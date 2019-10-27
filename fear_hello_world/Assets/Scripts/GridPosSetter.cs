using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosSetter : MonoBehaviour
{
    [SerializeField]
    private int pos_x;
    [SerializeField]
    private int pos_y;
    
    private Color selectedColor = new Color(0, 233, 128);
    private Color normalColor = new Color(156, 144, 144);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int GetXPos()
    {
        return pos_x;
    }
    private int GetYPos()
    {
        return pos_y;
    }


}
