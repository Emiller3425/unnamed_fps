using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

// TODO: Add max size to player iventory

public class PlayerWeaponInventory : MonoBehaviour
{
    public Transform weaponHolder;
    public PlayerAnimationController animController;
    public GameObject equippedWeapon;
    public Dictionary<string, GameObject> weaponDictionary = new Dictionary<string, GameObject>();
    private InputAction dropAction;
    private Collider playerCollider;
    private PlayerStatsManager playerStatsManager;

    private void OnEnable()
    {
        GameEvents.current.OnWeaponPickup += PickupWeapon;
        GameEvents.current.OnEquipmentPrimed += DisableEquippedWeapon;
        GameEvents.current.OnEquipmentThrownComplete += EnableEquippedWeapon;
    }
    private void Awake()
    {
        playerCollider = GetComponent<Collider>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }
    private void Start()
    {
        PopulateInventory();
        EquipWeapon(weaponHolder.GetChild(0).name, false);

        dropAction = InputSystem.actions.FindAction("Drop");
        dropAction.Enable();
        dropAction.started += OnDrop;
    }

        public void EquipNextWeapon(bool wasDropped)
    {
        // Swap or drop weapon
        if (weaponDictionary.Count > 1 || (wasDropped && equippedWeapon != null)) {
            int currentIndex = weaponDictionary.Keys.ToList().IndexOf(equippedWeapon.name);
            int nextIndex = (currentIndex + 1) % weaponDictionary.Count;
            string nextWeaponName = weaponDictionary.Keys.ElementAt(nextIndex);

            EquipWeapon(nextWeaponName, wasDropped);
        }
    }

    public void EquipWeapon(string weaponName, bool wasDropped)
    {
        if (weaponDictionary.ContainsKey(weaponName))
        {
            if (equippedWeapon == null) {
                CompleteWeaponSwitch(weaponName, wasDropped);
            } else if (wasDropped)
            {
                CompleteWeaponSwitch(weaponName, wasDropped);
                animController.TriggerWeaponSwap();
            } 
            else
            {
                animController.TriggerWeaponSwap(() => CompleteWeaponSwitch(weaponName, wasDropped));
            }
        }
    }

    private void PopulateInventory()
    {
        foreach (Transform child in weaponHolder)
        {
            weaponDictionary.Add(child.name, child.gameObject);

            // Disable physics for all weapons in inventory
            child.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            child.gameObject.GetComponent<BoxCollider>().enabled = false;

            // Start with every weapon inactive
            child.gameObject.SetActive(false);
        }
    }

    private void AddToInventory(GameObject weapon)
    {
        // Add weapon to inventory
        weaponDictionary.Add(weapon.name, weapon);
        weapon.transform.SetParent(gameObject.transform, true);

        // Enable gun script physics components
        weapon.GetComponent<PlayerGun>().enabled = false;
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<BoxCollider>().enabled = false;

        SetWeaponPositionAndRotation(weapon.transform);

        weapon.SetActive(false);

        // Enable collision with player
        if (playerCollider != null)
        {
            Physics.IgnoreCollision(weapon.GetComponent<BoxCollider>(), playerCollider, false);
        }

        // If the newly added weapon is the only one in the inventory
        if (equippedWeapon == null && weaponDictionary.Count == 1)
        {
            EquipWeapon(weapon.name, false);
        }
    }

    private void RemoveFromInventory(GameObject weapon)
    {
        weaponDictionary.Remove(weapon.name);
    }
    private void CompleteWeaponSwitch(string weaponName, bool wasDropped)
    {
        if (equippedWeapon != null && !wasDropped)
        {
            equippedWeapon.SetActive(false);
        } else if (wasDropped)
        {
            // Remove weapons from inventory in the player heirarchy
            equippedWeapon.transform.SetParent(null, true);
            weaponDictionary.Remove(equippedWeapon.name, out equippedWeapon);

            // Disable gun script and enable physics components
            equippedWeapon.GetComponent<PlayerGun>().enabled = false;
            equippedWeapon.GetComponent<Rigidbody>().isKinematic = false;
            equippedWeapon.GetComponent<BoxCollider>().enabled = true;

            // Ignore collision with player on drop
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(equippedWeapon.GetComponent<BoxCollider>(), playerCollider, true);
            }

            // Apply throw force
            Vector3 throwForce = transform.forward + Vector3.up;
            equippedWeapon.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.Impulse);
           
           // Clear equippedWeapon
            equippedWeapon = null;
            if (weaponDictionary.Count == 0)
            {
                UpdateAmmoUI();
                return;
            }
        }

        // If there are no weapons equipped on switch, play the pickup animation
        if (equippedWeapon == null)
        {
            animController.TriggerWeaponPickup();
        }

        equippedWeapon = weaponDictionary[weaponName];
        equippedWeapon.SetActive(true);
        SetWeaponPositionAndRotation(equippedWeapon.transform);

        PlayerGun gunScript = equippedWeapon.GetComponent<PlayerGun>();
        gunScript.enabled = true;

        if (gunScript != null && gunScript.weaponAnimationOverride != null)
        {
            animController.animator.runtimeAnimatorController = gunScript.weaponAnimationOverride;
        }

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        PlayerGun gunScript = equippedWeapon?.GetComponent<PlayerGun>();
        if (gunScript == null)
        {
            // Negative value incdicates there is no equipped weapon
            GameEvents.current.AmmoChanged(-1, -1);
            return;
        }

        int reserveAmmo = 0;
        if (gunScript.GetComponent<IUsesPistolAmmo>() is IUsesPistolAmmo) reserveAmmo = PlayerStatsManager.Instance.GetPistolAmmo();
        if (gunScript.GetComponent<IUsesSMGAmmo>() is IUsesSMGAmmo) reserveAmmo = PlayerStatsManager.Instance.GetSMGAmmo();
        if (gunScript.GetComponent<IUsesRifleAmmo>() is IUsesRifleAmmo) reserveAmmo = PlayerStatsManager.Instance.GetRifleAmmo();
        if (gunScript.GetComponent<IUsesShotgunAmmo>() is IUsesShotgunAmmo) reserveAmmo = PlayerStatsManager.Instance.GetShotgunAmmo();

        GameEvents.current.AmmoChanged(gunScript.currentMag, reserveAmmo);
    }

    private void SetWeaponPositionAndRotation(Transform weaponTransform)
    {
        weaponTransform.position = transform.parent.position;
        weaponTransform.localPosition = Vector3.zero;
        weaponTransform.localRotation = Quaternion.identity;

        weaponTransform.localPosition -= weaponTransform.Find("GripAnchor").localPosition;
        weaponTransform.rotation = weaponTransform.Find("GripAnchor").rotation;
    }

    private void PickupWeapon(GameObject weapon)
    {
        Debug.Log($"{weaponDictionary.Count}, {PlayerStatsManager.Instance.GetMaxInventory()}");
        if (weaponDictionary.Count < PlayerStatsManager.Instance.GetMaxInventory())
        {
            AddToInventory(weapon);
        }
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        EquipNextWeapon(true);
    }

    private void DisableEquippedWeapon()
    {
        if (equippedWeapon)
            equippedWeapon.SetActive(false);
    }
    private void EnableEquippedWeapon()
    {
        if (equippedWeapon)
        equippedWeapon.SetActive(true);
    }

    private void OnDisable()
    {
        GameEvents.current.OnWeaponPickup -= PickupWeapon;
        GameEvents.current.OnEquipmentPrimed -= DisableEquippedWeapon;
        GameEvents.current.OnEquipmentThrownComplete -= EnableEquippedWeapon;
    }

    private void OnDestroy()
    {
        dropAction.started -= OnDrop;
        dropAction.Disable();
    }
}