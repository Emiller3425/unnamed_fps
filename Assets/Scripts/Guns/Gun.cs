using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.TextCore.Text;


public enum FiringType
{
    SEMIAUTO,
    BURST,
    FULLAUTO
}

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int maxAmmo = 30;
    public int damage = 10;
    public float maxReloadBuffer = 2f;
    public float maxFireRateBuffer = 0.2f;
    public FiringType firingType = FiringType.SEMIAUTO;
    protected float reloadBuffer = 0f;
    protected float fireRateBuffer = 0f;
    protected int currentAmmo;
    protected float bulletVelocity = 20f;
    private InputAction shoot;
    private InputAction reload;
    protected void Awake()
    {
        // define listeners
        shoot = InputSystem.actions.FindAction("Attack");
        reload = InputSystem.actions.FindAction("Reload");
    }

    public virtual void OnEnable()
    {
        // enable listeners
        shoot.Enable();
        reload.Enable();
        // subscribe
        shoot.started += OnShoot;
        reload.started += OnReload;
    }

    protected void Start()
    {
        currentAmmo = maxAmmo;
    }

    protected void Update()
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
    }

    // attempt shoot on shoot action
    protected void OnShoot(InputAction.CallbackContext context)
    {
        Debug.Log("Attempt Shot;");
        if (firingType == FiringType.SEMIAUTO)
        {
            if (currentAmmo > 0 && reloadBuffer <= 0f)
            {
                if (fireRateBuffer <= 0f)
                    ShootBullet();
            }
            else
            {
                Reload();
            }
        }
        else if (firingType == FiringType.BURST)
        {
            // add burst logic
        }
        else if (firingType == FiringType.FULLAUTO)
        {
            // add full auto logic
        }
    }


    // Attempt reload on reload action
    protected void OnReload(InputAction.CallbackContext context)
    {
        if (currentAmmo < maxAmmo && reloadBuffer <= 0)
            Reload();
    }

    // Shoots bulllet
    public virtual void ShootBullet()
    {
        if (fireRateBuffer <= 0)
        {
            GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.Shoot(transform.TransformDirection(Vector3.forward), bulletVelocity);
            currentAmmo--;
            fireRateBuffer = maxFireRateBuffer;
        }
    }

    // Reloads
    protected void Reload()
    {
        if (currentAmmo < maxAmmo && reloadBuffer <= 0f)
        {
            reloadBuffer = maxReloadBuffer;
            currentAmmo = maxAmmo;
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