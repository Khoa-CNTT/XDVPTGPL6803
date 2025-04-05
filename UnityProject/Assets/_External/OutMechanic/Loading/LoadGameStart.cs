using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace Core
{
    /// <summary> Loading khi mới khởi động game </summary>
    public class LoadGameStart : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtLoadingCounting;
        [SerializeField] private Slider filledLoading;
        private AsyncOperation m_async;

        private void Start()
        {
            m_async = SceneManager.LoadSceneAsync(SceneName.GameData.ToString(), LoadSceneMode.Additive);
            // m_async = SceneManager.LoadSceneAsync(GameScene.OptionsMenu.ToString(), LoadSceneMode.Additive);
        }

        private void FixedUpdate()
        {
            SetBarLoading(m_async.progress);

            // Neu scene DataHolder thuc su duoc load het
            if (m_async.isDone)
            {
                SceneManager.LoadScene(SceneName.Menu.ToString());
            }
        }

        public void SetBarLoading(float loadingProgress)
        {
            if (txtLoadingCounting)
            {
                txtLoadingCounting.text = $"ĐANG TẢI {(loadingProgress * 100).ToString("f0")}%";
            }

            if (filledLoading)
            {
                filledLoading.value = loadingProgress;
            }
        }
    }
}
