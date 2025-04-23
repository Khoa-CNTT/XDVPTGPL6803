using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ButtonOptions : MonoBehaviour
{
    [System.Serializable]
    public class OptionButton
    {
        public Transform buttonTransform;
        public string value;
    }

    [SerializeField] private List<OptionButton> optionButtons;
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color unselectedColor = new Color(1, 1, 1, 0.5f);
    
    private string currentValue;

    void Start()
    {
        // Setup click listeners cho mỗi button
        foreach (var option in optionButtons)
        {
            option.buttonTransform.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(option.value));
        }
    }

    public void SetCurrentOption(string value)
    {
        currentValue = value;
        UpdateButtonsVisibility();
    }

    private void OnOptionSelected(string value)
    {
        currentValue = value;
        UpdateButtonsVisibility();
        // Thêm event hoặc callback nếu cần
    }

    private void UpdateButtonsVisibility()
    {
        foreach (var option in optionButtons)
        {
            // Giữ Image component enabled nhưng thay đổi alpha
            Color color = option.value == currentValue ? selectedColor : unselectedColor;
            option.buttonTransform.GetComponent<Image>().color = color;
            
            // Button vẫn có thể click được vì Image vẫn enabled
        }
    }
}
