using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionScrollView;
    private Stack<GameObject> explosions;

    public GameObject selection;
    public Camera camera;
    [SerializeField]
    private TMP_Text remainingBudget;
    [SerializeField]
    private TMP_Text projectedCost;
    public int budget= 300;

    // Start is called before the first frame update
    void Start()
    {
        explosions = new Stack<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            RemovePlacedObject();
        if (selection != null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit,100.0f,1<<9))
            {
                Debug.Log("Moving");
                selection.transform.position = hit.point;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            selection = null;
        }

        remainingBudget.text = $"Budget: {budget}";
    }

    //void Populate()
    //{

    //    GameObject newObj;
    //    int maxX = 8, maxZ = 2;
    //    Button[] buttons = new Button[numberOfButtons];

    //    for (int i = 0; i< numberOfButtons; i++)
    //    {
    //        newObj = (GameObject) Instantiate(Resources.Load("UI/ArmyBuild/GUIButton"), transform) as GameObject;
    //        buttons[i] = newObj.GetComponent<Button>();
    //    }

    #region button setting
    //    buttons[0].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(0, 1); buttons[0].interactable = false; });
    //    buttons[1].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(1, 1); buttons[1].interactable = false; });
    //    buttons[2].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(2, 1); buttons[2].interactable = false; });
    //    buttons[3].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(3, 1); buttons[3].interactable = false; });
    //    buttons[4].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(4, 1); buttons[4].interactable = false; });
    //    buttons[5].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(5, 1); buttons[5].interactable = false; });
    //    buttons[6].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(6, 1); buttons[6].interactable = false; });
    //    buttons[7].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(7, 1); buttons[7].interactable = false; });
    //    buttons[8].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(0, 0); buttons[8].interactable = false; });
    //    buttons[9].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(1, 0); buttons[9].interactable = false; });
    //    buttons[10].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(2, 0); buttons[10].interactable = false; });
    //    buttons[11].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(3, 0); buttons[11].interactable = false; });
    //    buttons[12].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(4, 0); buttons[12].interactable = false; });
    //    buttons[13].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(5, 0); buttons[13].interactable = false; });
    //    buttons[14].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(6, 0); buttons[14].interactable = false; });
    //    buttons[15].onClick.AddListener(() => { MainUI.GetComponent<UIPopulate>().OnClickAddTroop(7, 0); buttons[15].interactable = false; });
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

    #region Upper Panel
    public void SetSelection(string resourceName)
    {
        GameObject resource = (GameObject) Instantiate(Resources.Load(resourceName)) as GameObject;
        selection = resource;
        selection.transform.position = new Vector3(0,0.2f,10);

    }

    public void RemovePlacedObject()
    {
        Debug.Log("Checking");
        RaycastHit checkGameObject;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out checkGameObject, 100.0f,1<<10) && checkGameObject.transform != null)
        {
            Debug.Log("Deleting placed");
            Destroy(checkGameObject.transform.gameObject);
        }
    }

    #endregion

    #region Lower Panel
    /// <summary>
    /// Adds an explosion to the bottom panel in the army build visual scene.
    /// </summary>
    public void AddExplosion() //TODO: Update cost string
        {
            GameObject newExplosion = (GameObject)Instantiate(Resources.Load("UI/ArmyBuild/ExplosionImage"), explosionScrollView.transform);
            explosions.Push(newExplosion);
        }
    /// <summary>
    /// 
    /// </summary>
    public void RemoveExplosion() //TODO: Update Cost string
    {
        if (explosions.Count != 0)
        {
            GameObject oldExplosion = explosions.Pop();
            Destroy(oldExplosion);
        }
        
    }
    #endregion
}
