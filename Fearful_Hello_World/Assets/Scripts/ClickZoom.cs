using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickZoom : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cam.fieldOfView = 30;
        }
        if (Input.GetMouseButtonUp(1))
        {
            cam.fieldOfView = 50;
        }
    }

}
