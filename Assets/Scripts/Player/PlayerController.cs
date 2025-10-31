using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
// TODO: use velocity of x and z for dashing
// TODO: Handle character controller and capsule collider hitboxes when crouched

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 7f;
    public float adsWalkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 3f;
    public float lookSpeed = 0.6f;
    public float adsLookSpeed = 0.3f;
    public float jumpHeight = 5f;
    private CharacterController characterController;
    private Vector3 movementDirection = Vector3.zero;
    private float rotationX = 0f;
    private bool canMove = true;
    private bool canJump = true;
    private bool adsEnabled = false;
    private bool isCrouched = false;
    private float velocityY = 0f;
    private float velocityX = 0f;
    private float velocityZ = 0f;
    private float gravity = 10f;
    private float walkingStepNoiseBuffer = 0.5f;
    private float sprintingStepNoiseBuffer = 0.4f;
    // new input system
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction aimAction;

    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Dash");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        aimAction = InputSystem.actions.FindAction("Aim");
    }

    void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
        jumpAction.Enable();
        aimAction.Enable();
        // subscribe to function for immediately response on jump
        jumpAction.started += OnJump;
        dashAction.performed += OnDash;
        aimAction.started += OnAim;
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
            float speedX = moveValue.x * (crouchAction.IsPressed() ? crouchSpeed : adsEnabled ? adsWalkSpeed : (sprintAction.IsPressed() ? sprintSpeed : walkSpeed));
            float speedY = moveValue.y * (crouchAction.IsPressed() ? crouchSpeed : adsEnabled ? adsWalkSpeed : (sprintAction.IsPressed() ? sprintSpeed : walkSpeed));

            movementDirection = (right * speedX) + (forward * speedY);

            if (moveValue != Vector2.zero)
            {
                if (walkingStepNoiseBuffer <= 0f && !sprintAction.IsPressed())
                {
                    FindAnyObjectByType<AudioManager>().Play("footstep");
                    walkingStepNoiseBuffer = 0.5f;
                    sprintingStepNoiseBuffer = 0.4f;
                }
                else if (sprintingStepNoiseBuffer <= 0f && sprintAction.IsPressed())
                {
                    FindAnyObjectByType<AudioManager>().Play("footstep");
                    sprintingStepNoiseBuffer = 0.4f;
                    walkingStepNoiseBuffer = 0.5f;
                }
            }

            walkingStepNoiseBuffer -= Time.deltaTime;
            sprintingStepNoiseBuffer -= Time.deltaTime;

            // apply gravity
            if (!characterController.isGrounded)
            {
                velocityY -= gravity * Time.deltaTime;
            }
            // reset velocity and jump boolean if grounded
            if (characterController.isGrounded)
            {
                canJump = true;
                // Only overwrite velocityY if velocityY is < 0, this ensures that when we set it in the Input Callback it isn't overwritten.
                if (velocityY < 0f)
                {
                    velocityY = -2f;
                }
            }

            movementDirection.y = velocityY;

            characterController.Move(movementDirection * Time.deltaTime);

            // Lookaround logic
            Vector2 lookValue = lookAction.ReadValue<Vector2>();

            rotationX -= lookValue.y * (adsEnabled ? adsLookSpeed : lookSpeed);
            rotationX = Mathf.Clamp(rotationX, -80f, 80f);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            
            // handle camera height for crouching
            if (crouchAction.IsPressed())
            {
                isCrouched = true;
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, (Vector3.up * characterController.height / 2f - new Vector3(0f, 1f, 0f)), 0.1f); 
            }
            else
            {
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, Vector3.up * characterController.height / 2f - new Vector3(0f, 0.5f, 0f), 0.1f);
            }

            transform.Rotate((adsEnabled ? adsLookSpeed : lookSpeed) * lookValue.x * Vector3.up);
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            velocityY = jumpHeight;
            canJump = false;
        }
    }
    
    void OnAim(InputAction.CallbackContext context)
    {
        adsEnabled = !adsEnabled;
    }

    void OnDash(InputAction.CallbackContext context)
    {
        // TODO: Add dash logic -- initial burst then slows down
        if (context.control == Keyboard.current.wKey)
        {
            Debug.Log("Dash Forwards");
        }
        else if (context.control == Keyboard.current.sKey)
        {
            Debug.Log("Dash Backwards");
        }
        else if (context.control == Keyboard.current.aKey)
        {
            Debug.Log("Dash Left");
        }
        else if (context.control == Keyboard.current.dKey)
        {
            Debug.Log("Dash Right");
        }
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();

        jumpAction.started -= OnJump;
        jumpAction.Disable();

        dashAction.performed -= OnDash;
        dashAction.Disable();
    }

}