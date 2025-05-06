using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{

    public class PauseManager : MonoBehaviour
    {
        private bool isPaused = false;

        public UnityEvent onGamePaused;
        public UnityEvent onGameResumed;

        public bool IsPaused => isPaused;

        public void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        public void SetPause(bool value)
        {
            isPaused = value;
            if (isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        public void PauseGame()
        {
            isPaused = true;
            onGamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            isPaused = false;
            onGameResumed?.Invoke();
        }
    }

}