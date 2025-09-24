using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class ViewBobbingPivotPosition : MonoBehaviour
{
    CharacterController playerController;
    void Start()
    {
        playerController = GetComponentInParent<CharacterController>();
        transform.localPosition = transform.parent.localPosition + (Vector3.forward * playerController.radius) + (Vector3.right * playerController.radius / 1.5f) - (Vector3.up * playerController.height / 1.75f);
    }

    void Update()
    {
        transform.localPosition = transform.parent.localPosition + (Vector3.forward * playerController.radius) + (Vector3.right * playerController.radius / 1.5f) - (Vector3.up * playerController.height / 1.75f);
    }

}