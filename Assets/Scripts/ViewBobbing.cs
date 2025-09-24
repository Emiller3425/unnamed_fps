using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class ViewBobbing : MonoBehaviour
{
    private Vector3 pivotPoint;
    private float viewBobbingSpeed;
    private float effectHeight;
    private float effectWidth;
    private float sinTime;

    private InputAction moveAction;
    void Start()
    {
        pivotPoint = transform.parent.position; // view bobbing parent pivot position
        viewBobbingSpeed = 6f;
        effectHeight = 0.15f;
        effectWidth = 0.2f;
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 movementDirection = moveAction.ReadValue<Vector2>();
        if (movementDirection.magnitude > 0f)
        {
            pivotPoint = transform.parent.position; // view bobbing parent pivot position
            sinTime += Time.deltaTime * viewBobbingSpeed;
            float sinY = -Mathf.Abs(effectHeight * Mathf.Sin(sinTime));
            float sinX = effectWidth * Mathf.Cos(sinTime);

            Vector3 localOffset = transform.right * sinX + transform.up * sinY;

            // lerp so gun does not jerk initially
            transform.position = Vector3.Lerp(transform.position, pivotPoint + localOffset, 0.1f);
        }
        else
        {
            sinTime = 0f;
            transform.position = Vector3.Lerp(transform.position, transform.parent.position, 0.1f);
        }
    }
}