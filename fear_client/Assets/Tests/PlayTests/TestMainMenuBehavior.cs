using UnityEngine;
using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.TestTools;
using Scripts.Menus;

namespace PlayTests
{
    public class TestMainMenuBehavior : MonoBehaviour
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SceneManager.LoadScene(0);
        }

        [UnityTest]
        public IEnumerator TestWhetherButtonsAreActive()
        {
            // Arrange
            GameObject playButton = GameObject.Find("/Main/PrimaryButtons/PlayButton");
            GameObject tutorialButton = GameObject.Find("/Main/PrimaryButtons/TutorialButton");
            GameObject quitButton = GameObject.Find("/Main/LowerButtons/QuitButton");
            GameObject creditsButton = GameObject.Find("/Main/LowerButtons/CreditsButton");
            GameObject optionsButton = GameObject.Find("/Main/LowerButtons/OptionsButton");
           
            yield return new WaitForSeconds(1);
            // Assert
            Assert.IsTrue(playButton.activeSelf);
            Assert.IsTrue(tutorialButton.activeSelf);
            Assert.IsTrue(quitButton.activeSelf);
            Assert.IsTrue(creditsButton.activeSelf);
            Assert.IsTrue(optionsButton.activeSelf);


            yield return null;
        }

        [UnityTest]
        public IEnumerator TestCanvasSwitch()
        {
            Canvas mainCanvas = MainMenuButtons.MainCanvas;
            Canvas optionsCanvas = MainMenuButtons.OptionsCanvas;
            MainMenuButtons mainMenuButtons = GameObject.Find("ButtonController").GetComponent<MainMenuButtons>();

            // Act
            mainMenuButtons.SwitchCanvas(mainCanvas, optionsCanvas);

            // Assert

            Assert.IsFalse(mainCanvas.gameObject.activeSelf);
            Assert.IsTrue(optionsCanvas.gameObject.activeSelf);

            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            MainMenuButtons main = GameObject.Find("ButtonController").GetComponent<MainMenuButtons>();
            main.SwitchCanvas(MainMenuButtons.OptionsCanvas, MainMenuButtons.MainCanvas);
        }

        // Volume Tester somewhere in here later
    }
}