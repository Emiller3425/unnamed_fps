using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Explosive : MonoBehaviour
{
    [Header("Equipment Settings")]
    protected bool isPaused;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float areaOfEffect;
    [SerializeField]
    protected float dentonateForce;

    protected virtual void OnEnable()
    {
        GameEvents.current.OnTogglePause += HandlePause;
    }
    public virtual void Detonate()
    {
        PlaySFX();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaOfEffect);

        // Hashset to ensure we only damage an enemy once
        HashSet<int> damagedParentInstanceIds = new HashSet<int>();
        foreach (Collider c in hitColliders)
        {
            if (c.GetComponentInParent<IDamageable>() is not null)
            {
                int damagedParentInstanceId = c.GetComponentInParent<StatsManager>().GetInstanceID();

                if (damagedParentInstanceIds.Contains(damagedParentInstanceId)) continue;

                damagedParentInstanceIds.Add(damagedParentInstanceId);
                if (c.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
                {
                    if (c.GetComponentInParent<EntityStatsManager>()) {
                        if (!c.GetComponentInParent<EntityStatsManager>().isDead) {
                            damageable.ExplosiveDamage(damage, transform.position, areaOfEffect, dentonateForce);

                            Vector3 closestPoint = c.ClosestPoint(transform.position);
                            Vector3 normal = (closestPoint - transform.position).normalized;
                            Quaternion rotation = Quaternion.LookRotation(normal);
                            
                            GameEvents.current.PlayParticleSystem("blood", closestPoint, rotation);
                        }
                    } else if (c.GetComponentInParent<EnvironmentObjectStatsManager>())
                    {
                        damageable.ExplosiveDamage(damage, transform.position, areaOfEffect, dentonateForce);
                    }
                }
            } else if (c.attachedRigidbody != null)
            {
                c.attachedRigidbody.AddExplosionForce(dentonateForce, transform.position, areaOfEffect);
            }
        }
        Destroy(gameObject);
    }
    protected virtual void PlaySFX()
    {
        GameEvents.current.PlayVFX("grenadeExplosion", transform.position, Vector3.zero, Vector3.zero, null);
        GameEvents.current.PlaySFX("explosion");
    }
    protected void HandlePause(bool isToggled)
    {
        isPaused = isToggled;
    }
    
    protected void OnDisable()
    {
        GameEvents.current.OnTogglePause -= HandlePause;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
}