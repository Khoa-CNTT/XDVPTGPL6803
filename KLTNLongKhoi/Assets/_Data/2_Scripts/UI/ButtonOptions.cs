using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class ButtonOptions : MonoBehaviour
{
    [SerializeField] private List<Transform> optionButtons;
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color unselectedColor = new Color(1, 1, 1, 0.5f);

    private int currentIndex;

    public UnityEvent onClick;

    void Start()
    {
        // Setup click listeners cho mỗi button
        foreach (var option in optionButtons)
        {
            option.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(optionButtons.IndexOf(option)));
        }
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    private void OnOptionSelected(int index)
    {
        currentIndex = index;
        UpdateButtonsVisibility();
        onClick.Invoke(); // Thêm event hoặc callback nếu cần
    }

    private void UpdateButtonsVisibility()
    {
        for (int i = 0; i < optionButtons.Count; i++)
        {
            Transform button = optionButtons[i];
            Image image = button.GetComponent<Image>();
            image.color = i == currentIndex ? selectedColor : unselectedColor;
        }
    }

    public void SetCurrentIndex(int v)
    {
        if (v >= 0 && v < optionButtons.Count)
        {
            currentIndex = v;
            UpdateButtonsVisibility();
        }
        else
        {
            Debug.LogError("Index out of range: " + v);
        }
    }
}
