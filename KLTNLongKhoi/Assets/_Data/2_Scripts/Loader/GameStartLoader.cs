using UnityEngine;
using HieuDev;

namespace KLTNLongKhoi
{
    public class GameStartLoader : MonoBehaviour
    {
        LoadingUI loadingUI;
        LoadingProgress loadingProgress;
        public PanelPressToStart panelPressToStart;

        private void Start()
        {
            loadingUI = FindFirstObjectByType<LoadingUI>();
            loadingProgress = FindFirstObjectByType<LoadingProgress>();
        }

        private void FixedUpdate()
        {
            if (loadingProgress.getLoadingProgress() >= 1)
            {
                // delay 0.5s
                Invoke(nameof(EnablePanelPressToStart), 0.5f);
            }
        }

        private void EnablePanelPressToStart()
        {
            panelPressToStart.gameObject.SetActive(true);
            loadingUI.gameObject.SetActive(false);
        }
    }
}