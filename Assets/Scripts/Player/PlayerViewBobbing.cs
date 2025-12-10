using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class PlayerViewBobbing : MonoBehaviour
{
    public Crosshairs crosshairs;
    public ViewBobbingPivotPosition viewBobbingPivotPosition;
    public CharacterController playerController;
    private Vector3 pivotPoint;
    private Vector3 localOffset;
    private float viewBobbingSpeed;
    private float adsViewBobbingSpeed;
    private float sprintViewBobbingSpeed;
    private float jumpViewBobbingSpeed;
    private float effectHeight;
    private float adsEffectHeight;
    private float sprintEffectHeight;
    private float jumpEffectHeight;
    private float effectWidth;
    private float adsEffectWidth;
    private float sprintEffectWidth;
    private float jumpEffectWidth;
    private float sinTime;
    private bool adsEnabled = false;
    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        aimAction = InputSystem.actions.FindAction("Aim");
    }
    void OnEnable()
    {
        moveAction.Enable();
        aimAction.Enable();
        sprintAction.Enable();
        // subscriptions
        aimAction.started += OnAim;
    }

    void Start()
    {
        pivotPoint = transform.parent.position;

        viewBobbingSpeed = 6f;
        effectHeight = 0.15f;
        effectWidth = 0.2f;

        adsViewBobbingSpeed = 4f;
        adsEffectHeight = 0f;
        adsEffectWidth = 0.01f;

        sprintViewBobbingSpeed = 8f;
        sprintEffectHeight = 0.2f;
        sprintEffectWidth = 0.25f;

        jumpViewBobbingSpeed = 0.8f;
        jumpEffectHeight = 0.3f;
        jumpEffectWidth = 0;

    }

    void Update()
    {
        Vector2 movementDirection = moveAction.ReadValue<Vector2>();
        pivotPoint = transform.parent.position;
        if (adsEnabled)
        {
            // pivotPoint = Camera.main.transform.position;
            localOffset = CalculateViewBobbingOffset(adsEffectHeight, adsEffectWidth, adsViewBobbingSpeed);
        }
        else if (!playerController.isGrounded)
        {
            localOffset = CalculateViewBobbingOffset(jumpEffectHeight, jumpEffectWidth, jumpViewBobbingSpeed);
        }
        else if (sprintAction.IsPressed())
        {
            // pivotPoint = transform.parent.position
            localOffset = CalculateViewBobbingOffset(sprintEffectHeight, sprintEffectWidth, sprintViewBobbingSpeed);
        }
        else
        {
            // pivotPoint = transform.parent.position;
            localOffset = CalculateViewBobbingOffset(effectHeight, effectWidth, viewBobbingSpeed);
        }
        if (movementDirection.magnitude > 0f)
        {
            // lerp so gun does not jerk initially
            transform.position = Vector3.Lerp(transform.position, pivotPoint + localOffset, 0.1f);
        }
        else
        {
            sinTime = 0f;
            transform.position = Vector3.Lerp(transform.position, pivotPoint, 0.1f);
        }
    }

    Vector3 CalculateViewBobbingOffset(float effectHeight, float effectWidth, float viewBobbingEffectSpeed)
    {
        float sinY = -Mathf.Abs(effectHeight * Mathf.Sin(sinTime));
        float sinX = effectWidth * Mathf.Cos(sinTime);
        sinTime += Time.deltaTime * viewBobbingEffectSpeed;
        return transform.right * sinX + transform.up * sinY;
    }

    void OnAim(InputAction.CallbackContext context)
    {
        adsEnabled = !adsEnabled;
        viewBobbingPivotPosition.adsEnabled = adsEnabled;
        if (adsEnabled)
        {
            GameEvents.current.SetCrossHairDeactivated();
        }
        else
        {
            crosshairs.SetCrossHairActivated();
        }

    }


    void OnDisable()
    {
        aimAction.started -= OnAim;
        aimAction.Disable();
    }
}