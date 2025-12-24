using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    public float jumpHeight = 4f;
    public Vector3 movementDirection = Vector3.zero;
    public bool isSprinting = false;
    public bool isCrouched = false;
    public Animator animator;
    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;
    public bool canJump = true;
    public bool canSprint = false;
    private float rotationX = 0f;
    private bool canMove = true;
    private bool adsEnabled = false;
    private float velocityY = 0f;
    private float velocityX = 0f;
    private float velocityZ = 0f;
    private float gravity = 10f;
    private float walkingStepNoiseBuffer = 0.5f;
    private float sprintingStepNoiseBuffer = 0.4f;
    private float capsuleColliderStandHeight = 2f;
    private float capsuleColliderCrouchHeight = 1f;
    private Dictionary<string, int> movementValues;
    // new input system
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction aimAction;
    private InputAction interactAction;
    private InputAction emoteAction;

    private void Awake()
    {
        movementValues = new Dictionary<string, int>
        {
            {"w", 0},
            {"a", 0},
            {"s", 0},
            {"d", 0},
        };

        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Dash");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        aimAction = InputSystem.actions.FindAction("Aim");
        interactAction = InputSystem.actions.FindAction("Interact");
        emoteAction = InputSystem.actions.FindAction("Emote");

        moveAction.Enable();
        dashAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
        jumpAction.Enable();
        aimAction.Enable();
        interactAction.Enable();
        emoteAction.Enable();
        // subscribe to function for immediately response on jump
        jumpAction.started += OnJump;
        dashAction.performed += OnDash;
        aimAction.started += OnAim;
        interactAction.started += OnInteract;
        emoteAction.started += OnEmote;
    }

    private void Start()
    {
        // Game Events
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // movement logic
        if (canMove)
        {
            // Get WASD
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            canSprint = moveValue.y > 0.1f;

            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            // Get movement speeds
            float speedX = moveValue.x * (crouchAction.IsPressed() && characterController.isGrounded ? crouchSpeed : adsEnabled ? adsWalkSpeed : (sprintAction.IsPressed() && characterController.isGrounded && canSprint ? sprintSpeed : walkSpeed));
            float speedY = moveValue.y * (crouchAction.IsPressed() && characterController.isGrounded ? crouchSpeed : adsEnabled ? adsWalkSpeed : (sprintAction.IsPressed() && characterController.isGrounded && canSprint ? sprintSpeed : walkSpeed));

            isCrouched = crouchAction.IsPressed();
            isSprinting = sprintAction.IsPressed();

            movementDirection = (right * speedX) + (forward * speedY);

            if (moveValue != Vector2.zero)  
            {
                // crouch
                if (walkingStepNoiseBuffer <= 0f && (crouchAction.IsPressed() || !sprintAction.IsPressed() || !canSprint) && characterController.isGrounded)
                {
                    GameEvents.current.PlaySFX("footstep");
                    walkingStepNoiseBuffer = 0.5f;
                    sprintingStepNoiseBuffer = 0.4f;
                }
                // sprint
                else if (sprintingStepNoiseBuffer <= 0f && sprintAction.IsPressed() && !crouchAction.IsPressed() && characterController.isGrounded && canSprint)
                {
                    GameEvents.current.PlaySFX("footstep");
                    walkingStepNoiseBuffer = 0.5f;
                    sprintingStepNoiseBuffer = 0.4f;
                }
            }

            walkingStepNoiseBuffer -= Time.deltaTime;
            sprintingStepNoiseBuffer -= Time.deltaTime;

            // apply gravity
            if (!characterController.isGrounded)
            {
                canJump = false;
                velocityY -= gravity * Time.deltaTime;
            }
            // reset velocity and jump boolean if grounded
            if (characterController.isGrounded)
            {
                canJump = true;
                // Only overwrite velocityY if velocityY is < 0, this ensures that when we set it in the Input Callback it isn't overwritten.
                if (velocityY < 0f)
                {
                    velocityY = -10f;
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
            if (crouchAction.IsPressed() && characterController.isGrounded)
            {
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, (Vector3.up * characterController.height / 2f - new Vector3(0f, 1f, 0f)), 0.1f);
                capsuleCollider.height = capsuleColliderCrouchHeight;
            }
            else
            {
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, Vector3.up * characterController.height / 2f - new Vector3(0f, 0.5f, 0f), 0.1f);
                capsuleCollider.height = capsuleColliderStandHeight;
            }

            transform.Rotate((adsEnabled ? adsLookSpeed : lookSpeed) * lookValue.x * Vector3.up);
        }
    }
    private GameObject GetInteractionObject()
    {
        Ray cameraRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, 3f))
        {
            if (hit.collider.gameObject.GetComponentInParent<IInteractable>() != null)
            {
                Debug.Log("Interact PlayerController");
                return hit.collider.gameObject;
            }
        }
        return null;
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
        GameObject interactObject = GetInteractionObject();
        if (interactObject != null)
        {
            interactObject.GetComponentInParent<IInteractable>().HandleInteract();
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            velocityY = jumpHeight;
            canJump = false;
        }
    }
    
    private void OnAim(InputAction.CallbackContext context)
    {
        adsEnabled = !adsEnabled;
    }

    private void OnEmote(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Emote");
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        float dashForceX = 0;
        float dashForceZ = 0;
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

    private void OnDestroy()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();

        jumpAction.started -= OnJump;
        jumpAction.Disable();

        dashAction.performed -= OnDash;
        dashAction.Disable();

        aimAction.started -= OnAim;
        aimAction.Disable();

        interactAction.started -= OnInteract;
        interactAction.Disable();

        emoteAction.started -= OnEmote;
        emoteAction.Disable();
    }

}