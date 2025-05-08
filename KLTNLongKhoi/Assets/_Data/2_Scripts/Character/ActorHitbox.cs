using KLTNLongKhoi;
using Unity.VisualScripting;
using UnityEngine;

public class ActorHitbox : MonoBehaviour
{
    [SerializeField] GameObject hitParticle;
    [SerializeField] bool isAttacking;
    [SerializeField] string EnemyTag;
    [SerializeField] Transform attacker;
    public float damage = 100f;
    private CharacterAnimationEvents characterAnimationEvents;
    private BoxCollider boxCollider;

    private void Awake()
    {
        characterAnimationEvents = GetComponentInParent<CharacterAnimationEvents>();
        boxCollider = GetComponent<BoxCollider>();
        characterAnimationEvents.onSendDamage += OnSendDamage;
    }

    private void OnSendDamage(int sendDamage)
    {
        if (sendDamage == 1)
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == EnemyTag)
        {
            // Check if hit any damageable entity
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Vector3 hitDirection = (other.transform.position - transform.position).normalized;
                damageable.TakeDamage(damage, hitDirection, attacker);
                Instantiate(hitParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), other.transform.rotation);
                boxCollider.enabled = false;
            }
        }
    }
}
