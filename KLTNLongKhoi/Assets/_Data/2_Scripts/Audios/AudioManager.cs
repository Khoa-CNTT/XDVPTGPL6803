using UnityEngine;
using UnityEngine.Audio;

namespace KLTNLongKhoi
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;
        
        // Mixer group names
        private const string MASTER_VOLUME = "Master";
        private const string MUSIC_VOLUME = "Music"; 
        private const string SFX_VOLUME = "SFX";

        private GameSettings gameSettings;

        private void Start()
        {
            gameSettings = FindFirstObjectByType<GameSettings>();
            if (gameSettings == null)
            {
                Debug.LogWarning("GameSettings not found in scene!");
                return;
            }

            // Initial load from GameSettings
            InitializeVolumes();
        }

        private void InitializeVolumes()
        {
            SetMasterVolume(gameSettings.GetMasterVolume());
            SetMusicVolume(gameSettings.GetMusicVolume());
            SetSFXVolume(gameSettings.GetSFXVolume());
        }

        public void SetMasterVolume(float normalizedVolume)
        {
            SetVolume(MASTER_VOLUME, normalizedVolume);
        }

        public void SetMusicVolume(float normalizedVolume)
        {
            SetVolume(MUSIC_VOLUME, normalizedVolume);
        }

        public void SetSFXVolume(float normalizedVolume)
        {
            SetVolume(SFX_VOLUME, normalizedVolume);
        }

        private void SetVolume(string parameterName, float normalizedVolume)
        {
            if (audioMixer == null)
            {
                Debug.LogError("AudioMixer not assigned!");
                return;
            }

            // Clamp volume between 0 and 1
            normalizedVolume = Mathf.Clamp01(normalizedVolume);

            // Convert normalized volume (0 to 1) to decibels (-80dB to 0dB)
            float decibelVolume = normalizedVolume > 0 
                ? Mathf.Log10(normalizedVolume) * 20 
                : -80f;

            audioMixer.SetFloat(parameterName, decibelVolume);
        }

        public float GetMasterVolume()
        {
            return GetVolume(MASTER_VOLUME);
        }

        public float GetMusicVolume()
        {
            return GetVolume(MUSIC_VOLUME);
        }

        public float GetSFXVolume()
        {
            return GetVolume(SFX_VOLUME);
        }

        private float GetVolume(string parameterName)
        {
            if (audioMixer == null)
            {
                Debug.LogError("AudioMixer not assigned!");
                return 0f;
            }

            float decibelVolume;
            audioMixer.GetFloat(parameterName, out decibelVolume);
            
            // Convert decibels back to normalized volume (0 to 1)
            return decibelVolume > -80f 
                ? Mathf.Clamp01(Mathf.Pow(10, decibelVolume / 20)) 
                : 0f;
        }

        #region Audio Source Management
        public void PlaySound(AudioSource source, AudioClip clip, float volume = 1f)
        {
            if (source == null || clip == null) return;

            source.clip = clip;
            source.volume = volume;
            source.Play();
        }

        public void StopSound(AudioSource source)
        {
            if (source == null) return;
            source.Stop();
        }

        public void PauseSound(AudioSource source)
        {
            if (source == null) return;
            source.Pause();
        }

        public void UnPauseSound(AudioSource source)
        {
            if (source == null) return;
            source.UnPause();
        }
        #endregion
    }
}
