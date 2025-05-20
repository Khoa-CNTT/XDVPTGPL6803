using MBM;
using UnityEngine;
using System.Collections;

namespace KLTNLongKhoi
{
    public class MusicZone : MonoBehaviour
    {
        [SerializeField] private AudioClip zoneMusic;
        [SerializeField] private float fadeTime = 1.0f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float distance = 10f;
        private AudioSource audioSource;

        private bool isPlayingMusic = false;

       private void Update()
        {
            if (cameraTransform == null)
                cameraTransform = Camera.main.transform;

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            float currentDistance = Vector3.Distance(transform.position, cameraTransform.position);

            if (currentDistance <= distance)
            {
                if (!isPlayingMusic && zoneMusic != null)
                {
                    audioSource.clip = zoneMusic;
                    audioSource.loop = true;
                    audioSource.Play();
                    isPlayingMusic = true;
                }
            }
            else
            {
                if (isPlayingMusic)
                {
                    audioSource.Stop();
                    isPlayingMusic = false;
                }
            }
        }
    }
}