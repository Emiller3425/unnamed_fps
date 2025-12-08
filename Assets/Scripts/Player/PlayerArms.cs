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

public class PlayerArms : MonoBehaviour
{ 
    public Camera playerCamera;
    private float smoothSpeed = 10f;
    void Update()
    {
        // transform.position = Vector3.zero;
        Quaternion targetRotation = playerCamera.transform.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

}