using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string chapterName;
    public DateTime saveTime;
    public int currentSceneIndex;
    public int currentDialogueIndex;
    public string[] inventory;
    public Dictionary<string, bool> flags; // For tracking story flags/choices
    public string screenshotBase64; // Store screenshot as base64 string

    public SaveData(string chapter)
    {
        chapterName = chapter;
        saveTime = DateTime.Now;
        flags = new Dictionary<string, bool>();
    }
} 