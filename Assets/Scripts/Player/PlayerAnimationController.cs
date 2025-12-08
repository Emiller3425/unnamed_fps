using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimationController : AnimationController
{
    private PlayerController playerController;
    protected override void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void HandleAnimations()
    {
        if (playerController.movementDirection.x == 0f && playerController.movementDirection.z == 0f)
        {
            Debug.Log("Idle");
            animator.SetFloat("Speed", 0f, 0.2f, Time.deltaTime);
        } else
        {
            Debug.Log("Walk");
            animator.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
        }
        // TODO: Add sprinting, crouching, jumping, shooting
    }
}