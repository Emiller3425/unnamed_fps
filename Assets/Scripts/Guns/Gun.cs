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
    public Crosshairs crosshairs;
    public HitMarker hitMarker;
    public EntityStats entityStats;
    public GameObject muzzleFlash;
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
    protected VisualEffect muzzleVFX;
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

        if (muzzleFlash != null)
        {
            muzzleVFX = muzzleFlash.GetComponent<VisualEffect>();
        }
    }

    protected virtual void Update()
    {
        if (AmmoUIManager.Instance?.ammoUIText == null && isPlayerGun)
        {
            AmmoUIManager.Instance.UpdateAmmoUI(currentMag, currentAmmo);
        }
        // decriment reload and firerate buffers if they exist 
        if (reloadBuffer > 0f)
        {
            reloadBuffer -= Time.deltaTime;
        }
        // Update Ammo UI only after reload is complete
        else if (isPlayerGun)
        {
            AmmoUIManager.Instance?.UpdateAmmoUI(currentMag, currentAmmo);
        }
        if (fireRateBuffer > 0f)
        {
            fireRateBuffer -= Time.deltaTime;
        }
        if (muzzleTransform != null && muzzleFlash != null)
        {
            muzzleFlash.transform.position = muzzleTransform.position;
            muzzleFlash.transform.rotation = muzzleTransform.rotation;
        }
    }

    protected abstract void OnShoot(InputAction.CallbackContext context);

    protected virtual void AttemptShoot()
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
        Console.WriteLine("Shot for some fucking reason");
        if (fireRateBuffer <= 0)
        {
            // get ray for bullet
            Vector3 rayDirection = CalculateRay();
            if (rayDirection == Vector3.zero)
            {
                Debug.Log("Miss");
            }
            else
            {
                Debug.Log("Hit");
            }
            // Handle Muzzle Flash
            if (muzzleVFX != null)
            {
                Debug.Log("Muzzle flash");
                muzzleVFX.Play();
            }
            currentMag--;
            fireRateBuffer = maxFireRateBuffer;
            AmmoUIManager.Instance.UpdateAmmoUI(currentMag, currentAmmo);
        } 
    }

    protected Vector3 CalculateRay()
    {
        Vector3 crossHairScreenPosition = crosshairs.transform.position;
        Ray cameraRay = playerCamera.ScreenPointToRay(crossHairScreenPosition);

        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, maxRange))
        {
            if (hit.collider.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
            {
                damageable.ApplyDamage(damage);
                if (isPlayerGun)
                {
                    // not awaited because hit marker is not used in anyrthing else within this fucntion call
                    hitMarker.ShowHitMarker();
                    FindAnyObjectByType<AudioManager>().Play("hitmarker");
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
            reloadBuffer = maxReloadBuffer;
            currentAmmo -= (magSize - currentMag);
            currentMag = magSize;
        }
        // Enemy reload infinite ammo
        else
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