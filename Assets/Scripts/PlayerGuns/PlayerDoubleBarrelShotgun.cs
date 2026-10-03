using UnityEngine;

public class PlayerDoubleBarrelShotgun : PlayerShotgun, IUsesShotgunAmmo
{
    protected override void Awake()
    {
        base.Awake();
        magSize = 2;
        currentMag = magSize;
    }

    protected override void OnEnable()
    {
        minimumCrosshairsWidth = 25f;
        base.OnEnable();
    }

    protected override void Start()
    {
        // Set default values for Double Barrel Shotgun
        damage = 10; // damage per pellet
        maxReloadBuffer = 1.5f;
        maxFireRateBuffer = 0.5f;
        PlayerStatsManager.Instance.SetShotgunAmmo(PlayerStatsManager.Instance.GetShotgunAmmo());
        // sets currentAmmo to maxAmmo
        base.Start();
    }

    protected override void Update()
    {
        if (firstUpdate)
        {
            GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetShotgunAmmo());
            firstUpdate = false;
        }
        if (reloadBuffer > 0f)
        {
            reloadBuffer -= Time.deltaTime;
            if (reloadBuffer <= 0f)
            {
                GameEvents.current.ReloadFinished();
                GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetShotgunAmmo());
            }
        }
        base.Update();
    }

    protected override void Reload()
    {
        base.Reload();
        // Reload player mag if is less than max and we have ammo in reserves
        if (PlayerStatsManager.Instance.GetShotgunAmmo() > 0 && currentMag < magSize && reloadBuffer <= 0f)
        {
            GameEvents.current.ReloadStarted();
            reloadBuffer = maxReloadBuffer;
            PlayerStatsManager.Instance.SetShotgunAmmo(PlayerStatsManager.Instance.GetShotgunAmmo() - (magSize - currentMag));
            if (PlayerStatsManager.Instance.GetShotgunAmmo() < 0)
            {
                currentMag = magSize + PlayerStatsManager.Instance.GetShotgunAmmo();
                PlayerStatsManager.Instance.SetShotgunAmmo(0);
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

        GameEvents.current.AmmoChanged(currentMag, PlayerStatsManager.Instance.GetShotgunAmmo());
    }
} 