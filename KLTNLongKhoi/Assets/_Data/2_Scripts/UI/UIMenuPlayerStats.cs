using TMPro;
using UnityEngine;

namespace KLTNLongKhoi
{
    public class UIMenuPlayerStats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameAndLevel;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI moneyText2;
        [SerializeField] private TextMeshProUGUI diamondsText;
        [SerializeField] private TextMeshProUGUI diamondsText2;

        PlayerData playerData = null;

        void FixedUpdate()
        {
            playerData = DataManager.Instance.GameData.player;
            if (nameAndLevel != null) nameAndLevel.text = $"Name: {playerData.name} + Level: {playerData.level}";
            if (moneyText != null) moneyText.text = $"Money: {playerData.currency}";
            if (moneyText2 != null) moneyText2.text = $"Money: {playerData.currency}";
            if (diamondsText != null) diamondsText.text = $"Diamonds: {playerData.diamonds}";
            if (diamondsText2 != null) diamondsText2.text = $"Diamonds: {playerData.diamonds}";
        }
    }
}
