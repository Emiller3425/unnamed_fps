using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class BurstRifle : BurstGun
{
    protected override void Awake()
    {
        base.Awake();
        magSize = 35;
        currentAmmo = entityStats.GetRifleAmmo();
        currentMag = magSize;
    }
    protected override void Start()
    {
        // Set default values for BurstRifle
        damage = 12;
        maxReloadBuffer = 2.5f;
        maxFireRateBuffer = 0.1f;
        // sets currentAmmo to maxAmmo
        base.Start();
    }
}