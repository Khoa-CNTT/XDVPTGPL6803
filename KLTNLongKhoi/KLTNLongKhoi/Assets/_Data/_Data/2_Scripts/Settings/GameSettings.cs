using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class GameSettings : MonoBehaviour
    {

        [SerializeField] private GameSettingsData gameSettingsData;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private UnityEngine.Rendering.Volume globalVolume;

        public event Action onSettingsChanged;

        public GameSettingsData GameSettingsData { get => gameSettingsData; set => gameSettingsData = value; }

        private SaveLoadManager saveLoadManager;

        void Awake()
        {
            saveLoadManager = SaveLoadManager.Instance;
        }

        private void OnEnable()
        {
            saveLoadManager.OnLoaded += Init;
        }

        private void OnDisable()
        {
            saveLoadManager.OnLoaded -= Init;
        }

        public void Init()
        {
            gameSettingsData = saveLoadManager.LoadData<GameSettingsData>();

            SetQualityLevel(gameSettingsData.qualityLevel);
            SetResolution(gameSettingsData.resolution);
            SetFPS(gameSettingsData.targetFrameRate);
            SetBrightness(gameSettingsData.brightness);
            SetMasterVolume(gameSettingsData.masterVolume);
            SetMusicVolume(gameSettingsData.musicVolume);
            SetSFXVolume(gameSettingsData.sfxVolume); 

            onSettingsChanged?.Invoke();
        }

        public void SetQualityLevel(int quality)
        {
            gameSettingsData.qualityLevel = quality;
            QualitySettings.SetQualityLevel(quality, true);
            SaveSettings();
        }

        public void SetResolution(string resolution)
        {
            gameSettingsData.resolution = resolution;
            string[] dimensions = resolution.Split('x');
            if (dimensions.Length == 2 &&
                int.TryParse(dimensions[0], out int width) &&
                int.TryParse(dimensions[1], out int height))
            {
                Screen.SetResolution(width, height, Screen.fullScreen);
            }

            SaveSettings();
        }

        public void SetFPS(int fps)
        {
            gameSettingsData.targetFrameRate = fps;
            Application.targetFrameRate = fps;

            SaveSettings();
        }

        public void SetBrightness(float brightness)
        {
            gameSettingsData.brightness = brightness; 
            // Điều chỉnh Post Exposure trong Volume
            if (globalVolume != null && globalVolume.profile != null)
            {
                // Tìm component ColorAdjustments
                if (globalVolume.profile.TryGet(out UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments))
                {
                    colorAdjustments.postExposure.Override(brightness * 2);
                }
            }
            SaveSettings();
        }

        public void SetMasterVolume(float volume)
        {
            gameSettingsData.masterVolume = volume;
            float volumeValue = ConvertVolumeToDecibel(volume);
            audioMixer.SetFloat("Master", volumeValue);
            SaveSettings();
        }

        public void SetMusicVolume(float volume)
        {
            gameSettingsData.musicVolume = volume;
            float volumeValue = ConvertVolumeToDecibel(volume);
            audioMixer.SetFloat("Music", volumeValue);
            SaveSettings();
        }

        public void SetSFXVolume(float volume)
        {
            gameSettingsData.sfxVolume = volume;
            float volumeValue = ConvertVolumeToDecibel(volume);
            audioMixer.SetFloat("SFX", volumeValue);
            SaveSettings();
        }

        private float ConvertVolumeToDecibel(float volume)
        {
            return -20f + volume * 25f;
        }

        public void SaveSettings()
        {
            saveLoadManager.SaveData(gameSettingsData);
        }
    }
}
