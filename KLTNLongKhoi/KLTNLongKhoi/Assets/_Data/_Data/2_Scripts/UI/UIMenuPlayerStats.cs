using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace KLTNLongKhoi
{
    public class UIMenuPlayerStats : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> moneyText;

        SaveLoadManager saveLoadManager;
        PlayerData playerData = null; 
        void Start()
        {
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
            playerData = saveLoadManager.GetGameData().player;
        }

        void FixedUpdate()
        {
            foreach (var text in moneyText) text.text = $"{playerData.money}";
        }
    }
}
