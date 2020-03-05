using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUpEnemies : MonoBehaviour
{
    HashSet<GameObject> enemiesInBlast = new HashSet<GameObject>();
    public int countEnemiesInBlast = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && other.gameObject.GetComponent<CharacterFeatures>().team == 2)
        {
            Debug.Log("Object entered");
            Debug.Log($"{other.gameObject.layer}");
            enemiesInBlast.Add(other.gameObject);
            countEnemiesInBlast++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10 && other.gameObject.GetComponent<CharacterFeatures>().team == 2)
        {
            Debug.Log("Object exited");
            enemiesInBlast.Remove(other.gameObject);
            countEnemiesInBlast--;
        }
    }

    public HashSet<GameObject> GetEnemiesInBlast()
    {
        return enemiesInBlast;
    }
}
