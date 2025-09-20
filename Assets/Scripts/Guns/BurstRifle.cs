using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class BurstRifle : BurstGun
{
    protected override void Start()
    {
        // Set default values for BurstRifle
        maxAmmo = 180;
        magSize = 36;
        damage = 12;
        maxReloadBuffer = 2.5f;
        maxFireRateBuffer = 0.1f;
        bulletVelocity = 45f;
       // sets currentAmmo to maxAmmo
        base.Start();
    }
}