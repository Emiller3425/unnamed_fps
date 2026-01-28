using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerWeaponHolder : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject equippedWeapon;
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

    private void Start()
    {
        GameObject gunInstance = Instantiate(equippedWeapon, transform.position, transform.rotation);
        gunInstance.transform.SetParent(this.transform);
        Gun gunScript = gunInstance.GetComponent<Gun>();

        if (gunScript)
        {
            gunScript.playerCamera = playerCamera;
            // gunScript.animator = 
            // gunScript.entityStats = 
        }
    }

    private void OnSwap(InputAction.CallbackContext context)
    {
        // implement weapon swap
        Debug.Log("swap initiated");
    }
}