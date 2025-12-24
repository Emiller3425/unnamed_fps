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


public abstract class Gun : MonoBehaviour
{
    // public GameObject bulletPrefab;
    public Camera playerCamera;
    public EntityStats entityStats;
    public Animator animator;
    public bool isPlayerGun = false;
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
    protected InputAction shootAction;
    protected InputAction reloadAction;
    protected Vector3 muzzleLocation;
    protected Transform muzzleTransform;
    protected bool firstUpdate = true;
    protected Vector2 screenCenter;
    protected virtual void Awake()
    {
        // define listeners
        if (isPlayerGun)
        {
            shootAction = InputSystem.actions.FindAction("Attack");
            reloadAction = InputSystem.actions.FindAction("Reload");
        }
    }

    protected void OnEnable()
    {
        if (isPlayerGun)
        {
            // enable listeners
            shootAction.Enable();
            reloadAction.Enable();
            // subscribe
            shootAction.started += OnShoot;
            reloadAction.started += OnReload;
        }
    }

    protected virtual void Start()
    {
        muzzleTransform = transform.Find("Muzzle");
        screenCenter = new Vector2 (Screen.width / 2f, Screen.height / 2f);
    }

    protected virtual void Update()
    {
        if (firstUpdate)
        {
            GameEvents.current.AmmoChanged(currentMag, currentAmmo);
            firstUpdate = false;
        }
        // decriment reload and firerate buffers if they exist 
        if (reloadBuffer > 0f)
        {
            reloadBuffer -= Time.deltaTime;
            if (reloadBuffer <= 0f)
            {
                GameEvents.current.ReloadFinished();
                GameEvents.current.AmmoChanged(currentMag, currentAmmo);
            }
        }
        if (fireRateBuffer > 0f)
        {
            fireRateBuffer -= Time.deltaTime;
        }
        // GameEvents.current.SetCrossHairPosition(screenCenter);
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
            // Handle Muzzle Flash
            GameEvents.current.PlayVFX("glockMuzzleFlash", muzzleTransform.position, Vector3.zero, muzzleTransform);
            if (animator != null)
            {
                animator.SetTrigger("Shoot");
            }
            currentMag--;
            fireRateBuffer = maxFireRateBuffer;
            if (isPlayerGun)
            {
                GameEvents.current.AmmoChanged(currentMag, currentAmmo);
            }
        } 
    }

    protected Vector3 CalculateRay()
    {
        Ray cameraRay = playerCamera.ScreenPointToRay(screenCenter);

        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 100f, Color.red, 0f);

        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, maxRange))
        {
            if (hit.collider.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
            {
                damageable.HealthSubtracted(damage);
                EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
                enemyController.PlayBloodSplatter(hit);
                if (isPlayerGun)
                {
                    // not awaited because hit marker is not used in anything else within this fucntion call
                    GameEvents.current.SetHitMarkerActivated();
                    GameEvents.current.PlaySFX("hitmarker");
                }
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
        
        // Reload player mag if is less than max and we have ammo in reserves
        if (currentAmmo > 0 && currentMag < magSize && reloadBuffer <= 0f && isPlayerGun)
        {
            GameEvents.current.ReloadStarted();
            reloadBuffer = maxReloadBuffer;
            currentAmmo -= (magSize - currentMag);
            if (currentAmmo < 0)
            {
                currentMag = magSize + currentAmmo;
                currentAmmo = 0;
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

    // disable InputSystem subscriptions
    protected void OnDisable()
    {
        // unsubscribe and disable listeners
        if (isPlayerGun)
        {
            shootAction.started -= OnShoot;
            reloadAction.started -= OnReload;
            shootAction.Disable();
            reloadAction.Disable();
        }
    }
}