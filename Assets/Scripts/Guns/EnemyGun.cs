using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.VFX;

public abstract class EnemyGun : MonoBehaviour
{
    public EntityStats entityStats;
    public int magSize = 30;
    public int damage = 10;
    public int currentAmmo;
    public int currentMag;
    public float maxReloadBuffer = 2f;
    public float maxFireRateBuffer = 0.2f;
    public float maxRange = 100f;
    protected float reloadBuffer = 0f;
    protected float fireRateBuffer = 0f;
    protected int maxAmmo;
    protected Vector3 muzzleLocation;
    protected Transform muzzleTransform;

    protected virtual void Update()
    {
        // decriment reload and firerate buffers if they exist 
        if (reloadBuffer > 0f)
        {
            reloadBuffer -= Time.deltaTime;
        }
        // Update Ammo UI only after reload is complete
        if (fireRateBuffer > 0f)
        {
            fireRateBuffer -= Time.deltaTime;
        }
    }

    protected abstract void OnShoot(InputAction.CallbackContext context);

    public virtual void AttemptShoot()
    {
        // player shoot logic
        if (currentMag > 0 && reloadBuffer <= 0f)
        {
            if (fireRateBuffer <= 0f)
                ShootBullet();
            if (currentMag <= 0)
            {
                Reload();
            }
        }
        else {
            Reload();
        }
    }

    // Attempt reload on reload action
    protected void OnReload(InputAction.CallbackContext context)
    {
        if (currentMag < magSize && reloadBuffer <= 0)
            Reload();
    }

    // Shoots bullet
    protected void ShootBullet()
    {
        if (fireRateBuffer <= 0)
        {
            // get ray for bullet
            Vector3 rayDirection = CalculateRay();
            // Play Gunshot
            GameEvents.current.PlaySFX("gunshot");
            currentMag--;
            fireRateBuffer = maxFireRateBuffer;
        } 
    }

    // TODO: Fix the fucking calculate ray, we might want to use projectiles from enemies
    protected Vector3 CalculateRay()
    {
        Ray cameraRay = new Ray(muzzleTransform.position, muzzleTransform.forward);

        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, maxRange))
        {
            if (hit.collider.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
            {
                damageable.ApplyDamage(damage);
                EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
                enemyController.PlayBloodSplatter(hit);
            }
            return (hit.point - muzzleLocation).normalized;
        }
       else
        {
            // we did not hit anything
            return Vector3.zero;
        }
    }

    // Reloads
    protected void Reload()
    {
        // Already in the middle of a reload
        if (reloadBuffer > 0f)
            return;
        
            reloadBuffer = maxReloadBuffer;
            currentMag = magSize;
    }
}