using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace KLTNLongKhoi
{
    public class GlobalButtonSoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip defaultClickSound;
        [SerializeField] private float volume = 1.0f;
        
        [Tooltip("Nếu true, sẽ tự động tìm và thêm âm thanh cho tất cả button khi scene được load")]
        [SerializeField] private bool autoFindAllButtons = true;
        
        [Tooltip("Danh sách các button cụ thể cần thêm âm thanh (nếu không dùng autoFindAllButtons)")]
        [SerializeField] private List<Button> specificButtons;
        
        private void Awake()
        {
            // Nếu không có AudioSource được gán, tạo mới
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }
        
        private void Start()
        {
            
            if (autoFindAllButtons)
            {
                // Tìm tất cả button trong scene
                Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
                
                int addedCount = 0;
                foreach (Button button in allButtons)
                {
                    if (button.gameObject.scene.isLoaded)
                    {
                        // Kiểm tra xem button đã có ButtonSoundPlayer chưa
                        if (button.GetComponent<ButtonSoundPlayer>() == null)
                        {
                            // Thêm listener cho button
                            button.onClick.AddListener(() => PlayClickSound());
                            addedCount++;
                        }
                    }
                }
            }
            else if (specificButtons != null && specificButtons.Count > 0)
            {
                // Thêm listener cho các button cụ thể
                foreach (Button button in specificButtons)
                {
                    if (button != null && button.GetComponent<ButtonSoundPlayer>() == null)
                    {
                        button.onClick.AddListener(() => PlayClickSound());
                    }
                }
            }
        }
        
        public void PlayClickSound()
        {
            if (audioSource != null && defaultClickSound != null)
            {
                audioSource.PlayOneShot(defaultClickSound, volume);
            }
            else
            {
                Debug.LogWarning($"GlobalButtonSoundManager: Cannot play sound, audioSource={audioSource}, defaultClickSound={defaultClickSound}");
            }
        }
        
        // Phương thức để thêm button mới vào danh sách (có thể gọi từ script khác)
        public void AddButtonSound(Button button)
        {
            if (button != null && button.GetComponent<ButtonSoundPlayer>() == null)
            {
                button.onClick.AddListener(() => PlayClickSound());
            }
        }
    }
}

