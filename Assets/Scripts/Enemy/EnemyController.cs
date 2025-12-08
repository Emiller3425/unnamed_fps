using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    public GameObject bloodSplatter;
    public EnemyGun enemyGun;
    private Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0f;
    private float velocityY = 0f;
    private float gravity = 10f;
     
    private bool canJump = true;
    private CharacterController enemyController;
    private PlayerController playerTarget;
    private VisualEffect bloodVFX;

    void Start()
    {
        enemyController = GetComponent<CharacterController>();
        playerTarget = FindAnyObjectByType<PlayerController>();
        bloodVFX = bloodSplatter.GetComponent<VisualEffect>();
    }
    void Update()
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
                // 
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

    }

    public CharacterController GetController()
    {
        return enemyController;
    }

    public void PlayBloodSplatter(UnityEngine.RaycastHit hit)
    {
        bloodSplatter.transform.position = hit.point;
        bloodVFX.SetVector3("BloodVelocity", -hit.normal * 2);
        bloodVFX.Play();
    }
}