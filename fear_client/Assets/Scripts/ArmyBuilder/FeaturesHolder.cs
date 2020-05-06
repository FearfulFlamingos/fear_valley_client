using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts.ArmyBuilder
{
    /// <summary>
    /// Smaller wrapper class to keep track of the choices the player made when building the character.
    /// </summary>
    /// <remarks>
    /// This could certainly be a POCO, but for such a small script it would be less efficient to create
    /// a wrapper so that it could be attached to a GameObject.
    /// </remarks>
    public class FeaturesHolder : MonoBehaviour
    {
        /// <summary>
        /// Options selected for this particular troop.
        /// See <see cref="PopulateGrid.Build"/> for more information.
        /// </summary>
        public PopulateGrid.Build TroopInfo { set; get; }
        
        /// <summary>Cost of the character. Used with <see cref="PopulateGrid.CancelPurchase"/> to refund.</summary>
        public int Cost { set; get; }
        
        /// <summary>Whether this is the last placed character.</summary>
        public bool IsActive { set; get; }
    }
}