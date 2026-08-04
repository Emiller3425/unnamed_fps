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

// TODO: Implement a base EnemyController using nav meshes, create a new enemy prefab using the new model.
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
    protected float checkTimer;
    protected bool isRunningChecks = false;
    protected int parentInstanceId;
    protected EnemyState currentState;
    
    // Encapsulate State 
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
        checkTimer = maxCheckTimer;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        detectedObjects = new HashSet<GameObject>();
        animator.enabled = false;
        State = EnemyState.IDLE;

        parentInstanceId = gameObject.GetInstanceID();
    }
    protected void Update()
    {
        if (State != EnemyState.DEAD)
        {
            if (!isRunningChecks)
            {
                StartCoroutine(EnemyChecks());
            }
        }
    }

    protected IEnumerator EnemyChecks()
    {
        isRunningChecks = true;
        while (checkTimer >= 0)
        {
            if (State == EnemyState.DEAD)
            {
                Debug.Log("This Check");
                yield break;
            }
            checkTimer -= Time.deltaTime;
            yield return null;
        }
        CheckForDetectableObjects();
        RemoveDetectableObjects();
        HandleState();
        ProcessState();
        checkTimer = maxCheckTimer;
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
                navAgent.isStopped = true;
                break;
            default:
                break;
        }
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