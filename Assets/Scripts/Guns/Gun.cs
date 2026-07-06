using System.Data.Common;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public abstract class Gun : MonoBehaviour, IInteractable
{
    // public GameObject bulletPrefab;
    public Camera playerCamera;
    public EntityStats entityStats;
    public AnimatorOverrideController weaponAnimationOverride;
    public Crosshairs crosshairs;
    public bool isPlayerGun = false;
    public int magSize = 30;
    public int damage = 10;
    public int currentMag;
    public float maxReloadBuffer = 2f;
    public float maxFireRateBuffer = 0.2f;
    public float maxRange = 100f;
    public GameObject bulletHolePrefab;
    protected float reloadBuffer = 0f;
    protected float fireRateBuffer = 0f;
    protected int maxAmmo;
    protected InputAction shootAction;
    protected InputAction reloadAction;
    protected Vector3 muzzleLocation;
    protected Transform muzzleTransform;
    protected bool firstUpdate = true;
    protected Vector2 screenCenter;
    protected bool isPaused = false;
    protected Rigidbody rigidBody;
    protected BoxCollider boxCollider;
    protected GameObject GripAnchor;

    public void HandleInteract()
    {
        GameEvents.current.WeaponPickup(gameObject);
    }
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
    protected virtual void Awake()
    {
        // define listeners
        if (isPlayerGun)
        {
            shootAction = InputSystem.actions.FindAction("Attack");
            reloadAction = InputSystem.actions.FindAction("Reload");
        }
    }

    protected virtual void OnEnable()
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

        GameEvents.current.OnTogglePause += HandlePause;
        GameEvents.current.OnTogglePlayerInventory += HandlePlayerInventory;
        GameEvents.current.OnScreenResize += RecalculateScreenCenter;
    }

    protected virtual void Start()
    {
        muzzleTransform = transform.Find("Muzzle");
        screenCenter = new Vector2 (Screen.width / 2f, Screen.height / 2f);

        // Disable physics components
        rigidBody = transform.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        boxCollider = transform.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
    }

    protected virtual void Update()
    {
        // decriment reload and firerate buffers if they exist 
        if (fireRateBuffer > 0f)
        {
            fireRateBuffer -= Time.deltaTime;
        }
    }

    protected abstract void OnShoot(InputAction.CallbackContext context);

    // Attempt reload on reload action
    protected void OnReload(InputAction.CallbackContext context)
    {
        if (currentMag < magSize && reloadBuffer <= 0)
            Reload();
    }

    // Shoots bullet
    protected virtual void ShootBullet()
    {
        if (fireRateBuffer <= 0 && !isPaused)
        {
            // get ray for bullet
            Vector3 rayDirection = CalculateRay();
            // Bloom crosshairs
            GameEvents.current.Bloom(15f, true);
            // Play Gunshot
            GameEvents.current.PlaySFX("gunshot");
            // Handle Muzzle Flash
            GameEvents.current.PlayVFX("glockMuzzleFlash", muzzleTransform.position, muzzleTransform.rotation.eulerAngles, Vector3.zero, muzzleTransform);
            
            GameEvents.current.WeaponFired();

            currentMag--;
            fireRateBuffer = maxFireRateBuffer;
        } 
    }

    protected Vector3 CalculateRay()
    {
        // Calculate bloom
        Vector2 randomBloomOffset;
        if (crosshairs) {
            randomBloomOffset = Random.insideUnitCircle * crosshairs.currentBloomRadius;
        } else
        {
            randomBloomOffset = Vector2.zero;
        }
        Vector3 bloomArea = new Vector3(screenCenter.x + randomBloomOffset.x, screenCenter.y + randomBloomOffset.y, 0f);
        
        Ray cameraRay = playerCamera.ScreenPointToRay(bloomArea);

        // Debug.DrawRay(cameraRay.origin, cameraRay.direction * 100f, Color.red, 0f);

        RaycastHit hit;

        // Ignore equipped weapon from what the raycast can hit
        int layerMask = ~(1 << LayerMask.NameToLayer("EquippedWeapon"));

        if (Physics.Raycast(cameraRay, out hit, maxRange, layerMask))
        {
            if (hit.collider.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
            {
                if (!hit.collider.GetComponentInParent<StatsManager>().isDead) {
                    damageable.BulletDamage(damage);
                    GameEvents.current.PlayVFX("bloodSplatter", hit.point, Vector3.zero, hit.normal * 2, null);
                    if (isPlayerGun)
                    {
                        // not awaited because hit marker is not used in anything else within this fucntion call
                        GameEvents.current.SetHitMarkerActivated();
                        GameEvents.current.PlaySFX("hitmarker");
                    }
                }
            } else
            {
                SpawnBulletHole(hit);
            }
            return (hit.point - muzzleLocation).normalized;
        }
       else
        {
            // we did not hit anything
            return Vector3.zero;
        }
    }

    protected void SpawnBulletHole(RaycastHit hit)
    {
        GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(-hit.normal));

        bulletHole.transform.SetParent(hit.transform);

        Vector3 vfxRotation = Quaternion.FromToRotation(Vector3.up, hit.normal).eulerAngles;

        GameEvents.current.PlayVFX("bulletSurfaceHit", hit.point, vfxRotation, Vector3.zero, null);

        Destroy(bulletHole, 10f);
    }

    // Reloads
    protected virtual void Reload()
    {
        // Already in the middle of a reload
        if (reloadBuffer > 0f && !isPaused)
            return;
    }

    protected void HandlePause(bool isToggled)
    {
        isPaused = isToggled;
    }

    protected void HandlePlayerInventory(bool isToggled)
    {
        isPaused = isToggled;
    }

    protected void RecalculateScreenCenter()
    {
        screenCenter = new Vector2 (Screen.width / 2f, Screen.height / 2f);
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

        GameEvents.current.OnTogglePause -= HandlePause;
        GameEvents.current.OnTogglePlayerInventory -= HandlePlayerInventory;
        GameEvents.current.OnScreenResize -= RecalculateScreenCenter;
    }
}