using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Pistol : Gun
{
    // Shoots bullet
    public override void ShootBullet()
    {
        base.ShootBullet();

        Debug.Log("pistol shot");
    }

}