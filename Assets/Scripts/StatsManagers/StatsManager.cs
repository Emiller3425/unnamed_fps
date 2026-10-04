using UnityEngine;

public class StatsManager : MonoBehaviour, IDamageable
{
    public virtual void BulletDamage(float damage, Vector3 hitNormal)
    {

    }

    public virtual void ExplosiveDamage(float damage, Vector3 explosionOrigin=default, float explosionRadius=0f, float explosionForce=0f)
    {

    }
}