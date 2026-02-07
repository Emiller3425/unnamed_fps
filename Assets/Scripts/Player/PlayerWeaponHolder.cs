using System;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerWeaponHolder : MonoBehaviour
{
    public PlayerWeaponInventory inventory;
    protected InputAction swapWeapon;
    private void Awake()
    {
        swapWeapon = InputSystem.actions.FindAction("Swap");
    }

    private void OnEnable()
    {
        // enable listeners
        swapWeapon.Enable();
        // subscribe
        swapWeapon.started += OnSwap;
    }

    private void OnSwap(InputAction.CallbackContext context)
    {
        inventory.EquipNextWeapon();
    }
}