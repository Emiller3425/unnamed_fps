using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class EnemyViewBobbing : MonoBehaviour
{
    private Vector3 pivotPoint;
    void Start()
    {
        pivotPoint = transform.parent.position;
    }

    void Update()
    {
        pivotPoint = transform.parent.position;
        transform.position = pivotPoint;
    }
}