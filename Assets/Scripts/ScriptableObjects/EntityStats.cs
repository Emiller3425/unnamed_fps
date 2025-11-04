using System.Buffers.Text;
using NUnit.Framework.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "ScriptableObjects/EntityStats")]
public class EntityStats : ScriptableObject
{
    [SerializeField] protected int currentLevel = 1;
    [SerializeField] protected int maxLevel = 100;
    [SerializeField] protected float currentHealth = 100f;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected int currentPistolAmmo = 72;
    [SerializeField] protected int maxPistolAmmo = 72;
    [SerializeField] protected int currentMachineGunAmmo = 160;
    [SerializeField] protected int maxMachineGunAmmo = 160;
    [SerializeField] protected int currentRifleAmmo = 180;
    [SerializeField] protected int maxRifleAmmo = 180;

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public int GetMaxLevel()
    {
        return maxLevel;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetCurrentPistolAmmo()
    {
        return currentPistolAmmo;
    }
    public int GetMaxPistolAmmo()
    {
        return maxPistolAmmo;
    }
    public int GetCurrentMachineGunAmmo()
    {
        return currentMachineGunAmmo;
    }
    public int GetMaxMachineGunAmmo()
    {
        return maxMachineGunAmmo;
    }
    public int GetCurrentRifleAmmo()
    {
        return currentRifleAmmo;
    }
    public int GetMaxRifleAmmo()
    {
        return maxRifleAmmo;
    }
}