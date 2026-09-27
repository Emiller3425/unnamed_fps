using System.Data.Common;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
public class PlayerShotgun : PlayerGun
{
    protected int pelletCount = 24;
    protected override void OnShoot(InputAction.CallbackContext context)
    {
        AttemptShoot();
    }

    protected override void ShootBullet()
    {
        if (fireRateBuffer <= 0 && !isPaused)
        {
            // Bitwise to ignore equipped weapon from what the raycast can hit
            int layerMask = ~(1 << LayerMask.NameToLayer("EquippedWeapon"));
            for (int i = 0; i < pelletCount; i++)
            {
            // get ray for bullet
            Vector3 rayDirection = CalculateRay(layerMask);
            }

            GameEvents.current.Bloom(15f, true);
            GameEvents.current.PlaySFX("gunshot");
            GameEvents.current.PlayVFX("glockMuzzleFlash", muzzleTransform.position, muzzleTransform.rotation.eulerAngles, Vector3.zero, muzzleTransform);
            GameEvents.current.SpawnLight(muzzleFlashLight, muzzleTransform.position, muzzleTransform.rotation.eulerAngles, 0.05f);

            // Handle animations and recoil
            GameEvents.current.WeaponFired();

            currentMag--;
            fireRateBuffer = maxFireRateBuffer;
        } 
    } 
}