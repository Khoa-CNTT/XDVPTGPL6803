using UnityEngine;
using System;

namespace KLTNLongKhoi
{
    public class EnemyStatus : CharacterStatus
    {
        CharacterVision characterVision;
        [Header("Reward Settings")]
        [SerializeField] private float moneyReward = 100f; // Số tiền thưởng khi giết enemy
        private PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();
            // Initialize enemy-specific properties if needed
            characterVision = GetComponentInChildren<CharacterVision>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
        }

        protected override void Start()
        {
            base.Start();
            // Additional enemy initialization if needed
        }

        public override void TakeDamage(float damage, Vector3 hitDirection, Transform attacker)
        {
            base.TakeDamage(damage, hitDirection, attacker);
            // Additional logic for enemy damage handling if needed
            characterVision.Target = attacker;

        }

        protected override void HandleReward()
        {
            playerStatsManager.AddMoney(moneyReward);
        }
    }
}
