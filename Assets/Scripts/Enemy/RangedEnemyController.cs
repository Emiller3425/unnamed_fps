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
public class RangedEnemyController : EnemyController
{
    protected override float detectDistance => 30f;
    protected override float attackRange => 15f;
    public event Action OnShoot;
    protected override void Start()
    {
        base.Start();
        damage = 20f;
    }
    protected override void Attack()
    {
        OnShoot?.Invoke();
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
                Attack();
                navAgent.SetDestination(detectedTarget.transform.position);
                break;
            default:
                break;
        }
    }
}