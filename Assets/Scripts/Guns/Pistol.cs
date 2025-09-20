using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Pistol : SemiAutoGun
{
    protected override void Start()
    {
        // Set default values for Pistol
        maxAmmo = 72;
        magSize = 12;
        damage = 10;
        maxReloadBuffer = 1.5f;
        maxFireRateBuffer = 0.2f;
        bulletVelocity = 40f;
       // sets currentAmmo to maxAmmo
        base.Start();
    }
}