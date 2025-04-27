using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class BagOpener : MonoBehaviour
    {
        public UnityEvent openPanel;
        public UnityEvent closePanel;
        PauseManager pauseManager;
        [SerializeField] bool isOpen = false;

        private void Start()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        private void Update()
        {
            if (pauseManager.IsPaused && isOpen == false)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isOpen = !isOpen;

                if (isOpen)
                {
                    OpenStorage();
                }
                else
                {
                    CloseStorage();
                }
            }
        }

        public void OpenStorage()
        {
            openPanel.Invoke();
            pauseManager.PauseGame();
            isOpen = true;
        }

        public void CloseStorage()
        {
            closePanel.Invoke();
            pauseManager.ResumeGame();
            isOpen = false;
        }
    }
}