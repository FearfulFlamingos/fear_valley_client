using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Scripts.Menus
{
    public class MainMenuButtons : MonoBehaviour
    {
        public Button playButton, tutorialButton, quitButton, creditsButton, optionsButton;
        public Canvas mainCanvas, optionsCanvas, creditsCanvas;
        public Button optionsReturn, creditsReturn;
        public TMP_InputField displayName;
        public TMP_Text storedName, gamesPlayed, gamesWon;
        public GameObject nameSetAlert;
        public Button setDisplayName;
        public static Canvas MainCanvas { set; get; }
        public static Canvas OptionsCanvas { set; get; }

        void Start()
        {
            playButton.onClick.AddListener(() => SceneManager.LoadScene("ServerConnect"));
            quitButton.onClick.AddListener(() => Application.Quit());
            creditsButton.onClick.AddListener(() => SwitchCanvas(mainCanvas, creditsCanvas));
            optionsButton.onClick.AddListener(() => SwitchCanvas(mainCanvas, optionsCanvas));
            optionsReturn.onClick.AddListener(() => SwitchCanvas(optionsCanvas, mainCanvas));
            creditsReturn.onClick.AddListener(() => SwitchCanvas(creditsCanvas, mainCanvas));
            setDisplayName.onClick.AddListener(() =>
            {
                if (displayName.text != "")
                {
                    PlayerPrefs.SetString("PlayerName", displayName.text);
                    nameSetAlert.GetComponent<CanvasGroup>().alpha = 1;
                    StartCoroutine(FadeOut(nameSetAlert.GetComponent<CanvasGroup>()));
                    storedName.SetText($"Name: {displayName.text}");
                    displayName.text = "";
                }
            });

            MainCanvas = mainCanvas;
            OptionsCanvas = optionsCanvas;
            int gamesPlayedVal = PlayerPrefs.HasKey("GamesPlayed") ? PlayerPrefs.GetInt("GamesPlayed") : 0;
            int gamesWonVal = PlayerPrefs.HasKey("GamesWon") ? PlayerPrefs.GetInt("GamesWon") : 0;
            storedName.SetText("Name: " + PlayerPrefs.GetString("PlayerName"));
            gamesPlayed.SetText($"Games Played: {gamesPlayedVal}");
            gamesWon.SetText($"Games Won: {gamesWonVal}");
        }

        public void SwitchCanvas(Canvas from, Canvas to)
        {
            from.gameObject.SetActive(false);
            to.gameObject.SetActive(true);
        }

        public IEnumerator FadeOut(CanvasGroup canvasGroup)
        {
            const float RATE = 0.01f;
            yield return new WaitForSeconds(1);
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= RATE;
                yield return null;
            }
        }
    }
}
