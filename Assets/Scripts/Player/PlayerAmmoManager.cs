using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerAmmoManager : MonoBehaviour
{
    public int pistolAmmo;
    public int smgAmmo;
    public int rifleAmmo;
    void Start()
    {
        pistolAmmo = 72;
        smgAmmo = 120;
        rifleAmmo = 150;
    }

    public void decreasePistolAmmo()
    {
        pistolAmmo--;
    }

    public void decreaseSMGAmmo()
    {
        smgAmmo--;
    }
    public void decreaseRifleAmmo()
    {
        rifleAmmo--;
    }
}