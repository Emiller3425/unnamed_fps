using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class StatsManager : MonoBehaviour, IDamageable, IHealable
{
    public EntityStats entityStats;
    protected int currentLevel;
    protected int maxLevel;
    protected float maxHealth;
    protected float currentHealth;
    protected int currentPistolAmmo;
    protected int maxPistolAmmo;
    protected int currentMachineGunAmmo;
    protected int maxMachineGunAmmo;
    protected int currentRifleAmmo;
    protected int maxRifleAmmo;
    public virtual void Awake()
    {
        currentHealth = entityStats.GetCurrentHealth();
        maxHealth = entityStats.GetMaxHealth();
        currentLevel = entityStats.GetCurrentLevel();
        maxLevel = entityStats.GetMaxLevel();
        currentPistolAmmo = entityStats.GetCurrentPistolAmmo();
        maxPistolAmmo = entityStats.GetMaxPistolAmmo();
        currentMachineGunAmmo = entityStats.GetCurrentMachineGunAmmo();
        maxMachineGunAmmo = entityStats.GetMaxMachineGunAmmo();
        currentRifleAmmo = entityStats.GetCurrentRifleAmmo();
        maxRifleAmmo = entityStats.GetMaxRifleAmmo();
    }

    public virtual void ApplyDamage(float damage)
    {
        currentHealth -= damage;
    }

    public virtual void ApplyHealing(float healing)
    {
        currentHealth += healing;
        if (currentHealth > entityStats.GetMaxHealth())
        {
            currentHealth = entityStats.GetMaxHealth();
        }
    }

    protected virtual void OnDestroy()
    {
        // TODO: Player death
    }
}