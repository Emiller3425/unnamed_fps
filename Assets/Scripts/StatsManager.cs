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
    protected float currentHealth;
    protected int currentPistolAmmo;
    protected int currentMachineGunAmmo;
    protected int currentRifleAmmo;
    protected virtual void Awake()
    {
        currentHealth = entityStats.GetHealth();
    }

    public virtual void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public virtual void ApplyHealing(float healing)
    {
        currentHealth += healing;
        if (currentHealth > entityStats.GetHealth())
        {
            currentHealth = entityStats.GetHealth();
        }
    }

    protected virtual void OnDestroy()
    {
        // TODO: Player death
    }
}