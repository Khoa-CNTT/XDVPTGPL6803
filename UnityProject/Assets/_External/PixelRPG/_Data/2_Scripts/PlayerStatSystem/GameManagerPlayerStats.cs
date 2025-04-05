using DG.Tweening.Core.Easing;
using UnityEngine;

public class GameManagerPlayerStats : MonoBehaviour
{
    //script này dùng để lưu trữ chỉ số không biến mất thì đổi sang scene khác
    public static GameManagerPlayerStats Instance { get; private set; }

    public int HP;
    public int Money;
    public int Strength;
    public int Charm;
    public int Intelligence;

    private UIPlayerStats uiStats; // Lưu tham chiếu UI
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Luôn tồn tại khi đổi scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        uiStats = FindFirstObjectByType<UIPlayerStats>(); // Tìm UIPlayerStats lúc khởi tạo
    }

    //Nếu cần chức năng reroll stats ở select character creen
    public void RerollStats()
    {
        HP = Random.Range(1, 10);
        Money = Random.Range(1, 10);
        Strength = Random.Range(1, 10);
        Charm = Random.Range(1, 10);
        Intelligence = Random.Range(1, 10);
        uiStats?.UpdateStatsUIGameManagerPlayerStats(); // Cập nhật UI
    }
}
