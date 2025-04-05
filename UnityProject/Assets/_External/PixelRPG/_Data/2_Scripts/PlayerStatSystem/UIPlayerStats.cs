using TMPro;
using UnityEngine;

public class UIPlayerStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI charmText;
    [SerializeField] private TextMeshProUGUI intelligenceText;

    private GameManagerPlayerStats playerStatsA;
    private PlayerStats playerStatsB;

    void Start()
    {
        GameObject obj = GameObject.Find("PlayerStatsManager"); // Tìm theo tên GameObject trước
        if (obj != null)
        {
            playerStatsB = obj.GetComponent<PlayerStats>(); // Lấy script từ GameObject
        }
        else
        {
            playerStatsB = FindFirstObjectByType<PlayerStats>(); // Nếu không tìm thấy theo tên, tìm theo script
        }

        if (playerStatsB != null)
        {
            UpdateStatsUIPlayerStats();
        }
        else
        {
            playerStatsA = GameManagerPlayerStats.Instance;
            UpdateStatsUIGameManagerPlayerStats();
        }
    }

    public void UpdateStatsUIGameManagerPlayerStats()
    {
        if (playerStatsA == null) return;

        hpText.text = $"HP: {playerStatsA.HP}";
        moneyText.text = $"Money: {playerStatsA.Money}";
        strengthText.text = $"Strength: {playerStatsA.Strength}";
        charmText.text = $"Charm: {playerStatsA.Charm}";
        intelligenceText.text = $"Intelligence: {playerStatsA.Intelligence}";
    }

    public void UpdateStatsUIPlayerStats()
    {
        if (playerStatsB == null) return;

        hpText.text = $"HP: {playerStatsB.HP}";
        moneyText.text = $"Money: {playerStatsB.Money}";
        strengthText.text = $"Strength: {playerStatsB.Strength}";
        charmText.text = $"Charm: {playerStatsB.Charm}";
        intelligenceText.text = $"Intelligence: {playerStatsB.Intelligence}";
    }
}
