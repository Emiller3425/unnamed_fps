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
    public float walkSpeed = 7f;
    public float sprintSpeed = 9f;
    public float lookSpeed = 0.6f;
    public float jumpHeight = 7f;
    private CharacterController characterController;
    private Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;
    private bool canJump = true;
    private float velocityY = 0;
    private float gravity = 25f;

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

            Vector3 right = transform.TransformDirection(Vector3.right);
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

            characterController.Move(movementDirection * Time.deltaTime);

            // Lookaround logic
            Vector2 lookValue = lookAction.ReadValue<Vector2>();

            rotationX -= lookValue.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -80f, 80f);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            // camera always will be at top of character controller
            playerCamera.transform.localPosition = (Vector3.up * characterController.height / 2f);

            transform.Rotate(lookSpeed * lookValue.x * Vector3.up);
        }

    }

}