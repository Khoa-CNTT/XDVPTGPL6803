using UnityEngine;

public class BoxHealing : MonoBehaviour
{
    public float healAmount = 10f;
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if hit any damageable entity
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Heal(healAmount);
        }
    }
}
