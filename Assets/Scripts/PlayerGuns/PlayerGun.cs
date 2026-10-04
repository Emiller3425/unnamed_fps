using System.Data.Common;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public abstract class PlayerGun : MonoBehaviour, IInteractable
{
    // public GameObject bulletPrefab;
    public Camera playerCamera;
    public EntityStats entityStats;
    public AnimatorOverrideController weaponAnimationOverride;
    public Crosshairs crosshairs;
    public GameObject muzzleFlashLight;
    public int magSize = 30;
    public int damage = 10;
    public int currentMag;
    public float maxReloadBuffer = 2f;
    public float maxFireRateBuffer = 0.2f;
    public float maxRange = 100f;
    public GameObject bulletHolePrefab;
    protected float reloadBuffer = 0f;
    protected float fireRateBuffer = 0f;
    protected float minimumCrosshairsWidth = 3f;
    protected int maxAmmo;
    protected InputAction shootAction;
    protected InputAction reloadAction;
    protected Transform muzzleTransform;
    protected bool firstUpdate = true;
    protected Vector2 screenCenter;
    protected bool isPaused = false;
    protected Rigidbody rigidBody;
    protected BoxCollider boxCollider;
    protected GameObject gripAnchor;

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
        shootAction = InputSystem.actions.FindAction("Attack");
        reloadAction = InputSystem.actions.FindAction("Reload");
    }

    protected virtual void OnEnable()
    {
        shootAction.Enable();
        reloadAction.Enable();

        shootAction.started += OnShoot;
        reloadAction.started += OnReload;

        GameEvents.current.OnTogglePause += HandlePause;
        GameEvents.current.OnTogglePlayerInventory += HandlePlayerInventory;
        GameEvents.current.OnScreenResize += RecalculateScreenCenter;

        GameEvents.current.ToggleMinimumCrosshairsWidth(minimumCrosshairsWidth);
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
            // Bitwise to ignore equipped weapon from what the raycast can hit
            int layerMask = ~(1 << LayerMask.NameToLayer("EquippedWeapon"));
            // get ray for bullet
            Vector3 rayDirection = CalculateRay(layerMask);

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

// TODO: Damage falloff
    protected virtual Vector3 CalculateRay(int layerMask)
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

        if (Physics.Raycast(cameraRay, out hit, maxRange, layerMask))
        {
            if (hit.collider.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
            {
                if (hit.collider.GetComponentInParent<EntityStatsManager>()) {
                    if (!hit.collider.GetComponentInParent<EntityStatsManager>().isDead) {
                        damageable.BulletDamage(damage, -hit.normal);
                        GameEvents.current.PlayParticleSystem("blood", hit.point, Quaternion.LookRotation(hit.normal));

                        // not awaited because hit marker is not used in anything else within this fucntion call
                        GameEvents.current.SetHitMarkerActivated();
                        GameEvents.current.PlaySFX("hitmarker");
                    }
                } else if (hit.collider.GetComponentInParent<EnvironmentObjectStatsManager>())
                {
                    damageable.BulletDamage(damage, -hit.normal);
                }
            } else
            {
                SpawnBulletHole(hit, cameraRay);
            }
            return (hit.point - muzzleTransform.position).normalized;
        } else
        {
            // we did not hit anything
            return Vector3.zero;
        }
    }

    protected void SpawnBulletHole(RaycastHit hit, Ray cameraRay)
    {
        // Orientate bullet in direction it was shot
        Vector3 bulletDirection = cameraRay.direction;
        Vector3 surfaceProjectionDirection = Vector3.ProjectOnPlane(bulletDirection, hit.normal);
        Quaternion hitOrientation = Quaternion.LookRotation(-hit.normal, surfaceProjectionDirection);

        // Spanw bullet hole based on this position and orientation
        GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, hitOrientation);

        bulletHole.transform.SetParent(hit.transform);

        // VFX
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

        shootAction.started -= OnShoot;
        reloadAction.started -= OnReload;
        shootAction.Disable();
        reloadAction.Disable();

        GameEvents.current.OnTogglePause -= HandlePause;
        GameEvents.current.OnTogglePlayerInventory -= HandlePlayerInventory;
        GameEvents.current.OnScreenResize -= RecalculateScreenCenter;
    }
}