using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BfieldUIControl : MonoBehaviour
{
    // Actions Panel
    public TMP_Text actionsPanelLeftHandText, actionsPanelRightHandText;
    public TMP_Text moveButtonText, attackButtonText, magicButtonText, retreatButtonText;

    // Attack Panel
    public TMP_Text attackPanelAttackInfo, attackPanelEnemyInfo;
    public TMP_Text confirmAttackButtonText, cancelAttackButtonText, closeButtonText;

    // Victory Panel
    public TMP_Text victoryText, exitButtonText;

    public void ChangeText(TMP_Text textObject, string updatedText)
    {
        textObject.SetText(updatedText);
    }

}
