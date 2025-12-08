using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;


public class BurstGun : Gun
{
    public int burstRounds = 3;
    private int burstRoundsLeft = 0;
    protected override void Update()
    {
        base.Update();
        if (burstRoundsLeft > 0)
        {
            AttemptShoot();
        }
    }
    protected override void OnShoot(InputAction.CallbackContext context)
    {
        if (burstRoundsLeft <= 0 && reloadBuffer <= 0)
        {
            if (currentAmmo > 0)
            {
                burstRoundsLeft = burstRounds;
            }
            AttemptShoot();
        }
    }

    public override void AttemptShoot()
    {
        if (currentAmmo > 0 && reloadBuffer <= 0f)
        {
            if (fireRateBuffer <= 0f)
            {
                ShootBullet();
                burstRoundsLeft--;
            }
        }
            else
            {
                burstRoundsLeft = 0;
                Reload();
            }
    }
}