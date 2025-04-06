using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace HieuDev
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public TMP_Dropdown resolutionDropdown;
        Resolution[] resolutions; // Array to store available screen resolutions

        void Start()
        {
            SetResolution();
        }
        
        private void SetResolution()
        {
            resolutions = Screen.resolutions; // Get all available screen resolutions from the system

            resolutionDropdown.ClearOptions(); // Clear any existing options in the dropdown

            List<string> options = new List<string>(); // Create a list to store resolution strings

            int currentResolutionIndex = 0; // Index of the current resolution

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height; // Format resolution as a string

                options.Add(option); // Add the resolution string to the options list

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i; // Set the index of the current resolution
                }
            }

            resolutionDropdown.AddOptions(options); // Add the resolution options to the dropdown
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }
    }
}
