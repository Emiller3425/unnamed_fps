using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.VFX;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemyController : EnemyController
{
    public event Action OnMelee;
    protected float attackCooldown;
    protected float maxAttackCooldown = 2f;

    protected override void Start()
    {
        base.Start();
        
        // There should be no attack cooldown when the melee enemy is instantiated
        attackCooldown = 0f;
    }
    protected override void Update()
    {
        base.Update();

        // Decrement melee cooldown
        if (attackCooldown >= 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }
    protected override void ProcessState()
    {
        switch (State) {
            case EnemyState.IDLE:
                navAgent.isStopped = true;
                break;
            case EnemyState.PATROL:
                break;
            case EnemyState.PURSUIT:
                navAgent.isStopped = false;
                navAgent.SetDestination(detectedTarget.transform.position);
                break;
            case EnemyState.ATTACKING:
                if (attackCooldown < 0f)
                {
                    navAgent.isStopped = true;
                    Attack();
                }
                break;
            default:
                break;
        }
    }
    protected override void Attack()
    {
        // This may or may not be needed, might have an EnemyMeleeWeapon class
        // similar to how we are doing the EnemyGun as its own class
        // OnMelee?.Invoke();
        Melee();
    }

    protected void Melee()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);

        foreach(Collider c in hitColliders)
        {
            if (c.IsPlayer())
            {
                if (c.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
                {
                    if (!c.GetComponentInParent<EntityStatsManager>().isDead) {
                        damageable.BulletDamage(damage, transform.position);
                    }
                }
            }
        }

        attackCooldown = maxAttackCooldown;
    }
}