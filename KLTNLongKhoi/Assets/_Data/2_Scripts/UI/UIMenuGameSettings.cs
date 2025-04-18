using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace KLTNLongKhoi
{
    public class UIMenuGameSettings : MonoBehaviour
    {
        [Header("Graphics Settings")]
        [SerializeField] private btnNextOptions qualityOptions;
        [SerializeField] private btnNextOptions resolutionOptions; 
        [SerializeField] private btnNextOptions fpsOptions;
        [SerializeField] private btnNextOptions brightnessOptions;
        [SerializeField] private btnNextOptions rayTracingOptions;
        [SerializeField] private btnNextOptions vSyncOptions;

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
                    new string[] { "Low", "Medium", "High", "Ultra" },
                    gameSettings.GetQualityLevel()
                );
            }

            if (resolutionOptions != null)
            {
                Resolution[] resolutions = Screen.resolutions;
                string[] resolutionStrings = resolutions.Select(r => $"{r.width}x{r.height}").ToArray();
                resolutionOptions.SetOptions(resolutionStrings, gameSettings.GetCurrentResolution());
            }

            if (fpsOptions != null)
            {
                fpsOptions.SetOptions(
                    new string[] { "30 FPS", "60 FPS", "120 FPS", "144 FPS", "240 FPS" },
                    $"{gameSettings.GetTargetFrameRate()} FPS"
                );
            }

            if (brightnessOptions != null)
            {
                brightnessOptions.SetOptions(
                    new string[] { "Very Dark", "Dark", "Normal", "Bright", "Very Bright" },
                    GetBrightnessText(gameSettings.GetBrightness())
                );
            }

            if (rayTracingOptions != null)
            {
                rayTracingOptions.SetOptions(
                    new string[] { "Off", "On" },
                    gameSettings.IsRayTracingEnabled() ? "On" : "Off"
                );
            }

            if (vSyncOptions != null)
            {
                vSyncOptions.SetOptions(
                    new string[] { "Off", "On" },
                    gameSettings.IsVSyncEnabled() ? "On" : "Off"
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
            
            if (resolutionOptions != null)
                resolutionOptions.onClick.AddListener(OnResolutionChanged);
            
            if (fpsOptions != null)
                fpsOptions.onClick.AddListener(OnFPSChanged);
            
            if (brightnessOptions != null)
                brightnessOptions.onClick.AddListener(OnBrightnessChanged);

            if (rayTracingOptions != null)
                rayTracingOptions.onClick.AddListener(OnRayTracingChanged);

            if (vSyncOptions != null)
                vSyncOptions.onClick.AddListener(OnVSyncChanged);
            
            if (masterVolumeSlider != null)
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            
            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        #region Event Handlers
        private void OnQualityChanged()
        {
            string quality = qualityOptions.options[qualityOptions.GetCurrentIndex()];
            gameSettings.SetQualityLevel(quality);
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

        private void OnBrightnessChanged()
        {
            float brightness = GetBrightnessFromIndex(brightnessOptions.GetCurrentIndex());
            gameSettings.SetBrightness(brightness);
        }

        private void OnRayTracingChanged()
        {
            bool isEnabled = rayTracingOptions.GetCurrentIndex() == 1;
            gameSettings.SetRayTracing(isEnabled);
        }

        private void OnVSyncChanged()
        {
            bool isEnabled = vSyncOptions.GetCurrentIndex() == 1;
            gameSettings.SetVSync(isEnabled);
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

        #region Helper Methods
        private int GetFPSDropdownIndex(int fps)
        {
            return fps switch
            {
                30 => 0,
                60 => 1,
                120 => 2,
                144 => 3,
                240 => 4,
                _ => 1 // Default to 60 FPS
            };
        }

        private int GetFPSFromDropdownIndex(int index)
        {
            return index switch
            {
                0 => 30,
                1 => 60,
                2 => 120,
                3 => 144,
                4 => 240,
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
            if (brightness <= 0.25f) return "Very Dark";
            if (brightness <= 0.5f) return "Dark";
            if (brightness <= 0.75f) return "Normal";
            if (brightness <= 1.0f) return "Bright";
            return "Very Bright";
        }
        #endregion
    }
}
