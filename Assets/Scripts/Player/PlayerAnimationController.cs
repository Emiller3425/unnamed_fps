using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimationController : AnimationController
{
    private PlayerController playerController;

    protected override void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name);
        }
    }
    protected override void Start()
    {
        // Subscribe to events
        GameEvents.current.OnWeaponFired += PlayShootAnimation;
        GameEvents.current.OnWeaponReloaded += PlayReloadAnimation;
    }

    protected override void PlayShootAnimation()
    {
        animator.SetTrigger("Shoot");
    }
    protected override void PlayReloadAnimation()
    {
        animator.SetTrigger("Reload");
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void HandleAnimations()
    {
        // Idle
        if(playerController.canJump) {
            if (playerController.movementDirection.x == 0f && playerController.movementDirection.z == 0f)
            {
                animator.SetFloat("Speed", 0f, 0.2f, Time.deltaTime);
            } else
            {
                // Crouch Walk
                if (playerController.isCrouched)
                {
                    animator.SetFloat("Speed", 0.25f, 0.2f, Time.deltaTime);
                // Sprint
                } else if (playerController.isSprinting && playerController.canSprint) {
                    animator.SetFloat("Speed", 0.75f, 0.2f, Time.deltaTime);
                // Walk
                } else
                {
                    animator.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
                }
            }
        } else
        {
            animator.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
        }
        
        // TODO: Add jumping, aiming
    }

    public void TriggerWeaponSwap(Action onSwapMidpoint)
    {
        StartCoroutine(SwapRoutine(onSwapMidpoint));
    }

    private IEnumerator SwapRoutine(Action onSwapMidpoint)
    {
        animator.SetLayerWeight(0, 0f);
        animator.SetFloat("SwapSpeed", 1f);
        animator.Play("Swap", 3, 0f);

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(3).length);

        onSwapMidpoint?.Invoke();

        animator.SetFloat("SwapSpeed", -1f);
        animator.Play("Swap", 3, 1f);

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(3).length);


        animator.Play("Empty State", 3, 0f);
        animator.SetLayerWeight(0, 1f);
    }

    protected override void OnDestroy()
    {
        GameEvents.current.OnWeaponFired -= PlayShootAnimation;
        GameEvents.current.OnWeaponReloaded -= PlayReloadAnimation;
    }
}