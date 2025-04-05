using DG.Tweening.Core.Easing;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //script này dùng để lấy chỉ số từ GameManagerPlayerStats, player sẽ xài các thuộc tính chỉ số từ script này
    public int HP;
    public int Money;
    public int Strength;
    public int Charm;
    public int Intelligence;

    private UIPlayerStats UiStats; // Lưu tham chiếu UI

    void Start()
    {
        UiStats = FindFirstObjectByType<UIPlayerStats>(); // Tìm UI tự động
        UpdateStatsFromGameManager();
    }

    public void UpdateStatsFromGameManager()
    {
        if (GameManagerPlayerStats.Instance != null && UiStats != null)
        {
            HP = GameManagerPlayerStats.Instance.HP;
            Money = GameManagerPlayerStats.Instance.Money;
            Strength = GameManagerPlayerStats.Instance.Strength;
            Charm = GameManagerPlayerStats.Instance.Charm;
            Intelligence = GameManagerPlayerStats.Instance.Intelligence;
            if (UiStats != null)
            {
                UiStats.UpdateStatsUIPlayerStats();
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameManager, dùng giá trị mặc định!");
        }
    }
}
