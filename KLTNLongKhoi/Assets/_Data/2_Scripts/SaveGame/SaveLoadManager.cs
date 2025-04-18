using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        [SerializeField] private GameObject saveLoadPanel;
        DataManager dataManager;

        // UnityEvents for UI to listen to
        public UnityEvent onStartLoadGameSettings = new UnityEvent();
        public UnityEvent onFinishLoadGameSettings = new UnityEvent();
        public UnityEvent onStartLoadGameWorld = new UnityEvent();
        public UnityEvent onFinishLoadGameWorld = new UnityEvent();

        private List<ISaveData> saveDataComponents = new List<ISaveData>();
        private bool isInitialized = false;

        protected override void Awake()
        {
            base.Awake();
            SetDontDestroyOnLoad(true);
        }

        private void Start()
        {
            InitializeGame();

            // Register to scene loading events to handle finding save components in new scenes
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Hide panel on start
            if (saveLoadPanel != null)
            {
                saveLoadPanel.SetActive(false);
            }
        }

        private void InitializeGame()
        {
            if (isInitialized) return;

            // Find DataManager if not assigned
            dataManager = GetComponent<DataManager>();

            // Load saved data
            if (dataManager != null)
            {
                dataManager.ImportFileGameData();
                
                // Nếu có data đã lưu, load vào game
                if (!dataManager.IsNewGameplay())
                {
                    LoadGame();
                }
            }

            FindSaveDataComponents();
            LoadGameSettings();

            isInitialized = true;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Find all ISaveData components in the new scene
            FindSaveDataComponents();

            // Nếu đã initialize và có data, load cho scene mới
            if (isInitialized && dataManager != null && !dataManager.IsNewGameplay())
            {
                LoadGameWorld();
            }
            
            LoadGameSettings();
        }

        private void FindSaveDataComponents()
        {
            saveDataComponents.Clear();

            // Tìm tất cả các object có implement ISaveData trong tất cả các scene
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
                if (saveComponent is MonoBehaviour mb)
                {
                    string componentType = mb.GetType().Name;
                    Debug.Log("Saving component: " + componentType);
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
                    }
                }
            }

            // Save using DataManager
            dataManager.ArchiveGameData();
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

            dataManager.ImportFileGameData();

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

        private void LoadGameState()
        {
            LoadGameSettings();
            LoadGameWorld();
        }

        private void LoadGameSettings()
        {
            onStartLoadGameSettings.Invoke();
            
            GameData gameData = dataManager.GameData;
            
            // Load settings-related components
            foreach (var saveComponent in saveDataComponents)
            {
                if (saveComponent is MonoBehaviour mb)
                {
                    string componentType = mb.GetType().Name;
                    switch (componentType)
                    {
                        case "GameSettings":
                            saveComponent.LoadData(gameData.gameSettings);
                            break;
                        case "UIGameSettings":
                            saveComponent.LoadData(gameData.gameSettings);
                            break;
                    }
                }
            }

            onFinishLoadGameSettings.Invoke();
        }

        private void LoadGameWorld()
        {
            onStartLoadGameWorld.Invoke();

            GameData gameData = dataManager.GameData;

            // Load scene if different from current
            if (SceneManager.GetActiveScene().buildIndex != gameData.worldState.currentSceneIndex)
            {
                SceneManager.LoadScene(gameData.worldState.currentSceneIndex);
                return; // Scene loading will trigger OnSceneLoaded and handle the rest
            }

            // Find components before loading
            FindSaveDataComponents();

            // Load world-related components
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
                        case "WorldItems":
                            saveComponent.LoadData(gameData.worldItems);
                            break;
                    }
                }
            }

            onFinishLoadGameWorld.Invoke();
        }

        private void UpdateGameDataBeforeSave()
        {
            GameData gameData = dataManager.GameData;

            // Update basic world state information
            gameData.worldState.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            gameData.worldState.sceneName = SceneManager.GetActiveScene().name;
            gameData.worldState.dayTime = System.DateTime.Now.ToString();
        }

        public void StartNewGame()
        {
            if (dataManager != null)
            {
                dataManager.ResetGameplayData();
                isInitialized = false;
                InitializeGame();
            }
        }

        public void QuitGame()
        {
            SaveGame(); // Save before quitting
            Application.Quit();
        }

        void OnApplicationQuit()
        {
            SaveGame(); // Save before quitting 
        }

        // Method để reset toàn bộ game state
        public void ResetGame()
        {
            if (dataManager != null)
            {
                dataManager.DeleteFileSave();
                isInitialized = false;
                InitializeGame();
            }
        }
    }
}
