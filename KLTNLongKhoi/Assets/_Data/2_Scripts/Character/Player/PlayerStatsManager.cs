using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace KLTNLongKhoi
{
    public class PlayerStatsManager : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
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
                StatsUpdatedEvent?.Invoke();
            }
        }
        public float CurrentSP
        {
            get => currentSP;
            set
            {
                currentSP = Mathf.Clamp(value, 0, PlayerData.baseSP);
                StatsUpdatedEvent?.Invoke();
            }
        }
        public float CurrentMP
        {
            get => currentMP;
            set
            {
                currentMP = Mathf.Clamp(value, 0, PlayerData.baseMP);
                StatsUpdatedEvent?.Invoke();
            }
        }
        public float CurrentMoney
        {
            get => PlayerData.money;
            set
            {
                PlayerData.money = value;
                StatsUpdatedEvent?.Invoke();
                saveLoadManager.SaveData(PlayerData);
            }
        }

        public PlayerData PlayerData { get => playerData; set => playerData = value; }

        private void Awake()
        {
            // Sử dụng Instance thay vì FindFirstObjectByType
            saveLoadManager = SaveLoadManager.Instance;
            
            if (saveLoadManager == null)
            {
                Debug.LogError("PlayerStatsManager: SaveLoadManager.Instance is null");
                // Thử tìm kiếm một lần nữa
                saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
            }
            
            playerStatus = FindFirstObjectByType<PlayerStatus>();
        }

        // Note: Cách load save
        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            saveLoadManager.OnLoaded += Init;
        }

        private void OnDisable()
        {
            saveLoadManager.OnLoaded -= Init;
            saveLoadManager.SaveData(PlayerData);
        }

        public void Init()
        {
            if (saveLoadManager.IsNewGameplay() == false)
            {
                PlayerData = saveLoadManager.GetGameData().player;
                playerStatus.transform.position = PlayerData.position;
            }

            CurrentHP = PlayerData.baseHP;
            CurrentSP = PlayerData.baseSP;
            CurrentMP = PlayerData.baseMP;
            CurrentMoney = PlayerData.money;
        }

        public void SetCheckpoint(Vector3 position)
        {
            PlayerData.position = position;
            PlayerData.IsNewGameplay = false;
        }

        public Vector3 LastCheckpointPosition() => PlayerData.position;

        public void AddExperience(int amount)
        {
            PlayerData.experience += amount;
            LevelUp();
            saveLoadManager.SaveData(PlayerData);
        }

        public void AddMoney(float amount)
        { 
            CurrentMoney += amount;
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
            StatsUpdatedEvent?.Invoke();
            saveLoadManager.SaveData(PlayerData);
        }
    }
}
