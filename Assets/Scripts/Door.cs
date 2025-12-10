using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Door : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;
    public float rotationSpeed = 5f;
    public Transform hingeTransform;
    private float targetRotation = 0f;
    private bool isDoorClosed = true;

    private void Update()
    {
        Quaternion target = Quaternion.Euler(0f, targetRotation, 0f);

        hingeTransform.localRotation = Quaternion.Lerp(
            hingeTransform.localRotation,
            target,
            Time.deltaTime * rotationSpeed
        );

    }

    public void HandleInteract()
    {
        if (isDoorClosed)
        {
            openDoor();
        }
        else
        {
            closeDoor();
        }
        isDoorClosed = !isDoorClosed;
    }

    public void openDoor()
    {
        targetRotation = openAngle;
    }

    public void closeDoor()
    {
        targetRotation = 0f;
    }
}