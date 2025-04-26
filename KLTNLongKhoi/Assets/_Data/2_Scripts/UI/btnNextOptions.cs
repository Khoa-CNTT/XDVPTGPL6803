using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class btnNextOptions : MonoBehaviour
    {
        [SerializeField] private Button btnLeft;
        [SerializeField] private Button btnRight;
        [SerializeField] private TextMeshProUGUI displayText;

        public string[] options;
        [SerializeField] private int currentIndex = 0;

        public UnityEvent onClick;

        void Start()
        {
            btnLeft.onClick.AddListener(OnLeftButtonClick);
            btnRight.onClick.AddListener(OnRightButtonClick);
            UpdateDisplayText();
        }

        private void OnLeftButtonClick()
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = options.Length - 1;
            }
            UpdateDisplayText();
            onClick.Invoke();
        }

        private void OnRightButtonClick()
        {
            currentIndex++;
            if (currentIndex >= options.Length)
            {
                currentIndex = 0;
            }
            UpdateDisplayText();
            onClick.Invoke();
        }

        private void UpdateDisplayText()
        {
            if (options != null && options.Length > 0 && currentIndex >= 0 && currentIndex < options.Length)
            {
                displayText.text = options[currentIndex];
            }
        }

        public string GetCurrentOption()
        {
            if (options != null && options.Length > 0 && currentIndex >= 0 && currentIndex < options.Length)
            {
                return options[currentIndex];
            }
            return string.Empty;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public void SetCurrentIndex(int index)
        {
            if (index >= 0 && index < options.Length)
            {
                currentIndex = index;
                UpdateDisplayText();
            }
        }

        public void SetOptions(string[] newOptions, string currentValue = null)
        {
            options = newOptions;
            
            if (currentValue != null)
            {
                int index = System.Array.IndexOf(options, currentValue);
                currentIndex = index != -1 ? index : 0;
            }
            else
            {
                currentIndex = 0;
            }
            
            UpdateDisplayText();
        }

        #region Option Generators
        [ContextMenu("Generate Resolution Options")]
        public void GenerateResolutionOptions()
        {
            Resolution[] resolutions = Screen.resolutions;
            options = new string[resolutions.Length];

            for (int i = 0; i < resolutions.Length; i++)
            {
                Resolution resolution = resolutions[i];
                options[i] = $"{resolution.width}x{resolution.height}";
            }

            currentIndex = options.Length - 1; // Highest resolution by default
            UpdateDisplayText();
        }

        [ContextMenu("Generate Quality Options")]
        public void GenerateQualityOptions()
        {
            options = new string[] { "Thấp", "Trung bình", "Cao", "Cực cao" };
            currentIndex = 1; // Default to Medium
            UpdateDisplayText();
        }

        [ContextMenu("Generate FPS Options")]
        public void GenerateFPSOptions()
        {
            options = new string[] { "30 FPS", "60 FPS", "120 FPS", "144 FPS", "240 FPS" };
            currentIndex = 1; // Default to 60 FPS
            UpdateDisplayText();
        }

        [ContextMenu("Generate Brightness Options")]
        public void GenerateBrightnessOptions()
        {
            options = new string[] { "Tối", "Tối hơn", "Bình thường", "Sáng", "Sáng hơn" };
            currentIndex = 2; // Default to Normal
            UpdateDisplayText();
        }

        [ContextMenu("Generate Toggle Options")]
        public void GenerateToggleOptions()
        {
            options = new string[] { "Tắt", "Bật" };
            currentIndex = 0; // Default to Off
            UpdateDisplayText();
        }
        #endregion
    }
}
