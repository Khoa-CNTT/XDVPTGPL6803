using UnityEngine;
using System.Collections;

namespace KLTNLongKhoi
{
    public class GameIntro : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private GameObject playerUI;
        [SerializeField] private DialogContent introDialogContent;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip introMusic;
        [SerializeField] private Animator introAnimator; 
        [SerializeField] private GameObject mainVirtualCamera;
        
        private PauseManager pauseManager;
        private SaveLoadManager saveLoadManager;
        private bool introCompleted = false;

        private void Awake()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
        }

        private void Start()
        {
            // Kiểm tra xem đây có phải là game mới không
            if (saveLoadManager != null && saveLoadManager.IsNewGameplay())
            {
                StartIntro();
            }
            else
            {
                // Nếu không phải game mới, tắt intro ngay
                gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (introCompleted) return;

            // nhận biết animation kết thúc thông qua normalizedTime
            // if (introAnimator != null)
            // {
            //     if (introAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            //     {
            //         EndIntro();
            //     }
            // }
            
            // kiểm tra dialog box kết thúc
            if (dialogBox != null && !dialogBox.gameObject.activeSelf)
            {
                EndIntro();
            }
        }

        private void StartIntro()
        {
            // Pause game
            pauseManager.PauseGame();
            playerUI.SetActive(false);
            mainVirtualCamera.SetActive(false);

            // Phát nhạc nền
            if (musicSource != null && introMusic != null)
            {
                musicSource.clip = introMusic;
                musicSource.Play();
            }

            // Hiển thị dialog box
            if (dialogBox != null && introDialogContent != null)
            {
                dialogBox.SetDialogLines(introDialogContent);
            }
        }

        private void OnAnimationEnd()
        {
            // Được gọi khi animation kết thúc thông qua sự kiện từ CharacterAnimationEvents
            Debug.Log("Intro animation ended");
        }

        private void EndIntro()
        {
            if (introCompleted) return;
            
            introCompleted = true;
            
            // Resume game
            pauseManager.ResumeGame();
            playerUI.SetActive(true);
            mainVirtualCamera.SetActive(true);
            
            // Tắt đối tượng intro
            gameObject.SetActive(false);
        }
    }
}
