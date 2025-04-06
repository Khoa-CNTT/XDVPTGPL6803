using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, Vector3 hitDirection);
    void Heal(float amount);
}