
using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class UpgrageAndCraftOpener : MonoBehaviour
    {
        public UnityEvent openPanel;
        public UnityEvent closePanel;
        PauseManager pauseManager;
        bool isOpen = false;

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
            if (Input.GetKeyDown(KeyCode.X))
            {
                OnClick();
            }
        }

        public void OnClick()
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