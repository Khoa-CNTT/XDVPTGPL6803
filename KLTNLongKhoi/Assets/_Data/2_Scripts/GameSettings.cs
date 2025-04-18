using System;
using UnityEngine;

namespace KLTNLongKhoi
{
    public class GameSettings : MonoBehaviour, ISaveData
    {
        [Header("Graphics Settings")]
        [SerializeField] private string _qualityLevel = "Medium";
        [SerializeField] private Resolution[] _resolutions;
        [SerializeField] private string _currentResolution = "1920x1080";
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private float _brightness = 1f;
        [SerializeField] private bool _rayTracingEnabled;
        [SerializeField] private bool _vSyncEnabled;
        
        [Header("Audio Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float _masterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float _musicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float _sfxVolume = 1f;

        private void Start()
        {
            _resolutions = Screen.resolutions;
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            ApplySettings();
        }

        private int MapStringToQuality(string quality)
        {
            return quality switch
            {
                "Low" => 0,
                "Medium" => 1,
                "High" => 2,
                _ => 1  // Default to Medium
            };
        }

        private void ApplySettings()
        {
            // Áp dụng graphics settings
            QualitySettings.SetQualityLevel(MapStringToQuality(_qualityLevel), true);
            
            // Áp dụng resolution
            string[] dimensions = _currentResolution.Split('x');
            if (dimensions.Length == 2 && 
                int.TryParse(dimensions[0], out int width) && 
                int.TryParse(dimensions[1], out int height))
            {
                Screen.SetResolution(width, height, Screen.fullScreen);
            }
            
            // Áp dụng frame rate và vsync
            Application.targetFrameRate = _targetFrameRate;
            QualitySettings.vSyncCount = _vSyncEnabled ? 1 : 0;

            // Áp dụng master volume
            AudioListener.volume = _masterVolume;
            
            // Tìm và áp dụng Audio Mixer settings
            var audioMixer = Resources.Load<UnityEngine.Audio.AudioMixer>("MainAudioMixer");
            if (audioMixer != null)
            {
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(_musicVolume) * 20);
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(_sfxVolume) * 20);
            }

            // Todo: Áp dụng brightness thông qua post-processing
            // var postProcessVolume = FindFirstObjectByType<UnityEngine.Rendering.PostProcessing.PostProcessVolume>();
            // if (postProcessVolume != null && 
            //     postProcessVolume.profile.TryGetSettings(out UnityEngine.Rendering.PostProcessing.ColorGrading colorGrading))
            // {
            //     colorGrading.postExposure.value = _brightness;
            // }

            // Ray tracing settings
            if (_rayTracingEnabled)
            {
                // Implement ray tracing logic here if your project supports it
            }
        }

        public void SetQualityLevel(string quality)
        {
            _qualityLevel = quality;
            PlayerPrefs.SetString("QualityLevel", _qualityLevel);
        }

        public void SetResolution(string resolution)
        {
            _currentResolution = resolution;
            string[] dimensions = resolution.Split('x');
            if (dimensions.Length == 2 && 
                int.TryParse(dimensions[0], out int width) && 
                int.TryParse(dimensions[1], out int height))
            {
                Screen.SetResolution(width, height, Screen.fullScreen);
                PlayerPrefs.SetString("Resolution", _currentResolution);
            }
        }

        public void SetTargetFrameRate(int fps)
        {
            _targetFrameRate = fps;
            Application.targetFrameRate = fps;
            PlayerPrefs.SetInt("TargetFPS", fps);
        }

        public void SetBrightness(float brightness)
        {
            _brightness = Mathf.Clamp(brightness, 0f, 2f);
            // Implement brightness adjustment logic here
            // This might involve post-processing or other screen effects
            PlayerPrefs.SetFloat("Brightness", _brightness);
        }

        public void SetRayTracing(bool enabled)
        {
            _rayTracingEnabled = enabled;
            // Implement ray tracing toggle logic here
            PlayerPrefs.SetInt("RayTracing", enabled ? 1 : 0);
        }

        public void SetVSync(bool enabled)
        {
            _vSyncEnabled = enabled;
            QualitySettings.vSyncCount = enabled ? 1 : 0;
            PlayerPrefs.SetInt("VSync", enabled ? 1 : 0);
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            AudioListener.volume = _masterVolume;
            PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            // Implement music volume adjustment logic here
            // This might involve finding and adjusting an audio mixer
            PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            // Implement SFX volume adjustment logic here
            // This might involve finding and adjusting an audio mixer
            PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        }

        #region ISaveData Implementation
        public void LoadData<T>(T data)
        {
            if (data is GameSettingsData settings)
            {
                _masterVolume = settings.masterVolume;
                _musicVolume = settings.musicVolume;
                _sfxVolume = settings.sfxVolume;
                _qualityLevel = settings.graphics;
                _targetFrameRate = settings.targetFrameRate;

                ApplySettings();
            }
        }

        public T SaveData<T>()
        {
            GameSettingsData settings = new GameSettingsData
            {
                masterVolume = _masterVolume,
                musicVolume = _musicVolume,
                sfxVolume = _sfxVolume,
                graphics = _qualityLevel,
                targetFrameRate = _targetFrameRate
            };

            return (T)(object)settings;
        }
        #endregion

        // Getters for current settings
        public float GetMasterVolume() => _masterVolume;
        public float GetMusicVolume() => _musicVolume;
        public float GetSFXVolume() => _sfxVolume;
        public string GetQualityLevel() => _qualityLevel;
        public Resolution[] GetResolutions() => _resolutions;
        public string GetCurrentResolution() => _currentResolution;
        public int GetTargetFrameRate() => _targetFrameRate;
        public float GetBrightness() => _brightness;
        public bool IsRayTracingEnabled() => _rayTracingEnabled;
        public bool IsVSyncEnabled() => _vSyncEnabled;

        // Khi cần lưu settings
        public void SaveSettings()
        {
            if (DataManager.Instance != null)
            {
                DataManager.Instance.SaveGameData();
            }
        }

        public string GetResolution()
        {
            return _currentResolution;
        }
    }
}
