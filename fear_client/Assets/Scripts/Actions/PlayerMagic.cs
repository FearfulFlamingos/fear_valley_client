using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{

    private bool placingExplosion = false;
    private Vector3 startingPosition;
    private GameObject selection;
    public ParticleSystem explosionEffect;

    // Update is called once per frame
    private void Update()
    {
        if (placingExplosion)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, 1 << 9)) //Only look on ground layer
            {
                if (Vector3.Distance(hit.point, startingPosition) < 2)
                {
                    Debug.Log("Moving");
                    selection.transform.position = hit.point;
                } 
                
            }

            if (Input.GetMouseButtonDown(0))
            {
                CreateExplosion();
            }
        }
    }

    private void CreateExplosion()
    {
        placingExplosion = false;
        ParticleSystem ex = Instantiate(explosionEffect, selection.transform.position, Quaternion.identity);
        ex.Play();
        Destroy(selection);
        
    }

    public void PlaceExplosion()
    {
        selection = Instantiate(Resources.Load("MagicAttackAreaTemp")) as GameObject;
        startingPosition = transform.position;
        selection.transform.position = startingPosition;
        placingExplosion = true;
    }
        
}
