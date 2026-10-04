using UnityEngine;

public class EntityStatsManager : StatsManager, IDamageable, IHealable
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
    protected int maxShotgunAmmo;
    protected int currentShotgunAmmo;
    protected int maxEquipment;
    protected int currentEquipment;
    protected int instanceId;
    public virtual void Awake()
    {
        // Health
        currentHealth = entityStats.GetCurrentHealth();
        maxHealth = entityStats.GetMaxHealth();
        // Level
        currentLevel = entityStats.GetCurrentLevel();
        maxLevel = entityStats.GetMaxLevel();
        // Pistol Ammo
        currentPistolAmmo = entityStats.GetCurrentPistolAmmo();
        maxPistolAmmo = entityStats.GetMaxPistolAmmo();
        // SMG Ammo
        currentSMGAmmo = entityStats.GetCurrentSMGAmmo();
        maxSMGAmmo = entityStats.GetMaxSMGAmmo();
        // Rifle Ammo
        currentRifleAmmo = entityStats.GetCurrentRifleAmmo();
        maxRifleAmmo = entityStats.GetMaxRifleAmmo();
        // Shotgun Ammo
        currentShotgunAmmo = entityStats.GetCurrentShotgunAmmo();
        maxShotgunAmmo = entityStats.GetMaxShotgunAmmo();
        // Equipment
        currentEquipment = entityStats.GetCurrentEquipment();
        maxEquipment = entityStats.GetMaxEquipement();
        // Instance Id
        instanceId = gameObject.GetInstanceID();
    }

    public override void BulletDamage(float damage, Vector3 hitNormal)
    {
        currentHealth -= damage;
    }

    public override void ExplosiveDamage(float damage, Vector3 explosionOrigin=default, float explosionRadius=0f, float explosionForce=0f)
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
        // TODO: What to do on player death in regards to stats....
        // - should xp stay?
        // - should current ammo stay?
    }
}