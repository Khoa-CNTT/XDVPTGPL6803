using UnityEngine;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    public class SoundEffectButton : MonoBehaviour
    {
        [SerializeField] private AudioSource buttonAudioSources;
        [SerializeField] private AudioClip soundEffectOnClick;

        private void Start()
        {
            GetComponent<UnityEngine.UI.Button>()?.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (buttonAudioSources != null && soundEffectOnClick != null)
            {
                buttonAudioSources.PlayOneShot(soundEffectOnClick);
            }
        }
    }
}
