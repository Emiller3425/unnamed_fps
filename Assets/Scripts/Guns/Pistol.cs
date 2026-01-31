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
        PlayerStatsManager.Instance.SetPistolAmmo(PlayerStatsManager.Instance.GetPistolAmmo());
        currentMag = magSize;
    }

    protected override void Start()
    {
        // Set default values for Pistol
        damage = 10;
        maxReloadBuffer = 1.5f;
        maxFireRateBuffer = 0.2f;
        // sets currentAmmo to maxAmmo
        base.Start();
    }

    protected override void Update()
    {
        if (firstUpdate)
        {
            GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetPistolAmmo());
            firstUpdate = false;
        }
        if (reloadBuffer > 0f)
        {
            reloadBuffer -= Time.deltaTime;
            if (reloadBuffer <= 0f)
            {
                GameEvents.current.ReloadFinished();
                GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetPistolAmmo());
            }
        }
        base.Update();
    }

    protected override void Reload()
    {
        base.Reload();
        // Reload player mag if is less than max and we have ammo in reserves
        if (PlayerStatsManager.Instance.GetPistolAmmo() > 0 && currentMag < magSize && reloadBuffer <= 0f && isPlayerGun)
        {
            GameEvents.current.ReloadStarted();
            reloadBuffer = maxReloadBuffer;
            PlayerStatsManager.Instance.SetPistolAmmo(PlayerStatsManager.Instance.GetPistolAmmo() - (magSize - currentMag));
            if (PlayerStatsManager.Instance.GetPistolAmmo() < 0)
            {
                currentMag = magSize + PlayerStatsManager.Instance.GetPistolAmmo();
                PlayerStatsManager.Instance.SetPistolAmmo(0);
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

    protected override void ShootBullet()
    {
        base.ShootBullet();
        if (isPlayerGun)
        {
            GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetPistolAmmo());
        }
    }
} 