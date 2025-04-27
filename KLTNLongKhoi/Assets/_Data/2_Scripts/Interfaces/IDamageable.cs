using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, Vector3 hitDirection, Transform attacker);
    void RestoreHealth(float amount);
}