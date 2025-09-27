using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.TextCore.Text;

public abstract class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Camera playerCamera;
    public Crosshairs crosshairs;
    public int maxAmmo = 120;
    public int magSize = 30;
    public int damage = 10;
    public float maxReloadBuffer = 2f;
    public float maxFireRateBuffer = 0.2f;
    public float bulletVelocity = 30f;
    public float range = 100f;
    protected float reloadBuffer = 0f;
    protected float fireRateBuffer = 0f;
    protected int currentAmmo;
    protected int currentMag;
    protected InputAction shoot;
    protected InputAction reload;
    protected Vector3 muzzleLocation;
    protected void Awake()
    {
        // define listeners
        shoot = InputSystem.actions.FindAction("Attack");
        reload = InputSystem.actions.FindAction("Reload");
    }

    protected void OnEnable()
    {
        // enable listeners
        shoot.Enable();
        reload.Enable();
        // subscribe
        shoot.started += OnShoot;
        reload.started += OnReload;
    }

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
        currentMag = magSize;
        muzzleLocation = transform.Find("Muzzle").position;
    }

    protected virtual void Update()
    {
        // decriment reload and firerate buffers if they exist
        if (reloadBuffer > 0f)
        {
            reloadBuffer -= Time.deltaTime;
        }
        if (fireRateBuffer > 0f)
        {
            fireRateBuffer -= Time.deltaTime;
        }
        muzzleLocation = transform.Find("Muzzle").position;
    }

    protected abstract void OnShoot(InputAction.CallbackContext context);

    protected virtual void AttemptShoot()
    {
        if (currentMag > 0 && reloadBuffer <= 0f)
        {
            if (fireRateBuffer <= 0f)
                ShootBullet();
        }
        else
        {
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
            Vector3 rayDirection = calculateRay(out rayDirection);
            GameObject bulletObject = Instantiate(bulletPrefab, muzzleLocation, transform.rotation);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.Shoot(rayDirection, bulletVelocity);
            currentMag--;
            fireRateBuffer = maxFireRateBuffer;
        }
    }

    protected Vector3 calculateRay(out Vector3 rayDirection)
    {
        Vector3 crossHairScreenPosition = crosshairs.transform.position;

        Ray cameraRay = playerCamera.ScreenPointToRay(crossHairScreenPosition);

        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(cameraRay, out hit, range))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = cameraRay.GetPoint(range);
        }
        rayDirection = (targetPoint - muzzleLocation).normalized;
        
        return rayDirection;
    }

    // Reloads
    protected void Reload()
    {
        if (currentAmmo > 0 && currentMag < magSize && reloadBuffer <= 0f)
        {
            reloadBuffer = maxReloadBuffer;
            currentAmmo -= (magSize - currentMag);
            currentMag = magSize;
        }
    }

    // disable InputSystem subscriptions
    protected void OnDisable()
    {
        // unsubscribe and disable listeners
        shoot.started -= OnShoot;
        reload.started -= OnReload;
        shoot.Disable();
        reload.Disable();
    }
}