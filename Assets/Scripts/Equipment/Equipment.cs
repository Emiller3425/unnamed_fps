using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

// TODO: Equipment cooldown
public enum EquipmentTypes
{
    NONE,
    GRENADE,
    MOLOTOV,
    SHOCKGRENADE,
}

public abstract class Equipment : Explosive, IInteractable
{
    [Header("Equipment Settings")]
    public EquipmentTypes equipmentType;
    public Crosshairs crosshairs;
    public bool isPlayerEquipment;
    protected InputAction throwAction;
    protected Rigidbody rigidBody;
    protected Collider meshCollider;
    public void HandleInteract()
    {
        GameEvents.current.EquipmentPickup(gameObject);
    }
    protected virtual void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody>();
        meshCollider = transform.GetComponent<Collider>();
        meshCollider.enabled = true;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        rigidBody.mass = 10f;
    }

    public override void Detonate()
    {
        base.Detonate();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
}