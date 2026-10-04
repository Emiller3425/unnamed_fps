using UnityEngine;

public class EnvironmentObjectStatsManager : StatsManager, IDamageable
{
    public EnvironmentObjectStats environmentObjectStats;
    protected float maxHealth;
    protected float currentHealth;
    protected int instanceId;
    public virtual void Awake()
    {
        // Health
        currentHealth = environmentObjectStats.GetCurrentHealth();
        maxHealth = environmentObjectStats.GetMaxHealth();
        // Instance Id
        instanceId = gameObject.GetInstanceID();
    }

    public override void BulletDamage(float damage, Vector3 hitNormal)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            HandleDestroy();
        }
    }

    public override void ExplosiveDamage(float damage, Vector3 explosionOrigin=default, float explosionRadius=0f, float explosionForce=0f)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            HandleDestroy();
        }
    }

    protected virtual void HandleDestroy()
    {
        OmitInstanceIdOnDeath();
    }

    protected virtual void OmitInstanceIdOnDeath()
    {
        GameEvents.current.BreakMesh(instanceId);
    }

}