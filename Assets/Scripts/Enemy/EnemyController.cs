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
    public float walkSpeed;
    public float sprintSpeed;
    public float lookSpeed;
    private Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0f;
    private float velocityY = 0f;
    private float gravity = 25f;
    private bool canMove = true;
    private bool canJump = true;
    private CharacterController enemyController;

    void Start()
    {
        enemyController = GetComponent<CharacterController>();
    }
    void Update()
    {
        // Get directional vectors
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (enemyController.isGrounded)
        {
            velocityY -= gravity * Time.deltaTime;
        }

        if (enemyController.isGrounded)
        {
            canJump = true;
        }
        movementDirection.y = velocityY;
        enemyController.Move(new Vector3(1f, 0f, 1f) * Time.deltaTime);

        // Enemy lookaround logic

    }
}