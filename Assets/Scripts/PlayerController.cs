using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float lookSpeed = 0.6f;
    public float jumpHeight = 7f;
    public bool isWalking = false;
    private CharacterController characterController;
    private Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;
    private bool canJump = true;
    private float velocityY = 0;
    private float gravity = 20f;
    // new input system
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // movement logic
        if (canMove)
        {
            // Get WASD
            Vector2 moveValue = moveAction.ReadValue<Vector2>();

            // get right direction vector
            Vector3 right = transform.TransformDirection(Vector3.right);
            // Get forward direction vector
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            // Get movement speeds
            float speedX = moveValue.x * (sprintAction.IsPressed() ? sprintSpeed : walkSpeed);
            float speedY = moveValue.y * (sprintAction.IsPressed() ? sprintSpeed : walkSpeed);

            movementDirection = (right * speedX) + (forward * speedY);
            
            // apply gravity
            if (!characterController.isGrounded)
            {
                velocityY -= gravity * Time.deltaTime;
            }
            // reset velocity and jump boolean if grounded
            if (characterController.isGrounded)
            {
                velocityY = 0f;
                canJump = true;
            }
            // handle jump
            if (jumpAction.IsPressed() && canJump)
            {
                velocityY = jumpHeight;
                canJump = false;
            }
            
            movementDirection.y = velocityY;

            // apply movement
            characterController.Move(movementDirection * Time.deltaTime);
            // Is charcter walking for view bobbing
            if (movementDirection.x != 0f || movementDirection.z != 0f)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
            Debug.Log(isWalking);

            // Lookaround logic
            Vector2 lookValue = lookAction.ReadValue<Vector2>();
            // Apply rotation to the camera around the X-axis for looking around vertically
            rotationX -= lookValue.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -80f, 80f);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            // Rotate the entire player object around the Y-Axis for looking around horizontally
            transform.Rotate(lookSpeed * lookValue.x * Vector3.up);
        }

    }

    void OnDestroy()
    {
        
    }

}