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
    private int currentEquipment;
    private int maxEquipment;
    private float throwForce = 8f;
    private void Awake()
    {
        throwAction = InputSystem.actions.FindAction("UseEquipment");
    }

    private void Start()
    {
        PlayerStatsManager.Instance.SetEquipment(PlayerStatsManager.Instance.GetEquipment());
    }

    private void OnEnable()
    {
        throwAction.Enable();
        throwAction.started += OnThrow;

    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        if (PlayerStatsManager.Instance.GetEquipment() > 0)
            Throw();
    }

    private void Throw()
    {
        PlayerStatsManager.Instance.SetEquipment(PlayerStatsManager.Instance.GetEquipment() - 1);

        GameObject thrownEquipment = Instantiate(equippedEquipment, transform.position, transform.rotation);

        Rigidbody thrownEquipmentRb = thrownEquipment.GetComponent<Rigidbody>();

        if (thrownEquipmentRb != null)
        {
            Vector3 throwDirection = transform.forward + transform.up * 0.8f;

            thrownEquipmentRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
    }
}