using UnityEngine;

public class ActorHitbox : MonoBehaviour
{
    [SerializeField] GameObject hitParticle;
    [SerializeField] bool isAttacking;
    [SerializeField] Animator animator;
    [SerializeField] string EnemyTag;
    private Collider colliderBoxHit;

    public bool IsAttacking
    {
        get => isAttacking; 
        set
        {
            isAttacking = value;
            colliderBoxHit.enabled = isAttacking;
        }
    }

    private void Start()
    {
        colliderBoxHit = GetComponent<Collider>();
        IsAttacking = false;
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
                damageable.TakeDamage(10f, hitDirection);
                Instantiate(hitParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), other.transform.rotation);
            }
        }
    }
}
