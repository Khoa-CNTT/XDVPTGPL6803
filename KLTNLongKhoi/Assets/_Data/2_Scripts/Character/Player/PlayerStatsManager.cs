using UnityEngine;

namespace KLTNLongKhoi
{
    public class PlayerStatsManager : MonoBehaviour, ISaveData
    {
        [Header("Base Stats")]
        [SerializeField] private int baseHP = 5;
        [SerializeField] private int baseStamina = 5;
        [SerializeField] private float baseMana = 5;
        [SerializeField] private int baseMoney = 0;
        [SerializeField] private int baseStrength = 5;
        [SerializeField] private int baseCritical = 5;
        [SerializeField] private int baseIntelligence = 5;

        [Header("Current Stats")]
        private float currentHealth;
        private float currentStamina;
        private float currentMana;
        private float manaRegenRate = 2f;
        private int level = 1;
        private int experience = 0;
        private Vector3 lastCheckpoint;

        [Header("Derived Stats")]
        public float MaxHealth => baseHP * 10f;
        public float MaxStamina => baseStamina * 10f;
        public float MaxMana => baseMana * 10f;
        public float DamageReduction => baseStrength * 0.05f;
        public float AttackPower => baseStrength * 0.1f;
        public float CriticalChance => baseCritical * 0.01f;
        public float SpellPower => baseIntelligence * 0.1f;

        [Header("Level System")]
        private int[] experienceRequirements = { 100, 300, 600, 1000, 1500, 2100, 2800, 3600, 4500, 5500 };

        // Public Properties
        public Vector3 LastCheckpoint => lastCheckpoint;
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
        public float BaseMana => baseMana;
        public int BaseMoney => baseMoney;
        public int BaseStrength => baseStrength;
        public int BaseCritical => baseCritical;
        public int BaseIntelligence => baseIntelligence;

        private void Awake()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            currentHealth = MaxHealth;
            currentMana = MaxMana;
            currentStamina = MaxStamina;
        }

        private void Start()
        {
            StartManaRegeneration();
        }

        private void Update()
        {
            RegenerateMana();
        }

        public void RerollStats()
        {
            baseHP = Random.Range(1, 10);
            baseStamina = Random.Range(1, 10);
            baseMoney = Random.Range(1, 10);
            baseStrength = Random.Range(1, 10);
            baseCritical = Random.Range(1, 10);
            baseIntelligence = Random.Range(1, 10);

            InitializeStats();
            OnStatsUpdated();
        }

        #region Save/Load Data
        public T SaveData<T>()
        {
            PlayerData data = new PlayerData
            {
                name = "Player",
                level = this.level,
                experience = this.experience,
                health = this.baseHP,
                position = transform.position,
                currency = this.baseMoney,
                baseStrength = this.baseStrength,
                baseCritical = this.baseCritical,
                baseIntelligence = this.baseIntelligence,
                baseStamina = this.baseStamina,
                inventory = new InventoryData()
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

                InitializeStats();

                if (transform != null)
                {
                    transform.position = playerData.position;
                }

                OnStatsUpdated();
            }
        }
        #endregion

        #region Stat Modification Methods
        public void SetCheckpoint(Vector3 position)
        {
            lastCheckpoint = position;
        }

        public void ResetStats()
        {
            CurrentHealth = MaxHealth;
            CurrentMana = MaxMana;
            CurrentStamina = MaxStamina;
        }

        public void ModifyHealth(float amount)
        {
            CurrentHealth += amount;
        }

        public void ModifyMana(float amount)
        {
            CurrentMana += amount;
        }

        public void ModifyStamina(float amount)
        {
            CurrentStamina += amount;
        }

        public void ModifyMoney(int amount)
        {
            baseMoney += amount;
            OnStatsUpdated();
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
                damage *= 2f;
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
            baseHP += 5;
            baseStamina += 5;
            baseStrength += 2;
            baseCritical += 1;
            baseIntelligence += 2;

            InitializeStats();
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
