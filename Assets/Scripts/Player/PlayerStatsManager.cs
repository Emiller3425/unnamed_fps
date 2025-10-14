using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerStatsManager : StatsManager
{
    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);
        HealthBarUIManager.Instance.ApplyDamage(damage, entityStats.GetHealth(), currentHealth);
    }
    public override void ApplyHealing(float healing)
    {
        base.ApplyHealing(healing);
        HealthBarUIManager.Instance.ApplyHealing(healing, entityStats.GetHealth(), currentHealth);
    }
    protected override void OnDestroy()
    {
        // TODO: Player death
    }
}