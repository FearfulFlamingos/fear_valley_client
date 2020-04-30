using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts.ArmyBuilder
{
    public class FeaturesHolder : MonoBehaviour
    {
        // Start is called before the first frame update
        public PopulateGrid.Build TroopInfo { set; get; }
        public int Cost { set; get; }
        public bool IsActive { set; get; }
    }
}