using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using UnityEditor.MPE;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.VFX;

public enum EnemyState
{
    IDLE,
    PATROL, 
    ATTACKING,
    RELOADING,
    PURSUIT
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
    private float detectDistance = 7f;
    private float detectArcDegrees = 60f;
    private float attackRange = 3f;
    private float pursuitRange = 15f;
    private HashSet<GameObject> detectedObjects;
    private GameObject detectedTarget;
    private EnemyState state;
    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        detectedObjects = new HashSet<GameObject>();
        animator.enabled = false;
        state = EnemyState.IDLE;
    }
    private void Update()
    {
        // TODO: Change these calls so they only update every 10-15 frames or so
        CheckForDetectableObjects();
        RemoveDetectableObjects();
        HandleState();
        ProcessState();
    }
    /// <summary>
    /// 1. sphere check
    /// 2. filter for angle
    /// 3. raycast if there are detectables
    /// </summary>
    private void CheckForDetectableObjects()
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

    private void RemoveDetectableObjects()
    {
        // If detected objects are far enough away remove them from the lsit of detected objects
        detectedObjects.RemoveWhere(detected => 
            detected == null || Vector3.Distance(transform.position, detected.transform.position) > detectDistance
        );
        // Clear target
        if (detectedObjects.Count == 0)
        {
            detectedTarget = null;
        }
    }

    private void HandleState()
    {
        // HANDLE IDLE / PATROL
        if (state == EnemyState.IDLE || state == EnemyState.PATROL) {
        if (detectedObjects.Count > 0)
        {
            if (!detectedTarget)
            {
                detectedTarget = detectedObjects.First();
                state = EnemyState.PURSUIT;
                return;
            }
        }
        }

        // HANDLE PURSUIT
        if (state == EnemyState.PURSUIT)
        {
            if (detectedTarget)
            {
                if (Vector3.Distance(transform.position, detectedTarget.transform.position) < attackRange)
                {
                    state = EnemyState.ATTACKING;
                    return;
                }
            } else
            {
                state = EnemyState.IDLE;
                return;
            }
        }

        // HANDLE ATTACKING
        if (state == EnemyState.ATTACKING)
        {
            if (detectedTarget)
            {
                if (Vector3.Distance(transform.position, detectedTarget.transform.position) > attackRange)
                {
                    state = EnemyState.PURSUIT;
                }
            } else
            {
                state = EnemyState.IDLE;
                return;
            }
        }
    }

    private void ProcessState()
    {
        switch (state) {
            case EnemyState.IDLE:
                navAgent.isStopped = true;
                Debug.Log("IDLE");
                break;
            case EnemyState.PATROL:
                break;
            case EnemyState.PURSUIT:
                navAgent.isStopped = false;
                navAgent.SetDestination(detectedTarget.transform.position);
                Debug.Log("PURSUIT");
                break;
            case EnemyState.ATTACKING:
                navAgent.isStopped = true;
                Debug.Log("ATTACKING");
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        
    }
}