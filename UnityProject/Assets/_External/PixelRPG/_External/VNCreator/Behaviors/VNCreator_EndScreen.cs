using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VNCreator
{
    public class VNCreator_EndScreen : MonoBehaviour
    {
        public Button restartButton;
        public Button mainMenuButton;
        [Scene]
        public string mainMenu;

        void Start()
        {
            restartButton.onClick.AddListener(Restart);
            mainMenuButton.onClick.AddListener(MainMenu);
        }

        void Restart()
        {
            GameSaveManager.NewLoad("MainGame");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        void MainMenu()
        {
            SceneManager.LoadScene(mainMenu, LoadSceneMode.Single);
        }
    }
}
