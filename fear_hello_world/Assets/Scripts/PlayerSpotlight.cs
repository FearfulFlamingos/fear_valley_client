using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpotlight : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    private void Start()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }
    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("Frame");
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Physics.Raycast(ray, out hit, 100.0f));
            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                if (hit.transform != null)
                {
                    //PrintName(hit.transform.gameObject);
                    if (hit.transform.gameObject.name == "King")
                    {
                        camera1.gameObject.SetActive(false);
                        camera2.gameObject.SetActive(true);

                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);
        }
        

    }

    private void PrintName(GameObject go)
    {
        print(go.name);
    }
    //private void switchCamera(int camPosition)
    //{
    //    if (camPosition > 1)
    //    {
    //        camPosition = 0;
    //    }

    //    PlayerPrefs.SetInt("CameraPosition", camPosition);
    //    if (camPosition == 0)
    //    {
    //        camera1.SetActive(true);
    //        camera2.SetActive(false);
    //    }
    //    if (camPosition == 1)
    //    {
    //        camera1.SetActive(false);
    //        camera2.SetActive(true);
    //    }
    }

