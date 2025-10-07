using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class FullAutoGun : Gun
{
    protected override void Update()
    {
        base.Update();
        if (shootAction.IsPressed())
        {
            AttemptShoot();
        }
    }
    protected override void OnShoot(InputAction.CallbackContext context)
    {
        // do nothing because we are handling shoot logic in Update();
    }
} 