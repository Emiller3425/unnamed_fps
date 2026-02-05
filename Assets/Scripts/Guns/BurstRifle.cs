using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class BurstRifle : BurstGun, IUsesRifleAmmo
{
    protected override void Awake()
    {
        base.Awake();
        magSize = 35;
        PlayerStatsManager.Instance.SetRifleAmmo(entityStats.GetCurrentRifleAmmo());
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

    protected override void Update()
    {
        if (firstUpdate)
        {
            GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetRifleAmmo());
            firstUpdate = false;
        }
        if (reloadBuffer > 0f)
        {
            reloadBuffer -= Time.deltaTime;
            if (reloadBuffer <= 0f)
            {
                GameEvents.current.ReloadFinished();
                GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetRifleAmmo());
            }
        }
        base.Update();
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
            GameEvents.current.WeaponReloaded();
        }
        // Enemy reload infinite ammo
        else if (!isPlayerGun)
        {
            reloadBuffer = maxReloadBuffer;
            currentMag = magSize;
        }
    }

    protected override void ShootBullet()
    {
        base.ShootBullet();
        if (isPlayerGun)
        {
            GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetRifleAmmo());
        }
    }
}