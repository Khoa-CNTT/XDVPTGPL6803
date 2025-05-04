using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace KLTNLongKhoi
{
    public class PlayerStatsManager : MonoBehaviour
    {
        [SerializeField] PlayerData playerData;
        [SerializeField] private float currentHP;
        [SerializeField] private float currentSP;
        [SerializeField] private float currentMP;

        // chỉ số kinh nghiệm cần để lên cấp
        private int[] experienceRequirements = { 100, 300, 600, 1000, 1500, 2100, 2800, 3600, 4500, 5500 };

        private SaveLoadManager saveLoadManager;
        private PlayerStatus playerStatus;

        public event Action StatsUpdatedEvent;

        // Public Properties
        public float GetPhysicsDamage => PlayerData.baseStr * GetCriticalChance;
        public float GetMagicDamage => PlayerData.baseInt * GetCriticalChance;
        public float GetCriticalChance => UnityEngine.Random.Range(PlayerData.baseInt * 0.5f, PlayerData.baseInt * 1.5f) / 100f;
        public float CurrentHP
        {
            get => currentHP;
            set
            {
                currentHP = Mathf.Clamp(value, 0, PlayerData.baseHP);
                OnStatsUpdated();
            }
        }
        public float CurrentSP
        {
            get => currentSP;
            set
            {
                currentSP = Mathf.Clamp(value, 0, PlayerData.baseSP);
                OnStatsUpdated();
            }
        }
        public float CurrentMP
        {
            get => currentMP;
            set
            {
                currentMP = Mathf.Clamp(value, 0, PlayerData.baseMP);
                OnStatsUpdated();
            }
        }
        public float CurrentMoney
        {
            get => PlayerData.money;
            set
            {
                PlayerData.money = value;
                OnStatsUpdated();
            }
        }
        public PlayerData PlayerData { get => playerData; set => playerData = value; }

        private void Awake()
        {
            saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
            playerStatus = FindFirstObjectByType<PlayerStatus>();

        }

        private void Start()
        {
            PlayerData = saveLoadManager.GetGameData().player;
            saveLoadManager.OnLoaded += () =>
            {
                PlayerData = saveLoadManager.GetGameData().player;
                Init();
            };
        }

        public void Init()
        {
            if (saveLoadManager.IsNewGameplay()) return;
            CurrentHP = PlayerData.baseHP;
            CurrentSP = PlayerData.baseSP;
            CurrentMP = PlayerData.baseMP;
            CurrentMoney = PlayerData.money;

            playerStatus.transform.position = PlayerData.position;
            OnStatsUpdated();
        }

        private void OnDestroy()
        {
            saveLoadManager.SaveData(PlayerData);
        }

        public void SetCheckpoint(Vector3 position)
        {
            PlayerData.position = position;
        }

        public Vector3 LastCheckpointPosition() => PlayerData.position;

        public void AddExperience(int amount)
        {
            PlayerData.experience += amount;
            LevelUp();
            OnStatsUpdated();
        }

        public void AddMoney(float amount)
        {
            Debug.Log($"Add money: {amount}");
            PlayerData.money += amount;
            OnStatsUpdated();
        }

        private void LevelUp()
        {
            if (PlayerData.experience < experienceRequirements[(int)PlayerData.level - 1]) return;
            PlayerData.level++;
            PlayerData.baseHP += 5;
            PlayerData.baseSP += 5;
            PlayerData.baseStr += 2;
            PlayerData.baseCri += 5;
            PlayerData.baseInt += 2;

            Init();
            OnStatsUpdated();
        }

        private void OnStatsUpdated()
        {
            saveLoadManager.SaveData(PlayerData);
            StatsUpdatedEvent?.Invoke();
        }
    }
}
