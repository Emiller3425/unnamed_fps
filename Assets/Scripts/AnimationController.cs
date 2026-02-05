using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Could not find animator");
        }
    }
    protected virtual void PlayShootAnimation()
    {
        
    }
    protected virtual void PlayReloadAnimation()
    {
        
    }
    protected virtual void Update()
    {
       HandleAnimations();
    }
    
    protected virtual void HandleAnimations()
    {
        // do nothing in base class
    }

    protected virtual void OnDestroy()
    {
        
    }
}