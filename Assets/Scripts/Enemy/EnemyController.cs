using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.VFX;

public enum EnemyState
{
    IDLE,
    PATROL, 
    ATTACKING,
    RELOADING
}

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    public float walkSpeed = 0.5f;
    public float sprintSpeed = 7f;
    public float lookSpeed = 10f;
    public EnemyGun enemyGun;
    private Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0f;
    private float velocityY = 0f;
    private float gravity = 10f;
    private bool canJump = true;
    private CharacterController enemyController;
    private PlayerController playerTarget;
    private VisualEffect bloodVFX;
    private float maxMeleeCooldown = 5f;
    private float currentMeleeCooldown = 0f;
    private float meleeRange = 10f;
    private float meleeDamage = 5f;

    private void Start()
    {
        enemyController = GetComponent<CharacterController>();
        playerTarget = FindAnyObjectByType<PlayerController>();
    }
    private void Update()
    {
        // Get directional vectors
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        forward.y = 0f;

        if (playerTarget != null)
        {
            Vector3 directionToTarget = playerTarget.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
            float distanceToPlayer = Vector3.Distance(transform.position, FindAnyObjectByType<PlayerController>().transform.position);
            if (distanceToPlayer < 100f)
            {
                AttemptMelee();
            }

            enemyController.Move(forward * Time.deltaTime);
        }

        velocityY -= gravity * Time.deltaTime;

        if (enemyController.isGrounded)
        {
            canJump = true;
        }
        movementDirection.y = velocityY;

        enemyController.Move(movementDirection.y * Vector3.up * Time.deltaTime);

        if (currentMeleeCooldown > 0f)
        {
            currentMeleeCooldown -= Time.deltaTime;
        }

    }

    public CharacterController GetController()
    {
        return enemyController;
    }

    public void PlayBloodSplatter(UnityEngine.RaycastHit hit)
    {
        GameEvents.current.PlayVFX("bloodSplatter", hit.point, Vector3.zero, hit.normal * 2, null);
    }

    // TODO: THIS IS A TEMP FUNCTION AT THE MOMENT - fix it idk

    private void AttemptMelee()
    {
    if (currentMeleeCooldown <= 0f)
    {
        // 1. Define the Ray (Origin and Direction)
        Vector3 rayOrigin = transform.position + Vector3.up; // Start at chest height
        Vector3 rayDirection = transform.forward;

        // 2. Fire the Raycast
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, meleeRange))
        {
            // 3. Check if the object hit has the IDamageable interface
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                // 4. Apply damage and trigger effects
                damageable.HealthSubtracted(meleeDamage);
                Debug.Log("Enemy Hit");
                PlayBloodSplatter(hit); 
            }
        }

        // Reset cooldown
        currentMeleeCooldown = maxMeleeCooldown;
    }
    else
    {
        // Reduce cooldown over time (usually done in Update, but can be managed here)
        currentMeleeCooldown -= Time.deltaTime;
    }
}
}