using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private GameObject saveSlotPrefab;
    [SerializeField] private Transform[] savePageContainers; // Array of containers for each page
    [SerializeField] private GameObject saveLoadPanel;
    [SerializeField] private int slotsPerPage = 6;
    [SerializeField] private Camera screenshotCamera;
    [SerializeField] private Button[] pageButtons; // Array of page selection buttons

    private string savePath;
    private bool isSaveMode;
    private int currentPage = 0;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saves");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // Validate arrays
        if (savePageContainers == null || savePageContainers.Length == 0)
        {
            Debug.LogError("No save page containers assigned!");
            return;
        }

        if (pageButtons == null || pageButtons.Length == 0)
        {
            Debug.LogError("No page buttons assigned!");
            return;
        }

        if (pageButtons.Length != savePageContainers.Length)
        {
            Debug.LogError($"Number of page buttons ({pageButtons.Length}) does not match number of containers ({savePageContainers.Length})!");
            return;
        }

        // Initialize page buttons
        for (int i = 0; i < pageButtons.Length; i++)
        {
            if (pageButtons[i] != null)
            {
                int pageIndex = i;
                pageButtons[i].onClick.AddListener(() => SwitchToPage(pageIndex));
            }
        }

        // Hide all pages except the first one
        for (int i = 1; i < savePageContainers.Length; i++)
        {
            if (savePageContainers[i] != null)
            {
                savePageContainers[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowSavePanel()
    {
        isSaveMode = true;
        saveLoadPanel.SetActive(true);
        RefreshCurrentPage();
    }

    public void ShowLoadPanel()
    {
        isSaveMode = false;
        saveLoadPanel.SetActive(true);
        RefreshCurrentPage();
    }

    private void SwitchToPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= savePageContainers.Length)
        {
            Debug.LogError($"Invalid page index: {pageIndex}. Must be between 0 and {savePageContainers.Length - 1}");
            return;
        }

        if (savePageContainers[currentPage] != null)
        {
            savePageContainers[currentPage].gameObject.SetActive(false);
        }
        
        currentPage = pageIndex;
        
        if (savePageContainers[currentPage] != null)
        {
            savePageContainers[currentPage].gameObject.SetActive(true);
            RefreshCurrentPage();
            UpdatePageButtonsVisual();
        }
    }

    private void UpdatePageButtonsVisual()
    {
        for (int i = 0; i < pageButtons.Length; i++)
        {
            // Assuming you have some way to show selected state (e.g., different color)
            pageButtons[i].GetComponent<Image>().color = (i == currentPage) ? Color.gray : Color.white;
        }
    }

    private void RefreshCurrentPage()
    {
        Transform currentContainer = savePageContainers[currentPage];
        
        // Clear existing slots
        foreach (Transform child in currentContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new slots for current page
        for (int i = 0; i < slotsPerPage; i++)
        {
            GameObject slot = Instantiate(saveSlotPrefab, currentContainer);
            SaveSlotUI slotUI = slot.GetComponent<SaveSlotUI>();
            int globalSlotIndex = (currentPage * slotsPerPage) + i;
            slotUI.Initialize(globalSlotIndex, OnSlotSelected);
        }
    }

    private string CaptureScreenshot()
    {
        RenderTexture rt = new RenderTexture(480, 270, 24);
        screenshotCamera.targetTexture = rt;
        
        screenshotCamera.Render();
        RenderTexture.active = rt;
        
        Texture2D screenshot = new Texture2D(480, 270, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, 480, 270), 0, 0);
        screenshot.Apply();
        
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        
        byte[] bytes = screenshot.EncodeToPNG();
        Destroy(screenshot);
        
        return Convert.ToBase64String(bytes);
    }

    private void OnSlotSelected(int slotIndex)
    {
        if (isSaveMode)
        {
            SaveGame(slotIndex);
        }
        else
        {
            LoadGame(slotIndex);
        }
        saveLoadPanel.SetActive(false);
    }

    private void SaveGame(int slotIndex)
    {
        string currentChapter = "A Trip To Paradise"; // Example
        
        SaveData saveData = new SaveData(currentChapter);
        saveData.currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        saveData.screenshotBase64 = CaptureScreenshot();
        
        string json = JsonUtility.ToJson(saveData);
        string filePath = Path.Combine(savePath, $"save_{slotIndex}.json");
        File.WriteAllText(filePath, json);
    }

    private void LoadGame(int slotIndex)
    {
        string filePath = Path.Combine(savePath, $"save_{slotIndex}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            UnityEngine.SceneManagement.SceneManager.LoadScene(saveData.currentSceneIndex);
        }
    }
} 