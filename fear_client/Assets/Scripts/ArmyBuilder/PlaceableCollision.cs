using UnityEngine;

namespace Scripts.ArmyBuilder
{
    public class PlaceableCollision : MonoBehaviour
    {
        public bool shouldMove = true;


        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<PlaceableCollision>().shouldMove = false;
        }

        private void OnTriggerExit(Collider other)
        {
            other.gameObject.GetComponent<PlaceableCollision>().shouldMove = true;
        }


    }
}