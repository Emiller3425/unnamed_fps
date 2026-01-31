using System;
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
            Debug.Log(child.name);

            // Start with everything turned off
            child.gameObject.SetActive(false);
        }
    }

    // TODO: We need to add more flexible logic to the typing of guns, make it ewasier to determine if a gun is a pistol a rifle or an smg -- maybe use an interface?
    public void EquipWeapon(string weaponName)
    {
        if (weaponDictionary.ContainsKey(weaponName))
        {
            if (currentWeapon != null)
            {
                currentWeapon.SetActive(false);
            }
            currentWeapon = weaponDictionary[weaponName];
            currentWeapon.SetActive(true);
            if (currentWeapon.GetComponent<Pistol>())
            {
                GameEvents.current.AmmoChanged(currentWeapon.GetComponent<Gun>().currentMag, PlayerStatsManager.Instance.GetPistolAmmo());
            } else if (currentWeapon.GetComponent<MachineGun>())
            {
                GameEvents.current.AmmoChanged(currentWeapon.GetComponent<Gun>().currentMag, PlayerStatsManager.Instance.GetSMGAmmo());
            } else if (currentWeapon.GetComponent<BurstRifle>())
            {
                GameEvents.current.AmmoChanged(currentWeapon.GetComponent<Gun>().currentMag, PlayerStatsManager.Instance.GetRifleAmmo());
            }
        }
    }

}