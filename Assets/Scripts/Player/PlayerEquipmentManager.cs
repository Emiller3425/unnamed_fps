using System.Runtime.CompilerServices;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerEquipmentManager: MonoBehaviour

{
    public EntityStats entityStats;
    public GameObject equippedEquipment;
    private InputAction throwAction;
    private GameObject primedEquipment;
    private int currentEquipment;
    private int maxEquipment;
    private float throwForce = 1f;
    private void Awake()
    {
        throwAction = InputSystem.actions.FindAction("UseEquipment");
    }

    private void Start()
    {
        PlayerStatsManager.Instance.SetEquipment(PlayerStatsManager.Instance.GetEquipment());
        GameEvents.current.EquipmentCountChanged(PlayerStatsManager.Instance.GetEquipment());
    }

    private void OnEnable()
    {
        throwAction.Enable();
        throwAction.started += OnThrow;
        throwAction.canceled += OnRelease;

    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        if (PlayerStatsManager.Instance.GetEquipment() > 0)
        {
            Prime();
        }
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        if (primedEquipment)
        {
            Release();
        }
    }

    private void Prime()
    {
        PlayerStatsManager.Instance.SetEquipment(PlayerStatsManager.Instance.GetEquipment() - 1);
        // Update UI
        GameEvents.current.EquipmentCountChanged(PlayerStatsManager.Instance.GetEquipment());

        GameObject equipment = Instantiate(equippedEquipment, transform.position, transform.rotation);
        equipment.transform.SetParent(gameObject.transform);
        Rigidbody equipmentRb = equipment.GetComponent<Rigidbody>();
        equipmentRb.isKinematic = true;

        if (equipmentRb != null)
        {
            primedEquipment = equipment;
        }
    }

    private void Release()
    {
        Rigidbody primedEquipmentRb = primedEquipment.GetComponent<Rigidbody>();
        Vector3 throwDirection = transform.forward + transform.up * 0.4f;

        primedEquipmentRb.isKinematic = false;
        primedEquipment.transform.SetParent(null);
        primedEquipmentRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        primedEquipment = null;
    }

    private void OnDisable()
    {
        throwAction.Disable();
        throwAction.started -= OnThrow;
        throwAction.canceled -= OnRelease;

    }
}