using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class GameSettings : MonoBehaviour, ISaveData
    {
        // Một event duy nhất cho mọi thay đổi
        public UnityEvent onSettingsChanged = new UnityEvent();

        [Header("Graphics Settings")]
        [SerializeField] private int _qualityLevel = 1;
        [SerializeField] private Resolution[] _resolutions;
        [SerializeField] private string _currentResolution = "1920x1080";
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private float _brightness = 1f;

        [Header("Audio Settings")]
        [SerializeField] private AudioMixer _audioMixer;
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

        private void ApplySettings()
        {
            // Áp dụng graphics settings
            QualitySettings.SetQualityLevel(_qualityLevel, true);

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

            ApplyAudioSettings();

            // Todo: Áp dụng brightness thông qua post-processing
            // var postProcessVolume = FindFirstObjectByType<UnityEngine.Rendering.PostProcessing.PostProcessVolume>();
            // if (postProcessVolume != null && 
            //     postProcessVolume.profile.TryGetSettings(out UnityEngine.Rendering.PostProcessing.ColorGrading colorGrading))
            // {
            //     colorGrading.postExposure.value = _brightness;
            // } 

            onSettingsChanged.Invoke();
        }

        private void ApplyAudioSettings()
        {
            // Tìm và áp dụng Audio Mixer settings
            if (_audioMixer != null)
            {
                _audioMixer.SetFloat("Master", Mathf.Log10(_masterVolume) * 20);
                _audioMixer.SetFloat("Music", Mathf.Log10(_musicVolume) * 20);
                _audioMixer.SetFloat("SFX", Mathf.Log10(_sfxVolume) * 20);
            }
        }

        public void SetQualityLevel(int quality)
        {
            _qualityLevel = quality;
            QualitySettings.SetQualityLevel(quality, true);
            Debug.Log("Quality level set to: " + quality);
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
            }
        }

        public void SetTargetFrameRate(int fps)
        {
            _targetFrameRate = fps;
            Application.targetFrameRate = fps;
        }

        public void SetBrightness(float brightness)
        {
            _brightness = Mathf.Clamp(brightness, 0f, 2f);
            // Implement brightness adjustment logic here
            // This might involve post-processing or other screen effects
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            AudioListener.volume = _masterVolume;

            ApplyAudioSettings();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            // Implement music volume adjustment logic here
            // This might involve finding and adjusting an audio mixer 
            ApplyAudioSettings();
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            // Implement SFX volume adjustment logic here
            // This might involve finding and adjusting an audio mixer 
            ApplyAudioSettings();
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
                _currentResolution = settings.resolution;
                _brightness = settings.brightness;

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
                targetFrameRate = _targetFrameRate,
                resolution = _currentResolution,
                brightness = _brightness,
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
        public string GetCurrentResolution() => _currentResolution;
        public int GetTargetFrameRate() => _targetFrameRate;
        public float GetBrightness() => _brightness;

        // Khi cần lưu settings
        public void SaveSettings()
        {
            if (SaveLoadManager.Instance != null)
            {
                SaveLoadManager.Instance.SaveGame();
            }
        }

        public string GetResolution()
        {
            return _currentResolution;
        }
    }
}
