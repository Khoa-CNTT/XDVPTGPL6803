using UnityEngine;

namespace KLTNLongKhoi
{
    public class GameManagerPlayerStats : MonoBehaviour
    {
        public static GameManagerPlayerStats Instance { get; private set; }

        public int HP;
        public int stamina;
        public int Money;
        public int Strength;
        public int Critical; // Đổi từ Charm thành Critical
        public int Intelligence;

        private UIPlayerStats uiStats; // Lưu tham chiếu UI
        void Awake()
        {
            uiStats = FindFirstObjectByType<UIPlayerStats>(); // Tìm UIPlayerStats lúc khởi tạo
        }

        //Nếu cần chức năng reroll stats ở select character creen
        public void RerollStats()
        {
            HP = Random.Range(1, 10);
            stamina = Random.Range(1, 10);
            Money = Random.Range(1, 10);
            Strength = Random.Range(1, 10);
            Critical = Random.Range(1, 10); // Đổi từ Charm thành Critical
            Intelligence = Random.Range(1, 10);
            uiStats?.UpdateStatsUIGameManagerPlayerStats(); // Cập nhật UI
        }
    }
}
