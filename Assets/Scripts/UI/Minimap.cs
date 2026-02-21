using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Minimap : MonoBehaviour
{
    public Transform playerTransform;
    private void LateUpdate()
    {
       Vector3 newPosition = playerTransform.position;
       newPosition.y = transform.position.y;
       transform.position = newPosition;
    }
}