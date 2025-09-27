using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Health : MonoBehaviour
{
    private float maxHealth = 100f;
    private float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void applyDamage(float damage)
    {
        currentHealth -= damage;
    }
    void applyHealing(float healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}