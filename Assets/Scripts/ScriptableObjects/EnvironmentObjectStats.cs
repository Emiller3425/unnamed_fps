using System.Buffers.Text;
using NUnit.Framework.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentObjectStats", menuName = "ScriptableObjects/EnvironmentObjectStats")]
public class EnvironmentObjectStats : ScriptableObject
{
    [SerializeField] protected float currentHealth = 100f;
    [SerializeField] protected float maxHealth = 100f;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

}