using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class PlayerStatsManager : StatsManager
{
    public static PlayerStatsManager Instance { get; private set; }
    private int maxExperiencePoints = 100;
    private int experienceToNextLevel;

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

    // initializes player UI based on stats on load
    private void Start()
    {
        ExperienceAdded(0);
        HealthAdded(0);
    }

    public override void HealthSubtracted(float damage)
    {
        base.HealthSubtracted(damage);
        GameEvents.current.HealthSubtracted(damage, entityStats.GetMaxHealth(), currentHealth);
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
    public override void HealthAdded(float healing)
    {
        base.HealthAdded(healing);
        GameEvents.current.HealthAdded(healing, entityStats.GetMaxHealth(), currentHealth);
    }

    public void ExperienceAdded(int experience)
    {
        experienceToNextLevel -= experience;
        if (experienceToNextLevel <= 0)
        {
            LevelUp(experienceToNextLevel);
        }
        // Update Experience UI
        GameEvents.current.ExperienceAdded(maxExperiencePoints, maxExperiencePoints - experienceToNextLevel);
     }
    public void LevelUp(int experienceOver)
    {
        if (currentLevel < maxLevel)
        {
            currentLevel += 1;
        }
        IncreaseStatsOnLevelUp(experienceOver);
        if (experienceOver > 0)
            ExperienceAdded(experienceOver);
    }

    public void IncreaseStatsOnLevelUp(int experienceOver)
    {
        IncreaseHealth();
        IncreaseExperienceToNextLevel(experienceOver);
    }

    public void IncreaseHealth()
    {
        maxHealth += 10f;
        currentHealth = maxHealth;
    }
    
    public void IncreaseExperienceToNextLevel(int experienceOver)
    {
        maxExperiencePoints = currentLevel * 100;
        // experience over is negative
        experienceToNextLevel = maxExperiencePoints + experienceOver;
    }
    protected override void OnDestroy()
    {
        // TODO: Player death
    }
}