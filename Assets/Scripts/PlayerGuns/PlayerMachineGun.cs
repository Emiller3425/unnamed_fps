using UnityEngine;

public class PlayerMachineGun : PlayerFullAutoGun, IUsesSMGAmmo
{
    protected override void Awake()
    {
        base.Awake();
        magSize = 35;
        currentMag = magSize;
    }
    protected override void Start()
    {
        // Set default values for MachineGun
        damage = 25;
        maxReloadBuffer = 2.5f;
        maxFireRateBuffer = 0.05f;
        PlayerStatsManager.Instance.SetSMGAmmo(PlayerStatsManager.Instance.GetSMGAmmo());
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
        if (PlayerStatsManager.Instance.GetSMGAmmo() > 0 && currentMag < magSize && reloadBuffer <= 0f)
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
    }

    protected override void ShootBullet()
    {
        base.ShootBullet();

        GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetSMGAmmo());
    }
}