using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MachineGun : FullAutoGun
{
    protected override void Awake()
    {
        base.Awake();
        magSize = 35;
        currentAmmo = entityStats.GetCurrentMachineGunAmmo();
        currentMag = magSize;
    }
    protected override void Start()
    {
        // Set default values for MachineGun
        damage = 5;
        maxReloadBuffer = 2.5f;
        maxFireRateBuffer = 0.05f;
        // sets currentAmmo to maxAmmo
        base.Start();
    }
}