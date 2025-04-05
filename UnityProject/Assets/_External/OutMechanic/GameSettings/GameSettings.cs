using UnityEngine;
using UnityEngine.SceneManagement;

namespace MBM
{
    public class GameSettings : MonoBehaviour
    {

        // Hàm để điều chỉnh âm lượng nhạc nền
        public void SetBackgroundMusicVolume(float volume)
        {
            AudioManager.Instance.SetVolume("BackgroundMusic", volume);
        }

        // Hàm để điều chỉnh âm lượng hiệu ứng
        public void SetEffectsVolume(float volume)
        {
            AudioManager.Instance.SetVolume("Effects", volume);
        }

        // Hàm để thay đổi độ phân giải video
        public void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }

        // Hàm để chuyển đổi chế độ toàn màn hình
        public void ToggleFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        // Hàm để thay đổi chất lượng đồ họa
        public void SetGraphicsQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        // Hàm để thiết lập điều khiển
        public void SetControlScheme(string controlScheme)
        {
            ControlManager.Instance.SetScheme(controlScheme);
        }

        // Hàm để thay đổi ngôn ngữ
        public void SetLanguage(string languageCode)
        {
            LanguageManager.Instance.SetLanguage(languageCode);
        }

        public void SaveGame()
        {
            // Lưu tên của scene hiện tại
            string currentSceneName = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("SavedScene", currentSceneName);

            // Lưu âm lượng nhạc nền
            float backgroundMusicVolume = AudioManager.Instance.GetVolume("BackgroundMusic");
            PlayerPrefs.SetFloat("BackgroundMusicVolume", backgroundMusicVolume);

            // Lưu âm lượng hiệu ứng
            float effectsVolume = AudioManager.Instance.GetVolume("Effects");
            PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);

            // Lưu chế độ toàn màn hình
            bool isFullScreen = Screen.fullScreen;
            PlayerPrefs.SetInt("FullScreenMode", isFullScreen ? 1 : 0);

            // Lưu độ phân giải
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;
            PlayerPrefs.SetInt("ScreenWidth", screenWidth);
            PlayerPrefs.SetInt("ScreenHeight", screenHeight);

            // Lưu chất lượng đồ họa
            int graphicsQuality = QualitySettings.GetQualityLevel();
            PlayerPrefs.SetInt("GraphicsQuality", graphicsQuality);

            PlayerPrefs.Save();
        }

        public void LoadGame()
        {
            // Đọc tên của scene đã lưu và load scene đó
            string savedSceneName = PlayerPrefs.GetString("SavedScene", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(savedSceneName);

            // Tải âm lượng nhạc nền
            float backgroundMusicVolume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 1.0f);
            AudioManager.Instance.SetVolume("BackgroundMusic", backgroundMusicVolume);

            // Tải âm lượng hiệu ứng
            float effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
            AudioManager.Instance.SetVolume("Effects", effectsVolume);

            // Tải chế độ toàn màn hình
            bool isFullScreen = PlayerPrefs.GetInt("FullScreenMode", 1) == 1;
            Screen.fullScreen = isFullScreen;

            // Tải độ phân giải
            int screenWidth = PlayerPrefs.GetInt("ScreenWidth", Screen.width);
            int screenHeight = PlayerPrefs.GetInt("ScreenHeight", Screen.height);
            Screen.SetResolution(screenWidth, screenHeight, Screen.fullScreen);

            // Tải chất lượng đồ họa
            int graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", QualitySettings.GetQualityLevel());
            QualitySettings.SetQualityLevel(graphicsQuality);
        }
    }
}
