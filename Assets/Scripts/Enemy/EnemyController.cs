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

public enum EnemyState
{
    IDLE,
    PATROL, 
    ATTACKING,
    RELOADING,
    PURSUIT,
    DEAD
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public float walkSpeed = 0.5f;
    public float sprintSpeed = 7f;
    public float lookSpeed = 10f;
    public EnemyGun enemyGun;
    public NavMeshAgent navAgent;
    public Animator animator;
    protected float detectDistance = 10f;
    protected float detectArcDegrees = 120f;
    protected float attackRange = 3f;
    protected float pursuitRange = 30f;
    protected HashSet<GameObject> detectedObjects;
    protected GameObject detectedTarget;
    protected float maxCheckTimer = 0.5f;
    protected float currentCheckTimer;
    protected bool isRunningChecks = false;
    protected int parentInstanceId;
    protected float attackCooldown;
    protected float maxAttackCooldown = 2f;
    protected float damage = 10f;
    protected EnemyState currentState;

    // Encapsulate state 
    public EnemyState State
    {
        get => currentState;
        protected set
        {
            if (currentState == EnemyState.DEAD) return;
            currentState = value;
        }
    }

    protected virtual void OnEnable()
    {
        GameEvents.current.OnEntityDeath += SetDeadState;
    }
    protected void Start()
    {
        currentCheckTimer = maxCheckTimer;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        detectedObjects = new HashSet<GameObject>();
        animator.enabled = false;
        State = EnemyState.IDLE;

        parentInstanceId = gameObject.GetInstanceID();
        attackCooldown = 0f;
    }
    protected void Update()
    {
        if (State != EnemyState.DEAD)
        {
            if (!isRunningChecks)
            {
                StartCoroutine(EnemyChecksRoutine());
            }
        }
        if (attackCooldown >= 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    protected IEnumerator EnemyChecksRoutine()
    {
        isRunningChecks = true;
        while (currentCheckTimer >= 0)
        {
            // If enemy is dead we should not be updating state
            if (State == EnemyState.DEAD)
            {
                yield break;
            }
            currentCheckTimer -= Time.deltaTime;
            yield return null;
        }
        // Run detection checks and update state
        CheckForDetectableObjects();
        RemoveDetectableObjects();
        HandleState();
        ProcessState();
        currentCheckTimer = maxCheckTimer;
        isRunningChecks = false;
    }
    protected void CheckForDetectableObjects()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectDistance);

        foreach (Collider c in detectedColliders) {
            // Must be a detectable for enemy to target
            if (c.GetComponent<IDetectable>() is IDetectable detectable)
            {
                Vector3 targetPosition = c.transform.position;
                Vector3 directionToTarget = targetPosition - transform.position;

                // Check if detectable is within the enemies look radius
                if (Vector3.Angle(transform.forward, directionToTarget) <= detectArcDegrees / 2f)
                {
                    float distanceToTarget = directionToTarget.magnitude;

                    detectedObjects.Add(c.gameObject);
                }
            }
        }
    }

    protected void RemoveDetectableObjects()
    {
        // If detected objects are far enough away remove them from the list of detected objects
        detectedObjects.RemoveWhere(detected => 
            detected == null || Vector3.Distance(transform.position, detected.transform.position) > detectDistance
        );
        // Clear target
        if (detectedObjects.Count == 0)
        {
            detectedTarget = null;
        }
    }

    protected void HandleState()
    {
        // HANDLE IDLE / PATROL
        if (State == EnemyState.IDLE || State == EnemyState.PATROL) {
        if (detectedObjects.Count > 0)
        {
            if (!detectedTarget)
            {
                detectedTarget = detectedObjects.First();
                State = EnemyState.PURSUIT;
                return;
            }
        }
        }

        // HANDLE PURSUIT
        if (State == EnemyState.PURSUIT)
        {
            if (detectedTarget)
            {
                if (Vector3.Distance(transform.position, detectedTarget.transform.position) < attackRange)
                {
                    currentState = EnemyState.ATTACKING;
                    return;
                }
            } else
            {
                State = EnemyState.IDLE;
                return;
            }
        }

        // HANDLE ATTACKING
        if (State == EnemyState.ATTACKING)
        {
            if (detectedTarget)
            {
                if (Vector3.Distance(transform.position, detectedTarget.transform.position) > attackRange)
                {
                    State = EnemyState.PURSUIT;
                }
            } else
            {
                State = EnemyState.IDLE;
                return;
            }
        }
    }

    protected void ProcessState()
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

    protected void Attack()
    {
        Debug.Log("Enemy Attack");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);

        foreach(Collider c in hitColliders)
        {
            if (c.IsPlayer())
            {
                if (c.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
                {
                    if (!c.GetComponentInParent<StatsManager>().isDead) {
                        damageable.BulletDamage(damage, transform.position);
                    }
                }
            }
        }

        attackCooldown = maxAttackCooldown;
    }

    protected void SetDeadState(int instanceId)
    {
        if (instanceId == parentInstanceId)
        {
            State = EnemyState.DEAD;
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
        }
    }

    protected virtual void OnDisable()
    {
        GameEvents.current.OnEntityDeath -= SetDeadState;
    }

    protected void OnDrawGizmos()
    {
        
    }
}