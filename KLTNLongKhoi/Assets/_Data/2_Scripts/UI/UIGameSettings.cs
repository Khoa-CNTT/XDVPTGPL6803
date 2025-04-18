using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KLTNLongKhoi
{
    public class UIGameSettings : MonoBehaviour
    {
        [Header("Graphics Settings")]
        [SerializeField] private btnNextOptions qualityOptions;
        [SerializeField] private btnNextOptions fpsOptions;

        [Header("Audio Settings")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        private GameSettings gameSettings;

        private void Start()
        {
            gameSettings = FindFirstObjectByType<GameSettings>();
            if (gameSettings == null)
            {
                Debug.LogError("GameSettings not found!");
                return;
            }

            InitializeUI();
            SetupListeners();
        }

        private void InitializeUI()
        {
            if (qualityOptions != null)
            {
                qualityOptions.SetOptions(
                    new string[] { "Low", "Medium", "High" },
                    MapQualityToString(gameSettings.GetQualityLevel())
                );
            }

            if (fpsOptions != null)
            {
                fpsOptions.SetOptions(
                    new string[] { "30 FPS", "60 FPS", "120 FPS" },
                    $"{gameSettings.GetTargetFrameRate()} FPS"
                );
            }

            if (masterVolumeSlider != null)
                masterVolumeSlider.value = gameSettings.GetMasterVolume();
            if (musicVolumeSlider != null)
                musicVolumeSlider.value = gameSettings.GetMusicVolume();
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.value = gameSettings.GetSFXVolume();
        }

        private void SetupListeners()
        {
            if (qualityOptions != null)
                qualityOptions.onClick.AddListener(OnQualityChanged);
            
            if (fpsOptions != null)
                fpsOptions.onClick.AddListener(OnFPSChanged);
            
            if (masterVolumeSlider != null)
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            
            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        private string MapQualityToString(string qualityLevel)
        {
            return qualityLevel switch
            {
                "Low" => "Low",
                "Medium" => "Medium",
                "High" => "High",
                "Ultra" => "Ultra",
                _ => "Medium"
            };
        }

        private int MapStringToQuality(string quality)
        {
            return quality switch
            {
                "Low" => 1,
                "Medium" => 3,
                "High" => 5,
                _ => 3  // Default to Medium
            };
        }

        private int GetFPSFromString(string fpsString)
        {
            return fpsString switch
            {
                "30 FPS" => 30,
                "60 FPS" => 60,
                "120 FPS" => 120,
                _ => 60  // Default to 60 FPS
            };
        }

        #region Event Handlers
        private void OnQualityChanged()
        {
            string quality = qualityOptions.GetCurrentOption();
            int unityQualityLevel = MapStringToQuality(quality);
            QualitySettings.SetQualityLevel(unityQualityLevel, true);
            gameSettings.SetQualityLevel(quality); // Lưu setting vào GameSettings
        }

        private void OnFPSChanged()
        {
            string fpsString = fpsOptions.GetCurrentOption();
            int fps = GetFPSFromString(fpsString);
            gameSettings.SetTargetFrameRate(fps);
        }

        private void OnMasterVolumeChanged(float value)
        {
            gameSettings.SetMasterVolume(value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            gameSettings.SetMusicVolume(value);
        }

        private void OnSFXVolumeChanged(float value)
        {
            gameSettings.SetSFXVolume(value);
        }
        #endregion

        public void SaveSettings()
        {
            gameSettings.SaveSettings();
        }
    }
}
