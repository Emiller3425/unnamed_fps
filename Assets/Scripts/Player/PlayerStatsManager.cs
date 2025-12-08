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

    public void Start()
    {
        ExperienceBarUIManager.Instance.AddExperiencePoints(maxExperiencePoints, maxExperiencePoints - experienceToNextLevel);
    }

    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);
        HealthBarUIManager.Instance.ApplyDamage(damage, entityStats.GetCurrentHealth(), currentHealth);
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
    public override void ApplyHealing(float healing)
    {
        base.ApplyHealing(healing);
        HealthBarUIManager.Instance.ApplyHealing(healing, entityStats.GetCurrentHealth(), currentHealth);
    }

    public void AddExperiencePoints(int experience)
    {
        experienceToNextLevel -= experience;
        if (experienceToNextLevel <= 0)
        {
            LevelUp(experienceToNextLevel);
        }
        ExperienceBarUIManager.Instance.AddExperiencePoints(maxExperiencePoints, maxExperiencePoints - experienceToNextLevel);
     }
    public void LevelUp(int experienceOver)
    {
        if (currentLevel < maxLevel)
        {
            currentLevel += 1;
        }
        IncreaseStatsOnLevelUp(experienceOver);
        if (experienceOver > 0)
            AddExperiencePoints(experienceOver);
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