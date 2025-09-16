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
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float lookSpeed = 2f;


    private CharacterController characterController;
    public Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;

    // new system
    InputAction moveAction;
    InputAction jumpAction;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // movement logic
        if (canMove)
        {
            // Get WASD
            Vector2 moveValue = moveAction.ReadValue<Vector2>() / 100f;
            // Because W/S are the Y value of the Vector2, we apply it to the Z value of the Vector3 to simulate movement
            movementDirection = new Vector3(moveValue.x, 0f, moveValue.y);
            characterController.Move(movementDirection);

            if (jumpAction.IsPressed())
            {
                // jump logic
            }
            // apply gravity

            // lookaround logic
        }

    }

    void OnDestroy()
    {
        
    }

}