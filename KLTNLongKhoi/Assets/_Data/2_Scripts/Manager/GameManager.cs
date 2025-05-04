using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CharacterStatus finalBoss;

        [Header("Events")]
        public UnityEvent onGameOver;
        public UnityEvent onWinGame;
        public UnityEvent onRestartGame;
        [SerializeField] private float delayTimeShowPanel = 1.5f;

        private PlayerStatsManager playerStatsManager;
        private PlayerStatus playerStatus;

        PauseManager pauseManager;

        [Header("Game State")]
        private bool isGameOver = false;
        public bool IsGameOver => isGameOver;

        private void Awake()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            playerStatus = FindFirstObjectByType<PlayerStatus>();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (finalBoss != null && finalBoss.IsDead())
            {
                WinGame();
            }

            if (playerStatus.IsDead())
            {
                LoseGame();
            }
        }

        private void LoseGame()
        {
            if (isGameOver) return;

            Invoke(nameof(ShowLosePanel), delayTimeShowPanel);
            isGameOver = true;
            pauseManager.PauseGame();
        }

        private void ShowLosePanel()
        {
            onGameOver?.Invoke();
        }

        // UI chọn restart gameplay
        public void RestartGame()
        {
            onRestartGame?.Invoke();
        }

        public void ContinueGame()
        {
            isGameOver = false;
            finalBoss = null;
            pauseManager.ResumeGame();
        }

        private void WinGame()
        {
            if (isGameOver) return;

            Invoke(nameof(ShowWinPanel), delayTimeShowPanel);

            isGameOver = true;
            pauseManager.PauseGame();
        }

        private void ShowWinPanel()
        {
            onWinGame?.Invoke();
        }
    }
}
