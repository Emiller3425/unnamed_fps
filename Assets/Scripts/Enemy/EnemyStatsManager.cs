using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class EnemyStatsManager : MonoBehaviour, IDamageable
{
    public EnemyStats enemyStats;
    private float currentHealth;
    private int currentPistolAmmo;
    private int currentMachineGunAmmo;
    private int currentRifleAmmo;
    void Awake()
    {
        currentHealth = enemyStats.GetHealth();
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy health:" + currentHealth);
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    } 

    public void ApplyHealing(float healing)
    {
        currentHealth += healing;
        if (currentHealth > enemyStats.GetHealth())
        {
            currentHealth = enemyStats.GetHealth();
        }
    }

    void OnDestroy()
    {
        // on destroy logic---death screen
    }
}