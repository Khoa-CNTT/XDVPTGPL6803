using UnityEngine;
using System;

namespace KLTNLongKhoi
{
    public class EnemyStatus : CharacterStatus
    {
        [SerializeField] MonsterData monsterData;
        [SerializeField] private float moneyReward = 100f;
        [SerializeField] private float timeHurt = 2f;

        private PlayerStatsManager playerStatsManager;
        private CharacterVision characterVision;
        private EnemyCtrl enemyCtrl;
        private Animator animator;

        protected override void Awake()
        {
            base.Awake();
            characterVision = GetComponentInChildren<CharacterVision>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            enemyCtrl = GetComponent<EnemyCtrl>();
            animator = GetComponent<Animator>();
        }

        public override void TakeDamage(float damage, Vector3 hitDirection, Transform attacker)
        {
            base.TakeDamage(damage, hitDirection, attacker);
            characterVision.Target = attacker;
            if (!IsDead() && enemyCtrl.CanMove)
            {
                animator.SetTrigger("Hurt");
                Invoke("CanMove", timeHurt);
            }
        }

        protected override void OnDie()
        {
            playerStatsManager.AddMoney(moneyReward);
            GameEvents.Notify(GameEventType.EnemyDefeated, monsterData.id);
        }

        private void CanMove()
        {
            enemyCtrl.CanMove = true;
        }
    }
}
