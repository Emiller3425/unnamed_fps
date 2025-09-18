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
    public int maxAmmo = 30;
    public int currentAmmo;
    public int damage = 10;
    public int fireRate = 2;
    public FiringType firingType = FiringType.SEMIAUTO;
    public GameObject bulletPrefab;
    private InputAction shoot;
    private float reloadBuffer;
    void Awake()
    {
        shoot = InputSystem.actions.FindAction("Attack");
    }

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        shoot.started += context =>
        {
            if (context.interaction is TapInteraction && reloadBuffer == 0f)
            {
                ShootBullet();
                currentAmmo--;
            }
        };
    }

    void ShootBullet()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Shoot(transform.TransformDirection(Vector3.forward), 15f);
    }

    void Reload()
    {
        reloadBuffer = 2f;
        currentAmmo = maxAmmo;
    }

    void OnDestroy()
    {
        // do nothing
    }

}