using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Health : MonoBehaviour
{
    private float maxHealth = 100f;
    private float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    // this update function should update the ui for the currentHealth : maxHealth ratio for the given entity it is attached to
    void Update()
    {
        
    }
}