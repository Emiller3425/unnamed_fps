using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Entity : MonoBehaviour
{
    public int maxHealth = 100;
    protected int currenthealth;

    void Start()
    {
        currenthealth = maxHealth;
    }

    void Update()
    {

    }

    void OnDisable()
    {
        
    }

    void OnDestroy()
    {
        
    }

}