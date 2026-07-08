using UnityEngine;

public interface IDamageable
{
    void BulletDamage(float damage, Vector3 hitNormal); // For bullets
    void ExplosiveDamage(float damage, Vector3 explosionOrigin, float explosionRadius, float explosionForce);
}