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
                GameOver();
            }
        }

        private void GameOver()
        {
            if (isGameOver) return;

            Invoke(nameof(ShowGameOverPanel), 1f);
            isGameOver = true;
            pauseManager.PauseGame();
        }

        private void ShowGameOverPanel()
        {
            onGameOver?.Invoke();
        }

        // UI chọn restart gameplay
        public void RestartGame()
        {
            isGameOver = false;
            pauseManager.ResumeGame();
            playerStatus.RespawnToLastPoint();
            playerStatsManager.ResetStats();
        }

        private void WinGame()
        {
            if (isGameOver) return;

            isGameOver = true;
            pauseManager.PauseGame();
            onWinGame?.Invoke();
        }
    }
}
