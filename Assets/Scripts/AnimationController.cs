using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class AnimationController : MonoBehaviour
{
    protected Animator animator;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Could not find animator");
        }
    }
    protected virtual void Update()
    {
       HandleAnimations();
    }
    
    protected virtual void HandleAnimations()
    {
        // do nothing in base class
    }
}