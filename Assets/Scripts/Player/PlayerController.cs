using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// TODO: Handle character controller and capsule collider hitboxes when crouched

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 12f;
    public float adsWalkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 3f;
    public float lookSpeed = 0.6f;
    public float adsLookSpeed = 0.3f;
    public float jumpHeight = 4f;
    public Vector3 movementDirection = Vector3.zero;
    public bool isSprinting = false;
    public bool isCrouched = false;
    private bool isDashing = false;
    private float currentDashCooldown = 0f;
    private float maxDashCooldown = 5f;
    public Animator animator;
    private CharacterController characterController;
    public bool canJump = true;
    public bool canSprint = false;
    private float rotationX = 0f;
    private bool canMove = true;
    private bool isPaused = false;
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
        crouchAction.started += OnCrouchEnabled;
        crouchAction.canceled += OnCrouchDisabled;
    }

    private void OnEnable()
    {
        GameEvents.current.OnTogglePause += HandlePause;
        GameEvents.current.OnTogglePlayerInventory += HandlePlayerInventory;
    }

    private void Start()
    {
        // Game Events
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // movement logic
        if (canMove && !isPaused)
        {
            // Get WASD
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            
            canSprint = false;
            // canSprint = moveValue.y > 0.1f;

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
            transform.Rotate((adsEnabled ? adsLookSpeed : lookSpeed) * lookValue.x * Vector3.up);
            
            // Crosshairs bloom logic

            // In air
            if (!characterController.isGrounded)
            {
                GameEvents.current.Bloom(8f, false);
            } 
            // Walking
            else if (Mathf.Abs(speedX) > crouchSpeed + 0.1f || Mathf.Abs(speedY) > crouchSpeed + 0.1f)
            {
                GameEvents.current.Bloom(6f, false);
            } 
            // Crouch walking
            else if (Mathf.Abs(speedX) > 0.1f || Mathf.Abs(speedY) > 0.1f)
            {
                GameEvents.current.Bloom(2.5f, false);
            }
            // Not moving
            else
            {
                GameEvents.current.Bloom(1f, false);
            }

            if (currentDashCooldown > 0f)
            {
                currentDashCooldown -= Time.deltaTime;
            }
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
        if (isPaused)
        {
            return;
        }
        GameObject interactObject = GetInteractionObject();
        if (interactObject != null)
        {
            interactObject.GetComponentInParent<IInteractable>().HandleInteract();
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            return;
        }
        if (canJump)
        {
            velocityY = jumpHeight;
            canJump = false;
        }
    }
    
    private void OnAim(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            return;
        }
        adsEnabled = !adsEnabled;
    }

    private void OnEmote(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            return;
        }
        animator.SetTrigger("Emote");
    }

    private void OnCrouchEnabled(InputAction.CallbackContext context)
    {
        GameEvents.current.Crouch(true, characterController.isGrounded);
    }

    private void OnCrouchDisabled(InputAction.CallbackContext context)
    {
        GameEvents.current.Crouch(false, characterController.isGrounded);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (currentDashCooldown <= 0f)
        {
            // Dash Forward
            if (context.control == Keyboard.current.wKey)
            {
                StartCoroutine(
                    DashRoutine(transform.forward)
                    );
            }
            // Dash Backwards
            else if (context.control == Keyboard.current.sKey)
            {
                StartCoroutine(
                    DashRoutine(-transform.forward)
                    );
            }
            // Dash Right
            else if (context.control == Keyboard.current.dKey)
            {
                StartCoroutine(
                    DashRoutine(transform.right)
                    );
            }
            // Dash Left
            else if (context.control == Keyboard.current.aKey)
            {
                StartCoroutine(
                    DashRoutine(-transform.right)
                    );
            }
        }
    }

    private IEnumerator DashRoutine(Vector3 dashDirection)
    {
        float startTime = Time.time;
        float dashDuration = 0.25f;
        float dashSpeed = 50f;

        isDashing = true;
        Debug.Log(Time.time);
        Debug.Log(startTime + dashDuration);
        while (Time.time < startTime + dashDuration) {
            float normalizedTime = (Time.time - startTime) / dashDuration;
            float currentSpeed = Mathf.Lerp(dashSpeed, walkSpeed, normalizedTime);
            Debug.Log(currentSpeed * Time.deltaTime * dashDirection);
            characterController.Move(currentSpeed * Time.deltaTime * dashDirection);
            yield return null;
        }
        currentDashCooldown = maxDashCooldown;
        isDashing = false;
    }

    private void HandlePause(bool isToggled)
    {
       isPaused = isToggled;
    }

    private void HandlePlayerInventory(bool isToggled)
    {
        isPaused = isToggled;
    }

    private void OnDisable()
    {
        GameEvents.current.OnTogglePause -= HandlePause;
        GameEvents.current.OnTogglePlayerInventory -= HandlePlayerInventory;
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