using System;
using System.Runtime.InteropServices;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class SemiAutoGun : Gun
{
    protected override void OnShoot(InputAction.CallbackContext context)
    {
        AttemptShoot();
    }

}