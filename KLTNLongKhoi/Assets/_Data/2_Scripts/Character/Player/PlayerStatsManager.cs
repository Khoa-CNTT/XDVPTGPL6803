using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace KLTNLongKhoi
{
    public class PlayerStatsManager : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField] PlayerData playerData;
        [SerializeField] private float currentHP;
        [SerializeField] private float currentSP;
        [SerializeField] private float currentMP;

        // chỉ số kinh nghiệm cần để lên cấp
        private int[] experienceRequirements = { 100, 300, 600, 1000, 1500, 2100, 2800, 3600, 4500, 5500 };

        private SaveLoadManager saveLoadManager;
        private PlayerStatus playerStatus;

        public UnityEvent StatsUpdatedEvent;

        // Public Properties
        public float GetPhysicsDamage => playerData.baseStr * GetCriticalChance;
        public float GetMagicDamage => playerData.baseInt * GetCriticalChance;
        public float GetCriticalChance => Random.Range(playerData.baseInt * 0.5f, playerData.baseInt * 1.5f) / 100f;
        public float BaseHP => playerData.baseHP;
        public float BaseSP => playerData.baseSP;
        public float BaseMP => playerData.baseMP;
        public float CurrentHP
        {
            get => currentHP;
            set
            {
                currentHP = Mathf.Clamp(value, 0, playerData.baseHP);
                OnStatsUpdated();
            }
        }
        public float CurrentSP
        {
            get => currentSP;
            set
            {
                currentSP = Mathf.Clamp(value, 0, playerData.baseSP);
                OnStatsUpdated();
            }
        }
        public float CurrentMP
        {
            get => currentMP;
            set
            {
                currentMP = Mathf.Clamp(value, 0, playerData.baseMP);
                OnStatsUpdated();
            }
        }
        public float CurrentMoney
        {
            get => playerData.money;
            set
            {
                playerData.money = value;
                OnStatsUpdated();
            }
        }

        private void Awake()
        {
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
            playerStatus = FindFirstObjectByType<PlayerStatus>();
        }

        private void Start()
        {
            playerData = saveLoadManager.GetGameData().player;
            Init();
        }

        public void Init()
        {
            if (saveLoadManager.IsNewGameplay()) return;
            CurrentHP = playerData.baseHP;
            CurrentSP = playerData.baseSP;
            CurrentMP = playerData.baseMP;
            CurrentMoney = playerData.money;

            playerStatus.transform.position = playerData.position;
            OnStatsUpdated();
        }

        private void OnDestroy()
        {
            saveLoadManager.SaveData(playerData);
        }

        public void SetCheckpoint(Vector3 position)
        {
            playerData.position = position;
        }

        public Vector3 LastCheckpointPosition() => playerData.position;

        public void AddExperience(int amount)
        {
            playerData.experience += amount;
            LevelUp();
            OnStatsUpdated();
        }

        public void AddMoney(float amount)
        {
            Debug.Log($"Add money: {amount}");
            playerData.money += amount;
            OnStatsUpdated();
        }

        private void LevelUp()
        {
            if (playerData.experience < experienceRequirements[(int)playerData.level - 1]) return;
            playerData.level++;
            playerData.baseHP += 5;
            playerData.baseSP += 5;
            playerData.baseStr += 2;
            playerData.baseCri += 5;
            playerData.baseInt += 2;

            Init();
            OnStatsUpdated();
        }

        private void OnStatsUpdated()
        {
            StatsUpdatedEvent?.Invoke();
        }
    }
}
