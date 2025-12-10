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

    private void Awake()
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

    private void Start()
    {
        GameEvents.current.OnAmmoChanged += UpdateAmmoUI;
    }

    private void UpdateAmmoUI(int currentMag, int currentAmmo)
    {
       if (ammoUIText != null) 
        {
         ammoUIText.text = $"{currentMag} / {currentAmmo}";
        } else
        {
            Debug.LogError("Ammo UI Text is null, check component hierachy");
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.OnAmmoChanged -= UpdateAmmoUI;
    }

}