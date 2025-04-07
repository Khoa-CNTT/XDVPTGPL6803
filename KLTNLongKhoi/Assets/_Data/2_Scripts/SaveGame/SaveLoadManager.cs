using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace KLTNLongKhoi
{
    public class SaveLoadManager : MonoBehaviour
    {
        [SerializeField] private GameObject saveLoadPanel;
        [SerializeField] private DataManager dataManager;

        private List<ISaveData> saveDataComponents = new List<ISaveData>();

        private void Start()
        {
            // Find DataManager if not assigned
            if (dataManager == null)
            {
                dataManager = FindFirstObjectByType<DataManager>();
            }

            // Hide panel on start
            if (saveLoadPanel != null)
            {
                saveLoadPanel.SetActive(false);
            }

            // Register to scene loading events to handle finding save components in new scenes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Find all ISaveData components in the new scene
            FindSaveDataComponents();
        }

        private void FindSaveDataComponents()
        {
            saveDataComponents.Clear();
            
            // Find all objects that implement ISaveData in all scenes
            var objectsWithSaveData = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveData>();
            saveDataComponents.AddRange(objectsWithSaveData);
        }

        public void ShowSavePanel()
        {
            if (saveLoadPanel != null)
            {
                saveLoadPanel.SetActive(true);
            }
        }

        public void HideSavePanel()
        {
            if (saveLoadPanel != null)
            {
                saveLoadPanel.SetActive(false);
            }
        }

        public void SaveGame()
        {
            if (dataManager == null)
            {
                Debug.LogError("DataManager not found!");
                return;
            }

            GameData gameData = dataManager.GameData;

            // Update basic world state
            UpdateGameDataBeforeSave();

            // Save data from all ISaveData components
            foreach (var saveComponent in saveDataComponents)
            {
                // Call SaveData on each component and store the result in appropriate GameData property
                if (saveComponent is MonoBehaviour mb)
                {
                    string componentType = mb.GetType().Name;
                    switch (componentType)
                    {
                        case "Player":
                            gameData.player = saveComponent.SaveData<PlayerData>();
                            break;
                        case "GameSettings":
                            gameData.gameSettings = saveComponent.SaveData<GameSettingsData>();
                            break;
                        case "WorldItems":
                            gameData.worldItems = saveComponent.SaveData<WorldItemsData>();
                            break;
                        case "Monster":
                            var monsterData = saveComponent.SaveData<MonsterData>();
                            if (!gameData.monsters.Any(m => m.id == monsterData.id))
                            {
                                gameData.monsters.Add(monsterData);
                            }
                            break;
                        // Add more cases as needed
                    }
                }
            }

            // Save using DataManager
            dataManager.SaveGameData();
            Debug.Log("Game saved successfully");
            HideSavePanel();
        }

        public void LoadGame()
        {
            if (dataManager == null)
            {
                Debug.LogError("DataManager not found!");
                return;
            }

            dataManager.LoadGameData();

            if (!dataManager.IsNewGameplay())
            {
                LoadGameState();
            }
            else
            {
                Debug.Log("No saved game found - starting new game");
            }

            HideSavePanel();
        }

        private void UpdateGameDataBeforeSave()
        {
            GameData gameData = dataManager.GameData;
            
            // Update basic world state information
            gameData.worldState.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            gameData.worldState.sceneName = SceneManager.GetActiveScene().name;
            gameData.worldState.dayTime = System.DateTime.Now.ToString(); // Or your in-game time system
        }

        private void LoadGameState()
        {
            GameData gameData = dataManager.GameData;

            // Load scene if different from current
            if (SceneManager.GetActiveScene().buildIndex != gameData.worldState.currentSceneIndex)
            {
                SceneManager.LoadScene(gameData.worldState.currentSceneIndex);
                return; // Scene loading will trigger OnSceneLoaded which will handle the rest
            }

            // Load data into all ISaveData components
            foreach (var saveComponent in saveDataComponents)
            {
                if (saveComponent is MonoBehaviour mb)
                {
                    string componentType = mb.GetType().Name;
                    switch (componentType)
                    {
                        case "Player":
                            saveComponent.LoadData(gameData.player);
                            break;
                        case "GameSettings":
                            saveComponent.LoadData(gameData.gameSettings);
                            break;
                        case "WorldItems":
                            saveComponent.LoadData(gameData.worldItems);
                            break;
                        // Add more cases as needed
                    }
                }
            }
        }

        public void StartNewGame()
        {
            if (dataManager != null)
            {
                dataManager.ResetGameplayData();
                SceneManager.LoadScene(1); // Assuming your first gameplay scene is at build index 1
            }
        }

        public void QuitGame()
        {
            SaveGame(); // Save before quitting
            Application.Quit();
        }
    }
}
