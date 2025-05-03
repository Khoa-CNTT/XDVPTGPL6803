using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class GameSettings : MonoBehaviour
    {
        // Một event duy nhất cho mọi thay đổi
        public UnityEvent onSettingsChanged = new UnityEvent();

        [SerializeField] private GameSettingsData gameSettingsData;
        [SerializeField] private AudioMixer audioMixer;

        public GameSettingsData GameSettingsData { get => gameSettingsData; set => gameSettingsData = value; }

        private SaveLoadManager saveLoadManager;

        void Awake()
        {
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
        }

        private void Start()
        {
            GameSettingsData = saveLoadManager.GetGameData().gameSettings;
            saveLoadManager.OnLoaded.AddListener(() => GameSettingsData = saveLoadManager.GetGameData().gameSettings);
            Init();
        }

        void OnDestroy()
        {
            SaveSettings();
        }

        public void SaveSettings()
        {
            saveLoadManager.SaveData(gameSettingsData);
        }

        public void Init()
        {
            SetQualityLevel(gameSettingsData.qualityLevel);
            SetResolution(gameSettingsData.resolution);
            SetTargetFrameRate(gameSettingsData.targetFrameRate);
            SetBrightness(gameSettingsData.brightness);
            SetMasterVolume(gameSettingsData.masterVolume);
            SetMusicVolume(gameSettingsData.musicVolume);
            SetSFXVolume(gameSettingsData.sfxVolume);

            onSettingsChanged.Invoke();
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

        public void SetTargetFrameRate(int fps)
        {
            gameSettingsData.targetFrameRate = fps;
            Application.targetFrameRate = fps;

            SaveSettings();
        }

        public void SetBrightness(float brightness)
        {
            gameSettingsData.brightness = Mathf.Clamp(brightness, 0f, 2f);

            // TODO: Chỉnh ánh sáng lại 

            SaveSettings();
        }

        public void SetMasterVolume(float volume)
        {
            gameSettingsData.masterVolume = Mathf.Clamp01(volume);
            audioMixer.SetFloat("Master", Mathf.Log10(gameSettingsData.masterVolume) * 20);
            SaveSettings();
        }

        public void SetMusicVolume(float volume)
        {
            gameSettingsData.musicVolume = Mathf.Clamp01(volume);
            audioMixer.SetFloat("Music", Mathf.Log10(gameSettingsData.musicVolume) * 20);
            SaveSettings();
        }

        public void SetSFXVolume(float volume)
        {
            gameSettingsData.sfxVolume = Mathf.Clamp01(volume);
            audioMixer.SetFloat("SFX", Mathf.Log10(gameSettingsData.sfxVolume) * 20);
            SaveSettings();
        }
    }
}
