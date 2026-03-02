using UnityEngine;
using UnityEngine.InputSystem;

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