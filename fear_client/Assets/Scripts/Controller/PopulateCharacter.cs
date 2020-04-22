using Scripts.CharacterClass;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controller
{
    public class PopulateCharacter : MonoBehaviour
    {
        /// <summary>
        /// This function is designed to create all the objects fed in by the database
        /// it is currently only set up to create the objects for player 1 and another
        /// functions needs to be added to deal with a second player.
        /// </summary>
        /// <param name="features">The POCO character class instance.</param>
        /// <param name="xpos">Absolute x position on the board.</param>
        /// <param name="zpos">Absolute z position on the board.</param>
        public GameObject DuplicateObjects(ICharacterFeatures features, float xpos, float zpos)
        {
            GameObject tile = (GameObject)Instantiate(Resources.Load("GameCharacters/" + features.Charclass));
            GameObject board = GameObject.Find("/Cube");
            if (features.Team == 2)
            {
                // These positions need to be mirrored across x AND z axes, otherwise movements
                // start looking pretty strange as someone across the map suddenly hits you
                zpos = board.transform.localScale.z - zpos;
                xpos = board.transform.localScale.x - xpos;
                tile.layer = 11;
                tile.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
            }
            // Placing objects where they need to be and scaling them
            tile.transform.position = new Vector3(xpos, 0.2f, zpos);

            tile.GetComponent<Character>().Features = features;

            GameLoop getdictionary = gameObject.GetComponent<GameLoop>();
            getdictionary.AddtoDict(features.Team, features.TroopId, tile);

            return tile;
        }
    }
}