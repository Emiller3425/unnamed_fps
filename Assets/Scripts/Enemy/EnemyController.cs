using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


// TODO: We need to have melee types and ranged type enemy controllers:
// I think the attack should reference different functions one being shoot and the other melee
// ranged enemy should require an enemyGun refrence, melee type should not require a weapon 
// but can have one, ranged enemy should be able to melee.

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
    public NavMeshAgent navAgent;
    public Animator animator;
    public event Action OnAttack;
    public event Action OnDeath;
    protected virtual float detectDistance => 10f;
    protected float detectArcDegrees = 120f;
    protected virtual float attackRange => 3f;
    protected float pursuitRange = 200f;
    protected HashSet<GameObject> detectedObjects;
    protected GameObject detectedTarget;
    protected float maxCheckTimer = 0.5f;
    protected float currentCheckTimer;
    protected bool isRunningChecks = false;
    protected int parentInstanceId;
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
    protected virtual void Start()
    {
        currentCheckTimer = maxCheckTimer;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        detectedObjects = new HashSet<GameObject>();
        animator.enabled = false;
        State = EnemyState.IDLE;

        parentInstanceId = gameObject.GetInstanceID();
    }
    protected virtual void Update()
    {
        if (State != EnemyState.DEAD)
        {
            if (!isRunningChecks)
            {
                StartCoroutine(EnemyChecksRoutine());
            }
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
            detected == null || Vector3.Distance(transform.position, detected.transform.position) > pursuitRange
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
                float targetDistance = Vector3.Distance(transform.position, detectedTarget.transform.position);
                if (targetDistance < attackRange)
                {
                    currentState = EnemyState.ATTACKING;
                    return;
                } else if (targetDistance < pursuitRange)
                {
                    return;
                }
            } 
            State = EnemyState.IDLE;
            return;
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

    protected virtual void ProcessState()
    {
        // Empty because each inheritor should have it's own implementation
    }

    protected virtual void Attack()
    {
        OnAttack?.Invoke();
    }

    protected void SetDeadState(int instanceId)
    {
        if (instanceId == parentInstanceId)
        {
            State = EnemyState.DEAD;
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            OnDeath?.Invoke();
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