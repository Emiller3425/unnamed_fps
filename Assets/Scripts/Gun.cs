using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.TextCore.Text;

/** 
TODO Add reload logic
**/


public enum FiringType
{
    SEMIAUTO,
    BURST,
    FULLAUTO
}

public class Gun : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo;
    public int damage = 10;
    public int fireRate = 2;
    public FiringType firingType = FiringType.SEMIAUTO;
    public GameObject bulletPrefab;
    private InputAction shoot;
    private InputAction reload;
    private float reloadBuffer;
    void Awake()
    {
        // define listeners
        shoot = InputSystem.actions.FindAction("Attack");
        reload = InputSystem.actions.FindAction("Reload");
    }

    void OnEnable()
    {
        // enable listeners
        shoot.Enable();
        reload.Enable();
        // subscribe
        shoot.started += OnShoot;
        reload.started += OnReload;
    }


    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // probably handle fully auto here
        Debug.Log(currentAmmo);
    }

    // attempt shoot on shoot action
    void OnShoot(InputAction.CallbackContext context)
    {
        if (currentAmmo > 0)
        {
            ShootBullet();
        }
        else
        {
            Reload();
        }
    }

    // attempt reload on reload action
    void OnReload(InputAction.CallbackContext context)
    {
        if (currentAmmo < maxAmmo)
            Reload();
    }

    // shoot logic
    void ShootBullet()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Shoot(transform.TransformDirection(Vector3.forward), 15f);
        currentAmmo--;
    }

    // reload logic
    void Reload()
    {
        Debug.Log("reload");
        reloadBuffer = 2f;
        currentAmmo = maxAmmo;
    }

    // disable InputSystem subscriptions
    void OnDisable()
    {
        // unsubscribe
        shoot.started -= OnShoot;
        reload.started -= OnReload;
        // disable listeners
        shoot.Disable();
        reload.Disable();
    }
}