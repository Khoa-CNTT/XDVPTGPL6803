using UnityEngine;

namespace KLTNLongKhoi
{
    public class PlayerStatsManager : MonoBehaviour, ISaveData
    {
        [Header("Base Stats")]
        private int baseHP;
        private int baseStamina;
        private int baseMoney;
        private int baseStrength;
        private int baseCritical; // Đổi từ baseCharm thành baseCritical
        private int baseIntelligence;
        private int level = 1;
        private int experience = 0;

        [Header("Current Stats")]
        private float currentHealth;
        private float currentMana;
        private float manaRegenRate = 2f; // Mana regen per second

        [Header("Derived Stats")]
        public float MaxHealth => baseHP * 10f;
        public float MaxStamina => baseStamina * 10f;
        public float MaxMana => baseIntelligence * 10f;
        public float DamageReduction => baseStrength * 0.05f;
        public float AttackPower => baseStrength * 0.1f;
        public float CriticalChance => baseCritical * 0.01f;
        public float SpellPower => baseIntelligence * 0.1f;

        [Header("Level System")]
        private int[] experienceRequirements = { 100, 300, 600, 1000, 1500, 2100, 2800, 3600, 4500, 5500 };

        // Public Properties
        public int Level => level;
        public int Experience => experience;
        public float CurrentHealth 
        {
            get => currentHealth;
            set
            {
                currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                OnStatsUpdated();
            }
        }
        public float CurrentMana
        {
            get => currentMana;
            set
            {
                currentMana = Mathf.Clamp(value, 0, MaxMana);
                OnStatsUpdated();
            }
        }
        public float CurrentStamina
        {
            get => currentStamina;
            set
            {
                currentStamina = Mathf.Clamp(value, 0, MaxStamina);
                OnStatsUpdated();
            }
        }
        public int BaseHP => baseHP;
        public int BaseStamina => baseStamina;
        public int BaseMoney => baseMoney;
        public int BaseStrength => baseStrength;
        public int BaseCritical => baseCritical; // Đổi từ BaseCharm thành BaseCritical
        public int BaseIntelligence => baseIntelligence;

        private float currentStamina;

        private void Start()
        {
            LoadStatsFromGameManager();
            currentHealth = MaxHealth;
            currentMana = MaxMana;
            StartManaRegeneration();
        }

        private void Update()
        {
            RegenerateMana();
        }

        #region Save/Load Data
        public T SaveData<T>()
        {
            PlayerData data = new PlayerData
            {
                name = "Player", // Có thể thêm system đặt tên sau
                level = this.level,
                experience = this.experience,
                health = this.baseHP,
                position = transform.position,
                currency = this.baseMoney,
                baseStrength = this.baseStrength,
                baseCritical = this.baseCritical,
                baseIntelligence = this.baseIntelligence,
                baseStamina = this.baseStamina,
                inventory = new InventoryData() // Inventory system sẽ xử lý riêng
            };
            
            return (T)(object)data;
        }

        public void LoadData<T>(T data)
        {
            if (data is PlayerData playerData)
            {
                level = playerData.level;
                experience = playerData.experience;
                baseHP = playerData.health;
                baseMoney = playerData.currency;
                baseStrength = playerData.baseStrength;
                baseCritical = playerData.baseCritical;
                baseIntelligence = playerData.baseIntelligence;
                baseStamina = playerData.baseStamina;
                
                // Cập nhật các chỉ số derived
                currentHealth = MaxHealth;
                currentMana = MaxMana;
                currentStamina = MaxStamina;
                
                // Cập nhật position nếu cần
                if (transform != null)
                {
                    transform.position = playerData.position;
                }

                // Cập nhật GameManagerPlayerStats
                if (GameManagerPlayerStats.Instance != null)
                {
                    GameManagerPlayerStats.Instance.HP = baseHP;
                    GameManagerPlayerStats.Instance.Money = baseMoney;
                    GameManagerPlayerStats.Instance.Strength = baseStrength;
                    GameManagerPlayerStats.Instance.Critical = baseCritical;
                    GameManagerPlayerStats.Instance.Intelligence = baseIntelligence;
                }

                StatsUpdatedEvent?.Invoke();
            }
        }
        #endregion

        public void LoadStatsFromGameManager()
        {
            if (GameManagerPlayerStats.Instance == null) return;

            baseHP = GameManagerPlayerStats.Instance.HP;
            baseMoney = GameManagerPlayerStats.Instance.Money;
            baseStrength = GameManagerPlayerStats.Instance.Strength;
            baseCritical = GameManagerPlayerStats.Instance.Critical; // Đã đổi từ Charm sang Critical
            baseIntelligence = GameManagerPlayerStats.Instance.Intelligence;
            baseStamina = GameManagerPlayerStats.Instance.stamina; // Lấy từ GameManager thay vì hardcode

            // Cập nhật các chỉ số hiện tại
            currentHealth = MaxHealth;
            currentMana = MaxMana;
            currentStamina = MaxStamina;

            StatsUpdatedEvent?.Invoke();
        }

        #region Stat Modification Methods
        public void ModifyHealth(float amount)
        {
            CurrentHealth += amount;
        }

        public void ModifyMana(float amount)
        {
            CurrentMana += amount;
        }

        public void ModifyMoney(int amount)
        {
            baseMoney += amount;
            if (GameManagerPlayerStats.Instance != null)
            {
                GameManagerPlayerStats.Instance.Money = baseMoney;
            }
            StatsUpdatedEvent?.Invoke();
        }

        public bool HasEnoughMana(float manaCost)
        {
            return CurrentMana >= manaCost;
        }

        public void ConsumeMana(float amount)
        {
            if (HasEnoughMana(amount))
            {
                ModifyMana(-amount);
            }
        }
        #endregion

        #region Combat Methods
        public float CalculateFinalDamage(float rawDamage)
        {
            return Mathf.Max(0, rawDamage * (1 - DamageReduction));
        }

        public float CalculateSpellDamage(float baseDamage)
        {
            float damage = baseDamage * (1 + SpellPower);
            if (Random.value <= CriticalChance)
            {
                damage *= 2f; // Critical hit multiplier
            }
            return damage;
        }
        #endregion

        #region Level System
        public void AddExperience(int amount)
        {
            experience += amount;
            CheckLevelUp();
            OnStatsUpdated();
        }

        private void CheckLevelUp()
        {
            while (level - 1 < experienceRequirements.Length && 
                   experience >= experienceRequirements[level - 1])
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            level++;
            // Increase base stats
            baseHP += 5;
            baseStamina += 5;
            baseStrength += 2;
            baseCritical += 1; // Đổi từ baseCharm thành baseCritical
            baseIntelligence += 2;

            // Heal to full on level up
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;

            OnLevelUp?.Invoke(level);
            OnStatsUpdated();
        }
        #endregion

        #region Mana Regeneration
        private void StartManaRegeneration()
        {
            InvokeRepeating(nameof(RegenerateMana), 0f, 0.1f);
        }

        private void RegenerateMana()
        {
            if (CurrentMana < MaxMana)
            {
                ModifyMana(manaRegenRate * Time.deltaTime);
            }
        }
        #endregion

        #region Events
        private void OnStatsUpdated()
        {
            StatsUpdatedEvent?.Invoke();
        }

        public delegate void StatsUpdateHandler();
        public event StatsUpdateHandler StatsUpdatedEvent;

        public delegate void LevelUpHandler(int newLevel);
        public event LevelUpHandler OnLevelUp;
        #endregion
    }
}
