using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNCreator
{
    [RequireComponent(typeof(AudioSource))]
    public class VNCreator_MusicSource : MonoBehaviour
    {
        AudioSource source;

        public static VNCreator_MusicSource instance;

        private void Awake()
        {
            instance = this;
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = true;
            source.volume = GameOptions.musicVolume;
        }

        public void Play(AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }
    }
}
