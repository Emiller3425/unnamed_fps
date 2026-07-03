using System;
using System.Collections;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum EquipmentTypes
{
    NONE,
    GRENADE,
    MOLOTOV,
    SHOCKGRENADE,
}

public abstract class Equipment : MonoBehaviour, IInteractable
{
    [Header("Equipment Settings")]
    public EquipmentTypes equipmentType;
    public Crosshairs crosshairs;
    public bool isPlayerEquipment;
    protected InputAction throwAction;
    protected bool isPaused;
    protected Rigidbody rigidBody;
    protected Collider meshCollider;
    protected float damage;
    protected float areaOfEffect;
    public void HandleInteract()
    {
        GameEvents.current.EquipmentPickup(gameObject);
    }

    protected virtual void OnEnable()
    {
        if (isPlayerEquipment)
        {
            throwAction.Enable();
        }
        GameEvents.current.OnTogglePause += HandlePause;
    }
    protected virtual void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody>();
        meshCollider = transform.GetComponent<Collider>();
        meshCollider.enabled = true;
    }

    protected virtual void Detonate()
    {
        Debug.Log("Detonate");
        Destroy(gameObject, 2.5f);
    }
    protected void HandlePause(bool isToggled)
    {
        isPaused = isToggled;
    }
    
    protected void OnDisable()
    {
        GameEvents.current.OnTogglePause -= HandlePause;
    }
    protected void OnDestroy()
    {
         
    }
}