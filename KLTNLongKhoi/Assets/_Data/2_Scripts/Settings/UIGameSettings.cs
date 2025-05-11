using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

namespace KLTNLongKhoi
{
    public class UIGameSettings : MonoBehaviour
    {
        [Header("Graphics Settings")]
        [SerializeField] private ButtonOptions qualityOptions;
        [SerializeField] private btnNextOptions resolutionOptions;
        [SerializeField] private btnNextOptions fpsOptions;
        [SerializeField] private Slider brightnessOptions; // từ 0-5

        [Header("Audio Settings")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        private GameSettings gameSettings;

        private void Start()
        {
            gameSettings = FindFirstObjectByType<GameSettings>();
            gameSettings.onSettingsChanged += UpdateUI;
            SetupListeners();
            UpdateUI();
        } 
        
        // Initialize UI elements from GameSettings
        private void UpdateUI()
        {
            GameSettingsData gameSettingsData = gameSettings.GameSettingsData;
            if (qualityOptions != null)
            {
                qualityOptions.SetCurrentIndex(gameSettingsData.qualityLevel);
            }

            if (resolutionOptions != null)
            {
                Resolution[] resolutions = Screen.resolutions;
                string[] resolutionStrings = resolutions.Select(r => $"{r.width}x{r.height}").ToArray();
                resolutionOptions.SetOptions(resolutionStrings, gameSettingsData.resolution);
            }

            if (fpsOptions != null)
            {
                fpsOptions.SetOptions(
                    new string[] { "30 FPS", "60 FPS", "120 FPS" },
                    $"{gameSettingsData.targetFrameRate} FPS"
                );
            }

            if (brightnessOptions != null)
                brightnessOptions.value = gameSettingsData.brightness / 5;

            if (masterVolumeSlider != null)
                masterVolumeSlider.value = gameSettingsData.masterVolume;

            if (musicVolumeSlider != null)
                musicVolumeSlider.value = gameSettingsData.musicVolume;

            if (sfxVolumeSlider != null)
                sfxVolumeSlider.value = gameSettingsData.sfxVolume;
        }


        // Setup listeners for UI elements
        private void SetupListeners()
        {
            qualityOptions.onClick.AddListener(OnQualityChanged);
            resolutionOptions.onClick.AddListener(OnResolutionChanged);
            fpsOptions.onClick.AddListener(OnFPSChanged);
            brightnessOptions.onValueChanged.AddListener(OnBrightnessChanged);
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        #region Event Handlers
        private void OnQualityChanged()
        {
            int quality = qualityOptions.GetCurrentIndex();
            gameSettings.SetQualityLevel(quality); // Lưu setting vào GameSettings
        }

        private void OnResolutionChanged()
        {
            string resolution = resolutionOptions.options[resolutionOptions.GetCurrentIndex()];
            gameSettings.SetResolution(resolution);
        }

        private void OnFPSChanged()
        {
            gameSettings.SetTargetFrameRate(GetFPSFromDropdownIndex(fpsOptions.GetCurrentIndex()));
        }

        private void OnBrightnessChanged(float brightness)
        {
            gameSettings.SetBrightness(brightness);
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

        #region Helper Methods
        private int GetFPSFromDropdownIndex(int index)
        {
            return index switch
            {
                0 => 30,
                1 => 60,
                2 => 120,
                _ => 60
            };
        }

        private int GetBrightnessIndex(float brightness)
        {
            if (brightness <= 0.25f) return 0;      // Very Dark
            if (brightness <= 0.5f) return 1;       // Dark
            if (brightness <= 0.75f) return 2;      // Normal
            if (brightness <= 1.0f) return 3;       // Bright
            return 4;                               // Very Bright
        }

        private float GetBrightnessFromIndex(int index)
        {
            return index switch
            {
                0 => 0.25f,    // Very Dark
                1 => 0.5f,     // Dark
                2 => 0.75f,    // Normal
                3 => 1.0f,     // Bright
                4 => 1.25f,    // Very Bright
                _ => 0.75f     // Default to Normal
            };
        }

        private string GetBrightnessText(float brightness)
        {
            if (brightness <= 0.25f) return "Tối";
            if (brightness <= 0.5f) return "Tối hơn";
            if (brightness <= 0.75f) return "Trung bình";
            if (brightness <= 1.0f) return "Sáng";
            return "Sáng hơn";
        }
        #endregion
    }
}
