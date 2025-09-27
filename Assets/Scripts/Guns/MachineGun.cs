using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MachineGun : FullAutoGun
{
    protected override void Start()
    {
        // Set default values for MachineGun
        maxAmmo = 175;
        magSize = 35;
        damage = 5;
        maxReloadBuffer = 2.5f;
        maxFireRateBuffer = 0.05f;
        bulletVelocity = 30f;
        flattenTrajectoryRange = 3f;
       // sets currentAmmo to maxAmmo
        base.Start();
    }
}