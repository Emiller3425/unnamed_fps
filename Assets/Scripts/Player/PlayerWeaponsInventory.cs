using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerWeaponInventory : MonoBehaviour
{
    public Transform weaponHolder;
    public PlayerAnimationController animController;
    public GameObject currentWeapon;
    private Dictionary<string, GameObject> weaponDictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        PopulateInventory();
        EquipWeapon("Pistol"); 
    }

    private void PopulateInventory()
    {
        foreach (Transform child in weaponHolder)
        {
            weaponDictionary.Add(child.name, child.gameObject);

            // Start with everything turned off
            child.gameObject.SetActive(false);
        }
    }

    // TODO: We need to add more flexible logic to the typing of guns, make it ewasier to determine if a gun is a pistol a rifle or an smg -- maybe use an interface?
    public void EquipWeapon(string weaponName)
    {
        if (weaponDictionary.ContainsKey(weaponName))
        {
            if (currentWeapon == null) {
                CompleteWeaponSwitch(weaponName);
            } else
            {
                animController.TriggerWeaponSwap(() => CompleteWeaponSwitch(weaponName));
            }
        }
    }

    private void CompleteWeaponSwitch(string weaponName)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        currentWeapon = weaponDictionary[weaponName];
        currentWeapon.SetActive(true);

        Gun gunScript = currentWeapon.GetComponent<Gun>();

        if (gunScript != null && gunScript.weaponAnimationOverride != null)
        {
            animController.animator.runtimeAnimatorController = gunScript.weaponAnimationOverride;
        }

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        Gun gunScript = currentWeapon.GetComponent<Gun>();
        if (gunScript == null)
        {
            return;
        }

        int reserveAmmo = 0;
        if (gunScript.GetComponent<IUsesPistolAmmo>() is IUsesPistolAmmo) reserveAmmo = PlayerStatsManager.Instance.GetPistolAmmo();
        if (gunScript.GetComponent<IUsesSMGAmmo>() is IUsesSMGAmmo) reserveAmmo = PlayerStatsManager.Instance.GetSMGAmmo();
        if (gunScript.GetComponent<IUsesRifleAmmo>() is IUsesRifleAmmo) reserveAmmo = PlayerStatsManager.Instance.GetRifleAmmo();

        GameEvents.current.AmmoChanged(gunScript.currentMag, reserveAmmo);
    }
}