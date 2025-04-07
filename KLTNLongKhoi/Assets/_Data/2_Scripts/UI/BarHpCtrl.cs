using UnityEngine;
using Microlight.MicroBar;

namespace KLTNLongKhoi
{
    public class BarHpCtrl : MonoBehaviour
    {
        [SerializeField] MicroBar microBar;
        private float hideDelay = 3f; // Thời gian delay trước khi ẩn (3 giây)
        private float hideTimer;

        private void Update()
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0)
            {
                HideHealthBar();
            }
        }

        public void ShowHealthBar()
        {
            if (microBar != null)
            {
                microBar.gameObject.SetActive(true);
                hideTimer = hideDelay;
            }
        }

        private void HideHealthBar()
        {
            if (microBar != null)
            {
                microBar.gameObject.SetActive(false);
            }
        }
    }
}
