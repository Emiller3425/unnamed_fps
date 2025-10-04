using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Pistol : SemiAutoGun
{
    protected override void Awake()
    {
        base.Awake();
        magSize = 12;
        currentAmmo = playerStats != null ? playerStats.GetPistolAmmo() : enemyStats.GetAmmo();
        currentMag = magSize;
    }
    protected override void Start()
    {
        // Set default values for Pistol
        damage = 10;
        maxReloadBuffer = 1.5f;
        maxFireRateBuffer = 0.2f;
        bulletVelocity = 45f;
        flattenTrajectoryRange = 5f;
        // sets currentAmmo to maxAmmo
        base.Start();
    }
}