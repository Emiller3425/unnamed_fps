using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class TrackParentTransformPosition : MonoBehaviour
{
    void Start()
    {
        transform.position = transform.parent.position;
    }
    void Update()
    {
        transform.position = transform.parent.position;
    }
}