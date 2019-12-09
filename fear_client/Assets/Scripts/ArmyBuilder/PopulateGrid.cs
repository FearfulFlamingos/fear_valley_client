using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
    public GameObject prefab; // button prefab
    public int numberOfButtons;

    [SerializeField]
    private GameObject MainUI;
    // Start is called before the first frame update
    void Start()
    {
        Populate();   
    }

    void Populate()
    {
        
        GameObject newObj;
        int maxX = 8, maxZ = 2;
        Button[] buttons = new Button[numberOfButtons];

        for (int i = 0; i< numberOfButtons; i++)
        {
            newObj = Instantiate(prefab, transform) as GameObject;
            buttons[i] = newObj.GetComponent<Button>();
        }

        #region button setting
        buttons[0].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(0, 0); buttons[0].interactable = false; });
        buttons[1].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(1, 0); buttons[1].interactable = false; });
        buttons[2].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(2, 0); buttons[2].interactable = false; });
        buttons[3].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(3, 0); buttons[3].interactable = false; });
        buttons[4].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(4, 0); buttons[4].interactable = false; });
        buttons[5].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(5, 0); buttons[5].interactable = false; });
        buttons[6].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(6, 0); buttons[6].interactable = false; });
        buttons[7].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(7, 0); buttons[7].interactable = false; });
        buttons[8].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(0, 1); buttons[8].interactable = false; });
        buttons[9].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(1, 1); buttons[9].interactable = false; });
        buttons[10].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(2, 1); buttons[10].interactable = false; });
        buttons[11].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(3, 1); buttons[11].interactable = false; });
        buttons[12].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(4, 1); buttons[12].interactable = false; });
        buttons[13].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(5, 1); buttons[13].interactable = false; });
        buttons[14].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(6, 1); buttons[14].interactable = false; });
        buttons[15].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(7, 1); buttons[15].interactable = false; });
        //int halfway = buttons.Length / 2;
        //for (int i = 0; i< halfway;i++)
        //{
        //    int temp = i;
        //    //Debug.Log(temp);
        //    buttons[i].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(temp, 0); buttons[i].interactable = false; });
        //}

        //for (int i = halfway; i < buttons.Length; i++)
        //{
        //    int temp = i - halfway;
        //    //Debug.Log(temp);
        //    buttons[i].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(temp, 1); buttons[i].interactable = false; });
        //}

        #endregion
    }


}
