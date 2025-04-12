using UnityEngine;

namespace KLTNLongKhoi
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameOverPanel gameOverPanel;
        [SerializeField] private SaveLoadManager saveLoadManager;
        [SerializeField] private GameObject saveLoadPanel;
        [SerializeField] private CharacterStatus finalBoss;
        [SerializeField] private GameObject youWinPanel;

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
            // Existing code...
            if (saveLoadPanel != null)
            {
                saveLoadPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (finalBoss != null && finalBoss.IsDead())
            {
                WinGame();
            }
        }


        public void GameOver()
        {
            if (isGameOver) return;

            isGameOver = true;
            // delay 1 khoản thời gian rồi hiện bản gameover
            Invoke(nameof(ShowGameOverPanel), 1f);

            // Thêm logic game over khác ở đây
            // Ví dụ: dừng nhạc, lưu điểm cao, etc.
        }

        private void ShowGameOverPanel()
        {
            gameOverPanel.Show();
            pauseManager.PauseGame();
        }

        // UI chọn restart gameplay
        public void RestartGame()
        {
            isGameOver = false;
            gameOverPanel.Hide();
            pauseManager.ResumeGame();

            // Reset game state
            playerStatus.RespawnToLastPoint();
            playerStatsManager.ResetStats();
        }

        private void WinGame()
        {
            if (isGameOver) return;

            isGameOver = true;
            youWinPanel.SetActive(true);
            pauseManager.PauseGame();
        }
    }
}
