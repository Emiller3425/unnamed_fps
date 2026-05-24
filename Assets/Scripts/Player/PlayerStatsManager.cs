using UnityEngine;

public class PlayerStatsManager : StatsManager
{
    public static PlayerStatsManager Instance { get; private set; }
    private float maxExperiencePoints = 100;
    private float currentExperiencePoints;
    private float experienceToNextLevel;

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

    // initializes player UI on load with correct values
    private void Start()
    {
        ExperienceAdded(currentExperiencePoints);
        HealthAdded(currentHealth);
    }

    public override void HealthSubtracted(float damage)
    {
        base.HealthSubtracted(damage);
        // Update Health UI
        GameEvents.current.HealthSubtracted(damage, maxHealth, currentHealth);
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
    public override void HealthAdded(float healing)
    {
        base.HealthAdded(healing);
        // Update Health UI
        GameEvents.current.HealthAdded(healing, maxHealth, currentHealth);
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
        GameEvents.current.ExperienceAdded(maxExperiencePoints, currentExperiencePoints, previousExperiencePoints);
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
        currentRifleAmmo -= ammo;
    }

    public void LevelUp(float experienceOver)
    {
        if (currentLevel < maxLevel)
        {
            currentLevel += 1;
        }
        Debug.Log(experienceOver);
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
    protected override void OnDestroy()
    {
        // TODO: Player death
    }
} 