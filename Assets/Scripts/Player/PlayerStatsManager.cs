using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerStatsManager : MonoBehaviour, IDamageable
{
    public PlayerStats playerStats;
    private float currentHealth;
    private int currentPistolAmmo;
    private int currentMachineGunAmmo;
    private int currentRifleAmmo;
    void Awake()
    {
        currentHealth = playerStats.GetHealth();
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        HealthBarUIManager.Instance.ApplyDamage(damage, playerStats.GetHealth(), currentHealth);
        if (currentHealth < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyHealing(float healing)
    {
        currentHealth += healing;
        HealthBarUIManager.Instance.ApplyHealing(healing, playerStats.GetHealth(), currentHealth);
        if (currentHealth > playerStats.GetHealth())
        {
            currentHealth = playerStats.GetHealth();
        }
    }

    void OnDestroy()
    {
         // on destroy logic---death screen
    }
}