using System;
using System.Collections;
using TreeEditor;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
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
    protected float dentonateForce;
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
        GameEvents.current.PlayVFX("grenadeExplosion", transform.position, Vector3.zero, Vector3.zero, null);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaOfEffect);
        foreach (Collider c in hitColliders)
        {
            // TODO: Use a hashmap to apply apply damage to one body part of an enemy per explosion
            if (c.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
            {
                if (!c.GetComponentInParent<StatsManager>().isDead) {
                    damageable.ExplosiveDamage(damage, transform.position, areaOfEffect, dentonateForce);
                }
               // Make it possible to play blood splatter vfx here
            }
        }
        Destroy(gameObject);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
}