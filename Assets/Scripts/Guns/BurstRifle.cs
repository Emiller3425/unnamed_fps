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
        PlayerStatsManager.Instance.SetPistolAmmo(entityStats.GetCurrentPistolAmmo());
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
    protected override void Reload()
    {
        base.Reload();
        // Reload player mag if is less than max and we have ammo in reserves
        if (PlayerStatsManager.Instance.GetRifleAmmo() > 0 && currentMag < magSize && reloadBuffer <= 0f && isPlayerGun)
        {
            GameEvents.current.ReloadStarted();
            reloadBuffer = maxReloadBuffer;
            PlayerStatsManager.Instance.SetRifleAmmo(PlayerStatsManager.Instance.GetRifleAmmo() - (magSize - currentMag));
            if (PlayerStatsManager.Instance.GetRifleAmmo() < 0)
            {
                currentMag = magSize + PlayerStatsManager.Instance.GetRifleAmmo();
                PlayerStatsManager.Instance.SetRifleAmmo(0);
            } else
            {
               currentMag = magSize; 
            }
            GameEvents.current.PlaySFX("reload");
        }
        // Enemy reload infinite ammo
        else if (!isPlayerGun)
        {
            reloadBuffer = maxReloadBuffer;
            currentMag = magSize;
        }
    }
}