using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 0.03f;
    public float sprintSpeed = 0.05f;
    public float lookSpeed = 0.25f;


    private CharacterController characterController;
    private Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;

    // new system
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
            // Because W/S are the Y value of the Vector2, we apply it to the Z value of the Vector3 to simulate movement
            movementDirection = new Vector3(moveValue.x, 0f, moveValue.y) * (sprintAction.IsPressed() ? sprintSpeed : walkSpeed);
            characterController.Move(movementDirection);

            if (jumpAction.IsPressed())
            {
                // jump logic
            }

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