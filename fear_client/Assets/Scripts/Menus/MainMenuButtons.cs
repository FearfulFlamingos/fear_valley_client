using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuButtons : MonoBehaviour
{
    public Button playButton, tutorialButton, hostButton, quitButton, creditsButton, optionsButton;
    public Canvas mainCanvas, optionsCanvas, creditsCanvas;
    public Button optionsReturn, creditsReturn;

    void Start()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(1));
        tutorialButton.onClick.AddListener(() => SceneManager.LoadScene(1));
        hostButton.onClick.AddListener(() => StartHost());
        quitButton.onClick.AddListener(() => Application.Quit());
        creditsButton.onClick.AddListener(() => SwitchCanvas(mainCanvas,creditsCanvas));
        optionsButton.onClick.AddListener(() => SwitchCanvas(mainCanvas,optionsCanvas));
        optionsReturn.onClick.AddListener(() => SwitchCanvas(optionsCanvas,mainCanvas));
        creditsReturn.onClick.AddListener(() => SwitchCanvas(creditsCanvas,mainCanvas));

    }

    private void SwitchCanvas(Canvas from, Canvas to)
    {
        from.gameObject.SetActive(false);
        to.gameObject.SetActive(true);
    }

    private void StartHost()
    {
        GameObject serverObject = (GameObject) Instantiate(Resources.Load("Server"));
        DontDestroyOnLoad(serverObject);
        SceneManager.LoadScene("ArmyBuild");
    }
}
