using UnityEngine;
using System.Collections.Generic;

namespace MBM
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Instance => _instance ?? (_instance = new GameObject("AudioManager").AddComponent<AudioManager>());

        private Dictionary<string, AudioSource> _audioSources;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                _audioSources = new Dictionary<string, AudioSource>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlaySound(string soundName, AudioClip clip, float volume = 1.0f)
        {
            if (!_audioSources.ContainsKey(soundName))
            {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = volume;
                newSource.Play();
                _audioSources[soundName] = newSource;
            }
        }

        public void SetVolume(string soundName, float volume)
        {
            if (_audioSources.ContainsKey(soundName))
            {
                _audioSources[soundName].volume = volume;
            }
        }

        public float GetVolume(string soundName)
        {
            if (_audioSources.ContainsKey(soundName))
            {
                return _audioSources[soundName].volume;
            }
            return 0f;
        }

        public void StopSound(string soundName)
        {
            if (_audioSources.ContainsKey(soundName))
            {
                _audioSources[soundName].Stop();
                Destroy(_audioSources[soundName]);
                _audioSources.Remove(soundName);
            }
        }
    }
}