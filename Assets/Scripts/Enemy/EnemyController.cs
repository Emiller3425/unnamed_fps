using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

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
    private Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0f;
    private float velocityY = 0f;
    private float gravity = 25f;
    private bool canMove = true;
    private bool canJump = true;
    private CharacterController enemyController;
    private PlayerController playerTarget;

    void Start()
    {
        enemyController = GetComponent<CharacterController>();
        playerTarget = FindAnyObjectByType<PlayerController>();
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
            Debug.Log(distanceToPlayer);

            enemyController.Move(forward * Time.deltaTime);
        }

        if (enemyController.isGrounded)
        {
            velocityY -= gravity * Time.deltaTime;
        }

        if (enemyController.isGrounded)
        {
            canJump = true;
        }
        movementDirection.y = velocityY;

    }
}