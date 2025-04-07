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

        PauseManager pauseManager;

        [Header("Game State")]
        private bool isGameOver = false;
        public bool IsGameOver => isGameOver;

        private void Awake()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        private void Start()
        {
            // Existing code...
            if (saveLoadPanel != null)
            {
                saveLoadPanel.SetActive(false);
            }
        }

        public void GameOver()
        {
            if (isGameOver) return;

            isGameOver = true;
            gameOverPanel?.Show();
            pauseManager.PauseGame();

            // Thêm logic game over khác ở đây
            // Ví dụ: dừng nhạc, lưu điểm cao, etc.
        }

        public void RestartGame()
        {
            isGameOver = false;
            gameOverPanel?.Hide();
            pauseManager.ResumeGame();

            // Reset game state
            // ...
        }

        private void Update()
        {
            if (finalBoss != null && finalBoss.IsDead())
            {
                WinGame();
            }
        }

        private void WinGame()
        {
            if (isGameOver) return;
            
            isGameOver = true;
            youWinPanel?.SetActive(true);
            pauseManager.PauseGame();
        }
    }
}
