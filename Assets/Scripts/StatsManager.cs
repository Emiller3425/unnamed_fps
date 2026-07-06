using UnityEngine;

public class StatsManager : MonoBehaviour, IDamageable, IHealable
{
    public EntityStats entityStats;
    public bool isDead;
    protected int currentLevel;
    protected int maxLevel;
    protected float maxHealth;
    protected float currentHealth;
    protected int currentPistolAmmo;
    protected int maxPistolAmmo;
    protected int currentSMGAmmo;
    protected int maxSMGAmmo;
    protected int currentRifleAmmo;
    protected int maxRifleAmmo;
    protected int maxEquipment;
    protected int currentEquipment;
    protected int instanceId;
    public virtual void Awake()
    {
        currentHealth = entityStats.GetCurrentHealth();
        maxHealth = entityStats.GetMaxHealth();
        currentLevel = entityStats.GetCurrentLevel();
        maxLevel = entityStats.GetMaxLevel();
        currentPistolAmmo = entityStats.GetCurrentPistolAmmo();
        maxPistolAmmo = entityStats.GetMaxPistolAmmo();
        currentSMGAmmo = entityStats.GetCurrentSMGAmmo();
        maxSMGAmmo = entityStats.GetMaxSMGAmmo();
        currentRifleAmmo = entityStats.GetCurrentRifleAmmo();
        maxRifleAmmo = entityStats.GetMaxRifleAmmo();
        currentEquipment = entityStats.GetCurrentEquipment();
        maxEquipment = entityStats.GetMaxEquipement();
        instanceId = gameObject.GetInstanceID();
    }

    public virtual void BulletDamage(float damage)
    {
        currentHealth -= damage;
    }

    public virtual void ExplosiveDamage(float damage, Vector3 explosionOrigin=default, float explosionRadius=0f, float explosionForce=0f)
    {
        currentHealth -= damage;
    }

    public virtual void HealthAdded(float healing)
    {
        currentHealth += healing;
        if (currentHealth > entityStats.GetMaxHealth())
        {
            currentHealth = entityStats.GetMaxHealth();
        }
    }

    protected virtual void HandleDeath(float timeToDestroy)
    {
        Destroy(gameObject, timeToDestroy);
        isDead = true;
        OmitInstanceIdOnDeath();
    }

    protected virtual void OmitInstanceIdOnDeath()
    {
        GameEvents.current.EntityDeath(instanceId);
    }

    protected virtual void OnDestroy()
    {
        // TODO: Player death
    }
}