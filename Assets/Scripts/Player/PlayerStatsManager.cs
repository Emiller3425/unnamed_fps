using UnityEngine;

public class PlayerStatsManager : StatsManager
{
    public static PlayerStatsManager Instance { get; private set; }
    private float maxExperiencePoints = 100;
    private float currentExperiencePoints;
    private float experienceToNextLevel;
    private static Vector3? respawnPosition;
    public override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
        experienceToNextLevel = maxExperiencePoints;
    }

    public void OnEnable()
    {
        GameEvents.current.OnUpdateRespawnPosition += UpdatePlayerRespawnPosition;
    }

    // initializes player UI on load with correct values
    private void Start()
    {
        ExperienceAdded(currentExperiencePoints);
        HealthAdded(currentHealth);
        GameEvents.current.LevelChanged(currentLevel);
        if (respawnPosition != null)
        {
            transform.position = respawnPosition.Value;
        }
    }

    public override void BulletDamage(float damage, Vector3 hitNormal)
    {
        base.BulletDamage(damage, hitNormal);
        // Update Health UI
        GameEvents.current.HealthSubtracted(damage, maxHealth, currentHealth);
        if (currentHealth <= 0f)
        {
            HandleDeath(0f);
        }
    }

    public override void ExplosiveDamage(float damage, Vector3 explosionOrigin=default, float explosionRadius=0f, float explosionForce=0f)
    {
        base.ExplosiveDamage(damage);
        // Update Health UI
        GameEvents.current.HealthSubtracted(damage, maxHealth, currentHealth);
        if (currentHealth <= 0f)
        {
            HandleDeath(0f);
        }
    }
    public override void HealthAdded(float healing)
    {
        base.HealthAdded(healing);
        // Update Health UI
        GameEvents.current.HealthAdded(healing, maxHealth, currentHealth);
    }
    protected override void HandleDeath(float timeToDestroy)
    {
        // base.HandleDeath(timeToDestroy);
        GameEvents.current.PlayerDeath();
    }

    public void ExperienceAdded(float experience, bool isLevelUp = false)
    {
        float previousExperiencePoints;
        previousExperiencePoints = maxExperiencePoints - experienceToNextLevel;

        if (!isLevelUp)
        {
            experienceToNextLevel -= experience;
        } else
        {
            previousExperiencePoints = 0f;
        }

        float currentExperiencePoints = maxExperiencePoints - experienceToNextLevel;

        // Update Experience UI
        GameEvents.current.ExperienceAdded(maxExperiencePoints, currentExperiencePoints, previousExperiencePoints, currentLevel);
        if (experienceToNextLevel <= 0)
        {
            // experience over is negative
            LevelUp(-experienceToNextLevel);
        }
     }
    public int GetPistolAmmo()
    {
        return currentPistolAmmo;
    }
    public void SetPistolAmmo(int ammo)
    {
        currentPistolAmmo = ammo;
    }
    public int GetSMGAmmo()
    {
        return currentSMGAmmo;
    }
    public void SetSMGAmmo(int ammo)
    {
        currentSMGAmmo = ammo;
    }
    public int GetRifleAmmo()
    {
        return currentRifleAmmo;
    }
    public void SetRifleAmmo(int ammo)
    {
        currentRifleAmmo = ammo;
    }

    public int GetEquipment()
    {
        return currentEquipment;
    }

    public void SetEquipment(int equipmentCount)
    {
        currentEquipment =  equipmentCount;
    }

    public void LevelUp(float experienceOver)
    {
        if (currentLevel < maxLevel)
        {
            currentLevel += 1;
        }
        IncreaseStatsOnLevelUp(experienceOver);
        if (experienceOver > 0)
            ExperienceAdded(experienceOver, true);
    }

    public void IncreaseStatsOnLevelUp(float experienceOver)
    {
        IncreaseHealth();
        IncreaseExperienceToNextLevel(experienceOver);
        // Update Health UI
        GameEvents.current.HealthAdded(0f, maxHealth, currentHealth);
    }

    public void IncreaseExperienceToNextLevel(float experienceOver)
    {
        maxExperiencePoints = currentLevel * 100;
        experienceToNextLevel = maxExperiencePoints - experienceOver;
    }
    public void IncreaseHealth()
    {
        maxHealth += 10f;
        currentHealth = maxHealth;
    }

    public void UpdatePlayerRespawnPosition(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
    }

    public void OnDisable()
    {
        GameEvents.current.OnUpdateRespawnPosition -= UpdatePlayerRespawnPosition;
    }

    protected override void OnDestroy()
    {
        // TODO: Player death screen
    }
} 