using UnityEngine;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(AudioSource))]
    public class ButtonSoundPlayer : MonoBehaviour
    {
        private AudioSource audioSource;
        [SerializeField] private AudioClip clickSound;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();

            // Nếu không có AudioSource được gán, tìm hoặc tạo mới
            if (audioSource == null)
            {
                // Tìm AudioSource trên GameObject này
                audioSource = GetComponent<AudioSource>();

                // Nếu không có, tạo mới
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                }
            }
        }

        private void OnEnable()
        {
            button.onClick.AddListener(PlayClickSound);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(PlayClickSound);
        }

        private void PlayClickSound()
        {
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
    }
}