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
    void Start()
    {
        // sets view bobbing pivot for fps camera this might need to be adjusted basewd on the weaponm tpe in the future
        transform.localPosition = new Vector3(0.5f, -0.5f, 1.2f);
    }

}