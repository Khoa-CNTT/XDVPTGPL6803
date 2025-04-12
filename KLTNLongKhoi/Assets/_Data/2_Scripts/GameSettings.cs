using UnityEngine;

namespace KLTNLongKhoi
{
    public class GameSettings : MonoBehaviour, ISaveData
    {
        [Header("Graphics Settings")]
        [SerializeField] private int _qualityLevel = 3;
        [SerializeField] private Resolution[] _resolutions;
        [SerializeField] private int _currentResolutionIndex;
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
            _currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
            _qualityLevel = PlayerPrefs.GetInt("QualityLevel", 3);
            _targetFrameRate = PlayerPrefs.GetInt("TargetFPS", 60);
            _brightness = PlayerPrefs.GetFloat("Brightness", 1f);
            _rayTracingEnabled = PlayerPrefs.GetInt("RayTracing", 0) == 1;
            _vSyncEnabled = PlayerPrefs.GetInt("VSync", 0) == 1;
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

            ApplySettings();
        }

        private void ApplySettings()
        {
            SetQualityLevel(_qualityLevel);
            SetResolution(_currentResolutionIndex);
            SetTargetFrameRate(_targetFrameRate);
            SetBrightness(_brightness);
            SetRayTracing(_rayTracingEnabled);
            SetVSync(_vSyncEnabled);
            SetMasterVolume(_masterVolume);
            SetMusicVolume(_musicVolume);
            SetSFXVolume(_sfxVolume);
        }

        public void SetQualityLevel(int level)
        {
            _qualityLevel = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1);
            QualitySettings.SetQualityLevel(_qualityLevel);
            PlayerPrefs.SetInt("QualityLevel", _qualityLevel);
        }

        public void SetResolution(int resolutionIndex)
        {
            if (resolutionIndex < 0 || resolutionIndex >= _resolutions.Length)
                return;

            _currentResolutionIndex = resolutionIndex;
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
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

        #region SaveData
        public void LoadData<T>(T data)
        {
            if (data is GameSettingsData settings)
            {
                _masterVolume = settings.masterVolume;
                _musicVolume = settings.musicVolume;
                _sfxVolume = settings.sfxVolume;
                _qualityLevel = settings.qualityLevel;
                _currentResolutionIndex = settings.resolutionIndex;
                _targetFrameRate = settings.targetFrameRate;
                _brightness = settings.brightness;
                _rayTracingEnabled = settings.rayTracingEnabled;
                _vSyncEnabled = settings.vSyncEnabled;

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
                qualityLevel = _qualityLevel,
                resolutionIndex = _currentResolutionIndex,
                targetFrameRate = _targetFrameRate,
                brightness = _brightness,
                rayTracingEnabled = _rayTracingEnabled,
                vSyncEnabled = _vSyncEnabled,
                graphics = QualitySettings.names[_qualityLevel]
            };

            return (T)(object)settings;
        }
        #endregion

        // Getters for current settings
        public float GetMasterVolume() => _masterVolume;
        public float GetMusicVolume() => _musicVolume;
        public float GetSFXVolume() => _sfxVolume;
        public int GetQualityLevel() => _qualityLevel;
        public Resolution[] GetResolutions() => _resolutions;
        public int GetCurrentResolutionIndex() => _currentResolutionIndex;
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
    }
}
