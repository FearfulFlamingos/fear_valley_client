using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Scripts.Controller;

/// <summary>
/// All of the scripts in the Main Menu.
/// </summary>
namespace Scripts.Menus
{
    /// <summary>
    /// Button Controller for the main menu.
    /// </summary>
    public class MainMenuButtons : MonoBehaviour
    {
        public Button playButton, tutorialButton, quitButton, creditsButton, optionsButton;
        public Canvas mainCanvas, optionsCanvas, creditsCanvas;
        public Button optionsReturn, creditsReturn;
        public TMP_InputField displayName;
        public TMP_Text storedName, gamesPlayed, gamesWon;
        public GameObject nameSetAlert;
        public Button setDisplayName;

        /// <summary>Reference to the main canvas. Used for Testing.</summary>
        public static Canvas MainCanvas { set; get; }

        /// <summary>Reference to the Options menu canvas. Used for Testing.</summary>
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
                    NameSetter.SelectedName = displayName.text;
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
            storedName.SetText("Name: " + NameSetter.SelectedName);
            gamesPlayed.SetText($"Games Played: {gamesPlayedVal}");
            gamesWon.SetText($"Games Won: {gamesWonVal}");
        }

        /// <summary>
        /// Switch the canvas from the <paramref name="from"/> canvas to the <paramref name="to"/> canvas.
        /// </summary>
        /// <param name="from">Canvas to switch from.</param>
        /// <param name="to">Canvas to switch to.</param>
        public void SwitchCanvas(Canvas from, Canvas to)
        {
            from.gameObject.SetActive(false);
            to.gameObject.SetActive(true);
        }

        /// <summary>
        /// Coroutine to fade an alert after it is triggered. 
        /// </summary>
        /// <param name="canvasGroup">CanvasGroup to fade.</param>
        /// <returns>Coroutine IEnumerator.</returns>
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
