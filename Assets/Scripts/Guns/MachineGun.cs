using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class MachineGun : FullAutoGun, IUsesSMGAmmo
{
    protected override void Awake()
    {
        base.Awake();
        magSize = 35;
        PlayerStatsManager.Instance.SetSMGAmmo(PlayerStatsManager.Instance.GetSMGAmmo());
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

    protected override void Update()
    {
        if (firstUpdate)
        {
            GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetSMGAmmo());
            firstUpdate = false;
        }
        if (reloadBuffer > 0f)
        {
            reloadBuffer -= Time.deltaTime;
            if (reloadBuffer <= 0f)
            {
                GameEvents.current.ReloadFinished();
                GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetSMGAmmo());
            }
        }
        base.Update();
    }

    protected override void Reload()
    {
        base.Reload();
        // Reload player mag if is less than max and we have ammo in reserves
        if (PlayerStatsManager.Instance.GetSMGAmmo() > 0 && currentMag < magSize && reloadBuffer <= 0f && isPlayerGun)
        {
            GameEvents.current.ReloadStarted();
            reloadBuffer = maxReloadBuffer;
            PlayerStatsManager.Instance.SetSMGAmmo(PlayerStatsManager.Instance.GetSMGAmmo() - (magSize - currentMag));
            if (PlayerStatsManager.Instance.GetSMGAmmo() < 0)
            {
                currentMag = magSize + PlayerStatsManager.Instance.GetSMGAmmo();
                PlayerStatsManager.Instance.SetSMGAmmo(0);
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
            GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetSMGAmmo());
        }
    }
}