using System;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class AmmoUIManager : MonoBehaviour
{
    public static AmmoUIManager Instance { get; private set; }
    [HideInInspector] public TextMeshProUGUI ammoUIText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        ammoUIText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateAmmoUI(int magAmmo, int reserveAmmo)
    {
        ammoUIText.text = $"{magAmmo} / {reserveAmmo}";
    }
}