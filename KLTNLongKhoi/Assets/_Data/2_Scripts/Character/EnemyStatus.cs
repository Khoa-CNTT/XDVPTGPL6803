using UnityEngine;

namespace KLTNLongKhoi
{
    public class EnemyStatus : CharacterStatus
    {
        CharacterVision characterVision;

        protected override void Awake()
        {
            base.Awake();
            // Initialize enemy-specific properties if needed
            characterVision = GetComponentInChildren<CharacterVision>();
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
    }
}
