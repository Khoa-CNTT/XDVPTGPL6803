using UnityEngine;

public class BoxTakeDamage : MonoBehaviour
{
    public float damage = 10f;
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if hit any damageable entity
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Vector3 hitDirection = (other.transform.position - transform.position).normalized;
            damageable.TakeDamage(damage, hitDirection);
        }
    }
}
