using UnityEngine;

namespace KLTNLongKhoi
{
    public class ActorHitbox : MonoBehaviour
    {
        [SerializeField] GameObject hitParticle;
        [SerializeField] string EnemyTag = "Enemy";
        [SerializeField] BoxCollider boxCollider;

        public float damage = 100f;
        public Transform attacker;

        private CharacterAnimationEvents characterAnimationEvents;

        private void Awake()
        {
            characterAnimationEvents = GetComponentInParent<CharacterAnimationEvents>();
            if (characterAnimationEvents != null)
            {
                characterAnimationEvents.onSendDamage += OnSendDamage;
            }

            if (boxCollider == null)
            {
                boxCollider = GetComponent<BoxCollider>();
            }

            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }
        }

        private void OnSendDamage(int sendDamage)
        {
            if (boxCollider == null) return;

            boxCollider.enabled = (sendDamage == 1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(EnemyTag))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    Vector3 hitDirection = (other.transform.position - transform.position).normalized;
                    damageable.TakeDamage(damage, hitDirection, attacker);

                    if (hitParticle != null)
                    {
                        Instantiate(hitParticle, transform.position, Quaternion.identity);
                    }

                    if (boxCollider != null)
                    {
                        boxCollider.enabled = false;
                    }
                }
            }
        }
    }
}
