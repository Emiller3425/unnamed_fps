using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class EnemyStatsManager : StatsManager
{
    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);
    }
    protected override void OnDestroy()
    {
        // TODO: Enemy death -- ideally a death animation
    }
}