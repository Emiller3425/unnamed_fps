using System;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class AmmoCounter : MonoBehaviour
{
    public GameObject equippedWeapon;
    private TextMeshProUGUI ammo;
    private int currentMagAmmo;
    private int currentReserveAmmo;

    void Awake()
    {
        ammo = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        currentMagAmmo = equippedWeapon.GetComponent<Pistol>().currentMag;
        currentReserveAmmo = equippedWeapon.GetComponent<Pistol>().currentAmmo;
        ammo.text = currentMagAmmo + " / " + currentReserveAmmo;
    }

    void Update()
    {
        
    }
}